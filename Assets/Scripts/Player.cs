using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 1f; // normal speed
    [SerializeField] private float recoveryAcceleration = 0.4f; // how fast speed recovers
    [SerializeField] private float obstacleSlowSpeed = 0.4f;    // reduced speed when hit

    private float currentSpeed; // runtime speed

    public int coinCollected = 0;
    public float LimitLeft = -1;
    public float LimitRight = -.5f;
    internal Animator animator;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();

        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted) return;
        HandleMovement();

        // Smooth speed transition 
        currentSpeed = Mathf.MoveTowards(currentSpeed, baseSpeed, recoveryAcceleration * Time.deltaTime);

        transform.position += transform.forward * Time.deltaTime * currentSpeed;

        // Update Blend Tree parameter
        float speedRatio = currentSpeed / baseSpeed; 
        animator.SetFloat("Speed", speedRatio);

        MenuUIHandler.Instance.UpdateVisual(coinCollected);
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

        Vector3 moveDir = new Vector3(inputVector.x, 0, 0);

        var newX = transform.position.x + (moveDir.x * Time.deltaTime * baseSpeed);

        newX = Mathf.Max(newX, LimitLeft);
        newX = Mathf.Min(newX, LimitRight);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        //Debug.Log(moveDir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCollected++;

            Destroy(other.gameObject);
            //Debug.Log(coinCollected);
        }
        if (other.CompareTag("FinishLine"))
        {
            //StartCoroutine(GameManager.Instance.GameEnded());
        }
    }

    public void OnObstacleHit()
    {
        // Drop speed immediately
        currentSpeed = obstacleSlowSpeed;
    }

    public void SaveScores()
    {
        DataManager.Instance.AddScoreToHighScores(DataManager.Instance.Name, coinCollected, DataManager.Instance.Email, DataManager.Instance.PhoneNumber);
        Debug.Log($"Score: {coinCollected} ,Name: {DataManager.Instance.Name} ,Email: {DataManager.Instance.Email} ,Phone number: {DataManager.Instance.PhoneNumber}");

        DataManager.Instance.SaveHighScores();
        MenuUIHandler.Instance.FillInHighScoreText();
    }
    
    internal void SetRunningState(bool isEnabled)
    {
        animator.SetBool("Running", isEnabled);
    }
}
