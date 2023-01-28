using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class MatchMakeManeger : MonoBehaviourPunCallbacks
{
	[SerializeField] TextMeshProUGUI MatchMakeText;
	bool isSoloMode = false;
	private void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}
	public override void OnJoinedLobby()
	{
		MatchMakeText.text = PhotonNetwork.CurrentLobby.Name+"�Q���ɐ������܂���";
		Debug.Log("���r�[�̎Q���ɐ���");
	}
	public override void OnConnectedToMaster()
	{
		var roomProps = new ExitGames.Client.Photon.Hashtable();
		roomProps["Player1Score"] = 0;
		roomProps["Player2Score"] = 0;
		roomProps["StartTime"] = 0;
		//�����̃J�X�^���v���p�e�B�Ƃ��ăv���C���[1�ƃv���C���[�Q�̃X�R�A��ݒ�
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 0;
		roomOptions.CustomRoomProperties=roomProps;
		Debug.Log("�N���C�A���g�̐ڑ��ɐ���");
		if (!isSoloMode)
		{
			PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
		}
		else
		{
			PhotonNetwork.CreateRoom(null,roomOptions,TypedLobby.Default);
		}
	}
	public override void OnJoinedRoom(){
		MatchMakeText.text = PhotonNetwork.CurrentRoom.Name+"�ɓ������܂���";
		Debug.Log("�����ւ̎Q���ɐ���");
		if (isSoloMode)
		{
			SetTimeProps();
			PhotonNetwork.LoadLevel("MainGame");
			//�\�����[�h�̏ꍇ�͕����ɎQ�������瑦�Q�[���֑J��
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			MatchMakeText.text = ("�}�b�`���O���������܂����I�I");
			SetTimeProps();
			PhotonNetwork.LoadLevel("MainGame");
		}
	}
	public void StartConect()
	{
		MatchMakeText.text = "�}�b�`���O���Ă��܂�";
		Debug.Log("�ڑ����J�n");
		PhotonNetwork.ConnectUsingSettings();
	}
	// Update is called once per frame
	public void SoloMode()
	{
		PhotonNetwork.ConnectUsingSettings();
		MatchMakeText.text = "��l�ŊJ�n���܂�";
		isSoloMode = true;
	}
	void SetTimeProps() 
	{
		//�����z�X�g�̓������Ԃ���ɓ����p�̃Q�[���J�n������ݒ�
		//�������邱�Ƃɂ��Q�[���I�����Ԃ̌덷���Ȃ��Ȃ�
		if (PhotonNetwork.IsMasterClient)
		{
			var timeProps = new ExitGames.Client.Photon.Hashtable();
			timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
			PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
			Debug.Log("StartTime=" + timeProps["StartTime"].ToString());
		}
	}
}
