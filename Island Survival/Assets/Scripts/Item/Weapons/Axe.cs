using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{

    public Axe() : base()
    {
        Name = "Axe";
    }

    public override bool UseItem(Player user)
    {
        Debug.Log("Axe swung!");
        Instantiate();
        return true;
    }

}
