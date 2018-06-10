using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peca : MonoBehaviour {

	public Jogador jogador;
	public int tipo;
	public Posicao posicao;
	private int timeToFadeOut = 50;
	private SpriteRenderer spriteRenderer;
    private Color newColor;
	private bool destruirFlag = false;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();		
		// força sprite 100% opaco
		newColor = spriteRenderer.color;
		newColor.a = 1f;
		spriteRenderer.color = newColor;
	}
	
	void FixedUpdate () {
		//fade out animation
		if (destruirFlag){
			timeToFadeOut--;
			newColor = spriteRenderer.color;
			newColor.a -= 0.02f;
			spriteRenderer.color = newColor;
			
			if (timeToFadeOut <= 0)
				Destroy(this.gameObject);
		}
	}

	public void destruir(){
		destruirFlag = true;
	}
}
