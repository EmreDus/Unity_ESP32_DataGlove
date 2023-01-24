using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public GameObject[] objectsToReset;
    private Vector3[] originalPositions;
    
    private float maxVelocity = 1.0f;

    void Start()
    {
        originalPositions = new Vector3[objectsToReset.Length];
        for (int i = 0; i < objectsToReset.Length; i++){
            originalPositions[i] = objectsToReset[i].transform.position;
        }
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            if(objectsToReset[i] != null)
            {
                Rigidbody rb = objectsToReset[i].GetComponent<Rigidbody>();
                if(rb != null)
                {
                    if (rb.velocity.magnitude > maxVelocity)
                    {
                        rb.velocity = rb.velocity.normalized * maxVelocity;
                    }
                }
            }
        }
    }

    public void onPressed()
    {
        for (int i = 0; i < objectsToReset.Length; i++)
        {
            objectsToReset[i].transform.position = originalPositions[i]; // Reset positions
            objectsToReset[i].transform.localEulerAngles = Vector3.zero; // Reset rotations

            Rigidbody rb = objectsToReset[i].GetComponent<Rigidbody>(); // Reset physics
            if(rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
        }
    }
}
