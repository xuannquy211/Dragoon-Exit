using UnityEngine;

public abstract class Abnormality : MonoBehaviour
{
    [SerializeField] protected bool isSpecial;
    
    [Button]
    public abstract void Active();
    public abstract void Deactive();
}