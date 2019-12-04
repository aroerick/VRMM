// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script takes the Unity Events you define on the Radial Menu and passes them
// down to the appropriate buttons as click events

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRMM {

    public class ButtonEvents : MonoBehaviour
    {
        public List<UnityEvent> eventList;
        
        private RadialButton[] buttons;
        
        // Send the Unity event from eventList to the corresponding button
        private void Start() {
            buttons = GetComponentsInChildren<RadialButton>();

            for(var i = 0; i < buttons.Length; i++){
                buttons[i].SetOnButtonPress(eventList[i]);
            }
        }
    }
}
