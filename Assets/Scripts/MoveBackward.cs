using UnityEngine;

public class MoveBackward : MonoBehaviour
{
    [SerializeField] float speed = 10;

    private void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }
}
