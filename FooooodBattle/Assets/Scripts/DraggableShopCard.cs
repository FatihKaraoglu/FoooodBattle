using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableShopCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public List<Image> _imageList;
    [HideInInspector] public Transform parentAfterDrag;
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        transform.SetAsLastSibling();
        foreach(var image in _imageList)
        {
            image.raycastTarget = false;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        gameObject.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        parentAfterDrag.SetAsLastSibling();
        foreach (var image in _imageList)
        {
            image.raycastTarget = true;
        }
    }
}
