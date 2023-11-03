// Step 3 is to add randomly objects on top of the mesh
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGen3 : MonoBehaviour
{
    public GameObject blockGameObject;

    public GameObject objectToSpawn;

    [SerializeField] private int gridX = 80;
    [SerializeField] private int gridZ = 80;
    [SerializeField] private int noiseHeight = 3;    // how high the hills are
    [SerializeField] public float gridScale = 1.15f; // spacing

    // Hash table containing the already-used locations in which decor objects have been placed.
    private List<Vector3> blockPositions = new List<Vector3>(); 

    // Start is called before the first frame update
    void Start()
    {
        for (int x = -gridX; x < gridX; x++)
        {
            for (int z = -gridZ; z < gridZ; z++)
            {

                // The offset after generateNoise (+ 4) is the height of the lowest point in the ground
                Vector3 pos = new Vector3(x + gridX * gridScale / 10, generateNoise(x, z, 20f) + 2.75f, z + gridZ * gridScale / 10); 

                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;

                // Record the possible positions for decorations to be placed
                blockPositions.Add(block.transform.position);

                block.transform.SetParent(this.transform);
            }
        }
        // Place decorations
        SpawnObject();
    }

    /* Generates perlin noise using Mathf.
     * Inputs: x and z are the x and z coordinates within the noise map. detailscale is how much to zoom in on the map (high values stretch)
     * Output: The output is the height of the point chosen
     * all inputs can be negatives or 0 with no issues
     */
    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.z) / detailScale;

        return noiseHeight * Mathf.PerlinNoise(xNoise, zNoise);
    }

    /* Chooses a location from the blockpositions table, so an object can be placed at the location. The chosen location is then removed from the table.
     * Inputs: none
     * Output: The output is a vector leading to the chosen location.
     * Adding an offset to the y value of the vector can be used to ensure objects are placed at the proper height,
     * And the offset starts from the height of the cube it is placed above so the height can be static
     */
    private Vector3 ObjectSpawnLocation()
    {
        int rndIndex = Random.Range(0, blockPositions.Count);

        Vector3 newPos = new Vector3(
            blockPositions[rndIndex].x,
            blockPositions[rndIndex].y + 0.5f,
            blockPositions[rndIndex].z
        );

        blockPositions.RemoveAt(rndIndex);
        return newPos;
    }

    private void SpawnObject()
    {
        for (int c = 0; c < 20; c++)
        {
            GameObject toPlaceObject = Instantiate(objectToSpawn, ObjectSpawnLocation(), Quaternion.identity);
        }
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
}
