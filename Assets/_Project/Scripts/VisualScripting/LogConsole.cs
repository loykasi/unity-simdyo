using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Loykas.Scripting
{
    public class LogConsole : Singleton<LogConsole>
    {
        [SerializeField] private GameObject _console;
        [SerializeField] private TMP_Text _textBox;
        [SerializeField] private ScrollRect _scrollRect;
        private string _log = string.Empty;

        private WaitForEndOfFrame _waitForEndOfFrame = new();

        private void OnEnable()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.OnSceneStart += OnSceneStart;
            }
        }

        private void OnDisable()
        {
            if (SceneManager.Instance != null)
            {
                SceneManager.Instance.OnSceneStart -= OnSceneStart;
            }
        }

        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Log($"GG_{Time.frameCount}");
            }
        }

        private void OnSceneStart()
        {
            if (_console == null)
            {
                return;
            }
            
            _log = string.Empty;
            _textBox.SetText(_log);
        }

        public void Toggle()
        {
            if (_console == null)
            {
                return;
            }

            _console.SetActive(!_console.activeSelf);
        }

        public void Log(object value)
        {
            if (_console == null)
            {
                return;
            }

            string message;
            if (value is IList list)
            {
                string listValue = "";
                for (int i = 0; i < list.Count; i++)
                {
                    listValue += list[i].ToString();
                    if (i < list.Count -1)
                    {
                        listValue += ", ";
                    }
                }

                message = $"[{listValue}]";
                Debug.Log($"{Time.frameCount} | {message}");
            }
            else
            {
                message = value.ToString();
                Debug.Log($"{Time.frameCount} | {value}");
            }

            _log += message + "\n";
            _textBox.SetText(_log);
            
            StartCoroutine(UpdateScrollRect());
        }

        private IEnumerator UpdateScrollRect()
        {
            yield return _waitForEndOfFrame;
            _scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}