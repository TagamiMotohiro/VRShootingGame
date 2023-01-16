using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Target : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject DestroyEffect;
    [SerializeField]
    protected int HP;
    [SerializeField]
    int hit_Score=100;
    [SerializeField]
    protected int deferted_Score=1000;
    protected MainGamePUNmaneger maneger;
    bool isTargeted;
    bool sceneUnload = false;
    Behaviour halo;
    // Start is called before the first frame update
    protected void Start()
    {
        halo = (Behaviour)gameObject.GetComponent("Halo");
        maneger = GameObject.Find("PUN2Script").GetComponent<MainGamePUNmaneger>();
    }
    // Update is called once per frame
     public override void OnDisable()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
           {
            PhotonNetwork.Destroy(this.gameObject);
           }
    }
    void LateUpdate()
    {
        if (this.HP <= 0)//HPが0になったら
        {
            gameObject.SetActive(false);
            Debug.Log("消滅した");
            //非アクティブ化
			Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            //爆発エフェクトを発動
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
    { }
    public void Targeting()
    {
        isTargeted = true;
    }
    private void OnApplicationQuit()
    {
        sceneUnload = true;
    }
	public void OnCollisionEnter(Collision collision)
	{   
        PhotonView Collision_photonView = collision.gameObject.GetComponent<PhotonView>();
        //当たったオブジェクトのPhotonViewを取得
        if (Collision_photonView == null) { return; }//PhtonViewを持たないオブジェクトに当たった場合何もしない
        this.HP--;//自身の耐久値を減らす
        if (Collision_photonView.IsMine) {
            Debug.Log(collision.gameObject.name);
			if (collision.gameObject.tag == "Player")
			{
				this.HP = 0;
				maneger.PlusScore(-deferted_Score);
				//Instantiate(DestroyEffect,transform.position,Quaternion.identity);
				return;
			}
            //else
            //if (collision.gameObject.tag == "Shield")
            //{
            //	this.HP = 0;
            //	maneger.PlusScore(200);
            //	return;
            //}
            if (this.HP <= 0)
            {
                maneger.PlusScore(deferted_Score);
            }
            maneger.PlusScore(hit_Score);
        }     
	}
}
