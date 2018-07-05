using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChecker : MonoBehaviour {

    public static AnimationChecker Instance;
    public bool JumpOverAnimationEnded { get; set; }
                                       // Use this for initialization
    void Start () {
        if (Instance != null)
            return;
        Instance = this;
        JumpOverAnimationEnded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
   
    public void JumpOverObstacleAnimationStarted()
    {
        JumpOverAnimationEnded = false;
        Debug.Log("JumpOverObstacleAnimationStarted");
    }
    public void JumpOverObstacleAnimationEnded()
    {
        JumpOverAnimationEnded = true;
        Debug.Log("JumpOverObstacleAnimationEnded");

    }
}
