using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AvatarManager : MonoBehaviourPunCallbacks
{
    bool gameStart = false;
    bool IsMaster = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            IsMaster = true;
        }
        else
        {
            IsMaster = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            var players = PhotonNetwork.PlayerList;
            if (players.Length > 1 && gameStart == false && IsMaster == true)
            {
                //ƒQ[ƒ€ŠJnˆ—
                var position = new Vector3(-3f, 0f);
                PhotonNetwork.Instantiate("ball", position, Quaternion.identity);
                gameStart = true;
            }
            else if (players.Length == 1)
            {
                gameStart = false;
            }
        }
    }
}
