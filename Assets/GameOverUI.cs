using DG.Tweening;
using System.Collections;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;
    public Transform[] Stars;
    public float interval;
    public float duration = 0.5f;
    public float yOffset = 10;
    public float floatingDuration = 2;
    public float rotationDuration = 2;
    public float rotationOffset = 10;
    //public Transform CubeSample;
    private void Awake()
    {
        Instance = this;
    }
    public void StartStarsAnimation()
    {
        StartCoroutine(nameof(AnimateStars));

    }
    IEnumerator AnimateStars()
    {
        //CubeSample.DOShakeScale(1,0.001f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        //CubeSample.DOShakeRotation(1,0.001f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        for (int i = 0; i < Stars.Length; i++)
        {
            Stars[i].DOBlendableLocalMoveBy(new Vector3(0, yOffset,0), floatingDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            Stars[i].DORotate(new Vector3(0, 0, Stars[i].rotation.eulerAngles.z + rotationOffset), rotationDuration).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
            Stars[i].DOScale(1, duration).SetEase(Ease.InOutQuad);
            yield return new WaitForSeconds(interval);
        }
    }
}
