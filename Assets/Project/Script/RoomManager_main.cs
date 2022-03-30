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
    public ReactiveProperty<string> text;
    public ReactiveProperty<string> result;
    bool avatarCreated = false;
    string myTeam;
    private void Awake()
    {
        //roomAPI = new RoomAPI();
        //�G���g���[�|�C���gentrypoint
        //�T�[�o�[�ɐڑ�
        roomAPI.ConnectServer();
    }
    //�T�[�o�[�ڑ��̃R�[���o�b�N�Ń��[���ɐڑ�
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
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

    }
    public void joinRightTeam()
    {
        canjoinTeam.Value = false;
        Debug.Log("joinright");
        myTeam = "Right";
        roomAPI.JoinTeam(myTeam);
        text.Value = "Ready�c";
    }
    //�`�[���o�^�󋵃A�b�v�f�[�g�̃R�[���o�b�N�ŃQ�[���Q���\�Ȃ�J�n�{�^���𐶐�����
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
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
        RoomPropertiesUpDate_CreateAvatar();
        RoomPropertiesUpDate_Checkmywin();
    }
    //���[���v���p�e�B��Game�ɕύX���ꂽ�Ƃ���Avatar�𐶐�����
    private void RoomPropertiesUpDate_CreateAvatar()
    {
        string room = (string)PhotonNetwork.CurrentRoom.CustomProperties["RoomState"];
        if (room == "Game" && avatarCreated == false)
        {
            roomAPI.CreateAvatar();
            text.Value = "";
            canStart.Value = false;
            avatarCreated = true;
        }
    }
    //DeadLine����s�҂����̊֐����Ă�
    public void GameJudge(string loser)
    {
        if (loser == myTeam)
        {
            Debug.Log("����");
            result.Value = "Lose";
            roomAPI.LoseEvent(loser);
            GameInit();
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
            photonView.RPC(nameof(roomAPI.BallDestroy), RpcTarget.All);
            //�Đ�{�^���̕\��
            canRetry.Value = true;
        }
    }
    //�Q�[���ĊJ����
    public void Retry()
    {
        //UI�̏���
        canRetry.Value = false;
        photonView.RPC(nameof(RpcResultUIInit), RpcTarget.All);
        roomAPI.CreateBall();
    }
    [PunRPC]
    void RpcResultUIInit()
    {
        Debug.Log("���s�\��������");
        result.Value = "";
    }
}
