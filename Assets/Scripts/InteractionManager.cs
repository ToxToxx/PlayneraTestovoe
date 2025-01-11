using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private float _raycastDistance = 100f;
    [SerializeField] private LayerMask _interactableLayer;

    private InputManager _inputManager;
    private IDraggable _currentDraggable;

    private void Awake() => _inputManager = GetComponent<InputManager>();

    private void Update()
    {
        if (_inputManager.IsInputStarted())
            TryStartDrag();
        else if (_inputManager.IsInputEnded())
            EndDrag();
        else if (_currentDraggable != null && _inputManager.IsInputActive())
            UpdateDrag();
    }

    private void TryStartDrag()
    {
        Vector3 worldPosition = _inputManager.GetWorldPosition();
        Vector2 position2D = (Vector2)worldPosition;

        RaycastHit2D hit = Physics2D.Raycast(position2D, Vector2.zero, _raycastDistance, _interactableLayer);

        if (hit.collider != null)
        {
            _currentDraggable = hit.collider.GetComponent<IDraggable>();
            _currentDraggable?.OnStartDrag();
        }
    }

    private void UpdateDrag() =>
        _currentDraggable?.OnDrag(_inputManager.GetWorldPosition());

    private void EndDrag()
    {
        _currentDraggable?.OnEndDrag();
        _currentDraggable = null;
    }
}
