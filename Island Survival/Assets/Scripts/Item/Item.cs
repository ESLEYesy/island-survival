using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private string name;
    //public GameObject textMeshPrefab;

    //private GameObject label;

    /*public void Awake()
    {
        label = Instantiate(textMeshPrefab, this.transform.position + new Vector3(0f, 0.4f, 0f), this.transform.rotation);
        label.transform.parent = this.transform;
    }

    public void showLabel()
    {
        label.SetActive(true);
    }

    public void hideLabel()
    {
        label.SetActive(false);
    }*/

    public string getName()
    {
        return name;
    }

    public void setName(string name)
    {
        this.name = name;
        //label.GetComponent<TextMesh>().text = this.name;
    }

}
