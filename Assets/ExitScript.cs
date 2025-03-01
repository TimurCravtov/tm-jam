using UnityEditor;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void QuitGame()
    {
        Debug.Log("Quit button clicked!");
#if UNITY_EDITOR
        EditorApplication.isPlaying = false; // Stops play mode in the Unity Editor
#else
                Application.Quit(); // Quits the application in a built version
#endif
    }
}
