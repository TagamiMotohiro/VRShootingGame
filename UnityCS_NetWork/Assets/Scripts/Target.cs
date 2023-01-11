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
           �@//��A�N�e�B�u�����������(���ł͕����z�X�g���s��)
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
    void LateUpdate()
    {
        if (this.HP <= 0)//HP��0�ɂȂ�����
        {
            this.gameObject.SetActive(false);
            //��A�N�e�B�u��
			Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            //�����G�t�F�N�g�𔭓�
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
        //���������I�u�W�F�N�g��PhotonView���擾
        if (Collision_photonView == null) { return; }//PhtonView�������Ȃ��I�u�W�F�N�g�ɓ��������ꍇ�������Ȃ�
        this.HP--;//���g�̑ϋv�l�����炷
        if (Collision_photonView.IsMine) {
            maneger.PlusScore(hit_Score);
            if (this.HP <= 0)
            {
                maneger.PlusScore(deferted_Score);
                return;
            }
        }
        
	}
}
