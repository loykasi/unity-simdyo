using UnityEngine;

public class GameCommunication : MonoBehaviour
{
    public void Pause()
    {
        SceneManager.Instance.Pause();
    }

    public void Resume()
    {
        SceneManager.Instance.Resume();
    }

    public void Restart()
    {
        SceneManager.Instance.Restart();
    }
}