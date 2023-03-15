using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
public class TimeManeger : MonoBehaviourPunCallbacks
{
    //制作担当　田上
    //制限時間に関するクラス
    bool start = false;
    bool timeset = false;
    [SerializeField]
    TMPro.TextMeshProUGUI TimeText;
    [SerializeField]
    TMPro.TextMeshProUGUI FinishText;
    [SerializeField]
    TMPro.TextMeshProUGUI CountDownText;
    int startTime;
    [SerializeField]
    int firstSpawn;//ゲームスタート直後に生成するターゲットの数
    [SerializeField]
    int timeMin = 1;//残り時間(分)
    int countNum;
    int timesec;
    int latetimesec;
    TargetManeger t;

	// Start is called before the first frame update
	private void Awake()
	{
        //ゲーム開始時刻を全ROM間で同期するためにホストが開始時間を取得
        if (PhotonNetwork.IsMasterClient) 
        {
            var timeProps = new ExitGames.Client.Photon.Hashtable();
            timeProps["StartTime"] = PhotonNetwork.ServerTimestamp;
            PhotonNetwork.CurrentRoom.SetCustomProperties(timeProps);
        }
        t = this.GetComponent<TargetManeger>();
	}
    void Start()
    {
        
        
    }
	// Update is called once per frame
	void Update()
    {
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
            StartCoroutine(LoadResult(3f));
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
    IEnumerator LoadResult(float waitTime)
    {
        //リザルトシーンに遷移
        yield return new WaitForSeconds(waitTime);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Result");
        yield break;
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
        if (start||!timeset) { CountDownText.text = "READY";return; }
        Debug.Log("<color=yellow>StartTime=" + startTime + "</color>");
        //ゲーム開始までのカウントダウン
        int time = unchecked(PhotonNetwork.ServerTimestamp - startTime);
        countNum = 4 - (time / 1000) % 60;
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
            startTime = PhotonNetwork.ServerTimestamp;
            //時間計測用のタイムスタンプを更新(前に同期しているのでここでの取得はずれがない)
            CountDownText.gameObject.SetActive(false);
            if (!PhotonNetwork.IsMasterClient) { return; }
            for (int i = 0; i < firstSpawn*PhotonNetwork.CurrentRoom.Players.Count; i++)
            {
                //ゲームが始まったら的を設定した数*プレイヤー数分生成
                t.Spawn();
            }
        }
    }
	public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
	{
        //開始時間を共有
        startTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        if (startTime == 0)//プロパティを作った時にも変更したコールバックを受け取っている(?)みたいなので
                           //0の状態でコールバックを受け取っても一旦待つ
        { return; }
        timeset = true;
    }
}
