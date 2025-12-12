using System.Collections;
using TMPro;
using UnityEngine;

namespace Loykas.Scripting
{
    public class LogCommand : Singleton<LogCommand>
    {
        [SerializeField] private GameObject _console;
        [SerializeField] private TMP_Text _textBox;
        private string _log = string.Empty;

        public void Toggle()
        {
            _console.SetActive(!_console.activeSelf);
        }

        public void Log(object value)
        {
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
        }
    }
}