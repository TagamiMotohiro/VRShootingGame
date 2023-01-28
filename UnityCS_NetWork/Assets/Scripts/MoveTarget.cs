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
	
	// Update is called once per frame
	void Update()
    {
        //�^�[�Q�b�g��NULL�o�Ȃ��ꍇ
        if (target == null) { return; }
        //�ڕW�v���C���[�ɐݒ肵�����x�Őڋ�
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
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
