using TMPro;
using UnityEngine;

public class LogCommand : Singleton<LogCommand>
{
    [SerializeField] private TMP_Text _textBox;

    public void Log(object message)
    {
        _textBox.SetText(_textBox.text + "\n" + message.ToString());
    }
}