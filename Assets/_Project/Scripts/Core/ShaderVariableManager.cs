using UnityEngine;

public class ShaderVariableManager : MonoBehaviour
{
    private int _unscaledTimeID = Shader.PropertyToID("_UnscaledTime");

    private void Update()
    {
        Shader.SetGlobalFloat(_unscaledTimeID, Time.unscaledTime);
    }
}