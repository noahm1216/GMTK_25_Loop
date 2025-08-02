using UnityEngine;

public class ShaderGlobalMousePos : MonoBehaviour
{
    private static readonly int MousePosition = Shader.PropertyToID("_MousePos");

    void Update()
    {
        Shader.SetGlobalVector(MousePosition, Input.mousePosition);
    }
}
