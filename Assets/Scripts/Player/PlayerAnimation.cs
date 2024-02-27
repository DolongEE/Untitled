using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider playerLeftHand;
    [SerializeField] private BoxCollider playerLeftWeapon;

    private bool isNPCNear = false;
    private bool isItemNear = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerLeftHand = GameObject.Find("Left Weapon Arm").GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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

        if (inputHorizontal != 0.0f || inputVertical != 0.0f)
        {
            if (PlayerStatus.Instance.playerEquipItem == false)
                DeactivateHandCollider();
            else if (PlayerStatus.Instance.playerEquipItem == true)
                DeactiveAllColliders();
        }

        PickUpItem();
        
        if (PlayerStatus.Instance.playerEquipItem == true)
            Attack();
        else
            Mining();
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += TogglePressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= TogglePressed;
    }

    private void PickUpItem()
    {
        if (Input.GetKeyDown(KeyCode.G) && isItemNear == true)
        {
            animator.SetTrigger("isPickUp");
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

    private void TogglePressed()
    {
        if (isNPCNear == true)
        {
            animator.SetBool("isNPC", true);
            return;
        }
    }

    // �÷��̾��� �տ� �ִ� �ݶ��̴��� Ȱ�� / ��Ȱ��ȭ ���ִ� �Լ�
    // ���� �ڽ� ������Ʈ�� ������ ��쿡�� �ڽ� ������Ʈ�� �ݶ��̴����� �������ش�.
    private void ActivateHandCollider()
    {
        if(playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
            playerLeftWeapon.enabled = false;
        }

        playerLeftHand.enabled = true;
    }
    private void DeactivateHandCollider()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
            playerLeftWeapon.enabled = true;
        }

        playerLeftHand.enabled = false;
    }
    private void DeactiveAllColliders()
    {
        if (playerLeftHand.gameObject.transform.childCount > 0)
        {
            playerLeftWeapon = playerLeftHand.gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
            playerLeftWeapon.enabled = false;
        }
        playerLeftHand.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPCNear = true;
            Debug.Log("�ݰ����ϴ� NPC�Դϴ�.");
        }
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Collectable"))
        {
            isItemNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPCNear = false;
            animator.SetBool("isNPC", false);
            Debug.Log("�����մϴ� NPC�����ϴ�.");
        }
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Collectable"))
        {
            isItemNear = false;
        }
    }
}