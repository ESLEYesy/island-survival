using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Underwater : MonoBehaviour
{
    // Water details
    public float waterLevel;
    public bool isUnderwater;
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
    public int bubblesLeft;
    private bool isHidden;

    public bool drowning;

    // Start is called before the first frame update
    void Start()
    {

        // Initialize color for fog
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.40f, 0.65f, 0.77f, 0.5f);

        // Get reference to player
        player = GameObject.FindObjectOfType<Player>();

        // Initialize bubble variables
        bubblesLeft = 5;
        isUnderwater = false;
        drowning = false;

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
    private void ShowUI()
    {
        SetBubbleAlpha(100f);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.localPlayer)
        {
            double playerLevel = Math.Round((double)(new decimal(transform.position.y)), 2); //round to 2 decimal positions

            if (playerLevel <= waterLevel) // Player is underwater
            {
                if (!isUnderwater) // Player doesn't know they're underwater yet, so start underwater activities
                {
                    isUnderwater = true;

                    CancelInvoke("GainBubble");
                    InvokeRepeating("LoseBubble", 1f, 1f);
                    CancelInvoke("HideUI");

                    if (isHidden) // If bubbles are currently hidden, show bubbles
                    {
                        ShowUI(); // show UI
                    }
                }
                else // Player remains underwater
                {
                    // Start drowning if we are are out of air.
                    if (bubblesLeft == 0 && isUnderwater && !drowning) // Don't start drowning if we're already drowning.
                    {
                        drowning = true;
                        InvokeRepeating("Drowning", 0.1f, 0.1f); //every 10th of a second
                    }
                }
            }
            else // Player has emerged
            {
                if (isUnderwater) //Player doesn't know they've emerged, so start out of water activities
                {
                    drowning = false; //stop drowning
                    isUnderwater = false;

                    CancelInvoke("Drowning"); //stop taking damage\
                    CancelInvoke("LoseBubble");
                    InvokeRepeating("GainBubble", 1f, 1f);
                }
                else // Player remains emerged
                {
                    if (!isHidden && bubblesLeft == 5) // if bubbles are currently visible, hide bubbles if we are fully stocked on air.
                    {
                        Invoke("HideUI", 2.0f);
                    }
                }
            }
        }
    }


    // Player loses a bubble every second underwater
    void LoseBubble()
    {
        if (bubblesLeft >= 1)
        {
            switch (bubblesLeft)
            {
                case 1:
                    bubble005.gameObject.SetActive(false);
                    break;
                case 2:
                    bubble004.gameObject.SetActive(false);
                    break;
                case 3:
                    bubble003.gameObject.SetActive(false);
                    break;
                case 4:
                    bubble002.gameObject.SetActive(false);
                    break;
                case 5:
                    bubble001.gameObject.SetActive(false);
                    break;
            }
            bubblesLeft = bubblesLeft - 1;
        }
        
    }

    // Player gains a bubble every second above water
    void GainBubble()
    {
        if (bubblesLeft <= 5)
        {
            switch (bubblesLeft)
            {
                case 1:
                    bubble005.gameObject.SetActive(true);
                    break;
                case 2:
                    bubble004.gameObject.SetActive(true);
                    break;
                case 3:
                    bubble003.gameObject.SetActive(true);
                    break;
                case 4:
                    bubble002.gameObject.SetActive(true);
                    break;
                case 5:
                    bubble001.gameObject.SetActive(true);
                    break;
            }
            bubblesLeft = bubblesLeft + 1;
        }
    }

    // Player loses 2 health every 1/10th second (20 per second) they have no air
    void Drowning()
    {
        player.loseHealth(2);
    }
}
