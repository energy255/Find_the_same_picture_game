using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickManager : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string cardTag;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clicked = eventData.pointerCurrentRaycast.gameObject;
        if (clicked == null) return;

        if (!clicked.CompareTag("Untagged") && clicked.CompareTag(cardTag))
        {
            CardGameManager.Instance.SelectCardInput(clicked);
        }
    } 
}
