using System;
using System.IO;
using UnityEngine;

public class GpxRecorder : MonoBehaviour
{
    [SerializeField] private bool _record;
    [SerializeField] private FocusView _focusView;

    private bool _lastFrameRecord = false;

    private OnlineMapsGPXObject _gpxTrack;
    private int _currentTrackIndex = 0;

    private void Awake()
    {
        var name = Guid.NewGuid().ToString();
        _gpxTrack = new OnlineMapsGPXObject(name);
        OnlineMapsGPXObject.Meta meta = _gpxTrack.metadata = new OnlineMapsGPXObject.Meta();
        meta.author = new OnlineMapsGPXObject.Person
        {
            email = new OnlineMapsGPXObject.EMail("support", "infinity-code.com"),
            name = name
        };
        var bounds = ContextView.ContextBounds;
        meta.bounds = new OnlineMapsGPXObject.Bounds(bounds.left, bounds.bottom, bounds.right, bounds.top);

        // Creates a copyright
        meta.copyright = new OnlineMapsGPXObject.Copyright("Infinity Code")
        {
            year = 2016
        };
    }

    private void OnEnable()
    {
        _focusView.OnLoaded += Register;
    }

    private void OnDisable()
    {
        _focusView.OnLoaded -= Register;
    }

    private void Register(OnlineMaps obj)
    {
        _focusView.FocusViewChannel.OnChange += Record;
        _focusView.ViewFinderChannel.OnChange += Record;
    }

    private void Update()
    {
        if (_lastFrameRecord == false && _record == true)
        {
            // start record
            _gpxTrack.tracks.Add(new OnlineMapsGPXObject.Track());
            _currentTrackIndex = _gpxTrack.tracks.Count - 1;
            _gpxTrack.tracks[_currentTrackIndex].segments.Add(new OnlineMapsGPXObject.TrackSegment());
        }

        _lastFrameRecord = _record;
    }

    private void OnApplicationQuit()
    {
        var gpxString = _gpxTrack.ToXML();
        var path = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        // File.WriteAllText(Path.Combine(path, _gpxTrack.metadata.author.name + ".gpx"), gpxString.outerXml);
    }

    private void Record(GeoCoord geoCoord)
    {
        if (!_record || _gpxTrack == null) return;
        var waypoint = new OnlineMapsGPXObject.Waypoint(geoCoord.Longitude, geoCoord.Latitude);
        _gpxTrack.tracks[_currentTrackIndex].segments[0].points.Add(waypoint);
    }

    
}