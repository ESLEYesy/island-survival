using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
	public Terrain islandTerrain;
	public LayerMask terrainLayer;
	public static float terrainLeft, terrainRight, terrainTop, terrainBottom, terrainWidth, terrainLength, terrainHeight;

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
		// Main terrain dimensions
		terrainLeft = islandTerrain.transform.position.x + 110;
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
		trpTop = terrainTop + 50;

		InstantiateRandomPosition("Prefabs/enchantedforest_tree_1", 200, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_4", 200, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_5", 200, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_bush_5", 100, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_flower_3", 100, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_flower_5", 100, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_fallen_small", 100, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_tree_stump_2", 100, 0f);
		InstantiateRandomPosition("Prefabs/enchantedforest_stone_2", 100, 0f);
		InstantiateRandomPosition("Prefabs/island_bush_1", 100, 0f);
		InstantiateRandomPosition("Prefabs/island_campfire", 5, 0f);
		InstantiateRandomPosition("Prefabs/island_bush_palm", 100, 0f);
		InstantiateRandomPosition("Prefabs/island_cattail", 100, 0f);
		InstantiateRandomPosition("Prefabs/island_dirtpile", 100, 0f);
	}

	public void InstantiateRandomPosition(string resource, int amount, float addedHeight)
	{
		var i = 0;
		var blpCount = 10;
		var trpCount = 1;
		float height = 0;
		RaycastHit hit;
		float randPosX, randPosY, randPosZ;
		Vector3 randPos = Vector3.zero;
		do 
		{
			i++;
			if (blpCount == 20)
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
			}
			
			
			if (Physics.Raycast(new Vector3(randPosX, 9999f, randPosZ), Vector3.down, out hit, Mathf.Infinity))
			{
				if (hit.collider.gameObject.tag == "Foliage")
				{
					// If collision, we throw this out and try again
					i--;
					//Debug.Log("Collided with foliage!");
				}

				else
				{
					// No collision, we are good to go
					terrainHeight = hit.point.y;
					randPosY = terrainHeight + addedHeight;
					randPos = new Vector3(randPosX, randPosY, randPosZ);
					Instantiate(Resources.Load(resource, typeof(GameObject)), randPos, Quaternion.Euler(0, Random.Range(0f,360f),0f), foliageContainer.transform);
				}
			}			
		} while (i < amount);
	}
}
