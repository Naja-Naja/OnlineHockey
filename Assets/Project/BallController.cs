using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    // Start is called before the first frame update
    void Start()
    {
        rb2d.AddForce(new Vector2(15f, 15f));

    }
}
