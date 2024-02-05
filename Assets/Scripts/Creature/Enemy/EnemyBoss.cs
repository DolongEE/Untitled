using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyController
{
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 centerPos;
    private Vector3 setTarget;
    [SerializeField] float jumpTime ;
    [SerializeField] float readyTime;


    private void BeforeAttack()
    {
        startPos = transform.position;
        //endPos = _target;
    }

    private void AttackRush()
    {        
        transform.position += (transform.forward * 10f * Time.fixedDeltaTime);
    }


    private void AttackCrash()
    {
        readyTime += Time.fixedDeltaTime;
        if (readyTime < jumpTime)
            _rigidbody.AddForce(transform.up * 50f);
    }

}
