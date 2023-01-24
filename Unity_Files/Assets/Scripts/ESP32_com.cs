using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;


public class ESP32_com : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM3", 9600);
    
    public string strReceived; 
    private string[] strData = new string[9];
    public string[] strData_received = new string[9];
    public static float qw, qx, qy, qz, serce, yuzuk, orta, isaret, bas;
    //public Transform hand, index1, index2, index3, middle1, middle2, middle3, ring1, ring2, ring3, pinky0, pinky1, pinky2, pinky3, thumb1, thumb2, thumb3;
    
    void Start()
    {
        stream.Open();  
        stream.ReadTimeout = 5000;  
    }

    void Update()
    {
        if (stream.IsOpen){
            try{
                strReceived = stream.ReadLine();  
                strData = strReceived.Split(','); 

                if (strData[0] != "" && strData[1] != "" && strData[2] != "" && strData[3] != "" && strData[4] != "" 
                && strData[5] != "" && strData[6] != "" && strData[7] != ""  && strData[8] != "")
                {
                    strData_received[0] = strData[0];
                    strData_received[1] = strData[1];
                    strData_received[2] = strData[2];
                    strData_received[3] = strData[3];
                    strData_received[4] = strData[4];
                    strData_received[5] = strData[5];
                    strData_received[6] = strData[6];
                    strData_received[7] = strData[7];
                    strData_received[8] = strData[8];
                    
                    //El Hareketi: 
                    qw = float.Parse(strData_received[0]);
                    qx = float.Parse(strData_received[1]); 
                    qy = float.Parse(strData_received[2]); 
                    qz = float.Parse(strData_received[3]); 

                    //hand.transform.rotation = new Quaternion(-qy, -qz, qx, qw);
                    
                    //Parmak Hareketleri:
                    serce = float.Parse(strData_received[4]);
                    yuzuk = float.Parse(strData_received[5]);
                    orta = float.Parse(strData_received[6]);
                    isaret = float.Parse(strData_received[7]);  
                    bas = float.Parse(strData_received[8]);

                    //isaret joints: index1 , index2 , index3
                    //orta joints: middle1 , middle2 ,  middle3
                    //yuzuk joints: ring1 , ring2 , ring3
                    //serce joints: pinky0 , pinky1 , pinky2 , pinky3
                    //bas joints: thumb1 , thumb2 , thumb3 
                    /*
                    index1.transform.localRotation = Quaternion.Euler(-74, 106, 61-isaret);
                    index2.transform.localRotation = Quaternion.Euler(0, 0, index1.transform.localRotation.z - isaret);
                    index3.transform.localRotation = Quaternion.Euler(0, 0, index2.transform.localRotation.z - isaret);
                    
                    middle1.transform.localRotation = Quaternion.Euler(-80, 18, 151-orta);
                    middle2.transform.localRotation = Quaternion.Euler(0, 0, middle1.transform.localRotation.z - orta);
                    middle3.transform.localRotation = Quaternion.Euler(0, 0, middle2.transform.localRotation.z - orta);

                    ring1.transform.localRotation = Quaternion.Euler(-69, -21, -169-yuzuk);
                    ring2.transform.localRotation = Quaternion.Euler(0, 0, ring1.transform.localRotation.z - yuzuk);
                    ring3.transform.localRotation = Quaternion.Euler(0, 0, ring2.transform.localRotation.z - yuzuk);

                    pinky0.transform.localRotation = Quaternion.Euler(-50, -26, 26);
                    pinky1.transform.localRotation = Quaternion.Euler(-1, -2, -175-serce);
                    pinky2.transform.localRotation = Quaternion.Euler(0, 0, pinky1.transform.localRotation.z - serce);
                    pinky3.transform.localRotation = Quaternion.Euler(0, 0, pinky2.transform.localRotation.z );
                    
                    thumb1.transform.localRotation = Quaternion.Euler(9, 156, 27-bas);
                    thumb2.transform.localRotation = Quaternion.Euler(0, 0, thumb1.transform.localRotation.z - bas);
                    thumb3.transform.localRotation = Quaternion.Euler(0, 0, thumb2.transform.localRotation.z - bas);
                    */
                }

            }
            catch (Exception){
                Debug.Log("We have a problem");
            }
        }        
    }
}
