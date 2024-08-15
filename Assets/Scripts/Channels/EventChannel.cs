using System;
using UnityEngine;

namespace Channels
{
    public abstract class EventChannel<T>
    {
        public event Action<T> OnChange;


        [field: SerializeField]
        public T Data { get; private set; }

        public void RaiseEvent(T data)
        {
            Data = data;
            OnChange?.Invoke(data);
        }

        public void RaiseEventIfChanged(T data)
        {
            if (Data.Equals(data)) return;
            RaiseEvent(data);
        }

        public void Subscribe(Action<T> callback) 
        {
            if (callback == null) return;
            OnChange += callback;
            callback(Data);
        }

        public void Unsubscribe(Action<T> callback)
        {
            if (callback == null) return;
            OnChange -= callback;
        }
    }
}