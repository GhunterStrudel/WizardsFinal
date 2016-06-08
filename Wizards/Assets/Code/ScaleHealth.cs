using UnityEngine;
using System.Collections;

public class ScaleHealth : MonoBehaviour {

	public Enemy e;
	// Update is called once per frame
	void Update () 
	{
		transform.localScale = new Vector3(e.health/50, 0.2f, 0.001f);
	}
}
