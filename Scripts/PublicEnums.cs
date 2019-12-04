using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        [Description("Build New Menu")]
        BuildNewMenu,
        UpdateExistingMenu
    }    
    public enum e_labelDisplay {
        TaggleOnHover,
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
