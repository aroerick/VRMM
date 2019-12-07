// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// Simple script used to attach the menu to the selected anchor

using UnityEngine;

namespace VRMM {

    public class AttachToAnchor : MonoBehaviour
    {
        public GameObject attachPoint;
        private bool _isAttached;

        private void Update()
        {
            if (!_isAttached && attachPoint != null)
            {
                Attach();
            }
        }

        // Make this a child of the given attachPoint object
        private void Attach()
        {
            transform.position = Vector3.zero;
            transform.SetParent(attachPoint.transform, false);
            _isAttached = true;
        }
    }
}
