using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabuleiro : MonoBehaviour {

	string[][] matriz_tabuleiro = new string[8][8];
	List<GameObject> pedras_blue = new List<GameObject>();
	List<GameObject> pedras_red = new List<GameObject>();


	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	boolean makeMove(){
		//TODO Consulta máquina de regras para ver se jogada é válida
		// return false se não for
		//TODO callMakeValidMove()
	}


	private boolean makeValidMove(GameObject dama, int posx, int posy){
		
	}
}
