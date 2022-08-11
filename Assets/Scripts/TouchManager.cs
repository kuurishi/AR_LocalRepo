using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;

public class TouchManager : MonoBehaviour
{
    
    public GameManager gameManager;


    private void OnEnable()
    {
        LeanTouch.OnFingerTap += OnTap;
    }


    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnTap;
    }


    private void OnTap(LeanFinger _finger)
    {

        Debug.Log("tapped with finger" + _finger.Index + "at" + _finger.ScreenPosition);
        
        //gameManager.OnTap(_finger);

    }




}
