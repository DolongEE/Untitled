using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    Health health;

    [Header("Player Status")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerDefense;
    public Image playerHealthbar;
    public Image playerStaminabar;

    public int baseDamage = 10;
    public int baseDefense = 5;

    private float totalHp;
    private int totalDamage;
    private int totalDefense;

    private void Awake()
    {
        Instance = this;
        health = GetComponent<Health>();
        
        totalHp = 100.0f;
        health.SetHealth(totalHp);

        playerHealthbar.fillAmount = health.GetPercentage() / 100;

        totalDamage = baseDamage;
        totalDefense = baseDefense;

        LinkedStatus();
    }
    public void RechargingStamina()
    {
        if (playerStaminabar.fillAmount <= 1.0f)
        {
            playerStaminabar.fillAmount = Mathf.Lerp(playerStaminabar.fillAmount, 1.0f, 0.1f * Time.deltaTime);
        }
    }
    public void PlayerDeath()
    {
        if (playerHealthbar.fillAmount == 0)
        {
            if (health.IsDead())
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void EquipItem(EquippableItem eItem)
    {
        totalDamage += eItem.damage;
        totalDefense += eItem.defense;
        UpdateUI();
    }

    // 장비를 해제하는 함수
    public void UnequipItem(EquippableItem eItem)
    {
        totalDamage -= eItem.damage;
        totalDefense -= eItem.defense;
        UpdateUI();
    }

    // UI 등을 업데이트하는 함수 (예: 체력바, 스탯 텍스트 등)
    private void UpdateUI()
    {
        // 여기에 UI 업데이트 코드 추가
        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "체  력 : " + totalHp.ToString();
        playerDamage.text = "공격력 : " + totalDamage.ToString();
        playerDefense.text = "방어력 : " + totalDefense.ToString();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(this.gameObject, 5f);
            totalHp = health.GetHealth();
            playerHealthbar.fillAmount = health.GetPercentage() / 100;
            UpdateUI();
        }
    }
}