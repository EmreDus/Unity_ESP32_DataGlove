using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalibText : MonoBehaviour
{
    [SerializeField] private Text calibrationText;
    [SerializeField] private Text calibrationText2;

    private float timer = 10.5f;
    private float sec = 1.0f;

    void Update()
    {
        bool isPressed = FingerCalib.isButtonPressed;
        
        calibrationText.text = " ";
        calibrationText2.text = " ";

        if(isPressed)
        {
            timer -= sec * Time.deltaTime;
            if (timer < 0.0f)
            {
                timer = 0.0f;
            }

            if(timer > 8.0f)
            {
                calibrationText2.text = "CALIBRATION STARTED";
            }

            else if(timer > 5.5f) 
            {   
                calibrationText.text = "Open Your Hand";
            }

            else if(timer > 0.5f) 
            {
                calibrationText.text = "Close Your Hand";
            }

            else if(timer > 0.4f){
                calibrationText2.text = "Calibration Completed";
            }
        }

        else if(!isPressed)
        {
            calibrationText.text = " ";
            calibrationText2.text = " ";
            timer = 10.5f;
        }

    }

}
