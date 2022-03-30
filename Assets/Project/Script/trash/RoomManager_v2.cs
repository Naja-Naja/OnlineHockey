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
        // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
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
            //���g��������ł���Ε����̃v���p�e�B��"Matching"�ɐݒ肷��
            hashtable["RoomState"] = "Matching";
        }
    }




    //private void Start()
    //{
    //    // �v���C���[���g�̖��O��"Player"�ɐݒ肷��
    //    PhotonNetwork.NickName = "Player";
    //    // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
    //    PhotonNetwork.ConnectUsingSettings();
    //    PhotonNetwork.SendRate = 30;
    //}

    //// �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //public override void OnConnectedToMaster()
    //{
    //    // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
    //    PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions(), TypedLobby.Default);
    //}

    //// �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    //public override void OnJoinedRoom()
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
    //        var position = new Vector3(-6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }
    //    else
    //    {
    //        //���g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
    //        var position = new Vector3(6f, 0f);
    //        PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    //    }


    //}
}
