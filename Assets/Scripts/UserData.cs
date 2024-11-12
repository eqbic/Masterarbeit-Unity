using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New UserData", menuName = "Masterarbeit/New UserData", order = 0)]
public class UserData : ScriptableObject
{
    [field:SerializeField] public int UserId { get; set; }
    [field: SerializeField] public TouchControl TouchControl;
    [field: SerializeField] public TuiControl TuioControl;
}