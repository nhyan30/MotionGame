using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    public int coinCollected = 0;

    private void FixedUpdate()
    {
        if (!GameManager.Instance.GameStarted) return;
        HandleMovement();
        UpdateVisual();
    }

    private void Awake()
    {
        Instance = this;
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
            //Debug.Log(coinCollected);
        }
        if(other.gameObject.tag == "FinishLine")
        {
            //StartCoroutine(GameManager.Instance.GameEnded());
        }
    }

    private void UpdateVisual()
    {
        scoreText.text = $"{coinCollected}";
        gameOverText.text = $"Your Score : {coinCollected}";
    }

    public void SaveScores()
    {
        DataManager.Instance.AddScoreToHighScores(DataManager.Instance.Name, coinCollected, DataManager.Instance.Email, DataManager.Instance.PhoneNumber);
        Debug.Log($"Score: {coinCollected} ,Name: {DataManager.Instance.Name} ,Email: {DataManager.Instance.Email} ,Phone number: {DataManager.Instance.PhoneNumber}");

        DataManager.Instance.SaveHighScores();
    }

}
