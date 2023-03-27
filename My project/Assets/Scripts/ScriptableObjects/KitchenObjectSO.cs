using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
//This isn't a Monobehavior as we are creating this to be a data container for future inputs/methods for multiple kitchenObjects within Unity that will only be READ from and not written to.
//Monobehavior would allow us to destroyObject; etc. (writing to the code).
public class KitchenObjectSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
