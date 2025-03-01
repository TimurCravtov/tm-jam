using System.Collections;
using UnityEngine;
using TMPro;

public class TextCreator : MonoBehaviour
{
    public TMP_Text viewText;
    private string transferText;
    public float textSpeed = 0.03f; // Time between letters

    public void StartTextScroll(string text)
    {
        StopAllCoroutines(); // Stop any ongoing text animations
        transferText = text;
        viewText.text = ""; // Clear text before starting
        StartCoroutine(RollText());
    }

    IEnumerator RollText()
    {
        foreach (char c in transferText)
        {
            viewText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
