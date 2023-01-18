using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEngine.LowLevel;
using static UnityEngine.GraphicsBuffer;
using UnityChan;

public class ShotTarget : Target
{
    [SerializeField]
    bool isStart = false;
    [SerializeField]
    float late = 10f;
    float coolTime;
    GameObject firePos;
    [SerializeField]
    List<GameObject> Player_List;
    GameObject LookPlayer;
    LineRenderer myLR;
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            isStart = true;
        }
        myLR = GetComponent<LineRenderer>();
        Player_List = GameObject.FindGameObjectsWithTag("Player").ToList();
        //�S�v���C���[���擾
        base.Start();
        firePos = transform.GetChild(0).gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isStart) { return; }
        LookPlayer = closestPlayer();
        //�֐��ň�ԋ߂��v���C���[���Z�o
        LookAtTransformUp();
        //��ԋ߂��v���C���[�̂ق�������(�C�g��)
        if (coolTime > late)
        {
            coolTime = 0;
            if (PhotonNetwork.LocalPlayer.IsMasterClient == false) { return; }
            GameObject g = PhotonNetwork.Instantiate("ChaseSphere", firePos.transform.position, Quaternion.identity);
            //�N�[���^�C�����I�������e�𐶐�
            g.GetComponent<MoveTarget>().SetTarget(LookPlayer);
            //���̍ےe�ɍ������Ă���v���C���[�̏�����
        }
        coolTime += Time.deltaTime;
    }
	GameObject closestPlayer()//Player_List�̒����玩�g�Ɉ�ԋ߂��v���C���[���Z�o
    {
        GameObject clossest = null;
        float minDistance = float.MaxValue;
        foreach (GameObject g in Player_List)//�擾�����v���C���[���Q�Ƃ���Foreach
        {
            //�����̈ʒu�Ƒ���̈ʒu�̐�Βl���擾
            float gPos = Mathf.Abs((transform.position-g.transform.position).magnitude);
            if (gPos < minDistance)//�ʒu�̐�Βl������Œ�l���Ⴉ�����ꍇ
            {
                //�_���̃v���C���[���X�V
                clossest = g;
                //�ŏ��������X�V
                minDistance = gPos;
            }
        }
        //�ŏI�I�Ɉ�ԋ߂������v���C���[��Ԃ�
        return clossest;
    }
    void LookAtTransformUp()
    {
        this.transform.LookAt(LookPlayer.transform.position);
        this.transform.rotation = transform.rotation*Quaternion.AngleAxis(90,Vector3.right);
        myLR.SetPosition(0,firePos.transform.position);
        myLR.SetPosition(1, LookPlayer.transform.position + Vector3.down * 0.5f);
    }
}
