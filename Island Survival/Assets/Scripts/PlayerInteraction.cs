using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    //public GameObject player;
    private List<GameObject> interactable = new List<GameObject>();
    public Material noOutline;
    public Material outline;

    public GameObject closest;

    public void Update()
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

    public List<GameObject> ObjectList()
    {
        return interactable;
    }

    public void RemoveObject(GameObject toRemove)
    {
        interactable.Remove(toRemove);
    }

    public void DestroyObject(GameObject toDestroy)
    {
        RemoveObject(toDestroy);
        Destroy(toDestroy);
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

            Interactable i = other.gameObject.GetComponent<Interactable>();
            if(i != null)
            {
                Debug.Log(i.Title + " '" + i.Name + "' is within interacting range.");
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

            Interactable i = other.gameObject.GetComponent<Interactable>();
            if (i != null)
            {
                Debug.Log(i.Title + " '" + i.Name + "' is no longer within interacting range.");
                other.GetComponent<MeshRenderer>().material = noOutline;
            }
        }
    }
}
