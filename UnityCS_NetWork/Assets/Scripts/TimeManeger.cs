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
    int startTime;
    int startTimeSec=0;
    [SerializeField]
    int timeMin = 3;
    int countNum=3;
    int timesec;
    int latetimesec;

    // Start is called before the first frame update
    void Start()
    {
        startTime = PhotonNetwork.ServerTimestamp;
        //�Q�[���̊J�n�������T�[�o�[��̃^�C���X�^���v����擾(���Ԍv�����邤���Ŋ�ƂȂ鎞��)
    }
	public override void OnJoinedRoom()
	{
	  
	}
	// Update is called once per frame
	void Update()
    {
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
            //FINISH�̃e�L�X�g���o��
            Invoke("LoadResult",3f);
            //FINISH���o����3�b��Ƀ��U���g�֑J�ڂ���֐����N��
        }
        TextChengeColor();
        latetimesec = timesec;
    }
    void LoadResult()
    {
        //���U���g�V�[���ɑJ��
        PhotonNetwork.Disconnect();
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
        countNum = 3-(time / 1000)%60;
        CountDownText.text = countNum.ToString();
        //��{�I�ɂ͎c�莞�Ԍv���Ɠ�������
        if (countNum <= 0)
        { 
          //�J�E���g���O�ɂȂ����玞�Ԍv���J�n
          start = true;
          startTime= PhotonNetwork.ServerTimestamp;
          //���Ԍv���p�̃^�C���X�^���v���X�V
          CountDownText.gameObject.SetActive(false);
        }
	}
}
