using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public Animator fadeAnimator;
    [SerializeField] GameObject fadeScreenOut;
    public void ChangeScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        fadeScreenOut.SetActive(true);
        fadeAnimator.SetTrigger("FadeOut"); // Play fade out animation
        yield return new WaitForSeconds(1f); // Wait for animation to complete
        SceneManager.LoadScene(sceneName);
        
    }
}