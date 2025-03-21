using System.Collections;
using UnityEngine;
using TMPro;

public class TextCreator : MonoBehaviour
{
    public TMP_Text viewText;
    private string transferText;
    public float textSpeed = 0.03f;
    private bool isSkipping = false;
    public bool isTextFullyDisplayed { get; private set; } = false; // Track text completion

    public void StartTextScroll(string text)
    {
        StopAllCoroutines(); // Stop any ongoing text animations
        transferText = text;
        viewText.text = ""; // Clear text before starting
        isSkipping = false;
        isTextFullyDisplayed = false; // Reset flag
        StartCoroutine(RollText());
    }

    IEnumerator RollText()
    {
        foreach (char c in transferText)
        {
            if (isSkipping)
            {
                viewText.text = transferText;
                isTextFullyDisplayed = true; // Mark text as fully displayed
                yield break;
            }

            viewText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTextFullyDisplayed = true; // Mark text as fully displayed after normal scrolling
    }

    public void SkipOrNext()
    {
        if (!isTextFullyDisplayed)
        {
            isSkipping = true;
        }
    }
}
