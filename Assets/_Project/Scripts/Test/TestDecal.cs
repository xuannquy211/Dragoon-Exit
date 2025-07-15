using System;
using UnityEngine;

public class TestDecal : MonoBehaviour
{
    [SerializeField] private GameObject decal;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.LogError("Click");
            decal.SetActive(!decal.activeSelf);
        }
    }
}