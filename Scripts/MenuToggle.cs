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
        public bool toggle;
        public e_toggleButton toggleButton;

        private string toggleButtonInput;
        private string[] toggleAxisInput;
        private bool isEnabled;
        private RadialButtonContainer buttons;
        private bool axisInUse;

        //Finds the buttons that are within this menu
        private void Start() 
        {
            buttons = GetComponentInChildren<RadialButtonContainer>(true);

            if(toggleButtonInput == null)
            {
                switch (toggleButton)
                {
                    case e_toggleButton.LeftThumbAxis:
                        buttons.gameObject.SetActive(isEnabled);
                        isEnabled = !isEnabled;
                        toggleAxisInput = new string[]{"VRMM_Horizontal_Left", "VRMM_Vertical_Left"};
                        break;
                    case e_toggleButton.RightThumbAxis:
                        buttons.gameObject.SetActive(isEnabled);
                        isEnabled = !isEnabled;
                        toggleAxisInput = new string[]{"VRMM_Horizontal_Right", "VRMM_Vertical_Right"};
                        break;
                    case e_toggleButton.LeftTrigger:
                        toggleButtonInput = "VRMM_Trigger_Left";
                        break; 
                    case e_toggleButton.RightTrigger:
                        toggleButtonInput = "VRMM_Trigger_Right";
                        break;      
                    case e_toggleButton.LeftThumbPress:
                        toggleButtonInput = "VRMM_ThumbPress_Left";
                        break;  
                    case e_toggleButton.RightThumbPress:
                        toggleButtonInput = "VRMM_ThumbPress_Right";
                        break; 
                    case e_toggleButton.AButtonOculusOnly:
                        toggleButtonInput = "VRMM_OculusButton_A";
                        break; 
                    case e_toggleButton.BButtonOculusOnly:
                        toggleButtonInput = "VRMM_OculusButton_B";
                        break; 
                    case e_toggleButton.XButtonOculusOnly:
                        toggleButtonInput = "VRMM_OculusButton_X";
                        break; 
                    case e_toggleButton.YButtonOculusOnly:
                        toggleButtonInput = "VRMM_OculusButton_Y";
                        break;  
                }
            }
        }

        // Show or hide
        void Update()
        {
            ToggleMenu(toggleAxisInput, toggleButtonInput);
        }

        private void ToggleMenu(string[] axisInput, string buttonInput)
        {

            if (
                axisInput != null &&
                Input.GetAxisRaw(axisInput[0]) > 0.1 || 
                Input.GetAxisRaw(axisInput[1]) > 0.1 || 
                Input.GetAxisRaw(axisInput[0]) < -0.1 || 
                Input.GetAxisRaw(axisInput[1]) < -0.1)
            {
                if(!axisInUse)
                {
                    axisInUse = true;
                    buttons.gameObject.SetActive(isEnabled);
                    isEnabled = !isEnabled;
                }
            }
            else if(    
                axisInput != null &&            
                Input.GetAxisRaw(axisInput[0]) == 0 && 
                Input.GetAxisRaw(axisInput[1]) == 0 && 
                Input.GetAxisRaw(axisInput[0]) == -0 && 
                Input.GetAxisRaw(axisInput[1]) == -0)
            {
                if(axisInUse)
                {
                    axisInUse = false;
                    buttons.gameObject.SetActive(isEnabled);
                    isEnabled = !isEnabled;
                }
            }
            else if(buttonInput != null && Input.GetButtonDown(buttonInput))
            {
                buttons.gameObject.SetActive(isEnabled);
                isEnabled = !isEnabled;
            }
        }
    }
}
