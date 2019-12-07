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
        public EButtonStyles buttonStyle;

        private const float CursorMod = .08f;
        private MenuCursor _cursor;
        private float _horizontalInput;
        private float _verticalInput;

        private void Start()
        {
            _cursor = GetComponentInChildren<MenuCursor>();
        }

        // Get input of thumb and map it to the menu cursor
        private void Update()
        {
            _horizontalInput = Input.GetAxis("VRMM_Horizontal_Left");
            _verticalInput = Input.GetAxis("VRMM_Vertical_Left");

            _cursor.transform.localPosition = new Vector3(_horizontalInput * CursorMod, 0, _verticalInput * CursorMod);
        }
}
}