using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;

public class ScriptPlotManagerScript : MonoBehaviour
{
    [SerializeField] private TextCreator phrase;
    public TextMeshProUGUI phraseText;
    public TextMeshProUGUI phraseSpeakerName;
    public RawImage localBackground;
    public GameObject choicePanelOuter;
    public GameObject choicePanelInner;
    public TextMeshProUGUI choiceTextArea;

    public GameObject characterLeft;
    public GameObject characterCenter;
    public GameObject characterRight;
    private GameScript gameScript;
    private int currentDialogueIndex = 0;
    private Scene currentScene;
    [SerializeField] private ScriptLoader scriptLoader;
    [SerializeField] private SceneTransitionManager sceneTransitionManager;

    private string saveFilePath = "Assets/Resources/save.json";

    // Variables for choice selection
    private int currentChoiceIndex = 0;
    private bool isChoosingOption = false;
    

    public string GetCurrentSceneId()
    {
        return currentScene.id;
    }
    void Start()
    {
        choicePanelOuter.SetActive(false);

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
        StartCoroutine(LoadSceneWithFade("scene1"));
    }

    void Update()
    {
        if (isChoosingOption)
        {
            HandleChoiceNavigation();
        }
        else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            ShowDialogue();
        }
    }

    void HandleChoiceNavigation()
    {
        if (currentScene.next.choices == null || currentScene.next.choices.Count == 0)
            return;

        int choicesCount = currentScene.next.choices.Count;

        // Up arrow to navigate to previous choice
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentChoiceIndex = (currentChoiceIndex - 1 + choicesCount) % choicesCount;
            UpdateChoiceSelection();
        }
        // Down arrow to navigate to next choice
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentChoiceIndex = (currentChoiceIndex + 1) % choicesCount;
            UpdateChoiceSelection();
        }
        // Enter key to select choice
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SelectChoice(currentChoiceIndex);
        }
    }

    void UpdateChoiceSelection()
    {
        // Update the choice text with the selector indicator
        UpdateChoiceText();
    }

    IEnumerator LoadSceneWithFade(string sceneId)
    {
        if (sceneId != "scene1")
        {
            yield return StartCoroutine(sceneTransitionManager.Fade("FadeOut", "FadeIn", 1f));
        }

        foreach (Scene scene in gameScript.scenes)
        {
            if (scene.id == sceneId)
            {
                currentScene = scene;

                localBackground.texture = Resources.Load<Texture>($"Backgrounds/{scene.background}");
                if (scene.transition)
                {
                    StartCoroutine(sceneTransitionManager.Fade("FadeIn", "FadeOut", 1f));
                }

                currentDialogueIndex = 0;
                isChoosingOption = false;
                ShowDialogue();
                yield break;
            }
        }
        Debug.LogError("scene not load");
    }

    public void ShowDialogue()
    {
        if (currentDialogueIndex >= currentScene.dialogue.Count)
        {
            HandleNextNavigation();
            return;
        }

        Dialogue line = currentScene.dialogue[currentDialogueIndex];

        // Use the new scrolling text function
        phrase.StartTextScroll(line.text);

        // Display speaker name
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
        phraseSpeakerName.text = speakingCharacter != null ? speakingCharacter.name : "";

        // Update character display
        UpdateCharacterDisplay(line);

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
            StartCoroutine(LoadSceneWithFade(currentScene.next.scene));
        }
        // Otherwise, check if there are choices
        else if (currentScene.next.choices != null && currentScene.next.choices.Count > 0)
        {
            ShowChoices();
            isChoosingOption = true;
            currentChoiceIndex = 0;
            UpdateChoiceSelection();
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
        if (currentScene.next.choices == null || currentScene.next.choices.Count == 0)
        {
            choicePanelOuter.SetActive(false);
            return;
        }

        choicePanelOuter.SetActive(true);

        int choicesCount = currentScene.next.choices.Count;

        float choiceAdditionalHeight = 80;
        float defaultContainerHeight = 60f;

        RectTransform outerRect = choicePanelOuter.GetComponent<RectTransform>();
        float outerHeight = defaultContainerHeight + (choiceAdditionalHeight * choicesCount);

        outerRect.sizeDelta = new Vector2(outerRect.sizeDelta.x, outerHeight);

        // Make sure the choice text area is active
        choiceTextArea.gameObject.SetActive(true);

        // Update the choice text with the selector indicator
        UpdateChoiceText();
    }

    private void UpdateChoiceText()
    {
        // Format the choices text with the selector indicator (>>) next to the current choice
        string choicesText = "";
        for (int i = 0; i < currentScene.next.choices.Count; i++)
        {
            if (i > 0) choicesText += "\n\n";

            // Add selection indicator (>>) to the currently selected choice
            if (i == currentChoiceIndex)
            {
                choicesText += ">> ";
            }
            else
            {
                choicesText += "    ";
            }

                choicesText += currentScene.next.choices[i].text;
        }

        choiceTextArea.text = choicesText;
    }

    void SelectChoice(int index)
    {
        if (currentScene.next.choices != null && index < currentScene.next.choices.Count)
        {
            SceneChoices selectedChoice = currentScene.next.choices[index];

            // Reset choice state
            isChoosingOption = false;
            currentChoiceIndex = 0;

            // Hide choice panel
            choicePanelOuter.SetActive(false);

            // Load the next scene based on the choice
            if (!string.IsNullOrEmpty(selectedChoice.nextScene))
            {
                StartCoroutine(LoadSceneWithFade(selectedChoice.nextScene));
            }
            else
            {
                Debug.LogWarning($"Choice at index {index} does not have a valid next scene.");
            }
        }
    }
}

