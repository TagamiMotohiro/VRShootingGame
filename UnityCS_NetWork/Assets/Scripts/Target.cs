using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Target : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject Defended_Effect;
    [SerializeField]
    GameObject DestroyEffect;
    [SerializeField]
    protected int HP;
    [SerializeField]
    int hit_Score=100;
    [SerializeField]
    int deferted_Score=1000;
    [SerializeField]
    protected int anim_Speed=1;
    [SerializeField]
    AudioClip DestroySE;
    AudioSource myAS;
    protected MainGamePUNmaneger maneger;
    bool isTargeted;
    bool destroyed = false;
    Behaviour halo;
    // Start is called before the first frame update
    protected void Start()
    {
        myAS = this.gameObject.GetComponent<AudioSource>();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        maneger = GameObject.Find("PUN2Script").GetComponent<MainGamePUNmaneger>();
    }
    // Update is called once per frame
    // public override void OnDisable()
    //{
        
    //}
    void LateUpdate()
    {
        TargetAnimation();
        if (this.HP <= 0)//HPが0になったら
        {
            gameObject.SetActive(false);
            Debug.Log("消滅した");
            //非アクティブ化
			//Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            //爆発エフェクトを発動
           　if (PhotonNetwork.LocalPlayer.IsMasterClient)
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
        this.transform.Rotate(0, HP*Time.deltaTime, 0);
    }
    public void Targeting()
    {
        isTargeted = true;
    }
	public void OnCollisionEnter(Collision collision)
	{   
        PhotonView Collision_photonView = collision.gameObject.GetComponent<PhotonView>();
        //当たったオブジェクトのPhotonViewを取得
        if (Collision_photonView == null) { return; }//PhtonViewを持たないオブジェクトに当たった場合何もしない
        this.HP--;//自身の耐久値を減らす
        if (Collision_photonView.IsMine) {
			//         Debug.Log(collision.gameObject.name);
			if (collision.gameObject.tag == "Player")
			{
				this.HP = 0;
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                maneger.PlusScore(-deferted_Score);
				//Instantiate(DestroyEffect,transform.position,Quaternion.identity);
				return;
			}
			else
			if (collision.gameObject.tag == "Shield")
			{
				this.HP = 0;
                Instantiate(Defended_Effect, transform.position, Quaternion.identity);
				maneger.PlusScore(200);
				return;
			}
			if (this.HP <= 0&&!destroyed)
            {
                //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                maneger.PlusScore(deferted_Score);
                myAS.PlayOneShot(DestroySE);
                destroyed = true;
                return;
            }
            maneger.PlusScore(hit_Score);
        }
	}
}
