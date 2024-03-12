
using System.Collections;
using UnityEngine;

public class EnemyController : Creature
{
    [SerializeField] protected EnemyInfoSO monsterInfo;
    protected GameObject player;
    protected Transform playerTransform;
    protected int hp;
    public GameObject damageTextPrefab;
    private bool isDamaged = false;

    public float attackDamage { get { return monsterInfo.attackDamage; } }
    protected float moveSpeed { get { return monsterInfo.moveSpeed; } }
    protected float attackRange { get { return monsterInfo.attackRange; } }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        player = GameObject.FindWithTag("Player");
        _health.SetHealth(monsterInfo.health);
        hp = (int)_health.GetHealth();

        return true;
    }

    protected Vector3 TargetDir(Vector3 target) { return (target - transform.position).normalized; }

    protected bool AtTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        Quaternion targetRotation = Quaternion.LookRotation(TargetDir(playerTransform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
        return distanceToTarget < attackRange;
    }

    protected void TargetToMove(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(TargetDir(target));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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

    public IEnumerator Damage(Transform playerTransform)
    {
        isDamaged = true;
        float delay = 1.0f;

        _health.TakeDamage(this.gameObject, PlayerStatus.Instance.baseDamage);
        hp = (int)_health.GetHealth();

        ShowDamageText(transform.position + new Vector3(0f, 1.5f, 0f), PlayerStatus.Instance.baseDamage.ToString());

        yield return Managers.COROUTINE.WaitForSeconds(delay);

        isDamaged = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Item")) && isDamaged == false)
        {
            StartCoroutine(Damage(other.gameObject.transform));
        }
    }

    protected override void OnUpdate() { }
}
