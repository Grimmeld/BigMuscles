using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightedSelector : MonoBehaviour, IPointerEnterHandler
{
    private Selectable _selectableUIElement;


    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectableUIElement.Select();
    }

    private void Awake()
    {
        _selectableUIElement = this.gameObject.GetComponent<Selectable>();
    }

}
