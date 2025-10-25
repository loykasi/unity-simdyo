using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayTab : MonoBehaviour
{
    [SerializeField] private Image _playButtonImage;
    [SerializeField] private Sprite _playSprite;
    [SerializeField] private Sprite _stopSprite;

    private bool _isRunning;

    public void PlayToggle()
    {
        _isRunning = !_isRunning;
        if (_isRunning)
        {
            SceneManager.Instance.Play();

            _playButtonImage.sprite = _stopSprite;
        }
        else
        {
            SceneManager.Instance.Stop();

            _playButtonImage.sprite = _playSprite;
        }
    }


}