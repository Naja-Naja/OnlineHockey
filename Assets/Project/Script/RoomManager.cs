using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UniRx;

    //Avatar�����܂ł̐i�s����ɊǗ�
public class RoomManager : MonoBehaviourPunCallbacks
{
    //�e����͂̎�t
    public ReactiveProperty<bool> canStart;
    public ReactiveProperty<bool> canjoinTeam;
    public ReactiveProperty<bool> activetitle;
    private void Awake()
    {
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 30;
    }
    private void Start()
    {
        activetitle.Value = true;
        canjoinTeam.Value = true;
    }
    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        //�܂����g�̃v���p�e�B��"wait"�ɐݒ肵�A�����̃v���C���[�i���o�[��ݒ肵�A�`�[���I���{�^����\������
        int playernum = PhotonNetwork.LocalPlayer.ActorNumber;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["myTeam"] = "wait";
        hashtable["myNumber"] = playernum;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        if (PhotonNetwork.IsMasterClient)
        {
            //���g��������ł���Ε����̃v���p�e�B��"Ready"�ɐݒ肷��
            hashtable["RoomState"] = "Ready";
        }

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        //    var position = new Vector3(-6f, 0f);
        //    PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        //}
        //else
        //{
        //    //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        //    var position = new Vector3(6f, 0f);
        //    PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        //}
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("�N�������[���ɓ������܂���");
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("�N�������[������ޏo���܂���");
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
            //�����ɂ��̂����`�[���؂�ւ�����������
            return;
        }
        else
        {
            //Debug.LogError("�`�[�������󋵂��ُ�ł�"+myTeam);
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
            //�����ɂ��̂����`�[���؂�ւ�����������
            return;
        }
        else
        {
            //Debug.LogError("�`�[�������󋵂��ُ�ł�(1)");
        }
    }
    //�v���C���[�����B�ȍ~�̏�����Manager.cs�����S
    //private void createPlayer()
    //{
    //    string myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["myTeam"];
    //    if (myTeam == "Left")
    //    {
    //        //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
    //        var position = new Vector3(-6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else if (myTeam == "Right")
    //    {
    //        //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
    //        var position = new Vector3(6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        Debug.LogError("�`�[�������󋵂��ُ�ł�(2)");
    //    }
    //}
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        string myTeam = (PhotonNetwork.LocalPlayer.CustomProperties["myTeam"] is string value) ? value : "null";
        if (myTeam == "Left")
        {
            //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            var position = new Vector3(-6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
        else if (myTeam == "Right")
        {
            //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
            var position = new Vector3(6f, 0f);
            PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
        }
        else
        {
            return;
            //Debug.LogError("�`�[�������󋵂��ُ�ł�(2)");
        }
        activetitle.Value = false;
        canjoinTeam.Value = false;
    }
}
