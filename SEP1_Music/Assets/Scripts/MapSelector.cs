using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSelector : MonoBehaviour
{
    [Header("UI References")]
    public Image rightBigCoverThumbnail;
    public GameObject MapItemObject;
    public TMP_Text LeaderboardTextField;
    public Button mainMenuButton;
    public Button mapEditorButton;
    public Button newMapButton;
    public Button selectButton;
    public Sprite defaultSprite;

    [Header("Audio")]
    public float audioFadeDuration = 1.0f;
    public AudioSource musicPreviewSource;

    [Header("Input Actions")]
    public InputActionReference upAction;
    public InputActionReference downAction;
    public InputActionReference selectAction;

    [Header("Game Objects")]
    public GameObject[] selectObjects;

    [Header("Map Selection")]
    public int keepActive = 5;
    public float degreeOffset = 25.0f;

    // property to handle selection changes
    [SerializeField] private int _selectedMapIndex = 0;
    private int selectedMapIndex
    {
        get => _selectedMapIndex;
        set
        {
            _selectedMapIndex = value;
            OnSelectionChanged();
        }
    }

    [Header("Rotation Settings")]
    public float rotationDuration = 0.5f;
    public float edgeBounceAmount = 7.0f;
    public float edgeBounceDuration = 0.2f;

    public int speedUpThreshold = 10;
    public int speedUpMultiplier = 5;

    [SerializeField] private bool isRotating = false;
    [SerializeField] private int stackedRotates = 0;

    [Header("Scene Transition")]
    public float sceneTransitionDuration = 1.0f;
    public float sceneTransitionDelay = 0.5f;
    public float moveCapsuleDistance = 10.0f;

    [SerializeField] private bool isTransitioning = false;

    [Header("Target Scenes")]
    public string mapPlaySceneName = "BeatmapPlayScene";
    public string mainMenuSceneName = "MainMenu";

    [Header("Leaderboard Data")]
    public int leaderboardEntryCount = 10;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip currentlyPlayingAudio;
    [SerializeField] private Coroutine audioFadeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserMaps.RefreshMaps();
        selectObjects = new GameObject[UserMaps.Maps.Count];

        if (selectButton != null) selectButton.onClick.AddListener(SelectMap);

        int indexer = 0;
        foreach (Map map in UserMaps.Maps)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, -degreeOffset * indexer); // Rotate based of offset
            GameObject game_object = Instantiate(MapItemObject, this.transform.position, rotation); // Create object

            // Set subelements of map item
            Transform Capsule = game_object.transform.GetChild(0);
            SpriteRenderer spriteRenderer = Capsule.GetChild(0).GetComponent<SpriteRenderer>();

            if (File.Exists(map.CoverFilePath))
            {
                Texture2D cover_texture = LoadTexture(map.CoverFilePath);
                Rect rect = new Rect(0, 0, cover_texture.width, cover_texture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f);

                Sprite cover_sprite = Sprite.Create(
                    cover_texture,
                    rect,
                    pivot,
                    100f 
                );

                spriteRenderer.sprite = cover_sprite;
            }
            else
            {
                Debug.LogWarning($"No cover image found for map in folder: {map.MapFolder}");
                // modify this to default if no image found
                spriteRenderer.sprite = defaultSprite;
            }

            game_object.transform.SetParent(this.transform, true); // Set Controller to parent of object

            selectObjects[indexer] = game_object; 

            indexer++;
        }
        ModifyActiveGameObjects();
        OnSelectionChanged();

        mainMenuButton.onClick.AddListener(() => {
            StartCoroutine(AnimateSceneChange(mainMenuSceneName));
        });
        selectButton.onClick.AddListener(() => {
            SelectMap();
        });
    }

    public Texture2D LoadTexture(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false); // create a new texture (size will be replaced by LoadImage)
        tex.LoadImage(fileBytes);
        return tex;
    }

    // register input action callbacks
    void OnEnable()
    {
        if (upAction != null) upAction.action.performed += OnUpPerformed;
        if (downAction != null) downAction.action.performed += OnDownPerformed;
        if (selectAction != null) selectAction.action.performed += OnSelectPerformed;
    }

    void OnDisable()
    {
        if (upAction != null) upAction.action.performed -= OnUpPerformed;
        if (downAction != null) downAction.action.performed -= OnDownPerformed;
        if (selectAction != null) selectAction.action.performed -= OnSelectPerformed;
    }

    // Input Action Callbacks, callbackcontext parameters are required even if unused
    private void OnUpPerformed(InputAction.CallbackContext ctx) => NavigateUp();
    private void OnDownPerformed(InputAction.CallbackContext ctx) => NavigateDown();
    private void OnSelectPerformed(InputAction.CallbackContext ctx) => SelectMap();


    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateUp() {
        QueueRotate(-1);
    }

    public void NavigateDown() {
        QueueRotate(1);
    }

    private void QueueRotate(int direction)
    {
        // Coroutines are used to handle smooth rotation over time
        // coroutines are basically "background" functions that can pause execution and return to the main thread, then continue execution later

        int targetIndex = selectedMapIndex + direction;

        if (targetIndex < 0 || targetIndex >= selectObjects.Length)
        {
            if (!isRotating) StartCoroutine(EdgeBounce(direction));
            return;
        }

        stackedRotates += direction;
        if (!isRotating) StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        while (stackedRotates != 0)
        {
            isRotating = true;

            // ternary operator: condition ? value_if_true : value_if_false
            int step = stackedRotates > 0 ? 1 : -1;

            // check bounds again in case of rapid input
            if (selectedMapIndex + step < 0 || selectedMapIndex + step >= selectObjects.Length)
            {
                stackedRotates = 0;
                StartCoroutine(EdgeBounce(step)); // run edge bounce when going oob
                yield break;
            }


            stackedRotates -= step;

            selectedMapIndex += step;
            ModifyActiveGameObjects();

            float startRot = transform.eulerAngles.z;
            float endRot = startRot + degreeOffset * step;

            float t = 0f;
            while (t < rotationDuration)
            {
                t += Time.deltaTime * (Mathf.Abs(stackedRotates) > speedUpThreshold ? speedUpMultiplier : 1); // spin faster if too many stacked rotates

                // use lerp to interpolate rotation over time
                float lerp = Mathf.SmoothStep(0, 1, t / rotationDuration);
                float rot = Mathf.Lerp(startRot, endRot, lerp);

                transform.eulerAngles = new Vector3(0, 0, rot);
                yield return null;
            }

            transform.eulerAngles = new Vector3(0, 0, endRot);
        }

        isRotating = false;
    }

    private IEnumerator EdgeBounce(int direction)
    {
        isRotating = true;

        float startRot = transform.eulerAngles.z;
        float outRot = startRot + direction * edgeBounceAmount;

        // could be more line-efficient by combining both while loops into one and then swapping the lerp targets, but this is more readable

        float t = 0f;
        // bounce out
        while (t < edgeBounceDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / edgeBounceDuration);
            float rot = Mathf.Lerp(startRot, outRot, lerp);
            transform.eulerAngles = new Vector3(0, 0, rot);
            yield return null;
        }

        t = 0f;
        // bounce back
        while (t < edgeBounceDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / edgeBounceDuration);
            float rot = Mathf.Lerp(outRot, startRot, lerp);
            transform.eulerAngles = new Vector3(0, 0, rot);
            yield return null;
        }

        transform.eulerAngles = new Vector3(0, 0, startRot);
        isRotating = false;
    }


    public void SelectMap() { 
        CrossSceneManager.SelectedMap = UserMaps.Maps[selectedMapIndex];
        StartCoroutine(AnimateSceneChange(mapPlaySceneName));
    }

    IEnumerator AnimateSceneChange(string sceneName)
    // coroutine to animate scene transition
    {
        if (isTransitioning) yield break; // early return if already transitioning

        Debug.Log("Starting Scene Transition");
        isTransitioning = true;
        yield return new WaitForSeconds(sceneTransitionDelay);


        float t = 0f;


        // getchild is used on every object to make sure the capsule subobject is moved instead of the empty centered on the selector
        Vector3[] startPositions = new Vector3[selectObjects.Length];
        for (int i = 0; i < selectObjects.Length; i++)
        {
            startPositions[i] = selectObjects[i].transform.GetChild(0).localPosition;
        }

        while (t < sceneTransitionDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / sceneTransitionDuration);

            for (int i = 0; i < selectObjects.Length; i++)
            {
                GameObject obj = selectObjects[i];
            
                obj.transform.GetChild(0).localPosition = new Vector3(Mathf.Lerp(startPositions[i].x, 
                                                                      startPositions[i].x - moveCapsuleDistance, 
                                                                      lerp), 0, 0);
            }

            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }


    public void ModifyActiveGameObjects()
    {
        // sets active only a subset of map objects around the selected index to avoid overlapping multiple rotations
        for (int i = 0; i < selectObjects.Length; i++)
        {
            if (i >= selectedMapIndex - keepActive / 2 && i <= selectedMapIndex + keepActive / 2)
            {
                selectObjects[i].SetActive(true);
            }
            else
            {
                selectObjects[i].SetActive(false);
            }
        }
    }

    // called whenever selectedMapIndex is changed, updates images + leaderboard etc.
    void OnSelectionChanged()
    {
        string newLeaderboardText = "LEADERBOARD\n";
        Map selectedMap = UserMaps.Maps[selectedMapIndex];

        if (selectedMap.leaderboard != null)
        {
            int leaderboardDisplayCount = Mathf.Min(leaderboardEntryCount, selectedMap.leaderboard.leaderboard.Count);
            for (int i = 0; i < leaderboardDisplayCount; i++)
            {
                LeaderboardEntry entry = selectedMap.leaderboard.leaderboard[i];
                newLeaderboardText += $"{i + 1}. {entry.playerName} - {entry.score}\n";
            }
        }
        LeaderboardTextField.text = newLeaderboardText;
        // check if file exists and display default if not
        rightBigCoverThumbnail.sprite = File.Exists(selectedMap.CoverFilePath) ? Sprite.Create(
            LoadTexture(selectedMap.CoverFilePath),
            new Rect(0, 0, LoadTexture(selectedMap.CoverFilePath).width, LoadTexture(selectedMap.CoverFilePath).height),
            new Vector2(0.5f, 0.5f),
            100f 
        ) : defaultSprite;

    }
}
