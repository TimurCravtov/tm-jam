using System.Collections;
using UnityEngine;

public class Scene02Event : MonoBehaviour
{

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
        yield return new WaitForSeconds(0.5f);
        fadeScreenIn.SetActive(false);
    }
}
