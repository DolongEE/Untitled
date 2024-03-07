using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider playerLeftHand;
    [SerializeField] private CapsuleCollider playerLeftWeapon;

    public bool isItemNear = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerLeftHand = GameObject.Find("Left Weapon Arm").GetComponent<BoxCollider>();
        
        foreach(var animationClip in animator.runtimeAnimatorController.animationClips)
        {            
            if(animationClip.isLooping == false)
            {
                AddAnimationEvents(animationClip);
            }            
        }
    }

    void Update()
    {
        if (Managers.otherAction)
            return;

        PickUpItem();

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        //Horizontal Direction
        if (inputHorizontal > 0.0f)
        {
            animator.SetBool("isMove", true);
        }
        else if (inputHorizontal < 0.0f)
        {
            animator.SetBool("isMove", true);
        }

        //Vertical Direction
        if (inputVertical > 0.0f)
        {
            animator.SetBool("isMove", true);
        }
        else if (inputVertical < 0.0f)
        {
            animator.SetBool("isMove", true);
        }

        if (inputHorizontal == 0 && inputVertical == 0)
        {
            animator.SetBool("isMove", false);
        }

        animator.SetFloat("Vertical", inputVertical);
        animator.SetFloat("Horizontal", inputHorizontal);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (PlayerStatus.Instance.playerEquipItem == true)
            Attack();
        else
            Mining();
    }

    private void AddAnimationEvents(AnimationClip clip)
    {
        AnimationEvent animationStartEvent = new AnimationEvent();
        animationStartEvent.time = 0;
        animationStartEvent.functionName = "OtherActionStart";
        animationStartEvent.stringParameter = clip.name;

        AnimationEvent animationEndEvent = new AnimationEvent();
        animationEndEvent.time = clip.length;
        animationEndEvent.functionName = "OtherActionEnd";
        animationEndEvent.stringParameter = clip.name;

        AnimationEvent animationEventColliderOn = new AnimationEvent();
        animationEventColliderOn.time = clip.length / 2;
        animationEventColliderOn.functionName = "EquipColliderOn";
        animationEventColliderOn.stringParameter = clip.name;

        AnimationEvent animationEventColliderOff = new AnimationEvent();
        animationEventColliderOff.time = clip.length;
        animationEventColliderOff.functionName = "EquipColliderOff";
        animationEventColliderOff.stringParameter = clip.name;

        clip.AddEvent(animationStartEvent);
        clip.AddEvent(animationEndEvent);
        clip.AddEvent(animationEventColliderOn);
        clip.AddEvent(animationEventColliderOff);
    }

    private void OtherActionStart()
    {
        Managers.otherAction = true;        
    }

    private void OtherActionEnd()
    {
        Managers.otherAction = false;
    }

    private void EquipColliderOn()
    {
        if (PlayerStatus.Instance.playerEquipItem == false)
            ActivateHandCollider();
        else
            DeactivateHandCollider();
    }

    private void EquipColliderOff()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = false;
        }
        playerLeftHand.enabled = false;
    }

    private void PickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && isItemNear == true)
        {            
            animator.SetTrigger("isPickUp");
            isItemNear = false;
        }
    }

    private void Mining()
    {
        // ���콺 ��Ŭ���� ���� ��
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isMining");        
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
        }
    }

    public void Talk(bool isTalking)
    {        
        animator.SetBool("isNPC", isTalking);
        Managers.otherAction = isTalking;
    }

    // �÷��̾��� �տ� �ִ� �ݶ��̴��� Ȱ�� / ��Ȱ��ȭ ���ִ� �Լ�
    // ���� �ڽ� ������Ʈ�� ������ ��쿡�� �ڽ� ������Ʈ�� �ݶ��̴����� �������ش�.
    private void ActivateHandCollider()
    {
        if(playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = false;
        }

        playerLeftHand.enabled = true;
    }
    private void DeactivateHandCollider()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = true;
            Debug.Log("����");
        }
        Debug.Log("attack");

        playerLeftHand.enabled = false;
    }
}