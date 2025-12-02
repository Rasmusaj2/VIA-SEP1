using UnityEngine;
using UnityEngine.UIElements;

public class OptionsController : MonoBehaviour
{
    public OptionsData optionsData;

    public VisualElement mainMenuUI;
    public ScrollView optionsSettings;
    

    

    private void Start()
    {
        mainMenuUI = GetComponent<UIDocument>().rootVisualElement;

        optionsSettings = mainMenuUI.Q<ScrollView>("optionssettings");
        optionsSettings.dataSource = optionsData; 


    }
}

public class OptionsData
{
    public int audioLevel;
}
