using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    Health health;

    private void Start()
    {
        health = GetComponent<Health>();
        health.SetHealth(80f);
    }

    private void OnCollisionEnter(Collision col)
    {
        Debug.Log(col.gameObject.tag + "가 몬스터에게 충돌");
        if (col.gameObject.CompareTag("Player"))
        {
            health.TakeDamage(this.gameObject, 5f);
            Debug.Log("몬스터 아프다! 5의 데미지를 입었다.");
            if(health.GetPercentage() == 0)
            {
                Debug.Log("사망띠");
            }
        }
    }
}