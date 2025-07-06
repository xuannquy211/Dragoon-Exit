using System;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancing : MonoBehaviour
{
    [Serializable]
    public class LODMesh
    {
        public Mesh mesh;
        public Material[] materials; // phải có số lượng = subMeshCount
        public float maxDistance; // hiển thị LOD này nếu <= khoảng cách này
    }

    [SerializeField] private MeshRenderer[] houses;
    [SerializeField] private LODMesh lod0; // gần camera
    [SerializeField] private LODMesh lod1; // xa hơn
    [SerializeField] private float maxRenderDistance = 300f;
    [SerializeField] private Camera targetCamera;

    private readonly int _maxBatchSize = 1023;
    private Matrix4x4[] _matrixBuffer;

    class SubMeshGroup
    {
        public Mesh mesh;
        public int subMeshIndex;
        public Material material;
        public List<Matrix4x4> matrices = new();
    }

    List<Vector3> instancePositions = new(); // vị trí của tất cả nhà
    List<Matrix4x4> worldMatrices = new();
    List<SubMeshGroup> lod0Groups = new();
    List<SubMeshGroup> lod1Groups = new();

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        _matrixBuffer = new Matrix4x4[_maxBatchSize];
        Setup();
    }

    void Setup()
    {
        lod0Groups.Clear();
        lod1Groups.Clear();
        instancePositions.Clear();
        worldMatrices.Clear();

        foreach (var renderer in houses)
        {
            var filter = renderer.GetComponent<MeshFilter>();
            if (filter == null) continue;

            instancePositions.Add(renderer.transform.position);
            worldMatrices.Add(renderer.transform.localToWorldMatrix);
            renderer.enabled = false;
        }
    }

    void Update()
    {
        Vector3 camPos = targetCamera.transform.position;
        lod0Groups.ForEach(g => g.matrices.Clear());
        lod1Groups.ForEach(g => g.matrices.Clear());

        for (int i = 0; i < instancePositions.Count; i++)
        {
            float dist = Vector3.Distance(camPos, instancePositions[i]);
            if (dist > maxRenderDistance) continue;

            var lod = dist <= lod0.maxDistance ? lod0 : lod1;
            var groups = dist <= lod0.maxDistance ? lod0Groups : lod1Groups;

            for (int sub = 0; sub < lod.mesh.subMeshCount; sub++)
            {
                if (sub >= lod.materials.Length) break;

                Material mat = lod.materials[sub];
                if (!mat.enableInstancing)
                    mat.enableInstancing = true;

                var group = groups.Find(g => g.mesh == lod.mesh && g.subMeshIndex == sub && g.material == mat);
                if (group == null)
                {
                    group = new SubMeshGroup
                    {
                        mesh = lod.mesh,
                        subMeshIndex = sub,
                        material = mat
                    };
                    groups.Add(group);
                }

                group.matrices.Add(worldMatrices[i]);
            }
        }

        // Vẽ các nhóm instanced
        DrawGroups(lod0Groups);
        DrawGroups(lod1Groups);
    }

    void DrawGroups(List<SubMeshGroup> groups)
    {
        foreach (var group in groups)
        {
            int total = group.matrices.Count;
            for (int i = 0; i < total; i += _maxBatchSize)
            {
                int count = Mathf.Min(_maxBatchSize, total - i);
                group.matrices.CopyTo(i, _matrixBuffer, 0, count);
                Graphics.DrawMeshInstanced(group.mesh, group.subMeshIndex, group.material, _matrixBuffer, count);
            }
        }
    }
}
