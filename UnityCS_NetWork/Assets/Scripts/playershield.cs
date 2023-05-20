using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.VisualScripting;

public class playershield : MonoBehaviourPunCallbacks
{
    //����S���@�c��
    //����̏������Ɋւ���N���X
    GameObject shield;
    float charge=0;
    [SerializeField]
    float speed;//�V�[���h�W�J���x
    [SerializeField]
    float shield_Distance;//�R���g���[���[�Ǝ��ۓW�J�����V�[���h�̋���
    // Start is called before the first frame update
    void Start()
    {
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            shield = PhotonNetwork.Instantiate("shield", this.transform.position+this.transform.forward*shield_Distance, this.transform.rotation * Quaternion.EulerAngles(90f, 0f, 0f));
            SetCanWalk(false);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger))
        {
            ShieldCharge();
        }
        else
        {
            SetCanWalk(true);
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger))
        {
            charge = 0;
            PhotonNetwork.Destroy(shield);
        }
    }
    void ShieldCharge()
    { 
        if (shield == null) { return; }
        shield.transform.position = this.transform.position + this.transform.forward * shield_Distance;
        shield.transform.rotation = this.transform.rotation * Quaternion.EulerAngles(90f, 0f, 0f);
        shield.transform.localScale = new Vector3(0.15f * charge, 0.05f, 0.15f * charge);
        charge += Time.deltaTime * speed;
        if (charge >= 2)
        {
            charge = 2;
        }
    }
    void SetCanWalk(bool isCanWalk)
    {
		transform.root.gameObject.GetComponent<VRPlayerWork>().SetStert(isCanWalk);
	}
}
