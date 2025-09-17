using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private Vector2 baseSpeed = new Vector2(1,1); // normal speed
    [SerializeField] private float recoveryAcceleration = 0.4f; // how fast speed recovers
    [SerializeField] private float obstacleSlowSpeed = 0.2f;    // reduced speed when hit

    private float currentSpeed; // runtime speed

    public int coinCollected = 0;
    public float LimitLeft = -1;
    public float LimitRight = -.5f;
    internal Animator animator;
    private float externalInputX = 0f;

    AudioSource audioSource;
    [SerializeField] AudioClip[] coinCollectSound;
    //[SerializeField] AudioClip[] stumbleSound;


    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentSpeed = baseSpeed.y;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameStarted) return;
        HandleMovement();

        // Smooth speed transition 
        currentSpeed = Mathf.MoveTowards(currentSpeed, GameManager.Instance.isGameEnded ? 0 : baseSpeed.y, recoveryAcceleration * Time.deltaTime);

        transform.position += transform.forward * Time.deltaTime * currentSpeed;

        // Update Blend Tree parameter
        float speedRatio = currentSpeed / baseSpeed.y; 
        animator.SetFloat("Speed", speedRatio);

        MenuUIHandler.Instance.UpdateVisual(coinCollected);
    }

    private void HandleMovement()
    {
        //Vector2 inputVector = new Vector2(0, 0);

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    inputVector.x = -1;
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    inputVector.x = +1;
        //}
        //inputVector = inputVector.normalized;

        float inputX = externalInputX; // Kinect input

        // if Kinect not giving input
        if (Mathf.Approximately(inputX, 0f))
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) inputX = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) inputX = +1;
        }

        Vector3 moveDir = new Vector3(inputX, 0, 0);

        var newX = transform.position.x + (moveDir.x * Time.deltaTime * baseSpeed.x);
            
        newX = Mathf.Max(newX, LimitLeft);
        newX = Mathf.Min(newX, LimitRight);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    public void SetKinectInput(float x)
    {
        externalInputX = x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            int randomInput = UnityEngine.Random.Range(0, coinCollectSound.Length);
            audioSource.PlayOneShot(coinCollectSound[randomInput], 1);

            coinCollected++;

            Destroy(other.gameObject);
            //Debug.Log(coinCollected);
        }
        if (other.CompareTag("FinishLine"))
        {
            GameManager.Instance.FinishGame();
        }
    }

    public void OnObstacleHit()
    {
        //int randomInput = UnityEngine.Random.Range(0, stumbleSound.Length);
        //audioSource.PlayOneShot(stumbleSound[randomInput], 1);
        // Drop speed immediately
        currentSpeed = obstacleSlowSpeed;
        animator.SetLayerWeight(1, 1);
        DOVirtual.DelayedCall(1f, () => animator.SetLayerWeight(1, 0));
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

    internal void SetWinningState(bool isEnabled)
    {
        animator.SetBool("Winning", isEnabled);
    }
    internal void SetHittingState()
    {
        animator.SetTrigger("Hit");
    }

    public void DeductCoin(int amount)
    {
        coinCollected = Mathf.Max(0, coinCollected - amount);
    }
}
