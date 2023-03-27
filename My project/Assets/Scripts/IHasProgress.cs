using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHasProgress
{

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
        #region normalizing
        //progressNormalized is going to automatically convert our "float" of KitchenObjectInput of "cutting" and pressing ALTInput 3 times (5 for cabbage) to cut one of our kitchenobjects and convert it to unity's slider,
        //for our progress bar that we created which runs with the 1.00 to 0.00 filling up our bar slider.
        #endregion
    }

}
