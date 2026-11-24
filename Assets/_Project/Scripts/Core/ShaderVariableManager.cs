using UnityEngine;

public class ShaderVariableManager : MonoBehaviour
{
    private int _unscaledTimeID = Shader.PropertyToID("_UnscaledTime");
    private int _borderWidthID = Shader.PropertyToID("_BorderWidth");

    private void Awake()
    {
        Shader.SetGlobalFloat(_borderWidthID, 0.1f);
    }

    private void Update()
    {
        Shader.SetGlobalFloat(_unscaledTimeID, Time.unscaledTime);
    }
}