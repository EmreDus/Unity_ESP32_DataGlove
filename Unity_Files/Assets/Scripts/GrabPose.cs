using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabPose : MonoBehaviour
{
    private float pose_serce, pose_yuzuk, pose_orta, pose_isaret, pose_bas;
    public GameObject GrabZone;
    public XRSocketInteractor socket;
    [SerializeField] private Rigidbody rb;
    [Space]
    public float pose_min = 30f;
    public float pose_max = 50f;
    [Space]
    public float pose_isaret_min = 8f;
    public float pose_isaret_max = 15f;
    public float pose_bas_min = 8f;
    public float pose_bas_max = 8f;


    void Start()
    {
        socket = GrabZone.GetComponent<XRSocketInteractor>();
        rb = GetComponent<Rigidbody>();
        socket.socketActive = false;
        GrabZone.GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        float pose_isaret = FingerCalib.Cal_isaret;
        float pose_orta = FingerCalib.Cal_orta;
        float pose_yuzuk = FingerCalib.Cal_yuzuk;
        float pose_serce = FingerCalib.Cal_serce;
        float pose_bas = FingerCalib.Cal_bas;

        if(pose_isaret >= pose_isaret_min && pose_isaret <= pose_isaret_max && pose_bas >= pose_bas_min && pose_bas <= pose_bas_max && pose_orta <= pose_max 
        && pose_orta >= pose_min && pose_yuzuk <= pose_max && pose_yuzuk >= pose_min && pose_serce <= pose_max && pose_serce >= pose_min){
            
            socket.socketActive = true;
            GrabZone.GetComponent<Renderer>().enabled = true; // for showing grab zone
            rb.constraints = RigidbodyConstraints.FreezeAll; // for freze the hand move
        }

        else{
            socket.socketActive = false;
            GrabZone.GetComponent<Renderer>().enabled = false;
            rb.constraints = RigidbodyConstraints.None;
        }
    }
}
