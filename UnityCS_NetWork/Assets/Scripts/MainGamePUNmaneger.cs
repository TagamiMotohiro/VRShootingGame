using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class MainGamePUNmaneger : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float instanceDistance=2;
    float score = 0;
    [SerializeField]
    TextMeshProUGUI scoreText;
    GameObject player;
    GameObject OVRcamera;
    GameObject RightHand;
    // Start is called before the first frame update

    void Start()
    {
		PhotonNetwork.IsMessageQueueRunning = true;
		player = PhotonNetwork.Instantiate("VRPlayer", Vector3.zero, Quaternion.identity);
		OVRcamera = GameObject.Find("OVRCameraRig");
		RightHand = GameObject.Find("RighthandPrefub");
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
        scoreText.text = score.ToString();
    }
    public void PlusScore(int plusScore)
    {
        score += plusScore;
    }
}
