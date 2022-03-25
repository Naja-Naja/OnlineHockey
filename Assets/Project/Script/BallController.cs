using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody2D rb2d;
    bool movestart = false;
    bool IsGameEnd = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("ballStart", 3f);

    }
    private void FixedUpdate()
    {
        var speed = rb2d.velocity;
        if (Mathf.Abs(speed.x) < 0.1&&movestart==true&&IsGameEnd==false)
        {
            rb2d.AddForce(new Vector2(Mathf.Sign(speed.x)*15f, 0f));
            //Debug.Log(Mathf.Sign(speed.x) * 15);
        }
    }
    void ballStart()
    {
        //rb2d.AddForce(new Vector2(15f, 15f));
        rb2d.AddForce(new Vector2(-30f, -15f));//test
        movestart = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetPhotonView().IsMine == false) { Debug.Log("‚â‚ç‚È‚¢"); }
            else
            {
                photonView.RPC(nameof(RpcBallBound), RpcTarget.All,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.y),
                    rb2d.velocity
                    );
            }
        }
        else if (collision.gameObject.tag == "deadline")
        {
            IsGameEnd = true;
        }
    }

    [PunRPC]
    private void RpcBallBound(Vector3 position,Vector2 vector)
    {
        this.transform.position = position;
        rb2d.velocity = vector;
    }

}
