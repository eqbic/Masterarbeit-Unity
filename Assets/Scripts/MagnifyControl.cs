using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TuioNet.Tuio11;
using TuioUnity.Common;
using TuioUnity.Utils;
using UnityEngine;
using UnityEngine.UI;

public class MagnifyControl : MonoBehaviour
{
    [SerializeField] private TuioSessionBehaviour _session;

    [SerializeField] private Magnify _magnify;

    private RectTransform _rect;
    private Vector2 _screenPosition = Vector2.zero;
    private Tuio11Dispatcher Dispatcher => _session.TuioDispatcher as Tuio11Dispatcher;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var objects = Dispatcher.GetTuioObjects();
        var magnify = objects.FirstOrDefault(tuioObject => tuioObject.SymbolId == 37);
        if(magnify == null) return;

        var normalizedPosition = magnify.Position.ToUnity();
        _screenPosition.x = normalizedPosition.x * Screen.width;
        _screenPosition.y = -normalizedPosition.y * Screen.height;
        _rect.anchoredPosition = _screenPosition;

        _magnify.Zooming(magnify.RotationSpeed);
    }
}
