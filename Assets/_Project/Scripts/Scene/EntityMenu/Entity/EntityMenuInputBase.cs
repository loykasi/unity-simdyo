using UnityEngine;

public abstract class EntityMenuInputBase : MonoBehaviour
{
    [SerializeField] protected EntityPropertyType _type;
    
    public abstract void UpdateValue();
}