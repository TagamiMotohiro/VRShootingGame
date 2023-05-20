using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGun : MonoBehaviourPunCallbacks
{
    //����S���@�c��
    //�e�̐����A�ˏo�Ɋ֘A����N���X
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
    [Header ("�v���C���[�X�e�[�^�X")]
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
    [Header ("�Ə��p�̃��C���[�}�X�N")]
    [SerializeField]
    LayerMask rayMask;
    [Header ("�����f��")]
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
        //�R���|�[�l���g�擾���`���[�W�������L���[�u�𐶐�
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
        //�e���^�C�v�ύX
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            //�X�e�[�g�ԍ����v���X
            state_num++;
            //�ԍ��ɍ��킹�ăX�e�[�^�X��ύX
            state = (GUN_STATE)(state_num%System.Enum.GetValues(typeof(GUN_STATE)).Length);
            //magazine���O��(�ˌ����ɃX�e�[�^�X�ύX�����ꍇ���̂���߂�)
            magazine = 0;
        }
    }
    void ChargeCtrl()
    {
        Ray ray = new Ray(this.transform.position + this.transform.forward * 2, this.transform.forward);
        if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            //�E��g���K�[�������Ă����
            magazine = 0;
            //magazine��0��
            if (Physics.Raycast(ray, out hit, rayMask))
            {
                //RayCast�����������瓖�������^�[�Q�b�g���������e�̒ǔ��Ώۂ�
                LockOn(hit);
            }
            else
            {
                //Hit���Ă��Ȃ���ΑΏۂ͂Ȃ���
                aim_Target = new Vector3();
            }
            //�_���̐���\��
            if (Charge >= maxCharge)
            {
                myLR.enabled = true;
            }
            else
            {
                myLR.enabled = false;
            }
            //�`���[�W
            EnelgyCharge();
        }
        else
        {
            //�����Ă��Ȃ���
            myLR.enabled = false;
            if (Charge >= maxCharge)
            {
                //�`���[�W�������Ă�����e���[
                magazine = maxMagazine;
            }
            //�`���[�W�O��
            Charge = 0;
            if (magazine > 0)
            {
                if (coolTime >= late)
                {
                    switch (state) {
                    case GUN_STATE.RAPID:
                        //magazine���Ȃ��Ȃ�܂Œe����
                        myAS.volume = 0.25f;
                        myAS.PlayOneShot(Shot_SE);
                        Fire(transform.position,transform.forward);
                        coolTime = 0;
                        magazine--;
                    break;
                    case GUN_STATE.SHOTGUN:
                        //�U�e����񂾂�����
                        myAS.volume = 0.5f;
                        myAS.PlayOneShot(Shot_SE);
                        ShotGun();
                    break;
                    }
                }
            }
        }
        //�N�[���^�C���𑝉�
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
                //�U�e�Ȃ̂Ń����_���Ƀx�N�g�����Z�o
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
        //�`���[�W�ʂ��󂯂ăL���[�u���g��
        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(0.1f * Charge, 0.1f * Charge, 0.1f * Charge);
    }
    void SetPointer()
    {
       //AIM�p�̃|�C���^�[�𐶐�
       myLR.SetPosition(0, this.transform.position);
       myLR.SetPosition(1, this.transform.position + this.transform.forward * lineRange);
    }
    void Fire(Vector3 pos,Vector3 forcevec)
    {
        //�e�𐶐�
        //���̂܂ܗ͂�^���Ĕ�΂�
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
            //�e�������͉�]����
            go.transform.Rotate(0, CubeRotateSpeed*Time.deltaTime, 0);
        }
    }
    void LockOn(RaycastHit hit)
    {
        //���b�N�����I�̃N���X���擾
        if (hit.collider.tag != "Target") { return; }
        Debug.Log(hit.collider.gameObject.name);
        GameObject g = hit.collider.gameObject;
        //aim_Target = hit.point;
        Target t = g.GetComponent<Target>();
        if (t == null) { return; }
        //�I���Ŏ��g�����点�鏈�����s��
        t.Targeting();
    }
    public void SetStart(bool b)
    {
        stert = b;
    }
}
