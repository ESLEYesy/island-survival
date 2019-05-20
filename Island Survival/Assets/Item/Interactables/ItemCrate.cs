using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrate : Interactable
{

    public ItemManager itemManager;

    public ItemCrate() : base()
    {
        Name = "Item Crate";
    }

    private Vector3 RandomRange(float min, float max, bool upwardsBias)
    {
        return new Vector3(UnityEngine.Random.Range(min, max),
            (upwardsBias ? System.Math.Abs(UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)) : UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)),
            UnityEngine.Random.Range(min, max));
    }

    public override void Interact(Player user)
    {
        Debug.Log(user.playerName + " has opened an Item Crate!");

        foreach(GameObject prefab in itemManager.prefabs)
        {
            GameObject spawnItem = Instantiate(prefab, this.transform.position + RandomRange(-0.5f, 0.5f, true), this.transform.rotation);
            Debug.Log("Item Crate spawned '" + prefab.GetComponent<Item>().Name + "'!");
            spawnItem.GetComponent<Rigidbody>().AddForce(RandomRange(-8f, 8f, true));
            spawnItem.GetComponent<Rigidbody>().AddTorque(RandomRange(-20f, 20f, false));
        }
        itemManager.PlaySound("crateBreak");
        this.tag = "Untagged";
        user.cutInteraction(gameObject);

        //explosion
        Invoke("Break", 0.05f);
    }

    public void Break()
    {

        Destroy(gameObject);
    }

}
