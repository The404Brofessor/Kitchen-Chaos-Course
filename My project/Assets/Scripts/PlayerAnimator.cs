using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is to create code to interact with transitions of the animator!

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    [SerializeField] Player player; //by serializing this field, the "Player" player will appear in the inspector of Unity to assign a variable to it (like the movement speed in player class under the animator)
    private Animator animator;

    private void Awake() //Awake is a unity call-back where it is awaiting a signal from Unity, where it will then call this code once Unity initiates the call to animate
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking()); //because this code requires (no way around it) a string we must create a const (constant) in order to allow for differing call functions to the Unity check box on "IsWalking"
                                             //this will be important because if we needed this to activate for a jump code, we would need to Utilize it in that jump code as well without the overlapping "move-while-jumping AND Idle to walk, codes operate"
                                             //We created another const for this in the player class (player.cs) in order to pull this data from unity and apply it to the code within this class (PlayerAnimator.cs) and then communicate with the method from player.cs to perform the function of if recieving movement input, do this.
    
    }

}
