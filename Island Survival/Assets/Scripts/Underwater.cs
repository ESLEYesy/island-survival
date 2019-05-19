using System;
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
    public GameObject airLabel;

    // Handling air and bubbles for the player
    public Image bubble001;
    public Image bubble002;
    public Image bubble003;
    public Image bubble004;
    public Image bubble005;
    private List<Image> bubbles;
    private int currentBubble;
    private int numBubbles;
    private bool loseBubbleCompleted;
    private bool gainBubbleCompleted;
    private bool bubbleCheckCompleted;
    private bool isHidden;

    private bool hideTimer;

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

        //list form
        bubbles = new List<Image>();
        bubbles.Add(bubble001);
        bubbles.Add(bubble002);
        bubbles.Add(bubble003);
        bubbles.Add(bubble004);
        bubbles.Add(bubble005);

        //hide bubbles
        isHidden = true;
        SetBubbleAlpha(0f);
        hideTimer = false;
    }

    private void SetBubbleAlpha(float alpha)
    {
        //airLabel.SetActive(alpha > 0);
        isHidden = alpha <= 0;
        
        foreach (Image im in bubbles)
        {
            im.color = new Color(100f, 100f, 100f, alpha);
        }
    }

    private void HideUI()
    {
        SetBubbleAlpha(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.localPlayer)
        {
            if (isUnderwater) // if we are underwater...
            {
                CancelInvoke("HideUI");
                if (isHidden) // and bubbles are currently hidden, show bubbles
                {
                    SetBubbleAlpha(100f);
                }
            }
            else // if we aren't underwater...
            {
                if (!isHidden && numBubbles >= 5 && !hideTimer) // and bubbles are currently visible, hide bubbles if we are fully stocked on air.
                {
                    Invoke("HideUI", 2.0f);
                }
            }

            // Take off health when player has no air
            if (numBubbles == 0 && bubbleCheckCompleted)
            {
                bubbleCheckCompleted = false;
                Invoke("BubbleCheck", 1);
            }

            decimal dec = new decimal(transform.position.y);
            double playerLevel = Math.Round((double)dec, 2);

            // Player becomes submerged or emerges from water
            if ((playerLevel <= waterLevel) != isUnderwater)
            {
                isUnderwater = playerLevel < waterLevel;
                if (isUnderwater)
                {
                    //SetUnderwater();
                    loseBubbleCompleted = false;
                    Invoke("LoseBubble", 1);
                }

                if (!isUnderwater)
                {
                    gainBubbleCompleted = false;
                    Invoke("GainBubble", 1);
                }
            }

            // Player remains underwater
            else if (playerLevel < waterLevel)
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
            else if (playerLevel >= waterLevel)
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
    }

    // Player loses a bubble every second underwater
    void LoseBubble()
    {
        if (isUnderwater)
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
    }

    // Player gains a bubble every second above water
    void GainBubble()
    {
        if (!isUnderwater)
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
    }

    // Player loses 20 health every second they have no air
    void BubbleCheck()
    {
        player.health -= 20;
        bubbleCheckCompleted = true;
    }
}
