using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public static PlayerVisuals instance;
    public bool stopOtherAnims = false;
    public Animator animator;
    void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (stopOtherAnims) return;

        if (PlayerController.instance._moveXInput == 1 || PlayerController.instance._moveXInput == -1)
        {
            animator.Play("Player_Walk");
        }
        else
        {
            animator.Play("Player_Idle");
        }
    }

    public void getHurt()
    {
        animator.SetTrigger("isHurt");
        stopOtherAnims = true;
    }

    public void setIsHurtFalse()
    {
        animator.ResetTrigger("isHurt");
        stopOtherAnims = false;
    }

    public void setAttackFalse()
    {
        animator.ResetTrigger("Attack");
        stopOtherAnims = false;
    }
}