// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This class is where the magic happens. It takes all of the input options you selected
// in the Menu Maker window and builds your VR ready menu. It contains the functions to 
// build new menus as well as update existing menus already existing in the scene

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VRMM.Editor
{

    public class MenuMaker : MonoBehaviour
    {

        public static void MakeMenu(
            string menuName,
            GameObject radialMenuPrefab,
            Material buttonHighlightMat,
            GameObject buttonPrefab,
            EButtonStyles buttonStyle,
            Material[] buttonMats,
            int numberOfButtons, 
            ELabelDisplay labelDisplay,
            Font labelFont,
            EHapticHand hapticHand,
            EHapticIntensity hapticIntensity,
            ESelectionButton selectionButton,
            Object handAttachPoint,
            bool buttonsMatch,
            Color sharedButtonColor,
            Color[] buttonColors,
            string[] buttonLabels,
            Object[] buttonIcons,
            bool playSoundOnClick,
            Object onClickSound,
            bool toggleMenu,
            EToggleButton menuToggleButton
            )
        {
            //Create new menu
            var radialMenuClone = Instantiate(radialMenuPrefab);
            radialMenuClone.name = menuName == "" ? "RadialMenu" : menuName;
            radialMenuClone.GetComponent<RadialMenu>().buttonStyle = buttonStyle;

            if(toggleMenu)
            {
                var menuToggle = radialMenuClone.GetComponent<MenuToggle>();
                menuToggle.toggle = true;
                menuToggle.toggleButton = menuToggleButton;
            }

            //Add Unity Event list of proper length
            var buttonEvents = radialMenuClone.GetComponent<ButtonEvents>();
            for (var i = 0; i < numberOfButtons; i++)
            {
                buttonEvents.eventList.Add(new UnityEvent());
            }

            //Find Menu Cursor in instantiated menu and pass on needed variables
            var cursor = radialMenuClone.GetComponentInChildren<MenuCursor>();
            cursor.labelDisplayOption = labelDisplay;

            //Check if play haptics
            cursor.playHaptics = hapticHand != EHapticHand.NoHaptics;
            cursor.hapticHandOption = hapticHand;
            cursor.hapticIntensityOption = hapticIntensity;
            cursor.highlightMat = buttonHighlightMat;
            cursor.selectionButton = selectionButton;

            //Attach menu to specified attach point
            var anchorAttach = radialMenuClone.GetComponent<AttachToAnchor>();
            anchorAttach.attachPoint = handAttachPoint as GameObject;

            //Create buttons for menu
            for (var i = 0; i < numberOfButtons; i++)
            {
                //Create each button as a child of the button container using specific button model
                var buttonClone = Instantiate(buttonPrefab, radialMenuClone.GetComponentInChildren<RadialButtonContainer>().transform);

                var renderer = buttonClone.GetComponent<Renderer>();
                renderer.material = buttonMats[i];
                renderer.sharedMaterial.color = buttonsMatch ? sharedButtonColor : buttonColors[i];

                //Handle button labels
                var buttonText = buttonClone.GetComponentInChildren<Text>(true);
                buttonText.text = buttonLabels[i];
                if(labelFont == null)
                {
                    var defaultFont = Resources.Load<Font>("Fonts/Rubik-Regular");
                    labelFont = defaultFont;
                }
                buttonText.font = labelFont;

                if (labelDisplay == ELabelDisplay.AlwaysShow)
                {
                    buttonText.gameObject.SetActive(true);
                }

                //Handle button icons
                var buttonIcon = buttonClone.GetComponentInChildren<Image>();
                if (buttonIcons[i] == null)
                {
                    buttonIcon.color = Color.clear;
                }
                else
                {
                    buttonIcon.sprite = buttonIcons[i] as Sprite;
                }

                //Rotate each button to proper spot around center and name GameObject appropriately
                buttonClone.transform.rotation = Quaternion.Euler(new Vector3(0, ((360 / numberOfButtons) * i) - ((360 / numberOfButtons) / 2) - 90, 0));
                buttonClone.transform.position = Vector3.zero;
                buttonClone.name = buttonLabels[i] == null ? "Button " + (i + 1) : buttonLabels[i];

                //Check button position and flip label/icon if on bottom half of menu
                if (buttonText.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonText.transform.Rotate(new Vector3(180, 180, 0));
                }
                if (buttonIcon.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonIcon.transform.Rotate(new Vector3(180, 180, 0));
                }

                //Make sure Audio exists or is disabled
                var audioSource = buttonClone.GetComponent<AudioSource>();
                if (playSoundOnClick)
                {
                    cursor.playSound = true;
                    if (onClickSound != null)
                    {
                        audioSource.clip = onClickSound as AudioClip;
                    }
                    else
                    {
                        playSoundOnClick = false;
                    }
                    audioSource.playOnAwake = false;
                }
                else
                {
                    audioSource.enabled = false;
                }
            }
            Debug.Log("Menu built. Find RadialMenu object in scene to add button events!");
        }

        public static void UpdateMenu(
            RadialMenu currentMenu,
            Color highlightColor,
            GameObject buttonPrefab,
            EButtonStyles buttonStyle,
            Material[] buttonMats,
            int numberOfButtons, 
            ELabelDisplay labelDisplay,
            Font labelFont,
            EHapticHand hapticHand,
            EHapticIntensity hapticIntensity,
            ESelectionButton selectionButton,
            Object handAttachPoint,
            bool buttonsMatch,
            Color sharedButtonColor,
            Color[] buttonColors,
            string[] buttonLabels,
            Object[] buttonIcons,
            bool playSoundOnClick,
            Object onClickSound,
            bool menuToggle,
            EToggleButton menuToggleButton
            )
        {
            currentMenu.GetComponent<RadialMenu>().buttonStyle = buttonStyle;
            var currentButtons = currentMenu.GetComponentsInChildren<RadialButton>();
            foreach (var button in currentButtons)
            {
                DestroyImmediate(button.gameObject);
            }

            //Add Unity Event list of proper length
            var buttonEvents = currentMenu.GetComponent<ButtonEvents>();

            if(numberOfButtons > currentButtons.Length)
            {
                for (var i = 0; i < numberOfButtons - currentButtons.Length; i++)
                {
                    buttonEvents.eventList.Add(new UnityEvent());
                }
            }
            else
            {
                buttonEvents.eventList.RemoveRange(numberOfButtons - 1, currentButtons.Length - numberOfButtons);
            }

            //Find Menu Cursor in instantiated menu and pass on needed variables
            var cursor = currentMenu.GetComponentInChildren<MenuCursor>();
            cursor.labelDisplayOption = labelDisplay;

            // Check if play haptics
            cursor.playHaptics = hapticHand != EHapticHand.NoHaptics;

            cursor.hapticHandOption = hapticHand;
            cursor.hapticIntensityOption = hapticIntensity;
            cursor.highlightMat.color = highlightColor;
            cursor.selectionButton = selectionButton;

            //Attach menu to specified attach point
            var handAttach = currentMenu.GetComponent<AttachToAnchor>();
            handAttach.attachPoint = handAttachPoint as GameObject;


            //Create buttons for menu
            for (var i = 0; i < numberOfButtons; i++)
            {
                //Create each button as a child of the button container using specific button model
                var buttonClone = Instantiate(buttonPrefab, currentMenu.GetComponentInChildren<RadialButtonContainer>().transform);

                var renderer = buttonClone.GetComponent<Renderer>();
                renderer.material = buttonMats[i];
                renderer.sharedMaterial.color = buttonsMatch ? sharedButtonColor : buttonColors[i];

                //Handle button labels
                var buttonText = buttonClone.GetComponentInChildren<Text>(true);
                buttonText.text = buttonLabels[i];
                buttonText.font = labelFont;
                if (labelDisplay == ELabelDisplay.AlwaysShow)
                {
                    buttonText.gameObject.SetActive(true);
                }

                //Handle button icons
                var buttonIcon = buttonClone.GetComponentInChildren<Image>();
                if (buttonIcons[i] == null)
                {
                    buttonIcon.color = Color.clear;
                }
                else
                {
                    buttonIcon.sprite = buttonIcons[i] as Sprite;
                }

                //Rotate each button to proper spot around center and name GameObject appropriately
                buttonClone.transform.rotation = Quaternion.Euler(new Vector3(0, ((360 / numberOfButtons) * i) - ((360 / numberOfButtons) / 2) - 90, 0));
                buttonClone.transform.localPosition = Vector3.zero;
                buttonClone.name = buttonLabels[i] == null ? "Button " + (i + 1) : buttonLabels[i];

                //Check button position and flip label/icon if on bottom half of menu
                if (buttonText.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonText.transform.Rotate(new Vector3(180, 180, 0));
                }
                if (buttonIcon.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonIcon.transform.Rotate(new Vector3(180, 180, 0));
                }

                //Make sure Audio exists or is disabled
                var audioSource = buttonClone.GetComponent<AudioSource>();
                if (playSoundOnClick)
                {
                    cursor.playSound = true;
                    if (onClickSound != null)
                    {
                        audioSource.clip = onClickSound as AudioClip;
                    }
                    else
                    {
                        playSoundOnClick = false;
                    }
                    audioSource.playOnAwake = false;
                }
                else
                {
                    audioSource.enabled = false;
                }
            }
            Debug.Log("Menu built. Find RadialMenu object in scene to add button events!");
        }
    }
}