using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounter : MonoBehaviour
{
    //singleton pattern inbound because we don't want to constantly "scan" with every frame

    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start() //Awake goes before start, therefore we want this method to be called from our Player.cs AFTER unity has read the code calling this method to activate.
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter) //e. opens the attached script within Unity's obj. (this case is a counter) and compares it with our current Player scripted clearCounter obj.
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    { 
        foreach (GameObject visualGameObject in visualGameObjectArray) 
        { 
            visualGameObject.SetActive(true);
        }
         
    }

    private void Hide()
    { 
        foreach (GameObject visualGameObject in visualGameObjectArray) 
        { 
            visualGameObject.SetActive(false); 
        }
    }
}
