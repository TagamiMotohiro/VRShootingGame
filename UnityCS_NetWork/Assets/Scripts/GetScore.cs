using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GetScore : MainGamePUNmaneger {
    [SerializeField] GameObject score_object1=null;
    [SerializeField] GameObject score_object2=null;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI score_text1=score_object1.GetComponent<TextMeshProUGUI>();
        score_text1.text=MainGamePUNmaneger.score.ToString();
        TextMeshProUGUI score_text2 = score_object2.GetComponent<TextMeshProUGUI>( );
        score_text2.text = "00000";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
