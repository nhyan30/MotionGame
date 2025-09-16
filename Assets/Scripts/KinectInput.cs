using UnityEngine;

public class KinectInput : MonoBehaviour
{
    public Player player;
    public Transform UCharacter;
    public float sensitivity = 2f;    // adjust scaling of movement
    private float centerX = 0f;       // neutral center position


    private void Start()
    {
        if (UCharacter != null)
        {
            // Calibrate: take initial X as the neutral center
            centerX = UCharacter.position.x;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted) return;
        if (UCharacter == null) return;

        // Calculate offset from center
        float offsetX = (UCharacter.position.x - centerX) * sensitivity;

        float kinectX = Mathf.Clamp(offsetX, -1f, 1f);

        player.SetKinectInput(kinectX);
    }
}
