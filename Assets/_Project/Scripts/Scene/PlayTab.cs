using TMPro;
using UnityEngine;

public class PlayTab : MonoBehaviour
{
    [SerializeField] private TMP_Text _playButtonTextField;

    private bool _isRunning;

    public void PlayToggle()
    {
        _isRunning = !_isRunning;
        // if (_isRunning)
        // {
        //     EngineManager.Instance.Play();

        //     _playButtonTextField.text = "Stop";
        // }
        // else
        // {
        //     EngineManager.Instance.Stop();

        //     _playButtonTextField.text = "Play";
        // }
    }


}