using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler {
    
    public FMODUnity.StudioEventEmitter emitter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        emitter.Play();
    }

}
