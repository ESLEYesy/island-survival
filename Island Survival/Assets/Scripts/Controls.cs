using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    // Text for health, air, and win/lose messages
    // public TextMeshProUGUI healthText;
    // public TextMeshProUGUI airText;
    // public TextMeshProUGUI centerText;
    //
    // // Canvas for dimming the screen
    // public GameObject screen;
    // public Image pauseMenu;
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //
    // }
    //
    // void Update()
    // {
    // 	// Restarts the game
    // 	if (Input.GetKeyDown("r") )
    // 	{
    //       crabs.SetActive(false);
    // 	    centerText.text = "Restarting...";
    //       screen.transform.GetChild(0).gameObject.SetActive(true);
    //       Invoke("SceneChangeRestart", 3);
    // 	}
    //
    // 	// Quits the game
    // 	if (Input.GetKeyDown("q"))
    // 	{
    // 		crabs.SetActive(false);
    // 		centerText.text = "Quitting...";
    // 		screen.transform.GetChild(0).gameObject.SetActive(true);
    // 		Invoke("SceneChange", 3);
    // 	}
    //
    // 	// Disables/re-enables the crabs
    // 	if (Input.GetKeyDown("c"))
    // 	{
    // 		if (centerText.text == "Crabs disabled")
    // 		{
    // 			crabs.SetActive(true);
    // 			centerText.text = "";
    // 		}
    //
    // 		else
    // 		{
    // 			crabs.SetActive(false);
    // 			centerText.text = "Crabs disabled";
    // 		}
    // 	}
    //
    // 	// Provides a bridge/stairs up to the chest
    // 	if (Input.GetKeyDown("b"))
    // 	{
    // 		if (!bridgeVisible)
    // 		{
    // 			centerText.text = "TA access granted. Enjoy your gold";
    // 			bridge.SetActive(true);
    // 			bridgeVisible = true;
    // 		}
    //
    // 		else
    // 		{
    // 			centerText.text = "";
    // 			bridge.SetActive(false);
    // 			bridgeVisible = false;
    // 		}
    // 	}
    //
    // 	// Pause and show controls
    // 	if (Input.GetKeyDown("p"))
    // 	{
    // 		StartCoroutine(PauseCoroutine());
    // 	}
    // }
    //
    // IEnumerator PauseCoroutine() {
    //
    // 	if (Time.timeScale == 0)
    // 	{
    // 		centerText.text = "";
    // 		screen.transform.GetChild(0).gameObject.SetActive(false);
    // 		screen.transform.GetChild(1).gameObject.SetActive(false);
    // 		screen.transform.GetChild(2).gameObject.SetActive(false);
    // 		Time.timeScale = 1;
    // 	}
    //
    // 	else
    // 	{
    // 		centerText.text = "Paused";
    // 		screen.transform.GetChild(0).gameObject.SetActive(true);
    // 		screen.transform.GetChild(1).gameObject.SetActive(true);
    // 		screen.transform.GetChild(2).gameObject.SetActive(true);
    // 		Time.timeScale = 0;
    // 	}
    //
    // 	return null;
    // }
    //
    // void SceneChange()
    // {
    // 	// Remove dim screen
    // 	screen.transform.GetChild(0).gameObject.SetActive(false);
		//   SceneManager.LoadScene(0);
    // }
    //
    // void SceneChangeRestart()
    // {
    // 	// Remove dim screen
    // 	screen.transform.GetChild(0).gameObject.SetActive(false);
		//   SceneManager.LoadScene(1);
    // }
    //
    // // Updating score and win text
    // public void WinGame()
    // {
    // 	// Ensure you don't lose after winning
    // 	crabs.SetActive(false);
    //
    // 	// Display win text
    //    	centerText.text = "YOU WON";
    //
    //   	// Dim screen
    //   	screen.transform.GetChild(0).gameObject.SetActive(true);
    //
    //   	// Return to menu after a short delay
    //    	Invoke("SceneChange", 3);
    // }
    //
    // public void LoseGame()
    // {
    // 	crabs.SetActive(false);
    //
    // 	// Display lose text
    // 	centerText.text = "YOU LOST";
    //
    // 	// Dim screen
    // 	screen.transform.GetChild(0).gameObject.SetActive(true);
    //
    // 	// Restart game
    // 	Invoke("SceneChangeRestart", 3);
    // }
}
