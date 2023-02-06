using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class MainGamePUNmaneger : MonoBehaviourPunCallbacks
{
    GameObject player;
	[SerializeField]
    GameObject OVRcamera;
	[SerializeField]
    GameObject RightHand;
	// Start is called before the first frame update
	private void Awake()
	{
	}
	void Start()
    {
       
		PhotonNetwork.IsMessageQueueRunning = true;
		player = PhotonNetwork.Instantiate("VRPlayer", Vector3.zero, Quaternion.identity);
		//scriptにプレイヤーの移動スクリプトを取得
		var script = OVRcamera.gameObject.GetComponent<VRPlayerWork>();
		var gun = RightHand.gameObject.GetComponent<PlayerGun>();
		//プレイヤーが動けるようになるフラグをonにする
		script.SetStert(true);
		gun.SetStart(true);
		//プレイヤーにカメラをセット
		OVRcamera.transform.position = player.transform.position;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
