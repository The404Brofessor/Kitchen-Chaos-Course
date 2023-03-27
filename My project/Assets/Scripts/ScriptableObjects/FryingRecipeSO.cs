using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()] //This will create within Unity, the CuttingRecipeSO creation option
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float fryingTimerMax; //unity already has a system in place to set the "max" within the unity framework without having to hard code the "max and mins" into C#`
}
