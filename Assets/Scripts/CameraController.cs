using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Controller controller;

    // Camera Angle
    public float mouseSense = 2.0f;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;

    private Vector3 cameraPlayerOffset;

    public void ResetRotation() { transform.eulerAngles = new Vector3(0f, 0f, 0f); }

    void Start()
    {
        // Retrieve Controller
        GameObject temp = GameObject.Find("Players");
        controller = temp.transform.GetComponent<Controller>() as Controller;

        
    }
    
    void Update()
    {
        // Set Distance From Player
        cameraPlayerOffset.z = -5;
        cameraPlayerOffset.y = 2;
        transform.position = controller.getCurrentObject().transform.position + cameraPlayerOffset;

        // Get Mouse Movement
        mouseX += mouseSense * Input.GetAxis("Mouse X");
        mouseY -= mouseSense * Input.GetAxis("Mouse Y");

        // Angle Camera Based on Mouse Movement and Sense
        transform.eulerAngles = new Vector3(mouseY, mouseX, 0.0f);

    }
    
    
}
