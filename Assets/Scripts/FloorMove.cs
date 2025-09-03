using JetBrains.Annotations;
using UnityEngine;

public class FloorMove : MonoBehaviour
{
    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        if (transform.position.z < startPos.z - 40)
        {
            transform.position = startPos;
        }
    }
}
