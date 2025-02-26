using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonBehaviour : MonoBehaviour
{
    [Header("Scene Transition")]
    public string targetSceneName = "Gameplay";
    public Animator transitionAnimator;
    public float transitionTime = 1.0f;
    public string animationTriggerName = "FadeIn";

    // Reference to button
    private Button button;

    void Start()
    {
        // Get the button component and add listener
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(TransitToAnotherStage);
        }

        // Make sure we have an animator
        if (transitionAnimator == null)
        {
            Debug.LogWarning("No transition animator assigned to ButtonBehaviour. Please assign an animator with transition animation.");
        }
    }

    public void TransitToAnotherStage()
    {
        Debug.Log("Starting transition to scene: " + targetSceneName);
        StartCoroutine(LoadSceneWithTransition());
    }

    IEnumerator LoadSceneWithTransition()
    {
        // Trigger the animation
        if (transitionAnimator != null)
        {
            transitionAnimator.SetTrigger(animationTriggerName);

            // Wait for animation to complete
            yield return new WaitForSeconds(transitionTime);
        }

        // Load the new scene
        SceneManager.LoadScene(targetSceneName);
    }

    public void Exit()
    {
        Debug.Log("Exiting application");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}