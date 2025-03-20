using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerFinal : MonoBehaviour
{
    [SerializeField] private SceneTransitionManager transitionManager;
    [SerializeField] private Animator textAnimator;

    private void Start()
    {
        StartCoroutine(EnterSceneSequence());
    }

    private IEnumerator EnterSceneSequence()
    {
        // Ensure transitionManager is set
        if (transitionManager != null)
        {
            yield return transitionManager.Fade("FadeIn", "FadeOut", 1f);
        }

        // Start text animation after fade-in completes
        if (textAnimator != null)
        {
            textAnimator.SetTrigger("StartTextAnimation");
        }

        SceneManager.LoadScene("TitleScene");
    }
}
