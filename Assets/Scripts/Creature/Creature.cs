using UnityEngine;

[RequireComponent(typeof(Health))]
public class Creature : MonoBehaviour
{
    protected Health _health;
    protected Animator _animator;
    protected Rigidbody _rigidbody;
    protected float _currentHp;
    protected float _currentStaminar;

    protected virtual bool Init()
    {
        _health = GetComponent<Health>();
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponentInChildren<Rigidbody>();

        return true;
    }

    private void Awake()
    {
        Init();
    }

    private void FixedUpdate()
    {
        if (_health.IsDead()) return;

        OnFixedUpdate();
    }

    protected virtual void OnFixedUpdate()
    {
        
    }

}
