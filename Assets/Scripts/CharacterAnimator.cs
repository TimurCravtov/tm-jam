using TMPro;
using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class CharacterAnimator : MonoBehaviour
{
    private GameObject characterObject;
    private DialogueCharacter character;
    public float closeMouthDuration = 0.08f;
    public float openMouthDuration = 0.15f;

    private Func<bool> isSpeaking;
    

    public void StartAnimation(GameObject pCharacterObject, DialogueCharacter pCharacter, Func<bool> pIsSpeaking)
    {
        StopAllCoroutines(); // Stop any ongoing text animations
        characterObject = pCharacterObject;
        character = pCharacter;
        isSpeaking = pIsSpeaking;
        StartCoroutine(Speak());
    }

    IEnumerator Speak()
    {
        string emotionPath = $"Characters/{character.id}/{character.emotion}";
        while (isSpeaking())
        {
            Texture openMouth = Resources.Load<Texture>(emotionPath + "_o");
            Texture closedMouth = Resources.Load<Texture>(emotionPath);

            if (openMouth != null)
            {
                characterObject.GetComponent<RawImage>().texture = openMouth;
                yield return new WaitForSeconds(openMouthDuration);

            }
            else break;

            if (closedMouth != null)
            {
                characterObject.GetComponent<RawImage>().texture = closedMouth;
                yield return new WaitForSeconds(closeMouthDuration);
            }
            else break;
        }
    }
}
