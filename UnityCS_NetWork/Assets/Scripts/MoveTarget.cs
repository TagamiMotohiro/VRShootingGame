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
        //継承元のCollisionEnterも同時に発動
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag == "Player")//プレイヤーに当たったら
        {
            //プレイヤーのPhotonViewを取得してそれが自身のものだったら
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            this.HP = 0;
            //自身を消す
            maneger.PlusScore(-2000);
            //減点
        }
        else
        if (collision.gameObject.tag == "Shield")//盾に当たった場合
        {
            //同様にPhotonViewを取得し自分だったら消す処理を行う
            if (collision.gameObject.GetComponent<PhotonView>().IsMine == false) { return; }
            PhotonNetwork.Destroy(this.gameObject);
            maneger.PlusScore(200);
            //防いだ場合はちょっとだけ加点
        }
    }
    // Update is called once per frame
    void Update()
    {
        //ターゲットがNULL出ない場合
        if (target == null) { return; }
        //目標プレイヤーに設定した速度で接近
        transform.position = Vector3.MoveTowards(transform.position,target.transform.position,chaseSpeed*Time.deltaTime);
    }

    public void SetTarget(GameObject g)
    {
        //目標プレイヤーを設定
        target = g;
    }
}
