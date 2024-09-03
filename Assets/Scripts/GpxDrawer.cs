using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GpxDrawer : MonoBehaviour
{
    [SerializeField] private List<GpxData> _gpxData;
    [SerializeField] private ViewBase _view;
    private OnlineMapsGPXObject _gpxObject;
    private OnlineMapsDrawingElementManager _elementManager;
    private OnlineMapsDrawingLine _line;
    private void OnEnable()
    {
        _view.OnLoaded += SetupWaypoints;
    }

    private void OnDisable()
    {
        _view.OnLoaded -= SetupWaypoints;
    }

    private void SetupWaypoints(OnlineMaps map)
    {
        _elementManager = map.drawingElementManager;
        foreach (var data in _gpxData)
        {
            _gpxObject = OnlineMapsGPXObject.Load(data.GpxString);
            List<OnlineMapsGPXObject.Waypoint> points;
            try
            {
                points = _gpxObject.tracks[0]?.segments[0]?.points;
            }
            catch (ArgumentOutOfRangeException e)
            {
                print($"Is own recorded track");
                points = _gpxObject.waypoints;
            }
                
            var p = points.Select(w => new OnlineMapsVector2d(w.lon, w.lat));
            _line = new OnlineMapsDrawingLine(p, Color.cyan, 5);
            _elementManager.Add(_line);
        }
    }
}