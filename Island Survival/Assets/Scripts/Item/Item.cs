using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable
{

    private GameObject itemManager;

    public Item()
    {
        Title = "Item";
        Name = "Null_Item";
    }

    public virtual bool UseItem(Player user)
    {
        Debug.Log("Attempted to use Null_Item");
        return false;
    }

}
