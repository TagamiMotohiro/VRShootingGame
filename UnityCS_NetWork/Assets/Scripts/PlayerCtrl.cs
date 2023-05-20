using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPunCallbacks
{
    //����S���@�c��
    //�ړ��ȊO�̃v���C���[����֘A�̃N���X
    private GameObject OVRcamera;
    [SerializeField]
    GameObject Panel;
    public int hp;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //�����̃I�u�W�F�N�g���J�����`��ΏۊO�̃��C���[�ɂ���
            this.gameObject.layer = 6;
            this.transform.GetChild(0).gameObject.layer = 6;
        }
        else
        {
            //����̃I�u�W�F�N�g��[����]�p�l����\��������
            Panel.SetActive(true);
        }
        OVRcamera = GameObject.Find("OVRCameraRig");
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            this.transform.position = OVRcamera.transform.position;
        }
        else
        {
            //
            Panel.transform.LookAt(OVRcamera.transform.position);
        }
	}
}
