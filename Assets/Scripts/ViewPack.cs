using UnityEngine;

public class ViewPack
{
    public ViewPack(GeoCoord viewFinderPosition)
    {
        ViewFinderPosition = ViewFinderPosition;
        FocusViewPosition = FocusViewPosition;
        OffsetScreenPosition = Vector2.zero;
    }
    
    public GeoCoord ViewFinderPosition { get; set; }
    public GeoCoord FocusViewPosition { get; set; }
    public Vector2 OffsetScreenPosition { get; set; }
}