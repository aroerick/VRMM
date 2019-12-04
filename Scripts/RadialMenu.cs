// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// This defines the radial menu object and takes user input to move cursor

using UnityEngine;

namespace VRMM {

    public class RadialMenu : MonoBehaviour
    {
        [HideInInspector]
        public e_buttonStyles buttonStyle;

        private float cursorMod = .08f;
        private MenuCursor cursor;
        private float horizontalInput;
        private float verticalInput;

        private void Start()
        {
            cursor = GetComponentInChildren<MenuCursor>();
        }

        // Get input of thumb and map it to the menu cursor
        private void Update()
        {
            horizontalInput = Input.GetAxis("VRMM_Horizontal_Left");
            verticalInput = Input.GetAxis("VRMM_Vertical_Left");

            cursor.transform.localPosition = new Vector3(horizontalInput * cursorMod, 0, verticalInput * cursorMod);
        }
}
}