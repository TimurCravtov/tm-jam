using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class SaveGameState : MonoBehaviour
{
    public Button saveButtonSlot;
    public TextMeshProUGUI saveButtonText;
    public string saveFilePath;
    public string saveSlotFilePath; // Path for the save slot file
    private int slotNum;
    void Start()
    {
        string filePath;
        if (saveSlotFilePath == "Assets/Resources/save.json") {
            filePath = saveFilePath;
            slotNum = (int)(saveFilePath[saveFilePath.Length - 6] - '0');

        }
        else
        {
            filePath = saveSlotFilePath;
            slotNum = (int)(saveSlotFilePath[saveSlotFilePath.Length - 6] - '0');
        }
        UpdateButtonLabel(filePath); // Update once when the game starts

    }

    void UpdateButtonLabel(string FilePath)
    {
        if (File.ReadAllText(FilePath) != "")
        {
            string json = File.ReadAllText(FilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            if (saveData != null && !string.IsNullOrEmpty(saveData.saveDate))
            {
                saveButtonText.text = "Last Save: " + saveData.currentSceneId + ",\n" + saveData.saveDate;
            }
            else
            {
                saveButtonText.text = "";
            }
        }
        else
        {
            saveButtonText.text = "Save Slot " + slotNum + "\n<empty>";
        }
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
            // If it exists, copy its content to the other json file
            File.Copy(saveFilePath, saveSlotFilePath, true);

            Debug.Log("Game state saved/loaded");
        }
        else
        {
            Debug.LogError(".json not found!");
        }
    }
}
