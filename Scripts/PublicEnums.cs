// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// Just defining enums for use across Menu Maker

namespace VRMM {
    
    public enum EButtonStyles {
        ChooseStyle,
        Standard,
        LowPoly
    }
    public enum EBuildOptions {
        BuildNewMenu,
        UpdateExistingMenu
    }    
    public enum ELabelDisplay {
        ToggleOnHover,
        AlwaysShow,
        NoLabels
    }
    public enum ESelectionButton {
        LeftTrigger,
        RightTrigger,
        LeftThumbPress,
        RightThumbPress,
        AButtonOculusOnly,
        BButtonOculusOnly,
        XButtonOculusOnly,
        YButtonOculusOnly
    }
    public enum EToggleButton {
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
    public enum EHapticHand {
        NoHaptics,
        LeftController,
        RightController
    }
    public enum EHapticIntensity
	{
		Light,
		Medium,
		Strong,
	}
}
