using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletCtrl : MonoBehaviourPunCallbacks
{
    GameObject target;
    [SerializeField]
    GameObject HitEffect;
    [SerializeField]
    float time=0;
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            this.gameObject.layer = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 3)
        {
            //3ïbêiÇÒÇ≈è¡ñ≈
            if (!photonView.IsMine) { return; }
            PhotonNetwork.Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
        }
        //
        //Homing();
    }
	private void OnCollisionEnter(Collision collision)
    {
        Instantiate(HitEffect,transform.position,Quaternion.identity);
        if (!photonView.IsMine) { return; }
        PhotonNetwork.Destroy(gameObject);
    }
    void Homing()
    {
        if (!photonView.IsMine) { return; }
        if (target == null)
        { this.transform.position += transform.forward; }
        else
        {
            Vector3 velocity = (transform.position - target.transform.position).normalized;
            this.transform.position += (velocity*30)*Time.deltaTime;
        }
    }
    public void SetTarget(GameObject g)
    {
        target = g;
    }
}
