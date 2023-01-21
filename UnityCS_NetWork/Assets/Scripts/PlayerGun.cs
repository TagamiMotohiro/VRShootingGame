using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGun : MonoBehaviourPunCallbacks
{
    bool stert = false;
    float Charge = 0;
    public GameObject Bullet;
    public float velocity;
    public float lineRange;
    int magazine=0;
    float coolTime=0;
    [SerializeField]
    float maxCharge;
    [SerializeField]
    int maxMagazine = 9;
    [SerializeField]
    float late=0.1f;
    [SerializeField]
    LayerMask rayMask;
    GameObject go;
    Vector3 aim_Target;
    LineRenderer myLR;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント取得＆チャージを示すキューブを生成
        myLR = GetComponent<LineRenderer>();
        go = PhotonNetwork.Instantiate("27Cube", this.transform.position, Quaternion.identity);
    }
	// Update is called once per frame
	public override void OnJoinedRoom()
	{
        if (go == null)
        {
            go = PhotonNetwork.Instantiate("27Cube", this.transform.position, Quaternion.identity);
        }
	}
	void Update()
    {
        if (stert)
        {
            SetPointer();
            ChargeCtrl();
            CubeTransForm();
        }
    }
    void ChargeCtrl()
    {
        Ray ray = new Ray(this.transform.position + this.transform.forward * 2, this.transform.forward);
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            //右手トリガーを押している間
            magazine = 0;
            //magazineを0に
            if (Physics.Raycast(ray, out hit, rayMask))
            {
                //RayCastが当たったら当たったターゲットが発光＆弾の追尾対象に
                LockOn(hit);
            }
            else
            {
                //Hitしていなければ対象はなしに
                aim_Target = new Vector3();
            }
            //狙いの線を表示
            myLR.enabled = true;
            //チャージ
            EnelgyCharge();
        }
        else
        {
            //押していない間
            myLR.enabled = false;
            if (Charge >= maxCharge)
            {
                //チャージ完了していたら弾を補充
                magazine = maxMagazine;
            }
            //チャージ０に
            Charge = 0;
            if (magazine > 0)
            {
                if (coolTime >= late)
                {
                    //magazineがなくなるまで弾発射
                    Fire();
                    coolTime = 0;
                    magazine--;
                }
            }
        }
        coolTime+=Time.deltaTime;
    }
    void CubeTransForm()
    {
        //チャージ量を受けてキューブが拡大
        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(0.1f * Charge, 0.1f * Charge, 0.1f * Charge);
    }
    void SetPointer()
    {
       myLR.SetPosition(0, this.transform.position);
       myLR.SetPosition(1, this.transform.position + this.transform.forward * lineRange);
    }
    void Fire()
    {
        Debug.Log(aim_Target);
        GameObject g = PhotonNetwork.Instantiate("Bullet", this.transform.position,transform.rotation);
        ////射撃時に追尾対象を設定
        //g.GetComponent<BulletCtrl>().SetTarget(aim_Target);
        g.GetComponent<Rigidbody>().AddForce(this.transform.forward * velocity, ForceMode.Impulse);
    }
    void EnelgyCharge()
    {
        Charge += 3 * Time.deltaTime;
        if (Charge > maxCharge)
        {
            Charge = maxCharge;
        }
        else
        {
            //生成中は回転する
            go.transform.Rotate(0, 4, 0);
        }
    }
    void LockOn(RaycastHit hit)
    {
        //ロックした的のクラスを取得
        if (hit.collider.tag != "Target") { return; }
        Debug.Log(hit.collider.gameObject.name);
        GameObject g = hit.collider.gameObject;
        //aim_Target = hit.point;
        Target t = g.GetComponent<Target>();
        if (t == null) { return; }
        //的側で自身を光らせる処理を行う
        t.Targeting();
    }
    public void SetStart(bool b)
    {
        stert = b;
    }
}
