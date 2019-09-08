using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFormer : MonoBehaviour
{
    public int nDivision;
    public float terrainSize;
    Vector3[] vert;

    // Use this for initialization
    void Start()
    {
      
        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();

        float divisionSize = terrainSize / (nDivision * nDivision);
        int nVert = nDivision + 1;

        Vector2[] uv;
        int[] tri;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vert = new Vector3[(nVert) * (nVert)];
        uv = new Vector2[(nVert) * (nVert)];
        tri = new int[(nVert) * (nVert) * 6];

        float halfSize = 0.5f * terrainSize;
        float floatDivision = (float)nDivision;
        float uvInterval = 1 / (floatDivision + 1);
        for (int i = 0; i < ((nVert) * (nVert)); i++)
        {
            vert[i] = new Vector3((divisionSize * (i % (nVert))), 0.0f, -(divisionSize * (i / (nVert))));
            uv[i] = new Vector2((i % (nVert)) * uvInterval, (i / (nVert)) * uvInterval);
        }

        int topLeft;
        int topRight;
        int botLeft;
        int botRight;
        int triCount = 0;
        for (int i = 0; i < nDivision; i++)
        {
            for (int j = 0; j < nDivision; j++)
            {
                topLeft = j + i * (nVert);
                topRight = topLeft + 1;
                botLeft = topLeft + nVert;
                botRight = botLeft + 1;

                tri[triCount] = topLeft;
                triCount += 1;
                tri[triCount] = topRight;
                triCount += 1;
                tri[triCount] = botRight;
                triCount += 1;
                tri[triCount] = topLeft;
                triCount += 1;
                tri[triCount] = botRight;
                triCount += 1;
                tri[triCount] = botLeft;
                triCount += 1;
            }
        }

        mesh.vertices = vert;
        mesh.triangles = tri;
        mesh.uv = uv;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        collider.sharedMesh = mesh;
        collider.convex = true;
    }
}

