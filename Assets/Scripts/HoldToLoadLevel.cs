using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f;
    public Image fillPortal;

    private float holdTimer = 0;
    private bool isHolding = false;
   
    public static event Action OnHoldComplete;
    // Update is called once per frame
    void Update()
    {
       if (isHolding) {
        holdTimer += Time.deltaTime;
        fillPortal.fillAmount = holdTimer / holdDuration;
        if (holdTimer >= holdDuration) {
            //load next level
            OnHoldComplete.Invoke();
            ResetHold();
        }
       }    
    }

    public void onHold(InputAction.CallbackContext context) {
        if (context.started) {
            isHolding = true;
        } else if(context.canceled) {
            //reset holding
            ResetHold();
        }
    }

    private void ResetHold() {
        isHolding = false;
        holdTimer = 0;
        fillPortal.fillAmount = 0;
    }
}