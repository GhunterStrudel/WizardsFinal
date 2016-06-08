using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] musicClips;
	AudioClip clip;
	public AudioSource source;
    public Backgrounds background;
	

	public void ChangeMusic(int id)
	{
		clip = musicClips[id];

		if(source.clip != clip)
		{
			source.clip = clip;
			source.Play();
            background.ChangeBackground(id);
		}
	}
}
