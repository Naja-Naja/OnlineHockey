using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallController : MonoBehaviourPunCallbacks
{
    [SerializeField] Rigidbody2D rb2d;

    //効果音
    [SerializeField] AudioClip wallBound;
    [SerializeField] AudioClip playerBound;
    [SerializeField] AudioClip playerSmashBound;

    //bool movestart = false;
    //bool IsGameEnd = false;
    bool speedCon = false;
    bool smash = false;
    void Start()
    {
        Invoke("ballStart", 3f);
    }
    void ballStart()
    {
        rb2d.AddForce(new Vector2(20f, 15f));
        //rb2d.AddForce(new Vector2(-30f, -15f));//test
        //movestart = true;
        Invoke("startSpeedCon", 0.5f);
    }
    void startSpeedCon()
    {
        speedCon = true;
    }
    private void FixedUpdate()
    {
        //序盤0.5秒以降で速度が一定以下になったら加速する（詰み回避）
        if (speedCon == false) { return; }
        var speed = rb2d.velocity;
        if (Mathf.Abs(speed.x) < 0.1/* && movestart == true && IsGameEnd == false*/)
        {
            rb2d.AddForce(new Vector2(Mathf.Sign(speed.x) * 15f, 0f));
            //Debug.Log(Mathf.Sign(speed.x) * 15);
        }
        if (Mathf.Abs(speed.y) < 0.1/* && movestart == true && IsGameEnd == false*/)
        {
            rb2d.AddForce(new Vector2(0f, Mathf.Sign(speed.y) * 5f));
            //Debug.Log(Mathf.Sign(speed.x) * 15);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log(Vector2.Distance(this.transform.position, collision.transform.position));
            if (collision.gameObject.GetPhotonView().IsMine == false) 
            { 
                Debug.Log("やらない");
                //rb2d.velocity = Vector2.zero;
                //speedCon = false;
                //Invoke("startSpeedCon", 0.2f);
            }
            //スマッシュ
            else if (1f<Vector2.Distance(this.transform.position,collision.transform.position))
            {
                
                photonView.RPC(nameof(RpcBallSmash), RpcTarget.AllViaServer,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    rb2d.velocity
                    );
            }
            //通常反射
            else
            {
                photonView.RPC(nameof(RpcBallBound), RpcTarget.AllViaServer,
                    new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z),
                    rb2d.velocity
                    );
            }
        }
        else if (collision.gameObject.tag == "wall")
        {
            AudioManager.SE_Play(wallBound);
        }
        rb2d.velocity = rb2d.velocity * 1.02f;
        
    }
    [PunRPC]
    private void RpcBallBound(Vector3 position,Vector2 vector)
    {
        
        this.transform.position = position;
        if (smash)
        {
            rb2d.velocity = new Vector2(vector.x - Mathf.Sign(vector.x) * 3f, vector.y - Mathf.Sign(vector.y) * 3f);
        }
        else { rb2d.velocity = vector * 1.02f; }
        AudioManager.SE_Play(playerBound);
        smash = false;
    }
    [PunRPC]
    private void RpcBallSmash(Vector3 position, Vector2 vector)
    {
        smash = true;
        this.transform.position = position;
        rb2d.velocity = new Vector2(vector.x + Mathf.Sign(vector.x) * 3f, vector.y + Mathf.Sign(vector.y) * 3f) * 1.2f;
        AudioManager.SE_Play(playerSmashBound);
    }
}
