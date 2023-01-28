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
    int firstSpawn;//ゲームスタート直後に生成する弾の数
    [SerializeField]
    int timeMin = 1;//残り時間(分)
    int countNum=3;
    int timesec;
    int latetimesec;

	// Start is called before the first frame update
	private void Awake()
	{
		//プレイの開始時刻をプレイヤー間で共有するために部屋ホストのメインゲーム入室時間を
		//プロパティ上に取得

		//修正中
		//if (!PhotonNetwork.IsMasterClient) { return; }
		//var timeProps = new ExitGames.Client.Photon.Hashtable();
		//timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
		//PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
        //Debug.Log("StartTime="+timeProps["StartTime"].ToString());
       startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
	}
    void Start()
    {
        //マッチメイク時点で設定したゲーム開始時刻を全ROM間で同期
       
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
        //現在の経過時間(int型で1000ミリ秒単位)
        timesec = 59-(time/1000)%60;
        //59に現在の経過時間を秒数に直したものを引いて残り秒数を算出
        if (timesec==59&&latetimesec==0)
        {
        //残り時間の秒数が59になり前フレームの秒数が0だったら
        //残り時間(Min)を１減らす
            timeMin -= 1;
        }
        //時間の算出結果をテキストに出力する
        TimeText.text = timeMin.ToString("D2")+":"+timesec.ToString("D2");
        if (timesec == 0 && timeMin == 0)
        { 
            //残り時間(Sec/Min)が０になった場合
            TimeText.gameObject.SetActive(false);
            //残り時間表示を消す
            FinishText.gameObject.SetActive(true);
            if (PhotonNetwork.IsMasterClient)
            {
                //的を全消滅させる
                TargetAllDestroy();   
            }
            //FINISHのテキストを出す
            Invoke("LoadResult",3f);
            //FINISHを出した3秒後にリザルトへ遷移する関数を起動
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
        //リザルトシーンに遷移
        UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
    }
    void TextChengeColor()//残り30秒になったら時間表示を赤くする
    {
        if (timesec <= 30 && timeMin == 0)
        { 
            TimeText.color = Color.red;
        }
    }
    void CountDown()
    {
        //ゲーム開始までのカウントダウン
        if (start) { return; }
		int time = unchecked(PhotonNetwork.ServerTimestamp - startTime);
        countNum = 5-(time / 1000)%60;
        if (countNum >= 4)
        {
            CountDownText.text = "READY";
            return;
        }
        CountDownText.text = countNum.ToString();
        //基本的には残り時間計測と同じ原理
        if (countNum <= 0)
        { 
          //カウントが0になったら時間計測開始
          start = true;
          startTime= PhotonNetwork.ServerTimestamp;
          //時間計測用のタイムスタンプを更新
          CountDownText.gameObject.SetActive(false);
            if (!PhotonNetwork.IsMasterClient) { return; }
            for (int i = 0; i < firstSpawn; i++)
            {
                //ゲームが始まったら的を3つ生成
                this.GetComponent<TargetManeger>().Spawn();
            }
        }
	}
	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
        startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
    }
}
