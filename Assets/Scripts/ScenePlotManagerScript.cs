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
    public TextMeshProUGUI choiceTextArea;
    public TextMeshProUGUI variantChoosenArea;

    public GameObject characterLeft;
    public GameObject characterCenter;
    public GameObject characterRight;
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

                localBackground.texture = Resources.Load<Texture>($"Backgrounds/{scene.background}");

                if (localBackground.texture == null)
                {
                    Debug.LogError("Background is null");
                    Debug.LogError(scene.background); // scene.png

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
            HandleNextNavigation();
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

        ShowChoices();

        if (line.waitForInput)
        {
            currentDialogueIndex++;
        }
    }

    void HandleNextNavigation()
    {
        // Check if there's a direct next scene
        if (!string.IsNullOrEmpty(currentScene.next.scene))
        {
            // Direct navigation to the next scene
            LoadScene(currentScene.next.scene);
        }
        // Otherwise, check if there are choices
        else if (currentScene.next.choices != null && currentScene.next.choices.Count > 0)
        {
            ShowChoices();
        }
        else
        {
            Debug.LogWarning("No next navigation or choices found for scene: " + currentScene.id);
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
        // Only show choice panel if there are choices to display
        if (currentScene.next.choices == null || currentScene.next.choices.Count == 0)
        {
            choicePanelOuter.SetActive(false);
            return;
        }

        choicePanelOuter.SetActive(true);

        int choicesCount = currentScene.next.choices.Count;

        // Calculate sizes based on the number of choices
        float choiceHeight = 60f; // Height per choice
        float padding = 20f; // Padding around choices

        // Set outer panel size
        RectTransform outerRect = choicePanelOuter.GetComponent<RectTransform>();
        float outerHeight = (choiceHeight * choicesCount) + (padding * 2);
        outerRect.sizeDelta = new Vector2(outerRect.sizeDelta.x, outerHeight);

        // Set inner panel size (slightly smaller than the outer panel, with padding)
        RectTransform innerRect = choicePanelInner.GetComponent<RectTransform>();
        float innerHeight = (choiceHeight * choicesCount) + padding;
        innerRect.sizeDelta = new Vector2(innerRect.sizeDelta.x - padding * 2, innerHeight - padding * 2);

        // Center the inner panel inside the outer panel
        innerRect.anchoredPosition = Vector2.zero;

        // Build the choices text dynamically
        string choicesText = "";
        for (int i = 0; i < currentScene.next.choices.Count; i++)
        {
            if (i > 0) choicesText += "\n\n";
            choicesText += currentScene.next.choices[i].text;
        }

        choiceTextArea.text = choicesText;

        // Make sure the text is properly positioned within the inner panel
        RectTransform textRect = choiceTextArea.GetComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(innerRect.sizeDelta.x - padding * 2, innerHeight - padding * 2);
    }

}