using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Host()
    {
        this.gameObject.SetActive(false);
    }

    public void Client()
    {
        this.gameObject.SetActive(false);
    }

    public void Server()
    {
        this.gameObject.SetActive(false);
    }

    public void Quit()
    {
    	Application.Quit();
    }
}