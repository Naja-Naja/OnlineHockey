using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
public class RoomManager_v2 : MonoBehaviourPunCallbacks
{
    [SerializeField]RoomManager_main roomManager_Main;
    private void Awake()
    {
        //roomManager_Main = new RoomManager_main(this);
    }
    public void ConnectServer()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 30;
    }
    public void roomPropatiesInit()
    {
        int playernum = PhotonNetwork.LocalPlayer.ActorNumber;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["myTeam"] = "Choice";
        hashtable["myNumber"] = playernum;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (PhotonNetwork.IsMasterClient)
        {
            //自身が部屋主であれば部屋のプロパティを"Matching"に設定する
            hashtable["RoomState"] = "Matching";
        }
    }




    //private void Start()
    //{
    //    // プレイヤー自身の名前を"Player"に設定する
    //    PhotonNetwork.NickName = "Player";
    //    // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
    //    PhotonNetwork.ConnectUsingSettings();
    //    PhotonNetwork.SendRate = 30;
    //}

    //// マスターサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnConnectedToMaster()
    //{
    //    // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    //}

    //// ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    //public override void OnJoinedRoom()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        //自身のアバター（ネットワークオブジェクト）を生成する
    //        var position = new Vector3(-6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        //自身のアバター（ネットワークオブジェクト）を生成する
    //        var position = new Vector3(6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }


    //}
}
