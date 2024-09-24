using System;
using System.IO;
using System.Xml.Linq;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

public class GpxRecorder : MonoBehaviour
{
    [SerializeField] private bool _record;
    [SerializeField] private FocusView _focusView;
    [SerializeField] private LongPressGesture _gesture;
    [SerializeField] private Image _recordSign;

    private bool _lastFrameRecord = false;

    private OnlineMapsGPXObject _gpxTrack;
    private OnlineMapsGPXObject.Meta _metaData;
    private string _savePath;

    private void Awake()
    {
        _metaData =  new OnlineMapsGPXObject.Meta
        {
            author = new OnlineMapsGPXObject.Person
            {
                email = new OnlineMapsGPXObject.EMail("support", "infinity-code.com"),
            }
        };
        var bounds = ContextView.ContextBounds;
        _metaData.bounds = new OnlineMapsGPXObject.Bounds(bounds.left, bounds.bottom, bounds.right, bounds.top);

        // Creates a copyright
        _metaData.copyright = new OnlineMapsGPXObject.Copyright("Infinity Code")
        {
            year = 2016
        };
        
        _savePath = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
    }

    private void OnEnable()
    {
        _focusView.OnLoaded += Register;
        _gesture.LongPressed += ToggleRecord;
    }

    private void OnDisable()
    {
        _focusView.OnLoaded -= Register;
        _gesture.LongPressed -= ToggleRecord;
    }

    private void ToggleRecord(object sender, EventArgs e)
    {
        _record = !_record;
        _recordSign.enabled = _record;

        if (_record)
        {
            StartRecord();
        }
        else
        {
            StopRecord();
        }
    }


    private void StartRecord()
    {
        var trackName = $"{_focusView.FocusMapControl.InputTypeCode}_{DateTime.Now:yy-MM-dd-HH-mm-ss}";
        _metaData.author.name = trackName;
        _gpxTrack = new OnlineMapsGPXObject(trackName)
        {
            metadata = _metaData
        };
        _gpxTrack.tracks.Add(new OnlineMapsGPXObject.Track());
        _gpxTrack.tracks[0].segments.Add(new OnlineMapsGPXObject.TrackSegment());
    }
    
    private void StopRecord()
    {
        var gpxString = _gpxTrack.ToXML();
        var doc = XDocument.Parse(gpxString.outerXml);
        File.WriteAllText(Path.Combine(_savePath, _gpxTrack.metadata.author.name + ".gpx"), doc.ToString());
    }

    private void Register(OnlineMaps obj)
    {
        _focusView.FocusViewChannel.OnChange += Record;
        _focusView.ViewFinderChannel.OnChange += Record;
    }

    private void Record(GeoCoord geoCoord)
    {
        if (!_record || _gpxTrack == null) return;
        var waypoint = new OnlineMapsGPXObject.Waypoint(geoCoord.Longitude, geoCoord.Latitude)
        {
            time = DateTime.Now
        };
        _gpxTrack.tracks[0].segments[0].points.Add(waypoint);
    }

    
}