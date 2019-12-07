// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script defines the cursor that is used for button selection in the Radial Menu. A
// sphere collider is moved based on the axis input to collide with and trigger buttons.
//
// Button hover colors, on click sound and haptics, as well as the triggering of button
// events are all handled from this script.

using System;
using UnityEngine;
using UnityEngine.UI;

namespace VRMM {

    public class MenuCursor : MonoBehaviour
    {
        public ELabelDisplay labelDisplayOption;
        public EHapticHand hapticHandOption;
        public EHapticIntensity hapticIntensityOption;
        public ESelectionButton selectionButton;
        public Material highlightMat;
        public bool playSound;
        public AudioSource clickAudio;
        public bool playHaptics;

        private string _selectButton;
        private Material _buttonMat;
        private HapticsController _hapticsController;
        private bool _useAxis;
        private bool _axisInUse;

        private void Start()
        {
            _hapticsController = GetComponent<HapticsController>();

            if (_selectButton != null) return;
            switch (selectionButton)
            {
                case ESelectionButton.LeftTrigger:
                    _selectButton = "VRMM_Trigger_Left";
                    break; 
                case ESelectionButton.RightTrigger:
                    _selectButton = "VRMM_Trigger_Right";
                    break;      
                case ESelectionButton.LeftThumbPress:
                    _selectButton = "VRMM_ThumbPress_Left";
                    break;  
                case ESelectionButton.RightThumbPress:
                    _selectButton = "VRMM_ThumbPress_Right";
                    break; 
                case ESelectionButton.AButtonOculusOnly:
                    _selectButton = "VRMM_OculusButton_A";
                    break; 
                case ESelectionButton.BButtonOculusOnly:
                    _selectButton = "VRMM_OculusButton_B";
                    break; 
                case ESelectionButton.XButtonOculusOnly:
                    _selectButton = "VRMM_OculusButton_X";
                    break; 
                case ESelectionButton.YButtonOculusOnly:
                    _selectButton = "VRMM_OculusButton_Y";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Update the button material to hover material and toggle label if applicable
        private void OnTriggerEnter(Collider other)
        {
            _buttonMat = null;

            var button = other.GetComponentInParent<RadialButton>();

            if (button == null) return;
            var component = button.GetComponent<Renderer>();
            if(component != null && _buttonMat == null)
            { 
                _buttonMat = component.sharedMaterial;
            }

            if(_buttonMat != null)
            {
                component.sharedMaterial = highlightMat;
            }

            if (labelDisplayOption != ELabelDisplay.ToggleOnHover) return;
            var buttonText = button.GetComponentInChildren<Text>(true);

            if (buttonText != null)
            {
                buttonText.gameObject.SetActive(true);
            }
        }

        // Handle button press
        private void OnTriggerStay(Collider other)
        {
            var button = other.GetComponentInParent<RadialButton>();

            if (button == null) return;
            clickAudio = button.GetComponent<AudioSource>();

            if(selectionButton == ESelectionButton.LeftTrigger || selectionButton == ESelectionButton.RightTrigger)
            {
                _useAxis = true;
            }
            if (!_useAxis && Input.GetButtonDown(_selectButton))
            {
                button.onButtonPress.Invoke();
                if(clickAudio != null && playSound)
                {
                    clickAudio.Play();
                }
                if(clickAudio != null && playHaptics)
                {
                    _hapticsController.Vibrate(hapticIntensityOption, hapticHandOption);
                }
            }
            else if(_useAxis && Input.GetAxisRaw(_selectButton) > 0.5)
            {
                if (_axisInUse) return;
                _axisInUse = true;
                button.onButtonPress.Invoke();
                if(clickAudio != null && playSound)
                {
                    clickAudio.Play();
                }
                if(clickAudio != null && playHaptics)
                {
                    _hapticsController.Vibrate(hapticIntensityOption, hapticHandOption);
                }
            }
            else if(_useAxis && Input.GetAxisRaw(_selectButton) == 0)
            {
                if(_axisInUse)
                {
                    _axisInUse = false;
                }
            }
        }

        // Change material back to default and toggle label if applicable
        private void OnTriggerExit(Collider other)
        {
            var button = other.GetComponentInParent<RadialButton>();

            if (button == null) return;
            var component = button.GetComponent<Renderer>();

            if(component != null)
            {
                component.sharedMaterial = _buttonMat;
                    
                if(component.sharedMaterial != highlightMat)
                {
                    _buttonMat = null;
                }
            }

            if (labelDisplayOption != ELabelDisplay.ToggleOnHover) return;
            var buttonText = button.GetComponentInChildren<Text>(true);

            if (buttonText != null)
            {
                buttonText.gameObject.SetActive(false);
            }
        }
    }
}
