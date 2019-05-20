using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	Animator animator;

    // Booleans for determining correct animation
    //private bool isWalkingForwardsPressed;
    //private bool isWalkingBackwardsPressed;
    //private bool isJumpingPressed;
    private bool isUnderwater;
    private bool isPunching;
    private bool isSwinging;
    private bool isDancing;
    //private bool rotationRequired;
    //private bool isDancingPressed;

    // Music
    //bool currPlaying;
    //AudioSource music;


    // Start is called before the first frame update
    void Start()
    {
        // Animation
        animator = GetComponent<Animator>();
        //isWalkingForwardsPressed = false;
        //isWalkingBackwardsPressed = false;
        //isJumpingPressed = false;
        isUnderwater = false;
        isPunching = false;
        isSwinging = false;
        //rotationRequired = false;
        //isDancingPressed = false;

        // Audio
        //music = GetComponent<AudioSource>();
        //currPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if walking forwards
        //isWalkingForwardsPressed = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey("w"));

        // Check if walking backwards
        //isWalkingBackwardsPressed = (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"));

        // Check if underwater
        //rotationRequired = !isUnderwater && transform.position.y < 10;
        isUnderwater = transform.position.y < 14.4;

        // Check if jumping
        //isJumpingPressed = Input.GetKey(KeyCode.Space);

        // Check if dancing
        //isDancingPressed = Input.GetKey("v");

        // Walking forwards
        //animator.SetBool("IsWalkingForwards", isWalkingForwardsPressed);

        // Walking backwards
        //animator.SetBool("IsWalkingBackwards", isWalkingBackwardsPressed);

        // Punching
        isPunching = Input.GetMouseButtonDown(1);
        animator.SetBool("IsPunching", isPunching);

        // Swinging
        //animator.SetBool("IsSwinging", isSwinging);

        // Swimming
        animator.SetBool("IsSwimming", isUnderwater);

        // Idle swim (treading water)
        //animator.SetBool("IsTreadingWater", isUnderwater);

        // Dancing
        //animator.SetBool("IsDancing", isDancingPressed);

        // Play music while dancing
        //if (isDancingPressed && !currPlaying)
        //{
        //    music.Play();
        //    currPlaying = true;
        //}

        //if (!isDancingPressed && currPlaying)
        //{
        //    music.Stop();
        //    currPlaying = false;
        //}
    }
}
