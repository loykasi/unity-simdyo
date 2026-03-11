using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private EntityMenu _meshEntityMenu;
    [SerializeField] private SceneMenu _sceneMenu;

    private void OnEnable()
    {
        ObjectManager.Instance.OnObjectSelected += OnObjectSelected;
        ObjectManager.Instance.OnObjectDeselected += OnObjectDeselected;
    }

    private void OnDisable()
    {
        if (ObjectManager.Instance)
        {
            ObjectManager.Instance.OnObjectSelected -= OnObjectSelected;
            ObjectManager.Instance.OnObjectDeselected -= OnObjectDeselected;
        }
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        _sceneMenu.Close();

        if (entity is MeshEntity meshEntity)
        {
            _meshEntityMenu.Open(meshEntity);
        }
    }

    private void OnObjectDeselected()
    {
        _meshEntityMenu.Close();
        _sceneMenu.Open();
    }

}