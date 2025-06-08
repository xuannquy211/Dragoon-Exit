using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Game Datas/DataContainer")]
public class DataContainer : ScriptableObject
{
    public ScriptableObject[] scriptableObjects;
}