using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
	public Terrain islandTerrain;
	public LayerMask terrainLayer;
	public static float terrainLeft, terrainRight, terrainTop, terrainBottom, terrainWidth, terrainLength, terrainHeight;


	public static ArrayList units = new ArrayList();
	public static ArrayList positions = new ArrayList();
	public static ArrayList rotations = new ArrayList();

	public void Awake()
	{
		// Get terrain dimensions
		terrainLeft = islandTerrain.transform.position.x;
		terrainBottom = islandTerrain.transform.position.z;
		terrainWidth = islandTerrain.terrainData.size.x;
		terrainLength = islandTerrain.terrainData.size.z;
		terrainHeight = islandTerrain.terrainData.size.y;
		terrainRight = terrainLeft + terrainWidth;
		terrainTop = terrainBottom + terrainLength;

		InstantiateRandomPosition("Prefabs/enchantedforest_tree_1", 300, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_4", 300, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_5", 300, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_bush_5", 150, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_flower_3", 150, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_flower_5", 150, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_fallen_small", 150, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_stump_2", 150, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_stone_2", 150, 0f);
		InstantiateRandomPosition("Prefabs/island_bush_1", 150, 0f);
		InstantiateRandomPosition("Prefabs/island_campfire", 5, 0f);
		InstantiateRandomPosition("Prefabs/island_bush_palm", 150, 0f);
		InstantiateRandomPosition("Prefabs/island_cattail", 150, 0f);
		InstantiateRandomPosition("Prefabs/island_dirtpile", 150, 0f);
	}

	public void InstantiateRandomPosition(string resource, int amount, float addedHeight)
	{
		var i = 0;
		float height = 0;
		RaycastHit hit;
		float randPosX, randPosY, randPosZ;
		Vector3 randPos = Vector3.zero;
		do 
		{
			i++;
			randPosX = Random.Range(terrainLeft, terrainRight);
			randPosZ = Random.Range(terrainBottom, terrainTop);
			
			if (Physics.Raycast(new Vector3(randPosX, 9999f, randPosZ), Vector3.down, out hit, Mathf.Infinity, terrainLayer))
			{
				terrainHeight = hit.point.y;
			}

			randPosY = terrainHeight + addedHeight;
			randPos = new Vector3(randPosX, randPosY, randPosZ);
			Instantiate(Resources.Load(resource, typeof(GameObject)), randPos, Quaternion.identity);
		} while (i < amount);
	}
}
