using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRMM {

    [InitializeOnLoad]
    class MenuBuilderInputManager
    {
        static MenuBuilderInputManager()
        {
            EditorApplication.delayCall += OnDelayCall;
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
            NewInputAxis(new Axis() { name = "VRMM_Horizontal_Left",  dead = 0.1f, axis = 0 });
            NewInputAxis(new Axis() { name = "VRMM_Vertical_Left",    dead = 0.1f, axis = 1 });
            NewInputAxis(new Axis() { name = "VRMM_Horizontal_Right", dead = 0.1f, axis = 3 });
            NewInputAxis(new Axis() { name = "VRMM_Vertical_Right",   dead = 0.1f, axis = 4 });
            NewInputAxis(new Axis() { name = "VRMM_Trigger_Left",     dead = 0.1f, axis = 8 });
            NewInputAxis(new Axis() { name = "VRMM_Trigger_Right",    dead = 0.1f, axis = 9 });
            NewInputAxis(new Axis() { name = "VRMM_Grip_Left",        dead = 0.1f, axis = 10 });
            NewInputAxis(new Axis() { name = "VRMM_Grip_Right",       dead = 0.1f, axis = 11 });
            NewInputAxis(new Axis() { name = "VRMM_TriggerPress_Left",  type = 0, axis = 3 });
            NewInputAxis(new Axis() { name = "VRMM_TriggerPress_Right", type = 0, axis = 4 });
            NewInputAxis(new Axis() { name = "VRMM_ThumbPress_Left",    type = 0, axis = 7 });
            NewInputAxis(new Axis() { name = "VRMM_ThumbPress_Right",   type = 0, axis = 8 });
            NewInputAxis(new Axis() { name = "VRMM_GripPress_Left",     type = 0, axis = 13 });
            NewInputAxis(new Axis() { name = "VRMM_GripPress_Right",    type = 0, axis = 14 });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_A",     type = 0, axis = -1 });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_B",     type = 0, axis = 0 });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_X",     type = 0, axis = 1 });
            NewInputAxis(new Axis() { name = "VRMM_OculusButton_Y",     type = 0, axis = 2 });
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
            public int type = 2;
            public int axis = 0;
            public int joyNum = 0;
        }

        private static void OnDelayCall()
        {
            AddInputBindings();
        }
    }
}