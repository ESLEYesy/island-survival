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

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("You've been hit!");

        if (other.CompareTag("Harvestable"))
        {
            other.GetComponent<Harvestable>().strike(this);

        } else if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            p.loseHealth(damage);
            Vector3 dir = transform.position - other.transform.position;
            dir = -dir.normalized;

            other.GetComponent<Rigidbody>().AddForce((dir * damage / 3) + new Vector3(0, 5f, 0));
        }
    }

}
