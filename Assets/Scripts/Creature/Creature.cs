using UnityEngine;

[RequireComponent(typeof(Health))]
public class Creature : MonoBehaviour
{
    private CreatureType _creatureType;
    [Header("Creature ID")]
    [SerializeField] private int _creatureId;

    public CreatureType CreatureType { get { return _creatureType; } set { _creatureType = value; } }    
    public int CreatureID { get { return _creatureId; } set { _creatureId = value; } }

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
