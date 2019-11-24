﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRMM;

public class ButtonEvents : MonoBehaviour
{
    // public UnityEvent[] events;
    public List<UnityEvent> eventList;
    
    private RadialButton[] buttons;
    private void Start() {
        buttons = GetComponentsInChildren<RadialButton>();

        for(var i = 0; i < buttons.Length; i++){
            buttons[i].SetOnButtonPress(eventList[i]);
        }
    }
}
