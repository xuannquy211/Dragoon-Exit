using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpriteGroup : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float alpha = 1;
    public bool Interactable = true;
    public bool BlockRaycasts = true;
    public LayerMask layer;

    public float Alpha
    {
        get => alpha;
        set
        {
            alpha = value;
            UpdateState();
        }
    }

    private List<SpriteRenderer> _spriteRenderers;
    private List<Collider2D> _colliders;

    void Awake()
    {
        _spriteRenderers = new List<SpriteRenderer>();
        _colliders = new List<Collider2D>();
    }

    void GetChild(Transform parent)
    {
        _spriteRenderers.AddRange(parent.GetComponentsInChildren<SpriteRenderer>());
        _colliders.AddRange(parent.GetComponentsInChildren<Collider2D>());

        if (parent.childCount == 0) return;
        for (var i = 0; i < parent.childCount; i++) GetChild(parent.GetChild(i));
    }

    void UpdateState()
    {
        _spriteRenderers.Clear();
        _colliders.Clear();
        GetChild(transform);
        
        foreach (var sr in _spriteRenderers)
        {
            if (IsSortingLayerInLayerMask(sr, layer))
            {
                var color = sr.color;
                color.a = alpha;
                sr.color = color;
            }
        }

        foreach (var collider in _colliders)
        {
            collider.enabled = BlockRaycasts && Interactable;
        }
    }
    
    public bool IsSortingLayerInLayerMask(SpriteRenderer spriteRenderer, LayerMask layerMask)
    {
        var sortingLayerID = spriteRenderer.gameObject.layer;
        if ((layerMask.value & (1 << sortingLayerID)) != 0)
        {
            return true;
        }

        return false;
    }
    
    #if UNITY_EDITOR
    void Update()
    {
        UpdateState();
    }
    #endif
}