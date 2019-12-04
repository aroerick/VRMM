// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This script simply defines the buttons and adds unity events from ButtonEvents

using UnityEngine;
using UnityEngine.Events;

namespace VRMM{

   [System.Serializable]
   public class RadialButton : MonoBehaviour
   {
      [HideInInspector]public UnityEvent onButtonPress;
      
      // Takes the unity events passed to it from ButtonEvents and creates events for the button
      public void SetOnButtonPress(UnityEvent value) => onButtonPress = value;
}
}

