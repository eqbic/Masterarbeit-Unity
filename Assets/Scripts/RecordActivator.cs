using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Channels;
using UnityEngine;

public class RecordActivator : MonoBehaviour
{
    [SerializeField] private bool _inZone = false;
    [SerializeField] private float _timeInZone = 0f;
    [SerializeField] private float _startThreshold = 5f;
    [SerializeField] private float _endThreshold = 3f;
    [SerializeField] private RecordSign _recordSign;
    [SerializeField] private GpxRecorder _recorder;

    private List<GpxTrack> _tracks;
    private GeoCoordChannel _focusViewChannel;

    private GpxTrack _currentTrack = null;
    private GpxTrack _possibleNextTrack = null;

    private bool _isOnTrack;
    private float _radius;
    
    public void Init(GeoCoordChannel focusViewChannel)
    {
        _tracks = GpxManager.Instance.Tracks;
        _radius = GpxManager.Instance.ZoneRadiusInMeter;
        _focusViewChannel = focusViewChannel;
        _focusViewChannel.OnChange += CheckDistance;
    }

    private void CheckDistance(GeoCoord geoPosition)
    {
        if (_currentTrack == null)
        {
            var nextTrack = _tracks.FirstOrDefault(track => GetGeoDistance(track.Start, geoPosition) < _radius);
            if (nextTrack != null) // inside startzone
            {
                if (_inZone)
                    return;
                _timeInZone = 0f;
                _inZone = true;
                _possibleNextTrack = nextTrack;
                StartCoroutine(CountTime(_startThreshold, StartRecord, true));
            }
            else
            {
                _possibleNextTrack = null;
                _inZone = false;
                _recordSign.Progress = 0f;
            }
        }
        else
        {

            if (GetGeoDistance(_currentTrack.End, geoPosition) < _radius)
            {
                if (_inZone) return;
                _timeInZone = 0f;
                _inZone = true;
                StartCoroutine(CountTime(_endThreshold, StopRecord, false));
            }
            else
            {
                _inZone = false;
                _recordSign.Progress = 1.0f;
            }
            
        }
    }

    private double GetGeoDistance(GeoCoord from, GeoCoord to)
    {
        return OnlineMapsUtils.DistanceBetweenPoints(from.Longitude, from.Latitude, 0, to.Longitude, to.Latitude, 0) * 1000;
    }

    private void StartRecord()
    {
        _currentTrack = _possibleNextTrack;
        print("Start Record");
        _recorder.StartRecord(_currentTrack.Name);
    }
    
    private void StopRecord()
    {
        print("Stop Record");
        _recorder.StopRecord();
        _possibleNextTrack = null;
        _currentTrack = null;
    }
    //
    IEnumerator CountTime(float timeThreshold, Action onFinish, bool isStart)
    {
        while (_timeInZone < timeThreshold && _inZone)
        {
            _timeInZone += Time.deltaTime;
            var progress = _timeInZone / timeThreshold;
            if (isStart)
            {
                _recordSign.Progress = progress;
            }
            else
            {
                _recordSign.Progress = 1.0f - progress;
            }
            yield return null;
        }
    
        if (!(_timeInZone < timeThreshold))
        {
            onFinish?.Invoke();
        }

        _inZone = false;
    }

}