using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanCameraScript : MonoBehaviour
{
    public GameObject target;
    public float distane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) { return; }
        var p = target.transform.position;
        transform.position = p + target.transform.forward * distane * -1 + Vector3.up;
        p.y = 1f;
        transform.LookAt(p);
    }
}
