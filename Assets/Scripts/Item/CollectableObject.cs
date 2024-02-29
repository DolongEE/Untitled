using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public virtual IEnumerator Damage(Transform playerTransform)
    {
        yield return null;
    }

    public virtual IEnumerator HitSwayCoroutine(Transform target)
    {
        yield return null;
    }

    public virtual bool CheckThreadhold()
    {
        return true;
    }

    public virtual void CheckDirection(Vector3 rotationDir)
    {

    }

    public virtual void Destruction()
    {

    }
}