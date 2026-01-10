using UnityEngine;

public class ThumbnailRender : Singleton<ThumbnailRender>
{
    [SerializeField] private Camera _thumbnailCamera;
    [SerializeField] private RenderTexture _thumbnailRenderTexture;

    private void Update()
    {
        if (_thumbnailCamera.gameObject.activeInHierarchy)
        {
            Camera editorCamera = EngineManager.Instance.EditorCamera;
            _thumbnailCamera.transform.position = editorCamera.transform.position;
            _thumbnailCamera.orthographicSize = editorCamera.orthographicSize;
            _thumbnailCamera.backgroundColor = editorCamera.backgroundColor;
        }
    }

    public byte[] GetThumbnail()
    {
        int width = _thumbnailRenderTexture.width;
        int height = _thumbnailRenderTexture.height;
        Texture2D texture = new(width, height, TextureFormat.RGBA32, false, false);
        RenderTexture.active = _thumbnailRenderTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        byte[] bytes = texture.EncodeToPNG();
        RenderTexture.active = null;

        if (Application.isPlaying)
        {
            Destroy(texture);
        }
        else
        {
            DestroyImmediate(texture);
        }

        return bytes;
    }

    public void EnablePreview()
    {
        _thumbnailCamera.gameObject.SetActive(true);
    }

    public void DisablePreview()
    {
        _thumbnailCamera.gameObject.SetActive(false);
    }
}