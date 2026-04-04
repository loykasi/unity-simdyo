using UnityEngine;

public struct ColorHSV
{
    public float H;
    public float S;
    public float V;
    public float A;

    public ColorHSV(float h, float s, float v, float a)
    {
        H = h;
        S = s;
        V = v;
        A = a;
    }

    public ColorHSV(Color color)
    {
        Color.RGBToHSV(color, out H, out S, out V);
        A = color.a;
    }


    public readonly Color ToUnityColor()
    {
        Color color = Color.HSVToRGB(H, S, V);
        color.a = A;
        return color;
    }

    public static ColorHSV Default => new(0, 0, 1, 1);
}