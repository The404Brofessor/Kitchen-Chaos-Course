using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter //this means as it is not just Monobehaviour
{
   
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) //override allows us to utilize the "virtual" function in BaseCounter.cs to access the base method.
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject here
            if (player.HasKitchenObject())
            //Player is carrying something
            {
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else 
            { 
                //Player not carrying anything
            }
        }
        else 
        {
            //there is a KitchenObject here
            if (player.HasKitchenObject())
            {
                //player IS Carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObjects plateKitchenObjects))
                {
                    //Player is Holding a Plate
                    if (plateKitchenObjects.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();                        
                    }                   
                }
                else
                {
                    //Player is not carrying a Plate but something else
                    if (GetKitchenObject().TryGetPlate(out PlateKitchenObjects plateKitchenObject))
                    {
                        //counter IS holding a Plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            { 
                //Player is NOT carrying anything
                  this.GetKitchenObject().SetKitchenObjectParent(player);
                //this is the reverse of the above in order to allow the player to PICK UP the object from the counter IF it HAS a kitchen object already
            }
        }
    }


}
