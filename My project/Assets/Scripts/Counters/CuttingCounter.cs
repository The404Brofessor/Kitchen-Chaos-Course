using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CuttingCounter : BaseCounter, IHasProgress
{

    public event EventHandler <IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;



    private int cuttingProgress;

    public override void Interact(Player player) //override allows us to utilize the "virtual" function in BaseCounter.cs to access the base method.
    {
        if (!HasKitchenObject())
        {
            //There is no KitchenObject here
            if (player.HasKitchenObject())
            //Player is carrying something
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }

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
            }
            else
            {
                //Player is NOT carrying anything
                this.GetKitchenObject().SetKitchenObjectParent(player);
                //this is the reverse of the above in order to allow the player to PICK UP the object from the counter IF it HAS a kitchen object already
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //There is a KitchenObject here AND it can be cut
            cuttingProgress++; //this translate's to cuttingProgress += 1, just in shorthand form. OR cuttingProgress = cuttingProgress +1

            OnCut?.Invoke(this, EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });

            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            
                GetKitchenObject().DestroySelf();

                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;
        #region Input Recognition code
        //foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        //{
        //    if (cuttingRecipeSO.input == inputKitchenObjectSO)
        //    {
        //        return true;
        //    }
        //}

        //return false;
        //This code is to stop crashes/null exceptions when placing objects that are NOT within the cuttingRecipeSO being "cut"
        #endregion
    }

    #region SOArray
    //the code below essentially means that we are telling our alternate interact key to "cut" an object, to first pull from our KitchenSO and determine what's being help, to then SEARCH for the matching code of that SO during the "cut" and output/generate the slices that are linked via unity with the output cuttingRecipeSO
    #endregion


    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }

    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;

    }

}
