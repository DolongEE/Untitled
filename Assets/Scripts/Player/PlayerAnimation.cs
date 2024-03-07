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

        // �¿� ���� ������ ��
        // �޼� �ݶ��̴� ���ִ°� << ���������� ������ �� ������Ʈ �浹 �� ������ ��.
        if (inputHorizontal != 0.0f || inputVertical != 0.0f)
        {
            if (PlayerStatus.Instance.playerEquipItem == false)
                DeactivateHandCollider();
            else if (PlayerStatus.Instance.playerEquipItem == true)
                DeactiveAllColliders();
        }

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

        clip.AddEvent(animationStartEvent);
        clip.AddEvent(animationEndEvent);
    }

    private void OtherActionStart()
    {
        Managers.otherAction = true;        
    }

    private void OtherActionEnd()
    {
        Managers.otherAction = false;
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
            if (PlayerStatus.Instance.playerEquipItem == false)
                ActivateHandCollider();
            else
                DeactivateHandCollider();
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isAttack");
            if (PlayerStatus.Instance.playerEquipItem == false)
                ActivateHandCollider();
            else
                DeactivateHandCollider();
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
        }

        playerLeftHand.enabled = false;
    }
    private void DeactiveAllColliders()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();
            playerLeftWeapon.enabled = false;
        }
        playerLeftHand.enabled = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("NPC"))
    //    {
    //        isNPCNear = true;
    //    }
    //    if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Collectable"))
    //    {
    //        isItemNear = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("NPC"))
    //    {
    //        isNPCNear = false;
    //        animator.SetBool("isNPC", false);
    //    }
    //    if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Collectable"))
    //    {
    //        isItemNear = false;
    //    }
    //}
}