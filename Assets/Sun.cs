using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public bool enableRandomRotation = true;
    public float zFixed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enableRandomRotation)
        {
            float randomRotationDegX = Random.Range(30, 150);
            float randomRotationDegY = Random.Range(30, 150);
            transform.localEulerAngles = new Vector3(randomRotationDegX, randomRotationDegY, zFixed);
        }
    }
}
