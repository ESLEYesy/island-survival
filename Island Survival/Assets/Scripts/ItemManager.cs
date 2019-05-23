using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    //Sounds
    public AudioClip foodEat;
    public AudioClip crateBreak;
    public AudioClip drinkGulp;
    public AudioClip attackSwing;
    public AudioClip crateOpen;
    public AudioClip hitHard;
    public AudioClip hitSoft;
    public AudioClip itemDrop;
    public AudioClip itemPickup;
    public AudioClip takeDamage;
    public AudioClip playerDeath;

    public AudioClip bubble;
    public AudioClip lightTick;
    public AudioClip interfaceError;
    public AudioClip healDamage;
    public AudioClip bubbleShort;

    //Prefabs
    public GameObject axePrefab;
    public GameObject macePrefab;
    public GameObject machetePrefab;
    public GameObject shivPrefab;
    public GameObject rumPrefab;
    public GameObject breadPrefab;

    public List<GameObject> prefabs;

    //Non-Item Prefabs
    public GameObject itemCratePrefab;
    public GameObject spawnPoint;

    //Hitboxes
    public GameObject Hitbox;

    //Sprites
    public Sprite axeSprite;
    public Sprite shivSprite;
    public Sprite maceSprite;
    public Sprite macheteSprite;
    public Sprite rumSprite;
    public Sprite breadSprite;
    public Sprite defaultSprite;

    //Particles
    public GameObject hpParticle;
    public GameObject explosionParticle;

    void Start()
    {
        prefabs = new List<GameObject>
        {
            axePrefab,
            rumPrefab,
            macePrefab,
            machetePrefab,
            shivPrefab,
            breadPrefab
        };
    }

    public void SpawnItem(GameObject item, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 torque)
    {
        GameObject spawn;
        if(item != null)
        {
            spawn = GameObject.Instantiate(item, position, rotation);
            spawn.GetComponent<Rigidbody>().AddForce(velocity);
            spawn.GetComponent<Rigidbody>().AddTorque(torque);
        }
    }

    public Vector3 RandomRange(float min, float max, bool upwardsBias)
    {
        return new Vector3(UnityEngine.Random.Range(min, max),
            (upwardsBias ? Math.Abs(UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)) : UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)),
            UnityEngine.Random.Range(min, max));
    }

    public Vector3 RandomRange(float min, float max)
    {
        return RandomRange(min, max, false);
    }

    public void LaunchItem(GameObject item, Vector3 position, Quaternion rotation)
    {
        SpawnItem(item, position, rotation, (Camera.main.transform.up * 24 + RandomRange(-2, 2, true)), RandomRange(-10, 10, false));
    }

    public GameObject GetItem(string item)
    {
        switch (item)
        {
            case "Axe":
                return axePrefab;
            case "Mace":
                return macePrefab;
            case "Machete":
                return machetePrefab;
            case "Shiv":
                return shivPrefab;
            case "Rum":
                return rumPrefab;
            case "Bread":
                return breadPrefab;
            case "ItemCrate":
                return itemCratePrefab;
        }
        return null;
    }

    public void SpawnParticle(string particle, Vector3 pos, string particleString)
    {
        switch (particle)
        {
            case "HPParticle":
                GameObject hpp = Instantiate(hpParticle, pos, Camera.main.transform.rotation);
                TextMesh tm = hpp.transform.Find("HPLabel").GetComponent<TextMesh>();
                tm.text = particleString;
                break;
        }
    }

    public void SpawnParticle(string particle)
    {

        switch (particle) {
            case "smallExplosion":
                GameObject part = Instantiate(explosionParticle, this.transform.position, this.transform.rotation);
                part.GetComponent<ParticleSystem>().Play();
                break;
        }

    }

    public void UseItem(Player user, string item)
    {
        bool visibleHitboxes = false;
        GameObject box;
        Hitbox hbScript;
        switch (item)
        {
            case "Mace":
                //use Mace
                box = Instantiate(Hitbox, user.transform.position + (user.transform.up * 0.5f) + (user.transform.forward * 1.2f), user.transform.rotation);
                box.transform.localScale = new Vector3(1.3f, 1.5f, 1.3f);
                box.transform.parent = user.transform;
                hbScript = box.GetComponent<Hitbox>();
                hbScript.damage = 50;
                hbScript.harvestTrees = false;
                hbScript.harvestRocks = true;
                hbScript.swingDelay = 0.75f;
                hbScript.hitTime = 0.6f;
                box.GetComponent<MeshRenderer>().enabled = visibleHitboxes;
                Debug.Log(user.playerName + " swung their Mace!");
                PlaySound("attackSwing", user.actionSound);
                break;
            case "Machete":
                //use Machete
                box = Instantiate(Hitbox, user.transform.position + (user.transform.up * 0.5f) + (user.transform.forward * 1.25f), user.transform.rotation);
                box.transform.localScale = new Vector3(1.1f, 1.5f, 1.1f);
                box.transform.parent = user.transform;
                hbScript = box.GetComponent<Hitbox>();
                hbScript.damage = 25;
                hbScript.harvestTrees = false;
                hbScript.harvestRocks = false;
                hbScript.swingDelay = 0f;
                hbScript.hitTime = 0.3f;
                box.GetComponent<MeshRenderer>().enabled = visibleHitboxes;
                Debug.Log(user.playerName + " swung their Machete!");
                PlaySound("attackSwing", user.actionSound);
                break;
            case "Shiv":
                //use Shiv
                box = Instantiate(Hitbox, user.transform.position + (user.transform.up * 0.5f) + (user.transform.forward * 1.0f), user.transform.rotation);
                box.transform.localScale = new Vector3(0.7f, 1.5f, 0.7f);
                box.transform.parent = user.transform;
                hbScript = box.GetComponent<Hitbox>();
                hbScript.damage = 20;
                hbScript.harvestTrees = false;
                hbScript.harvestRocks = false;
                hbScript.swingDelay = 0f;
                hbScript.hitTime = 0.1f;
                box.GetComponent<MeshRenderer>().enabled = visibleHitboxes;
                Debug.Log(user.playerName + " swung their Shiv!");
                PlaySound("attackSwing", user.actionSound);
                break;
            case "Axe":
                //use Axe
                box = Instantiate(Hitbox, user.transform.position + (user.transform.up * 0.5f) + (user.transform.forward * 1.3f), user.transform.rotation);
                box.transform.localScale = new Vector3(1f, 1.5f, 1f);
                box.transform.parent = user.transform;
                hbScript = box.GetComponent<Hitbox>();
                hbScript.damage = 30;
                hbScript.harvestTrees = true;
                hbScript.harvestRocks = false;
                hbScript.swingDelay = 0f;
                hbScript.hitTime = 0.2f;
                box.GetComponent<MeshRenderer>().enabled = visibleHitboxes;
                Debug.Log(user.playerName + " swung their Axe!");
                PlaySound("attackSwing", user.actionSound);
                break;
            case "Bread":
                //use Bread
                user.GainEnergy(50);
                PlaySound("foodEat", user.actionSound);
                user.DestroyEquippedItem();
                break;
            case "Rum":
                user.GainHealth(15);
                PlaySound("drinkGulp", user.actionSound);
                user.DestroyEquippedItem();
                break;
            default:
                //use ERROR
                Debug.Log("ERROR! User " + user.playerName + " attempted to use an item that doesn't exist: " + item);
                break;
        }
    }

    public Sprite GetSprite(string item)
    {
        switch (item)
        {
            case "Axe":
                return axeSprite;
            case "Rum":
                return rumSprite;
            case "Bread":
                return breadSprite;
            case "Shiv":
                return shivSprite;
            case "Mace":
                return maceSprite;
            case "Machete":
                return macheteSprite;
            case null:
                return defaultSprite;
        }
        return defaultSprite;
    }

    public void PlaySound(string sound, AudioSource source)
    {
        switch (sound)
        {
            case "foodEat":
                source.clip = foodEat;
                source.Play();
                break;
            case "crateBreak":
                source.clip = crateBreak;
                source.Play();
                break;
            case "drinkGulp":
                source.clip = drinkGulp;
                source.Play();
                break;
            case "attackSwing":
                source.clip = attackSwing;
                source.Play();
                break;
            case "bubble":
                source.clip = bubble;
                source.Play();
                break;
            case "bubbleShort":
                source.clip = bubbleShort;
                source.Play();
                break;
            case "crateOpen":
                source.clip = crateOpen;
                source.Play();
                break;
            case "healDamage":
                source.clip = healDamage;
                source.Play();
                break;
            case "hitHard":
                source.clip = hitHard;
                source.Play();
                break;
            case "hitSoft":
                source.clip = hitSoft;
                source.Play();
                break;
            case "interfaceError":
                source.clip = interfaceError;
                source.Play();
                break;
            case "itemDrop":
                source.clip = itemDrop;
                source.Play();
                break;
            case "itemPickup":
                source.clip = itemPickup;
                source.Play();
                break;
            case "takeDamage":
                source.clip = takeDamage;
                source.Play();
                break;
            case "playerDeath":
                source.clip = playerDeath;
                source.Play();
                break;
            case "lightTick":
                source.clip = lightTick;
                source.Play();
                break;
        }
    }
}
