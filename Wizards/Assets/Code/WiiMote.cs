using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Linq;
using WiimoteApi;


public class WiiMote : MonoBehaviour {

	public Wiimote wiimote;
	private Vector3 wmpOffset = Vector3.zero;
	//public float[] accel;

    public float pointer0;
    public float pointer1;
    public int smoothTreshold;
    public RectTransform ir_pointer;
    LimitedQueue<float> inputX;
    LimitedQueue<float> inputY;

    void Start()
    {
        inputX = new LimitedQueue<float>(smoothTreshold);
        inputY = new LimitedQueue<float>(smoothTreshold);
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (!WiimoteManager.HasWiimote()) 
		{ 
			 return; 
		}

		wiimote = WiimoteManager.Wiimotes[0];

		int ret;
        do  
            ret = wiimote.ReadWiimoteData();
        while (ret > 0);

		WiimoteStatus.buttonA = wiimote.Button.a;
        WiimoteStatus.buttonB = wiimote.Button.b;
		

		//wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS_ACCEL_EXT16);
		//accel = wiimote.Accel.GetCalibratedAccelData();

        float[] pointer = wiimote.Ir.GetPointingPosition();
        pointer0 = pointer[0]*10;
        pointer1 = pointer[1]*10;

        ir_pointer.anchorMin = new Vector2(pointer[0], pointer[1]);
        ir_pointer.anchorMax = new Vector2(pointer[0], pointer[1]);
        //cube.transform.position = new Vector3(pointer[0], pointer[1], 5.9f);

        inputX.Enqueue(pointer0);
        inputY.Enqueue(pointer1);

        WiimoteStatus.WiiX = inputX.Average();
        WiimoteStatus.WiiY = inputY.Average();       
		//cube.transform.eulerAngles = GetAccelVector()*10;
	}

    float nunnormalizer;
	void OnGUI()
	{

    		GUILayout.Label("Wiimote Found: " + WiimoteManager.HasWiimote());
            if (GUILayout.Button("Find Wiimote"))
                WiimoteManager.FindWiimotes();

            if (GUILayout.Button("Cleanup"))
            {
                WiimoteManager.Cleanup(wiimote);
                WiimoteStatus.WiiEnabled = false;
                wiimote = null;
            }

            if (wiimote == null)
            return;

            if(GUILayout.Button("Basic", GUILayout.Width(100)))
            {
                nunnormalizer = 123f;
                wiimote.SetupIRCamera(IRDataType.BASIC);
            }

        if (wiimote != null && wiimote.current_ext != ExtensionController.NONE)
        {
            WiimoteStatus.WiiEnabled = true;
            GUIStyle bold = new GUIStyle(GUI.skin.button);
            bold.fontStyle = FontStyle.Bold;
            if (wiimote.current_ext == ExtensionController.NUNCHUCK) 
            {
                 GUILayout.Label("Wiimote X:" + pointer0);
                 GUILayout.Label("Wiimote Y:" + pointer1);
                GUILayout.Label("Nunchuck:", bold);
                NunchuckData data = wiimote.Nunchuck;
                GUILayout.Label("Stick X: " + data.stick[0] + ", Stick Y " + data.stick[1]);
                WiimoteStatus.nunchuckX = data.stick[0]-nunnormalizer;
                WiimoteStatus.nunchuckY = data.stick[1];
                WiimoteStatus.buttonZ = data.z;

                GUILayout.Label("C: " + data.c);
                GUILayout.Label("Z: " + data.z);
            }
        }
            
	}

	private Vector3 GetAccelVector()
    {
        float accel_x;
        float accel_y;
        float accel_z;

        float[] accel = wiimote.Accel.GetCalibratedAccelData();
        accel_x = accel[0];
        accel_y = -accel[2];
        accel_z = -accel[1];

        return new Vector3(accel_x, accel_y, accel_z).normalized;
    }

    void OnApplicationQuit() 
    {
        if(wiimote != null)
        {
			WiimoteManager.Cleanup(wiimote);
	        wiimote = null;
        }
	}

    void Smoothing()
    {

    }
}

public class LimitedQueue<T> : Queue<T> {
        private int limit = -1;

        public int Limit {
            get { return limit; }
            set { limit = value; }
        }

        public LimitedQueue(int limit)
            : base(limit) {
            this.Limit = limit;
        }

        public new void Enqueue(T item) {
            while (this.Count >= this.Limit) {
                this.Dequeue();
            }
            base.Enqueue(item);
        }
    }
