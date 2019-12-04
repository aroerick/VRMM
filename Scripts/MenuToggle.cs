// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// A script you can add to your menu (top level object) to toggle its visibility on button press.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRMM {

    public class MenuToggle : MonoBehaviour
    {
        private bool isEnabled;
        private RadialButtonContainer buttons;

        //Finds the buttons that are within this menu
        private void Start() 
        {
            buttons = GetComponentInChildren<RadialButtonContainer>(true);
        }

        // Show or hide
        void Update()
        {
            if (Input.GetButtonDown("VVRMM_TriggerPress_Left"))
            {
                buttons.gameObject.SetActive(isEnabled);
                isEnabled = !isEnabled;
            }
        }
    }
}
