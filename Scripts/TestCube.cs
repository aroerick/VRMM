// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// Just a cube with some pre-built functions to test your menus

using UnityEngine;

namespace VRMM
{
    public class TestCube : MonoBehaviour
    {
        private Material _mat;

        private void Start() {
            _mat = GetComponent<Renderer>().sharedMaterial;
        }
        // Grow the cube
        public void GrowCube(){
            transform.localScale *= 1.05f;
        }

        // Shrink the cube
        public void ShrinkCube(){
            transform.localScale /= 1.05f;
        }

        // Make the cube red
        public void ColorCube(){
            _mat.color = Color.red;
        }

        // Delete the cube
        public void DeleteCube(){
            Destroy(gameObject);
        }

        // Reset cube color when destroyed
        private void OnDestroy()
        {
            _mat.color = Color.white;
        }
    }
}
