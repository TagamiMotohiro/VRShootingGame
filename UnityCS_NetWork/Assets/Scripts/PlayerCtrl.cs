using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    //制作担当　田上
    //移動以外のプレイヤー動作関連のクラス
    private GameObject OVRcamera;
    [SerializeField]
    GameObject Panel;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //自分のオブジェクトをカメラ描画対象外のレイヤーにする
            this.gameObject.layer = 6;
            this.transform.GetChild(0).gameObject.layer = 6;
        }
        else
        {
            //相手のオブジェクトに[相手]パネルを表示させる
            Panel.SetActive(true);
        }
        OVRcamera = GameObject.Find("OVRCameraRig");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            this.transform.position = OVRcamera.transform.position;
        }
        else
        {
            //
            Panel.transform.LookAt(OVRcamera.transform.position);
        }
	}
}
