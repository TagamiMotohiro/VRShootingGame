using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextManeger : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    List<Transform> text;
    [SerializeField]
    List<Transform> anchor;
    [SerializeField]
    List<LineRenderer> textLR;
    // Start is called before the first frame update
    void Start()
    {
        for (int i=0;i<text.Count;i++) {
            LineRenderer r = text[i].GetComponent<LineRenderer>();
            textLR.Add(r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0;i<anchor.Count;i++) {
            SetLine(text[i], anchor[i], textLR[i]);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two)) {
            canvas.SetActive(!canvas.activeSelf);
        }
    }
    void SetLine(Transform textPos,Transform anchor,LineRenderer lr) {
        lr.SetPosition(0,textPos.position);
        lr.SetPosition(1,anchor.position);
    }
}
