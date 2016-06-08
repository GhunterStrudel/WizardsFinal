using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawLine : MonoBehaviour
{
    public float distance = 1;
    public ArrayList arraylist = new ArrayList();

    public InputField input;
    public GestureRecognizer gr;
    public Camera drawCam;

    public RectTransform wiiPointer;
    bool draw;
    public ParticleSystem ps;

    void Start()
    {
        gr.loadGestures();
        //GestureRecognizer.generateGestures();
    }

    void Update()
    {
        if(WiimoteStatus.WiiEnabled)
        {
            draw = WiimoteStatus.buttonA;
        }
        else
            draw = Input.GetMouseButton(0);


        if(draw)
        {
            Vector3 mousePosition = Vector3.zero;
            Vector3 objPosition = Vector3.zero;;

            if(WiimoteStatus.WiiEnabled)
            {
                mousePosition = new Vector3(wiiPointer.transform.position.x, wiiPointer.transform.position.y, distance);
                objPosition = drawCam.ScreenToWorldPoint(mousePosition);
            }
            else
            {
                mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                objPosition = drawCam.ScreenToWorldPoint(mousePosition);
            }
                       
            // Vector3 objPosition = pointer.transform.localPosition;

                transform.position = new Vector3(objPosition.x, objPosition.y, distance);

            arraylist.Add(new Vector2(objPosition.x, objPosition.y));
        }

        //if (Input.GetMouseButtonUp(0))
        if(!draw)
        {
            if(arraylist.Count > 0)
            {
                gr.startRecognizer(arraylist);
                Reset();
            }
        }

        //Save gesture template when using right moue button
        //if (Input.GetMouseButton(1))
        //{
        //    Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        //    Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //    transform.position = new Vector3(objPosition.x, objPosition.y, distance);
        //    arraylist.Add(new Vector2(objPosition.x, objPosition.y));
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    gr.recordTemplate(arraylist, input.text);
        //    GetComponent<TrailRenderer>().Clear();
        //    arraylist.Clear();
        //}
    }

    public void Reset()
    {
        GetComponent<TrailRenderer>().Clear();
        arraylist.Clear();
    }
}
