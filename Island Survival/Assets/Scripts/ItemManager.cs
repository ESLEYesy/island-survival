using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    //Sounds
    public AudioSource physicalAudioSource;

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


    public AudioSource headAudioSource;
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

    public void spawnParticle(string particle, Vector3 pos, string particleString)
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

    public void spawnParticle(string particle)
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
                PlaySound("attackSwing");
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
                PlaySound("attackSwing");
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
                PlaySound("attackSwing");
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
                PlaySound("attackSwing");
                break;
            case "Bread":
                //use Bread
                user.gainEnergy(50);
                PlaySound("foodEat");
                user.DestroyEquippedItem();
                break;
            case "Rum":
                user.gainHealth(15);
                PlaySound("drinkGulp");
                user.DestroyEquippedItem();
                break;
            default:
                //use ERROR
                Debug.Log("ERROR! User " + user.playerName + " attempted to use an item that doesn't exist: " + item);
                break;
        }
    }

    public Sprite getSprite(string item)
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

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "foodEat":
                physicalAudioSource.clip = foodEat;
                physicalAudioSource.Play();
                break;
            case "crateBreak":
                physicalAudioSource.clip = crateBreak;
                physicalAudioSource.Play();
                break;
            case "drinkGulp":
                physicalAudioSource.clip = drinkGulp;
                physicalAudioSource.Play();
                break;
            case "attackSwing":
                physicalAudioSource.clip = attackSwing;
                physicalAudioSource.Play();
                break;
            case "bubble":
                headAudioSource.clip = bubble;
                headAudioSource.Play();
                break;
            case "bubbleShort":
                headAudioSource.clip = bubbleShort;
                headAudioSource.Play();
                break;
            case "crateOpen":
                physicalAudioSource.clip = crateOpen;
                physicalAudioSource.Play();
                break;
            case "healDamage":
                headAudioSource.clip = healDamage;
                headAudioSource.Play();
                break;
            case "hitHard":
                physicalAudioSource.clip = hitHard;
                physicalAudioSource.Play();
                break;
            case "hitSoft":
                physicalAudioSource.clip = hitSoft;
                physicalAudioSource.Play();
                break;
            case "interfaceError":
                headAudioSource.clip = interfaceError;
                headAudioSource.Play();
                break;
            case "itemDrop":
                physicalAudioSource.clip = itemDrop;
                physicalAudioSource.Play();
                break;
            case "itemPickup":
                physicalAudioSource.clip = itemPickup;
                physicalAudioSource.Play();
                break;
            case "takeDamage":
                physicalAudioSource.clip = takeDamage;
                physicalAudioSource.Play();
                break;
            case "playerDeath":
                physicalAudioSource.clip = playerDeath;
                physicalAudioSource.Play();
                break;
            case "lightTick":
                headAudioSource.clip = lightTick;
                headAudioSource.Play();
                break;
        }
    }
}
