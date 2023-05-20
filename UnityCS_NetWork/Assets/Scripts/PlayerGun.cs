using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGun : MonoBehaviourPunCallbacks
{
    //制作担当　田上
    //弾の生成、射出に関連するクラス
    public enum GUN_STATE :int{ 
        RAPID=0,
        SHOTGUN=1
    }
    public GUN_STATE state { get; private set; } = GUN_STATE.RAPID;
    int state_num=0;
    bool stert = false;
    float Charge = 0;
    public float velocity;
    public float lineRange;
    float coolTime=0;
    int magazine=0;
    [Header ("プレイヤーステータス")]
    [SerializeField]
    float maxCharge;
    [SerializeField]
    int maxMagazine = 9;
    [SerializeField]
    float ChargeSpeed;
    [SerializeField]
    float late=0.1f;
    [SerializeField]
    float CubeRotateSpeed;
    [Header ("照準用のレイヤーマスク")]
    [SerializeField]
    LayerMask rayMask;
    [Header ("音声素材")]
    [SerializeField]
    AudioClip Shot_SE;
    [SerializeField]
    AudioClip Charge_SE;
    AudioSource myAS;
    GameObject go;
    Vector3 aim_Target;
    LineRenderer myLR;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        //コンポーネント取得＆チャージを示すキューブを生成
        myAS = this.gameObject.GetComponent<AudioSource>();
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
            SwichState();
            SetPointer();
            ChargeCtrl();
            CubeTransForm();
        }
    }
    void SwichState()
    {
        //銃口タイプ変更
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            //ステート番号をプラス
            state_num++;
            //番号に合わせてステータスを変更
            state = (GUN_STATE)(state_num%System.Enum.GetValues(typeof(GUN_STATE)).Length);
            //magazineを０に(射撃中にステータス変更した場合撃つのをやめる)
            magazine = 0;
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
            if (Charge >= maxCharge)
            {
                myLR.enabled = true;
            }
            else
            {
                myLR.enabled = false;
            }
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
                    switch (state) {
                    case GUN_STATE.RAPID:
                        //magazineがなくなるまで弾発射
                        myAS.volume = 0.25f;
                        myAS.PlayOneShot(Shot_SE);
                        Fire(transform.position,transform.forward);
                        coolTime = 0;
                        magazine--;
                    break;
                    case GUN_STATE.SHOTGUN:
                        //散弾を一回だけ発射
                        myAS.volume = 0.5f;
                        myAS.PlayOneShot(Shot_SE);
                        ShotGun();
                    break;
                    }
                }
            }
        }
        //クールタイムを増加
        coolTime+=Time.deltaTime;
    }
    void ShotGun()
    {
        float drag = 0.1f;
        Vector3 pos = transform.position;
        Vector3 dragX= -transform.right*drag;
        Vector3 dragY = -transform.up*drag;
        for (int x=0;x<3;x++)
        {
            for (int y=0;y<3;y++) {
                pos += dragY;
                //散弾なのでランダムにベクトルを算出
                var dir = new Vector3(transform.forward.x+Random.Range(-drag*0.1f,drag*0.1f),
                                      transform.forward.y+Random.Range(-drag*0.1f,drag*0.1f),
                                      transform.forward.z+Random.Range(-drag*0.1f,drag*0.1f));
                Fire(pos,dir);
                dragY += transform.up*drag;
            }
            dragY = -transform.up*drag;
            dragX += transform.right*drag;
            pos += dragX;
        }
        magazine = 0;
    }
    void CubeTransForm()
    {
        //チャージ量を受けてキューブが拡大
        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(0.1f * Charge, 0.1f * Charge, 0.1f * Charge);
    }
    void SetPointer()
    {
       //AIM用のポインターを生成
       myLR.SetPosition(0, this.transform.position);
       myLR.SetPosition(1, this.transform.position + this.transform.forward * lineRange);
    }
    void Fire(Vector3 pos,Vector3 forcevec)
    {
        //弾を生成
        //そのまま力を与えて飛ばす
        GameObject g = PhotonNetwork.Instantiate("Bullet",pos,transform.rotation);
        g.GetComponent<Rigidbody>().AddForce(forcevec * velocity, ForceMode.Impulse);
    }
    void EnelgyCharge()
    {
        Charge += ChargeSpeed * Time.deltaTime;
        if (Charge > maxCharge)
        {
            Charge = maxCharge;
        }
        else
        {
            //弾生成中は回転する
            go.transform.Rotate(0, CubeRotateSpeed*Time.deltaTime, 0);
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
