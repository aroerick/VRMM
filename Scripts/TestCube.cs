// VR Menu Maker V1.3
// Created by Adam Roerick
//
// VRMM is a tool I've created to help empower content creation for VR
//
// Just a cube with some pre-built functions to test your menus

using UnityEngine;

public class TestCube : MonoBehaviour
{
    private Material _mat;

    private void Start() {
        _mat = GetComponent<Renderer>().sharedMaterial;
    }
    public void GrowCube(){
        transform.localScale *= 1.05f;
    }
    public void ShrinkCube(){
        transform.localScale /= 1.05f;
    }
    public void ColorCube(){
        _mat.color = Color.red;
    }
    public void DeleteCube(){
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        _mat.color = Color.white;
    }
}
