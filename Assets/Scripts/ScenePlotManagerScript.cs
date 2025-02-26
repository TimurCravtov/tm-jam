using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ScriptPlotManagerScript : MonoBehaviour
{
    public TextMeshProUGUI phraseText;
    public TextMeshProUGUI phraseSpeakerName;
    public RawImage localBackground;
    public GameObject choicePanelOuter;
    public GameObject choicePanelInner;
    public GameObject characterLeft;
    public GameObject characterCenter;
    public GameObject characterRight;
    public GameObject choiceButtonPrefab;
    private GameScript gameScript;
    private int currentDialogueIndex = 0;
    private Scene currentScene;
    [SerializeField] private ScriptLoader scriptLoader;

    void Start()
    {
        if (scriptLoader == null)
        {
            Debug.LogError("ScriptLoader is not assigned in Inspector!");
            return;
        }
        gameScript = scriptLoader.LoadScript();
        if (gameScript == null)
        {
            Debug.LogError("Game script is not loaded!");
            return;
        }
        LoadScene("scene1");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowDialogue();
        }
    }

    void LoadScene(string sceneId)
    {
        foreach (Scene scene in gameScript.scenes)
        {
            Debug.Log(scene.id);
            if (scene.id == sceneId)
            {
                currentScene = scene;
                localBackground.texture = Resources.Load<Texture>($"{scene.background}");

                if (localBackground.texture == null) {
                    Debug.LogError("Background is null");

                }
                currentDialogueIndex = 0;
                ShowDialogue();
                return;
            }
        }
        Debug.LogError("scene not load");
    }

    public void ShowDialogue()
    {
        Debug.Log(currentDialogueIndex);
        Debug.Log(currentScene.dialogue.Count);
        if (currentDialogueIndex >= currentScene.dialogue.Count)
        {
            ShowChoices();
            return;
        }

        Dialogue line = currentScene.dialogue[currentDialogueIndex];
        phraseText.text = line.text;

        // Find the speaking character to display their name
        DialogueCharacter speakingCharacter = null;
        if (line.characters != null)
        {
            foreach (DialogueCharacter character in line.characters)
            {
                if (character.isSpeaking)
                {
                    speakingCharacter = character;
                    break;
                }
            }
        }

        // Display speaker name if found
        phraseSpeakerName.text = speakingCharacter != null ? speakingCharacter.name : "";

        // Update character display
        UpdateCharacterDisplay(line);

        if (line.waitForInput)
        {
            currentDialogueIndex++;
        }
    }

    void UpdateCharacterDisplay(Dialogue line)
    {
        // Clear all character displays first
        characterLeft.SetActive(false);
        characterCenter.SetActive(false);
        characterRight.SetActive(false);

        // If there are no characters, return early
        if (line.characters == null || line.characters.Count == 0)
        {
            return;
        }

        // Display all characters in the scene
        foreach (DialogueCharacter character in line.characters)
        {
            GameObject characterObject = GetCharacterGameObject(character.position);
            if (characterObject != null && character.visible)
            {
                characterObject.SetActive(true);

                // Load the character emotion texture using the format: characterID/emotion
                string emotionPath = $"{character.id}/{character.emotion}";
                Texture characterTexture = Resources.Load<Texture>(emotionPath);

                if (characterTexture != null)
                {
                    characterObject.GetComponent<RawImage>().texture = characterTexture;
                }
                else
                {
                    Debug.LogWarning($"Could not load texture: {emotionPath}");
                }
            }
        }
    }

    // Helper method to get the appropriate character GameObject based on position
    GameObject GetCharacterGameObject(string position)
    {
        switch (position.ToLower())
        {
            case "left":
                return characterLeft;
            case "center":
                return characterCenter;
            case "right":
                return characterRight;
            default:
                Debug.LogWarning($"Unknown character position: {position}");
                return null;
        }
    }

    public void ShowChoices()
    {
        choicePanelOuter.SetActive(true);
        foreach (Transform child in choicePanelInner.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (SceneChoices choice in currentScene.next.choices)
        {
            GameObject choiceButton = Instantiate(choiceButtonPrefab, choicePanelInner.transform);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            choiceButton.GetComponent<Button>().onClick.AddListener(() => LoadScene(choice.nextScene));
        }
    }
}