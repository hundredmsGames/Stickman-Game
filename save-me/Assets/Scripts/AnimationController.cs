using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CharacterController
{
    void UpdateCharacterAnimations()
    {
        animator.SetBool("isJumping", jumping);
        animator.SetBool("isRunning", grounded);
        animator.SetBool("isFalling", falling);
        animator.SetBool("isJumpOver", jumpOver);
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

        GUI.Label(new Rect(0, 00, 100, 100), "Running :" + grounded.ToString(), new GUIStyle() { fontSize = 16 });
        GUI.Label(new Rect(0, 20, 100, 100), "Jumping :" + jumping.ToString(), new GUIStyle() { fontSize = 16 });
        GUI.Label(new Rect(0, 40, 100, 100), "Falling    :" + falling.ToString(), new GUIStyle() { fontSize = 16 });
        GUI.Label(new Rect(0, 60, 100, 100), "Grounded   :" + grounded.ToString(), new GUIStyle() { fontSize = 16 });
        GUI.Label(new Rect(0, 80, 100, 100), "JumpOver   :" + jumpOver.ToString(), new GUIStyle() { fontSize = 16 });

    }
}
