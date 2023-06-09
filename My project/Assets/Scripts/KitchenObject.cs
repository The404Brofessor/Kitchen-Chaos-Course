using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    private IKitchenObjectParent kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO()
    {
        return kitchenObjectSO;
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (this.kitchenObjectParent != null) 
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        this.kitchenObjectParent = kitchenObjectParent;

        if (this.kitchenObjectParent.HasKitchenObject()) 
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
        }
        this.kitchenObjectParent.SetKitchenObject(this);

        transform.parent = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }

    public IKitchenObjectParent GetKitchenObjectParent() 
    {
        return kitchenObjectParent;
    }
    //another way to code lines 11, 18-24 in one line would be public ClearCounter ClearCounter {get; set;}

    public void DestroySelf() 
    { 
        kitchenObjectParent.ClearKitchenObject() ;
        Destroy(gameObject);
    }

    public bool TryGetPlate(out PlateKitchenObjects plateKitchenObjects) 
    { 
        if (this is PlateKitchenObjects)   
        { 
            plateKitchenObjects = this as PlateKitchenObjects;
            return true;
        }
        else 
        { 
            plateKitchenObjects = null;
            return false; 
        }
    }

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) 
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

}
