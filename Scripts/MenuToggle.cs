// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// A script you can add to your menu (top level object) to toggle its visibility on button press.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRMM {

    public class MenuToggle : MonoBehaviour
    {
        public bool toggle;
        public EToggleButton toggleButton;

        private string _toggleButtonInput;
        private string[] _toggleAxisInput;
        private bool _isEnabled;
        private RadialButtonContainer _buttons;
        private bool _axisInUse;

        //Finds the buttons that are within this menu
        private void Start() 
        {
            _buttons = GetComponentInChildren<RadialButtonContainer>(true);

            if (_toggleButtonInput != null) return;
            switch (toggleButton)
            {
                case EToggleButton.LeftThumbAxis:
                    _buttons.gameObject.SetActive(_isEnabled);
                    _isEnabled = !_isEnabled;
                    _toggleAxisInput = new[]{"VRMM_Horizontal_Left", "VRMM_Vertical_Left"};
                    break;
                case EToggleButton.RightThumbAxis:
                    _buttons.gameObject.SetActive(_isEnabled);
                    _isEnabled = !_isEnabled;
                    _toggleAxisInput = new[]{"VRMM_Horizontal_Right", "VRMM_Vertical_Right"};
                    break;
                case EToggleButton.LeftTrigger:
                    _toggleButtonInput = "VRMM_Trigger_Left";
                    break; 
                case EToggleButton.RightTrigger:
                    _toggleButtonInput = "VRMM_Trigger_Right";
                    break;      
                case EToggleButton.LeftThumbPress:
                    _toggleButtonInput = "VRMM_ThumbPress_Left";
                    break;  
                case EToggleButton.RightThumbPress:
                    _toggleButtonInput = "VRMM_ThumbPress_Right";
                    break; 
                case EToggleButton.AButtonOculusOnly:
                    _toggleButtonInput = "VRMM_OculusButton_A";
                    break; 
                case EToggleButton.BButtonOculusOnly:
                    _toggleButtonInput = "VRMM_OculusButton_B";
                    break; 
                case EToggleButton.XButtonOculusOnly:
                    _toggleButtonInput = "VRMM_OculusButton_X";
                    break; 
                case EToggleButton.YButtonOculusOnly:
                    _toggleButtonInput = "VRMM_OculusButton_Y";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Show or hide
        private void Update()
        {
            ToggleMenu(_toggleAxisInput, _toggleButtonInput);
        }

        private void ToggleMenu(IReadOnlyList<string> axisInput, string buttonInput)
        {

            if (
                axisInput != null &&
                Input.GetAxisRaw(axisInput[0]) > 0.1 || 
                Input.GetAxisRaw(axisInput[1]) > 0.1 || 
                Input.GetAxisRaw(axisInput[0]) < -0.1 || 
                Input.GetAxisRaw(axisInput[1]) < -0.1)
            {
                if (_axisInUse) return;
                _axisInUse = true;
                _buttons.gameObject.SetActive(_isEnabled);
                _isEnabled = !_isEnabled;
            }
            else if(    
                axisInput != null &&            
                Input.GetAxisRaw(axisInput[0]) == 0 && 
                Input.GetAxisRaw(axisInput[1]) == 0 && 
                Input.GetAxisRaw(axisInput[0]) == -0 && 
                Input.GetAxisRaw(axisInput[1]) == -0)
            {
                if (!_axisInUse) return;
                _axisInUse = false;
                _buttons.gameObject.SetActive(_isEnabled);
                _isEnabled = !_isEnabled;
            }
            else if(buttonInput != null && Input.GetButtonDown(buttonInput))
            {
                _buttons.gameObject.SetActive(_isEnabled);
                _isEnabled = !_isEnabled;
            }
        }
    }
}
