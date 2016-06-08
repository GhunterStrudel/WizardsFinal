using UnityEngine;
using System.Collections;

public class Pillar : MonoBehaviour {

	public GameObject child;
	// Update is called once per frame
	void Update () 
	{
		child.transform.localScale = Vector3.Lerp(child.transform.localScale, new Vector3(1,0.75f,1), 10 * Time.deltaTime);
	}
}
