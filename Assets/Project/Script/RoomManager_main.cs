using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UniRx;

public class RoomManager_main : MonoBehaviourPunCallbacks
{
    
    [SerializeField] RoomAPI roomAPI;
    public ReactiveProperty<bool> canjoinTeam;
    public ReactiveProperty<bool> canStart;
    public ReactiveProperty<bool> canRetry;
    public ReactiveProperty<bool> canLeftGame;
    public ReactiveProperty<bool> canBack;
    public ReactiveProperty<string> text;
    public ReactiveProperty<string> result;
    bool avatarCreated = false;
    string myTeam;
    public string playerName="Player";
    private void Awake()
    {
        //roomAPI = new RoomAPI();
        //�G���g���[�|�C���gentrypoint
        //�T�[�o�[�ɐڑ�
    }
    public void InputPlayerName(string playername)
    {
        if (playername == "")
        {
            playername = playerName;
        }
        roomAPI.ConnectServer(playername);
    }
    //�T�[�o�[�ڑ��̃R�[���o�b�N�Ń��[���ɐڑ�
    public override void OnConnectedToMaster()
    {
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom("Room", roomOptions, TypedLobby.Default);
    }

    //�����̃R�[���o�b�N�Ŏ��g��Choice�ɁA�z�X�g�Ȃ畔����Matching�ɐݒ�
    public override void OnJoinedRoom()
    {
        roomAPI.roomPropatiesInit();
        //UI�̏�����
        canStart.Value = false;
        canRetry.Value = false;
        result.Value = "";
        myTeam = "beforejoin";
        //�Q���`�[���I���{�^����\��
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        Debug.Log("Room State is " + room);

        canjoinTeam.Value = true;

        text.Value = "Choice your Team";
    }
    //�Q���`�[���I���{�^���ɂ���ă`�[���ɎQ������
    public void joinLeftTeam()
    {
        canjoinTeam.Value = false;
        Debug.Log("joinleft");
        myTeam = "Left";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Ready�c";
        canBack.Value = true;
    }
    public void joinRightTeam()
    {
        canjoinTeam.Value = false;
        Debug.Log("joinright");
        myTeam = "Right";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Ready�c";
        canBack.Value = true;
    }
    //�`�[����I�тȂ���
    public void BackChoiceTeam()
    {
        canjoinTeam.Value = true;
        myTeam = "beforejoin";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Choice your Team";
        canBack.Value = false;
    }
    //�`�[���o�^�󋵃A�b�v�f�[�g�̃R�[���o�b�N�ŃQ�[���Q���\�Ȃ�J�n�{�^���𐶐�����
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        if ((string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"] != "Ready") { return; }
        Debug.Log("�J�n�`�F�b�N");
        if (PhotonNetwork.IsMasterClient)
        {
            canStart.Value = roomAPI.CheckCanGameStart();
        }
    }
    //�J�n�{�^���������ꂽ�畔����Game�ɐݒ�
    public void GameStart()
    {

        roomAPI.SetRoomProperties("Game");
        if (PhotonNetwork.IsMasterClient)
        {
            roomAPI.CreateBall();
        }
    }
    //�����̃v���p�e�B�ύX�̃R�[���o�b�N
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        //RoomPropertiesUpDate_ChangeUI();
        //RoomPropertiesUpDate_BGMChanger();
        RoomPropertiesUpDate_ChangeUI();
        RoomPropertiesUpDate_CreateAvatar();
        RoomPropertiesUpDate_Checkmywin();
        //RoomPropertiesUpDate_CheckCanRetry();
    }
    void RoomPropertiesUpDate_ChangeUI()
    {
        //string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        //if (room == "Game")
        //{
        //    canjoinTeam.Value = false;
        //    canStart.Value = false;
        //    canRetry.Value = false;
        //    canLeftGame.Value = false;
        //    canBack.Value = false;
        //    text.Value = "";
        //    result.Value = "";
        //}
        //else if (room == "Result")
        //{

        //}
        //else if (room == "Ready")
        //{

        //}
    }
    //���[���v���p�e�B��Game�ɕύX���ꂽ�Ƃ���Avatar�𐶐�����
    private void RoomPropertiesUpDate_CreateAvatar()
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Game" && avatarCreated == false)
        {
            Debug.Log(room);
            roomAPI.CreateAvatar();
            text.Value = "";
            canStart.Value = false;
            canBack.Value = false;
            avatarCreated = true;
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer + "���ޏo���܂����B�Q�[���I���������܂�");
        GameJudge("None");
    }
    //DeadLine����s�҂����̊֐����Ă�
    public void GameJudge(string loser)
    {
        if (loser == myTeam)
        {
            roomAPI.SetRoomProperties("Result");
            Debug.Log("����");
            result.Value = "Lose";
            roomAPI.LoseEvent(loser);
            GameInit();
            
        }
        else if (loser == "None")
        {
            roomAPI.SetRoomProperties("Result");
            Debug.Log("�ؒf�ɂ�钆�f");
            result.Value = "Disconnect";
            roomAPI.LoseEvent(loser);
            GameInitOnDisConnect();
        }
    }
    //���҂̕\������
    public void RoomPropertiesUpDate_Checkmywin()
    {
        string winner = (string)PhotonNetwork.CurrentRoom.CustomProperties["Winner"];
        if (winner == myTeam)
        {
            result.Value = "Win";
            GameInit();
        }

    }
    //�Q�[���̏���������
    public void GameInit()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //�{�[������
            //photonView.RPC(nameof(roomAPI.BallDestroy), RpcTarget.All);
            roomAPI.BallDestroy();
            //�Đ�{�^���̕\��
            canRetry.Value = true;
            canLeftGame.Value = true;
        }
    }
    void GameInitOnDisConnect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //�{�[������
            //photonView.RPC(nameof(roomAPI.BallDestroy), RpcTarget.All);
            roomAPI.BallDestroy();
            canLeftGame.Value = true;
        }
    }
    //�Q�[���ĊJ����
    public void Retry()
    {
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["Winner"] = "NowFighting";
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        //UI�̏���
        canRetry.Value = false;
        canLeftGame.Value = false;
        photonView.RPC(nameof(RpcResultUIInit), RpcTarget.All);
        roomAPI.CreateBall();
        roomAPI.SetRoomProperties("Game");
    }
    [PunRPC]
    void RpcResultUIInit()
    {
        Debug.Log("���s�\��������");
        result.Value = "";

    }
    public void leftGame()
    {
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["Winner"] = "NowFighting";
        PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
        Debug.Log("�Q�[���𔲂��܂�");
        roomAPI.SetRoomProperties("Ready");
        photonView.RPC(nameof(RpcResultUIInit), RpcTarget.All);
        photonView.RPC(nameof(RpcLeftGame), RpcTarget.AllViaServer);
        canRetry.Value = false;
        canLeftGame.Value = false;
    }
    [PunRPC]
    void RpcLeftGame()
    {

        text.Value = "Choice your Team";
        canjoinTeam.Value = true;
        roomAPI.JoinTeam("beforejoin");
        //roomAPI.AvatarDestroy();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetPhotonView().IsMine)
            {
                PhotonNetwork.Destroy(players[i]);
            }
        }
        avatarCreated = false;


    }
    //void RoomPropertiesUpDate_CheckCanRetry()
    //{
    //    Debug.Log("�J�n�`�F�b�N");
    //    string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
    //    if (PhotonNetwork.IsMasterClient&&room=="Result")
    //    {
    //        canRetry.Value = roomAPI.CheckCanGameStart();
    //    }
    //}
}
