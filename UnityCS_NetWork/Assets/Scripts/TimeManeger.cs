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
        timesec = 59-(time/1000)%60;
        
        if (timesec==59&&latetimesec==0)
        {
            timeMin -= 1;
            Debug.Log("timeMinå∏è≠" + "timesec=" + timesec.ToString()) ;
        }
        TimeText.text = timeMin.ToString("D2")+":"+timesec.ToString("D2");
        if (timesec == 0 && timeMin == 0)
        { 
            TimeText.gameObject.SetActive(false);
            FinishText.gameObject.SetActive(true);
            Invoke("LoadResult",3f);
        }
        TextChengeColor();
        latetimesec = timesec;
    }
    void LoadResult()
    {
        PhotonNetwork.Disconnect();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
    }
    void TextChengeColor()
    {
        if (timesec <= 30 && timeMin == 0)
        { 
            TimeText.color = Color.red;
        }
    }
    void CountDown()
    {
        if (start) { return; }
		int time = unchecked(PhotonNetwork.ServerTimestamp - startTime);
        countNum = 3-(time / 1000)%60;
        CountDownText.text = countNum.ToString();
        if (countNum <= 0)
        { start = true;
          startTime= PhotonNetwork.ServerTimestamp;
          CountDownText.gameObject.SetActive(false);
        }
	}
}
