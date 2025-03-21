using TMPro;
using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public GameObject choiceContainer;   // Reference to the ChoiceContainer GameObject
    public GameObject choicePrefab;      // Reference to the TextMeshPro prefab for choices
    public string[] choices;            // Array to hold the choice options
    public Transform choiceContainerInner; // Reference to the ChoiceContainerInner to display choices

    private int currentChoice = 0;       // Index for the current choice
    private string displayChoices = "";  // String to hold the formatted choices

    void Start()
    {
        // Initially, populate the choices
        UpdateChoices();
    }

    void Update()
    {
        // Input detection for moving between choices
        if (Input.GetKeyDown(KeyCode.UpArrow)) { currentChoice = Mathf.Max(0, currentChoice - 1); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { currentChoice = Mathf.Min(choices.Length - 1, currentChoice + 1); }

        // Update the displayed choices based on the current index
        UpdateChoices();
    }

    void UpdateChoices()
    {
        // Clear the existing choices
        foreach (Transform child in choiceContainerInner)
        {
            Destroy(child.gameObject);
        }

        // Create new choices dynamically
        for (int i = 0; i < choices.Length; i++)
        {
            GameObject newChoice = Instantiate(choicePrefab, choiceContainerInner);
            TextMeshProUGUI choiceText = newChoice.GetComponent<TextMeshProUGUI>();

            // Format the text with the ">>" symbol for the selected choice
            if (i == currentChoice)
                choiceText.text = ">> " + choices[i];
            else
                choiceText.text = "   " + choices[i];
        }

        // Dynamically resize the container based on the number of choices
        RectTransform containerRect = choiceContainer.GetComponent<RectTransform>();
        containerRect.sizeDelta = new Vector2(containerRect.sizeDelta.x, choices.Length * 30);  // Adjust height based on choice count
    }
}

