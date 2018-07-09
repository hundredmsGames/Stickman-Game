using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController
{
    void UpdateCharacterAnimations()
    {
        //IdleWalkRunAnim();
        //CharacterStateAnimations();

        
        animator.SetBool("isJumping", jumping);
        animator.SetBool("isRunning", grounded);
        animator.SetBool("isFalling", falling);
        //animator.SetBool("isJumpOver", jumping);
    }

    public void JumpingAnimationEnded()
    {   
    }

    public void JumpOverAnimationEnded()
    {   
    }

    public void FallingAnimationEnded()
    {
    }

    private void OnGUI()
    {
        
        GUI.color = Color.blue;
        
        string animationInfo = "";
        if (grounded)
        {
            animationInfo = "running";
        }
        if (jumping)
        {
            animationInfo = "jumping";
        }
        if(falling)
        {
            animationInfo = "falling";
        }
        GUI.Label(new Rect(0, 0, 100, 100), animationInfo);
    }
}
