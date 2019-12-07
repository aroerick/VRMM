// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script takes the Unity Events you define on the Radial Menu and passes them
// down to the appropriate buttons as click events

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace VRMM {

    public class ButtonEvents : MonoBehaviour
    {
        public List<UnityEvent> eventList;
        
        private RadialButton[] _buttons;
        
        // Send the Unity event from eventList to the corresponding button
        private void Start() {
            _buttons = GetComponentsInChildren<RadialButton>(true);

            for(var i = 0; i < _buttons.Length; i++){
                _buttons[i].SetOnButtonPress(eventList[i]);
            }
        }
    }
}
