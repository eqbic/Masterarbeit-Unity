using UnityEngine;

[CreateAssetMenu(fileName = "GpxData", menuName = "Masterarbeit/New GPX Track", order = 0)]
public class GpxData : ScriptableObject
{
    public string GpxString { get; set; }

    public void Init(string data)
    {
        GpxString = data;
    }
}