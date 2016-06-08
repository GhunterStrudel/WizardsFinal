using UnityEngine;
using System.Collections;

public class Scroll : MonoBehaviour 
{

	public GameObject background;
	public Color[] colors;

	public Texture rune;
    public string spellName;
    public int spellIndex;
    public string spellText;

    public SpellBook sb;
         
     int currentIndex = 0;
     private int nextIndex;

     bool show;
     
     float changeColourTime = 1.0f;
     
     private float lastChange = 0.0f;
     private float timer = 0.0f;
     
     void Start() {
         if (colors == null || colors.Length < 2)
             Debug.Log ("Need to setup colors array in inspector");
         
         nextIndex = (currentIndex + 1) % colors.Length;    
     }
     
     void Update() {
         
         timer += Time.deltaTime;
         
         if (timer > changeColourTime) {
             currentIndex = (currentIndex + 1) % colors.Length;
             nextIndex = (currentIndex + 1) % colors.Length;
             timer = 0.0f;
             
         }
         background.GetComponent<Renderer>().material.color = Color.Lerp (colors[currentIndex], colors[nextIndex], timer / changeColourTime );
     }

     public Rect windowRect = new Rect(Screen.width/2, Screen.height/2, 150, 100);

    void OnGUI() 
    {
        if(show)
            windowRect = GUI.Window(0, new Rect(Screen.width/2-250, Screen.height/2-250, 430, 250), DoMyWindow, "Spell Get");       
    }

    void DoMyWindow(int windowID) 
    {
        string title = "You found the " + spellName + " spell. Press 'Fire' to close.";
        GUI.Label(new Rect(10 + title.Length, 15, 500, 20), title);
        GUI.Label(new Rect(10, 40, 200, 200), rune);
        spellText = GUI.TextArea(new Rect(220, 40, 200, 200), spellText, 9999);

        sb.spells[spellIndex].unlocked = true;

        if(WiimoteStatus.WiiEnabled)
        {
            if(WiimoteStatus.buttonA)
                Destroy(this.gameObject);
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
                Destroy(this.gameObject);
        }
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            show = true;
        }
    }
}
