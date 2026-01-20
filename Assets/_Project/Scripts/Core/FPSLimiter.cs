using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [SerializeField] private int _frameRate = 60;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = _frameRate;
    }
}