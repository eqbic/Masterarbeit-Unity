using System;
using System.Collections.Generic;
using TuioNet.Tuio20;
using UnityEngine;

[CreateAssetMenu(fileName = "Tui Combination", menuName = "TUI/New Tui Combination", order = 0)]
public class TuiCombination : ScriptableObject
{
    [field: SerializeField] public uint MagnifyId { get; set; }
    [field: SerializeField] public uint ViewFinderId { get; set; }
    [field: SerializeField] public uint JoystickId { get; set; }
    [field: SerializeField] public uint ZoomTokenId { get; set; }

    public HashSet<uint> Ids { get; private set; } = new();

    private void OnEnable()
    {
        Ids.Add(MagnifyId);
        Ids.Add(ViewFinderId);
        Ids.Add(JoystickId);
        Ids.Add(ZoomTokenId);
    }

    public Tuio20Object Magnify { get; set; } = null;
    public Tuio20Object ViewFinder { get; set; } = null;
    public Tuio20Object Joystick { get; set; } = null;
    public Tuio20Object ZoomToken { get; set; } = null;
}