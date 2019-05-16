using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{

    public GameObject hitbox;
    public readonly new string name = "Axe";
    
    public bool useItem(Player user)
    {
        Instantiate(hitbox);
        return true;
    }

}
