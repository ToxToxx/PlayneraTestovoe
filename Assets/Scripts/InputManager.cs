using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _mainCamera;
    private float _pinchStartDistance;
    private bool _isPinching;

    [SerializeField] private float _pinchDepthSensitivity = 0.1f;

    private void Awake() => _mainCamera = Camera.main;

    public Vector3 GetWorldPosition()
    {
        Vector3 inputPosition;

        if (Input.touchCount > 0)
        {
            inputPosition = Input.GetTouch(0).position;
            Debug.Log("Touch");
        }
        else
        {
            inputPosition = Input.mousePosition;
            Debug.Log("Click");
        }

        inputPosition.z = -_mainCamera.transform.position.z;
        return _mainCamera.ScreenToWorldPoint(inputPosition);
    }

    public float GetDepthDelta()
    {
        if (Application.platform != RuntimePlatform.Android &&
            Application.platform != RuntimePlatform.IPhonePlayer)
        {
            return Input.mouseScrollDelta.y;
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                _pinchStartDistance = Vector2.Distance(touch0.position, touch1.position);
                _isPinching = true;
                return 0;
            }

            if (_isPinching)
            {
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);
                float delta = (currentDistance - _pinchStartDistance) * _pinchDepthSensitivity;
                _pinchStartDistance = currentDistance;
                return delta;
            }
        }
        else
        {
            _isPinching = false;
        }

        return 0;
    }

    public bool IsInputStarted()
    {
        return Input.GetMouseButtonDown(0) ||
               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    public bool IsInputEnded()
    {
        return Input.GetMouseButtonUp(0) ||
               (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended);
    }

    public bool IsInputActive()
    {
        return Input.GetMouseButton(0) || Input.touchCount > 0;
    }
}
