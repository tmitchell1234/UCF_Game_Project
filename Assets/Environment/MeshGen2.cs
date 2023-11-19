using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeshGen2 : MonoBehaviour
{

    [SerializeField] private int gridX = 10;
    [SerializeField] private int gridZ = 10;
    [SerializeField] private float noiseHeight = 3;
    [SerializeField] public float gridScale = 1.15f;

    private Mesh mesh;

    private int[] triangles;
    private Vector3[] vertices;

    public GameObject objectToSpawn;
    // Hash table containing the already-used locations in which decor objects have been placed.
    private List<Vector3> nodePositions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateMesh();
        SpawnObject();
        UpdateMesh();
        // Make a collision mesh that matches the ground mesh
        var collider = this.AddComponent<MeshCollider>();
        collider.sharedMesh = this.mesh;

    }


    void GenerateMesh()
    {
        // 6 vertices per quad
        triangles = new int[gridX * gridZ * 6]; // * 2 + 1 so that the mesh generates both directions from the center
        // 4 quads in the x direction = 5 vertices in the x direction
        vertices = new Vector3[(gridX + 1) * (gridZ + 1)]; // * 2 + 1 so that the mesh generates both directions from the center

<<<<<<< HEAD
        for (int i = 0, z = 120; z <= gridZ + 120; z++)
=======
        for (int i = 0, z = 0; z <= gridZ; z++)
>>>>>>> c7551979a90fa95cdb3d59e569756b600afe307b
        {
            for (int x = 0; x <= gridX; x++)
            {
                // Add a node. generateNoise is called twice with different scales to make the terrain look more natural
<<<<<<< HEAD
                vertices[i] = new Vector3(x - gridX / 2, generateNoise(x, z, gridScale) + 4 * generateNoise(x + 2, z - 2, gridScale * 5.5f) + Mathf.Pow(1.1f, 0.5f * (z - 240)) - 23.75f, z);
=======
                vertices[i] = new Vector3(x - gridX / 2, Mathf.Max(generateNoise(x, -z, gridScale) + 4 * generateNoise(x + 2, -z - 2, gridScale * 5.5f), 2) - 23.75f, z + 120);
>>>>>>> c7551979a90fa95cdb3d59e569756b600afe307b

                // Save the node for decoration placement
                nodePositions.Add(vertices[i] + new Vector3(-2, 21.5f - generateNoise(x, z, gridScale), -2));
                i++;
            }
        }

        // Assign vertices to triangles
        int tris = 0;
        int verts = 0;

        for (int z = 0; z <= gridZ - 1; z++)
        {
            for (int x = 0; x <= gridX - 1; x++)
            {
                // A quad is defined as points [0, 1, 2], [1, 3, 2]
                triangles[tris + 0] = verts + 0;
                triangles[tris + 1] = verts + gridZ + 1;
                triangles[tris + 2] = verts + 1;

                triangles[tris + 3] = verts + 1;
                triangles[tris + 4] = verts + gridZ + 1;
                triangles[tris + 5] = verts + gridZ + 2;

                verts++;
                tris += 6;
            }
            verts++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    // Generates a height from perlin noise using an x coordinate, z coodinate, and zoom amount. Zoom out by choosing values between 0 and 1, not negative
    private float generateNoise(int x, int z, float detailScale)
    {
        // As much as I love dividing by zero computers do not
        if (detailScale == 0)
        {
            detailScale = 0.1f;
        }
        float xNoise = (x + this.transform.position.x) / detailScale;
<<<<<<< HEAD
        float zNoise = (z + this.transform.position.z) / detailScale;
=======
        float zNoise = (z + this.transform.position.z) / detailScale + 240;
>>>>>>> c7551979a90fa95cdb3d59e569756b600afe307b

        return noiseHeight * Mathf.PerlinNoise(xNoise, zNoise);
    }

    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, nodePositions.Count);

        if (Mathf.Abs(nodePositions[rndIndex].x) > 7 && Mathf.Abs(nodePositions[rndIndex].z) > 7)
        { }

        Vector3 newPos = new Vector3(
            nodePositions[rndIndex].x,
            nodePositions[rndIndex].y + 0.5f,
            nodePositions[rndIndex].z
        );

        nodePositions.RemoveAt(rndIndex);
        return newPos;
    }

    private void SpawnObject()
    {
        for (int c = 0; c < 135; c++)
        {
            GameObject toPlaceObject = Instantiate(objectToSpawn, ObjectSpawnLocation(), Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            // Random sizes
            toPlaceObject.transform.localScale += new Vector3(Random.Range(0.35f, 2.15f), Random.Range(0.35f, 2.15f), Random.Range(0.35f, 2.15f));
        }
    }
}
