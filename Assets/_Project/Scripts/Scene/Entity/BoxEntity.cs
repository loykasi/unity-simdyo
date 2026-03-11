using Clipper2Lib;
using UnityEngine;

public class BoxEntity : MeshEntity
{
    public override EntityType EntityType => EntityType.Box;
    public override Collider Collider => _collider;
    [SerializeField] private BoxCollider _collider;

    public BoxBorder Border;
    public float Width;
    public float Height;
    public TextBox TextBox;

    public override Bounds Bounds => Renderer.bounds;

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

    [SerializeField] private BoxCollider2D _interactionBox;
    private Vector3[] _vertices = new Vector3[4];
    private BoxState _boxState;

    public void SetSize(float width, float height)
    {
        Width = width;
        Height = height;
        Vector2 size = new Vector2(width, height);
        _collider.SetSize(size);

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        _vertices[0] = new Vector3(halfWidth, halfHeight);
        _vertices[1] = new Vector3(- halfWidth, halfHeight);
        _vertices[2] = new Vector3(- halfWidth, - halfHeight);
        _vertices[3] = new Vector3(halfWidth, - halfHeight);
        MeshFilter.mesh.SetVertices(_vertices);
        MeshFilter.mesh.RecalculateBounds();

        _interactionBox.size = size;
        Border.SetBorder(Width, Height);
        TextBox.Resize(Width, Height);
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

        Vector2 size = new Vector2(Width, Height);
        _collider.SetSize(size);

        _interactionBox.size = size;
        Border.SetBorder(Width, Height);
        TextBox.Resize(Width, Height);
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

    public void ResizeByTexture()
    {
        if (TextureSlot == null) return;
        float height = Width * TextureSlot.Texture.height / TextureSlot.Texture.width;
        SetSize(Width, height);
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

    public override void OnSceneStart()
    {
        _boxState = new()
        {
            Size = new Vector2(Width, Height)
        };
        base.OnSceneStart();
    }

    public override void OnSceneStop()
    {
        if (IsAddOnRuntime)
        {
            return;
        }
        SetSize(_boxState.Size.x, _boxState.Size.y);
        base.OnSceneStop();
    }

    public override SceneEntity CloneEntity()
    {
        BoxEntity entity = ShapeGenerator.Instance.AddBox(Position, Width, Height);
        CopyPropertyTo(entity);
        ObjectManager.Instance.AddEntity(entity);

        return entity;
    }

    public override void CopyPropertyTo(SceneEntity entity)
    {
        base.CopyPropertyTo(entity);
        if (entity is BoxEntity boxEntity)
        {
            boxEntity.TextBox.CopyFrom(TextBox);   
        }
    }

    public override PathsD ToPaths()
    {
        int count = 4;
        double[] dpoints = new double[count * 2];

        dpoints[0] = TopRight.x;
        dpoints[1] = TopRight.y;
        dpoints[2] = TopLeft.x;
        dpoints[3] = TopLeft.y;
        dpoints[4] = BottomLeft.x;
        dpoints[5] = BottomLeft.y;
        dpoints[6] = BottomRight.x;
        dpoints[7] = BottomRight.y;
        
        PathsD paths = new()
        {
            Clipper.MakePath(dpoints)
        };

        return paths;
    }

    public override EntityData CreateSaveData()
    {
        Debug.Log("Create box entity save data");
        return new BoxEntityData
        {
            Id = Id,
            Name = Name,
            Type = EntityType,
            Position = transform.position,
            Rotation = transform.rotation,
            ZDepth = ZDepth,
            Width = Width,
            Height = Height,
            Text = TextBox.Text,
            TextColor = new ColorHSV(TextBox.Color),
            TextSize = TextBox.Size,
            TextHorizontalAlignment = TextBox.HorizontalAlignment,
            TextVerticalAlignment = TextBox.VerticalAlignment,
            ColliderEnabled = IsColliderEnabled,
            GravityEnabled = IsGravityEnabled,
            Layer = Layer,
            Color = CurrentColor,
            TextureSlotKey = TextureSlotKey
        };
    }
}