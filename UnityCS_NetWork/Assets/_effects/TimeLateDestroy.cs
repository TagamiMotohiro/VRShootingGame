using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLateDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //エフェクトをインスタンスしたら3秒後に消滅
        Invoke("MineDestroy",3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void MineDestroy()
    {
        Destroy(gameObject);
    }
}
