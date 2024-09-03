using Channels;
using TuioNet.Tuio20;
using TuioUnity.Tuio20;
using UI;
using UnityEngine;

public class ViewFinder : MonoBehaviour
{
    [SerializeField] private OffsetMarker _offsetMarker;

    [SerializeField] private Draggable _touchDrag;
    [SerializeField] private Tuio20TokenTransform _tokenTransform;
    
    [Header("UI")]
    [SerializeField] private CircleUI _viewFinderUI;
    [SerializeField] private CircleUI _offsetMarkerUI;
    
    public GeoCoordChannel ViewFinderChannel { get; private set; }
    public GeoCoordChannel FocusViewChannel { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public Color Color => _viewFinderUI.Color;
    
    private OnlineMaps _contextViewMap;
    public RectTransform OffsetMarker => _offsetMarker.transform as RectTransform;
    
    public uint Id { get; private set; }

    public void Init(uint id, Vector2 position, OnlineMaps contextViewMap)
    {
        Id = id;
        RectTransform = GetComponent<RectTransform>();
        RectTransform.anchoredPosition = position;
        _contextViewMap = contextViewMap;
        
        ViewFinderChannel = new GeoCoordChannel();
        FocusViewChannel = new GeoCoordChannel();
        
        ViewFinderChannel.RaiseEvent(ScreenToCoords(position));
        
        _viewFinderUI.Init();
        _offsetMarkerUI.Init(_viewFinderUI.Color);
        _offsetMarker.Init(FocusViewChannel, contextViewMap);
        
    }

    public void InitTouch()
    {
        Destroy(_tokenTransform);
    }

    public void InitTui(Tuio20Object tuioObject)
    {
        Destroy(_touchDrag);
        _tokenTransform.UpdateRotation = false;
        transform.rotation = Quaternion.identity;
        _tokenTransform.Initialize(tuioObject, RenderMode.ScreenSpaceCamera);
    }
    
    private GeoCoord ScreenToCoords(Vector2 screenPosition)
    {
        _contextViewMap.control.GetCoords(screenPosition, out var lng, out var lat);
        return new GeoCoord(lng, lat);
    }
    
    private void Update()
    {
        if (RectTransform.hasChanged)
        {
            var coords = ScreenToCoords(RectTransform.anchoredPosition);
            ViewFinderChannel.RaiseEvent(coords);
            RectTransform.hasChanged = false;
        }
    }

    // private void OnDestroy()
    // {
    //     if(_focus.gameObject)
    //         Destroy(_focus.gameObject);
    // }
    
}