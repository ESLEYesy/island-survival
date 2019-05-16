using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Collections.Generic;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class Player : NetworkBehaviour
{
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleRemote;

	[SyncVar]
	public string chatBox = "Enter message...";


    private Queue<Item> inventory  = new Queue<Item>();
    public Item equipped;

    Vector3 camDiff;
    public GameObject itemContainerPrefab;
    public GameObject textMeshPrefab;
    public GameObject interactionRadius;
    private PlayerInteraction interaction;

    private GameObject interactLabel;

    public float throwForce = 15.0f;

    // Start is called before the first frame update
    void Start()
    {
        EnablePlayer();
				if (isLocalPlayer)
				{
					Camera.main.transform.position = this.transform.position - this.transform.forward*6 + this.transform.up*6;
					Camera.main.transform.LookAt(this.transform.position);
                    camDiff = Camera.main.transform.position - this.transform.position;
					//Camera.main.transform.parent = this.transform;
				}

        //GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);

        interactLabel = Instantiate(textMeshPrefab);
        interaction = interactionRadius.GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        //CAMERA FOLLOW
        Camera.main.transform.position = this.transform.position + camDiff;

        //spawn item - K (axe)
        if (Input.GetKeyDown("k"))
        {
            GameObject spawnContainer = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward * 1.01f + new Vector3(0f, 0.5f, 0f)), Random.rotation);
            Axe newItem = spawnContainer.AddComponent(typeof(Axe)) as Axe;
            Debug.Log("Spawned a " + newItem.Name + " " + newItem.Title + ".");

            spawnContainer.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
            spawnContainer.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), Random.Range(0f, 2f)));
        }

        //drop item - X
        if (equipped != null && Input.GetKeyDown("x"))
        {
            GameObject spawnContainer = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward * 1.01f + new Vector3(0f, 0.5f, 0f)), Random.rotation);
            Item newItem = spawnContainer.AddComponent(equipped.GetType()) as Item;
            Debug.Log("Dropped a " + equipped.Name + ".");
            equipped = null;

            spawnContainer.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
            spawnContainer.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), Random.Range(0f, 2f)));
        }

        //use item - MOUSE1
        if (equipped != null && Input.GetMouseButtonDown(0))
        {
            equipped.UseItem(this);
        }

        //interact item - E
        GameObject closestObject = interaction.closest;
        if (Input.GetKeyDown("e"))
        {
            if (closestObject == null) // nothing to interact with
            {
                Debug.Log("There is no item in interacting distance");

            }
            else // something to interact with!
            {
                Item pickup = closestObject.GetComponent<Item>();
                if (pickup != null) // interactable is an item
                {

                    if (equipped == null) // pick up the item
                    {
                        equipped = pickup;
                        Debug.Log("Picked up '" + pickup.Name + "'!");
                        //closestObject
                        //interaction.DestroyObject(closestObject);
                        //Destroy(closestObject);
                    }
                    else // already have an item - do nothing
                    {
                        Debug.Log("Cannot pick up '" + pickup.Name + "' - you already have an item.");
                        closestObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 4f), Random.Range(0f, 1f)));
                        closestObject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), Random.Range(0f, 2f)));
                    }

                }
                else //interactable is not an item
                {
                    Debug.Log("Interacted with '" + closestObject.GetComponent<Item>().Name + "' !");
                    //interaction.DestroyObject(closestObject);
                }
            }
        }
        //update closest interactable label
        if (closestObject != null)
        {
            interactLabel.transform.position = closestObject.transform.position + new Vector3(0f, 0.4f, 0f);
            interactLabel.transform.rotation = Camera.main.transform.rotation;
            interactLabel.GetComponent<TextMesh>().text = closestObject.GetComponent<Item>().Name;

        }
        else
        {
            interactLabel.GetComponent<TextMesh>().text = "";
        }

    }
    
    private void FixedUpdate()
    {
        
    }

    void DisablePlayer()
    {
    	onToggleShared.Invoke(false);

    	if (isLocalPlayer)
    	{
    		onToggleLocal.Invoke(false);
    	}

    	else
    	{
    		onToggleRemote.Invoke(false);
    	}
    }

    void EnablePlayer()
    {
    	onToggleShared.Invoke(true);

    	if (isLocalPlayer)
    	{
    		onToggleLocal.Invoke(true);
    	}

    	else
    	{
    		onToggleRemote.Invoke(true);
    	}
    }

		void OnGUI()
		{
			if (isLocalPlayer)
			{
				chatBox = GUI.TextField(new Rect(25, Screen.height - 40, 120, 30), chatBox);
				if (GUI.Button(new Rect(150, Screen.height - 40, 80, 30), "Send"))
				{

				}
			}
		}
}
