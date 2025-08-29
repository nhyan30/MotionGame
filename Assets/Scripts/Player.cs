using TMPro;
using Unity.Jobs;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOver;
    private int coinCollected = 0;

    private void Update()
    {
        HandleMovement();
        UpdateVisual();
    }


    private void HandleMovement()
    {
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            inputVector.x = +1;
        }


        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0, 1);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
        //Debug.Log(moveDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin")
        {
            other.gameObject.SetActive(false);
            coinCollected++;
            Debug.Log(coinCollected);
        }
        if(other.gameObject.tag == "FinishLine")
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
            DataManager.Instance.AddScoreToHighScores(DataManager.Instance.Name, coinCollected, DataManager.Instance.PhoneNumber);
            Debug.Log($"Score: {coinCollected} ,Name: {DataManager.Instance.Name}, Phone number: {DataManager.Instance.PhoneNumber}");

            DataManager.Instance.SaveHighScores();
        }
    }

    private void UpdateVisual()
    {
        scoreText.text = $"Score: {coinCollected}";
    }
}
