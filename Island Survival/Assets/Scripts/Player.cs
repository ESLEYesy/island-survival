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

    Vector3 camDiff;
    public GameObject itemContainerPrefab;
    public GameObject interactionRadius;
    public GameObject textMeshPrefab;

    private GameObject interactLabel;

    public float throwForce = 10.0f;

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    int itemTest = 0;
    private void FixedUpdate()
    {
        //CAMERA FOLLOW
        Camera.main.transform.position = this.transform.position + camDiff;

        //drop item test
        if (Input.GetKeyDown("x"))
        {
            GameObject newItem = Instantiate(itemContainerPrefab, (this.transform.position + this.transform.forward*1.01f + new Vector3(0f, 0.5f, 0f)), Random.rotation);

            Item itemController = newItem.GetComponent<Item>();
            itemController.setName("TestItem #" + itemTest);

            newItem.GetComponent<Rigidbody>().AddForce(this.transform.forward * throwForce);

            Debug.Log("TestItem #" + itemTest + " created.");
            itemTest++;

        }

        GameObject closestObject = interactionRadius.GetComponent<PlayerInteraction>().closest;

        //pick up item test
        if (Input.GetKeyDown("j"))
        {
            if(closestObject == null)
            {
                Debug.Log("There is no item in interacting distance");

            } else
            {
                Debug.Log("Interacted with '" + closestObject.GetComponent<Item>().getName() + "' !");
                interactionRadius.GetComponent<PlayerInteraction>().RemoveObject(closestObject);
                Destroy(closestObject);
            }

        }

        
        //label test
        if (closestObject != null)
        {
            interactLabel.transform.position = closestObject.transform.position + new Vector3(0f, 0.4f, 0f);
            interactLabel.transform.rotation = Camera.main.transform.rotation;
            interactLabel.GetComponent<TextMesh>().text = closestObject.GetComponent<Item>().getName();

        } else
        {
            interactLabel.GetComponent<TextMesh>().text = "";
        }
        
        


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
