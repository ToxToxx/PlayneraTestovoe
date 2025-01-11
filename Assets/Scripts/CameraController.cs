using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 10f;
    [SerializeField] private Vector2 _scrollBounds = new(-10f, 10f);

    private Vector2 _touchStartPosition;
    private bool _isTwoFingerDrag;

    private void Update()
    {
        // ПК управление
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0)
            ScrollCamera(horizontalInput * _scrollSpeed * Time.deltaTime);

        // Мобильное управление
        HandleTwoFingerDrag();
    }

    private void HandleTwoFingerDrag()
    {
        if (Input.touchCount != 2)
        {
            _isTwoFingerDrag = false;
            return;
        }

        Touch touch0 = Input.GetTouch(0);
        Touch touch1 = Input.GetTouch(1);

        // Начало касания двумя пальцами
        if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
        {
            _isTwoFingerDrag = true;
            _touchStartPosition = (touch0.position + touch1.position) / 2;
            return;
        }

        // Перемещение камеры при движении пальцев
        if (_isTwoFingerDrag && (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved))
        {
            Vector2 currentTouchPosition = (touch0.position + touch1.position) / 2;
            float delta = (currentTouchPosition.x - _touchStartPosition.x) * _scrollSpeed * Time.deltaTime * 0.01f;
            ScrollCamera(-delta);
            _touchStartPosition = currentTouchPosition;
        }
    }

    private void ScrollCamera(float deltaX)
    {
        float newX = transform.position.x + deltaX;
        newX = Mathf.Clamp(newX, _scrollBounds.x, _scrollBounds.y);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void SetCameraPosition(float x)
    {
        float clampedX = Mathf.Clamp(x, _scrollBounds.x, _scrollBounds.y);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}