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

    public float baseHp = 100;
    public int baseDamage = 10;
    public int baseDefense = 5;

    public float maxHp;

    public bool isPlayerEquip;

    private void Awake()
    {
        Instance = this;

        health = GetComponent<Health>();
        GameObject statBag = GameObject.Find("statBag");

        playerHp = statBag.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        playerDamage = statBag.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        playerDefense = statBag.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        playerHealthbar = GameObject.Find("playerHealthBar").GetComponent<Image>();
        playerStaminabar = GameObject.Find("playerStaminaBar").GetComponent<Image>();

        isPlayerEquip = false;
    }

    private void Start()
    {
        health.SetHealth(baseHp);

        playerHealthbar.fillAmount = health.GetPercentage();

        baseHp = 100;
        baseDamage = 10;
        baseDefense = 5;
        maxHp = baseHp;

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
        maxHp += eItem.hp;
        health.SetHealth(baseHp, maxHp);

        baseDamage += eItem.damage;
        baseDefense += eItem.defense;
        isPlayerEquip = true;
        UpdateUI();
    }

    // 장비를 해제하는 함수
    public void UnequipItem(EquippableItem eItem)
    {
        maxHp -= eItem.hp;
        health.SetHealth(baseHp, maxHp);

        baseDamage -= eItem.damage;
        baseDefense -= eItem.defense;
        isPlayerEquip = false;
        UpdateUI();
    }

    public void UseItem(UsableItem _item)
    {
        baseHp += _item.heal;
        if (baseHp >= maxHp)
        {
            baseHp = maxHp;
        }
        UpdateUI();
    }

    // UI 등을 업데이트하는 함수 (예: 체력바, 스탯 텍스트 등)
    public void UpdateUI()
    {
        // 여기에 UI 업데이트 코드 추가
        baseHp = health.GetHealth();
        playerHealthbar.fillAmount = health.GetPercentage() / 100;

        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "체  력 : " + baseHp.ToString();
        playerDamage.text = "공격력 : " + baseDamage.ToString();
        playerDefense.text = "방어력 : " + baseDefense.ToString();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(this.gameObject, 10f);
            UpdateUI();
        }
    }
}