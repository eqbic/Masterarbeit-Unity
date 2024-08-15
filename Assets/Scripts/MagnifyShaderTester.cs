using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class MagnifyShaderTester : MonoBehaviour
    {
        [SerializeField] private List<Magnify> _positions;
        [SerializeField] private Material _magnifyMaterial;
        private static readonly int PositionCount = Shader.PropertyToID("_PositionCount");
        private static readonly int Positions = Shader.PropertyToID("_Positions");

        private void Start()
        {
            _magnifyMaterial.SetInt(PositionCount, _positions.Count);
        }

        private void Update()
        {
            _magnifyMaterial.SetVectorArray(Positions, _positions.Select(geo => geo.NormalizedPosition).ToArray());
        }
    }
}