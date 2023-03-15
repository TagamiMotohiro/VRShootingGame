using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueTarget : MoveTarget
{
	//制作担当　田上
	//直進の弾を放つ青い的の動きを書いたクラス
	protected override void Move()
	{
		transform.position += (transform.up*chaseSpeed)*Time.deltaTime;
	}
}
