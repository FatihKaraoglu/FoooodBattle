using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject _selectionBar;
    RectTransform _rectTransform;

    [SerializeField]
    public String sceneName;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(sceneName);
        LevelManager.Instance.loadScene(sceneName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectionBar.GetComponent<RectTransform>().anchoredPosition = gameObject.GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
    // Start is called before the first frame update

}
