using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Target : MonoBehaviourPunCallbacks
{
    [SerializeField]
    GameObject DestroyEffect;
    [SerializeField]
    int HP;
    [SerializeField]
    int hit_Score=100;
    [SerializeField]
    int deferted_Score=1000;
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
    private void OnDisable()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    void LateUpdate()
    {
        if (this.HP <= 0)
        {
            this.gameObject.SetActive(false);
			Instantiate(DestroyEffect, transform.position, Quaternion.identity);
		}
        if (isTargeted)
        {
            if (halo == null) { return; }
            halo.enabled = true;
        }
        else
        {
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
        if (Collision_photonView.IsMine) {//
            if (collision.gameObject.tag == "Player")
            {
                this.HP = 0;
                maneger.PlusScore(-deferted_Score);
				Instantiate(DestroyEffect, transform.position, Quaternion.identity);
				return;
            }
            maneger.PlusScore(hit_Score);
            if (this.HP <= 0)
            {
                maneger.PlusScore(deferted_Score);
                return;
            }
        }
        
	}
}
