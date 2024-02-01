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

    //데미지 계산
    public void TakeDamage(GameObject instigator, float damage)
    {
        _health = Mathf.Max(_health - damage, 0);
        print("체력 : " + _health);

        if (_health <= 0)
        {
            Dead();            
        }
    }

    //체력 퍼센트 반환
    public float GetPercentage()
    {
        return (_health / _maxHealth) * 100;
    }


    //사망 처리
    private void Dead()
    {
        if (isDead) return;

        isDead = true;
    }

    //사망 체크
    public bool IsDead()
    {
        return isDead;
    }

}
