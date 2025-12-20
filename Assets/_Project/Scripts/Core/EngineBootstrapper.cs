using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class EngineBootstrapper : MonoBehaviour
{
    [SerializeField] private int _engineSceneIndex;

    IEnumerator Start()
    {
        DateTime a = DateTime.Now;

        yield return LocalizationSettings.InitializationOperation;
        
        DateTime b = DateTime.Now;
        double diffInSeconds = (b - a).TotalSeconds;
        Debug.Log($"Initialization time: {diffInSeconds}");

        SceneLoader.Instance.LoadScene(_engineSceneIndex);
    }
}