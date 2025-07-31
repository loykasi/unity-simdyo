using System.Collections.Generic;
using UnityEngine;

public class EngineManager : Singleton<EngineManager>
{
    public List<VisualScripting> _visualScriptings;

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
}