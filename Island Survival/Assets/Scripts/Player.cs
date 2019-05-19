using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class Player : NetworkBehaviour
{
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleRemote;

	[SyncVar]
	public string chatBox = "Enter message...";


    private List<Item> inventory  = new List<Item>();
    public Item equipped;

    Vector3 camDiff;
    public GameObject itemContainerPrefab;
    public GameObject textMeshPrefab;
    public GameObject interactionRadius;
    private PlayerInteraction interaction;

    private GameObject interactLabel;

    public float throwForce = 15.0f;

	// Score functionality
	// private Controls controls;

	// Health
	public int health;
	public Image healthBar;

	// Energy
	public int energy;
	public Image energyBar;

    // Inventory
    public Image inventoryItem001;
    public Image inventoryItem002;
    public Image inventoryItem003;
    public Image inventoryItem004;
    public Image inventoryItem005;
    public Image inventoryPic001;
    public Image inventoryPic002;
    public Image inventoryPic003;
    public Image inventoryPic004;
    public Image inventoryPic005;
    public int currInventoryIndex;

    // Item sprites
    public Sprite axeSprite;

    // Animator
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        EnablePlayer();
		if (isLocalPlayer)
		{
			Camera.main.transform.position = this.transform.position - this.transform.forward*6 + this.transform.up*6 + this.transform.right*4;
			Camera.main.transform.LookAt(this.transform.position);
            camDiff = Camera.main.transform.position - this.transform.position;
			//Camera.main.transform.parent = this.transform;
			//controls = GameObject.FindObjectOfType<Controls>();
			health = 100;
            energy = 100;

            animator = gameObject.GetComponent<Animator>();
		}

        //GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);

        interactLabel = Instantiate(textMeshPrefab);
        interaction = interactionRadius.GetComponent<PlayerInteraction>();
    
        // Update health and energy every second
        InvokeRepeating("UpdateEnergy", 1f, 1f);
        InvokeRepeating("UpdateHealth", 1f, 1f);

        // Set current inventory item image
        currInventoryIndex = 0;

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

        if (Input.GetKeyDown("m"))
        {
            Debug.Log(Screen.currentResolution);
            Debug.Log(Screen.height);
            Debug.Log(Screen.width);
        }

        //drop item - X
        if (equipped != null && Input.GetKeyDown("x"))
        {
            GameObject spawnContainer = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward * 1.01f + new Vector3(0f, 0.5f, 0f)), Random.rotation);
            Item newItem = spawnContainer.AddComponent(equipped.GetType()) as Item;
            Debug.Log("Dropped a " + equipped.Name + ".");
            inventoryPic001.gameObject.SetActive(false);
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
                        
                        // For now just pick up Axe
                        inventoryPic001.sprite = axeSprite;
                        inventoryPic001.gameObject.SetActive(true);
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

        // Inventory scroll wheel functionality
        var delta = Input.GetAxis("Mouse ScrollWheel");
        if (delta > 0f)
        {
            // Scrolling down goes left in inventory
            if (currInventoryIndex == 0)
            {
                currInventoryIndex = 5;
            }

            else
            {
                currInventoryIndex = (currInventoryIndex - 1) % 5;
            }
        }
        else if (delta < 0f)
        {
            // Scrolling up goes right in inventory
            currInventoryIndex = (currInventoryIndex + 1) % 5;
        }

        // Highlight selected inventory item
        var tempColor001 = inventoryItem001.color;
        var tempColor002 = inventoryItem002.color;
        var tempColor003 = inventoryItem003.color;
        var tempColor004 = inventoryItem004.color;
        var tempColor005 = inventoryItem005.color;
        if (currInventoryIndex == 0)
        {
            tempColor001.a = 1f;
            tempColor002.a = 0.2f;
            tempColor003.a = 0.2f;
            tempColor004.a = 0.2f;
            tempColor005.a = 0.2f;
        }

        else if (currInventoryIndex == 1)
        {
            tempColor001.a = 0.2f;
            tempColor002.a = 1f;
            tempColor003.a = 0.2f;
            tempColor004.a = 0.2f;
            tempColor005.a = 0.2f;
        }

        else if (currInventoryIndex == 2)
        {
            tempColor001.a = 0.2f;
            tempColor002.a = 0.2f;
            tempColor003.a = 1f;
            tempColor004.a = 0.2f;
            tempColor005.a = 0.2f;
        }

        else if (currInventoryIndex == 3)
        {
            tempColor001.a = 0.2f;
            tempColor002.a = 0.2f;
            tempColor003.a = 0.2f;
            tempColor004.a = 1f;
            tempColor005.a = 0.2f;
        }

        else 
        {
            tempColor001.a = 0.2f;
            tempColor002.a = 0.2f;
            tempColor003.a = 0.2f;
            tempColor004.a = 0.2f;
            tempColor005.a = 1f;
        }
        // Assign new colors
        inventoryItem001.color = tempColor001;
        inventoryItem002.color = tempColor002;
        inventoryItem003.color = tempColor003;
        inventoryItem004.color = tempColor004;
        inventoryItem005.color = tempColor005;
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
			// if (isLocalPlayer)
			// {
			// 	chatBox = GUI.TextField(new Rect(25, Screen.height - 40, 120, 30), chatBox);
			// 	if (GUI.Button(new Rect(150, Screen.height - 40, 80, 30), "Send"))
			// 	{
			//
			// 	}
			// }
		}

	void UpdateHealth()
    {
        if (health == 0)
        {
            Debug.Log("You died.");
            healthBar.rectTransform.localScale = new Vector2(0f, 1f);
        }

        else if (health > 0)
        {
            healthBar.rectTransform.localScale = new Vector2(health/100f, 1f);
        }

        // Full energy allows you to regen health
        else if (energy == 100 && health < 100)
        {
            health += 1;
        }
    }

    void UpdateEnergy()
    {
        // Player loses 5 energy when jumping
        if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidFall" ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidJumpForwardLeft" ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidMidAirRight" ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidJumpForwardRight" ||
            animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidMidAirLeft")
        {
            if (energy > 0)
            {
                energy -= 5;
            }

            else
            {
                // Do something
                Debug.Log("You are out of energy.");
            }

        }

        // Player loses 2 energy while walking/running
        else if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidWalk")
        {
            if (energy > 0)
            {
                energy -= 2;
            }

            else
            {
                // Lose health
                health -= 10;
            }
        }

        // Player loses energy when fighting, etc.
        else if (false)
        {
            // Implement later on
        }

        // Player regains energy
        else
        {
            if (energy < 100)
            {
                energy += 1;
            }
        }

        // Update energy bar
        if (energy > 0)
        {
            energyBar.rectTransform.localScale = new Vector2(energy/100f, 1f);
        }

        else
        {
            energyBar.rectTransform.localScale = new Vector2(0f, 1f);
        }
    }
}
