using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private EntityMenu _meshEntityMenu;
    [SerializeField] private TracerMenu _tracerEntityMenu;
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

    private void Start()
    {
        _sceneMenu.Open();
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        _sceneMenu.Close();

        switch (entity)
        {
            case MeshEntity meshEntity:
                _meshEntityMenu.Open(meshEntity);
                break;
            case TracerEntity tracerEntity:
                _tracerEntityMenu.Open(tracerEntity);
                break;
        }
    }

    private void OnObjectDeselected()
    {
        _meshEntityMenu.Close();
        _tracerEntityMenu.Close();
        _sceneMenu.Open();
    }

}