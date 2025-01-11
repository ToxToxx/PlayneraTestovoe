using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 10f;
    [SerializeField] private Vector2 scrollBounds = new Vector2(-10f, 10f);

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        if (horizontalInput != 0)
            ScrollCamera(horizontalInput);
    }

    private void ScrollCamera(float direction)
    {
        float newX = transform.position.x + (direction * scrollSpeed * Time.deltaTime);
        newX = Mathf.Clamp(newX, scrollBounds.x, scrollBounds.y);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}