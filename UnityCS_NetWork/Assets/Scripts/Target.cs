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
        //�^�O�Ō��������ق��������炵���̂Ń^�O�Ō���
        audioManeger = GameObject.FindWithTag("Audio").GetComponent<AudioManeger>();
        halo = (Behaviour)gameObject.GetComponent("Halo");
        maneger = GameObject.FindWithTag("PUN2Maneger").GetComponent<ScoreManeger>();
    }
    // Update is called once per frame
    // public override void OnDisable()
    //{
        
    //}
    void LateUpdate()
    {
        TargetAnimation();
        if (this.HP <= 0)//HP��0�ɂȂ�����
        {
            Debug.Log("���ł���");
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
		}
        if (isTargeted)
        {
            //���g���_��ꂽ��
            if (halo == null) { return; }
            halo.enabled = true;
            //����
        }
        else
        {
            //�����łȂ��ꍇ��������߂�
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
        //�䂾�����牽����������
        if (collision.gameObject.tag == "Untagged")
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        PhotonView Collision_photonView = collision.gameObject.GetComponent<PhotonView>();
        //���������I�u�W�F�N�g��PhotonView���擾
        if (Collision_photonView == null) { return; }//PhtonView�������Ȃ��I�u�W�F�N�g�ɓ��������ꍇ�������Ȃ�
        this.HP--;//���g�̑ϋv�l�����炷
        if (Collision_photonView.IsMine&&!destroyed) {
			if (collision.gameObject.tag == "Player")
			{
				this.HP = 0;
                destroyed = true;
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                audioManeger.PlaySE(Player_Hit_SE);
                maneger.PlusScore(-deferted_Score);
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                return;
				//Instantiate(DestroyEffect,transform.position,Quaternion.identity);
			}
			else
			if (collision.gameObject.tag == "Shield")
			{
				this.HP = 0;
                destroyed = true;
                audioManeger.PlaySE(Guard_SE);
                Instantiate(Defended_Effect, transform.position, Quaternion.identity);
				maneger.PlusScore(deferted_Score/4);
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                return;
			}
			if (this.HP <= 0)
            {
                //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                //�I��j�󂵂��v���C���[�ɏ��L�����Ϗ�
                //�j�󂵂��v���C���[�ɃI�u�W�F�N�g�̔j����S�����Ă��炤
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                maneger.PlusScore(deferted_Score);
                destroyed = true;
                audioManeger.PlaySE(DestroySE);
                return;
            }
            audioManeger.PlaySE(Hit_SE);
            maneger.PlusScore(hit_Score);
        }
        
	}
}
