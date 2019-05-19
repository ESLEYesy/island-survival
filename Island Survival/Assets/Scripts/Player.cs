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
    [SerializeField] ToggleEvent onToggleShared;
    [SerializeField] ToggleEvent onToggleLocal;
    [SerializeField] ToggleEvent onToggleRemote;

    [SyncVar]
    public string chatBox = "Enter message...";


    private List<Item> inventory = new List<Item>();
    public Type equipped;
    private GameObject equippedObject;

    public GameObject itemMount;
    public GameObject axePrefab;

    Vector3 camDiff;
    public GameObject itemContainerPrefab;
    public GameObject textMeshPrefab;
    public GameObject interactionRadius;
    private PlayerInteraction interaction;

    private GameObject interactLabel;

    public float throwForce = 15.0f;

    // Score functionality
    // private Controls controls;

    //UI
    public GameObject dashboard;
    public float bottomPadding = 50f;

    // Health
    public float health;
    public float healthMax;
    public Image healthBar;

    // Energy
    public float energy;
    public float energyMax;
    public Image energyBar;

    public int jumpCost;

    // Inventory
    public GameObject inventoryUI;
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

    // localPlayer public boolean for other scripts
    public bool localPlayer;

    // Start is called before the first frame update
    void Start()
    {
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

            interactLabel = Instantiate(textMeshPrefab);
            interaction = interactionRadius.GetComponent<PlayerInteraction>();

            // Update health and energy every second
            InvokeRepeating("UpdateEnergy", 1f, 1f);
            InvokeRepeating("UpdateHealth", 1f, 1f);

            // Set current inventory item image
            currInventoryIndex = 0;

            
        }

        else
        {
            // Disable inventory for non local player
            inventoryUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        localPlayer = isLocalPlayer;
        if (isLocalPlayer)
        {
            //CAMERA FOLLOW
            Camera.main.transform.position = this.transform.position + camDiff;

            //spawn item - K (axe)
            if (Input.GetKeyDown("k"))
            {
                GameObject spawnContainer = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward * 1.01f + new Vector3(0f, 0.5f, 0f)), UnityEngine.Random.rotation);
                Axe newItem = spawnContainer.AddComponent(typeof(Axe)) as Axe;
                Debug.Log("Spawned a " + newItem.Name + " " + newItem.Title + ".");

                spawnContainer.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
                spawnContainer.GetComponent<Rigidbody>().AddTorque(new Vector3(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f)));
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
                GameObject spawnContainer = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward * 1.01f + new Vector3(0f, 0.5f, 0f)), UnityEngine.Random.rotation);
                // Item newItem = spawnContainer.AddComponent(equipped.GetType()) as Item;
                spawnContainer.AddComponent<Axe>();

                Debug.Log("Dropped a " + equipped.Name + ".");

                inventoryPic001.gameObject.SetActive(false);
                equipped = null;
                Destroy(equippedObject);
                equippedObject = null;

                spawnContainer.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);
                spawnContainer.GetComponent<Rigidbody>().AddTorque(new Vector3(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f)));
            }

            //use item - MOUSE1
            if (equipped != null && Input.GetMouseButtonDown(0))
            {
                Debug.Log("Swung the axe!");
                //equipped.UseItem(this);
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
                            equipped = pickup.GetType();
                            equippedObject = Instantiate(axePrefab, itemMount.transform.position, itemMount.transform.rotation);
                            equippedObject.transform.SetParent(itemMount.transform);
                            equippedObject.GetComponent<Rigidbody>().isKinematic = true;
                            equippedObject.GetComponent<Rigidbody>().detectCollisions = false;


                            Debug.Log("Picked up '" + pickup.Name + "'!");

                            // For now just pick up Axe
                            inventoryPic001.sprite = axeSprite;
                            inventoryPic001.gameObject.SetActive(true);
                            //closestObject
                            interaction.DestroyObject(closestObject);
                            //Destroy(closestObject);
                        }
                        else // already have an item - do nothing
                        {
                            Debug.Log("Cannot pick up '" + pickup.Name + "' - you already have an item.");
                            closestObject.GetComponent<Rigidbody>().AddForce(new Vector3(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 4f), UnityEngine.Random.Range(0f, 1f)));
                            closestObject.GetComponent<Rigidbody>().AddTorque(new Vector3(UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f), UnityEngine.Random.Range(0f, 2f)));
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
            inventoryUI.SetActive(true);
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

        else
        {
            inventoryUI.SetActive(false);
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
}
