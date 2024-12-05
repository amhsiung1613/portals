using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    public float speed;

    private Rigidbody2D rb;
    private Transform currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = pointA.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointA.transform) {
            rb.velocity = new Vector2(-speed, 0);
        } else {
            rb.velocity = new Vector2(speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 2.5f && currentPoint == pointB.transform) {
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 2.5f && currentPoint == pointA.transform) {
            currentPoint = pointB.transform;
        }
    }

    // private void onDrawGizmos() {
    //     Gizmos.color = Color.black;
    //     Gizmos.DrawWireSphere(pointA.transform.position, 5f);
    //     Gizmos.DrawWireSphere(pointB.transform.position, 5f);
    // }
}
