using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScenePlotManagerScript : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI phrase;
    public RawImage anitaObject;
    TimeSpan currentTime;
    TimeSpan startTime;
    public int rr;
    private bool isSpeaking = false;

    private string anitaBaseTextureStr = "Characters/Anita/";
    private string emotion = "neutral";
    private bool blinking = false;
    private bool talking = false;

    // Speech text to display
    private string[] dialogueLines = new string[] {
        "Hello, my name is Maria from Chisinau.",
        "It's nice to meet you today.",
        "I'm here to guide you through this demonstration.",
        "Let me know if you need any assistance."
    };

    // Emotion states for different dialogue lines
    private string[] emotionStates = new string[] {
        "neutral",
        "neutral",
        "neutral",
        "neutral",
    };

    private int currentDialogueLine = 0;
    private float speakDuration = 7f; // Speaking for 7 seconds
    private float pauseDuration = 2f; // Pause between speaking

    void Start()
    {
        currentTime = DateTime.Now.TimeOfDay;
        startTime = currentTime;

        characterName.text = "Maria";
        phrase.text = "";

        UpdateTexture();
        StartCoroutine(SpeakingSequence());
    }

    void Update()
    {
        currentTime = DateTime.Now.TimeOfDay;

        // Handle blinking every 3 seconds for 500ms
        if (currentTime.Seconds % 3 == 0 && currentTime.Milliseconds < 500)
        {
            blinking = true;
        }
        else
        {
            blinking = false;
        }

        UpdateTexture();
    }

    void UpdateTexture()
    {
        // Build texture path based on current state
        string texturePath = anitaBaseTextureStr + emotion;

        if (talking)
        {
            texturePath += "_o"; // Open mouth when talking
        }

        if (blinking)
        {
            texturePath += "_blinking"; // Add blinking state
        }

        // Load the appropriate texture
        anitaObject.texture = Resources.Load<Texture>(texturePath);
    }

    IEnumerator SpeakingSequence()
    {
        yield return new WaitForSeconds(1f); // Initial delay

        while (true)
        {
            // Start speaking
            isSpeaking = true;
            talking = true;
            phrase.text = dialogueLines[currentDialogueLine];
            emotion = emotionStates[currentDialogueLine];

            // Speaking for 7 seconds with mouth animation
            float speakEndTime = Time.time + speakDuration;
            while (Time.time < speakEndTime)
            {
                // Toggle talking state every 0.2 seconds for mouth movement
                if (Time.time % 0.4f < 0.2f)
                {
                    talking = true;
                }
                else
                {
                    talking = false;
                }

                yield return null;
            }

            // Pause between sentences
            isSpeaking = false;
            talking = false;
            phrase.text = "";
            yield return new WaitForSeconds(pauseDuration);

            // Move to next dialogue line
            currentDialogueLine = (currentDialogueLine + 1) % dialogueLines.Length;
        }
    }
}