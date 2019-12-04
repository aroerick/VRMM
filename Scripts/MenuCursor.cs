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

using UnityEngine;
using UnityEngine.UI;

namespace VRMM {

    public class MenuCursor : MonoBehaviour
    {
        public e_labelDisplay labelDisplayOption;
        public e_hapticHand hapticHandOption;
        public e_hapticIntensity hapticIntensityOption;
        public e_selectionButton selectionButton;
        public Material highlightMat;
        public bool playSound;
        public AudioSource clickAudio;
        public bool playHaptics;

        private string selectButton;
        private RadialButton[] radialButtons;
        private Material buttonMat = null;
        private OculusHapticsController hapticsController;

        void Start()
        {
            radialButtons = FindObjectsOfType<RadialButton>();
            hapticsController = GetComponent<OculusHapticsController>();
            
            if(selectButton == null)
            {
                switch (selectionButton)
                {
                    case e_selectionButton.LeftTrigger:
                        selectButton = "VRMM_Trigger_Left";
                        break; 
                    case e_selectionButton.RightTrigger:
                        selectButton = "VRMM_Trigger_Right";
                        break;      
                    case e_selectionButton.LeftThumbPress:
                        selectButton = "VRMM_ThumbPress_Left";
                        break;  
                    case e_selectionButton.RightThumbPress:
                        selectButton = "VRMM_ThumbPress_Right";
                        break; 
                    case e_selectionButton.AButtonOculusOnly:
                        selectButton = "VRMM_OculusButton_A";
                        break; 
                    case e_selectionButton.BButtonOculusOnly:
                        selectButton = "VRMM_OculusButton_B";
                        break; 
                    case e_selectionButton.XButtonOculusOnly:
                        selectButton = "VRMM_OculusButton_X";
                        break; 
                    case e_selectionButton.YButtonOculusOnly:
                        selectButton = "VRMM_OculusButton_Y";
                        break;  
                }
            }
        }

        // Update the button material to hover material and toggle label if applicable
        private void OnTriggerEnter(Collider other)
        {
            buttonMat = null;

            var button = other.GetComponentInParent<RadialButton>();

            if (button != null)
            {
                var renderer = button.GetComponent<Renderer>();
                if(renderer != null && buttonMat == null)
                { 
                    buttonMat = renderer.material;
                }

                if(buttonMat != null)
                {
                    renderer.material = highlightMat;
                }
                
                if(labelDisplayOption == e_labelDisplay.TaggleOnHover)
                {
                    Text buttonText = button.GetComponentInChildren<Text>(true);

                    if (buttonText != null)
                    {
                        buttonText.gameObject.SetActive(true);
                    }
                }

            }
        }

        // Handle button press
        private void OnTriggerStay(Collider other)
        {
            RadialButton button = other.GetComponentInParent<RadialButton>();

            if(button != null)
            {
                clickAudio = button.GetComponent<AudioSource>();
            }

            if (button != null)
            {
                if (Input.GetButtonDown(selectButton) || Input.GetAxis(selectButton) > 0.5)
                {
                    button.onButtonPress.Invoke();
                    if(clickAudio != null && playSound)
                        clickAudio.Play();
                    if(clickAudio != null && playHaptics)
                        hapticsController.Vibrate(hapticIntensityOption, hapticHandOption);
                }
            }
        }

        // Change material back to default and toggle label if applicable
        private void OnTriggerExit(Collider other)
        {
            var button = other.GetComponentInParent<RadialButton>();

            if (button != null)
            {
                var renderer = button.GetComponent<Renderer>();

                if(renderer != null)
                {
                    renderer.material = buttonMat;
                    
                    if(renderer.material != highlightMat)
                    {
                        buttonMat = null;
                    }
                }

                if (labelDisplayOption == e_labelDisplay.TaggleOnHover)
                {
                    Text buttonText = button.GetComponentInChildren<Text>(true);

                    if (buttonText != null)
                    {
                        buttonText.gameObject.SetActive(false);
                    }
                }
                
            }
        }
    }
}
