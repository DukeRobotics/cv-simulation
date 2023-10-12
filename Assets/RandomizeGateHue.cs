using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeGateHue : MonoBehaviour
{
    public List<GameObject> gateParts = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject gatePart in gateParts) {
            gatePart.GetComponent<Renderer>().material.color = Random.ColorHSV(0f,1f,0f,1f,0.1f,0.9f,1f,1f);
        }
    }
}
