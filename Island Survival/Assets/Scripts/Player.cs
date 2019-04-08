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
