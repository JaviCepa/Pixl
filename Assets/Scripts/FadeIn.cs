using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {
	
	private float fadeTime=255;
	public bool  fire=false;
	public bool  reset=false;
	
	// Use this for initialization
	void Start () {
		GetComponent<GUITexture>().color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
		if (fire) {
			fadeTime*=0.95f;
			GetComponent<GUITexture>().color=Color.Lerp(new Color(255,255,255,0), new Color(255,255,255,1), fadeTime/255);
			if (fadeTime<1) {fadeTime=1;}
		}
		
		if (reset && !fire) {
			fadeTime*=1.05f;
			GetComponent<GUITexture>().color=Color.Lerp(new Color(255,255,255,0), new Color(255,255,255,1), fadeTime/255);
			if (fadeTime>255) {
				fadeTime=255;
			}
		}
	}
	
	public void Begin() {
		fire=true;
		reset=false;
	}
	public void End() {
		fire=false;
		reset=true;
	}
}
