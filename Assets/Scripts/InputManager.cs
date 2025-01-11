using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 lastMousePosition;

    private void Awake() => mainCamera = Camera.main;

    public Vector3 GetWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    public float GetDepthDelta() => Input.mouseScrollDelta.y;
}
