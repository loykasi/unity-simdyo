using UnityEngine;

public abstract class BaseEntityMenu : MonoBehaviour
{
    [SerializeField] protected GameObject _menuGameObject;

    public abstract void Setup();
    public abstract void UpdateUI();
    public abstract void AddEntity(SceneEntity entity);
    public abstract void Clear();
    public abstract void Show();
    public abstract void Close();
}