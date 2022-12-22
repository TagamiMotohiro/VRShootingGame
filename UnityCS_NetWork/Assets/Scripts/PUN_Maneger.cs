using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class PUN_Maneger : MonoBehaviourPunCallbacks
{
	GameObject human;
	GameObject OVRcamera;
	GameObject RightHand;
	float score = 0;
	[SerializeField]
	TextMeshProUGUI scoreText;
	public override void OnConnectedToMaster()//PUNに接続された際の処理（コールバック）
	{
		// 第一引数で指定した名前のルームに入室、なかった場合は作成して入室。
		PhotonNetwork.JoinOrCreateRoom("Game Room", new RoomOptions(), TypedLobby.Default);	
	}
	public override void OnJoinedRoom()//ロビーからルームに接続された時の処理（コールバック）
	{
		//プレイヤーとなるゲームオブジェクトを作成し、変数内に取得。
		human = PhotonNetwork.Instantiate("VRPlayer", Vector3.zero, Quaternion.identity);
		OVRcamera = GameObject.Find("OVRCameraRig");
		RightHand = GameObject.Find("RighthandPrefub");
		//scriptにプレイヤーの移動スクリプトを取得
		var script = OVRcamera.gameObject.GetComponent<VRPlayerWork>();
		var gun = RightHand.gameObject.GetComponent<PlayerGun>();
		//プレイヤーが動けるようになるフラグをonにする
		script.SetStert(true);
		gun.SetStart(true);
		//script.SetMessage(PhotonNetwork.NetworkClientState.ToString());
		//プレイヤーにカメラをセット
		//Camera.main.GetComponent<HumanCameraScript>().target=human;
		OVRcamera.transform.position = human.transform.position;

	}
	// Start is called before the first frame update
	void Start()
    {
		//PUNサーバーに接続する
		PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
		scoreText.text = score.ToString();
	}
	public void PlusScore(int plusScore)
	{
		score += plusScore;
	}
}
