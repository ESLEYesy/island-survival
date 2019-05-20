using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvestable : MonoBehaviour
{
    public int numHits;
    public bool rock;
    public bool tree;
    public bool minResources;
    public bool maxResources;

    public ItemManager itemManager;

    private void Start()
    {

    }

    public void strike(Hitbox hb)
    {

        if (rock)
        {
            if (hb.harvestRocks)
            {
                this.takeHit();
            }
        }
        else if (tree)
        {
            if (hb.harvestTrees)
            {
                this.takeHit();
            }
        }
    }


    private void takeHit()
    {
        itemManager.PlaySound("hitHard");
    }
}
