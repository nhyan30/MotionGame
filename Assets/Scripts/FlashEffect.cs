using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] private Color emissionColor = new Color(1.3f, 1.3f, 1.3f); // Light gray
    public float flashDuration = 0.1f;   // how long each flash step lasts
    public float totalFlashTime = 3f;    // total flashing time

    public bool isFlashing = false;

    private void Awake()
    {
        material.SetColor("_EmissionColor", Color.black);
    }

    private IEnumerator DoFlash()
    {
        Player.Instance.SetHittingState();

        isFlashing = true;
        float elapsedTime = 0f;
        while (elapsedTime < totalFlashTime)
        {
            // change to flash color
            material.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(flashDuration);

            // change back to original
            material.SetColor("_EmissionColor", Color.black);
            yield return new WaitForSeconds(flashDuration);

            elapsedTime += flashDuration * 2f; // one full cycle = flash + normal
        }

        // make sure it ends with original color
        material.SetColor("_EmissionColor", Color.black);
        isFlashing = false;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !isFlashing)
        {
            //Debug.Log("collision Detected!");
            Flash();
            Player.Instance.OnObstacleHit();
        }
    }
}
