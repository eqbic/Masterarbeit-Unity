using TuioNet.Tuio20;
using TuioUnity.Common;
using UnityEngine;

public class TuiSpawner : MonoBehaviour
{
    [SerializeField] private TuioSessionBehaviour _tuioSession;
    [SerializeField] private ViewFinderSpawner _viewFinderSpawner;

    private Tuio20Dispatcher Dispatcher => (Tuio20Dispatcher)_tuioSession.TuioDispatcher;

    private void OnEnable()
    {
        Dispatcher.OnObjectAdd += SpawnViewFinder;
        Dispatcher.OnObjectRemove += DestroyViewFinder;
    }

    private void OnDisable()
    {
        Dispatcher.OnObjectAdd -= SpawnViewFinder;
        Dispatcher.OnObjectRemove -= DestroyViewFinder;
    }

    private void SpawnViewFinder(object sender, Tuio20Object tuioObject)
    {
        if (!tuioObject.ContainsTuioToken()) return;
        var position = TuioUtils.ToScreenPoint(tuioObject.Token.Position);
        _viewFinderSpawner.SpawnViewFinder(tuioObject.Token.SessionId,position);
    }

    private void DestroyViewFinder(object sender, Tuio20Object tuioObject)
    {
        if (!tuioObject.ContainsTuioToken()) return;
        var id = tuioObject.Token.SessionId;
        _viewFinderSpawner.DestroyViewFinder(id);
    }
}