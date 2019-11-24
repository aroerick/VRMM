using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRMM {

    public class MenuToggle : MonoBehaviour
    {
        private bool isEnabled;
        private RadialButtonContainer buttons;

        private void Start() 
        {
            buttons = GetComponentInChildren<RadialButtonContainer>(true);
        }

        void Update()
        {
            if (Input.GetButtonDown("Oculus_CrossPlatform_Button2"))
            {
                buttons.gameObject.SetActive(isEnabled);
                isEnabled = !isEnabled;
            }
        }
    }
}
