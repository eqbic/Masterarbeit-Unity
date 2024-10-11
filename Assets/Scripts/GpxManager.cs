using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GpxManager : MonoBehaviour
{
    [SerializeField] private List<GpxData> _gpxTracks;
    [SerializeField] private float _zoneRadiusInMeter = 15;

    public static GpxManager Instance;

    private List<GpxTrack> _tracks;
    public List<GpxTrack> Tracks => _tracks;

    public float ZoneRadiusInMeter => _zoneRadiusInMeter;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetupTrackData();
    }

    private void SetupTrackData()
    {
        _tracks = new List<GpxTrack>();
        foreach (var track in _gpxTracks)
        {
            var gpxObject = OnlineMapsGPXObject.Load(track.GpxString);
            var startPoint = gpxObject.tracks[0]?.segments[0]?.points[0];
            var endPoint = gpxObject.tracks[0]?.segments[0]?.points.Last();
            var startCoord = new GeoCoord(startPoint.lon, startPoint.lat);
            var endCoord = new GeoCoord(endPoint.lon, endPoint.lat);
            var gpxTrack = new GpxTrack(startCoord, endCoord);
            _tracks.Add(gpxTrack);
        }
    }

    public List<GpxData> GpxTracks => _gpxTracks;
}

public class GpxTrack
{
    public GeoCoord Start { get; }
    public GeoCoord End { get; }

    public GpxTrack(GeoCoord start, GeoCoord end)
    {
        Start = start;
        End = end;
    }
}