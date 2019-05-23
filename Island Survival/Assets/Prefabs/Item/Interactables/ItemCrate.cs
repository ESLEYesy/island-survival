using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCrate : Interactable
{

    ItemManager itemManager;

    public void Start()
    {
        itemManager = Camera.main.transform.GetChild(3).gameObject.GetComponent<ItemManager>();
    }

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
        Debug.Log((user == null ? "Someone" : user.playerName) + " has opened an Item Crate!");

        foreach(GameObject prefab in itemManager.prefabs)
        {
            itemManager.LaunchItem(prefab, transform.position + transform.up, Random.rotation);
        }
        itemManager.PlaySound("crateOpen", gameObject.GetComponent<AudioSource>());
        this.tag = "Untagged";
        itemManager.SpawnParticle("smallExplosion");
        if (user != null)
        {
            user.cutInteraction(gameObject);
            gameObject.SetActive(false);
            Invoke("Break", 1f);
        }
        else
        {
            Break();
        }
    }

    public void Break()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hitbox"))
        {
            itemManager.PlaySound("hitHard", gameObject.GetComponent<AudioSource>());
            Interact(null);
        }
    }

}
