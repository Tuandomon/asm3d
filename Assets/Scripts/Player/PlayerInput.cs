using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;

    //click left attack
    public bool attackInput;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (!attackInput && Time.timeScale != 0)
        {
            attackInput = Input.GetMouseButtonDown(0); //click chuot trai
        }
    }

    private void OnDisable()
    {
        horizontalInput = 0;
        verticalInput = 0;
        attackInput = false;
    }
}