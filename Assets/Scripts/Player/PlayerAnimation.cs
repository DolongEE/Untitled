using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider playerLeftHand;
    [SerializeField] private GameObject playerRightHand;
    [SerializeField] private CapsuleCollider playerLeftWeapon;
    [SerializeField] private CapsuleCollider playerRightTool;

    public bool isItemNear = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerLeftHand = GameObject.Find("Left Weapon Hand").GetComponent<BoxCollider>();
        playerRightHand = GameObject.Find("Right Tool Hand");
        playerLeftHand.enabled = false;

        foreach (var animationClip in animator.runtimeAnimatorController.animationClips)
        {
            if (animationClip.isLooping == false)
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

        Mining();
        Attack();
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
        ColliderActivate(PlayerStatus.Instance.isPlayerEquip);
    }

    private void EquipColliderOff()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = false;
        }
        if (playerRightHand.transform.childCount > 0)
        {
            playerRightTool = playerRightHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerRightTool.enabled = false;
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
        if (Input.GetMouseButtonDown(1))
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

    // 플레이어의 손에 있는 콜라이더를 활성 / 비활성화 해주는 함수
    // 손의 자식 오브젝트가 존재할 경우에는 자식 오브젝트의 콜라이더까지 관리해준다.
    private void ColliderActivate(bool colliderEnabled)
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = colliderEnabled;
        }
        if (playerRightHand.transform.childCount > 0)
        {
            playerRightTool = playerRightHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerRightTool.enabled = colliderEnabled;
        }

        playerLeftHand.enabled = !colliderEnabled;
    }
}