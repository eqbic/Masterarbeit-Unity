using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GpxDrawer : MonoBehaviour
{
    [SerializeField] private ViewBase _view;
    [SerializeField] private float _drawWidth = 5f;
    private readonly Color _startColor = new(41f/255, 255f/255, 77f/255);   
    private readonly Color _endColor = new (255f/255, 41f/255, 48f/255);
    private readonly Color _trackColor = new(255f/255, 212f/255, 34f/255);
    
    
    private OnlineMapsGPXObject _gpxObject;
    private OnlineMapsDrawingElementManager _elementManager;
    private OnlineMapsDrawingLine _line;

    private float _radius;

    private void OnEnable()
    {
        _view.OnLoaded += SetupGpxUI;
    }

    private void OnDisable()
    {
        _view.OnLoaded -= SetupGpxUI;
    }


    private void SetupGpxUI(OnlineMaps map)
    {
        var tracks = GpxManager.Instance.GpxTracks;
        _radius = GpxManager.Instance.ZoneRadiusInMeter;
        _elementManager = map.drawingElementManager;
        foreach (var track in tracks)
        {
            DrawTrack(track);
        }
    }

    private void DrawTrack(GpxData data)
    {
        var gpxObject = OnlineMapsGPXObject.Load(data.GpxString);
        DrawLine(gpxObject);
        DrawZones(gpxObject);
    }

    private void DrawZones(OnlineMapsGPXObject gpxObject)
    {
        var points = gpxObject.tracks[0]?.segments[0]?.points;
        if (points == null) return;
        var start = points.First();
        var startPoint = new OnlineMapsVector2d(start.lon, start.lat);
        var last = points.Last();
        var lastPoint = new OnlineMapsVector2d(last.lon, last.lat);
        var startColor = _startColor.WithAlpha(0.2f);
        var startRimColor = _startColor.WithAlpha(1f);
        var endColor = _endColor.WithAlpha(0.2f);
        var endRimColor = _endColor.WithAlpha(1f);
        var startZone = new OnlineMapsDrawingPoly(GetCirclePolygon(startPoint,_elementManager.map, _radius, 50), startRimColor, 5f, startColor);
        var endZone = new OnlineMapsDrawingPoly(GetCirclePolygon(lastPoint,_elementManager.map, _radius, 50), endRimColor, 5f, endColor);
        _elementManager.Add(startZone);
        _elementManager.Add(endZone);
    }

    private IEnumerable GetCirclePolygon(OnlineMapsVector2d center, OnlineMaps map, float radiusInM, int resolution)
    {
        List<Vector2> points = new();
        var centerScreenPosition = map.control.GetScreenPosition(center.x, center.y);
        OnlineMapsUtils.GetCoordinateInDistance(center.x, center.y, radiusInM / 1000f, 0f, out var rimx, out var rimy);
        var rimScreenPosition = map.control.GetScreenPosition(rimx, rimy);
        var screenRadius = (rimScreenPosition - centerScreenPosition).magnitude;
        var angleIncrement = 2 * Math.PI / resolution;
        for (var i = 0; i < resolution; i++)
        {
            var angle = i * angleIncrement;
            var x = (float)(centerScreenPosition.x + screenRadius * Math.Cos(angle));
            var y = (float)(centerScreenPosition.y + screenRadius * Math.Sin(angle));
            var geoCoord = map.control.GetCoords(new Vector2(x, y));
            points.Add(geoCoord);
        }

        return points;
    }

    private void DrawLine(OnlineMapsGPXObject gpxObject)
    {
        List<OnlineMapsGPXObject.Waypoint> points;
        try
        {
            points = gpxObject.tracks[0]?.segments[0]?.points;
        }
        catch (ArgumentOutOfRangeException e)
        {
            print($"Is own recorded track");
            points = gpxObject.waypoints;
        }

        var lineColor = _trackColor.WithAlpha(1f);
        var p = points.Select(w => new OnlineMapsVector2d(w.lon, w.lat));
        var width = 20 * _elementManager.map.floatZoom * 0.1f;
        var line = new OnlineMapsDrawingLine(p, lineColor, width);
        _elementManager.Add(line);
    }
}