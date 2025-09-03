using DG.Tweening;
using UnityEngine;
public class CameraManager : MonoBehaviour
{
    public Transform FinalCameraTransform;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.DOMove(FinalCameraTransform.position,3).SetEase(Ease.InOutQuad);
            transform.DORotate(FinalCameraTransform.rotation.eulerAngles,3).SetEase(Ease.InOutQuad);
        }

    }
}
