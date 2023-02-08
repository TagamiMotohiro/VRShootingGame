using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletCtrl : MonoBehaviourPunCallbacks
{
    Vector3 target;
    [SerializeField]
    GameObject HitEffect;
    float time=0;
    // Start is called before the first frame update
    void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
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
            //3�b�i��ŏ���
            if (!photonView.IsMine) { return; }
            PhotonNetwork.Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
        }
        //�z�[�~���O�@�\�����悤�Ǝv������Aim�̕K�v����������Q�[������S�ۂł��Ȃ��\�������邽�ߕۗ�
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
        if (target == new Vector3())
        { this.transform.position += transform.forward; }
        else
        {
            Vector3 velocity = (target-transform.position).normalized;
            this.transform.position += (velocity*30)*Time.deltaTime;
        }
    }
    public void SetTarget(Vector3 pos)
    {
        if (target == null) { return; }
        target = pos;
    }
}
