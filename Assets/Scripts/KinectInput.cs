using UnityEngine;

public class KinectInput : MonoBehaviour
{
    public Player player;

    void Update()
    {
        if (!GameManager.Instance.GameStarted) return;

        // Example pseudo input from Kinect
        // Replace with your actual Azure Kinect SDK code
        float kinectX = 0f;

        // Example: if user moves right in front of sensor
        if (Input.GetKey(KeyCode.RightArrow)) // <-- placeholder until Kinect is set up
        {
            kinectX = 1f;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            kinectX = -1f;
        }

        // Send input to Player
        player.SetKinectInput(kinectX);
    }
}
