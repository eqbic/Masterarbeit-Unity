using System;
using UnityEngine;

public abstract class ViewBase : MonoBehaviour
{
    public event Action<OnlineMaps> OnLoaded;

    protected void Loaded(OnlineMaps maps)
    {
        OnLoaded?.Invoke(maps);
    }
}