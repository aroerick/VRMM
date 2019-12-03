// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This class is where the magic happens. It takes all of the input options you selected
// in the Menu Maker window and builds your VR ready menu. It contains the functions to 
// build new menus as well as update existing menus already existing in the scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRMM;

namespace VRMM
{

    public class MenuMaker : MonoBehaviour
    {

        public static void MakeMenu(
            string _menuName,
            GameObject _radialMenuPrefab,
            Material _buttonHighlightMat,
            GameObject _buttonPrefab,
            string _buttonStyle,
            Material[] _buttonDefaultMats,
            int _numberOfButtons, 
            string _labelDisplay,
            Font _labelFont,
            string _hapticHand,
            string _hapticIntensity,
            string _selectionButton,
            Object _handAttachPoint,
            bool _buttonsMatch,
            Color _sharedButtonColor,
            Color[] _buttonColors,
            string[] _buttonLabels,
            Object[] _buttonIcons,
            bool _playSoundOnClick,
            Object _onClickSound
            )
        {
            //Create new menu
            var radialMenuClone = Instantiate(_radialMenuPrefab);
            radialMenuClone.name = _menuName == "" ? "RadialMenu" : _menuName;
            radialMenuClone.GetComponent<RadialMenu>().buttonStyle = _buttonStyle;

            //Add Unity Event list of proper length
            var buttonEvents = radialMenuClone.GetComponent<ButtonEvents>();
            for (var i = 0; i < _numberOfButtons; i++)
            {
                buttonEvents.eventList.Add(new UnityEvent());
            }

            //Find Menu Cursor in instantiated menu and pass on needed variables
            //TODO: Haptics are currently tied to OVR and thus only work with Oculus
            //      Get Vive haptics figured out
            var cursor = radialMenuClone.GetComponentInChildren<MenuCursor>();
            cursor.labelDisplayOption = _labelDisplay;
            if (_hapticHand == "No Haptics")
            {
                cursor.playHaptics = false;
            }
            else
            {
                cursor.playHaptics = true;
            }
            cursor.hapticHandOption = _hapticHand;
            cursor.hapticIntensityOption = _hapticIntensity;
            cursor.highlightMat = _buttonHighlightMat;
            cursor.selectButtonOption = _selectionButton;

            //Attach menu to specified attach point
            var anchorAttach = radialMenuClone.GetComponent<AttachToAnchor>();
            anchorAttach.attachPoint = _handAttachPoint as GameObject;

            //Create buttons for menu
            for (var i = 0; i < _numberOfButtons; i++)
            {
                //Create each button as a child of the button container using specific button model
                var buttonClone = Instantiate(_buttonPrefab, radialMenuClone.GetComponentInChildren<RadialButtonContainer>().transform);

                var renderer = buttonClone.GetComponent<Renderer>();
                renderer.material = _buttonDefaultMats[i];
                if (_buttonsMatch)
                {
                    renderer.sharedMaterial.color = _sharedButtonColor;
                }
                else
                {
                    renderer.sharedMaterial.color = _buttonColors[i];
                }

                //Handle button labels
                var buttonText = buttonClone.GetComponentInChildren<Text>(true);
                buttonText.text = _buttonLabels[i];
                if(_labelFont == null)
                {
                    var defaultFont = Resources.Load<Font>("Fonts/Rubik-Regular");
                    _labelFont = defaultFont;
                }
                buttonText.font = _labelFont;

                if (_labelDisplay == "Always Show")
                {
                    buttonText.gameObject.SetActive(true);
                }

                //Handle button icons
                var buttonIcon = buttonClone.GetComponentInChildren<Image>();
                if (_buttonIcons[i] == null)
                {
                    buttonIcon.color = Color.clear;
                }
                else
                {
                    buttonIcon.sprite = _buttonIcons[i] as Sprite;
                }

                //Rotate each button to proper spot around center and name GameObject appropriately
                buttonClone.transform.rotation = Quaternion.Euler(new Vector3(0, ((360 / _numberOfButtons) * i) - ((360 / _numberOfButtons) / 2) - 90, 0));
                buttonClone.transform.position = Vector3.zero;
                buttonClone.name = _buttonLabels[i] == null ? "Button " + (i + 1) : _buttonLabels[i];

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
                if (_playSoundOnClick)
                {

                    if (_onClickSound != null)
                    {
                        audioSource.clip = _onClickSound as AudioClip;
                    }
                    else
                    {
                        _playSoundOnClick = false;
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
            RadialMenu _currentMenu,
            Material _buttonHighlightMat,
            GameObject _buttonPrefab,
            string _buttonStyle,
            Material[] _buttonDefaultMats,
            int _numberOfButtons, 
            string _labelDisplay,
            Font _labelFont,
            string _hapticHand,
            string _hapticIntensity,
            string _selectionButton,
            Object _handAttachPoint,
            bool _buttonsMatch,
            Color _sharedButtonColor,
            Color[] _buttonColors,
            string[] _buttonLabels,
            Object[] _buttonIcons,
            bool _playSoundOnClick,
            Object _onClickSound
            )
        {
            _currentMenu.GetComponent<RadialMenu>().buttonStyle = _buttonStyle;
            var currentButtons = _currentMenu.GetComponentsInChildren<RadialButton>();
            for(var i = 0; i < currentButtons.Length; i++)
            {
                DestroyImmediate(currentButtons[i].gameObject);
            }

            //Add Unity Event list of proper length
            var buttonEvents = _currentMenu.GetComponent<ButtonEvents>();

            if(_numberOfButtons > currentButtons.Length)
            {
                for (var i = 0; i < _numberOfButtons - currentButtons.Length; i++)
                {
                    buttonEvents.eventList.Add(new UnityEvent());
                }
            }
            else
            {
                buttonEvents.eventList.RemoveRange(_numberOfButtons - 1, currentButtons.Length - _numberOfButtons);
            }

            //Find Menu Cursor in instantiated menu and pass on needed variables
            //TODO: Haptics are currently tied to OVR and thus only work with Oculus
            //      Get Vive haptics figured out
            var cursor = _currentMenu.GetComponentInChildren<MenuCursor>();
            cursor.labelDisplayOption = _labelDisplay;
            if (_hapticHand == "No Haptics")
            {
                cursor.playHaptics = false;
            }
            else
            {
                cursor.playHaptics = true;
            }
            cursor.hapticHandOption = _hapticHand;
            cursor.hapticIntensityOption = _hapticIntensity;
            cursor.highlightMat = _buttonHighlightMat;
            cursor.selectButtonOption = _selectionButton;

            //Attach menu to specified attach point
            var handAttach = _currentMenu.GetComponent<AttachToAnchor>();
            handAttach.attachPoint = _handAttachPoint as GameObject;


            //Create buttons for menu
            for (var i = 0; i < _numberOfButtons; i++)
            {
                //Create each button as a child of the button container using specific button model
                var buttonClone = Instantiate(_buttonPrefab, _currentMenu.GetComponentInChildren<RadialButtonContainer>().transform);

                var renderer = buttonClone.GetComponent<Renderer>();
                renderer.material = _buttonDefaultMats[i];
                if (_buttonsMatch)
                {
                    renderer.sharedMaterial.color = _sharedButtonColor;
                }
                else
                {
                    renderer.sharedMaterial.color = _buttonColors[i];
                }

                //Handle button labels
                var buttonText = buttonClone.GetComponentInChildren<Text>(true);
                buttonText.text = _buttonLabels[i];
                buttonText.font = _labelFont;
                if (_labelDisplay == "Always Show")
                {
                    buttonText.gameObject.SetActive(true);
                }

                //Handle button icons
                var buttonIcon = buttonClone.GetComponentInChildren<Image>();
                if (_buttonIcons[i] == null)
                {
                    buttonIcon.color = Color.clear;
                }
                else
                {
                    buttonIcon.sprite = _buttonIcons[i] as Sprite;
                }

                //Rotate each button to proper spot around center and name GameObject appropriately
                buttonClone.transform.rotation = Quaternion.Euler(new Vector3(0, ((360 / _numberOfButtons) * i) - ((360 / _numberOfButtons) / 2) - 90, 0));
                buttonClone.transform.localPosition = Vector3.zero;
                buttonClone.name = _buttonLabels[i] == null ? "Button " + (i + 1) : _buttonLabels[i];

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
                if (_playSoundOnClick)
                {

                    if (_onClickSound != null)
                    {
                        audioSource.clip = _onClickSound as AudioClip;
                    }
                    else
                    {
                        _playSoundOnClick = false;
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