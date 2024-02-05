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
        Debug.Log(col.gameObject.tag + "�� ���Ϳ��� �浹");
        if (col.gameObject.CompareTag("Player"))
        {
            health.TakeDamage(this.gameObject, 5f);
            Debug.Log("���� ������! 5�� �������� �Ծ���.");
            if(health.GetPercentage() == 0)
            {
                Debug.Log("�����");
            }
        }
    }
}