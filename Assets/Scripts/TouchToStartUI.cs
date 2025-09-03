using UnityEngine;

public class TouchToStartUI : MonoBehaviour
{
    [SerializeField] GameObject menuList;

    private void Awake()
    {
        gameObject.SetActive(true);
        menuList.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            menuList.SetActive(true);
        }
    }
}
