using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed = 10f;
    [SerializeField] private Vector2 _scrollBounds = new(-10f, 10f);

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0)
            ScrollCamera(horizontalInput);
    }

    private void ScrollCamera(float direction)
    {
        float newX = transform.position.x + (direction * _scrollSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, _scrollBounds.x, _scrollBounds.y);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}