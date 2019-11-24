using UnityEngine;

public class TestCube : MonoBehaviour
{
    private Material _mat;

    private void Start() {
        _mat = GetComponent<Renderer>().sharedMaterial;
        Debug.Log(Input.GetJoystickNames()[0]);
        Debug.Log(Input.GetJoystickNames()[1]);
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
