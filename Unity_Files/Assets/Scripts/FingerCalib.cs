using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class FingerCalib : MonoBehaviour
{

    public static float Cal_isaret, Cal_orta, Cal_yuzuk, Cal_serce, Cal_bas;
    public float Mul_isaret, Mul_orta, Mul_yuzuk, Mul_serce, Mul_bas;
    private static float Min_isaret=0, Min_orta=0, Min_yuzuk=0, Min_serce=0, Min_bas=0;
    private static float Max_isaret=50, Max_orta=50, Max_yuzuk=50, Max_serce=50, Max_bas=50;
    private float _bas, _isaret, _orta, _yuzuk, _serce;
    public Transform index1, index2, index3, middle1, middle2, middle3, ring1, ring2, ring3, pinky0, pinky1, pinky2, pinky3, thumb1, thumb2, thumb3;

    public static bool isButtonPressed;
    
    private float timer = 10.5f;
    private float sec = 1.0f;

    void Update()
    {
        float _bas = ESP32_com.bas;
        float _isaret = ESP32_com.isaret;
        float _orta = ESP32_com.orta;
        float _yuzuk = ESP32_com.yuzuk;
        float _serce = ESP32_com.serce;

        index1.transform.localRotation = Quaternion.Euler(-74, 106, 61-(Cal_isaret*Mul_isaret));
        index2.transform.localRotation = Quaternion.Euler(0, 0, index1.transform.localRotation.z - (Cal_isaret*Mul_isaret));
        index3.transform.localRotation = Quaternion.Euler(0, 0, index2.transform.localRotation.z - (Cal_isaret*Mul_isaret));
                        
        middle1.transform.localRotation = Quaternion.Euler(-80, 18, 151-(Cal_orta*Mul_orta));
        middle2.transform.localRotation = Quaternion.Euler(0, 0, middle1.transform.localRotation.z - (Cal_orta*Mul_orta));
        middle3.transform.localRotation = Quaternion.Euler(0, 0, middle2.transform.localRotation.z - (Cal_orta*Mul_orta));

        ring1.transform.localRotation = Quaternion.Euler(-69, -21, -169-(Cal_yuzuk*Mul_yuzuk));
        ring2.transform.localRotation = Quaternion.Euler(0, 0, ring1.transform.localRotation.z - (Cal_yuzuk*Mul_yuzuk));
        ring3.transform.localRotation = Quaternion.Euler(0, 0, ring2.transform.localRotation.z - (Cal_yuzuk*Mul_yuzuk));

        pinky0.transform.localRotation = Quaternion.Euler(-50, -26, 26);
        pinky1.transform.localRotation = Quaternion.Euler(-1, -2, -175-(Cal_serce*Mul_serce));
        pinky2.transform.localRotation = Quaternion.Euler(0, 0, pinky1.transform.localRotation.z - (Cal_serce*Mul_serce));
        pinky3.transform.localRotation = Quaternion.Euler(0, 0, pinky2.transform.localRotation.z );
                        
        thumb1.transform.localRotation = Quaternion.Euler(9, 156, 27-(Cal_bas*Mul_bas));
        thumb2.transform.localRotation = Quaternion.Euler(0, 0, thumb1.transform.localRotation.z - (Cal_bas*Mul_bas));
        thumb3.transform.localRotation = Quaternion.Euler(0, 0, thumb2.transform.localRotation.z - (Cal_bas*Mul_bas));


        if(isButtonPressed){
            timer -= sec * Time.deltaTime;
            //Debug.Log("Time: " + timer);
            if (timer < 0.0f){
                timer = 0.0f;
            }

            if(timer > 5.5f) {   
                Debug.Log("Open your hand");
                // Gather min values
                Min_isaret = _isaret;
                Min_orta = _orta;
                Min_yuzuk = _yuzuk;
                Min_serce = _serce;
                Min_bas = _bas;              
            }

            else if(timer > 0.5f) {
                Debug.Log("Close your hand");
                // Gather max values  
                Max_isaret = _isaret;
                Max_orta = _orta;
                Max_yuzuk = _yuzuk;
                Max_serce = _serce;
                Max_bas = _bas;         
            } 
                
            else if(timer > 0.0f){
                Debug.Log("Calibration complete!"); 
                isButtonPressed = false;  
            }
        }


        Cal_isaret = Mathf.Clamp(_isaret, Min_isaret, Max_isaret); 
        Cal_orta = Mathf.Clamp(_orta, Min_orta, Max_orta);
        Cal_yuzuk = Mathf.Clamp(_yuzuk, Min_yuzuk, Max_yuzuk);
        Cal_serce = Mathf.Clamp(_serce, Min_serce, Max_serce);
        Cal_bas = Mathf.Clamp(_bas, Min_bas, Max_bas);

    }

    public void ButtonPressed()
    {
        isButtonPressed = true;
        timer = 10.5f;
    }
}
