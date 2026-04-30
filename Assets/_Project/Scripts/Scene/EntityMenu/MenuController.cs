using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private EntityMenu _entityMenu;
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

    private void Awake()
    {
        _sceneMenu.Setup();
        _entityMenu.Setup();
    }

    private void Start()
    {
        _entityMenu.Close();
        _sceneMenu.Open();
    }

    private void OnObjectSelected(SceneEntity entity)
    {
        _sceneMenu.Close();
        _entityMenu.Open(entity);
    }

    private void OnObjectDeselected()
    {
        _entityMenu.Close();
        _sceneMenu.Open();
    }

}