using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Target : MonoBehaviourPunCallbacks
{
    //制作担当　田上
    //弾を撃つことで壊せるすべての的の基底クラス
    [Header("ステータス関連")]
    [SerializeField]
    protected int HP;
    [SerializeField]
    int hit_Score = 100;
    [SerializeField]
    int deferted_Score = 1000;
    [SerializeField]
    protected int anim_Speed = 1;
    protected ScoreManeger maneger;
    bool isTargeted;
    bool destroyed = false;
    Behaviour halo;
    [Header("素材関連(SE エフェクト)")]
    [SerializeField]
    GameObject Defended_Effect;
    [SerializeField]
    GameObject DestroyEffect;
    [SerializeField]
    AudioClip DestroySE;
    [SerializeField]
    AudioClip Player_Hit_SE;
    [SerializeField]
    AudioClip Hit_SE;
    [SerializeField]
    AudioClip Guard_SE;
    AudioManeger audioManeger;
    // Start is called before the first frame update
    protected void Start()
    {
        //オブジェクト検索よりもタグで検索したほうが速いらしいのでタグで検索
        audioManeger = GameObject.FindWithTag("Audio").GetComponent<AudioManeger>();
        maneger = GameObject.FindWithTag("PUN2Maneger").GetComponent<ScoreManeger>();
        //HaloはBehabiourの文字列取得でしか取得できないのでそれで取得
        halo = (Behaviour)gameObject.GetComponent("Halo");
        
        PhotonNetwork.AddCallbackTarget(this.gameObject);
    }
    void LateUpdate()
    {
        TargetAnimation();
        if (destroyed)//HPが0になったら
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        if (isTargeted)
        {
            //自身が狙われたら発光
            if (halo == null) { return; }
            halo.enabled = true;
        }
        else
        {
            //そうでない場合発光をやめる
            halo.enabled = false;
        }
        isTargeted = false;
    }
    protected virtual void TargetAnimation()
    {
        this.transform.Rotate(0, (HP * Time.deltaTime)*anim_Speed, 0);
    }
    public void Targeting()
    {
        isTargeted = true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        //衝突先が謎だったりターゲット同士の衝突が起きてしまった場合は何もせず消滅
        if (collision.gameObject.tag=="Untagged"||collision.gameObject.tag=="Target")
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        PhotonView Collision_photonView = collision.gameObject.GetComponent<PhotonView>();
        //当たったオブジェクトのPhotonViewを取得
        if (Collision_photonView == null) { return; }//PhtonViewを持たないオブジェクトに当たった場合何もしない
        this.HP--;//自身の耐久値を減らす
        if (Collision_photonView.IsMine && !destroyed) {
            if (collision.gameObject.tag == "Player")
            {
                //プレイヤーに当たったら即座に破壊
                destroyed = true;
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                audioManeger.PlaySE(Player_Hit_SE);
                //破壊時スコアの半分を減点
                maneger.PlusScore(-deferted_Score/2);
                //オブジェクト所有権をリクエスト
                TargetOwnerRequest();
                return;
            }
            else
            if (collision.gameObject.tag == "Shield")
            {
                //盾に当たった際の処理
                //エフェクトや得点以外はプレイヤーにヒットした際の処理と同様
                destroyed = true;
                audioManeger.PlaySE(Guard_SE);
                Instantiate(Defended_Effect, transform.position, Quaternion.identity);
                maneger.PlusScore(deferted_Score / 4);
                TargetOwnerRequest();
                return;
            }
            if (this.HP <= 0)
            {
                //的を破壊したプレイヤーに所有権を委譲
                //破壊したプレイヤーにオブジェクトの破棄を担当してもらう
                GameObject g =Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                g.transform.localScale = transform.localScale * 5;
                TargetOwnerRequest();
                maneger.PlusScore(deferted_Score);
                destroyed = true;
                audioManeger.PlaySE(DestroySE);
                return;
            }
            audioManeger.PlaySE(Hit_SE);
            maneger.PlusScore(hit_Score);
        }
    }
    void TargetOwnerRequest()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //自身が部屋ホストでない場合にオブジェクト所有権をリクエスト(破壊されたときに使用)
            photonView.RequestOwnership();
        }
    }
}
