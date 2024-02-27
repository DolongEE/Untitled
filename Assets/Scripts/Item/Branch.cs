using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : CollectableObject
{
    [SerializeField] Health health;
    [SerializeField] private int hp;
    [SerializeField] private GameObject little_Twig;

    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    private bool isDamaged = false;

    private void Start()
    {
        health = GetComponent<Health>();
        hp = 40;
        health.SetHealth(hp);

        originRot = transform.rotation.eulerAngles;
        currentRot = originRot;
    }

    public override IEnumerator Damage(Transform playerTransform)
    {
        isDamaged = true;
        float delay = 1.0f;

        StartCoroutine(HitSwayCoroutine(playerTransform));
        //�÷��̾��� �������� ���� �޶���.
        health.TakeDamage(this.gameObject, PlayerStatus.Instance.totalDamage);
        hp = (int)health.GetHealth();

        if (hp <= 0)
            Destruction();

        yield return new WaitForSeconds(delay);

        isDamaged = false;
    }

    public override IEnumerator HitSwayCoroutine(Transform target)
    {
        // �÷��̾� >> ���������� ���ϴ� ����
        Vector3 direction = (target.position - transform.position).normalized;
        // �÷��̾� >> �������� ������ �ٶ󺸴� ������ ��
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);

        while (!CheckThreadhold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

        wantedRot = originRot;

        while(!CheckThreadhold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.15f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }
    }

    public override bool CheckThreadhold()
    {
        if (Mathf.Abs(wantedRot.x - currentRot.x) <= 0.5f &&
           Mathf.Abs(wantedRot.z - currentRot.z) <= 0.5f)
            return true;

        return false;
    }

    public override void CheckDirection(Vector3 rotationDir)
    {
        if (rotationDir.y > 180)
        {
            if (rotationDir.y > 300)
                wantedRot = new Vector3(-50f, 0, -50f);
            else if (rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -50f);
            else
                wantedRot = new Vector3(50f, 0f, -50f);
        }
        else if (rotationDir.y <= 180)
        {
            if (rotationDir.y < 60)
                wantedRot = new Vector3(-50f, 0f, 50f);
            else if (rotationDir.y > 120)
                wantedRot = new Vector3(0f, 0f, 50f);
            else
                wantedRot = new Vector3(50f, 0f, 50f);
        }
    }

    public override void Destruction()
    {
        GameObject littleTwig1 = Instantiate(little_Twig, gameObject.transform.position + new Vector3(0.2f, 0f, 0f),
            Quaternion.identity);
        GameObject littleTwig2 = Instantiate(little_Twig, gameObject.transform.position + new Vector3(-0.2f, 0f, 0f),
            Quaternion.identity);

        littleTwig1.GetComponent<BoxCollider>().isTrigger = true;
        littleTwig2.GetComponent<BoxCollider>().isTrigger = true;

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Item")) && isDamaged == false)
        {
            StartCoroutine(Damage(other.gameObject.transform));
        }
    }
}