// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// Just defining enums for use across Menu Maker

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRMM {
    
    public enum e_buttonStyles {
        ChooseStyle,
        Standard,
        LowPoly
    }
    public enum e_buildOptions {
        BuildNewMenu,
        UpdateExistingMenu
    }    
    public enum e_labelDisplay {
        ToggleOnHover,
        AlwaysShow,
        NoLabels
    }
    public enum e_selectionButton {
        LeftTrigger,
        RightTrigger,
        LeftThumbPress,
        RightThumbPress,
        AButtonOculusOnly,
        BButtonOculusOnly,
        XButtonOculusOnly,
        YButtonOculusOnly
    }
    public enum e_toggleButton {
        LeftThumbAxis,
        RightThumbAxis,
        LeftTrigger,
        RightTrigger,
        LeftThumbPress,
        RightThumbPress,
        AButtonOculusOnly,
        BButtonOculusOnly,
        XButtonOculusOnly,
        YButtonOculusOnly
    }
    public enum e_hapticHand {
        NoHaptics,
        LeftController,
        RightController
    }
    public enum e_hapticIntensity
	{
		Light,
		Medium,
		Strong,
	}
}
