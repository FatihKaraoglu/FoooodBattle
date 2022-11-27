using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableShopCard : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public List<Image> _imageList;
    [HideInInspector] public Transform parentAfterDrag;
    public bool _clicked = false;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clicked == false)
        {
            if(ArenaManager.Instance.selectedCard != null && ArenaManager.Instance.selectedCard != gameObject)
            {
                ArenaManager.Instance.selectedCard.GetComponent<DraggableShopCard>().changeScaleSmaller();
                ArenaManager.Instance.selectedCard.GetComponent<DraggableShopCard>()._clicked = false;
            }
            ArenaManager.Instance.selectedCard = gameObject;
            _clicked = true;
            changeScaleBigger();
        } else
        {
            _clicked = false;
            changeScaleSmaller();
        }  
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        changeScaleBigger();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_clicked == false)
        {
            changeScaleSmaller();
        }
       
    }

    public void changeScaleBigger()
    {
        gameObject.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void changeScaleSmaller()
    {
        gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
