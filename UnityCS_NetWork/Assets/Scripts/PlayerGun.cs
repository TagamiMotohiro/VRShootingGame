using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGun : MonoBehaviourPunCallbacks
{
    bool stert = false;
    float Charge = 0;
    public GameObject Bullet;
    public float velocity;
    public float lineRange;
    int magazine=0;
    float coolTime=0;
    [SerializeField]
    float maxCharge;
    [SerializeField]
    int maxMagazine = 9;
    [SerializeField]
    float late=0.1f;
    [SerializeField]
    LayerMask rayMask;
    GameObject go;
    LineRenderer myLR;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        myLR = GetComponent<LineRenderer>();
        go = PhotonNetwork.Instantiate("27Cube", this.transform.position, Quaternion.identity);
    }
	// Update is called once per frame
	public override void OnJoinedRoom()
	{
        
	}
	void Update()
    {
        if (stert)
        {
            myLR.SetPosition(0, this.transform.position);
            myLR.SetPosition(1, this.transform.position + this.transform.forward * lineRange);
            Ray ray = new Ray(this.transform.position+this.transform.forward*2, this.transform.forward);
            if (OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
            {
                magazine = 0;
                if (Physics.Raycast(ray, out hit, rayMask))
                {
                    LockOn(hit);
                }
                myLR.enabled = true;
                EnelgyCharge();
            }
            else {
                myLR.enabled = false;
                if (Charge >= maxCharge)
                {
                    magazine = maxMagazine;
                } 
                Charge = 0;
                if (magazine > 0)
                {
                    if (coolTime >= late)
                    {
                        Fire();
                        coolTime = 0; 
                        magazine--;
                    }
                }
            }
            coolTime += Time.deltaTime;
            CubeTransForm();
        }
    }
    void CubeTransForm()
    {
        go.transform.position = this.transform.position;
        go.transform.localScale = new Vector3(0.1f * Charge, 0.1f * Charge, 0.1f * Charge);
    }
    void Fire()
    {
        GameObject g = PhotonNetwork.Instantiate("Bullet", this.transform.position, Quaternion.identity);
        g.GetComponent<Rigidbody>().AddForce(this.transform.forward * velocity, ForceMode.Impulse);
    }
    void EnelgyCharge()
    {
        Charge += 3 * Time.deltaTime;
        if (Charge > maxCharge)
        {
            Charge = maxCharge;
        }
        else
        {
            go.transform.Rotate(0, 4, 0);
        }
    }
    void LockOn(RaycastHit hit)
    {
        Target t = hit.collider.gameObject.GetComponent<Target>();
        Debug.Log(hit.collider.gameObject.name);
        if (t == null) { return; }
        t.Targeting();
    }
    public void SetStart(bool b)
    {
        stert = b;
    }
}
