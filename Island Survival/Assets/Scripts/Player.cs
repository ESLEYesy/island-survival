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

    // Start is called before the first frame update
    void Start()
    {
        EnablePlayer();
				if (isLocalPlayer)
				{
						Camera.main.transform.position = this.transform.position - this.transform.forward*10 + this.transform.up*3;
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
}
