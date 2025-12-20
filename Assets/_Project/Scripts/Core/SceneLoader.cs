using System.Collections;
using UnityEngine;

public class SceneLoader : Singleton<SceneLoader>
{
    public void LoadScene(int index)
    {
        StartCoroutine(LoadingLevel(index));
    }

    private IEnumerator LoadingLevel(int index)
    {
        LoadingScreen.Instance.Toggle(true);

        AsyncOperation loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);

        while (!loadingOperation.isDone)
        {
            // float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            yield return null;
        }

        LoadingScreen.Instance.Toggle(false);
    }
}