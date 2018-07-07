using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour {

    public static AnimationChecker Instance;
    public bool JumpOverAnimationEnded { get; set; }
    public bool animationEnded;

    // Use this for initialization
    void Start ()
    {
        if (Instance != null)
            return;

        Instance = this;
        JumpOverAnimationEnded = false;

        animationEnded = false;
    }

    public void AnimationEnded()
    {
        animationEnded = true;
        Debug.Log("animation ended");
    }
   
    public void JumpOverObstacleAnimationStarted()
    {
        JumpOverAnimationEnded = false;
        //Debug.Log("JumpOverObstacleAnimationStarted");
    }
    public void JumpOverObstacleAnimationEnded()
    {
        JumpOverAnimationEnded = true;
        //Debug.Log("JumpOverObstacleAnimationEnded");

    }
}
