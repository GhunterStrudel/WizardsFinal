using UnityEngine;
using System.Collections;

public class Draw : MonoBehaviour {

    public GameObject drawWindow;
    public GameObject drawLine;
    bool doit = false;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(WiimoteStatus.WiiEnabled)
            doit = WiimoteStatus.buttonB;
        else
            doit = Input.GetKey(KeyCode.Q);

            drawWindow.SetActiveRecursively(doit);

            if(doit)
                Time.timeScale = 0.25F;
            else
                Time.timeScale = 1.0F;

            Time.fixedDeltaTime = 0.02F * Time.timeScale;


            if(!doit && drawLine.GetComponent<DrawLine>().arraylist.Count > 0)
            {
                drawLine.GetComponent<DrawLine>().Reset();
            }
    }
}
