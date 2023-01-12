using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class MathMakeManeger : MonoBehaviourPunCallbacks
{
	[SerializeField] TextMeshProUGUI MatchMakeText;
	bool isSoloMode = false;
	private void Awake()
	{
		PhotonNetwork.AutomaticallySyncScene = true;
	}
	public override void OnJoinedLobby()
	{
		MatchMakeText.text = "ロビーへの参加に成功しました";
		Debug.Log("ロビーの参加に成功");
	}
	public override void OnConnectedToMaster()
	{
		var roomProps = new ExitGames.Client.Photon.Hashtable();
		roomProps["Player1Score"] = 0;
		roomProps["Player2Score"] = 0;
		//部屋のカスタムプロパティとしてプレイヤー1とプレイヤー２のスコアを設定
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		roomOptions.CustomRoomProperties=roomProps;
		Debug.Log("クライアントの接続に成功");
		PhotonNetwork.JoinOrCreateRoom("Room1",roomOptions,TypedLobby.Default);
	}
	public override void OnJoinedRoom()
	{
		//PhotonNetwork.IsMessageQueueRunning = false;
		//部屋へ参加するがオブジェクト生成は別のシーンで行う
		MatchMakeText.text = "部屋への参加に成功しました";
		Debug.Log("部屋への参加に成功");
		if (isSoloMode)
		{ 
			PhotonNetwork.LoadLevel("MainGame");
			//ソロモードの場合は部屋に参加したら即ゲームへ遷移
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			MatchMakeText.text=("マッチングが完了しました！！");
			Debug.Log("マッチメイキング完了");
			PhotonNetwork.LoadLevel("MainGame");
		}
	}
	public void StartConect()
	{
		MatchMakeText.text = "マッチングしています";
		Debug.Log("接続を開始");
		PhotonNetwork.ConnectUsingSettings();
	}
	// Update is called once per frame
	public void SoloMode()
	{
		PhotonNetwork.ConnectUsingSettings();
		MatchMakeText.text = "一人で開始します";
		isSoloMode = true;
	}
}
