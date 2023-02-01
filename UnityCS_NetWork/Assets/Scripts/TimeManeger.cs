using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class TimeManeger : MonoBehaviourPunCallbacks
{
    bool start = false;
    bool timeset = false;
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
    int firstSpawn;//�Q�[���X�^�[�g����ɐ�������^�[�Q�b�g�̐�
    [SerializeField]
    int timeMin = 1;//�c�莞��(��)
    int countNum;
    int timesec;
    int latetimesec;

	// Start is called before the first frame update
	private void Awake()
	{
        if (PhotonNetwork.IsMasterClient) 
        {
            var timeProps = new ExitGames.Client.Photon.Hashtable();
            timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
            Debug.Log("<color=red>NowTime=" + timeProps["StartTime"] + "</color>");
            Debug.Log("<color=red>StartTime=" + PhotonNetwork.CurrentRoom.CustomProperties["StartTime"] + "</color>");
            PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
            Debug.Log("<color=red>StartTime=" + PhotonNetwork.CurrentRoom.CustomProperties["StartTime"] + "</color>");
        }
	}
    void Start()
    {
        //�Q�[���J�n������SROM�Ԃœ���
        
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
        if (start||!timeset) { CountDownText.text = "READY";return; }
        Debug.Log("<color=yellow>StartTime=" + startTime + "</color>");
        //�Q�[���J�n�܂ł̃J�E���g�_�E��
        int time = unchecked(PhotonNetwork.ServerTimestamp - startTime);
        countNum = 4 - (time / 1000) % 60;
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
            startTime = PhotonNetwork.ServerTimestamp;
            //���Ԍv���p�̃^�C���X�^���v���X�V
            CountDownText.gameObject.SetActive(false);
            if (!PhotonNetwork.IsMasterClient) { return; }
            for (int i = 0; i < firstSpawn*PhotonNetwork.CurrentRoom.Players.Count; i++)
            {
                //�Q�[�����n�܂�����I��ݒ肵����*�v���C���[��������
                this.GetComponent<TargetManeger>().Spawn();
            }
        }
    }
	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
        startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        Debug.Log("<color=blue>StartTime=" + PhotonNetwork.CurrentRoom.CustomProperties["StartTime"] + "</color>");
        if (startTime == 0)//�v���p�e�B����������ɂ��ύX�����R�[���o�b�N���󂯎���Ă���(?)�݂����Ȃ̂�
                           //0�̏�ԂŃR�[���o�b�N���󂯎���Ă���U�҂�
        { return; }
        timeset = true;
    }
}
