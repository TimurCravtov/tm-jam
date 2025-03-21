using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private TextMeshProUGUI myText;

    void Start()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.color = Color.grey;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.color = Color.white;
    }
}