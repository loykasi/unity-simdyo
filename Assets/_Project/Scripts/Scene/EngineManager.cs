using System.Collections.Generic;
using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public GameObject SelectedObject;

    public List<VisualScripting> _visualScriptings;

    [SerializeField] private float _selectRadius;

    private bool _isRunning = false;

    public void Select(Vector3 worldPoint)
    {
        worldPoint.z = 0;

        Collider2D collider = Physics2D.OverlapCircle(worldPoint, _selectRadius);

        if (collider == null)
        {
            SelectedObject = null;
            return;
        }

        Debug.Log(collider);
        SelectedObject = collider.gameObject;

        VisualScripting vs = SelectedObject.GetComponent<VisualScripting>();
        NodeBoard.Instance.SetVisualScripting(vs);
    }

    private void Update()
    {
        UpdateGame();   
    }

    private void StartGame()
    {
        for (int i = 0; i < _visualScriptings.Count; i++)
        {
            _visualScriptings[i].StartVS();
        }

        _isRunning = true;
    }

    private void UpdateGame()
    {
        if (!_isRunning)
        {
            return;
        }

        for (int i = 0; i < _visualScriptings.Count; i++)
        {
            _visualScriptings[i].UpdateVS();
        }
    }

    private void OnGUI()
    {
        if (SelectedObject != null)
        {
            GUILayout.Label(SelectedObject.name);
        }

        if (GUI.Button(new Rect(1810, 10, 100, 50), "Run"))
        {
            StartGame();
        }

        if (GUI.Button(new Rect(1700, 10, 100, 50), "Stop"))
        {
            _isRunning = false;
        }
    }
}