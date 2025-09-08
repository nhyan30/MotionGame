using DG.Tweening;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    float offset = 0.0125f;
    float randomNumber;
    private void Awake()
    {
        //randomNumber = Random.Range(-100f, 100f);
        //transform.DOLocalMoveY(transform.localPosition.y + offset, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
