using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public Button saveButton;
    public ScriptPlotManagerScript plotManager;
    public string saveFilePath;

    void Start()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveGame);
        }

        if (plotManager == null)
        {
            Debug.LogError("PlotManager not assigned!");
            return;
        }

        LoadGame(); // Load data when the game starts
    }

    void SaveGame()
    {
        if (plotManager == null)
        {
            Debug.LogError("PlotManager not found!");
            return;
        }

        SaveData saveData = new SaveData
        {
            currentSceneId = plotManager.GetCurrentSceneId(),
            currentDialogueIndex = plotManager.GetCurrentDialogueIndex() - 1,
            isChoosingOption = plotManager.IsChoosingOption(),
            currentChoiceIndex = plotManager.GetCurrentChoiceIndex(),
            selectedChoices = plotManager.GetSelectedChoices(),
            saveDate = System.DateTime.Now.ToString("dd-MM-yy HH:mm:ss")
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Game saved:\n" + json);
    }

    void LoadGame()
    {
        if (File.ReadAllText(saveFilePath) == "")
        {
            Debug.Log("Save file empty.");
            StartCoroutine(plotManager.LoadSceneWithFade("start"));
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SaveData loadedData = JsonUtility.FromJson<SaveData>(json);

        if (loadedData == null)
        {
            Debug.LogError("Failed to load save data.");
            return;
        }

        Debug.Log("Game loaded:\n" + json);

        // Apply loaded data to PlotManager
        plotManager.SetCurrentDialogueIndex(loadedData.currentDialogueIndex);
        plotManager.SetChoiceSelection(loadedData.currentChoiceIndex, loadedData.isChoosingOption);
        StartCoroutine(plotManager.LoadSceneWithFade(loadedData.currentSceneId));
    }
}

[System.Serializable]
public class SaveData
{
    public string currentSceneId;
    public int currentDialogueIndex;
    public bool isChoosingOption;
    public int currentChoiceIndex;
    public List<int> selectedChoices;
    public string saveDate;
}
