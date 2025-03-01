using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SaveManager : MonoBehaviour
{
    // Reference to the save button
    public Button saveButton;

    // Reference to the SceneManagerScript that holds the current scene
    public ScriptPlotManagerScript plotManager;

    // Path to the save file
    public string saveFilePath = "Assets/Resources/save.json";

    // Start is called before the first frame update
    void Start()
    {
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveGame);
        }

        if (plotManager == null)
        {
            Debug.LogError("PlotManager not assigned!");
        }
    }

    // Method to save the current scene ID into the save file
    void SaveGame()
    {
        // Get the current scene ID from the plotManager
        string currentSceneId = plotManager.GetCurrentSceneId();

        // Create a SaveData object to store the scene ID
        SaveData saveData = new SaveData
        {
            currentSceneId = currentSceneId
        };

        // Convert the SaveData object to JSON
        string json = JsonUtility.ToJson(saveData);

        // Write the JSON to the file
        File.WriteAllText(saveFilePath, json);
        Debug.Log(json);
        Debug.Log("Game saved with scene: " + currentSceneId);
        Debug.Log("Save file path: " + saveFilePath);
    }
}

// Define the structure of the save data
[System.Serializable]
public class SaveData
{
    public string currentSceneId;
}
