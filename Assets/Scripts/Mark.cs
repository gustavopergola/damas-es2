using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mark : MonoBehaviour {

	private int timeToFadeOut = 50;
	private SpriteRenderer spriteRenderer;
    private Color newColor;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();		
		// força sprite 100% opaco
		newColor = spriteRenderer.color;
		newColor.a = 1f;
		spriteRenderer.color = newColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//fade out animation
		
		timeToFadeOut--;
		newColor = spriteRenderer.color;
		newColor.a -= 0.02f;
		spriteRenderer.color = newColor;
		
		if (timeToFadeOut <= 0)
			Destroy(this.gameObject);
	}
}
