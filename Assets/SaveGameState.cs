using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveGameState : MonoBehaviour
{
    public Button saveButtonSlot;
    public string saveFilePath = "Assets/Resources/save.json";
    public string saveSlotFilePath; // Path for the save slot file
    private string emptySceneData = "{\"currentSceneId\": \"\"}";
    void Start()
    {

    }

    void Update()
    {
    }

    // Method that gets called when the save button is pressed
    public void SaveToSlot()
    {
        // Add listener to save button click
        saveButtonSlot.onClick.AddListener(SaveToSlot);
        // Check if the main save file exists
        if (File.Exists(saveFilePath))
        {
            // If it exists, copy its content to save_slot{num}.json
            File.Copy(saveFilePath, saveSlotFilePath, true);
            File.WriteAllText(saveFilePath, emptySceneData); // Clear and set to empty scene

            Debug.Log("Game state saved to the according save_slot json file");
        }
        else
        {
            Debug.LogError("save.json not found!");
        }
    }
}
