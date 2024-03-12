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

    public int baseDamage = 10;
    public int baseDefense = 5;

    private float maxHp;

    public bool isPlayerEquip;

    private bool isDamaged;

    public int KillCount;

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
        maxHp = 100;
        health.SetHealth(maxHp, maxHp);
        playerHealthbar.fillAmount = health.GetPercentage() / 100;

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
        health.SetHealth(health.GetHealth(), maxHp);

        baseDamage += eItem.damage;
        baseDefense += eItem.defense;
        isPlayerEquip = true;
        UpdateUI();
    }

    // 장비를 해제하는 함수
    public void UnequipItem(EquippableItem eItem)
    {
        maxHp -= eItem.hp;
        health.SetHealth(health.GetHealth(), maxHp);

        baseDamage -= eItem.damage;
        baseDefense -= eItem.defense;
        isPlayerEquip = false;
        UpdateUI();
    }

    public void UseItem(UsableItem _item)
    {
        health.SetHealth(health.GetHealth() + _item.heal);
        if (health.GetHealth() >= maxHp)
        {
            health.SetHealth(health.GetHealth(), maxHp);
        }
        UpdateUI();
    }

    // UI 등을 업데이트하는 함수 (예: 체력바, 스탯 텍스트 등)
    public void UpdateUI()
    {
        // 여기에 UI 업데이트 코드 추가
        playerHealthbar.fillAmount = health.GetPercentage() / 100;

        LinkedStatus();
    }

    private void LinkedStatus()
    {
        playerHp.text = "체  력 : " + health.GetHealth().ToString() + " / " + maxHp.ToString();
        playerDamage.text = "공격력 : " + baseDamage.ToString();
        playerDefense.text = "방어력 : " + baseDefense.ToString();
    }

    public IEnumerator Damage(Transform playerTransform)
    {
        isDamaged = true;
        float delay = 1.0f;
        float damage = playerTransform.gameObject.GetComponentInParent<EnemyNormal>().attackDamage;
        health.TakeDamage(this.gameObject, damage);

        ShowDamageText(transform.position + new Vector3(0f, 2.3f, 0f), damage.ToString());
        UpdateUI();

        yield return Managers.COROUTINE.WaitForSeconds(delay);
        isDamaged = false;

        StopCoroutine(Damage(playerTransform));
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Enemy") && isDamaged == false)
        {
            StartCoroutine(Damage(col.gameObject.transform));
        }
    }
}