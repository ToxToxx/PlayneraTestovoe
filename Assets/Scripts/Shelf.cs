using UnityEngine;

public class Shelf : MonoBehaviour
{
    [SerializeField] private float depth;
    [SerializeField] private float snapThreshold = 0.5f;
    public float Depth => depth;

    public bool TrySnap(Transform item, out Vector3 snapPosition)
    {
        snapPosition = transform.position;
        float distance = Mathf.Abs(item.position.z - depth);

        if (distance <= snapThreshold)
        {
            snapPosition.z = depth;
            return true;
        }
        return false;
    }
}
