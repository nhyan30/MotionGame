using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;

    }

    private void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;

        // Lock X so camera doesn’t follow sideways
        targetPos.x = transform.position.x;

        transform.position = targetPos;
    }

}
