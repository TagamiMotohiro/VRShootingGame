using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletCtrl : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject HitEffect;
    [SerializeField]
    float dulation;//弾の生存時間
    float time=0;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            //放たれた弾が自身のものでなければレイヤーを敵弾用のものに変更
            this.gameObject.layer = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= dulation)
        {
            //自身の放った球は一定秒数でヒットしなくても消滅
            if (!photonView.IsMine) { return; }
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
	private void OnDestroy()
	{
		Instantiate(HitEffect,transform.position,Quaternion.identity);
        //消滅時エフェクト
	}
	private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        //ダメージ判定は的側なので弾側の処理は消すだけ
    }
}
