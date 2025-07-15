using System;
using UnityEngine;

public class TestPopup : MonoBehaviour
{
    [SerializeField] private Transform optionContainer;
    [SerializeField] private OptionAbnormality prefab;
    [SerializeField] private int total = 23;

    private void Start()
    {
        for (var i = 0; i < total; i++)
        {
            var option = Instantiate(prefab, optionContainer);
            option.Init(i);
        }
    }
}