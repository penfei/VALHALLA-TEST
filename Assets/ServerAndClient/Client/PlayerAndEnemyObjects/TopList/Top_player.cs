using UnityEngine;
using UnityEngine.UI;

public class Top_player : MonoBehaviour {

    [HideInInspector]
    public string nick;
    [HideInInspector]
    public int z;
    [HideInInspector]
    public int mz;
    [HideInInspector]
    public int sz;
    [HideInInspector]
    public int score;

    Text nickT;
    Text zT;
    Text mzT;
    Text szT;
    Text scoreT;

    // Use this for initialization
    void Start () {
        nickT = transform.FindChild("nick").GetComponent<Text>();
        zT = transform.FindChild("zombie").GetComponent<Text>();
        mzT = transform.FindChild("mutantzombie").GetComponent<Text>();
        szT = transform.FindChild("strongzombie").GetComponent<Text>();
        scoreT = transform.FindChild("scores").GetComponent<Text>();

        if(nickT != null)
        {
            nickT.text = nick;
        }
        if (zT != null)
        {
            zT.text = z.ToString();
        }
        if (mzT != null)
        {
            mzT.text = mz.ToString();
        }
        if (szT != null)
        {
            szT.text = sz.ToString();
        }
        if (scoreT != null)
        {
            scoreT.text = score.ToString();
        }
    }
}
