using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletCtrl : MonoBehaviourPunCallbacks
{
    //����S���@�c��
    //�v���C���[�̒e�̐���N���X
    [SerializeField]
    GameObject HitEffect;
    float time=0;
    // Start is called before the first frame update
    void Awake()
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
            //3�b�i��ŏ���
            if (!photonView.IsMine) { return; }
            PhotonNetwork.Destroy(gameObject);
            Instantiate(HitEffect, transform.position, Quaternion.identity);
        }
    }
	private void OnCollisionEnter(Collision collision)
    {
        Instantiate(HitEffect,transform.position,Quaternion.identity);
        if (!photonView.IsMine) { return; }
        PhotonNetwork.Destroy(gameObject);
    }
}
