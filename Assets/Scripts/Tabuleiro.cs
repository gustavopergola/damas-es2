using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TiposNS;
using UnityEngine;

public class Tabuleiro : MonoBehaviour {

	public GameObject pedrasPretas;
	public GameObject pedrasVermelhas;
	public GameObject posicoesAgrupamento;

	private GameObject[,] matrizTabuleiroPosicoes = new GameObject[8,8];
	private int[,] matrizTabuleiroInt = new int[8,8];
	private List<GameObject> posicoes;

	// Use this for initialization
	void Awake () {
		inicalizaPecas();
		inicializaMatrizPosicoes();
		inicializaMatrizInt();
		mostraTabuleiro();
		MatrizPecasParaMatrizPosicoes();
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

	private void inicializaMatrizPosicoes(){//matriz das posições
        int col, lin, contador = 0;
        for (lin = matrizTabuleiroPosicoes.GetLength(0)-1; lin >= 0; lin--)
        { 
            for(col = matrizTabuleiroPosicoes.GetLength(1)-1; col >= 0; col--)
            {
                matrizTabuleiroPosicoes[lin, col] = posicoesAgrupamento.transform.GetChild(contador).gameObject;
                contador++;
            }
        }
    }

	private void inicializaMatrizInt(){
		preencheVazio();
		preenchePecasJogador1();
		preenchePecasJogador2();
	}
	
	private void MatrizPecasParaMatrizPosicoes(){
		int lin, col, atual;
		int pecasPretas = 0;
		int pecasVermelhas = 0;
		//TODO verificar se é dama ou não
		//se essa função for ser utilizada para outro propósito sem ser a inicialização isso será necessário
		for(lin=0;lin<matrizTabuleiroInt.GetLength(0);lin++){
			for(col=0;col<matrizTabuleiroInt.GetLength(0);col++){
				atual = matrizTabuleiroInt[lin, col];
				if(Tipos.isJogador1(atual)){
					matrizTabuleiroPosicoes[lin, col].gameObject.GetComponent<Posicao>().peca = pedrasPretas.transform.GetChild(pecasPretas).gameObject;
					pecasPretas++;
				}else if(Tipos.isJogador2(atual)){
					matrizTabuleiroPosicoes[lin, col].gameObject.GetComponent<Posicao>().peca = pedrasVermelhas.transform.GetChild(pecasVermelhas).gameObject;
					Debug.Log("A"+pedrasVermelhas.transform.GetChild(pecasPretas).gameObject);
					Debug.Log("B"+matrizTabuleiroPosicoes[lin, col].gameObject.GetComponent<Posicao>().peca);
					Debug.Log(matrizTabuleiroPosicoes[lin, col].gameObject);
					pecasVermelhas++;
				}
			}
		}
	}
	

	public void inicalizaPecas(){
		foreach(Transform peca in pedrasPretas.transform){
			peca.gameObject.GetComponent<Peca>().jogador = GameController.getJogador1();
			peca.gameObject.GetComponent<Peca>().tipo = Tipos.getPecaJogador1();
		}
		foreach(Transform peca in pedrasVermelhas.transform){
			peca.gameObject.GetComponent<Peca>().jogador = GameController.getJogador2();
			peca.gameObject.GetComponent<Peca>().tipo = Tipos.getPecaJogador2();
		}
	}

	private void preencheVazio(){
		int lin, col;
		for(lin=0;lin<matrizTabuleiroInt.GetLength(0);lin++){
			for(col=0;col<matrizTabuleiroInt.GetLength(0);col++){
				matrizTabuleiroInt[lin, col] = Tipos.vazio;
			}
		}
	}

	private void preenchePecasJogador1(){
		matrizTabuleiroInt[7,1] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[7,3] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[7,5] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[7,7] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[6,0] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[6,2] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[6,4] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[6,6] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[5,1] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[5,3] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[5,5] = Tipos.getPecaJogador1();
		matrizTabuleiroInt[5,7] = Tipos.getPecaJogador1();
	}

	private void preenchePecasJogador2(){
		matrizTabuleiroInt[0,0] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[0,2] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[0,4] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[0,6] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[1,1] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[1,3] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[1,5] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[1,7] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[2,0] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[2,2] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[2,4] = Tipos.getPecaJogador2();
		matrizTabuleiroInt[2,6] = Tipos.getPecaJogador2();
	}

	private void mostraTabuleiro(){
		int size = matrizTabuleiroInt.GetLength(0);
		string resp = "";
		for(int lin=0; lin<size; lin++){
			for(int col=0; col<size; col++){
				resp = resp + " " + matrizTabuleiroInt[lin,col];
			}
			Debug.Log(resp);
			resp = "";
		}
	}

	/*private List<GameObject> preenche_lista_posicoes(){
		int qtd_posicoes = posicoesAgrupamento.transform.childCount, i = 0;
		List<GameObject> posicoes_new = new List<GameObject> ();
        for (i = 0; i < qtd_posicoes; i++)
			posicoes_new.Add(posicoesAgrupamento.transform.GetChild(i).gameObject);

		return posicoes_new;
	}*/
		
}
