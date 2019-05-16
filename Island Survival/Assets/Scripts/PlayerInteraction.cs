using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    private List<GameObject> interactable = new List<GameObject>();
    public Material noOutline;
    public Material outline;

    public GameObject closest;

    public void FixedUpdate()
    {
        closest = closestObject();
    }

    public GameObject closestObject()
    {
        if (HasInteractableObjects())
        {
            GameObject closest = null;
            float bestDist = float.MaxValue;

            foreach (GameObject obj in interactable)
            {
                float testDist = Vector3.Distance(obj.transform.position, this.transform.position);
                if (testDist < bestDist)
                {
                    closest = obj;
                    bestDist = testDist;
                }                
                obj.GetComponent<MeshRenderer>().material = noOutline;

            }

            closest.GetComponent<MeshRenderer>().material = outline;
            return closest;
        }
        else
        {
            return null;
        }
    }

    public void RemoveObject(GameObject rem)
    {
        interactable.Remove(rem);
    }

    public bool HasInteractableObjects()
    {
        return (interactable.Count != 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            interactable.Add(other.gameObject);

            Item test = other.gameObject.GetComponent<Item>();
            if(test != null)
            {
                Debug.Log("Item '" + test.getName() + "' is within interacting range!");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactive"))
        {
            if (interactable.Contains(other.gameObject)){ 
                interactable.Remove(other.gameObject);
            } else
            {
                Debug.Log("Error! We have lost interaction with an object we never were able to interact with");
            }

            Item test = other.gameObject.GetComponent<Item>();
            if (test != null)
            {
                Debug.Log("Item '" + test.getName() + "' is no longer within interacting range!");
                other.GetComponent<MeshRenderer>().material = noOutline;
            }
        }
    }
}
