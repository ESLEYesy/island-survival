using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tide : MonoBehaviour
{

    public GameObject sun;

    private float average;
    public float tideOffset;
    public float waveOffset;

    // Start is called before the first frame update
    void Start()
    {
        average = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        tideOffset = Mathf.Sin(3.15f * sun.GetComponent<Sky>().dot);
        waveOffset = 0.75f * Mathf.Sin(0.5f * Time.time);

        this.transform.position = new Vector3(0, average + tideOffset + waveOffset, 0);
    }
}
