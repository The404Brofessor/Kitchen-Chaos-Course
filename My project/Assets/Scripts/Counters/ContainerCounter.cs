using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabbedObject; //this is to create an even to open and close the lid of the container

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Player is NOT carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
            #region oldKitchenobjectinfoinheritence
            //kitchenObjectTransform.localPosition = Vector3.zero;
            //kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>(); // this was compressed into the last line of code, essentially compressing the code. As we no longer need to determine the KitchenObject as the new code being written over the KitchenObject, will have been done for us.
            #endregion
            #region notes .localposition
            //.localPosition is setting a new standardized position in regards to the parent object rather than the global 3D origin within its space (in Unity's 3D engine).
            #endregion

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }


}
