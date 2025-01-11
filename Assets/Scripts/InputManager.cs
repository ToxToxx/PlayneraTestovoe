using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake() => _mainCamera = Camera.main;

    public Vector3 GetWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -_mainCamera.transform.position.z;
        return _mainCamera.ScreenToWorldPoint(mousePos);
    }

    public float GetDepthDelta() => Input.mouseScrollDelta.y;
}
