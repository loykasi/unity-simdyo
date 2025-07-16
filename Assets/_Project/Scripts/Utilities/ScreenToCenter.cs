using UnityEngine;

public class ScreenToCenter
{
    public static Vector3 GetPostionFromCenter(Vector3 position)
    {
        float x = position.x - Screen.width / 2.0f;
        float y = position.y - Screen.height / 2.0f;
        return new Vector3(x, y);
    }
}