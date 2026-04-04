using Clipper2Lib;
using UnityEngine;

public class BoxEntity : MeshEntity
{
    public override EntityType EntityType => EntityType.Box;
    public override Collider Collider => _collider;
    [SerializeField] private BoxCollider _collider;

    public float Width;
    public float Height;
    public Vector2 Size => new(Width, Height);
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
    [SerializeField] private BoxBorder _border;
    private BoxState _boxState;

    public void SetMesh(MeshWrapper meshWrapper, Material material, float width, float height)
    {
        Mesh = meshWrapper;
        Material = Instantiate(material);
        Width = width;
        Height = height;

        meshWrapper.AssignTo(MeshFilter);
        Renderer.sharedMaterial = Material;

        _interactionBox.size = Size;
        _border.SetMesh(meshWrapper);
        _border.SetSize(Size);
        _collider.SetSize(Size);
        TextBox.Resize(Size);
    }

    public void SetSize(float width, float height)
    {
        Width = width;
        Height = height;

        Vector2 halfSize = Size / 2f;

        Mesh.Vertices[0] = new Vector3(halfSize.x, halfSize.y);
        Mesh.Vertices[1] = new Vector3(- halfSize.x, halfSize.y);
        Mesh.Vertices[2] = new Vector3(- halfSize.x, - halfSize.y);
        Mesh.Vertices[3] = new Vector3(halfSize.x, - halfSize.y);
        Mesh.Update();

        _interactionBox.size = Size;
        _border.SetSize(Size);
        _collider.SetSize(Size);
        TextBox.Resize(Size);
    }

    public void SetSize(Vector3 from, Vector3 to)
    {
        UpdateSize(from, to, out float width, out float height);
        transform.position = (from + to) / 2f;;

        SetSize(width, height);
    }

    private void UpdateSize(Vector3 from, Vector3 to, out float width, out float height)
    {
        Vector3 right = transform.right;
        Vector3 xRight = Vector3Utils.ProjectOnVector(from, transform.position, right);
        Vector3 xLeft = Vector3Utils.ProjectOnVector(to, transform.position, right);
        width = Vector3.Distance(xLeft, xRight);

        Vector3 up = transform.up;
        Vector3 yTop = Vector3Utils.ProjectOnVector(from, transform.position, up);
        Vector3 yBottom = Vector3Utils.ProjectOnVector(to, transform.position, up);
        height = Vector3.Distance(yTop, yBottom);
    }

    public void ResizeByTexture()
    {
        if (TextureSlot == null) return;
        float height = Width * TextureSlot.Texture.height / TextureSlot.Texture.width;
        SetSize(Width, height);
    }

    public override void Select()
    {
        _border.Enable();
        _border.SetSize(Size);
    }

    public override void Deselect()
    {
        _border.Disable();
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
        BoxEntity entity = ShapeGenerator.Instance.Clone(this);
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