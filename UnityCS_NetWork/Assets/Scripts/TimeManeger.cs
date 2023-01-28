using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class TimeManeger : MonoBehaviourPunCallbacks
{
    bool start = false;
    [SerializeField]
    TMPro.TextMeshProUGUI TimeText;
    [SerializeField]
    TMPro.TextMeshProUGUI FinishText;
    [SerializeField]
    TMPro.TextMeshProUGUI CountDownText;
    [SerializeField]
    GameObject ExplainText;
    int startTime;
    int startTimeSec=0;
    [SerializeField]
    int firstSpawn;//�Q�[���X�^�[�g����ɐ�������e�̐�
    [SerializeField]
    int timeMin = 1;//�c�莞��(��)
    int countNum=3;
    int timesec;
    int latetimesec;

	// Start is called before the first frame update
	private void Awake()
	{
		//�v���C�̊J�n�������v���C���[�Ԃŋ��L���邽�߂ɕ����z�X�g�̃��C���Q�[���������Ԃ�
		//�v���p�e�B��Ɏ擾

		//�C����
		//if (!PhotonNetwork.IsMasterClient) { return; }
		//var timeProps = new ExitGames.Client.Photon.Hashtable();
		//timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
		//PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
        //Debug.Log("StartTime="+timeProps["StartTime"].ToString());
       startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
	}
    void Start()
    {
        //�}�b�`���C�N���_�Őݒ肵���Q�[���J�n������SROM�Ԃœ���
       
        //Debug.Log((int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"]);
    }
	// Update is called once per frame
	void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            ExplainText.SetActive(false);
        }else if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            ExplainText.SetActive(true);
        }
        CountDown();
        if (!start) { return; }
        int time =unchecked(PhotonNetwork.ServerTimestamp-startTime);
        //���݂̌o�ߎ���(int�^��1000�~���b�P��)
        timesec = 59-(time/1000)%60;
        //59�Ɍ��݂̌o�ߎ��Ԃ�b���ɒ��������̂������Ďc��b�����Z�o
        if (timesec==59&&latetimesec==0)
        {
        //�c�莞�Ԃ̕b����59�ɂȂ�O�t���[���̕b����0��������
        //�c�莞��(Min)���P���炷
            timeMin -= 1;
        }
        //���Ԃ̎Z�o���ʂ��e�L�X�g�ɏo�͂���
        TimeText.text = timeMin.ToString("D2")+":"+timesec.ToString("D2");
        if (timesec == 0 && timeMin == 0)
        { 
            //�c�莞��(Sec/Min)���O�ɂȂ����ꍇ
            TimeText.gameObject.SetActive(false);
            //�c�莞�ԕ\��������
            FinishText.gameObject.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
            {
                //�I��S���ł�����
                TargetAllDestroy();   
            }
            //FINISH�̃e�L�X�g���o��
            Invoke("LoadResult",3f);
            //FINISH���o����3�b��Ƀ��U���g�֑J�ڂ���֐����N��
        }
        TextChengeColor();
        latetimesec = timesec;
    }
    void TargetAllDestroy()
    {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Target");
        for (int i = 0; i < g.Length; i++)
        {
            PhotonNetwork.Destroy(g[i]);
        }
    }
    void LoadResult()
    {
        //���U���g�V�[���ɑJ��
        UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
    }
    void TextChengeColor()//�c��30�b�ɂȂ����玞�ԕ\����Ԃ�����
    {
        if (timesec <= 30 && timeMin == 0)
        { 
            TimeText.color = Color.red;
        }
    }
    void CountDown()
    {
        //�Q�[���J�n�܂ł̃J�E���g�_�E��
        if (start) { return; }
		int time = unchecked(PhotonNetwork.ServerTimestamp - startTime);
        countNum = 5-(time / 1000)%60;
        if (countNum >= 4)
        {
            CountDownText.text = "READY";
            return;
        }
        CountDownText.text = countNum.ToString();
        //��{�I�ɂ͎c�莞�Ԍv���Ɠ�������
        if (countNum <= 0)
        { 
          //�J�E���g��0�ɂȂ����玞�Ԍv���J�n
          start = true;
          startTime= PhotonNetwork.ServerTimestamp;
          //���Ԍv���p�̃^�C���X�^���v���X�V
          CountDownText.gameObject.SetActive(false);
            if (!PhotonNetwork.IsMasterClient) { return; }
            for (int i = 0; i < firstSpawn; i++)
            {
                //�Q�[�����n�܂�����I��3����
                this.GetComponent<TargetManeger>().Spawn();
            }
        }
	}
	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
        startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
    }
}
