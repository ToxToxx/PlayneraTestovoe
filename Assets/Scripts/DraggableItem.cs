using UnityEngine;

public class DraggableItem : MonoBehaviour, IDraggable, IPhysicsObject
{
    [SerializeField] private float _depthChangeSpeed = 0.5f;
    [SerializeField] private float _shelfCheckRadius = 0.5f;

    private Rigidbody2D _rb;
    private InputManager _inputManager;
    private bool _isDragging;
    private Shelf _currentShelf;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _inputManager = FindObjectOfType<InputManager>();
    }

    public void OnStartDrag()
    {
        _isDragging = true;
        DisablePhysics();
        _currentShelf = null;
    }

    public void OnDrag(Vector3 position)
    {
        if (!_isDragging) return;

        transform.position = new Vector3(position.x, position.y, transform.position.z);

        float depthDelta = _inputManager.GetDepthDelta();
        if (Mathf.Abs(depthDelta) > 0)
        {
            float newZ = transform.position.z + (depthDelta * _depthChangeSpeed);
            transform.position = new Vector3(position.x, position.y, newZ);
        }

        CheckForShelf(position);
    }

    private void CheckForShelf(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(position.x, position.y), _shelfCheckRadius);

        _currentShelf = null;
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<Shelf>(out var shelf))
            {
                _currentShelf = shelf;
                break;
            }
        }
    }

    public void OnEndDrag()
    {
        _isDragging = false;
        EnablePhysics();

        if (_currentShelf == null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
        }
        else
        {
            if (_currentShelf.TrySnap(transform, out Vector3 snapPosition))
            {
                _rb.bodyType = RigidbodyType2D.Static; 
                transform.position = snapPosition; 
            }
            else
            {
                _rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public void EnablePhysics()
    {
        if (_currentShelf == null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.velocity = Vector2.zero;
        }
    }

    public void DisablePhysics()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.velocity = Vector2.zero;
    }
}
