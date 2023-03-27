using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

    [SerializeField] private Transform counterTopPoint;
    
    private KitchenObject kitchenObject; //this can be set to protected so that ClearCounter.cs and ContainerCounter.cs can access it as their own versions.

    public virtual void Interact(Player player) //virtual allows the method to be open to be overridden by inherited classes of this base class.
    {
        Debug.LogError("BaseCounter.Interact();");
    }
    public virtual void InteractAlternate(Player player) //virtual allows the method to be open to be overridden by inherited classes of this base class.
    {
        //Debug.LogError("BaseCounter.InteractAlternate();");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
