using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
public class ScriptPlotManagerScript : MonoBehaviour
{
    public TextMeshProUGUI phraseText;
    public TextMeshProUGUI phraseSpeakerName;

    public RawImage localBackground;

    public GameObject choicePanelOuter;
    public GameObject choicePanelInner;

    public TextMeshProUGUI choicesOnPanel;
    public TextMeshProUGUI selectedChoiceArrowArea;


    public GameObject characterLeft;
    public GameObject chracterCenter;
    public GameObject characterRight;


    private GameScript gameScript;
    private int currentDialogueIndex = 0;
    private Scene currentScene;
    [SerializeField] private ScriptLoader scriptLoader;

    void Start()
    {
        Debug.Log(localBackground.ToString());
        if (scriptLoader == null)
        {
            Debug.LogError("ScriptLoader is not assigned in Inspector!");
            return;
        }

        gameScript = scriptLoader.gameScript;

        if (gameScript == null)
        {
            Debug.LogError("Game script is not loaded!");
            return;
        }
        LoadScene("scene1");
    }

    void LoadScene(string sceneId)
    {
        foreach (Scene scene in gameScript.scenes)
        {
            if (scene.id == sceneId)
            {
                currentScene = scene;
                localBackground.texture = Resources.Load<Texture>(scene.background);
                currentDialogueIndex = 0;
                ShowDialogue();
                return;
            }
        }
    }

    public void ShowDialogue()
    {
        if (currentDialogueIndex >= currentScene.dialogue.Count)
        {
            //ShowChoices();
            return;
        }

        Dialogue line = currentScene.dialogue[currentDialogueIndex];
        phraseText.text = line.text;

        if (line.waitForInput)
        {
            currentDialogueIndex++;
        }
    }

    //void ShowChoices()
    //{
    //    choicePanel.SetActive(true);

    //    foreach (Transform child in choicePanel.transform)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    foreach (SceneChoices choice in currentScene.next.choices)
    //    {
    //        button.GetComponentInChildren<Text>().text = choice.text;
    //        button.onClick.AddListener(() => LoadScene(choice.nextScene));
    //    }
    //}

    public void OnNextClicked()
    {
        ShowDialogue();
    }
}

public class LocalBackground
{
}