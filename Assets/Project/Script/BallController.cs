using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody2D rb2d;
    bool movestart = false;
    bool IsGameEnd = false;
    bool speedCon = false;
    void Start()
    {
        Invoke("ballStart", 3f);

    }

    private void FixedUpdate()
    {
        if (speedCon == false) { return; }
        var speed = rb2d.velocity;
        if (Mathf.Abs(speed.x) < 0.1 && movestart == true && IsGameEnd == false)
        {
            rb2d.AddForce(new Vector2(Mathf.Sign(speed.x) * 15f, 0f));
            //Debug.Log(Mathf.Sign(speed.x) * 15);
        }
        if (Mathf.Abs(speed.y) < 0.1 && movestart == true && IsGameEnd == false)
        {
            rb2d.AddForce(new Vector2(0f, Mathf.Sign(speed.y) * 5f));
            //Debug.Log(Mathf.Sign(speed.x) * 15);
        }
    }

    void ballStart()
    {
        rb2d.AddForce(new Vector2(15f, 15f));
        //rb2d.AddForce(new Vector2(-30f, -15f));//test
        movestart = true;
        Invoke("startSpeedCon", 0.5f);
    }
    void startSpeedCon()
    {
        speedCon = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false) { Debug.Log("‚â‚ç‚È‚¢"); }
            else
            {
                photonView.RPC(nameof(RpcBallBound), RpcTarget.All,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    rb2d.velocity
                    );
            }
        }
        else if (collision.gameObject.tag == "deadline")
        {
            IsGameEnd = true;
        }
        //else if (collision.gameObject.tag == "rpcline")
        //{
        //    photonView.RPC(nameof(RpcBallBound), RpcTarget.All);
        //}
        rb2d.velocity = rb2d.velocity * 1.02f;
        
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "rpcline")
    //    {
    //        Debug.Log("Dead");
    //        photonView.RPC(nameof(RpcBallDead), RpcTarget.All,
    //             new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
    //             rb2d.velocity
    //);
    //    }
    //}

    [PunRPC]
    private void RpcBallBound(Vector3 position,Vector2 vector)
    {
        this.transform.position = position;
        rb2d.velocity = vector*1.02f;
    }
    //[PunRPC]
    //private void RpcBallDead(Vector3 position, Vector2 vector)
    //{
    //    this.transform.position = position;
    //    rb2d.velocity = vector;
    //}

}
