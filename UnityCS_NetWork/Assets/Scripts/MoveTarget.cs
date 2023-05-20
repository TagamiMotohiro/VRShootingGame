using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
public class MoveTarget : Target
{
    //制作　田上
    //砲台型の敵が撃った弾に関するクラス(Targetからの継承)
    GameObject target;
    [SerializeField]
    protected float chaseSpeed;

    // Update is called once per frame
    void Update()
    {
        //ターゲットがNULL出ない場合
        if (target == null) { return; }
        //目標プレイヤーに設定した速度で接近
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
        //目標プレイヤーを設定
        target = g;
    }
}
