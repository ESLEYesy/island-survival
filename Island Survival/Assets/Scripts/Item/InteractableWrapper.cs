using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWrapper : MonoBehaviour
{
    public Interactable i { get; }

    public InteractableWrapper(Interactable i)
    {
        this.i = i;
    }
}
