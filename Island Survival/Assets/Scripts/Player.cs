using UnityEngine;
using UnityEngine.Events;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{
    #region "instance vars"
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SyncVar]
    public string chatBox = "Enter message...";

    Vector3 camDiff;

    //Items
    private GameObject[] inventory = new GameObject[5];
    private int inventorySpaceSelected;
    public GameObject itemMountPoint;
    public ItemManager itemManager;

    public GameObject itemSpawnPoint;
    public float throwForce = 15.0f;

    // Inventory
    public Image inventoryBorder001;
    public Image inventoryBorder002;
    public Image inventoryBorder003;
    public Image inventoryBorder004;
    public Image inventoryBorder005;
    private List<Image> InventoryItemBorders;
    public Image inventoryBack001;
    public Image inventoryBack002;
    public Image inventoryBack003;
    public Image inventoryBack004;
    public Image inventoryBack005;
    private List<Image> InventoryItemBacks;

    //Interaction
    public GameObject interactionRadius;
    private PlayerInteraction interaction;

    // Health
    public float health;
    public float healthMax;
    public Image healthBar;

    // Energy
    public float energy;
    public float energyMax;
    public Image energyBar;
    public int jumpCost;

    //Labels for interactive world objects
    public GameObject textMeshPrefab;
    private List<GameObject> interactLabels;
    private GameObject interactLabel;
    public bool showAllLabels;

    // Player name
    public GameObject textMeshName;
    [SyncVar]
    public string playerName;

    // UI
    public GameObject dashboard;
    public float bottomPadding = 50f;

    // Animator
    Animator animator;

    // localPlayer public boolean for other scripts
    public bool localPlayer;

    // Array to store all players
    public GameObject[] players;
    #endregion

    // Menus
    public GameObject menu;
    public bool menuNotActive;

    // Start is called before the first frame update
    void Start()
    {
        // Grab all players
        players = GameObject.FindGameObjectsWithTag("Player");

        localPlayer = isLocalPlayer;
        EnablePlayer();
        if (isLocalPlayer)
        {
            Camera.main.transform.position = this.transform.position - this.transform.forward * 6 + this.transform.up * 6 + this.transform.right * 4;
            Camera.main.transform.LookAt(this.transform.position);
            camDiff = Camera.main.transform.position - this.transform.position;
            //Camera.main.transform.parent = this.transform;
            //controls = GameObject.FindObjectOfType<Controls>();

            // Set health and energy
            health = healthMax = energy = energyMax = 100;
            jumpCost = 5;

            animator = gameObject.GetComponent<Animator>();

            dashboard.transform.position = new Vector3(Screen.width / 2, bottomPadding, 0f);

            //GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);

            interactLabels = new List<GameObject>();
            interactLabel = Instantiate(textMeshPrefab);
            interaction = interactionRadius.GetComponent<PlayerInteraction>();

            // Update health and energy every second
            InvokeRepeating("UpdateEnergy", 1f, 1f);

            // Set current inventory item image
            inventorySpaceSelected = 0;
            showAllLabels = false;

            InventoryItemBorders = new List<Image>
            {
                inventoryBorder001,
                inventoryBorder002,
                inventoryBorder003,
                inventoryBorder004,
                inventoryBorder005
            };

            InventoryItemBacks = new List<Image>
            {
                inventoryBack001,
                inventoryBack002,
                inventoryBack003,
                inventoryBack004,
                inventoryBack005
            };

            // Name setup
            textMeshName.transform.position = transform.position + new Vector3(0f, 2f, 0f);
            if (StaticData.PlayerName != null && StaticData.PlayerName != "")
            {
                playerName = StaticData.PlayerName;
                textMeshName.GetComponent<TextMesh>().text = playerName;
            }

            else
            {
                string randName = "Player ";
                randName += players.Length.ToString();
                playerName = randName;
                textMeshName.GetComponent<TextMesh>().text = playerName;
            }  
            // Menu
            menuNotActive = true;
        }

        else
        {
            // Disable UI elements for non local player
            dashboard.SetActive(false);
        }
    }

    private Vector3 RandomRange(float min, float max, bool upwardsBias)
    {
        return new Vector3(UnityEngine.Random.Range(min, max),
            (upwardsBias ? Math.Abs(UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)) : UnityEngine.Random.Range(min, max) * (upwardsBias ? 3f : 1f)),
            UnityEngine.Random.Range(min, max));
    }

    // Update is called once per frame
    void Update()
    {
        // Name follow
        textMeshName.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        textMeshName.GetComponent<TextMesh>().text = playerName;

        localPlayer = isLocalPlayer;
        if (isLocalPlayer)
        {
            // Camera follow
            Camera.main.transform.position = this.transform.position + camDiff;

            //spawn item - K (axe)
            if (Input.GetKeyDown("k"))
            {
                GameObject spawnItem = Instantiate(itemManager.itemCratePrefab, this.itemSpawnPoint.transform.position, itemSpawnPoint.transform.rotation);
                Debug.Log("Spawned '" + spawnItem.GetComponent<Interactable>().Name + "'!");
                spawnItem.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
                spawnItem.GetComponent<Rigidbody>().AddTorque(RandomRange(-15f, 15f, false));
            }

            //drop item - X
            if (inventory[inventorySpaceSelected] != null && Input.GetKeyDown("x"))
            {
                GameObject itemPrefab = inventory[inventorySpaceSelected];
                itemPrefab.transform.parent = null;
                itemPrefab.transform.position = this.itemSpawnPoint.transform.position;
                itemPrefab.transform.rotation = this.itemSpawnPoint.transform.rotation;
                itemPrefab.tag = "Interactive";
                inventory[inventorySpaceSelected] = null;
                itemPrefab.SetActive(true);

                Debug.Log("Dropped a " + itemPrefab.GetComponent<Item>().Name + ".");

                itemPrefab.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
                itemPrefab.GetComponent<Rigidbody>().AddTorque(RandomRange(-15f, 15f, false));

                updateInventory();
            }

            //use item - MOUSE1
            if (inventory[inventorySpaceSelected] != null && Input.GetMouseButtonDown(0))
            {
                itemManager.UseItem(this, inventory[inventorySpaceSelected].GetComponent<Item>().Name);
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
                        int inventorySpace = HasInventorySpace();
                        if (inventorySpace >= 0) // pick up the item
                        {

                            inventory[inventorySpace] = closestObject;
                            GameObject newItem = inventory[inventorySpace];
                            newItem.transform.parent = this.transform;
                            newItem.tag = "Untagged";
                            newItem.SetActive(false);
                            interaction.RemoveObject(newItem);

                            Debug.Log(this.playerName + " has picked up '" + pickup.Name + "'!");

                            updateInventory();
                        }
                        else // no space in inventory
                        {
                            Debug.Log("Cannot pick up '" + pickup.Name + "' - you have no room in your inventory!.");
                            closestObject.GetComponent<Rigidbody>().AddForce(RandomRange(-7f, 7f, true));
                            closestObject.GetComponent<Rigidbody>().AddTorque(RandomRange(-10f, -10f, false)); //toss it around
                        }

                    }
                    else //interactable is not an item
                    {
                        closestObject.GetComponent<Interactable>().Interact(this);
                    }
                }
            }


            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                foreach (GameObject o in interaction.ObjectList())
                { // add all interactable labels
                    if (o != closestObject)
                    {
                        GameObject newLabel = Instantiate(textMeshPrefab);
                        interactLabels.Add(newLabel);
                        newLabel.GetComponent<TextMesh>().text = o.GetComponent<Interactable>().Name;
                        newLabel.transform.position = o.transform.position + new Vector3(0f, 0.4f, 0f);
                        newLabel.transform.rotation = Camera.main.transform.rotation;
                        newLabel.transform.parent = o.transform;
                    }
                }

                showAllLabels = true;
            }

            /*
            if (showAllLabels)
            {
                foreach(GameObject o in interaction.ObjectList())
                {
                    if (!interactLabels.Contains(o)) //new object in range without a label
                    {
                        GameObject newLabel = Instantiate(textMeshPrefab);
                        interactLabels.Add(newLabel);
                        newLabel.GetComponent<TextMesh>().text = o.GetComponent<Item>().Name;
                        newLabel.transform.position = o.transform.position + new Vector3(0f, 0.4f, 0f);
                        newLabel.transform.rotation = Camera.main.transform.rotation;
                        newLabel.transform.parent = o.transform;
                    }

                }
                List<GameObject> toRemove = new List<GameObject>(); 
                foreach (GameObject o in interactLabels)
                {
                    if (!interaction.ObjectList().Contains(o)) // object left range
                    {
                        toRemove.Add(o);
                    }
                }
                foreach (GameObject o in toRemove)
                {
                    interactLabels.Remove(o);
                    Destroy(o);
                }
            }*/

            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                List<GameObject> toRemove = new List<GameObject>(); // remove all labels but the closest
                foreach (GameObject o in interactLabels)
                {
                    if (o != closestObject)
                    {
                        toRemove.Add(o);
                    }
                }
                foreach (GameObject o in toRemove)
                {
                    interactLabels.Remove(o);
                    Destroy(o);
                }
                showAllLabels = false;
            }

            // Menu functionality
            // Remember to change to escape
            if (Input.GetKeyDown("m"))
            {
                menu.SetActive(menuNotActive);
                menuNotActive = !menuNotActive;
            }

            if (interactLabels.Count > 0) //update all labels
            {
                foreach (GameObject o in interactLabels)
                {
                    Transform parent = o.transform.parent;
                    o.transform.parent = null;
                    o.transform.position = parent.position + new Vector3(0f, 0.4f, 0f);
                    o.transform.rotation = Camera.main.transform.rotation;
                    o.transform.parent = parent;
                }
            }

            //update closest label
            if (closestObject != null)
            {
                interactLabel.transform.position = closestObject.transform.position + new Vector3(0f, 0.4f, 0f);
                interactLabel.transform.rotation = Camera.main.transform.rotation;
                interactLabel.GetComponent<TextMesh>().text = closestObject.GetComponent<Interactable>().Name;
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
                if (inventorySpaceSelected == 0)
                {
                    inventorySpaceSelected = inventory.Length - 1; //loop back to right
                }

                else
                {
                    inventorySpaceSelected = (inventorySpaceSelected - 1) % 5;
                }
            }
            else if (delta < 0f)
            {
                // Scrolling up goes right in inventory
                inventorySpaceSelected = (inventorySpaceSelected + 1) % 5;
            }

            // Highlight selected inventory item
            dashboard.SetActive(true);
            highlightInventorySpace(0.2f, 1f);
        }

        else
        {
            dashboard.SetActive(false);
        }
    }

    public void cutInteraction(GameObject cut)
    {
        interaction.RemoveObject(cut);
    }

    public void DestroyEquippedItem()
    {
        if (inventory[inventorySpaceSelected] != null)
        {
            GameObject itemPrefab = inventory[inventorySpaceSelected];
            inventory[inventorySpaceSelected] = null;
            Destroy(itemPrefab);


            Debug.Log("Player " + playerName + " consumed a " + itemPrefab.GetComponent<Item>().Name + ".");
            updateInventory();

        }
        else
        {
            Debug.Log("Error: Player " + playerName + " attempted to consume an item that they don't have equipped");
        }
    }

    private void highlightInventorySpace(float hiddenAlpha, float selectedAlpha)
    {
        foreach (Image i in InventoryItemBorders)
        {
            Color tempColor = i.color;
            tempColor.a = (InventoryItemBorders.IndexOf(i) == inventorySpaceSelected) ? selectedAlpha : hiddenAlpha;
            i.color = tempColor;
        }
    }

    private void updateInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            InventoryItemBacks[i].sprite = itemManager.getSprite((inventory[i] == null) ? null : inventory[i].GetComponent<Item>().Name);
            InventoryItemBacks[i].gameObject.SetActive(InventoryItemBacks[i].sprite != null);
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
        // if (isLocalPlayer)
        // {
        // 	chatBox = GUI.TextField(new Rect(25, Screen.height - 40, 120, 30), chatBox);
        // 	if (GUI.Button(new Rect(150, Screen.height - 40, 80, 30), "Send"))
        // 	{
        //
        // 	}
        // }
    }

    public void loseHealth(int damage)
    {
        this.health = (this.health <= damage) ? 0 : this.health - damage;
        if (this.health == 0) //DIE
        {
            Debug.Log("You died.");
        }
        updateHealthBar();
    }

    public void gainHealth(int healing)
    {
        this.health = (this.health >= this.healthMax - healing) ? this.healthMax : this.health + healing;
        updateHealthBar();
    }

    public void gainEnergy(int energy)
    {
        this.energy = (this.energy >= this.energyMax - energy) ? this.energyMax : this.energy + energy;
        updateEnergyBar();
    }

    public void loseEnergy(int energy)
    {
        this.energy = (this.energy <= energy) ? 0 : this.energy - energy;
        updateEnergyBar();
    }

    private void updateHealthBar()
    {
        healthBar.rectTransform.localScale = new Vector3(health / healthMax, 1f, 1f);
    }

    private void updateEnergyBar()
    {
        energyBar.rectTransform.localScale = new Vector3(energy / energyMax, 1f, 1f);
    }

    private void UpdateEnergy() // called every second
    {
        if (energy == energyMax)
        {
            this.gainHealth(1); // heal 3 health when full on energy.
        }

        if (energy > 0) // we have energy to spend
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidWalk") // Player loses 2 energy while walking/running
            {
                this.loseEnergy(2);
            }
            else if (false) // Player loses energy when fighting, etc.
            {
                // Implement later on
            }
            else // If player does nothing to lose energy, they gain energy back.
            {
                gainEnergy(5);
            }
        }
        else
        {
            this.loseHealth(1);
            this.gainEnergy(5);
            Debug.Log("You are out of energy.");
        }
    }

    public int HasInventorySpace() // returns -1 if there is no empty inventory space, otherwise returns the index of the first empty inventory space.
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    // Destroy name when player disconnects
    void OnPlayerDisconnected()
    {
        Debug.Log("Player disconnected.");
        Destroy(textMeshName);
    }
}
