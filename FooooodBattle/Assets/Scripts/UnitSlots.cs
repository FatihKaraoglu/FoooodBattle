using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSlots : MonoBehaviour, IDropHandler
{
    //dropped == boughtUnit
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        Debug.Log(dropped);
        DraggableShopCard draggableCard = dropped.GetComponent<DraggableShopCard>();
        draggableCard.parentAfterDrag = transform;
        ArenaManager.Instance.Buy(gameObject, dropped);
    }
}
