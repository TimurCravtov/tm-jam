using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene02Event : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    [SerializeField] GameObject fadeScreenIn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(EventStarter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EventStarter() 
    {
        fadeScreenIn.SetActive(true);
        fadeAnimator.SetTrigger("FadeIn"); // Play fade out animation
        yield return new WaitForSeconds(1f);
        fadeScreenIn.SetActive(false);
        //yield return new WaitForSeconds(1f); // Wait for animation to complete
    }
}
