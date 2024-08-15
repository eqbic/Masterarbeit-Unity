using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(fileName = "ScreenPosition Channel", menuName = "EventChannel/New ScreenPosition Channel", order = 0)]
    public class ScreenPositionChannel : EventChannel<Vector2>
    {
        
    }
}