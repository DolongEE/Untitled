using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float _health;
    private float _maxHealth;

    private bool isDead = false;

    public void SetHealth(float health)
    {
        _health = health;
        _maxHealth = health;
    }

    //������ ���
    public void TakeDamage(GameObject instigator, float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        print("ü�� : " + _health);

        if (_health <= 0)
        {
            Dead();            
        }
    }

    //ü�� �ۼ�Ʈ ��ȯ
    public float GetPercentage()
    {
        return (_health / _maxHealth) * 100;
    }


    //��� ó��
    private void Dead()
    {
        if (isDead) return;

        isDead = true;
    }

    //��� üũ
    public bool IsDead()
    {
        return isDead;
    }

}
