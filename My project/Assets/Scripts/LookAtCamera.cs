using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private enum Mode //enum's are good for outlining a particular set of options/rules. Useful for "states"; jumping movement is different from swimming movement, etc.
    { 
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted,
    }

    [SerializeField] private Mode mode;

    private void LateUpdate() //LateUpdate runs AFTER all other "Updates"
    {
        switch (mode)
        {
            case Mode.LookAtInverted:
                transform.LookAt(Camera.main.transform); 
                break;

            case Mode.LookAt:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;

            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;

            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
        
    }


}
