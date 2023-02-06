using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Target : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject Defended_Effect;
    [SerializeField]
    GameObject DestroyEffect;
    [SerializeField]
    protected int HP;
    [SerializeField]
    int hit_Score = 100;
    [SerializeField]
    int deferted_Score = 1000;
    [SerializeField]
    protected int anim_Speed = 1;
    [SerializeField]
    AudioClip DestroySE;
    [SerializeField]
    AudioClip Player_Hit_SE;
    [SerializeField]
    AudioClip Hit_SE;
    [SerializeField]
    AudioClip Guard_SE;
    AudioManeger audioManeger;
    protected ScoreManeger maneger;
    bool isTargeted;
    bool destroyed = false;
    Behaviour halo;
    // Start is called before the first frame update
    protected void Start()
    {
        //タグで検索したほうが速いらしいのでタグで検索
        audioManeger = GameObject.FindWithTag("Audio").GetComponent<AudioManeger>();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        maneger = GameObject.FindWithTag("PUN2Maneger").GetComponent<ScoreManeger>();
        PhotonNetwork.AddCallbackTarget(this.gameObject);
    }
    // Update is called once per frame
    // public override void OnDisable()
    //{

    //}
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
            //自身が狙われたら
            if (halo == null) { return; }
            halo.enabled = true;
            //発光
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
        //謎だったりターゲット同士の衝突が起きてしまった場合は何もせず消滅
        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Target")
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
                destroyed = true;
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                audioManeger.PlaySE(Player_Hit_SE);
                maneger.PlusScore(-deferted_Score);
                TargetOwnerRequest();
                return;
            }
            else
            if (collision.gameObject.tag == "Shield")
            {
                destroyed = true;
                audioManeger.PlaySE(Guard_SE);
                Instantiate(Defended_Effect, transform.position, Quaternion.identity);
                maneger.PlusScore(deferted_Score / 4);
                TargetOwnerRequest();
                return;
            }
            if (this.HP <= 0)
            {
                //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
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
            photonView.RequestOwnership();
        }
    }
}
