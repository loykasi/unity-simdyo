void Fwidth_float(float In, out float Out)
{
    Out = fwidth(In);
}

void CircleSDF_float(float2 Point, float Radius, out float Distance)
{
    Distance = length(Point) - Radius;
}

void BoxSDF_float(float2 P, float2 Size, out float Distance)
{
    float2 d = abs(P) - Size;
    Distance = length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
}

void InlineSDF_float(float Distance, float Thickness, out float Out)
{
    Out = max(Distance, -Distance - Thickness);
}

float2 GetPoint(UnityTexture2D points, float count, float index)
{
    return SAMPLE_TEXTURE2D(
        points.tex,
        points.samplerstate,
        float2((index + 0.5) / count, 0.5)
    ).rg;
}

void PolygonSDF_float(float2 P, UnityTexture2D Points, float Count, out float Distance)
{
    // https://iquilezles.org/articles/distfunctions2d/
    float2 p1 = GetPoint(Points, Count, Count - 1);
    float d = dot(P - p1, P - p1);
    float s = 1.0;
    for(int i = 0; i < Count; i++)
    {
        float2 p2 = GetPoint(Points, Count, i);

        float2 e = p1 - p2;
        float2 w = P - p2;
        float2 b = w - e * clamp(dot(w,e) / dot(e,e), 0.0, 1.0);
        d = min(d, dot(b,b));
        bool3 c = bool3(P.y >= p2.y, P.y < p1.y, e.x*w.y > e.y*w.x);
        if(all(c) || all(!c)) s*=-1.0;

        p1 = p2;
    }
    Distance = s*sqrt(d);
}