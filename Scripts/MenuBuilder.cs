using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRMM;

namespace VRMM
{

    public class MenuBuilder : MonoBehaviour
    {

        public static void BuildMenu(
            GameObject _radialMenuPrefab,
            Material _buttonHighlightMat,
            GameObject[] _buttonPrefabs,
            Material[] _buttonDefaultMats,
            int _numberOfButtons, 
            string[] _labelDisplayOptions, 
            int _labelDisplayIndex,
            string[] _hapticHandOptions,
            int _hapticHandIndex,
            string[] _hapticIntensityOptions,
            int _hapticIntensityIndex,
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
            radialMenuClone.name = "RadialMenu";

            //Add Unity Event list of proper length
            var buttonEvents = radialMenuClone.GetComponent<ButtonEvents>();
            for (var i = 0; i < _numberOfButtons; i++)
            {
                buttonEvents.eventList.Add(new UnityEvent());
            }

            //Find Menu Cursor in instantiated menu and pass on needed 
            var cursor = radialMenuClone.GetComponentInChildren<MenuCursor>();
            cursor.labelDisplayOption = _labelDisplayOptions[_labelDisplayIndex];
            if (_hapticHandOptions[_hapticHandIndex] == "No Haptics")
            {
                cursor.playHaptics = false;
            }
            else
            {
                cursor.playHaptics = true;
            }
            cursor.hapticHandOption = _hapticHandOptions[_hapticHandIndex];
            cursor.hapticIntensityOption = _hapticIntensityOptions[_hapticIntensityIndex];
            cursor.highlightMat = _buttonHighlightMat;

            var handAttach = radialMenuClone.GetComponent<AttachToHand>();
            handAttach.handAttachPoint = _handAttachPoint as GameObject;

            for (var i = 0; i < _numberOfButtons; i++)
            {
                var buttonPrefab = _buttonPrefabs[_numberOfButtons - 2];
                var buttonClone = Instantiate(buttonPrefab, radialMenuClone.GetComponentInChildren<RadialButtonContainer>().transform);

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

                var buttonText = buttonClone.GetComponentInChildren<Text>(true);
                buttonText.text = _buttonLabels[i];

                if (_labelDisplayOptions[_labelDisplayIndex] == "Always Show")
                {
                    buttonText.gameObject.SetActive(true);
                }

                var buttonIcon = buttonClone.GetComponentInChildren<Image>();

                if (_buttonIcons[i] == null)
                {
                    buttonIcon.color = Color.clear;
                }
                else
                {
                    buttonIcon.sprite = _buttonIcons[i] as Sprite;
                }

                buttonClone.transform.rotation = Quaternion.Euler(new Vector3(0, ((360 / _numberOfButtons) * i) - ((360 / _numberOfButtons) / 2) - 90, 0));
                buttonClone.transform.position = Vector3.zero;
                buttonClone.name = _buttonLabels[i] == null ? "Button " + (i + 1) : _buttonLabels[i];

                if (buttonText.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonText.transform.Rotate(new Vector3(180, 180, 0));
                }
                if (buttonIcon.transform.position.z > 0 || (buttonText.transform.position.z == 0 && buttonText.transform.position.x > 0))
                {
                    buttonIcon.transform.Rotate(new Vector3(180, 180, 0));
                }

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