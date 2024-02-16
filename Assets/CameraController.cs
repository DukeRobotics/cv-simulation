using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/*
    Adapted from the following script by Windexglow 11-13-10.
    https://gist.github.com/gunderson/d7f096bd07874f31671306318019d996
*/

public class FlyCamera : MonoBehaviour {

    public bool manualPilot = false;
    float mainSpeed = 10.0f;
    float camSens = 0.25f;
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun= 1.0f;

    public bool autoPilot = false;

    public GameObject target;

    Transform targetPoint;

    public float secondsPerRotation = 0.01666667f;

    public float nextUpdateTime = 0.1f;


    void Start () {
        float x = UnityEngine.Random.Range(minBounds.x, maxBounds.x);
        float y = UnityEngine.Random.Range(minBounds.y, maxBounds.y);
        float z = UnityEngine.Random.Range(minBounds.z, maxBounds.z);
        Vector3 startingPos = new Vector3(x, y, z);
    }
    public float minRadius = 0.5f;     // trial-and-error
    public float maxRadius = 5f;       // trial-and-error
    public float minPolarAngle = 45f;  // To avoid direct top view
    public float maxPolarAngle = 135f; // To avoid direct bottom view
    public float maxYawAngle = 20f;    // trial-and-error
    public float minYawAngle = -20f;   // trial-and-error
    public float maxPitchAngle = 20f;  // trial-and-error
    public float minPitchAngle = -20f; // trial-and-error 
    public float maxRollAngle = 20f;   // trial-and-error
    public float minRollAngle = -20f;  // trial-and-error

    public Vector3 minBounds = new Vector3(-7.5f, -2.5f, -7.5f); // Replace with your calculated min values
    public Vector3 maxBounds = new Vector3(7.5f, 0.75f, 7.5f);   // Replace with your calculated max values

    void Update () {
        if (Time.fixedTime >= nextUpdateTime) {
            // Generate a random position within bounds
            // Generate random polar coordinates
            float radius = UnityEngine.Random.Range(minRadius, maxRadius);
            float polarAngle = UnityEngine.Random.Range(minPolarAngle, maxPolarAngle) * Mathf.Deg2Rad; // Convert to radians
            
            int flip = UnityEngine.Random.Range(0,2);

            // Convert polar to Cartesian coordinates
            float x = radius * Mathf.Sin(polarAngle);
            float z = radius * Mathf.Cos(polarAngle);
            float y = UnityEngine.Random.Range(minBounds.y, maxBounds.y);

            Vector3 randomPosition = new Vector3(x, y, z);

            // Set the camera's position
            transform.position = randomPosition;
            
            // Initially orient camera towards the target
            transform.LookAt(target.transform);

            // Apply random deviation for yaw (horizontal) and pitch (vertical) and roll
            float horizontalDeviation = UnityEngine.Random.Range(minYawAngle, maxYawAngle); // Yaw deviation
            float verticalDeviation = UnityEngine.Random.Range(minPitchAngle, maxPitchAngle); // Pitch deviation
            float rollDeviation = UnityEngine.Random.Range(minRollAngle, maxRollAngle); // Roll deviation

            transform.Rotate(Vector3.up, horizontalDeviation, Space.World); // Yaw
            transform.Rotate(Vector3.right, verticalDeviation, Space.Self); // Pitch
            transform.Rotate(Vector3.forward, rollDeviation, Space.Self); // Roll

            // Schedule the next update
            nextUpdateTime += secondsPerRotation;
        }

        if(manualPilot){
            manualPilotMove();
        }
    }

    private void manualPilotMove() {
        if (Input.GetMouseButton(0)){
                lastMouse = Input.mousePosition - lastMouse ;
                lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
                lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x , transform.eulerAngles.y + lastMouse.y, 0);
                transform.eulerAngles = lastMouse;
                lastMouse =  Input.mousePosition;
            }

            Vector3 p = GetBaseInput();
            if (p.sqrMagnitude > 0){
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;

                p = p * Time.deltaTime;
                Vector3 newPosition = transform.position;
                transform.Translate(p);
            }
    }

    private Vector3 GetBaseInput() {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            p_Velocity += new Vector3(0, 0 , 1);
        }
        if (Input.GetKey (KeyCode.S)){
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey (KeyCode.Space)){
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey (KeyCode.LeftShift)){
            p_Velocity += new Vector3(0, -1, 0);
        }
        return p_Velocity;
    }
}