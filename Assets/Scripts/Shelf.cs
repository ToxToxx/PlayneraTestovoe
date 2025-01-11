using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private float _shelfDepth;
    [SerializeField] private float _snapTreshold = 0.5f;

    public float Depth => _shelfDepth;

    public bool TrySnap(Transform item, out Vector3 snapPosition)
    {
        snapPosition = item.position;
        float distance = Mathf.Abs(item.position.z - _shelfDepth);

        if (distance <= _snapTreshold)
        {
            snapPosition.z = _shelfDepth; 
            return true;
        }
        return false;
    }
}
