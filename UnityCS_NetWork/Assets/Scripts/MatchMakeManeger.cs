using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class MatchMakeManeger : MonoBehaviourPunCallbacks
{
	//制作担当　田上
	//マッチングに関するクラス
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
		//サーバー接続に成功したら
		//部屋のカスタムプロパティとしてプレイヤー1とプレイヤー２のスコアと開始時間取得用のプロパティを作成
		var roomProps = new ExitGames.Client.Photon.Hashtable();
		roomProps["Player1Score"] = 0;
		roomProps["Player2Score"] = 0;
		roomProps["StartTime"] = 0;
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = 2;
		roomOptions.CustomRoomProperties=roomProps;
		Debug.Log("クライアントの接続に成功");
		//ソロモードフラグが立っているか否かで部屋名を分岐
		if (!isSoloMode)
		{
			PhotonNetwork.JoinOrCreateRoom("Room1", roomOptions, TypedLobby.Default);
		}
		else
		{
			PhotonNetwork.CreateRoom("Solo",roomOptions,TypedLobby.Default);
		}
	}
	public override void OnJoinedRoom(){
		MatchMakeText.text = "部屋に入室しました";
		Debug.Log("部屋への参加に成功");
		if (isSoloMode)
		{
			Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"]);
			PhotonNetwork.LoadLevel("MainGame");
			//ソロモードの場合は部屋に参加したら即ゲームへ遷移
		}
	}
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		//2人目がそろったらホストが時間を設定しメインゲームのSceneへ移動
		if (PhotonNetwork.LocalPlayer.IsMasterClient)
		{
			MatchMakeText.text = ("マッチングが完了しました！！");
			SetTimeProps();
			PhotonNetwork.LoadLevel("MainGame");
		}
	}
	public void StartConect()
	{
		//マッチング開始
		MatchMakeText.text = "マッチングしています";
		Debug.Log("接続を開始");
		PhotonNetwork.ConnectUsingSettings();
	}
	// Update is called once per frame
	public void SoloMode()
	{
		//一人でやる場合の設定
		//Photonに接続しないと一部オブジェクトが動作しないため1人用でも部屋につなぎます
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
		}
	}
}
