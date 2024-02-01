using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyController
{
    Vector3 startPos;
    Vector3 endPos;
    Vector3 centerPos;

    protected override void AttackBehaviour()
    {      
        Attack2();
    }

    protected override void AttackUpdateBehaviour()
    {
        //Attack1();
    }

    bool once;

    private void Attack1()
    {
        if (!once)
        {
            startPos = transform.position;
            endPos = target;
            once = true;
        }
        Vector3 targetDir = (endPos - startPos).normalized;
        transform.rotation = Quaternion.LookRotation( targetDir);
        transform.position += (transform.forward * 10f * Time.fixedDeltaTime);
    }

    private void Attack2()
    {
        if (!once)
        {
            startPos = transform.position;
            endPos = target;
            once = true;
        }

        rigid.AddForce(transform.up * 100f);
    }


}
