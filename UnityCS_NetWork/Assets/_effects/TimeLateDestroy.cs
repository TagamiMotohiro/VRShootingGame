using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLateDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�G�t�F�N�g���C���X�^���X������3�b��ɏ���
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
