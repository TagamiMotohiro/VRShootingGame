using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManeger : MonoBehaviour
{
	//����S���@�c��
	//�I�[�f�B�I�Đ����ɌĂяo���N���X
    AudioSource MyAS;

	private void Start()
	{
		MyAS = gameObject.GetComponent<AudioSource>();
	}
	public void PlaySE(AudioClip clip)
	{
		MyAS.PlayOneShot(clip);	
	}
}
