using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Target : MonoBehaviourPunCallbacks
{
    //����S���@�c��
    //�e�������Ƃŉ󂹂邷�ׂĂ̓I�̊��N���X
    [Header("�X�e�[�^�X�֘A")]
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
    [Header("�f�ފ֘A(SE �G�t�F�N�g)")]
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
        //�I�u�W�F�N�g���������^�O�Ō��������ق��������炵���̂Ń^�O�Ō���
        audioManeger = GameObject.FindWithTag("Audio").GetComponent<AudioManeger>();
        maneger = GameObject.FindWithTag("PUN2Maneger").GetComponent<ScoreManeger>();
        //Halo��Behabiour�̕�����擾�ł����擾�ł��Ȃ��̂ł���Ŏ擾
        halo = (Behaviour)gameObject.GetComponent("Halo");
        
        PhotonNetwork.AddCallbackTarget(this.gameObject);
    }
    void LateUpdate()
    {
        TargetAnimation();
        if (destroyed)//HP��0�ɂȂ�����
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
        if (isTargeted)
        {
            //���g���_��ꂽ�甭��
            if (halo == null) { return; }
            halo.enabled = true;
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
        this.transform.Rotate(0, (HP * Time.deltaTime)*anim_Speed, 0);
    }
    public void Targeting()
    {
        isTargeted = true;
    }
    public void OnCollisionEnter(Collision collision)
    {
        //�Փː悪�䂾������^�[�Q�b�g���m�̏Փ˂��N���Ă��܂����ꍇ�͉�����������
        if (collision.gameObject.tag=="Untagged"||collision.gameObject.tag=="Target")
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
        if (Collision_photonView.IsMine && !destroyed) {
            if (collision.gameObject.tag == "Player")
            {
                //�v���C���[�ɓ��������瑦���ɔj��
                destroyed = true;
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                audioManeger.PlaySE(Player_Hit_SE);
                //�j�󎞃X�R�A�̔��������_
                maneger.PlusScore(-deferted_Score/2);
                //�I�u�W�F�N�g���L�������N�G�X�g
                TargetOwnerRequest();
                return;
            }
            else
            if (collision.gameObject.tag == "Shield")
            {
                //���ɓ��������ۂ̏���
                //�G�t�F�N�g�⓾�_�ȊO�̓v���C���[�Ƀq�b�g�����ۂ̏����Ɠ��l
                destroyed = true;
                audioManeger.PlaySE(Guard_SE);
                Instantiate(Defended_Effect, transform.position, Quaternion.identity);
                maneger.PlusScore(deferted_Score / 4);
                TargetOwnerRequest();
                return;
            }
            if (this.HP <= 0)
            {
                //�I��j�󂵂��v���C���[�ɏ��L�����Ϗ�
                //�j�󂵂��v���C���[�ɃI�u�W�F�N�g�̔j����S�����Ă��炤
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
            //���g�������z�X�g�łȂ��ꍇ�ɃI�u�W�F�N�g���L�������N�G�X�g(�j�󂳂ꂽ�Ƃ��Ɏg�p)
            photonView.RequestOwnership();
        }
    }
}
