using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItem : MonoBehaviour, IDraggable, IPhysicsObject
{
    [SerializeField] private float depthChangeSpeed = 0.5f;
    [SerializeField] private float shelfCheckRadius = 0.5f;

    private Rigidbody2D rb;
    private InputManager inputManager;
    private bool isDragging;
    private Shelf currentShelf;
    private float currentShelfZ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public void OnStartDrag()
    {
        isDragging = true;
        DisablePhysics();
        currentShelf = null;
    }

    public void OnDrag(Vector3 position)
    {
        if (!isDragging) return;

        transform.position = new Vector3(position.x, position.y, transform.position.z);

        float depthDelta = inputManager.GetDepthDelta();
        if (Mathf.Abs(depthDelta) > 0)
        {
            float newZ = transform.position.z + (depthDelta * depthChangeSpeed);
            transform.position = new Vector3(position.x, position.y, newZ);
        }

        CheckForShelf(position);
    }

    private void CheckForShelf(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), shelfCheckRadius);

        currentShelf = null;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Shelf>(out var shelf))
            {
                currentShelf = shelf;
                currentShelfZ = transform.position.z;
                break;
            }
        }
    }

    public void OnEndDrag()
    {
        isDragging = false;
        EnablePhysics();

        if (currentShelf == null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                currentShelfZ
            );
        }
    }

    public void EnablePhysics()
    {
        if (currentShelf == null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.velocity = Vector2.zero;
        }
    }

    public void DisablePhysics()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.velocity = Vector2.zero;
    }
}
