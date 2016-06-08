using UnityEngine;
using System.Collections;

public class ShowRune : MonoBehaviour
{
    public Texture[] textures;
    public Texture rune;
    public bool show;
    public GameObject image;
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.P))
            show = !show;

        if (show)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                image.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Q))
                image.SetActive(false);

            image.GetComponent<Renderer>().material.mainTexture = rune;


            if (Input.GetKeyDown(KeyCode.Alpha1))
                rune = textures[0];

            if (Input.GetKeyDown(KeyCode.Alpha2))
                rune = textures[1];

            if (Input.GetKeyDown(KeyCode.Alpha3))
                rune = textures[2];

            if (Input.GetKeyDown(KeyCode.Alpha4))
                rune = textures[3];

            if (Input.GetKeyDown(KeyCode.Alpha5))
                rune = textures[4];

            if (Input.GetKeyDown(KeyCode.Alpha6))
                rune = textures[5];

            if (Input.GetKeyDown(KeyCode.Alpha7))
                rune = textures[6];

            if (Input.GetKeyDown(KeyCode.Alpha8))
                rune = textures[7];

            if (Input.GetKeyDown(KeyCode.Alpha9))
                rune = textures[8];

            if (Input.GetKeyDown(KeyCode.Alpha0))
                rune = textures[9];


            if (Input.GetKeyDown(KeyCode.F1))
                rune = textures[10];

            if (Input.GetKeyDown(KeyCode.F2))
                rune = textures[11];

            if (Input.GetKeyDown(KeyCode.F3))
                rune = textures[12];

            if (Input.GetKeyDown(KeyCode.F4))
                rune = textures[13];

            if (Input.GetKeyDown(KeyCode.F5))
                rune = textures[14];

            if (Input.GetKeyDown(KeyCode.F6))
                rune = textures[15];

            if (Input.GetKeyDown(KeyCode.F7))
                rune = textures[16];

            if (Input.GetKeyDown(KeyCode.F8))
                rune = textures[17];
        }
    }
}
