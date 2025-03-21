using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Generic;

public class ResetSave : MonoBehaviour
{
    public string saveFilePath = "Assets/Resources/save.json";

    void Start()
    {

    }

    public void ResetSaveFile()
    {

        File.WriteAllText(saveFilePath, "");

        Debug.Log("New Game loaded");
    }
}
