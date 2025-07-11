using UnityEngine;

public class ScreenToCenter
{
    public static Vector2 GetPostionFromCenter(Vector2 position)
    {
        float x = position.x - Screen.width / 2.0f;
        float y = position.y - Screen.height / 2.0f;
        return new Vector2(x, y);
    }
}