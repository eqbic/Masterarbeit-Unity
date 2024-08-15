using System;
using System.Collections.Generic;
using System.Linq;
using TuioNet.Common;
using TuioNet.Tuio11;
using TuioUnity.Common;
using UnityEngine;

namespace DefaultNamespace
{
    public class MagnifyManager : MonoBehaviour
    {
        [SerializeField] private TuioSessionBehaviour _session;
        [SerializeField] private Magnify _magnifyPrefab;
        [SerializeField] private Material _magnifyMapMaterial;
        [SerializeField] private RectTransform _magnifyCanvas;
        [SerializeField] private OnlineMaps _bigMap;

        private readonly Dictionary<uint, Magnify> _magnifies = new();
        private Tuio11Dispatcher Dispatcher => (Tuio11Dispatcher)_session.TuioDispatcher;
        
        // private static readonly int PositionCount = Shader.PropertyToID("_PositionCount");
        private static readonly int Positions = Shader.PropertyToID("_Positions");
        
        private readonly int[] _validIds = { 37, 38 };

        private readonly Vector4[] _positions = new Vector4[5];
        
        private void OnEnable()
        {
            Dispatcher.OnObjectAdd += AddMagnify;
            Dispatcher.OnObjectRemove += RemoveMagnify;
        }

        private void OnDisable()
        {
            Dispatcher.OnObjectAdd -= AddMagnify;
            Dispatcher.OnObjectRemove -= RemoveMagnify;
        }

        private void AddMagnify(object sender, Tuio11Object tuioObject)
        {
            if (_validIds.All(id => id != tuioObject.SymbolId)) return;
            var magnify = Instantiate(_magnifyPrefab, _magnifyCanvas);
            magnify.Init(tuioObject, _bigMap);
            _magnifies.Add(tuioObject.SessionId, magnify);
            // _magnifyMapMaterial.SetInt(PositionCount,_magnifies.Count);
        }

        private void RemoveMagnify(object sender, Tuio11Object tuioObject)
        {
            if (!_magnifies.Remove(tuioObject.SessionId, out var magnify)) return;
            // _magnifyMapMaterial.SetInt(PositionCount,_magnifies.Count);
            Destroy(magnify.gameObject);
        }

        private void Update()
        {
            for (var a = 0; a < _positions.Length; a++)
            {
                _positions[a] = Vector4.zero;
            }
            
            int i = 0;
            foreach (var magnify in _magnifies.Values)
            {
                _positions[i] = magnify.NormalizedPosition;
                i++;
            }
            
            

            _magnifyMapMaterial.SetVectorArray(Positions, _positions);
        }
    }
}