using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeGateGlyphs : MonoBehaviour
{
    public List<GameObject> frontGlyphs = new List<GameObject>();
    public List<GameObject> backGlyphs = new List<GameObject>();

    public List<string> labels = new List<string>();

    public List<Material> materials = new List<Material>();

    // Start is called before the first frame update
    void Start()
    {
        int firstMaterialIndex = Random.Range(0, 2);
        var renderer1 = frontGlyphs[0].GetComponent<Renderer>();
        var labeling1 = renderer1.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
        renderer1.material = materials[firstMaterialIndex];
        renderer1 = frontGlyphs[1].GetComponent<Renderer>();
        renderer1.material = materials[1 - firstMaterialIndex];
        labeling1.labels.Clear();
        labeling1.labels.Add(labels[firstMaterialIndex]);
        labeling1.RefreshLabeling();

        int secondMaterialIndex = Random.Range(0, 2);
        var renderer2 = backGlyphs[0].GetComponent<Renderer>();
        var labeling2 = renderer1.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
        renderer2.material = materials[secondMaterialIndex];
        renderer2 = backGlyphs[1].GetComponent<Renderer>();
        renderer2.material = materials[1 - secondMaterialIndex];
        labeling2.labels.Clear();
        labeling2.labels.Add(labels[1-firstMaterialIndex]);
        labeling2.RefreshLabeling();
    }

    // Update is called once per frame
    void Update()
    {
        int firstMaterialIndex = Random.Range(0, 2);
        var renderer1 = frontGlyphs[0].GetComponent<Renderer>();
        var labeling1 = renderer1.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
        renderer1.material = materials[firstMaterialIndex];
        renderer1 = frontGlyphs[1].GetComponent<Renderer>();
        renderer1.material = materials[1 - firstMaterialIndex];
        labeling1.labels.Clear();
        labeling1.labels.Add(labels[firstMaterialIndex]);
        labeling1.RefreshLabeling();

        int secondMaterialIndex = Random.Range(0, 2);
        var renderer2 = backGlyphs[0].GetComponent<Renderer>();
        var labeling2 = renderer1.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
        renderer2.material = materials[secondMaterialIndex];
        renderer2 = backGlyphs[1].GetComponent<Renderer>();
        renderer2.material = materials[1 - secondMaterialIndex];
        labeling2.labels.Clear();
        labeling2.labels.Add(labels[1-firstMaterialIndex]);
        labeling2.RefreshLabeling();
    }
}