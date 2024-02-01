using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    public Image playerHealthbar;
    Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }
    private void Start()
    {
        health.SetHealth(100.0f);
        Debug.Log("현재 체력 : " + health.GetPercentage());
    }
}