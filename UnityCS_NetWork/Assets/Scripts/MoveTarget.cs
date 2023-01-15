using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
public class MoveTarget : Target
{
    GameObject target;
    [SerializeField]
    float chaseSpeed;
    //Start is called before the first frame update

    void OnCollisionEnter(Collision collision)
    {
        //�p������CollisionEnter�������ɔ���
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Player")//�v���C���[�ɓ���������
        {
            //�v���C���[��PhotonView���擾���Ă��ꂪ���g�̂��̂�������
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            this.HP = 0;
            //���g������
            maneger.PlusScore(-2000);
            //���_
        }
        else
        if (collision.gameObject.tag == "Shield")//���ɓ��������ꍇ
        {
            //���l��PhotonView���擾������������������������s��
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            PhotonNetwork.Destroy(this.gameObject);
            maneger.PlusScore(200);
            //�h�����ꍇ�͂�����Ƃ������_
        }
    }
    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g��NULL�o�Ȃ��ꍇ
        if (target == null) { return; }
        //�ڕW�v���C���[�ɐݒ肵�����x�Őڋ�
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
    }

    public void SetTarget(GameObject g)
    {
        //�ڕW�v���C���[��ݒ�
        target = g;
    }
}
