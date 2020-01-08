using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject childText = null; 

    void Start()
    {
        Text text = GetComponentInChildren<Text>();
        if (text != null)
        {
            childText = text.gameObject;
            childText.SetActive(false);
        }
    }
    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        childText.SetActive(true);
    }
    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        childText.SetActive(false);
    }

}
