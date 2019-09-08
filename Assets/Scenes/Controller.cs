using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    public float camSensitivity = 0.25f;
    public float cameraspeed = 10.0f;
    private GameObject terrain;
    Rigidbody rb;

    // Use this for initialization
    private GameObject gameObject;
    void Start()
    {
        gameObject = GameObject.Find("Main Camera");
        terrain = GameObject.Find("Terrain");
        var startingLocation = gameObject.transform.localPosition;
        startingLocation.y = terrain.GetComponent<TerraFormer>().defaultHeight + 200.0f;
        gameObject.transform.localPosition = startingLocation;
    }

    // Update is called once per frame
    void Update(){
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSensitivity, lastMouse.x * camSensitivity, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;

        var cameraPosition = gameObject.transform.localPosition;
        var terrainPosition = terrain.transform.localPosition;
        var mesh = terrain.GetComponent<MeshFilter>().mesh;
        var vert = mesh.vertices;

        var terraformer = terrain.GetComponent<TerraFormer>();
        var nDivision = terraformer.nDivision;
        var size = terraformer.terrainSize / nDivision;
        var origin = 0f;




        if (cameraPosition.x <= origin)
        {
            cameraPosition.x = origin;
            transform.localPosition = cameraPosition;
        }
        if (cameraPosition.x > size)
        {
            cameraPosition.x = size;
            transform.localPosition = cameraPosition;
        }
        if (cameraPosition.z < -size)
        {
            cameraPosition.z = -size;
            transform.localPosition = cameraPosition;
        }
        if (cameraPosition.z > origin)
        {
            cameraPosition.z = origin;
            transform.localPosition = cameraPosition;
        }

        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, cameraspeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.S))
        {
            this.gameObject.transform.Translate(new Vector3(0, 0, -cameraspeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.A))
        {
            this.gameObject.transform.Translate(new Vector3(-cameraspeed * Time.deltaTime, 0));
        }

        if (Input.GetKey(KeyCode.D))
        {
            this.gameObject.transform.Translate(new Vector3(cameraspeed * Time.deltaTime, 0, 0 ));
        }


    }
     
}
