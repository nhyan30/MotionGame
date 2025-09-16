using System.Collections;
using UnityEngine;

public class FlashEffect : MonoBehaviour
{
    [SerializeField] Material[] materials;
    [SerializeField] private Color emissionColor = new Color(1.3f, 1.3f, 1.3f); // Light gray
    public float flashDuration = 0.1f;   // how long each flash step lasts
    public float totalFlashTime = 2f;    // total flashing time

    //public CanvasGroup detuctedScore;
    //public TextMeshProUGUI detuctedScoreText;

    public bool isFlashing = false;

    private void Awake()
    {
        foreach (var mat in materials)
        {
            mat.SetColor("_EmissionColor", Color.black);
        }
    }

    private IEnumerator DoFlash()
    {
        //MenuUIHandler.Instance.Fade(detuctedScore, true);

        Player.Instance.SetHittingState();

        isFlashing = true;
        float elapsedTime = 0f;
        while (elapsedTime < totalFlashTime)
        {
            // change to flash color
            foreach (var mat in materials)
                mat.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(flashDuration);

            // change back to original
            foreach (var mat in materials)
                mat.SetColor("_EmissionColor", Color.black);
            yield return new WaitForSeconds(flashDuration);

            elapsedTime += flashDuration * 2f; // one full cycle = flash + normal
        }

        //MenuUIHandler.Instance.Fade(detuctedScore, false);
        // make sure it ends with original color
        foreach (var mat in materials)
            mat.SetColor("_EmissionColor", Color.black);
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
            //Player.Instance.DeductCoin(3);
            //Debug.Log("collision Detected!");
            Flash();
            Player.Instance.OnObstacleHit();
        }
    }
}
