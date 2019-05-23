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

    public bool isDead;

    // Energy
    public float energy;
    public float energyMax;
    public Image energyBar;
    public int jumpCost;

    //Labels for interactive world objects
    public GameObject textMeshPrefab;
    private List<GameObject> interactLabels;
    private GameObject interactLabel;
    private GameObject mouseLabel;
    public bool showAllLabels;

    // Player name
    public GameObject textMeshName;
    [SyncVar]
    public string playerName;

    // UI
    public GameObject dashboard;
    public float bottomPadding = 50f;
    public GameObject deathOverlay;
    public GameObject dimObject;

    // Animator
    Animator animator;

    // localPlayer public boolean for other scripts
    public bool localPlayer;

    // Array to store all players
    public GameObject[] players;
    

    // Menus
    public GameObject menu;
    public bool menuNotActive;
    public GameObject settingsMenu;
    public GameObject networkManager;

    // Sound
    public AudioSource voiceSound;
    public AudioSource actionSound;
    public AudioSource uiSound;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Grab all players
        players = GameObject.FindGameObjectsWithTag("Player");
        // Get network manager
        networkManager = GameObject.FindGameObjectsWithTag("NetworkManager")[0];
        itemManager = Camera.main.transform.GetChild(3).gameObject.GetComponent<ItemManager>();

        isDead = false;
        health = healthMax = energy = energyMax = 100;
        jumpCost = 5;
        animator = gameObject.GetComponent<Animator>();
        // Update health and energy every second
        InvokeRepeating("UpdateEnergy", 1f, 1f);

        localPlayer = isLocalPlayer;
        EnablePlayer();
        if (isLocalPlayer)
        {
            uiSound = Camera.main.transform.GetChild(2).GetComponent<AudioSource>();
            Camera.main.transform.position = this.transform.position - this.transform.forward * 6 + this.transform.up * 6 + this.transform.right * 4;
            Camera.main.transform.LookAt(this.transform.position);
            camDiff = Camera.main.transform.position - this.transform.position;
            //Camera.main.transform.parent = this.transform;
            //controls = GameObject.FindObjectOfType<Controls>();

            // Set health and energy
            dashboard.transform.position = new Vector3(Screen.width / 2, bottomPadding, 0f);
            dimObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            //GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);

            interactLabels = new List<GameObject>();
            interactLabel = GameObject.Instantiate(textMeshPrefab);
            mouseLabel = GameObject.Instantiate(textMeshPrefab);
            interaction = interactionRadius.GetComponent<PlayerInteraction>();

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

    private void interact(GameObject obj)
    {
        Item pickup = obj.GetComponent<Item>();

        if (pickup != null) // interactable is an item
        {
            int inventorySpace = HasInventorySpace();
            if (inventorySpace >= 0) // pick up the item
            {

                inventory[inventorySpace] = obj;
                GameObject newItem = inventory[inventorySpace];
                newItem.transform.position = itemMountPoint.transform.position;
                newItem.transform.rotation = itemMountPoint.transform.rotation;
                newItem.transform.parent = itemMountPoint.transform;
                newItem.tag = "Untagged";
                newItem.GetComponent<Rigidbody>().isKinematic = true;
                newItem.GetComponent<Collider>().enabled = false;
                if (inventorySpace != inventorySpaceSelected)
                {
                    newItem.SetActive(false);
                }
                interaction.RemoveObject(newItem);

                Debug.Log(this.playerName + " has picked up '" + pickup.Name + "'!");
                PlaySound("itemPickup", "ui");
                
                updateInventory();
            }
            else // no space in inventory
            {
                obj.GetComponent<Rigidbody>().AddForce(itemManager.RandomRange(-7f, 7f, true));
                obj.GetComponent<Rigidbody>().AddTorque(itemManager.RandomRange(-15f, 15f, false)); //toss it around
                PlaySound("interfaceError", "ui");
            }

        }
        else //interactable is not an item
        {
            obj.GetComponent<Interactable>().Interact(this);
        }
    }

    void Update()
    {
        // Name follow
        textMeshName.transform.position = transform.position + new Vector3(0f, 2f, 0f);
        textMeshName.GetComponent<TextMesh>().text = playerName;

        if (!isDead)
        {
            //spawn custom item - K
            if (Input.GetKeyDown("k"))
            {
                itemManager.SpawnItem(itemManager.GetItem("ItemCrate"), transform.position + transform.forward * 2 + transform.up * 2, UnityEngine.Random.rotation, this.transform.forward * throwForce, itemManager.RandomRange(-15f, 15f, false));
                PlaySound("itemDrop", "action");
                Debug.Log("Spawned an item crate !");
            }

            //drop selected item - X
            if (inventory[inventorySpaceSelected] != null && Input.GetKeyDown("x"))
            {
                GameObject itemPrefab = inventory[inventorySpaceSelected];
                itemPrefab.transform.parent = null;
                itemPrefab.transform.position = this.itemSpawnPoint.transform.position;
                itemPrefab.transform.rotation = this.itemSpawnPoint.transform.rotation;
                itemPrefab.tag = "Interactive";
                inventory[inventorySpaceSelected] = null;
                itemPrefab.GetComponent<Rigidbody>().isKinematic = false;
                itemPrefab.GetComponent<Collider>().enabled = true;
                itemPrefab.SetActive(true);

                Debug.Log("Dropped a " + itemPrefab.GetComponent<Item>().Name + ".");
                PlaySound("itemDrop");

                itemPrefab.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
                itemPrefab.GetComponent<Rigidbody>().AddTorque(itemManager.RandomRange(-15f, 15f, false));

                updateInventory();
            }

            //use selected item - MOUSE1
            if (inventory[inventorySpaceSelected] != null && Input.GetMouseButtonDown(0))
            {
                itemManager.UseItem(this, inventory[inventorySpaceSelected].GetComponent<Item>().Name);
            }

            GameObject closestObject = interaction.closest;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool mouseOverInteractive = Physics.Raycast(ray, out hit, 500f, (1 << 8));
            GameObject found = null;
            if (mouseOverInteractive)
            {
                found = hit.collider.gameObject;
            }

            //interact with mouseover - MOUSE2
            if (Input.GetMouseButtonDown(1))
            {
                if (mouseOverInteractive)
                {
                    if (interaction.ObjectList().Contains(found)) //found item is interactable
                    {
                        interact(found);
                    }
                }
            }

            //interact with closest object - E
            if (Input.GetKeyDown("e"))
            {
                if (closestObject != null) // nothing to interact with
                {
                    interact(closestObject);
                }
            }

            localPlayer = isLocalPlayer;
            if (isLocalPlayer)
            {
                //Camera follow
                Camera.main.transform.position = this.transform.position + camDiff;

                //update mouse label
                if (mouseOverInteractive)
                {
                    mouseLabel.transform.position = found.transform.position + new Vector3(0f, 0.4f, 0f);
                    mouseLabel.transform.rotation = Camera.main.transform.rotation;
                    mouseLabel.GetComponent<TextMesh>().text = found.GetComponent<Interactable>().Name;
                }
                else
                {
                    mouseLabel.GetComponent<TextMesh>().text = "";
                }

                //update closest interactive label
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

                //show nearby objects - leftAlt
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

                //update nearby labels (from show nearby objects)
                if (interactLabels.Count > 0)
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

                // Toggle menu - Escape
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    PlaySound("interfaceError");
                    menu.SetActive(menuNotActive);
                    menuNotActive = !menuNotActive;
                }

                // Scroll through inventory
                var delta = Input.GetAxis("Mouse ScrollWheel");
                if (delta != 0f)
                {
                    if (inventory[inventorySpaceSelected] != null)
                        inventory[inventorySpaceSelected].SetActive(false);
                    PlaySound("lightTick");
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
                    else
                    {
                        // Scrolling up goes right in inventory
                        inventorySpaceSelected = (inventorySpaceSelected + 1) % 5;
                    }
                    if (inventory[inventorySpaceSelected] != null)
                        inventory[inventorySpaceSelected].SetActive(true);

                }

                // Highlight selected inventory item
                dashboard.SetActive(true);
                highlightInventorySpace(0.2f, 1f);

            }
            else
            {
                dashboard.SetActive(false);
            }

        } else //dead
        {
            Color oc = dimObject.GetComponent<Image>().color;
            dimObject.GetComponent<Image>().color = new Color(oc.r, oc.g, oc.b, (oc.a + 0.25f * (Time.deltaTime)));
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
            InventoryItemBacks[i].sprite = itemManager.GetSprite((inventory[i] == null) ? null : inventory[i].GetComponent<Item>().Name);
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

    public void Die()
    {
        if (!isDead)
        {
            this.health = 0;
            UpdateHealthBar();

            Debug.Log(playerName + " died.");
            itemManager.SpawnParticle("HPParticle", transform.position + new Vector3(0f, 3f, 0f), ("D E A D"));
            isDead = true;
            PlaySound("playerDeath", "voice");

            deathOverlay.SetActive(true);
            dimObject.SetActive(true);
            Invoke("QuitGame", 4f);
        }
    }

    public void LoseHealth(int damage)
    {
        if (!isDead)
        {
            itemManager.SpawnParticle("HPParticle", transform.position + new Vector3(0f, 3f, 0f), ("-" + damage));
            if (damage >= 10)
            {
                PlaySound("takeDamage", "voice");
            }
            this.health = (this.health <= damage) ? 0 : this.health - damage;
            if (this.health == 0) //DIE
            {
                Die();
            }
            UpdateHealthBar();
        }
    }

    public void GainHealth(int healing)
    {
        if (!isDead)
        {
            if (healing >= 10)
            {
                PlaySound("healDamage");
            }
            this.health = (this.health >= this.healthMax - healing) ? this.healthMax : this.health + healing;
            UpdateHealthBar();
        }
    }

    public void GainEnergy(int energy)
    {
        if (!isDead)
        {
            this.energy = (this.energy >= this.energyMax - energy) ? this.energyMax : this.energy + energy;
            UpdateEnergyBar();
        }
    }

    public void LoseEnergy(int energy)
    {
        if (!isDead)
        {
            this.energy = (this.energy <= energy) ? 0 : this.energy - energy;
            UpdateEnergyBar();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.rectTransform.localScale = new Vector3(health / healthMax, 1f, 1f);
    }

    private void UpdateEnergyBar()
    {
        energyBar.rectTransform.localScale = new Vector3(energy / energyMax, 1f, 1f);
    }

    private void UpdateEnergy()
    {
        if (energy == energyMax)
        {
            this.GainHealth(1); // heal 3 health when full on energy.
        }

        if (energy > 0) // we have energy to spend
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HumanoidWalk") // Player loses 2 energy while walking/running
            {
                this.LoseEnergy(2);
            }else // If player does nothing to lose energy, they gain energy back.
            {
                GainEnergy(5);
            }
        }
        else
        {
            this.LoseHealth(1);
            this.GainEnergy(5);
            Debug.Log("You are out of energy.");
        }
    }

    public void PlaySound(string sound)
    {
        PlaySound(sound, "ui");
    }

    public void PlaySound(string sound, string target)
    {
        AudioSource source = uiSound;
        if(target == "action")
        {
            source = actionSound;
        } else if (target == "voice")
        {
            source = voiceSound;
        }
        itemManager.PlaySound(sound, source);
    }

    public int HasInventorySpace() //returns -1 if there is no empty inventory space, otherwise returns the index of the first empty inventory space.
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

    public void QuitGame()
    {
        Destroy(textMeshName);
        networkManager.GetComponent<NetworkManagerHUD>().quitButtonClicked = true;
    }
}
