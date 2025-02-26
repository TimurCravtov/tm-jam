using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    // Singleton pattern for easy access
    public static SceneTransitionManager Instance { get; private set; }

    [Header("Transition Settings")]
    public Animator transitionAnimator;
    public float transitionTime = 1.0f;
    public string animationTriggerName = "Fade";

    private void Awake()
    {
        // Singleton implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        // Play the transition animation
        transitionAnimator.SetTrigger(animationTriggerName);

        // Wait for the animation to complete
        yield return new WaitForSeconds(transitionTime);

        // Load the new scene
        SceneManager.LoadScene(sceneName);

        // You might want to reset the animation state or play a reverse animation here
        // For example, to fade out and then fade back in:
        transitionAnimator.SetTrigger("EndTransition");
    }
}

public class ButtonBehaviour : MonoBehaviour
{
    public string targetSceneName = "YourNextSceneName";

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(TransitToAnotherStage);
        }
    }

    public void TransitToAnotherStage()
    {
        // Use the transition manager to handle the scene transition
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("SceneTransitionManager not found in the scene!");
            // Fallback direct loading if no manager is available
            SceneManager.LoadScene(targetSceneName);
        }
    }
}