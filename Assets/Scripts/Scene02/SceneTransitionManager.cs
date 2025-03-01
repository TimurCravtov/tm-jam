using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    [SerializeField] GameObject fadeScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Fade("FadeIn", 1f));
    }

    public IEnumerator Fade(string pType, float duration)
    {
        fadeScreen.SetActive(true);
        fadeAnimator.SetTrigger(pType); // Play fade out animation
        yield return new WaitForSeconds(duration);
        fadeScreen.SetActive(false);
    }
}
