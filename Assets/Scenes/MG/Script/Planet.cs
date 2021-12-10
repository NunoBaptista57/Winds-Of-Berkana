using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back};
    public FaceRenderMask faceRenderMask;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    PlanetFace[] terrainFaces;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    ShapeGenerator shapeGenerator;
    [HideInInspector]
    public bool shapeSettingsFoldout;
    [HideInInspector]
    public bool colorSettingsFoldout;

    private void OnValidate()
    {
        Initialize();
        GeneratePlanet();
    }

    void Initialize()
    {
        shapeGenerator = new ShapeGenerator(shapeSettings);

        if (meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];
        
        terrainFaces = new PlanetFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("HDRP/Lit"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();

                
            }
            terrainFaces[i] = new PlanetFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateColors();
        }
    }

    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialize();
            GenerateMesh();
        }
    }

    void GenerateMesh()
    {
        for(int i = 0; i<6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
                terrainFaces[i].ConstructMesh();
        }


        foreach(PlanetFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }


    void GenerateColors()
    {
        foreach(var m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.planetColor;
        }
    }


    void Update()
    {
        //this.transform.Rotate(new Vector3(0.0f, 0.0f, 1.0f), 0.1f);
       
        this.shapeSettings.noiseLayers[2].noiseSettings.simpleNoiseSettings.centre.x += 0.01f;

        //this.shapeSettings.noiseLayers[0].noiseSettings.simpleNoiseSettings.baseRoughness += 0.01f;

        OnShapeSettingsUpdated();

        /*foreach (var n in this.shapeSettings.noiseLayers)
        {
            n.noiseSettings.simpleNoiseSettings.centre.x += 0.1f;
        }*/


    }
}
