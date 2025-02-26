using UnityEngine;
using System.IO;

public class ScriptLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/Scripts/script.json";
    public GameScript gameScript;

    void Start()
    {
        LoadScript();
    }

    void LoadScript()
    {
        if (File.Exists(jsonFilePath))
        {
            string jsonText = File.ReadAllText(jsonFilePath);
            gameScript = JsonUtility.FromJson<GameScript>(jsonText);
            Debug.Log("Script loaded successfully!");
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }
}
