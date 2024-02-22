using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public float totalHp;
    public int totalDamage;
    public int totalDefense;

    public bool playerEquipItem;

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

        playerEquipItem = false;
    }

    private void Start()
    {
        health.SetHealth(baseHp);

        playerHealthbar.fillAmount = health.GetPercentage();

        totalHp = baseHp;
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
        playerEquipItem = true;
        UpdateUI();
    }

    // ��� �����ϴ� �Լ�
    public void UnequipItem(EquippableItem eItem)
    {
        totalDamage -= eItem.damage;
        totalDefense -= eItem.defense;
        playerEquipItem = false;
        UpdateUI();
    }

    public void UseItem(UsableItem _item)
    {
        if(totalHp < 100)
        {
            totalHp += _item.heal;
            if(totalHp > 100)
            {
                totalHp = 100;
            }
        }
        else
        {
            Debug.Log("�ִ� ü���̻��Դϴ�.");
        }
        UpdateUI();
    }

    // UI ���� ������Ʈ�ϴ� �Լ� (��: ü�¹�, ���� �ؽ�Ʈ ��)
    public void UpdateUI()
    {
        // ���⿡ UI ������Ʈ �ڵ� �߰�
        totalHp = health.GetHealth();
        playerHealthbar.fillAmount = health.GetPercentage() / 100;
        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "ü  �� : " + totalHp.ToString();
        playerDamage.text = "���ݷ� : " + totalDamage.ToString();
        playerDefense.text = "���� : " + totalDefense.ToString();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(this.gameObject, 5f);
            UpdateUI();
        }
    }
}