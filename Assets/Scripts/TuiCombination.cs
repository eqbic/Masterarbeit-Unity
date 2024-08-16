using System.Collections.Generic;
using TuioNet.Tuio20;
using UnityEngine;

[CreateAssetMenu(fileName = "Tui Combination", menuName = "TUI/New Tui Combination", order = 0)]
public class TuiCombination : ScriptableObject
{
    [field: SerializeField] public uint MagnifyId { get; set; }
    [field: SerializeField] public uint ViewFinderId { get; set; }

    
    public Tuio20Object Magnify { get; set; } = null;
    public Tuio20Object ViewFinder { get; set; } = null;
}