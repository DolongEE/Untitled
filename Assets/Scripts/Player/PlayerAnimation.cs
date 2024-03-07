using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private BoxCollider playerLeftHand;
    [SerializeField] private CapsuleCollider playerLeftWeapon;

    private bool isNPCNear = false;
    private bool isItemNear = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerLeftHand = GameObject.Find("Left Weapon Arm").GetComponent<BoxCollider>();

        foreach(var animatorClip in animator.runtimeAnimatorController.animationClips)
        {
            Debug.Log($"[애니메이션]{animatorClip.name}");
        }
    }

    void Update()
    {
        if (Managers.otherAction)
            return;

        PickUpItem();

        if (PlayerStatus.Instance.playerEquipItem == true)
            Attack();
        else
            Mining();

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

        // 좌우 상하 눌렀을 때
        // 왼손 콜라이더 꺼주는거 << 켜져있으면 움직일 때 오브젝트 충돌 시 데미지 들어감.
        if (inputHorizontal != 0.0f || inputVertical != 0.0f)
        {
            if (PlayerStatus.Instance.playerEquipItem == false)
                DeactivateHandCollider();
            else if (PlayerStatus.Instance.playerEquipItem == true)
                DeactiveAllColliders();
        }
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += TogglePressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= TogglePressed;
    }
    public void OtherActionStart()
    {
        Managers.otherAction = true;
    }
    public void OtherActionEnd()
    {
        Managers.otherAction = false;
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
        // 마우스 좌클릭을 했을 때
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

    // 플레이어의 손에 있는 콜라이더를 활성 / 비활성화 해주는 함수
    // 손의 자식 오브젝트가 존재할 경우에는 자식 오브젝트의 콜라이더까지 관리해준다.
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            isNPCNear = true;
            Debug.Log("반갑습니다 NPC입니다.");
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
            Debug.Log("감사합니다 NPC였습니다.");
        }
        if (other.gameObject.CompareTag("Item") || other.gameObject.CompareTag("Collectable"))
        {
            isItemNear = false;
        }
    }
}