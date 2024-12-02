using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BackgroundMoverCanvas : MonoBehaviour
{
    public float speed = 0.5f; // Tốc độ di chuyển của background
    public Vector2 moveRange = new Vector2(5f, 3f); // Phạm vi di chuyển (x, y)
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, moveRange.x) - moveRange.x / 2f;
        float y = Mathf.PingPong(Time.time * speed, moveRange.y) - moveRange.y / 2f;
        transform.position = new Vector3(startPosition.x + x, startPosition.y + y, transform.position.z);
    }
}


