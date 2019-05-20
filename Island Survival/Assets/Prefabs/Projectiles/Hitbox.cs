using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public int damage;
    public bool harvestTrees;
    public bool harvestRocks;
    public float swingDelay;
    public float hitTime;

    void Start()
    {
        Invoke("hit", swingDelay);
    }

    private void hit()
    {
        gameObject.tag = "Hitbox";
        Destroy(gameObject, hitTime);
    }
}
