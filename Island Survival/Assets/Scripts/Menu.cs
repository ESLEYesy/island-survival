using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    // Player name field
    public InputField playerNameField;
    public string playerName;

    // Start is called before the first frame update
    void Start()
    {
        playerNameField.text = "Player";
        playerNameField.onValueChange.AddListener(delegate {ValueChangeCheck(); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
    	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
    	Application.Quit();
    }

    public void ValueChangeCheck()
    {
        StaticData.PlayerName = playerNameField.text;
    }
}
