using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTarget : MoveTarget
{
	//����S���@�c��
	//���i�̒e������I�̓������������N���X
	protected override void Move()
	{
		transform.position += (transform.up*chaseSpeed)*Time.deltaTime;
	}
}
