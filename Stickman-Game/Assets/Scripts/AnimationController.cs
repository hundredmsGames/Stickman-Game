using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController
{
    void AnimationStart()
    {
        animator.SetFloat("state", (1f / 3f) * 2f);
    }

    void UpdateCharacterAnimations()
    {
        //IdleWalkRunAnim();
        //CharacterStateAnimations();


        if (AnimationChecker.Instance.animationEnded == true)
        {
            if (velocity.y > 5f)
            {
                animator.SetFloat("state", (1f / 3f));

                AnimationChecker.Instance.animationEnded = false;

                Debug.Log("jump");
            }
            //else if (velocity.y < 5f)
            //{
            //    animator.SetFloat("state", (1f / 3f) * 2f);

            //    AnimationChecker.Instance.animationEnded = false;

            //    Debug.Log("fall");

            //}
            else if((characterState & ~(GROUNDED | BOT_HOR_RAY | ANGLED_RAY)) == 0)
            {
                animator.SetFloat("state", 1f);

                AnimationChecker.Instance.animationEnded = false;

                Debug.Log("jump over");
            }
            else
            {
                animator.SetFloat("state", 0f);
            }
        }
    }

    public void IdleWalkRunAnim()
    {
        //decide the value 
        float normalizedVal = Nomalize(velocity.x, characterMinSpeed, characterMaxSpeed);
        animator.SetFloat("speed",normalizedVal);
    }

    public void CharacterStateAnimations()
    {
        //falling = jumping = grounded = false;

        //string binaryFormOfState = GetBinaryFormOfCharacterState();
        


        ////set animations
        //animator.SetBool("falling", falling);
    }
    

    public float Nomalize(float curr,float oldMin,float oldMax,float newMin=0,float newMax=1)
    {
        return ((curr - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
    }

}
