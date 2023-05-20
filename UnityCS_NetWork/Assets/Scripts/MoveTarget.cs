using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
public class MoveTarget : Target
{
    //����@�c��
    //�C��^�̓G���������e�Ɋւ���N���X(Target����̌p��)
    GameObject target;
    [SerializeField]
    protected float chaseSpeed;

    // Update is called once per frame
    void Update()
    {
        //�^�[�Q�b�g��NULL�o�Ȃ��ꍇ
        if (target == null) { return; }
        //�ڕW�v���C���[�ɐݒ肵�����x�Őڋ�
        Debug.Log(gameObject.name + HP.ToString());
        Move();
    }
    protected virtual void Move()
    {
        Vector3 velocity = (target.transform.position-transform.position).normalized;
        transform.position += (velocity * chaseSpeed)*Time.deltaTime;
    }
	protected override void TargetAnimation()
	{
	}
	public void SetTarget(GameObject g)
    {
        //�ڕW�v���C���[��ݒ�
        target = g;
    }
}
