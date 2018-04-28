using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Tabuleiro : MonoBehaviour {

	public List<GameObject> pedrasBlue;
	public List<GameObject> pedrasRed;
	public GameObject posicoesAgrupamento;

	private GameObject[,] matrizTabuleiro = new GameObject[8,8];
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
        int col, lin, contador = 0;
        string resposta = "";
        Debug.Log("QUE QUE ISSO? ", posicoesAgrupamento.transform);
        Debug.Log(matrizTabuleiro.GetLength(0));
        Debug.Log(matrizTabuleiro.GetLength(1));
        for (col = matrizTabuleiro.GetLength(0)-1; col >= 0; col--)
        { 
            for(lin = matrizTabuleiro.GetLength(1)-1; lin >= 0; lin--)
            {
                matrizTabuleiro[col, lin] = posicoesAgrupamento.transform.GetChild(contador).gameObject;
                resposta = resposta + matrizTabuleiro[col, lin];
                contador++;
                Debug.Log("COLUNA: " + col + " LINHA: " + lin + "     ELEMENTO: " + matrizTabuleiro[col, lin]);
                //Debug.Log("COLUNA: "+col+" LINHA: "+lin+"CONTADOR: "+contador+"ELEMENTO: "+ posicoesAgrupamento.transform.GetChild(contador).gameObject);
            }
            Debug.Log(resposta);
            resposta = "";
        }
    }

	private List<GameObject> preenche_lista_posicoes(){
		int qtd_posicoes = posicoesAgrupamento.transform.childCount, i = 0;
		List<GameObject> posicoes_new = new List<GameObject> ();
        for (i = 0; i < qtd_posicoes; i++)
			posicoes_new.Add(posicoesAgrupamento.transform.GetChild(i).gameObject);

		return posicoes_new;
	}
		
}
