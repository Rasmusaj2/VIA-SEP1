using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionsController : MonoBehaviour
{
    
    public OptionsData optionsData;
    public string fileName = "settings.json";
    public string fullPath;

    public VisualElement mainMenuUI;
    public ScrollView optionsSettings;

    public Button optionsCancelButton;
    public Button optionsApplyButton;
    

    private void Start()
    {
        //Læser eksisterende settings fil ved start. Opretter en ny, hvis den ikke findes.
        fullPath = Path.Combine(JSONPersistence.Appdatapath, fileName);

        if (File.Exists(fullPath))
        {
            optionsData = JSONPersistence.LoadFromJSON<OptionsData>(fileName);
        }
        else
        {
            optionsData = new OptionsData();
            JSONPersistence.SaveToJSON(optionsData, fileName);
        }

        mainMenuUI = GetComponent<UIDocument>().rootVisualElement;

        optionsSettings = mainMenuUI.Q<ScrollView>("optionssettings");
        //Parent elementet for alle settings databindes med optionsData klassen.
        //Da databindingen er sat til Two-way, vil ændringer både i klassen og elementer i options påvirke hinanden.
        optionsSettings.dataSource = optionsData;

        optionsApplyButton = mainMenuUI.Q<Button>("optionsapplybutton");
        optionsApplyButton.clicked += OnOptionsApplyButtonClicked;

        optionsCancelButton = mainMenuUI.Q<Button>("optionscancelbutton");
        optionsCancelButton.clicked += OnOptionsCancelButtonClicked;
    }

    //Gemmer og bruger nye settings, hvis spiller trykker på apply knappen. Ellers loades de settings, som spilleren havde før eventuelle ændringer.
    public void OnOptionsApplyButtonClicked()
    {
        JSONPersistence.SaveToJSON(optionsData, fileName);
        JSONPersistence.LoadFromJSON<OptionsData>(fileName);
    }

    public void OnOptionsCancelButtonClicked()
    {
        optionsData = JSONPersistence.LoadFromJSON<OptionsData>(fileName);
    }
}

//Denne klasse indeholder settings som data.
[Serializable]
public class OptionsData
{
    public int audioLevel;
}
