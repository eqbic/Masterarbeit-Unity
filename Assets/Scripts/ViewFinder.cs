using System;
using Channels;
using TouchScript.Gestures;
using UI;
using UnityEngine;

public class ViewFinder : MonoBehaviour
{
    [SerializeField] private OffsetMarker _offsetMarker;
    [SerializeField] private FocusViewSpawner _focusViewSpawner;
    
    [Header("Interaction")]
    [SerializeField] private Draggable _draggable;
    [SerializeField] private LongPressGesture _longPress;
    
    [Header("UI")]
    [SerializeField] private CircleUI _viewFinderUI;
    [SerializeField] private CircleUI _offsetMarkerUI;
    [SerializeField] private ConnectionUI _connectionUI;
    
    public GeoCoordChannel ViewFinderChannel { get; private set; }
    public GeoCoordChannel FocusViewChannel { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public Color Color => _viewFinderUI.Color;
    
    private OnlineMaps _contextViewMap;
    private FocusView _focus;
    
    public uint Id { get; private set; }

    public void Init(uint id, Vector2 position, OnlineMaps contextViewMap)
    {
        Id = id;
        RectTransform = GetComponent<RectTransform>();
        RectTransform.anchoredPosition = position;
        _contextViewMap = contextViewMap;
        
        ViewFinderChannel = new GeoCoordChannel();
        FocusViewChannel = new GeoCoordChannel();
        UpdateViewFinderCoords(RectTransform.anchoredPosition);
        
        _viewFinderUI.Init();
        _offsetMarkerUI.Init(_viewFinderUI.Color);
        _offsetMarker.Init(FocusViewChannel, contextViewMap);
        
        _focus = _focusViewSpawner.Spawn(this);
        _connectionUI.Init(_focus.transform as RectTransform, _viewFinderUI.Color);

        _draggable.OnDrag += UpdateViewFinderCoords;
        _longPress.LongPressed += DestroyViews;
    }
    
    private void DestroyViews(object sender, EventArgs e)
    {
        Destroy(_focus.gameObject);
        Destroy(gameObject);
    }

    private GeoCoord ScreenToCoords(Vector2 screenPosition)
    {
        _contextViewMap.control.GetCoords(screenPosition, out var lng, out var lat);
        return new GeoCoord(lng, lat);
    }

    private void UpdateViewFinderCoords(Vector2 screenPosition)
    {
        var coords = ScreenToCoords(screenPosition);
        ViewFinderChannel.RaiseEvent(coords);
    }

    private void OnDestroy()
    {
        _draggable.OnDrag -= UpdateViewFinderCoords;
        _longPress.LongPressed -= DestroyViews;
    }
    
}