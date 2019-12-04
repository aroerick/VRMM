// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This class creates the window that houses the menu builder interface. It creates all the needed variables 
// to make the menu, has functions to validate the options chosen to build and update your menu, and a
// function to load the data of an existing menu into the window.
//
// This script places a Tools dropdown on your Unity toolbar which houses the VR Menu Maker window, Tools/VR Menu Maker


using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityObject = UnityEngine.Object;

namespace VRMM {

    public class MenuMakerWindow : EditorWindow
    {
        // Window variables
        private Vector2 _scrollPosition;
        private GUIStyle _headerStyle;
        private GUIStyle _foldoutHeaderStyle;
        private GUILayoutOption[] _layoutOptions;

        // Asset variables
        private GameObject[] _buttonPrefabs;
        private GameObject _radialMenuPrefab;
        private Material[] _buttonMats;
        private Material _buttonHighlightMat;

        // Radial Menu Options
        private e_buildOptions _buildOption;
        private int _menuUpdateOptionsIndex;
        private string[] _menuUpdateOptions;
        private UnityObject _menuUpdate;
        private List<RadialMenu> _menusInScene = new List<RadialMenu>();

        // General Options
        private string _menuName;
        private e_buttonStyles _buttonStyle; 
        private int _numberOfButtons;

        // Label Options
        private e_labelDisplay _labelDisplay;
        private UnityObject _labelFont;

        // Color Option
        private bool _buttonsMatch = true;
        private Color _sharedButtonColor = new Color(0.8f, 0.8f, 0.8f);
        private Color _sharedHighlightColor = new Color(0.58f, 0.29f, 0.75f);

        // Control Options
        private e_selectionButton _selectionButton;
        private bool _menuToggle = false;
        private e_toggleButton _menuToggleButton;

        // Misc Options
        private UnityObject _handAttachPoint;
        private bool _playSoundOnClick;
        private UnityObject _onClickSound;
        private AudioClip _defaultClickSound;
        private e_hapticHand _hapticHand;
        private e_hapticIntensity _hapticIntensity;

        // Individual Button Options
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
            _menuName = "My Custom Menu";
        }

        public void OnGUI()
        {
            this.maxSize = new Vector2(500f, 1000f);

            // Find all Radial Menus currently in scene
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

            // Styles
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
            
            // Check if there are current menus in scene.
            // If there are, display Build/Update option
            if(currentMenus.Length > 0 )
            {
                _buildOption = (e_buildOptions)GUILayout.SelectionGrid((int)_buildOption, new string[2]{e_buildOptions.BuildNewMenu.ToString(), e_buildOptions.UpdateExistingMenu.ToString() }, 2, _layoutOptions);
                EditorGUILayout.Space();
            }
            else
            {
                _buildOption = e_buildOptions.BuildNewMenu;
                _menuUpdate = null;
                _menusInScene = new List<RadialMenu>();
            }
            if(_buildOption == e_buildOptions.UpdateExistingMenu)
            {
                _menuUpdateOptions = new string[_menusInScene.Count];
                for(var i = 0; i < _menuUpdateOptions.Length; i++)
                {
                    _menuUpdateOptions[i] = _menusInScene[i].gameObject.name;
                }
                _menuUpdate = EditorGUILayout.ObjectField("Menu to Update", _menuUpdate, typeof(RadialMenu), true, _layoutOptions);
                if(_menuUpdate != null) 
                {
                    _menuUpdateOptionsIndex = ArrayUtility.IndexOf<string>(_menuUpdateOptions, _menuUpdate.name);
                }

                if(_menuUpdate != null && GUILayout.Button("Load Menu", _layoutOptions))
                {
                    DisplayMenuToUpdate((RadialMenu)_menuUpdate);
                }
                else if(_menuUpdate == null && GUILayout.Button("Load Menu", _layoutOptions))
                {
                    Debug.LogError("VRMM: Please Select a Menu to Update.");
                }
            }

            GUILine();

            //Loading content needed to make menu
            _buttonPrefabs = Resources.LoadAll<GameObject>("ButtonPrefabs/" + _buttonStyle);
            _radialMenuPrefab = Resources.Load<GameObject>("MenuPrefabs/RadialMenu");
            _defaultClickSound = Resources.Load<AudioClip>("Sound/DefaultButtonPress");


            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);

            // General Button Options
            EditorGUILayout.LabelField("General Button Options", _headerStyle);
            if(_buildOption == e_buildOptions.BuildNewMenu)
            {
                _menuName = EditorGUILayout.TextField("Menu Name", _menuName, _layoutOptions);
            }
            
            _buttonStyle = (e_buttonStyles)EditorGUILayout.EnumPopup("Button Style", _buttonStyle, _layoutOptions);
            _numberOfButtons = EditorGUILayout.IntSlider("Number of Buttons", _numberOfButtons, 2, 8, _layoutOptions);

            GUILine();

            // Label Options
            EditorGUILayout.LabelField("Label Options", _headerStyle);
            _labelDisplay = (e_labelDisplay)EditorGUILayout.EnumPopup("Button Label Mode", _labelDisplay, _layoutOptions);
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
            // _buttonHighlightMat.color = _sharedHighlightColor;

            GUILine();

            //Control Options
            EditorGUILayout.LabelField("Control Options", _headerStyle);
            _selectionButton = (e_selectionButton)EditorGUILayout.EnumPopup("Confirm Selection Button", _selectionButton, _layoutOptions);
            _menuToggle = EditorGUILayout.Toggle("Menu Visibility Toggle", _menuToggle);
            if(_menuToggle)
            {
                _menuToggleButton = (e_toggleButton)EditorGUILayout.EnumPopup("Menu Toggle Button", _menuToggleButton, _layoutOptions);
            }

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
            _hapticHand = (e_hapticHand)EditorGUILayout.EnumPopup("Haptic Controller", _hapticHand, _layoutOptions);
            if (_hapticHand != e_hapticHand.NoHaptics)
            {
                _hapticIntensity = (e_hapticIntensity)EditorGUILayout.EnumPopup("Haptic Intensity", _hapticIntensity, _layoutOptions);
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
            if (
                _buttonStyle != e_buttonStyles.ChooseStyle && 
                _selectionButton.ToString() != _menuToggleButton.ToString() &&
                _buildOption == e_buildOptions.BuildNewMenu && 
                GUILayout.Button("Make Menu", _layoutOptions))
            {
                try{
                    if(ValidateBuild(_menuName, _menusInScene, out _buttonMats, _numberOfButtons))
                    {
                        MenuMaker.MakeMenu(
                            _menuName,
                            _radialMenuPrefab,
                            _buttonHighlightMat,
                            _buttonPrefabs[_numberOfButtons - 2],
                            _buttonStyle,
                            _buttonMats,
                            _numberOfButtons, 
                            _labelDisplay,  
                            (Font)_labelFont,
                            _hapticHand,
                            _hapticIntensity,
                            _selectionButton,
                            _handAttachPoint,
                            _buttonsMatch,
                            _sharedButtonColor,
                            _buttonColors,
                            _buttonLabels,
                            _buttonIcons,
                            _playSoundOnClick,
                            _onClickSound,
                            _menuToggle,
                            _menuToggleButton
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("VRMM: " + e.Message);
                }
            }
            else if (
                _buildOption == e_buildOptions.UpdateExistingMenu &&
                _selectionButton.ToString() != _menuToggleButton.ToString() &&
                GUILayout.Button("Update Menu", _layoutOptions))
            {
                try{
                    if(ValidateUpdate(out _buttonMats, _numberOfButtons, _menusInScene, _menuUpdateOptionsIndex))
                    {
                        MenuMaker.UpdateMenu(
                            _menusInScene[_menuUpdateOptionsIndex],
                            _sharedHighlightColor,
                            _buttonPrefabs[_numberOfButtons - 2],
                            _buttonStyle,
                            _buttonMats,
                            _numberOfButtons, 
                            _labelDisplay,
                            (Font)_labelFont,  
                            _hapticHand,
                            _hapticIntensity,
                            _selectionButton,
                            _handAttachPoint,
                            _buttonsMatch,
                            _sharedButtonColor,
                            _buttonColors,
                            _buttonLabels,
                            _buttonIcons,
                            _playSoundOnClick,
                            _onClickSound,
                            _menuToggle,
                            _menuToggleButton
                        );
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("VRMM: " + e.Message);
                }
            }
            else if(
                _buttonStyle == e_buttonStyles.ChooseStyle && 
                _selectionButton.ToString() != _menuToggleButton.ToString() && 
                _buildOption != e_buildOptions.UpdateExistingMenu && 
                GUILayout.Button("Make Menu", _layoutOptions))
            {
                Debug.LogWarning("VRMM: Please select a button style to create your menu!");
            }
            else if(
                _menuToggle && 
                _selectionButton.ToString() == _menuToggleButton.ToString() && 
                _buildOption != e_buildOptions.UpdateExistingMenu && 
                GUILayout.Button("Make Menu", _layoutOptions))
            {
                Debug.LogWarning("VRMM: Please choose different buttons for Confirm Selection and Menu Toggle");
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

        // This functino takes in a RadialMenu present in the scene and reads its properties,
        // then uses these properties to populate the window with that menu's current settings
        void DisplayMenuToUpdate(RadialMenu menu)
        {
            var menuButtons = menu.GetComponentsInChildren<RadialButton>();
            var menuCursor = menu.GetComponentInChildren<MenuCursor>();
            var menuToggle = menu.GetComponent<MenuToggle>();

            _menuName = menu.name;
            _buttonStyle = menu.buttonStyle;
            _numberOfButtons = menuButtons.Length;
            _labelDisplay = menuCursor.labelDisplayOption;
            
            // If all button colors match
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

            _selectionButton = menuCursor.selectionButton;
            _menuToggle = menuToggle.toggle;
            _menuToggleButton = menuToggle.toggleButton;

            _handAttachPoint = menu.GetComponent<AttachToAnchor>().attachPoint;
            _playSoundOnClick = menuCursor.playSound;
            if(_playSoundOnClick)
            {
                _onClickSound = menuCursor.clickAudio;
            }
            _hapticHand = menuCursor.hapticHandOption;
            if(_hapticHand != e_hapticHand.NoHaptics)
            {
                _hapticIntensity = menuCursor.hapticIntensityOption;
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

            if(!AssetDatabase.IsValidFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menuName)))
            {
                AssetDatabase.CreateFolder("Assets/VRMM/Resources/Materials", _menuName);
                AssetDatabase.CreateFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menuName), "Buttons");
                AssetDatabase.CreateFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menuName), "Highlight");
            }
            else
            {
                var materials = Resources.LoadAll(String.Format("Materials/{0}", _menuName));
                foreach(UnityObject material in materials)
                {
                    var path = AssetDatabase.GetAssetPath(material);
                    AssetDatabase.DeleteAsset(path);
                }
                var highlight = Resources.Load(String.Format("Materials/{0}/Highlight/Button_Highlight_Mat", _menuName));
                var highlightPath = AssetDatabase.GetAssetPath(highlight);
                AssetDatabase.DeleteAsset(highlightPath);
            }

            var highlightMat = new Material(Shader.Find("Standard"));
            AssetDatabase.CreateAsset(highlightMat, String.Format("Assets/VRMM/Resources/Materials/{0}/Highlight/Button_Highlight_Mat.mat", _menuName));
            _buttonHighlightMat = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Highlight/Button_Highlight_Mat.mat", _menuName));
            _buttonHighlightMat.color = _sharedHighlightColor;

            _buttonMats = new Material[_numberOfButtons];
            for(var i = 0; i < _numberOfButtons; i++)
            {
                var mat = new Material(Shader.Find("Standard"));
                
                AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
                _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
            }
            return true;
        }

        private bool ValidateUpdate(out Material[] _buttonMats, int _numberOfButtons, List<RadialMenu> _menusInScene, int _menuUpdateOptionsIndex)
        {
            _buttonMats = new Material[_numberOfButtons];
            if(AssetDatabase.IsValidFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menusInScene[_menuUpdateOptionsIndex].name)))
            {
                var materials = Resources.LoadAll<Material>(String.Format("Materials/{0}/Buttons", _menusInScene[_menuUpdateOptionsIndex].name));

                foreach(Material material in materials)
                {
                    var path = AssetDatabase.GetAssetPath(material);
                    AssetDatabase.DeleteAsset(path);
                }
                var mats = Resources.LoadAll<Material>(String.Format("Materials/{0}/Buttons", _menusInScene[_menuUpdateOptionsIndex].name));

                for(var i = 0; i < _numberOfButtons - mats.Length; i++)
                {
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
                }
                for(var i = mats.Length; i < _numberOfButtons; i++)
                {
                    var mat = new Material(Shader.Find("Standard"));
                    
                    AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menusInScene[_menuUpdateOptionsIndex].name, i));
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
                }
            }
            else
            {
                AssetDatabase.CreateFolder("Assets/VRMM/Resources/Materials", _menuName);
                AssetDatabase.CreateFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menuName), "Buttons");
                AssetDatabase.CreateFolder(String.Format("Assets/VRMM/Resources/Materials/{0}", _menuName), "Highlight");

                var highlightMat = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(highlightMat, String.Format("Assets/VRMM/Resources/Materials/{0}/Highlight/Button_Highlight_Mat.mat", _menuName));
                _buttonHighlightMat = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Highlight/Button_Highlight_Mat.mat", _menuName));
                _buttonHighlightMat.color = _sharedHighlightColor;
                
                for(var i = 0; i < _numberOfButtons; i++)
                {
                    var mat = new Material(Shader.Find("Standard"));
                    
                    AssetDatabase.CreateAsset(mat, String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
                    _buttonMats[i] = AssetDatabase.LoadAssetAtPath<Material>(String.Format("Assets/VRMM/Resources/Materials/{0}/Buttons/Button_{1}_Mat.mat", _menuName, i));
                }
            }
            return true;
        }
    }

}