using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance;
    public Health health;

    [Header("Player Status")]
    public TextMeshProUGUI playerHp;
    public TextMeshProUGUI playerDamage;
    public TextMeshProUGUI playerDefense;

    public int baseDamage = 10;
    public int baseDefense = 5;

    private float totalHp = 100.0f;
    private int totalDamage;
    private int totalDefense;

    private void Awake()
    {
        Instance = this;
        health = GetComponent<Health>();

        health.SetHealth(totalHp);

        totalDamage = baseDamage;
        totalDefense = baseDefense;

        LinkedStatus();
    }

    public void EquipItem(EquippableItem eItem)
    {
        totalDamage += eItem.damage;
        totalDefense += eItem.defense;
        UpdateUI();
    }

    // ��� �����ϴ� �Լ�
    public void UnequipItem(EquippableItem eItem)
    {
        totalDamage -= eItem.damage;
        totalDefense -= eItem.defense;
        UpdateUI();
    }

    // UI ���� ������Ʈ�ϴ� �Լ� (��: ü�¹�, ���� �ؽ�Ʈ ��)
    private void UpdateUI()
    {
        // ���⿡ UI ������Ʈ �ڵ� �߰�
        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "ü  �� : " + totalHp.ToString();
        playerDamage.text = "���ݷ� : " + totalDamage.ToString();
        playerDefense.text = "���� : " + totalDefense.ToString();
    }
}