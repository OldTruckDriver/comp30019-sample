using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpScript : MonoBehaviour
{
    Vector3[] vert;
    Vector2[] uv;
    int[] tri;

    // Use this for initialization
    void Start()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vert = new Vector3[4];
        vert[0] = new Vector3(-1.0f, 2.0f, 1.0f);
        vert[1] = new Vector3(1.0f, 0.0f, 1.0f);
        vert[2] = new Vector3(-1.0f, 0.0f, -1.0f);
        vert[3] = new Vector3(1.0f, 0.0f, -1.0f);

        uv = new Vector2[4];
        uv[0] = new Vector2(0.0f, 0.0f);
        uv[1] = new Vector2(1.0f, 0.0f);
        uv[2] = new Vector2(0.0f, 1.0f);
        uv[3] = new Vector2(1.0f, 1.0f);

        tri = new int[6];
        tri[0] = 0;
        tri[1] = 1;
        tri[2] = 3;

        tri[3] = 0;
        tri[4] = 3;
        tri[5] = 2;

        mesh.vertices = vert;
        mesh.uv = uv;
        mesh.triangles = tri;
    }
}
	
	
