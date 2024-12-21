using System.Collections.Generic;
using PaleLuna.Architecture.GameComponent;
using PaleLuna.Architecture.Loops;
using Services;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class TouchDetector : MonoBehaviour, IUpdatable, IStartable
{
    [Header("AutoFilling")]
    [SerializeField]
    private ARRaycastManager _raycastManager;

    private static List<ARRaycastHit> s_hits = new();

    public static UnityEvent<Vector3> PlaneTouchEvent = new();

    private bool _isStart = false;
    public bool IsStarted => _isStart;

    private void OnValidate() 
    {
        _raycastManager ??= GetComponent<ARRaycastManager>();
    }

    public void OnStart()
    {
        if(_isStart) return;
        _isStart = true;

        ServiceManager.Instance
            .GlobalServices.Get<GameLoops>()
            .Registration(this);
    }

    public void EveryFrameRun()
    {
        CheckTouchPlane();
    }

    private void CheckTouchPlane()
    {
        if (!TryGetTouchOnScreen(out Vector2 touchPos) ||
            !_raycastManager.Raycast(touchPos, s_hits, TrackableType.Planes)) 
            return;

        print("hit");
        Pose hitPose = s_hits[0].pose;

        PlaneTouchEvent.Invoke(hitPose.position);
    }
    private bool TryGetTouchOnScreen(out Vector2 touchPos)
    {
        bool isTouch = Input.touchCount > 0;

        if (isTouch)
            touchPos = Input.GetTouch(0).position;
        else touchPos = default;

        return isTouch;
    }
}
