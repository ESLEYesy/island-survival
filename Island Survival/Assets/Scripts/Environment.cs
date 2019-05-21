using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{

    public GameObject itemManager;

    public Terrain terrain;
    public LayerMask terrainLayer;
    public static float terrainLeft, terrainRight, terrainTop, terrainBottom, terrainWidth, terrainLength, terrainHeight;

    private TerrainData terrainData;
    private Vector3 terrainPos;

    // blp = bottom left peninsula
    public static float blpLeft, blpRight, blpTop, blpBottom;

    // trp = top right peninsula
    public static float trpLeft, trpRight, trpTop, trpBottom;

    public static ArrayList units = new ArrayList();
    public static ArrayList positions = new ArrayList();
    public static ArrayList rotations = new ArrayList();

    // Hierarchy parent to contain instantiated foliage game objects
    public GameObject foliageContainer;

    public void Awake()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
        terrainPos = terrain.transform.position;

        // Main terrain dimensions
        /*terrainLeft = islandTerrain.transform.position.x + 110;
		terrainBottom = islandTerrain.transform.position.z + 250;
		terrainWidth = islandTerrain.terrainData.size.x;
		terrainLength = islandTerrain.terrainData.size.z;
		terrainHeight = islandTerrain.terrainData.size.y;
		terrainRight = terrainLeft + terrainWidth - 230;
		terrainTop = terrainBottom + terrainLength - 450;

		// Bottom left corner peninsula
		blpLeft = islandTerrain.transform.position.x + 160;
		blpBottom = islandTerrain.transform.position.z + 150;
		blpRight = terrainLeft + 125;
		blpTop = terrainBottom;

		// Top right corner peninsula
		trpLeft = terrainRight - 125;
		trpBottom = terrainTop;
		trpRight = terrainRight - 25;
		trpTop = terrainTop + 50;*/

        List<int> texturesAllowed = new List<int>();
        texturesAllowed.Add(2);

        InstantiateRandomPosition("Prefabs/enchantedforest_bush_5", 100, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_flower_3", 400, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_flower_5", 400, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/island_bush_1", 100, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/island_bush_palm", 100, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/island_cattail", 100, 0f, texturesAllowed, ~(1 << 4));

        texturesAllowed.Add(0);
        texturesAllowed.Add(1);

        InstantiateRandomPosition("Prefabs/enchantedforest_stone_2", 600, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/island_dirtpile", 200, 0f, texturesAllowed, ~(1 << 4));

        texturesAllowed.Remove(2);
        InstantiateRandomPosition("Prefabs/enchantedforest_stone_2", 1000, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/ItemCrateResource", 50, 0f, texturesAllowed, ~(1 << 4));

        texturesAllowed.Remove(0);
        texturesAllowed.Remove(1);
        texturesAllowed.Add(2);

        InstantiateRandomPosition("Prefabs/enchantedforest_tree_fallen_small", 100, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_tree_stump_2", 100, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_tree_1", 200, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_tree_4", 200, 0f, texturesAllowed, ~(1 << 4));
        InstantiateRandomPosition("Prefabs/enchantedforest_tree_5", 200, 0f, texturesAllowed, ~(1 << 4));

        InstantiateRandomPosition("Prefabs/SpawnPoint", 10, 0f, new List<int>() { 0, 1 }, ~(0));


        //InstantiateRandomPosition("Prefabs/island_campfire", 5, 0f);



    }



    public void InstantiateRandomPosition(string resource, int amount, float addedHeight, List<int> texturesAllowed, int layersChecked)
    {
        //Debug.Log("The index of the terrain texture at 0,0 is: " + GetMainTexture(new Vector3(0f, 0f, 0f)));
        var i = 0;
        //var blpCount = 10;
        //var trpCount = 1;
        //float height = 0;
        RaycastHit hit;
        float randPosX, randPosY, randPosZ;
        Vector3 randPos = Vector3.zero;
        do
        {
            i++;
            /*if (blpCount == 20)
			{
				randPosX = Random.Range(blpLeft, blpRight);
				randPosZ = Random.Range(blpBottom, blpTop);
				blpCount = 1;
				trpCount++;
			}

			else if (trpCount == 20)
			{
				randPosX = Random.Range(trpLeft, trpRight);
				randPosZ = Random.Range(trpBottom, trpTop);
				trpCount = 1;
				blpCount++;
			}

			else
			{
				randPosX = Random.Range(terrainLeft, terrainRight);
				randPosZ = Random.Range(terrainBottom, terrainTop);
                blpCount++;
                trpCount++;
            }*/

            randPosX = Random.Range(-299, 299);
            randPosZ = Random.Range(-299, 299);


            if (Physics.Raycast(new Vector3(randPosX, 9999f, randPosZ), Vector3.down, out hit, Mathf.Infinity, layersChecked))
            {
                if (hit.collider.gameObject.tag != "Terrain")
                {
                    // Collided with something that isn't terrain.
                    i--;
                }
                else //we found terrain
                {
                    int texture = GetMainTexture(new Vector3(randPosX, 0, randPosZ));
                    if (texturesAllowed.Contains(texture))
                    {
                        terrainHeight = hit.point.y;
                        randPosY = terrainHeight + addedHeight;
                        randPos = new Vector3(randPosX, randPosY, randPosZ);
                        Instantiate(Resources.Load(resource, typeof(GameObject)), randPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0f), foliageContainer.transform);
                    }
                    else
                    {
                        i--; //wrong texture
                    }
                }
            }

        } while (i < amount);
    }

    private void InstantiateRandomPositionClustered(string resource, int amount, float addedHeight, List<int> texturesAllowed, int layersChecked, float radius, Vector3 center)
    {
        var i = 0;
        RaycastHit hit;
        float randPosX, randPosY, randPosZ;
        Vector3 randPos = Vector3.zero;
        do
        {
            i++;
            randPosX = Random.Range(center.x - radius, center.x + radius);
            randPosZ = Random.Range(center.z - radius, center.z + radius);
            if (Physics.Raycast(new Vector3(randPosX, 9999f, randPosZ), Vector3.down, out hit, Mathf.Infinity, layersChecked))
            {
                if (hit.collider.gameObject.tag != "Terrain")
                {
                    // Collided with something that isn't terrain.
                    i--;
                }
                else //we found terrain
                {
                    int texture = GetMainTexture(new Vector3(randPosX, 0, randPosZ));
                    if (texturesAllowed.Contains(texture))
                    {
                        terrainHeight = hit.point.y;
                        randPosY = terrainHeight + addedHeight;
                        randPos = new Vector3(randPosX, randPosY, randPosZ);
                        Instantiate(Resources.Load(resource, typeof(GameObject)), randPos, Quaternion.Euler(0, Random.Range(0f, 360f), 0f), foliageContainer.transform);
                        doExtraForResource(resource, new Vector3(randPosX, randPosY, randPosZ));
                    }
                    else
                    {
                        i--; //wrong texture
                    }
                }
            }

        } while (i < amount);
    }

    private void doExtraForResource(string res, Vector3 position)
    {
        switch (res)
        {
            case "Prefabs/SpawnPoint":
                InstantiateRandomPositionClustered("Prefabs/ItemCrateResource", 3, 0f, new List<int>() { 0, 1, 2 }, ~(1 << 4), 4, position);


                break;
        }
    }

    private float[] GetTextureMix(Vector3 WorldPos)
    {
        // returns an array containing the relative mix of textures
        // on the main terrain at this world position.

        // The number of values in the array will equal the number
        // of textures added to the terrain.

        // calculate which splat map cell the worldPos falls within (ignoring y)
        int mapX = (int)(((WorldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        int mapZ = (int)(((WorldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        // get the splat data for this cell as a 1x1xN 3d array (where N = number of textures)
        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        // extract the 3D array data to a 1D array:
        float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

        for (int n = 0; n < cellMix.Length; n++)
        {
            cellMix[n] = splatmapData[0, 0, n];
        }
        return cellMix;
    }

    private int GetMainTexture(Vector3 WorldPos)
    {
        // returns the zero-based index of the most dominant texture
        // on the main terrain at this world position.
        float[] mix = GetTextureMix(WorldPos);

        float maxMix = 0;
        int maxIndex = 0;

        // loop through each mix value and find the maximum
        for (int n = 0; n < mix.Length; n++)
        {
            if (mix[n] > maxMix)
            {
                maxIndex = n;
                maxMix = mix[n];
            }
        }
        return maxIndex;
    }



}
