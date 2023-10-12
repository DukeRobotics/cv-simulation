using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RotateGlyphs : MonoBehaviour
{

    public List<GameObject> glyphs1 = new List<GameObject>();
    public float yFixed1 = 90.0f;
    public float zFixed1 = 90.0f;

    public List<GameObject> glyphs2 = new List<GameObject>();
    public float yFixed2 = -90.0f;
    public float zFixed2 = 90.0f;

    public List<Material> materials = new List<Material>();
    public List<string> labels = new List<string>();

    public List<MaterialLabel> materialLabels = new List<MaterialLabel>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < materials.Count; i++) {
            materialLabels.Add(new MaterialLabel { label = labels[i], material = materials[i] });
        }
    }

    // Update is called once per frame
    void Update()
    {
        var rnd = new System.Random();

        materialLabels = materialLabels.OrderBy(x => rnd.Next(50)).ToList();
        int index = 0;

        foreach(GameObject glyph in glyphs1) {
            float randomRotationDeg = UnityEngine.Random.Range(0, 360);
            glyph.transform.localEulerAngles = new Vector3(randomRotationDeg, yFixed1, zFixed1);

            glyph.GetComponent<Renderer>().material = materialLabels[index].material;

            var labeling = glyph.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
            labeling.labels.Clear();
            labeling.labels.Add(materialLabels[index].label);
            labeling.RefreshLabeling();

            index++;
        }

        materialLabels = materialLabels.OrderBy(x => rnd.Next(50)).ToList();
        index = 0;

        foreach(GameObject glyph in glyphs2) {
            float randomRotationDeg = UnityEngine.Random.Range(0, 360);
            glyph.transform.localEulerAngles = new Vector3(randomRotationDeg, yFixed2, zFixed2);

            glyph.GetComponent<Renderer>().material = materialLabels[index].material;

            var labeling = glyph.GetComponent<UnityEngine.Perception.GroundTruth.LabelManagement.Labeling>();
            labeling.labels.Clear();
            labeling.labels.Add(materialLabels[index].label);
            labeling.RefreshLabeling();

            index++;
        }
    }
}

public class MaterialLabel {
    public string label { get; set; }
    public Material material { get; set; }
}
