using UnityEngine;
using System.IO;

public class ScriptLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/Scripts/script.json";
    public GameScript gameScript;

    void Start()
    {
        gameScript = LoadScript();

        if (gameScript != null)
        {
            Scene startScene = gameScript.scenes.Find(s => s.id == "start");

            if (startScene != null)
            {
                Debug.Log("First scene found: " + startScene.id);

                if (!string.IsNullOrEmpty(startScene.music))
                {
                    Debug.Log("Playing first scene music: " + startScene.music);
                    AudioClip firstSceneMusic = Resources.Load<AudioClip>("Music/" + startScene.music);

                    if (firstSceneMusic != null && MusicManager.Instance != null)
                    {
                        MusicManager.Instance.PlayMusic(firstSceneMusic, fadeDuration: 0.5f, forceRestart: true);
                    }
                    else
                    {
                        Debug.LogError("First scene music not found: " + startScene.music);
                    }
                }
                else
                {
                    Debug.Log("No music specified for the first scene.");
                }
            }
            else
            {
                Debug.LogError("First scene not found in script.json.");
            }
        }
    }


    public GameScript LoadScript()
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
        return gameScript;
    }
}
