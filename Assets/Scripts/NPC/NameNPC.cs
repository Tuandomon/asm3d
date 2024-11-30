using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameNPC : MonoBehaviour
{
    private Transform trans;
    private Vector3 offset = new Vector3(0, 180, 0);
    // Start is called before the first frame update
    void Start()
    {
        trans = GameObject.Find("FreeLook Camera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(trans);
        transform.Rotate(offset);
    }
}
