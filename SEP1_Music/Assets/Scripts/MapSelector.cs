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

    public Button mainMenuButton;
    public Button mapEditorButton;
    public Button newMapButton;
    public Button selectButton;

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
    [SerializeField] private int selectedMapIndex = 0;
    [SerializeField] private string mapPlaySceneName = "BeatmapPlayScene";

    [Header("Rotation Settings")]
    public float rotationDuration = 0.5f;
    public float edgeBounceAmount = 7.0f;
    public float edgeBounceDuration = 0.2f;

    public int speedUpThreshold = 10;
    public int speedUpMultiplier = 5;

    [SerializeField] private bool isRotating = false;
    [SerializeField] private int stackedRotates = 0;

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
            game_object.transform.SetParent(this.transform, true); // Set Controller to parent of object

            selectObjects[indexer] = game_object; 

            indexer++;
        }
        ModifyActiveGameObjects();
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

        float t = 0f;
        while (t < edgeBounceDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.SmoothStep(0, 1, t / edgeBounceDuration);
            float rot = Mathf.Lerp(startRot, outRot, lerp);
            transform.eulerAngles = new Vector3(0, 0, rot);
            yield return null;
        }

        t = 0f;
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


    public void SelectMap() { }


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

    void OnSelectionChanged()
    {
        // handle audio preview
    }
}
