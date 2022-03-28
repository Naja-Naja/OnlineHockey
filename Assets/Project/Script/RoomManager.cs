using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UniRx;

    //Avatar生成までの進行を主に管理
public class RoomManager : MonoBehaviourPunCallbacks
{
    //各種入力の受付
    public ReactiveProperty<bool> canStart;
    public ReactiveProperty<bool> canjoinTeam;
    public ReactiveProperty<bool> activetitle;
    private void Awake()
    {
        // プレイヤー自身の名前を"Player"に設定する
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 30;
    }
    private void Start()
    {
        activetitle.Value = true;
        canjoinTeam.Value = true;
    }
    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        //まず自身のプロパティを"wait"に設定し、自分のプレイヤーナンバーを設定し、チーム選択ボタンを表示する
        int playernum = PhotonNetwork.LocalPlayer.ActorNumber;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["myTeam"] = "wait";
        hashtable["myNumber"] = playernum;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (PhotonNetwork.IsMasterClient)
        {
            //自身が部屋主であれば部屋のプロパティを"Ready"に設定する
            hashtable["RoomState"] = "Ready";
        }

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    //自身のアバター（ネットワークオブジェクト）を生成する
        //    var position = new Vector3(-6f, 0f);
        //    PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        //}
        //else
        //{
        //    //自身のアバター（ネットワークオブジェクト）を生成する
        //    var position = new Vector3(6f, 0f);
        //    PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        //}
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("誰かがルームに入室しました");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("誰かがルームから退出しました");
    }
    public void joinLeftTeam()
    {
        //string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        string myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["myTeam"];
        Debug.Log(myTeam);
        if (myTeam == "wait")
        {
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["myTeam"] = "Left";
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            //createPlayer();
        }
        else if (myTeam== "Right" || myTeam == "Left")
        {
            //ここにそのうちチーム切り替え処理を書く
            return;
        }
        else
        {
            //Debug.LogError("チーム所属状況が異常です"+myTeam);
        }
    }
    public void joinRightTeam()
    {
        string myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["myTeam"];
        if (myTeam == "wait")
        {
            var hashtable = new ExitGames.Client.Photon.Hashtable();
            hashtable["myTeam"] = "Right";
            PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
            //createPlayer();
        }
        else if (myTeam == "Right" || myTeam == "Left")
        {
            //ここにそのうちチーム切り替え処理を書く
            return;
        }
        else
        {
            //Debug.LogError("チーム所属状況が異常です(1)");
        }
    }
    //プレイヤー生成。以降の処理はManager.csが中心
    //private void createPlayer()
    //{
    //    string myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["myTeam"];
    //    if (myTeam == "Left")
    //    {
    //        //自身のアバター（ネットワークオブジェクト）を生成する
    //        var position = new Vector3(-6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else if (myTeam == "Right")
    //    {
    //        //自身のアバター（ネットワークオブジェクト）を生成する
    //        var position = new Vector3(6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Debug.LogError("チーム所属状況が異常です(2)");
    //    }
    //}
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        if (myTeam == "Left")
        {
            //自身のアバター（ネットワークオブジェクト）を生成する
            var position = new Vector3(-6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
        else if (myTeam == "Right")
        {
            //自身のアバター（ネットワークオブジェクト）を生成する
            var position = new Vector3(6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
        else
        {
            return;
            //Debug.LogError("チーム所属状況が異常です(2)");
        }
        activetitle.Value = false;
        canjoinTeam.Value = false;
    }
}
