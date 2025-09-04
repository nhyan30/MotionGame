using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    [SerializeField] GameObject menuList;
    [SerializeField] GameObject touchToStart;

    bool started = false;

    private void Awake()
    {
        touchToStart.SetActive(true);
        menuList.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && started == false)
        {
            started = true;
            touchToStart.SetActive(false);
            menuList.SetActive(true);
        }
    }
}
