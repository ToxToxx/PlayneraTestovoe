using UnityEngine;

public interface IDraggable
{
    void OnStartDrag();
    void OnDrag(Vector3 position);
    void OnEndDrag();
}
