using UnityEngine;

public class KinectInput : MonoBehaviour
{
    public Player player;
    public Transform UCharacterTorso;

    [Header("Settings")]
    public float sensitivity = 2f; // how much chest movement affects playe
    private float centerX = 0f; // neutral center position

    private void Start()
    {
        if (UCharacterTorso != null)
        {
            // take initial X as the neutral center
            centerX = UCharacterTorso.position.x;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.GameStarted) return;
        if (UCharacterTorso == null) return;

        // Calculate offset from center
        float offsetX = (UCharacterTorso.position.x - centerX) * sensitivity;

        float kinectX = Mathf.Clamp(offsetX, -1f, 1f);

        player.SetKinectInput(offsetX);

        Debug.Log($"Kinect X: {offsetX}");
    }
}
