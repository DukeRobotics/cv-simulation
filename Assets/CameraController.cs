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
    // float radius = 7.0f;
    // float thetaRadians = 3.0f * (float) Math.PI / 2.0f;
    // float thetaIncrementDegrees = 10.0f;
    // float thetaIncrementRadians = 10.0f * (float) Math.PI / 180.0f;

    public GameObject target;

    Transform targetPoint;

    // Vector3[] targets = {new Vector3(0, -2.35, 0), new Vector3(0, -1.35, 0), new Vector(0, -0.35, 0), new Vector(1, -2.35, 0), new Vector(2, -2.35, 0), new Vector(3, -2.35 ) }

    // z: -7 to -2, increments of 1
    // y: -3.8 to -0.8, increments of 1
    // x: 0 to 7, increments of 1

    public float degreesPerSecond = 72.0f;
    float secondsPerRotation;

    public float minZ = -7.0f;
    public float maxZ = -2.0f;
    public float zIncrement = 1.0f;

    public float minY = -3.8f;
    public float maxY = -0.8f;
    public float yIncrement = 1.0f;

    public float minX = 0.0f;
    public float maxX = 5.0f;
    public float xIncrement = 1.0f;

    public float exclusionAngle = 20.0f;

    // public float minExcludeZ = -1.0f;
    // public float maxExcludeZ = 1.0f;

    // float minExcludeZ = 0.0f;
    // float maxExcludeZ = 0.0f;

    List<Vector3> startingPositions = new List<Vector3>();
    public int startingPositionIndex = 0;

    float nextUpdateTime = 0.1f;

    int interval;

    public float minAngle = -90f;
    public float maxAngle = 90f;
    public float minDistance = 0f;
    public float maxDistance = 2f;

    // public float updateInterval = 0.1f;

    void Start () {
        // secondsPerRotation = (360.0f - (4 * exclusionAngle)) / degreesPerSecond;
        float x = UnityEngine.Random.Range(minBounds.x, maxBounds.x);
        float y = UnityEngine.Random.Range(minBounds.y, maxBounds.y);
        float z = UnityEngine.Random.Range(minBounds.z, maxBounds.z);
        Vector3 startingPos = new Vector3(x, y, z);
    }


    public Vector3 minBounds = new Vector3(-7.5f, -1.5f, -7.5f); // Replace with your calculated min values
    public Vector3 maxBounds = new Vector3(7.5f, 0.0f, 7.5f);   // Replace with your calculated max values

    public float updateInterval = 0.5f; // Time interval for position update

    /* ((x, y, z), (row, pitch, yaw))
     *  We need all 6dof because we vary all of them
     * TODO: figure out how to set camera angles manually
    */
    public List<(Vector3, Vector3)> cameraPos = new List<(Vector3, Vector3)>();

    // how much we can vary row/pitch/yaw (while still keeping target in frame)
    // should be a function of the distance from the target (squared?)
    // we probably need to do some math for this but ugh
    public float distanceFromTarget;


    // secondsPerRotation = 0.1f;

    void Update () {
    secondsPerRotation = 0.1F;

    if (Time.fixedTime >= nextUpdateTime) {
        // Generate a random position within bounds
        float x = UnityEngine.Random.Range(minBounds.x, maxBounds.x);
        float y = UnityEngine.Random.Range(minBounds.y, maxBounds.y);
        float z = UnityEngine.Random.Range(minBounds.z, maxBounds.z);
        Vector3 randomPosition = new Vector3(x, y, z);

        // Set the camera's position
        transform.position = randomPosition;
        
        // Initially orient camera towards the target
        transform.LookAt(target.transform);

        // Apply random deviation
        float horizontalDeviation = UnityEngine.Random.Range(-30f, 30f); // Adjust deviation range as needed
        float verticalDeviation = UnityEngine.Random.Range(-30f, 30f); // Adjust deviation range as needed
        transform.Rotate(Vector3.up, horizontalDeviation, Space.World);
        transform.Rotate(Vector3.right, verticalDeviation, Space.Self);

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