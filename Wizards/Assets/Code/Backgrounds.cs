using UnityEngine;
using System.Collections;

public class Backgrounds : MonoBehaviour {

	public Texture[] textures;
	public GameObject background;
    public float scrollDecelerate;
	
	public void Update()
	{
		background.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(transform.position.x/ scrollDecelerate, 0);//Mathf.Clamp(-transform.position.y, -0.2f, 0f));
	}

	public void ChangeBackground(int id)
	{
		background.GetComponent<Renderer>().material.SetTexture("_MainTex", textures[id]);
	}
}
