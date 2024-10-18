using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GpxData", menuName = "Masterarbeit/New GPX Track", order = 0)]
public class GpxData : ScriptableObject
{
    [HideInInspector]
    [field:SerializeField] public string GpxString { get; set; }

    public void Init(string data)
    {
        GpxString = data;
    }
}