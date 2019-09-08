using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraFormer : MonoBehaviour {
    public int nDivision = 128;
    public float terrainSize = 200000;
    public float randomHeight = 170;
    public float defaultHeight = 200;
    public float randomHeightMultiplier = 0.5f;
    public float tiling = 10;
    public Shader shader;
    public Texture texture;
    Vector3[] vert;
    Vector3[] normals;

    // Use this for initialization
    void Start () {
        var water = GameObject.Find("Water");
        MeshCollider collider = this.gameObject.AddComponent<MeshCollider>();
        Physics.IgnoreCollision(water.GetComponent<MeshCollider>(), collider);

        float divisionSize = terrainSize / (nDivision * nDivision);
        int nVert = nDivision + 1;
        
        Vector2[] uv;
        int[] tri;


        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Color[] colors = new Color[nVert * nVert];
        normals = new Vector3[nVert * nVert];
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
            colors[i] = Color.clear;
        }

        int topLeft;
        int topRight;
        int botLeft;
        int botRight;
        int triCount = 0;
        for (int i = 0; i < nDivision ; i++)
        {
            for (int j = 0; j < nDivision ; j++)
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

        vert[0].y = randomHeight;
        vert[nDivision].y = randomHeight;
        vert[nVert * nVert - nVert].y = randomHeight;
        vert[nVert * nVert - 1].y = randomHeight;

        int currentDivision = nDivision;
        int row; int column;
        for (int n = 0; n < (int)Mathf.Log(nDivision, 2); n++)
        {
            row = 0;
            for (int i = 0; i < Mathf.Pow(2, n) ; i++)
            {
                column = 0;
                for (int j = 0; j < Mathf.Pow(2, n) ; j++)
                {
                    DSAlgorithm(row, column, currentDivision, randomHeight);
                    column += currentDivision;
                }
                row += currentDivision;
            }
            currentDivision /= 2;
            randomHeight *= randomHeightMultiplier;
        }

        float snowThreshold = randomHeight * 2f - randomHeight * 0.2f;
        float sandThreshold = randomHeight * 0.1f;
        for (int i = 0; i < nVert * nVert; i++)
        {
            if (vert[i].y > 300)
            {
                colors[i] = Color.white;
            }
            if (vert[i].y < 160){
                colors[i] = new Color(0.933f, 0.839f, 0.686f);
            }
        }

        Vector3 vectorA;
        Vector3 vectorB;
        Vector3 normal;
        int current;
        for (int i = 0; i < nVert - 1; i++){
            for (int j = 0; j < nVert ; j++){
                current = i * nVert + j;
                if (j % nVert == nDivision)
                {
                    vectorA = vert[current - 1] - vert[current];
                    vectorB = vert[current + nVert] - vert[current];
                }
                else{
                    vectorA = vert[current + 1] - vert[current];
                    vectorB = vert[current + nVert] - vert[current];
                }
                normal = Vector3.Cross(vectorA, vectorB);
                normals[current] = normal;
            }
        }
        for (int i = 0; i < nVert; i++){
            current = nVert * nDivision + i;
            if (i % nVert == nDivision)
            {
                vectorA = vert[current - 1] - vert[current];
                vectorB = vert[current - nVert] - vert[current];
            }
            else
            {
                vectorA = vert[current + 1] - vert[current];
                vectorB = vert[current - nVert] - vert[current];
            }
            normal = Vector3.Cross(vectorA, vectorB);
            normals[current] = normal;
        }
        mesh.vertices = vert;
        mesh.colors = colors;
        mesh.normals = normals;
        mesh.triangles = tri;
        mesh.uv = uv;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


        collider.sharedMesh = mesh;
        collider.convex = true;

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
        renderer.material.mainTexture = texture;
        renderer.material.mainTextureScale = new Vector2(400000, 400000);

	}

    void DSAlgorithm(int row, int column, int currentDivision, float randomHeight)
    {
        int halfDivision = (int)(currentDivision * 0.5f);
        int topLeft = column + row * (nDivision + 1);
        int botLeft = column + (row + currentDivision) * (nDivision + 1);
        int mid = (int)(column + halfDivision) + (int)(row + halfDivision) * (nDivision + 1);

        vert[mid].y = (vert[topLeft].y + vert[botLeft].y + vert[topLeft + currentDivision].y + vert[botLeft + currentDivision].y)*0.25f + Random.Range(-randomHeight, randomHeight);
        vert[topLeft + halfDivision].y = (vert[topLeft].y + vert[topLeft + currentDivision].y + vert[mid].y)/3 + Random.Range(-randomHeight, randomHeight);
        vert[mid - halfDivision].y = (vert[topLeft].y + vert[botLeft].y + vert[mid].y) / 3 + Random.Range(-randomHeight, randomHeight);
        vert[botLeft + halfDivision].y = (vert[botLeft].y + vert[botLeft + currentDivision].y + vert[mid].y) / 3 + Random.Range(-randomHeight, randomHeight);
        vert[mid + halfDivision].y = (vert[botLeft + currentDivision].y + vert[topLeft + currentDivision].y + vert[mid].y) / 3 + Random.Range(-randomHeight, randomHeight);
    }
}

