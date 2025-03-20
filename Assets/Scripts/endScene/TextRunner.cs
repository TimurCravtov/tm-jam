using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class ScrollCredits : MonoBehaviour
{
    [Header("References")]
    public Transform creditsContainer; // Parent container for text objects
    public GameObject textPrefab; // Prefab for each line of text

    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;
    public float spacing = 100f; // Space between lines
    public float initialDelay = 1f; // Wait before starting scroll
    public float fadeDistance = 150f; // Distance to fade in/out

    [Header("Content")]
    [TextArea(5, 10)]
    public string creditsText; // Enter credits directly in inspector
    public TextAsset creditsTextFile; // Or use a text file

    private Queue<string> creditLines = new Queue<string>(); // Lines to display
    private List<GameObject> activeLines = new List<GameObject>(); // Active text objects
    private bool hasStarted = false;
    private float timer = 0f;
    private float screenHeight;

    void Start()
    {
        screenHeight = Screen.height;

        // Load credits from text or file
        if (!string.IsNullOrEmpty(creditsText))
        {
            LoadCreditsFromText(creditsText);
        }
        else if (creditsTextFile != null)
        {
            LoadCreditsFromText(creditsTextFile.text);
        }
        else
        {
            // Example fallback credits
            creditLines.Enqueue("Director: John Doe");
            creditLines.Enqueue("Lead Developer: Jane Smith");
            creditLines.Enqueue("Music: Alan Walker");
            creditLines.Enqueue("Special Thanks to: You!");
        }
    }

    void Update()
    {
        // Initial delay
        if (!hasStarted)
        {
            timer += Time.deltaTime;
            if (timer >= initialDelay)
            {
                hasStarted = true;
                // Spawn the first few lines
                for (int i = 0; i < 3 && creditLines.Count > 0; i++)
                {
                    SpawnNextLine();
                }
            }
            return;
        }

        // Move all active text objects down
        for (int i = activeLines.Count - 1; i >= 0; i--)
        {
            GameObject line = activeLines[i];

            // Move the line DOWN
            line.transform.localPosition += Vector3.down * scrollSpeed * Time.deltaTime;

            // Handle fade in/out
            UpdateTextFade(line);

            // Remove text objects that are completely off-screen (bottom)
            if (line.transform.localPosition.y < -screenHeight - 100)
            {
                Destroy(line);
                activeLines.RemoveAt(i);
            }
        }

        // Check if we need to load a new line
        if (activeLines.Count > 0 && creditLines.Count > 0)
        {
            GameObject lastLine = activeLines[activeLines.Count - 1];
            float lastLineY = lastLine.transform.localPosition.y;

            // Load new line when the last one has moved far enough down
            float spawnThreshold = screenHeight - spacing;
            if (lastLineY <= spawnThreshold)
            {
                SpawnNextLine();
            }
        }

        // Check if all credits are done
        if (activeLines.Count == 0 && creditLines.Count == 0)
        {
            // Credits are finished - you can add an event here
            Debug.Log("Credits finished");
        }
    }

    private void SpawnNextLine()
    {
        if (creditLines.Count == 0) return;

        // Create a new text object
        GameObject newText = Instantiate(textPrefab, creditsContainer);
        TMP_Text tmpText = newText.GetComponent<TMP_Text>();
        tmpText.text = creditLines.Dequeue();

        // Set initial alpha to zero for fade-in
        Color textColor = tmpText.color;
        textColor.a = 0;
        tmpText.color = textColor;

        // Position at the top, just above the visible area
        newText.transform.localPosition = new Vector3(0, screenHeight + fadeDistance, 0);

        activeLines.Add(newText);
    }

    private void UpdateTextFade(GameObject textObject)
    {
        TMP_Text tmpText = textObject.GetComponent<TMP_Text>();
        float posY = textObject.transform.localPosition.y;

        // Fade in when entering from top
        float topFadeRegion = screenHeight;
        if (posY > topFadeRegion)
        {
            float fadeRatio = Mathf.InverseLerp(screenHeight + fadeDistance, topFadeRegion, posY);
            SetTextAlpha(tmpText, fadeRatio);
        }
        // Fade out when exiting at bottom
        else if (posY < fadeDistance)
        {
            float fadeRatio = Mathf.InverseLerp(-fadeDistance, fadeDistance, posY);
            SetTextAlpha(tmpText, fadeRatio);
        }
        // Full opacity in the middle
        else
        {
            SetTextAlpha(tmpText, 1f);
        }
    }

    private void SetTextAlpha(TMP_Text text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    private void LoadCreditsFromText(string text)
    {
        string[] lines = text.Split('\n');
        foreach (string line in lines)
        {
            string trimmedLine = line.Trim();
            if (!string.IsNullOrEmpty(trimmedLine))
            {
                creditLines.Enqueue(trimmedLine);
            }
        }
    }

    // Optional: Add this method to allow skipping the credits
    public void SkipCredits()
    {
        // Clear everything and end credits
        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }
        activeLines.Clear();
        creditLines.Clear();
    }
}