using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public Weapon() : base()
    {
        Name = "Null_Weapon";
    }

    public override bool UseItem(Player user)
    {
        Debug.Log("Attempted to use Null_Weapon");
        return false;
    }

}
