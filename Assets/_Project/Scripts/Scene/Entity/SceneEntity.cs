using System;
using UnityEngine;

public class SceneEntity : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer Renderer;
    public Collider2D Collider;
    public Rigidbody2D Rigidbody;
    public VisualScripting Script;

    public bool IsColliderEnabled => Collider.enabled;
    public bool IsGravityEnabled => Rigidbody.bodyType == RigidbodyType2D.Dynamic;

    private SceneEntityState _defaultState = new();

    public void AssignCollider(Collider2D collider)
    {
        Collider = collider;
    }

    public void OnSceneStart()
    {
        transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);

        _defaultState.Position = position;
        _defaultState.Rotation = rotation;
        _defaultState.ColliderEnabled = IsColliderEnabled;
        _defaultState.GravityEnabled = IsGravityEnabled;
        _defaultState.Velocity = Rigidbody.linearVelocity;
        _defaultState.AngularVelocity = Rigidbody.angularVelocity;

        Script.OnSceneStart();
    }

    public void OnSceneStop()
    {
        transform.SetPositionAndRotation(_defaultState.Position, _defaultState.Rotation);
        ToggleCollider(_defaultState.ColliderEnabled);
        ToggleGravity(_defaultState.GravityEnabled);
        if (_defaultState.GravityEnabled)
        {
            Rigidbody.linearVelocity = _defaultState.Velocity;
            Rigidbody.angularVelocity = _defaultState.AngularVelocity;
        }

        Script.OnSceneStop();
    }

    public void ToggleCollider(bool value)
    {
        Collider.enabled = value;
    }

    public void ToggleGravity(bool value)
    {
        if (value)
        {
            Rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            Rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}