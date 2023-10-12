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

    float minExcludeZ = 0.0f;
    float maxExcludeZ = 0.0f;

    List<Vector3> startingPositions = new List<Vector3>();
    public int startingPositionIndex = 0;

    float nextUpdateTime = 0.0f;

    int interval;

    void Start () {
        secondsPerRotation = (360.0f - (4 * exclusionAngle)) / degreesPerSecond;

        targetPoint = new GameObject().transform;
        targetPoint.position = target.transform.position;

        for(float x = minX; x <= maxX + xIncrement/10; x += xIncrement){
            for(float z = minZ; z <= maxZ + zIncrement/10; z += zIncrement){
                for(float y = minY; y <= maxY + yIncrement/10; y += yIncrement){
                    startingPositions.Add(new Vector3(x, y, z));
                }
            }
        }

        interval = startingPositions.Count / (int) (maxX - minX + 1);
    }

    void Update () {
        if(autoPilot){
            if(Time.fixedTime >= nextUpdateTime){
                if(startingPositionIndex >= startingPositions.Count){
                    print("restart positions");
                    startingPositionIndex = 0;
                }

                print("startingPositionIndex: " + startingPositionIndex);

                Vector3 newPosition = startingPositions[startingPositionIndex];

                transform.position = newPosition;

                if(startingPositionIndex % interval == 0) {
                    targetPoint.position = new Vector3(newPosition.x, targetPoint.position.y, targetPoint.position.z);
                }

                maxExcludeZ = Math.Abs(newPosition.z) * (float) Math.Sin(exclusionAngle * (float) Math.PI / 180.0f);
                minExcludeZ = -maxExcludeZ;

                nextUpdateTime += secondsPerRotation;
                startingPositionIndex++;
            }

            if(transform.position.z > minExcludeZ && transform.position.z < maxExcludeZ) {
                if(Math.Abs(transform.position.z - minExcludeZ) < Math.Abs(transform.position.z - maxExcludeZ)) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, maxExcludeZ);
                }
                else {
                    transform.position = new Vector3(transform.position.x, transform.position.y, minExcludeZ);
                }
            }

            transform.RotateAround(targetPoint.position, Vector3.up, degreesPerSecond * Time.deltaTime);
            transform.LookAt(targetPoint);
        }
        else if(manualPilot){
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