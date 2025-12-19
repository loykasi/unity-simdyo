using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private SceneEntity _sceneEntity;
    
    public SceneEntity Get()
    {
        return _sceneEntity;
    }
}