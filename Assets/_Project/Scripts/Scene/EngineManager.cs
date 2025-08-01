using System.Collections.Generic;
using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    private bool _isRunning = false;

    private void Update()
    {
        UpdateGame();   
    }

    public void Play()
    {
        StartGame();
    }

    public void Stop()
    {
        _isRunning = false;

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnSceneStop();
        }
    }

    private void StartGame()
    {
        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].OnSceneStart();
            entities[i].VisualScripting.StartVS();
        }

        _isRunning = true;
    }

    private void UpdateGame()
    {
        if (!_isRunning)
        {
            return;
        }

        var entities = ObjectManager.Instance.SceneEntities;
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].VisualScripting.UpdateVS();
        }
    }
}