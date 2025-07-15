using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionAbnormality : MonoBehaviour
{
    [SerializeField] private TMP_Text indexTxt;
    [SerializeField] private Button button;
    
    private int _index;
    
    public void Init(int index){
        _index = index;
        indexTxt.text = $"{index + 1}";
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        EnvironmentManager.Instance.ActiveAbnormality(_index);
        GameplayUIManager.Instance.CloseTest();
    }
}