using UnityEngine;

public abstract class Abnormality : MonoBehaviour
{
    [Button]
    public abstract void Active();
    public abstract void Deactive();
}