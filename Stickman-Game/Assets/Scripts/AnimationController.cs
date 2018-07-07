using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController  {

    public void IdleWalkRunAnim()
    {
        //decide the value 
        float normalizedVal = Nomalize(velocity.x, characterMinSpeed, characterMaxSpeed);
        animator.SetFloat("speed",normalizedVal);
    }
    public void CharacterStateAnimations()
    {
        falling = jumping = grounded = false;

        string binaryFormOfState = GetBinaryFormOfCharacterState();
        


        //set animations
        animator.SetBool("falling", falling);
    }
    void UpdateCharacterAnimations()
    {
        IdleWalkRunAnim();
        CharacterStateAnimations();
    }

    public float Nomalize(float curr,float oldMin,float oldMax,float newMin=0,float newMax=1)
    {
        return ((curr - oldMin) / (oldMax - oldMin)) * (newMax - newMin) + newMin;
    }

}
