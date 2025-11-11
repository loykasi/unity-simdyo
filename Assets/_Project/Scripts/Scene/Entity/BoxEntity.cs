using UnityEngine;

public class BoxEntity : SceneEntity
{
    public override EntityType EntityType => EntityType.Box;

    public BoxBorder Border;
    public float Width;
    public float Height;

    public Vector3 TopLeft
    {
        get
        {
            Vector3 point = transform.position + new Vector3(-Width / 2f, Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 TopRight
    {
        get
        {
            Vector3 point = transform.position + new Vector3(Width / 2f, Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 BottomRight
    {
        get
        {
            Vector3 point = transform.position + new Vector3(Width / 2f, -Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 BottomLeft
    {
        get
        {
            Vector3 point = transform.position + new Vector3(-Width / 2f, -Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 Left
    {
        get
        {
            Vector3 point = transform.position + new Vector3(-Width / 2f, 0f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 Right
    {
        get
        {
            Vector3 point = transform.position + new Vector3(Width / 2f, 0f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 Top
    {
        get
        {
            Vector3 point = transform.position + new Vector3(0f, Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    public Vector3 Bottom
    {
        get
        {
            Vector3 point = transform.position + new Vector3(0f, -Height / 2f, 0f);
            return Vector3Utils.RotatePointAroundPoint(point, transform.position, transform.rotation);
        }
    }

    private Vector3[] _vertices = new Vector3[4];

    public void SetSize(float width, float height)
    {
        Width = width;
        Height = height;
        ((BoxCollider2D)Collider).size = new Vector2(width, height);

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        _vertices[0] = new Vector3(halfWidth, halfHeight);
        _vertices[1] = new Vector3(- halfWidth, halfHeight);
        _vertices[2] = new Vector3(- halfWidth, - halfHeight);
        _vertices[3] = new Vector3(halfWidth, - halfHeight);
        MeshFilter.mesh.vertices = _vertices;
    }

    public void UpdateBox(Vector3 from, Vector3 to)
    {
        UpdateSize(from, to);

        Vector3 center = (from + to) / 2f;
        float halfWidth = Width / 2f;
        float halfHeight = Height / 2f;

        transform.position = center;

        _vertices[0] = new Vector3(halfWidth, halfHeight);
        _vertices[1] = new Vector3(- halfWidth, halfHeight);
        _vertices[2] = new Vector3(- halfWidth, - halfHeight);
        _vertices[3] = new Vector3(halfWidth, - halfHeight);
        MeshFilter.mesh.SetVertices(_vertices);
        MeshFilter.mesh.RecalculateBounds();

        ((BoxCollider2D)Collider).size = new Vector2(Width, Height);

        Border.SetBorder(Width, Height);
    }

    private void UpdateSize(Vector3 from, Vector3 to)
    {
        Vector3 right = transform.right;
        Vector3 xRight = Vector3Utils.ProjectOnVector(from, transform.position, right);
        Vector3 xLeft = Vector3Utils.ProjectOnVector(to, transform.position, right);
        Width = Vector3.Distance(xLeft, xRight);

        Vector3 up = transform.up;
        Vector3 yTop = Vector3Utils.ProjectOnVector(from, transform.position, up);
        Vector3 yBottom = Vector3Utils.ProjectOnVector(to, transform.position, up);
        Height = Vector3.Distance(yTop, yBottom);
    }

    public override void Select()
    {
        Border.Enable();
        Border.SetBorder(Width, Height);
    }

    public override void Deselect()
    {
        Border.Disable();
    }
}