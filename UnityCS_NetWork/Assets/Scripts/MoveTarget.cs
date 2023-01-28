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
        //ターゲットがNULL出ない場合
        if (target == null) { return; }
        //目標プレイヤーに設定した速度で接近
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
    }
	protected override void TargetAnimation()
	{
	}
	public void SetTarget(GameObject g)
    {
        //目標プレイヤーを設定
        target = g;
    }
}
