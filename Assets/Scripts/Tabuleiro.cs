using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabuleiro : MonoBehaviour {

	public List<GameObject> pedrasBlue;
	public List<GameObject> pedrasRed;
	public GameObject posicoesAgrupamento;

	private string[][] matrizTabuleiro = new string[8][];
	private List<GameObject> posicoes;

	// Use this for initialization
	void Start () {
		initialize_matriz_tabuleiro();
		posicoes = preenche_lista_posicoes ();
		Debug.Log ("Qtd de posicções:" + posicoes.Count);
	}

	bool makeMove(){
		//TODO Consulta máquina de regras para ver se jogada é válida
		// return false se não for
		//TODO callMakeValidMove()
		return true;
	}


	private bool makeValidMove(GameObject dama, int posx, int posy){
		return true;
	}

	private void initialize_matriz_tabuleiro(){
		int i = 0;
		for (i = 0; i < matrizTabuleiro.Length; i++) 
			matrizTabuleiro [i] = new string[8];
	}

	private List<GameObject> preenche_lista_posicoes(){
		int qtd_posicoes = posicoesAgrupamento.transform.childCount, i = 0;
		List<GameObject> posicoes_new = new List<GameObject> ();
		for (i = 0; i < qtd_posicoes; i++)
			posicoes_new.Add(posicoesAgrupamento.transform.GetChild(i).gameObject);

		return posicoes_new;
	}
		
}
