using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour
{
    public CharacterController chrController;

    public void JumpingAnimationEnded()
    {
        chrController.JumpingAnimationEnded();
    }

    public void JumpOverAnimationEnded()
    {
        chrController.JumpOverAnimationEnded();
    }

    public void FallingAnimationEnded()
    {
        chrController.FallingAnimationEnded();
    }
}
