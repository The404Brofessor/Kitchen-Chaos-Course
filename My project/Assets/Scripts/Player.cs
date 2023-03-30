using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

//Naming conventions and definitions https://youtu.be/0_UiF-4-7xM and https://www.youtube.com/watch?v=XtQMytORBmM

//definition:
//local = code, variables, etc. that only operate within the method itself. Cannot be called outside of the "local" method it was originally created within.
//field = "Class-wide" variables. Can be used within each method, can be set to private or public to determine method accessibility

public class Player : MonoBehaviour, IKitchenObjectParent
{
    
   

    //static means that it belongs to the "player-CLASS" itself instead of any "instances" of it (when using "new" keyword to pass in NEW information as a NEW instance of that class. IE. "GameInput" for multiple inputs from different sources. Gamepad, Keyboard, etc.
    //Keyboard "WASD" inputs would be the base template and take the information from the gamepad, to MAKE an INSTANCE of the GameInput for the Gamepad.
    public static Player Instance { get; private set; } //by setting the "instance" to public we allow the instance to be accessed and "read" but placing private on the "set" we do NOT allow outside code to "write-over" the instance.
                                                        //Applicable to player HP, where if P1 was poisoned, it would not ALSO affect P2 because the private set would not allow it to directly impact the base HP method, but could READ what that Max HP is.

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; //A type of event that comes with extra information data (obj/class sender of the event and event args [a class to be inherited to "pass-in" extra information added elsewhere])
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }
    #region
    //EventHandler sends information within it's confines (box); using "inheritance" (:) the returned "EventArgs" would inherit the characteristics(code) of the OnSelectedCounterEventArgs.
    //Lehman's Term: The countervisual will assume the characteristic's of the selectedCounter visual (the highlighted effect we modified in Unity to be roughly 1% larger than the original mesh and activate the highlight.
    #endregion
    #region notes
    //SerializeField (while under a private accessor) allows for modification of the values within the movement speed but does no allow for other methods to interact and change this unintentionally
    //Think "options menu" movement speed settings vs speed status effects
    #endregion
    [SerializeField] private float moveSpeed = 7f; //7f value isn't specific, but entails that the opted speed is 7 times what our standard inputVector.normalized values are. [float because it's dealing with decimals]
    [SerializeField] private GameInput gameInput;
    #region notes
    //Serialized fields show the variables in the inspector of unity (eg. GameInput, MoveSpeed)
    //GameInput can be made in the script of GameInput or in the Player script because both convert into the original input codes from our Player Script here. Up on D-pad = "W" key input.
    #endregion
    [SerializeField] private LayerMask countersLayerMask; //LayerMask is a function that interacts with casting forcing the cast only to recognize whatever components/obj. are within the "masked layer"
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player Instance"); //this line of code will allow us to know if there is multiple instances of whatever class is instanced as static. Because it's meant to be a singleton (only one instance) this code will let us know if there is more.
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        #region refactoring ex.
        //The code below is the exact same as the above 2 lines of code due to the code stored within private void HandleInteractions() acting as the framework and we are refactoring to the above code to "call" upon the already developed code.
        //Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        //Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        //if (moveDir != Vector3.zero)
        //{
        //    lastInteractDir = moveDir;
        //}
        //float interactDistance = 2f;
        //if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        //{
        //    if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
        //    {
        //        //Has ClearCounter
        //        clearCounter.Interact();
        //    }

        //}
        #endregion
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //Has ClearCounter
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }

            }
            else
            {
                SetSelectedCounter(null); //while this technically refactored, we still need to "pass-in" the information for it to inherit within the brackets as "null".
            }


        }
        else
        {
            SetSelectedCounter(null);
        }
        //Debug.Log(selectedCounter);
    }

    //We are cutting and pasting the input vectors of the keyboard, over to GameInput.sc script to a. keep clean code. b. have a seperate location to allow the refactoring to occur before any processes from this code line to interfere with its conversion. BRING THE NORMALISED INPUTVECTOR to KEEP SMOOTHING OF MOVEMENT VECTORS.
    private void HandleMovement()
    {
        #region notesinputcode
        //LOOK AT GAMEINPUT FROM NOW ON, THIS IS ONLY HERE FOR BASELINE CREATION, NO PORT CAPACITY HERE
        //Vector2 inputVector = new Vector2(0, 0);
        ////setting up key inputs for unity 3D vector spacing code via V.Studio coding in a 2D vector spacing
        //if (Input.GetKey(KeyCode.W))
        //    inputVector.y = +1;
        //if (Input.GetKey(KeyCode.S))
        //    inputVector.y = -1;
        //if (Input.GetKey(KeyCode.A))
        //   inputVector.x = -1;
        //if (Input.GetKey(KeyCode.D))
        //    inputVector.x = +1;
        #endregion
        #region notes
        //Developing and Refactoring Inputs:
        //Refactoring previous code's/methods (taking the input keys and porting them [plugging them into a different set of code for repurposing] to controller etc.) for other functions/porting
        //look to the "GameInput.cs" script in order to see the conversion process from keyboard inputs into other formats.
        #endregion

        //inputVector = inputVector.normalized;
        #region notes
        //this will make it so that the diagonal vector of movement is rounded down to be similar speed as if moving with only the one vector input. (Square rooting the added value of A and B for C)
        //pythagorean theorum's a2 + b2 = c2 (C is the diagonal vector and if a and b were added [pressing both buttons together], the player would move at a faster rate) 
        //inputVector.normalized essentially does this pythagorean theorum for us already.
        #endregion

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        #region notes
        //this line of code allows us to call on Vector2 as our gameInput.GetmovementVectorNormalized(); which was made in our GameInput.sc which can then translate to the below function properly.
        //Order of process ='s controller input into the GameInput script, translating over to our input's for the keyboard ---> then outputs to here where the below code then smooths the movements, rotates to the movements, etc.
        #endregion
        //we are creating vector3 (due to a 3D world vs 2D world grid-system we are going to pass along the Y data input from pressing "W" so that it will transfer onto the Z[up, down relative to cam. pos.] axis instead of the Y[up,down] (where it'd be flying/descending on "W" or "S")
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        #region notes
        //This is code is for collision detection within our 3D world: This Physics class is already pre-made within the unity engine which we are pulling from the top as they are now integrated.
        //bool canMove = !Physics.Raycast essentially states that if the laser detects an object, then the player can no longer move.
        #endregion

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f; //found this value by moving an object from 0 and moving it to the head of the model to see it was approximately 2 units on the Y axis
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        #region Raycasting
        //Raycasting is a tool that "shoots a laser out" from a specific location (origin defined by what's within the Physics.Raycast brackets), in a direction of your choosing based on what's set within its parameters.
        //Physics.cube or sphere or capsule, will give this laser a definitive shape whereas Raycast is a infinitely small outward line.
        //transform.position is unity's vector information of the obj., moveDir is the direction at which the laser is pointed, playerSize, is the length/max distance parameter for how far out the sensing laser reaches (radius of our player model).
        #endregion
        #region MovementNotes
        //we are going to use this code below to allow for the character to slide along an object if the Capsule cast detects an object, but will only disable the "forward" vector (z) while still attributing and allowing the x vector data input
        #endregion
        if (!canMove)
        {
            //cannot move towards moveDir
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;  //IF the player CANNOT (!) move due to the collider, this transposes the X value as the only vector that is registered matter currently and setting Y and Z to 0
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move only on the X

                //Attempt on Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //Can move only on the Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }
        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }
        #region notes 
        //Time.deltaTime is necessary because variability in the frame rates causing the capsule to move at different rates. This code effectively takes the difference in time (ms) between ONE frame to its next)
        //at 60 FPS, one frame = 1frame/60fps = 16.7ms , versus running at 144fps which would be 1frame/144fps = 6.9ms (you'd cover more distance in 60fps than 144)
        //Overall this is communicating that the position being moved by the obj. is taking into account the moveDir by the moveSpeed and the Time.deltaTime;
        #endregion
        #region notes
        //personal attempt to code in rotational data below:
        //if (moveDir == Vector3.zero)
        //{ 
        //        //another format for this could operate inversely and cleanly be coded as: if (moveDir != Vector3.zero) {transform.forward = moveDir}
        //}
        //else
        //{
        //    transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime);
        //    //this can be interpolated to and add Vector3.Lerp or Slerp (best for rotations) changing the data information into a scale Vector3.Slerp(Vector3 a, Vector3 b, float t)
        //                  //Think of Slerp as a circle being broken down into a straight line where A is the beginning and B is the end on a scale. t is the value along that scale.
        //                  //Lerp is simply a straight line in which it does not exhibit a spherical scale.
        //}

        //This is the clean short code from the video for above:
        #endregion

        isWalking = moveDir != Vector3.zero; //essentially stating that the character IS WALKING (animation) based on if the moveDir's vector isn't zero

        //now we must add speed of the rotation because as is, it is too slow
        float rotateSpeed = 10f; //10 is arbitrary but effectively means we are multiplying the rotate speed by 10 and must be applied to the Time.deltaTime

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

        //could use transform.lookat or tranform.eulerAngles as they function similarly . Using transform.right is similar in function but moreover better for 2D games, transform.forward works better in a 3D world because it will take the vector of Z and make it the models "true north" relative to the model and camera pos.

    }

    //^^ the entire method aboce can be taken and pulled from via "GameInput" to allow this method to be repurposed under the context of a controller due to it being public and separate from the "IsWalking" boolean

    public bool IsWalking()
    {
        return isWalking;
    }
    private void SetSelectedCounter(BaseCounter selectedCounter) //we are creating this to refactor the copied code above (to remove lengthy code and clean up repeat code, into a single method text.
    {
        this.selectedCounter = selectedCounter; //This code means that after this method is called, the new data overwrites the old selectedCounter class-wide data field. "this. =ORIGINAL CLASS Encompassing all methods within it [ours is "Player" at the top]"
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs //this code separates the ability for other code to change this method. This acts as the base to enable a subscriber to "listen" to what "OnSelectedCounterChangedEventArgs" is below.
        {
            selectedCounter = selectedCounter
        });
        //essentially this code means: The raycast will send a signal of interaction with the "selectedCounter" that information with then be passed to the "OnselectedCounterChangedEventArgs" if there is a counter detected, it will then take the code of the highlight mesh and pass it back via the EventArgs, to then turn ON that highlight if it is in fact detected as the current selected counter, if no counter is detected, then the highlight remains off.

    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) 
        { 
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}


