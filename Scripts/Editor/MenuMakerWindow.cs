﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;

// This class creates the window that houses the menu builder interface. It creates all the needed variables 
// to make the menu, has functions to validate the options chosen to build and update your menu, and a
// function to load the data of an existing menu into the window.
//
// This script places a Tools dropdown on your Unity toolbar which houses the VR Menu Maker window

namespace VRMM {


    public class MenuMakerWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private GUIStyle _headerStyle;
        private GUIStyle _foldoutHeaderStyle;
        private GUILayoutOption[] _layoutOptions;

        private GameObject[] _buttonPrefabs;
        private GameObject _radialMenuPrefab;
        private Material[] _buttonMats;
        private Material _buttonHighlightMat;

        private int _buildOptionsIndex;
        private string[] _buildOptions = 
            { "Build New Menu", "Update Existing Menu" };
        private int _menuUpdateOptionsIndex;
        private string[] _menuUpdateOptions;
        private UnityObject _menuUpdateOption;
        private List<RadialMenu> _menusInScene = new List<RadialMenu>();
        private string _menuName;

        private int _buttonStyleIndex;
        private readonly string[] _buttonStyleOptions = 
            { "Choose...", "Standard", "LowPoly" };
        private int _numberOfButtons;
        private int _labelDisplayIndex;
        private readonly string[] _labelDisplayOptions = 
            { "Toggle on Hover", "Always Show", "None" };
        private UnityObject _labelFont;


        private bool _buttonsMatch = true;
        private Color _sharedButtonColor = new Color(0.8f, 0.8f, 0.8f);
        private Color _sharedHighlightColor = new Color(0.58f, 0.29f, 0.75f);

        private int _selectionButtonIndex;
        private readonly string[] _selectionButtonOptions = 
            { 
                "Left Trigger", 
                "Right Trigger", 
                "Left Thumb Press", 
                "Right Thumb Press", 
                "A Button (Oculus Only)", 
                "B Button (Oculus Only)", 
                "X Button (Oculus Only)", 
                "Y Button (Oculus Only)" 
            };

        private UnityObject _handAttachPoint;
        private bool _playSoundOnClick;
        private UnityObject _onClickSound;
        private AudioClip _defaultClickSound;
        private int _hapticHandIndex;
        private readonly string[] _hapticHandOptions = 
            { "No Haptics", "Left", "Right" };
        private int _hapticIntensityIndex;
        private readonly string[] _hapticIntensityOptions = 
            { "Light", "Medium", "Hard" };

        private bool _showButtonOptions;
        private readonly string[] _buttonLabels = new string[8];
        private readonly Color[] _buttonColors = new Color[8];
        private readonly UnityObject[] _buttonIcons = new UnityObject[8];


        [MenuItem("Tools/VR Menu Maker")]
        public static void OpenMenuMaker()
        {
            GetWindow<MenuMakerWindow>("VR Menu Maker");
        }

        private void OnEnable() 
        {
            _labelFont = Resources.Load<Font>("Fonts/Rubik-Regular");
            EditorWindow window = this;
        }

        public void OnGUI()
        {
            this.maxSize = new Vector2(500f, 1000f);
            var currentMenus = FindObjectsOfType<RadialMenu>();
            foreach(RadialMenu menu in currentMenus)
            {
                if(_menusInScene != null && !_menusInScene.Contains(menu))
                {
                    _menusInScene.Add(menu);
                }
            }
            if(currentMenus.Length > 0)
            {
                foreach(RadialMenu menu in _menusInScene.ToList())
                {
                    if(!currentMenus.Contains(menu))
                    {
                        _menusInScene.Remove(menu);
                    }
                }
            }

            _headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                clipping = TextClipping.Overflow
            };
            _foldoutHeaderStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };
            _layoutOptions = new GUILayoutOption[]
            {
                GUILayout.MinWidth(300),
                GUILayout.MaxWidth(500)
            };

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Radial Menu Options", _headerStyle);
            
            if(currentMenus.Length > 0 )
            {
                _buildOptionsIndex = GUILayout.SelectionGrid(_buildOptionsIndex, new string[]{ "Build New Menu", "Update Existing Menu"}, 2, _layoutOptions);
                EditorGUILayout.Space();
            }
            else
            {
                _buildOptionsIndex = 0;
                _menuUpdateOption = null;
                _menusInScene = new List<RadialMenu>();
            }

            if(_buildOptionsIndex == 1)
            {
                _menuUpdateOptions = new string[_menusInScene.Count];
                for(var i = 0; i < _menuUpdateOptions.Length; i++)
                {
                    _menuUpdateOptions[i] = _menusInScene[i].gameObject.name;
                }
                _menuUpdateOption = EditorGUILayout.ObjectField("Menu to Update", _menuUpdateOption, typeof(RadialMenu), true, _layoutOptions);
                if(_menuUpdateOption != null) 
                {
                    _menuUpdateOptionsIndex = ArrayUtility.IndexOf<string>(_menuUpdateOptions, _menuUpdateOption.name);
                }

                if(_menuUpdateOption != null && GUILayout.Button("Load Menu", _layoutOptions))
                {
                    DisplayMenuToUpdate((RadialMenu)_menuUpdateOption);
                }
                else if(_menuUpdateOption == null && GUILayout.Button("Load Menu", _layoutOptions))
                {
                    Debug.LogError("VRMM: Please Select a Menu to Update.");
                }
            }

            GUILine();

            //Loading content needed to make menu
            _buttonPrefabs = Resources.LoadAll<GameObject>("ButtonPrefabs/" + _buttonStyleOptions[_buttonStyleIndex]);
            _radialMenuPrefab = Resources.Load<GameObject>("MenuPrefabs/RadialMenu");
            _defaultClickSound = Resources.Load<AudioClip>("Sound/DefaultButtonPress");

            _buttonHighlightMat = Resources.Load<Material>("Materials/Highlight/ButtonHighlight_Mat");


            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // General Button Options
            EditorGUILayout.LabelField("General Button Options", _headerStyle);
            if(_buildOptionsIndex == 0)
            {
                _menuName = EditorGUILayout.TextField("Menu Name", _menuName, _layoutOptions);
            }
            _buttonStyleIndex = EditorGUILayout.Popup("Button Style", _buttonStyleIndex, _buttonStyleOptions, _layoutOptions);
            _numberOfButtons = EditorGUILayout.IntSlider("Number of Buttons", _numberOfButtons, 2, 8, _layoutOptions);

            GUILine();

            EditorGUILayout.LabelField("Label Options", _headerStyle);
            _labelDisplayIndex = EditorGUILayout.Popup("Button Label Mode", _labelDisplayIndex, _labelDisplayOptions, _layoutOptions);
            _labelFont = EditorGUILayout.ObjectField("Label Font", _labelFont, typeof(Font), false, _layoutOptions);

            GUILine();

            //Color Options
            EditorGUILayout.LabelField("Color Options", _headerStyle);
            _buttonsMatch = EditorGUILayout.Toggle("All buttons same color", _buttonsMatch);
            if (_buttonsMatch)
            {
                _sharedButtonColor = EditorGUILayout.ColorField("Button Color", _sharedButtonColor, _layoutOptions);
            }
            _sharedHighlightColor = EditorGUILayout.ColorField("Button Hover Color", _sharedHighlightColor, _layoutOptions);
            _buttonHighlightMat.color = _sharedHighlightColor;

            GUILine();

            //Control Options
            EditorGUILayout.LabelField("Control Options", _headerStyle);
            _selectionButtonIndex = EditorGUILayout.Popup("Confirm Selection Button", _selectionButtonIndex, _selectionButtonOptions, _layoutOptions);

            GUILine();

            //Misc Options
            EditorGUILayout.LabelField("Misc Options", _headerStyle);
            _handAttachPoint = EditorGUILayout.ObjectField("Menu Hand Attach Point", _handAttachPoint, typeof(GameObject), true, _layoutOptions);
            _playSoundOnClick = EditorGUILayout.Toggle("Play Sound on Click", _playSoundOnClick);
            if (_playSoundOnClick)
            {
                _onClickSound = EditorGUILayout.ObjectField("Sound on Click", _onClickSound, typeof(AudioClip), true, _layoutOptions);
            }
            _onClickSound = _defaultClickSound;
            _hapticHandIndex = EditorGUILayout.Popup("Haptics", _hapticHandIndex, _hapticHandOptions, _layoutOptions);
            if (_hapticHandOptions[_hapticHandIndex] != "No Haptics")
            {
                _hapticIntensityIndex = EditorGUILayout.Popup("Haptic Intensity", _hapticIntensityIndex, _hapticIntensityOptions, _layoutOptions);
            }

            GUILine();

            //Individual Button Options
            _showButtonOptions = EditorGUILayout.Foldout(_showButtonOptions, "Individual Button Options", true, _foldoutHeaderStyle);
            if (_showButtonOptions)
            {

                for (var i = 0; i < _numberOfButtons; i++)
                {
                    if (i == 0)
                    {
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.LabelField("Button " + (i + 1), _headerStyle);
                    _buttonLabels[i] = EditorGUILayout.TextField("Button Name", _buttonLabels[i], _layoutOptions);

                    if (!_buttonsMatch)
                    {
                        _buttonColors[i] = EditorGUILayout.ColorField("Button Color", _buttonColors[i], _layoutOptions);
                    }
                    _buttonIcons[i] = EditorGUILayout.ObjectField("Button Icon", _buttonIcons[i], typeof(Sprite), false, _layoutOptions);

                    GUILine();
                }
            }

            GUILayout.EndScrollView();

            //Create menu on press of 'Make Menu' button
            if (_buttonStyleIndex != 0 && _buildOptionsIndex == 0 && GUILayout.Button("Make Menu", _layoutOptions))
            {
                try{
                    if(ValidateBuild(_menuName, _menusInScene, out _buttonMats, _numberOfButtons))
                    {
                        MenuMaker.MakeMenu(
                            _menuName,
                            _radialMenuPrefab,
                            _buttonHighlightMat,
                            _buttonPrefabs[_numberOfButtons - 2],
                            _buttonStyleOptions[_buttonStyleIndex],
                            _buttonMats,
                            _numberOfButtons, 
                            _labelDisplayOptions[_labelDisplayIndex],  
                            (Font)_labelFont,
                            _hapticHandOptions[_hapticHandIndex],
                            _hapticIntensityOptions[_hapticIntensityIndex],
                            _selectionButtonOptions[_selectionButtonIndex],
                            _handAttachPoint,
                            _buttonsMatch,
                            _sharedButtonColor,
                            _buttonColors,
                            _buttonLabels,
                            _buttonIcons,
                            _playSoundOnClick,
                            _onClickSound
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("VRMM: " + e.Message);
                }
            }
            else if (_buttonStyleIndex != 0 && _buildOptionsIndex == 1 && GUILayout.Button("Update Menu", _layoutOptions))
            {
                try{
                    if(ValidateUpdate(out _buttonMats, _numberOfButtons, _menuName, _menusInScene, _menuUpdateOptionsIndex))
                    {
                        MenuMaker.UpdateMenu(
                            _menusInScene[_menuUpdateOptionsIndex],
                            _buttonHighlightMat,
                            _buttonPrefabs[_numberOfButtons - 2],
                            _buttonStyleOptions[_buttonStyleIndex],
                            _buttonMats,
                            _numberOfButtons, 
                            _labelDisplayOptions[_labelDisplayIndex],
                            (Font)_labelFont,  
                            _hapticHandOptions[_hapticHandIndex],
                            _hapticIntensityOptions[_hapticIntensityIndex],
                            _selectionButtonOptions[_selectionButtonIndex],
                            _handAttachPoint,
                            _buttonsMatch,
                            _sharedButtonColor,
                            _buttonColors,
                            _buttonLabels,
                            _buttonIcons,
                            _playSoundOnClick,
                            _onClickSound
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("VRMM: " + e.Message);
                }
            }
            else if(_buttonStyleIndex == 0 && GUILayout.Button("Make Menu", _layoutOptions))
            {
                Debug.Log("VRMM: Please select a button style to create your menu!");
            }

            EditorGUILayout.Space();
        }

        //Used to draw section separation lines
        void GUILine(int i_height = 1)
        {
            EditorGUILayout.Space();

            Rect rect = EditorGUILayout.GetControlRect(false, i_height);
            rect.height = i_height;
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

            EditorGUILayout.Space();
        }

        // This functino takes in a RadialMenu present in the scene and reads its propertis,
        // then uses these properties to populate the window with that menu's current settings
        void DisplayMenuToUpdate(RadialMenu menu)
        {
            var menuButtons = menu.GetComponentsInChildren<RadialButton>();
            var menuCursor = menu.GetComponentInChildren<MenuCursor>();

            _menuName = menu.name;
            _buttonStyleIndex = ArrayUtility.IndexOf(_buttonStyleOptions, menu.GetComponent<RadialMenu>().buttonStyle);
            _numberOfButtons = menuButtons.Length;
            _labelDisplayIndex = ArrayUtility.IndexOf(_labelDisplayOptions, menuCursor.labelDisplayOption);
            for(var i = 0; i < menuButtons.Length; i++)
            {
                var rendererOne = menuButtons[i].GetComponent<Renderer>();
                var currentMatColor = rendererOne.sharedMaterial.color;

                if(i != menuButtons.Length - 1)
                {
                    var rendererTwo = menuButtons[i + 1].GetComponent<Renderer>();
                    var nextMatColor = rendererTwo.sharedMaterial.color;

                    if(currentMatColor != nextMatColor)
                    {
                        _buttonsMatch = false;
                        break;
                    }
                }
                _buttonsMatch = true;
            }
            _sharedButtonColor = menuButtons[0].GetComponent<Renderer>().sharedMaterial.color;
            _sharedHighlightColor = menuCursor.highlightMat.color;
            _selectionButtonIndex = ArrayUtility.IndexOf(_selectionButtonOptions, menuCursor.selectButtonOption);
            _handAttachPoint = menu.GetComponent<AttachToHand>().handAttachPoint;
            _playSoundOnClick = menuCursor.playSound;
            if(_playSoundOnClick)
            {
                _onClickSound = menuCursor.clickAudio;
            }
            _hapticHandIndex = ArrayUtility.IndexOf(_hapticHandOptions, menuCursor.hapticHandOption);
            if(_hapticHandIndex != 0)
            {
                _hapticIntensityIndex = ArrayUtility.IndexOf(_hapticIntensityOptions, menuCursor.hapticIntensityOption);
            }
            _buttonMats = new Material[menuButtons.Length];
            for(var i = 0; i < menuButtons.Length; i++)
            {
                _buttonLabels[i] = menuButtons[i].name;
                _buttonIcons[i] = menuButtons[i].GetComponentInChildren<Image>().sprite;
                _buttonMats[i] = menuButtons[i].GetComponent<Renderer>().sharedMaterial;
                if(!_buttonsMatch)
                {
                    _buttonColors[i] = _buttonMats[i].color;
                }
            }
        }

        private bool ValidateBuild(string _menuName, List<RadialMenu> _menusInScene, out Material[] _buttonMats, int _numberOfButtons)
        {
            if(_menuName == null || _menuName == "")
            {
                _menuName = "RadialMenu";
            }
            if(_menusInScene.Count > 0)
            {
                for(var i = 0; i < _menusInScene.Count; i++)
                {
                    if(_menusInScene[i].name == _menuName)
                    {
                        throw new Exception("Please use a unique name for your menu");
                    }
                }
            }

            if(!AssetDatabase.IsValidFolder(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}", _menuName)))
            {
                AssetDatabase.CreateFolder("Assets/VRMM/Resources/Materials/Buttons", _menuName);
            }
            else
            {
                var data = Resources.LoadAll(String.Format("Materials/Buttons/{0}", _menuName));
                foreach(UnityObject datum in data)
                {
                    var path = AssetDatabase.GetAssetPath(datum);
                    AssetDatabase.DeleteAsset(path);
                }
            }

            _buttonMats = new Material[_numberOfButtons];

            for(var i = 0; i < _numberOfButtons; i++)
            {
                var mat = new Material(Shader.Find("Standard"));
                
                AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
                _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
            }
            return true;
        }

        private bool ValidateUpdate(out Material[] _buttonMats, int _numberOfButtons, string _menuName, List<RadialMenu> _menusInScene, int _menuUpdateOptionsIndex)
        {
            _buttonMats = new Material[_numberOfButtons];
            var data = Resources.LoadAll<Material>(String.Format("Materials/Buttons/{0}", _menuName));
            foreach(Material datum in data)
            {
                var path = AssetDatabase.GetAssetPath(datum);
                AssetDatabase.DeleteAsset(path);
            }
            if(AssetDatabase.IsValidFolder(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}", _menusInScene[_menuUpdateOptionsIndex].name)))
            {
                var mats = Resources.LoadAll<Material>(String.Format("Materials/Buttons/{0}", _menusInScene[_menuUpdateOptionsIndex].name));

                for(var i = 0; i < _numberOfButtons - mats.Length; i++)
                {
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
                }
                for(var i = mats.Length; i < _numberOfButtons; i++)
                {
                    var mat = new Material(Shader.Find("Standard"));
                    
                    AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menusInScene[_menuUpdateOptionsIndex].name, i));
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
                }
            }
            else
            {
                AssetDatabase.CreateFolder("Assets/VRMM/Resources/Materials/Buttons", _menuName);

                for(var i = 0; i < _numberOfButtons; i++)
                {
                    var mat = new Material(Shader.Find("Standard"));
                    
                    AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/Buttons/{0}/Button_{1}_Mat.mat", _menuName, i));
                }
            }
            return true;
        }
    }

}