using UnityEngine;

public static class TuioUtils
{
    public static Vector2 ToScreenPoint(System.Numerics.Vector2 tuioPosition)
    {
        var x = tuioPosition.X * Screen.width;
        var y = (1f - tuioPosition.Y) * Screen.height;
        return new Vector2(x, y);
    }
}