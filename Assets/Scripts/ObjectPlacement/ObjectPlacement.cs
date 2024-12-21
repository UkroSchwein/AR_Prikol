using PaleLuna.Architecture.GameComponent;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour, IStartable
{
    [Header("MVC components")]
    [SerializeField]
    private ObjectPlacementView _view;
    [SerializeField] 
    private ObjectPlacementModel _model;
    
    [SerializeField]
    private Transform _cameraTransform;

    public bool IsStarted { get; private set; } = false;

    public void OnStart()
    {
        if (IsStarted) return;
        IsStarted = true;

        _view.onDropDownChange += value => _model.SetCurrentTransform(value);
        
        TouchDetector.PlaneTouchEvent.AddListener(OnPlaneTouch);
    }

    private void OnPlaneTouch(Vector3 touchPos)
    {
        if (!_model.currentGObj.activeSelf) _model.currentGObj.SetActive(true);
        
        _model.currentTransform.position = touchPos;

        var directionToLook = _cameraTransform.position - _model.currentTransform.position;
        var lookRotation = Quaternion.LookRotation(directionToLook);

        _model.currentTransform.rotation = Quaternion.AngleAxis(lookRotation.eulerAngles.y, Vector3.up);
    }
}
