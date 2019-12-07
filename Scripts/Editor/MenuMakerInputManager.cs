// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This class sets up the needed Unity inputs for the VR menu to function properly

using UnityEditor;

namespace VRMM.Editor {

    [InitializeOnLoad]
    internal class MenuMakerInputManager
    {   
        //Used to create custom input bindings when loaded
        static MenuMakerInputManager()
        {
            EditorApplication.delayCall += OnDelayCall;
        }

        private static void OnDelayCall()
        {
            AddInputBindings();
        }

        // Class to describe input bindings
        private class InputAxis
        {
            public string Name = string.Empty;
            public string DescriptiveName = string.Empty;
            public string DescriptiveNegativeName = string.Empty;
            public string NegativeButton = string.Empty;
            public string PositiveButton = string.Empty;
            public string AltNegativeButton = string.Empty;
            public string AltPositiveButton = string.Empty;
            public float Gravity;
            public float Dead = 0.001f;
            public float Sensitivity = 1.0f;
            public bool Snap = false;
            public bool Invert = false;
            public int Type;
            public int Axis;
            public int JoyNum = 0;
        }

        // Create new binding based on input
        private static void NewInputAxis(InputAxis inputAxis)
        {
            var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            var axesProperty = serializedObject.FindProperty("m_Axes");

            var axesCopy = axesProperty.Copy();
            axesCopy.Next(true);
            axesCopy.Next(true);
            while (axesCopy.Next(false))
            {
                if (axesCopy.FindPropertyRelative("m_Name").stringValue == inputAxis.Name)
                {
                    return;
                }
            }

            axesProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();

            var axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
            axisProperty.FindPropertyRelative("m_Name").stringValue = inputAxis.Name;
            axisProperty.FindPropertyRelative("descriptiveName").stringValue = inputAxis.DescriptiveName;
            axisProperty.FindPropertyRelative("descriptiveNegativeName").stringValue = inputAxis.DescriptiveNegativeName;
            axisProperty.FindPropertyRelative("negativeButton").stringValue = inputAxis.NegativeButton;
            axisProperty.FindPropertyRelative("positiveButton").stringValue = inputAxis.PositiveButton;
            axisProperty.FindPropertyRelative("altNegativeButton").stringValue = inputAxis.AltNegativeButton;
            axisProperty.FindPropertyRelative("altPositiveButton").stringValue = inputAxis.AltPositiveButton;
            axisProperty.FindPropertyRelative("gravity").floatValue = inputAxis.Gravity;
            axisProperty.FindPropertyRelative("dead").floatValue = inputAxis.Dead;
            axisProperty.FindPropertyRelative("sensitivity").floatValue = inputAxis.Sensitivity;
            axisProperty.FindPropertyRelative("snap").boolValue = inputAxis.Snap;
            axisProperty.FindPropertyRelative("invert").boolValue = inputAxis.Invert;
            axisProperty.FindPropertyRelative("type").intValue = inputAxis.Type;
            axisProperty.FindPropertyRelative("inputAxis").intValue = inputAxis.Axis;
            axisProperty.FindPropertyRelative("joyNum").intValue = inputAxis.JoyNum;
            serializedObject.ApplyModifiedProperties();
        }

        // Create all needed input bindings
        private static void AddInputBindings()
        {
            NewInputAxis(new InputAxis() { Name = "VRMM_Horizontal_Left",    Type = 2, Dead = 0.1f, Axis = 0 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Vertical_Left",      Type = 2, Dead = 0.1f, Axis = 1 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Horizontal_Right",   Type = 2, Dead = 0.1f, Axis = 3 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Vertical_Right",     Type = 2, Dead = 0.1f, Axis = 4 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Trigger_Left",       Type = 2, Dead = 0.1f, Axis = 8 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Trigger_Right",      Type = 2, Dead = 0.1f, Axis = 9 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Grip_Left",          Type = 2, Dead = 0.1f, Axis = 10 });
            NewInputAxis(new InputAxis() { Name = "VRMM_Grip_Right",         Type = 2, Dead = 0.1f, Axis = 11 });
            NewInputAxis(new InputAxis() { Name = "VRMM_OculusButton_A",     Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 0" });
            NewInputAxis(new InputAxis() { Name = "VRMM_OculusButton_B",     Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 1" });
            NewInputAxis(new InputAxis() { Name = "VRMM_OculusButton_X",     Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 2" });
            NewInputAxis(new InputAxis() { Name = "VRMM_OculusButton_Y",     Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 3" });
            NewInputAxis(new InputAxis() { Name = "VRMM_GripPress_Left",     Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 4" });
            NewInputAxis(new InputAxis() { Name = "VRMM_GripPress_Right",    Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 5" });
            NewInputAxis(new InputAxis() { Name = "VRMM_ThumbPress_Left",    Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 8" });
            NewInputAxis(new InputAxis() { Name = "VRMM_ThumbPress_Right",   Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 9" });
            NewInputAxis(new InputAxis() { Name = "VRMM_TriggerPress_Left",  Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 14" });
            NewInputAxis(new InputAxis() { Name = "VRMM_TriggerPress_Right", Gravity = 1000, Sensitivity = 1000, PositiveButton = "joystick button 15" });
        }
    }
}