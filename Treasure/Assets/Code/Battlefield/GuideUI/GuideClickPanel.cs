using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GuideClickPanel : MonoBehaviour, IPointerClickHandler
{
    public Action OnClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
