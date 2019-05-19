using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Underwater : MonoBehaviour
{
	// Water details
	public float waterLevel;
	private bool isUnderwater;
	private Color normalColor;
	private Color underwaterColor;

	private Player player;

	// Handling air and bubbles for the player
	public Image bubble001;
    public Image bubble002;
    public Image bubble003;
    public Image bubble004;
    public Image bubble005;
	private int currentBubble;
	private int numBubbles;
	private bool loseBubbleCompleted;
	private bool gainBubbleCompleted;
	private bool bubbleCheckCompleted;

    // Start is called before the first frame update
    void Start()
    {
    	// Initialize color for fog
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.40f, 0.65f, 0.77f, 0.5f);

        // Get reference to player
        player = GameObject.FindObjectOfType<Player>();

        // Initialize bubble variables
        currentBubble = 0;
        isUnderwater = false;
        numBubbles = 5;
        loseBubbleCompleted = false;
        gainBubbleCompleted = false;
        bubbleCheckCompleted = true;
    }

    // Update is called once per frame
    void Update()
    {
    	// Take off health when player has no air
    	if (numBubbles == 0 && bubbleCheckCompleted)
    	{
    		bubbleCheckCompleted = false;
    		Invoke("BubbleCheck", 1);
    	}

    	// Player becomes submerged or emerges from water
        if ((transform.position.y < waterLevel) != isUnderwater)
        {
        	isUnderwater = transform.position.y < waterLevel;
        	if (isUnderwater)
        	{
        		SetUnderwater();
        		loseBubbleCompleted = false;
        		Invoke("LoseBubble", 1);
        	}

        	if (!isUnderwater)
        	{
        		SetNormal();
        		gainBubbleCompleted = false;
				Invoke("GainBubble", 1);
        	}
        }

        // Player remains underwater
        else if (transform.position.y < waterLevel)
        {
        	if (numBubbles > 0 && loseBubbleCompleted)
        	{
        		numBubbles -= 1;
				currentBubble += 1;
        		loseBubbleCompleted = false;
        		Invoke("LoseBubble", 1);
        	}

        }

        // Player remains above water
        else if (transform.position.y >= waterLevel)
        {
        	if (numBubbles < 5 && gainBubbleCompleted)
        	{
        		numBubbles += 1;
				currentBubble -= 1;
				gainBubbleCompleted = false;
        		Invoke("GainBubble", 1);
        	}
        }
    }

    // Player loses a bubble every second underwater
    void LoseBubble()
    {
    	if (currentBubble == 0)
    	{
    		bubble001.gameObject.SetActive(false);
    	}

    	else if (currentBubble == 1)
    	{
    		bubble002.gameObject.SetActive(false);
    	}

        else if (currentBubble == 2)
        {
            bubble003.gameObject.SetActive(false);
        }

        else if (currentBubble == 3)
        {
            bubble004.gameObject.SetActive(false);
        }

    	else
    	{
    		bubble005.gameObject.SetActive(false);
    	}
    	loseBubbleCompleted = true;
    }

    // Player gains a bubble every second above water
    void GainBubble()
    {
    	if (currentBubble == 0)
    	{
    		bubble001.gameObject.SetActive(true);
    	}

    	else if (currentBubble == 1)
    	{
    		bubble002.gameObject.SetActive(true);
    	}

        else if (currentBubble == 2)
        {
            bubble003.gameObject.SetActive(true);
        }

        else if (currentBubble == 3)
        {
            bubble004.gameObject.SetActive(true);
        }

    	else
    	{
    		bubble005.gameObject.SetActive(true);
    	}
    	gainBubbleCompleted = true;
    }

    // Player loses 20 health every second they have no air
    void BubbleCheck()
    {
    	player.health -= 20;
    	bubbleCheckCompleted = true;
    }

    // Adjust fog for underwater effect
    void SetUnderwater()
    {
    	RenderSettings.fogColor = underwaterColor;
    	RenderSettings.fogDensity = 0.02f;
    }

    // Set fog back to normal
    void SetNormal()
    {
    	RenderSettings.fogColor = normalColor;
    	RenderSettings.fogDensity = 0.004f;
    }
}
