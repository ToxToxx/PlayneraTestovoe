using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private int raycastDistance = 100;
    [SerializeField] private LayerMask interactableLayer;

    private InputManager inputManager;
    private IDraggable currentDraggable;

    private void Awake() => inputManager = GetComponent<InputManager>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryStartDrag();
        else if (Input.GetMouseButtonUp(0))
            EndDrag();
        else if (currentDraggable != null)
            UpdateDrag();
    }

    private void TryStartDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, raycastDistance, interactableLayer);

        if (hit.collider != null)
        {
            currentDraggable = hit.collider.GetComponent<IDraggable>();
            currentDraggable?.OnStartDrag();
        }
    }

    private void UpdateDrag() =>
        currentDraggable?.OnDrag(inputManager.GetWorldPosition());

    private void EndDrag()
    {
        currentDraggable?.OnEndDrag();
        currentDraggable = null;
    }
}
