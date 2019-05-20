using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    //Sounds
    public AudioSource audioSource;
    public AudioClip foodEat;
    public AudioClip crateBreak;
    public AudioClip drinkGulp;

    //Prefabs
    public GameObject axePrefab;
    public GameObject rumPrefab;
    public GameObject breadPrefab;

    public List<GameObject> prefabs;

    //Non-Item Prefabs
    public GameObject itemCratePrefab;

    //Hitboxes
    public GameObject axeHitbox;

    //Sprites
    public Sprite axeSprite;
    public Sprite rumSprite;
    public Sprite breadSprite;
    public Sprite defaultSprite;

    void Start()
    {
        prefabs = new List<GameObject>
        {
            axePrefab,
            rumPrefab,
            breadPrefab
        };
    }

    public void UseItem (Player user, string item)
    {
        switch(item){
            case "Axe":
                //use Axe
                Debug.Log(user.playerName + " swung their Axe!");
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

    public Sprite getSprite (string item)
    {
        switch (item)
        {
            case "Axe":
                return axeSprite;
            case "Rum":
                return rumSprite;
            case "Bread":
                return breadSprite;
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
                audioSource.clip = foodEat;
                audioSource.Play();
                break;
            case "crateBreak":
                audioSource.clip = crateBreak;
                audioSource.Play();
                break;
            case "drinkGulp":
                audioSource.clip = drinkGulp;
                audioSource.Play();
                break;
        }
    }
}
