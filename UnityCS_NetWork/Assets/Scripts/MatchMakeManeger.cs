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
		MatchMakeText.text = PhotonNetwork.CurrentLobby.Name+"参加に成功しました";
		Debug.Log("ロビーの参加に成功");
	}
	public override void OnConnectedToMaster()
	{
		var roomProps = new ExitGames.Client.Photon.Hashtable();
		roomProps["Player1Score"] = 0;
		roomProps["Player2Score"] = 0;
		roomProps["StartTime"] = 0;
		//部屋のカスタムプロパティとしてプレイヤー1とプレイヤー２のスコアを設定
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 0;
		roomOptions.CustomRoomProperties=roomProps;
		Debug.Log("クライアントの接続に成功");
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
		MatchMakeText.text = PhotonNetwork.CurrentRoom.Name+"に入室しました";
		Debug.Log("部屋への参加に成功");
		if (isSoloMode)
		{
			SetTimeProps();
			PhotonNetwork.LoadLevel("MainGame");
			//ソロモードの場合は部屋に参加したら即ゲームへ遷移
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			MatchMakeText.text = ("マッチングが完了しました！！");
			SetTimeProps();
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
	void SetTimeProps() 
	{
		//部屋ホストの入室時間を基準に同期用のゲーム開始時刻を設定
		//こうすることによりゲーム終了時間の誤差がなくなる
		if (PhotonNetwork.IsMasterClient)
		{
			var timeProps = new ExitGames.Client.Photon.Hashtable();
			timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
			PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
			Debug.Log("StartTime=" + timeProps["StartTime"].ToString());
		}
	}
}
