using System.Collections;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    [SerializeField] GameObject fadeScreen;

    private bool isFading = false; // Track if a fade transition is occurring

    public bool IsFading() => isFading; // Allow other scripts to check if a fade is active

    public IEnumerator Fade(string initTriggerName, string resetTriggerName, float duration)
    {
        isFading = true; // Block input
        fadeAnimator.ResetTrigger(resetTriggerName);
        fadeScreen.SetActive(true);
        fadeAnimator.SetTrigger(initTriggerName); // Start fade animation

        // Wait until the animation actually completes
        yield return new WaitForSeconds(duration);

        fadeScreen.SetActive(false);
        isFading = false; // Allow input again
    }
}
