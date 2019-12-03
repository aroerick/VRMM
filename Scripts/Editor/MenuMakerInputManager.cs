using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRMM {

    [InitializeOnLoad]
    class MenuMakerInputManager
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

        private class Axis
        {
            public string name = String.Empty;
            public string descriptiveName = String.Empty;
            public string descriptiveNegativeName = String.Empty;
            public string negativeButton = String.Empty;
            public string positiveButton = String.Empty;
            public string altNegativeButton = String.Empty;
            public string altPositiveButton = String.Empty;
            public float gravity = 0.0f;
            public float dead = 0.001f;
            public float sensitivity = 1.0f;
            public bool snap = false;
            public bool invert = false;
            public int type = 0;
            public int axis = 0;
            public int joyNum = 0;
        }

        private static void NewInputAxis(Axis axis)
        {
            SerializedObject serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
            SerializedProperty axesProperty = serializedObject.FindProperty("m_Axes");

            SerializedProperty axesCopy = axesProperty.Copy();
            axesCopy.Next(true);
            axesCopy.Next(true);
            while (axesCopy.Next(false))
            {
                if (axesCopy.FindPropertyRelative("m_Name").stringValue == axis.name)
                {
                    return;
                }
            }

            axesProperty.arraySize++;
            serializedObject.ApplyModifiedProperties();

            SerializedProperty axisProperty = axesProperty.GetArrayElementAtIndex(axesProperty.arraySize - 1);
            axisProperty.FindPropertyRelative("m_Name").stringValue = axis.name;
            axisProperty.FindPropertyRelative("descriptiveName").stringValue = axis.descriptiveName;
            axisProperty.FindPropertyRelative("descriptiveNegativeName").stringValue = axis.descriptiveNegativeName;
            axisProperty.FindPropertyRelative("negativeButton").stringValue = axis.negativeButton;
            axisProperty.FindPropertyRelative("positiveButton").stringValue = axis.positiveButton;
            axisProperty.FindPropertyRelative("altNegativeButton").stringValue = axis.altNegativeButton;
            axisProperty.FindPropertyRelative("altPositiveButton").stringValue = axis.altPositiveButton;
            axisProperty.FindPropertyRelative("gravity").floatValue = axis.gravity;
            axisProperty.FindPropertyRelative("dead").floatValue = axis.dead;
            axisProperty.FindPropertyRelative("sensitivity").floatValue = axis.sensitivity;
            axisProperty.FindPropertyRelative("snap").boolValue = axis.snap;
            axisProperty.FindPropertyRelative("invert").boolValue = axis.invert;
            axisProperty.FindPropertyRelative("type").intValue = axis.type;
            axisProperty.FindPropertyRelative("axis").intValue = axis.axis;
            axisProperty.FindPropertyRelative("joyNum").intValue = axis.joyNum;
            serializedObject.ApplyModifiedProperties();
        }

        private static void AddInputBindings()
        {
            NewInputAxis(new Axis() { name = "VRMM_Horizontal_Left",    type = 2, dead = 0.1f, axis = 0 });
            NewInputAxis(new Axis() { name = "VRMM_Vertical_Left",      type = 2, dead = 0.1f, axis = 1 });
            NewInputAxis(new Axis() { name = "VRMM_Horizontal_Right",   type = 2, dead = 0.1f, axis = 3 });
            NewInputAxis(new Axis() { name = "VRMM_Vertical_Right",     type = 2, dead = 0.1f, axis = 4 });
            NewInputAxis(new Axis() { name = "VRMM_Trigger_Left",       type = 2, dead = 0.1f, axis = 8 });
            NewInputAxis(new Axis() { name = "VRMM_Trigger_Right",      type = 2, dead = 0.1f, axis = 9 });
            NewInputAxis(new Axis() { name = "VRMM_Grip_Left",          type = 2, dead = 0.1f, axis = 10 });
            NewInputAxis(new Axis() { name = "VRMM_Grip_Right",         type = 2, dead = 0.1f, axis = 11 });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_A",     gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 0" });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_B",     gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 1" });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_X",     gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 2" });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_Y",     gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 3" });
            NewInputAxis(new Axis() { name = "VRMM_GripPress_Left",     gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 4" });
            NewInputAxis(new Axis() { name = "VRMM_GripPress_Right",    gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 5" });
            NewInputAxis(new Axis() { name = "VRMM_ThumbPress_Left",    gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 8" });
            NewInputAxis(new Axis() { name = "VRMM_ThumbPress_Right",   gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 9" });
            NewInputAxis(new Axis() { name = "VRMM_TriggerPress_Left",  gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 14" });
            NewInputAxis(new Axis() { name = "VRMM_TriggerPress_Right", gravity = 1000, sensitivity = 1000, positiveButton = "joystick button 15" });
        }
    }
}