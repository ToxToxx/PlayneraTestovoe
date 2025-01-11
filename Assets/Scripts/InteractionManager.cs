using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private int _raycastDistance = 100;
    [SerializeField] private LayerMask _interactableLayer;

    private InputManager _inputManager;
    private IDraggable _currentDraggable;

    private void Awake() => _inputManager = GetComponent<InputManager>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();
        else if (Input.GetMouseButtonUp(0))
            EndDrag();
        else if (_currentDraggable != null)
            UpdateDrag();
    }

    private void TryStartDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, _raycastDistance, _interactableLayer);

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
