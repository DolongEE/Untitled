using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isNPCNear = false;
    private bool isItemNear = false;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += TogglePressed;
    }
    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= TogglePressed;
    }
    private void TogglePressed()
    {
        if(isNPCNear == true)
        {
            animator.SetBool("isNPC", true);
            return;
        }
        else if (isNPCNear == false)
        {
            animator.SetBool("isNPC", false);
            return;
        }
        else if(isItemNear == true)
        {
            animator.SetTrigger("isPickUp");
            return;
        }
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("NPC"))
        {
            isNPCNear = true;
            Debug.Log("반갑습니다 NPC입니다.");
        }
        if(col.gameObject.CompareTag("Item"))
        {
            isItemNear = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("NPC"))
        {
            isNPCNear = false;
            Debug.Log("감사합니다 NPC였습니다.");
        }
        if(col.gameObject.CompareTag("Item"))
        {
            isItemNear = false;
        }
    }
}
