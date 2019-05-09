using UnityEngine;
using UnityEngine.Events;
using Mirror;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class Player : NetworkBehaviour
{
	[SerializeField] ToggleEvent onToggleShared;
	[SerializeField] ToggleEvent onToggleLocal;
	[SerializeField] ToggleEvent onToggleRemote;

	[SyncVar]
	public string chatBox = "Enter message...";

    // Start is called before the first frame update
    void Start()
    {
        EnablePlayer();
				if (isLocalPlayer)
				{
					Camera.main.transform.position = this.transform.position - this.transform.forward*8 + this.transform.up*3;
					Camera.main.transform.LookAt(this.transform.position);
					Camera.main.transform.parent = this.transform;
				}

				//GetComponent<NetworkAnimator>().SetParameterAutoSend(0,true);
    }

    // Update is called once per frame
    void Update()
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
