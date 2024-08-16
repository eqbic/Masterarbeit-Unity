using System.Collections.Generic;
using System.Linq;
using TuioNet.Tuio20;
using TuioUnity.Common;
using UnityEngine;

public class TuiSpawner : MonoBehaviour
{
    [SerializeField] private TuioSessionBehaviour _tuioSession;
    [SerializeField] private LensSpawner _lensSpawner;
    [SerializeField] private List<TuiCombination> _tuiCombinations;

    private Tuio20Dispatcher Dispatcher => (Tuio20Dispatcher)_tuioSession.TuioDispatcher;

    private void OnEnable()
    {
        Dispatcher.OnObjectAdd += SpawnLens;
        Dispatcher.OnObjectRemove += DestroyLens;
    }

    private void OnDisable()
    {
        Dispatcher.OnObjectAdd -= SpawnLens;
        Dispatcher.OnObjectRemove -= DestroyLens;
    }

    private void SpawnLens(object sender, Tuio20Object tuioObject)
    {
        if (!tuioObject.ContainsTuioToken()) return;
        var position = TuioUtils.ToScreenPoint(tuioObject.Token.Position);
        var id = tuioObject.Token.ComponentId;
        var combo =  _tuiCombinations.First(combo => combo.ViewFinderId == id || combo.MagnifyId == id || combo.JoystickId == id);
        if (combo == null)
        {
            print("no match found");
            return;
        }

        if (id == combo.MagnifyId)
        {
            combo.Magnify = tuioObject;
            if (combo.ViewFinder == null) return;
            var viewfinderObject = combo.ViewFinder;
            var finder = _lensSpawner.SpawnViewFinder(viewfinderObject.Token.ComponentId, position, InputType.Tui, viewfinderObject);
            _lensSpawner.SpawnFocusView(tuioObject.Token.ComponentId,position, finder, InputType.Tui, tuioObject);
        }

        if (id == combo.ViewFinderId)
        {
            combo.ViewFinder = tuioObject;
            if (combo.Magnify == null) return;
            var magnifyObject = combo.Magnify;
            var finder = _lensSpawner.SpawnViewFinder(tuioObject.Token.ComponentId, position, InputType.Tui, tuioObject);
            _lensSpawner.SpawnFocusView(magnifyObject.Token.ComponentId, position, finder, InputType.Tui, magnifyObject);
        }

        if (id == combo.JoystickId)
        {
            combo.Joystick = tuioObject;
            _lensSpawner.AddJoystick(combo.MagnifyId, tuioObject);
        }
    }

    private void DestroyLens(object sender, Tuio20Object tuioObject)
    {
        if (!tuioObject.ContainsTuioToken()) return;
        var id = tuioObject.Token.ComponentId;
        var combo =  _tuiCombinations.First(combo => combo.ViewFinderId == id || combo.MagnifyId == id);
        if(combo == null)
            return;

        if (id == combo.MagnifyId)
        {
            combo.Magnify = null;
        }

        if (id == combo.ViewFinderId)
        {
            combo.ViewFinder = null;
        }

        if (id == combo.JoystickId)
        {
            combo.Joystick = null;
        }
        
        _lensSpawner.DestroyFocusView(combo.MagnifyId);
        _lensSpawner.DestroyViewFinder(combo.ViewFinderId);
    }
}