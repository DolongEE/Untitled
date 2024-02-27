using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : CollectableObject
{
    [SerializeField] Health health;
    [SerializeField] private int hp;
    [SerializeField] private GameObject little_Gravel;

    private Vector3 originRot;
    private Vector3 wantedRot;
    private Vector3 currentRot;

    private bool isDamaged = false;

    private void Start()
    {
        health = GetComponent<Health>();
        hp = 50;
        health.SetHealth(hp);

        originRot = transform.rotation.eulerAngles;
        currentRot = originRot;
    }

    public override IEnumerator Damage(Transform playerTransform)
    {
        isDamaged = true;
        float delay = 1.0f;

        StartCoroutine(HitSwayCoroutine(playerTransform));
        health.TakeDamage(this.gameObject, PlayerStatus.Instance.totalDamage);

        if (hp <= 0)
            Destruction();

        yield return new WaitForSeconds(delay);

        isDamaged = false;
    }

    public override IEnumerator HitSwayCoroutine(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 rotationDir = Quaternion.LookRotation(direction).eulerAngles;

        CheckDirection(rotationDir);

        while (!CheckThreadhold())
        {
            currentRot = Vector3.Lerp(currentRot, wantedRot, 0.25f);
            transform.rotation = Quaternion.Euler(currentRot);
            yield return null;
        }

        wantedRot = originRot;

        while (!CheckThreadhold())
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
                wantedRot = new Vector3(-20f, 0, -20f);
            else if (rotationDir.y > 240)
                wantedRot = new Vector3(0f, 0f, -20f);
            else
                wantedRot = new Vector3(20f, 0f, -20f);
        }
        else if (rotationDir.y <= 180)
        {
            if (rotationDir.y < 60)
                wantedRot = new Vector3(-20f, 0f, 20f);
            else if (rotationDir.y > 120)
                wantedRot = new Vector3(0f, 0f, 20f);
            else
                wantedRot = new Vector3(20f, 0f, 20f);
        }
    }

    public override void Destruction()
    {
        GameObject littleGravel1 = Instantiate(little_Gravel, gameObject.transform.position + new Vector3(0.2f, 0f, 0f),
            Quaternion.identity);
        GameObject littleGravel2 = Instantiate(little_Gravel, gameObject.transform.position + new Vector3(-0.2f, 0f, 0f),
            Quaternion.identity);

        littleGravel1.GetComponent<BoxCollider>().isTrigger = true;
        littleGravel2.GetComponent<BoxCollider>().isTrigger = true;

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
