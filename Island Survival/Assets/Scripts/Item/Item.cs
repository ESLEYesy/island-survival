using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{

    public readonly new string name = "Null_Item";
    public readonly new string title = "Item";

    public bool UseItem(Player user)
    {
        return false;
    }

}
