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
    public GameObject damageTextPrefab;

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

    public void ShowDamageText(Vector3 position, string damage)
    {
        GameObject damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        DamageTextUI damageTextUI = damageText.GetComponent<DamageTextUI>();

        if (damageTextUI != null)
        {
            damageTextUI.SetText(damage);
        }
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

    // ��� �����ϴ� �Լ�
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

    // UI ���� ������Ʈ�ϴ� �Լ� (��: ü�¹�, ���� �ؽ�Ʈ ��)
    public void UpdateUI()
    {
        // ���⿡ UI ������Ʈ �ڵ� �߰�
        baseHp = health.GetHealth();
        playerHealthbar.fillAmount = health.GetPercentage() / 100;

        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "ü  �� : " + baseHp.ToString() + " / " + maxHp.ToString();
        playerDamage.text = "���ݷ� : " + baseDamage.ToString();
        playerDefense.text = "���� : " + baseDefense.ToString();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            health.TakeDamage(this.gameObject, col.gameObject.GetComponent<EnemyNormal>().attackDamage);
            ShowDamageText(transform.position + new Vector3(0f, 2.3f, 0f), 
                col.gameObject.GetComponent<EnemyNormal>().attackDamage.ToString());
            UpdateUI();
        }
    }
}