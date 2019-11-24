using System;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using VRMM;

namespace VRMM {

    public class MenuBuilderWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private GUIStyle _buttonHeaderStyle;

        private GameObject[] _buttonPrefabs;
        private GameObject _radialMenuPrefab;
        private Material[] _buttonDefaultMats;
        private Material _buttonHighlightMat;

        private int _buttonStyleIndex;
        private readonly string[] _buttonStyleOptions = 
            { "Choose...", "Standard", "LowPoly" };
        private int _numberOfButtons;
        private int _labelDisplayIndex;
        private readonly string[] _labelDisplayOptions = 
            { "Toggle on Hover", "Always Show", "None" };

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


        [MenuItem("Tools/VR Menu Builder")]
        public static void OpenMenuBuilder()
        {
            GetWindow<MenuBuilderWindow>("VR Menu Builder");
        }

        public void OnGUI()
        {
            _buttonHeaderStyle = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold
            };

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Radial Menu Options", _buttonHeaderStyle);

            GUILine();

            //Loading content needed to build menu
            _buttonPrefabs = Resources.LoadAll<GameObject>("MenuBuilder/ButtonPrefabs/" + _buttonStyleOptions[_buttonStyleIndex]);
            _radialMenuPrefab = Resources.Load<GameObject>("MenuBuilder/MenuPrefabs/RadialMenu");
            _defaultClickSound = Resources.Load<AudioClip>("MenuBuilder/Sound/DefaultButtonPress");

            _buttonDefaultMats = Resources.LoadAll<Material>("MenuBuilder/Materials/Buttons");
            _buttonHighlightMat = Resources.Load<Material>("MenuBuilder/Materials/Highlight/ButtonHighlight_Mat");

            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // General Button Options
            EditorGUILayout.LabelField("General Button Options", _buttonHeaderStyle);
            _buttonStyleIndex = EditorGUILayout.Popup("Button Style", _buttonStyleIndex, _buttonStyleOptions);
            _numberOfButtons = EditorGUILayout.IntSlider("Number of Buttons", _numberOfButtons, 2, 8);
            _labelDisplayIndex = EditorGUILayout.Popup("Button Label Mode", _labelDisplayIndex, _labelDisplayOptions);

            GUILine();

            //Color Options
            EditorGUILayout.LabelField("Color Options", _buttonHeaderStyle);
            _buttonsMatch = EditorGUILayout.Toggle("All buttons same color", _buttonsMatch);
            if (_buttonsMatch)
            {
                _sharedButtonColor = EditorGUILayout.ColorField("Button Color", _sharedButtonColor);
            }
            _sharedHighlightColor = EditorGUILayout.ColorField("Button Hover Color", _sharedHighlightColor);
            _buttonHighlightMat.color = _sharedHighlightColor;

            GUILine();

            //Control Options
            EditorGUILayout.LabelField("Control Options", _buttonHeaderStyle);
            _selectionButtonIndex = EditorGUILayout.Popup("Confirm Selection Button", _selectionButtonIndex, _selectionButtonOptions);

            GUILine();

            //Misc Options
            EditorGUILayout.LabelField("Misc Options", _buttonHeaderStyle);
            _handAttachPoint = EditorGUILayout.ObjectField("Menu Hand Attach Point", _handAttachPoint, typeof(GameObject), true);
            _playSoundOnClick = EditorGUILayout.Toggle("Play Sound on Click", _playSoundOnClick);
            if (_playSoundOnClick)
            {
                _onClickSound = EditorGUILayout.ObjectField("Sound on Click", _onClickSound, typeof(AudioClip), true);
            }
            _onClickSound = _defaultClickSound;
            _hapticHandIndex = EditorGUILayout.Popup("Haptics", _hapticHandIndex, _hapticHandOptions);
            if (_hapticHandOptions[_hapticHandIndex] != "No Haptics")
            {
                _hapticIntensityIndex = EditorGUILayout.Popup("Haptic Intensity", _hapticIntensityIndex, _hapticIntensityOptions);
            }

            GUILine();

            //Individual Button Options
            _showButtonOptions = EditorGUILayout.Foldout(_showButtonOptions, "Individual Button Options", true);
            if (_showButtonOptions)
            {

                for (var i = 0; i < _numberOfButtons; i++)
                {
                    if (i == 0)
                    {
                        EditorGUILayout.Space();
                    }

                    EditorGUILayout.LabelField("Button " + (i + 1), _buttonHeaderStyle);
                    _buttonLabels[i] = EditorGUILayout.TextField("Name", _buttonLabels[i]);

                    if (!_buttonsMatch)
                    {
                        _buttonColors[i] = EditorGUILayout.ColorField("Button Color", _buttonColors[i]);
                    }
                    _buttonIcons[i] = EditorGUILayout.ObjectField("Icon", _buttonIcons[i], typeof(Sprite), false);

                    GUILine();
                }
            }

            GUILayout.EndScrollView();

            //Create menu on press of 'Build Menu' button
            if (_buttonStyleIndex != 0 && GUILayout.Button("Build Menu"))
            {
                try{
                    MenuBuilder.BuildMenu(
                        _radialMenuPrefab,
                        _buttonHighlightMat,
                        _buttonPrefabs,
                        _buttonDefaultMats,
                        _numberOfButtons, 
                        _labelDisplayOptions, 
                        _labelDisplayIndex, 
                        _hapticHandOptions,
                        _hapticHandIndex,
                        _hapticIntensityOptions,
                        _hapticIntensityIndex,
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
                catch (Exception e)
                {
                    Debug.LogError("Error: " + e.Message);
                }
            }
            else if(_buttonStyleIndex == 0 && GUILayout.Button("Build Menu"))
            {
                Debug.Log("Please select a button style to create your menu!");
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
    }

}