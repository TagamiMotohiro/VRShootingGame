using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetScore : MonoBehaviour
{
    [SerializeField] GameObject score_object1=null;
    [SerializeField] GameObject score_object2=null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Text score_text1=score_object1.GetComponent<Text>();
        score_text1.text="00000";
        Text score_text2 = score_object2.GetComponent<Text>( );
        score_text2.text = "00000";
    }
}
