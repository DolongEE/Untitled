using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider EnemyRightHand;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        EnemyRightHand = transform.GetComponentInChildren<BoxCollider>();
        EnemyRightHand.enabled = false;
    }

    private void Start()
    {
        foreach (var animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.name.Equals("PunchLeft"))
            {
                AddAnimationEvents(animationClip);
            }
        }
    }

    private void AddAnimationEvents(AnimationClip clip)
    {
        AnimationEvent animationEventColliderOn = new AnimationEvent();
        animationEventColliderOn.time = clip.length / 2;
        animationEventColliderOn.functionName = "ColliderOn";

        AnimationEvent animationEventColliderOff = new AnimationEvent();
        animationEventColliderOff.time = clip.length / 2 + 0.05f;
        animationEventColliderOff.functionName = "ColliderOff";

        clip.AddEvent(animationEventColliderOn);
        clip.AddEvent(animationEventColliderOff);
    }

    private void ColliderOn()
    {
        EnemyRightHand.enabled = true;
    }
    private void ColliderOff()
    {
        EnemyRightHand.enabled = false;
    }
}
