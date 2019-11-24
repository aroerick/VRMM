using UnityEngine;
using VRMM;

namespace VRMM {

    public class RadialMenu : MonoBehaviour
    {
        private float cursorMod = .08f;

        private MenuCursor cursor;

        private float horizontalInput;
        private float verticalInput;

        private void Start()
        {
            cursor = GetComponentInChildren<MenuCursor>();
        }

        private void Update()
        {
            horizontalInput = Input.GetAxis("VRMM_Horizontal_Left");
            verticalInput = Input.GetAxis("VRMM_Vertical_Left");

            cursor.transform.localPosition = new Vector3(horizontalInput * cursorMod, 0, verticalInput * cursorMod);
        }
}
}