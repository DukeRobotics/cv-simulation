using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeBoxes : MonoBehaviour
{
    public List<GameObject> boxes1 = new List<GameObject>();
    public List<GameObject> boxes2 = new List<GameObject>();

    public List<Material> materials = new List<Material>();
    // Start is called before the first frame update
    void Start()
    {
        int firstMaterialIndex = Random.Range(0, 2);
        var renderer1 = boxes1[0].GetComponent<Renderer>();
        renderer1.material = materials[firstMaterialIndex];
        renderer1 = boxes1[1].GetComponent<Renderer>();
        renderer1.material = materials[1 - firstMaterialIndex];

        int secondMaterialIndex = Random.Range(0, 2);
        var renderer2 = boxes2[0].GetComponent<Renderer>();
        renderer2.material = materials[secondMaterialIndex];
        renderer2 = boxes2[1].GetComponent<Renderer>();
        renderer2.material = materials[1 - secondMaterialIndex];
    }

    // Update is called once per frame
    void Update()
    {
        int firstMaterialIndex = Random.Range(0, 2);
        var renderer1 = boxes1[0].GetComponent<Renderer>();
        renderer1.material = materials[firstMaterialIndex];
        renderer1 = boxes1[1].GetComponent<Renderer>();
        renderer1.material = materials[1 - firstMaterialIndex];

        int secondMaterialIndex = Random.Range(0, 2);
        var renderer2 = boxes2[0].GetComponent<Renderer>();
        renderer2.material = materials[secondMaterialIndex];
        renderer2 = boxes2[1].GetComponent<Renderer>();
        renderer2.material = materials[1 - secondMaterialIndex];
    }
}
