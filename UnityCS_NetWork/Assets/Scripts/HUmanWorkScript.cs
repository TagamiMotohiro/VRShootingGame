using UnityEngine;

public class HUmanWorkScript : MonoBehaviour
{
    GUIStyle panelStyle;
    GUIStyle labelStyle;

    [SerializeField]
    bool flag = false;//Åö
    string message;

    Animator animator;

    public void Setflag(bool f)
    {
        flag = f;
    }

    public void SetMessage(string s)
    {
        message = s;
    }
    // Start is called before the first frame update
    void Start()
    {
        message = "Start.";
        animator = GetComponent<Animator>();
    }
    void GuiSetup()
    {
        panelStyle = new GUIStyle(GUI.skin.box);
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 16;
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.normal.textColor = Color.white;
    }
    void OnGui()
    {
        if (!flag) { return; }
        GuiSetup();
        GUILayout.BeginArea(new Rect(10, 10, 310, 210));
        GUILayout.BeginVertical(panelStyle);
        GUILayout.Box("Information");
        GUILayout.Label(message, labelStyle);
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    // Update is called once per frame
    void Update()
    {
        if (!flag) { return; }
        animator.SetFloat("Speed", 0f);
        animator.SetFloat("Direction", 0f);
        animator.SetBool("Jump", false);
        animator.SetBool("Rest", false);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetFloat("Speed", 1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetFloat("Speed", -1f);
        }
        var s = animator.GetFloat("Speed");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetFloat("Speed", s / 3);
        }
        s = animator.GetFloat("Speed");
        transform.position += transform.forward * 0.02f * s;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            var d = animator.GetFloat("Direction");
            animator.SetFloat("Direction", 0.2f);
            animator.SetFloat("GravityControl", 0.2f);
            transform.Rotate(0f,0.1f,0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var d = animator.GetFloat("Direction");
            animator.SetFloat("Direction", -0.2f);
            animator.SetFloat("GravityControl", 0.2f);
            transform.Rotate(0f, -0.1f, 0f);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Jump",true);
        }
    }
	private void OnCollisionEnter(Collision collision)
	{
        if (!flag) { return; }
        if (collision.gameObject.tag == "Player")
        {
            message = "Collision Other Player.";
        }
	}
	private void OnCollisionExit(Collision collision)
	{
        if (!flag) { return; }
        if (collision.gameObject.tag == "Player")
        {
            message = "Collision Exit.";
        }
	}
}
