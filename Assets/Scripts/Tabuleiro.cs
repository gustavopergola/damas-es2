using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TiposNS;
using UnityEngine;

public class Tabuleiro : MonoBehaviour {

	public List<GameObject> pedrasBlue;
	public List<GameObject> pedrasRed;
	public GameObject posicoesAgrupamento;

	private GameObject[,] matrizTabuleiroObj = new GameObject[8,8];
	private int[,] matrizTabuleiroInt = new int[8,8];
	private List<GameObject> posicoes;

	// Use this for initialization
	void Start () {
		initialize_matriz_tabuleiro();
		inicializaMatrizPecas();
		Debug.Log(matrizTabuleiroInt[0,0]);
		mostraTabuleiro();
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

	private void initialize_matriz_tabuleiro(){//matriz das posições
        int col, lin, contador = 0;
        for (col = matrizTabuleiroObj.GetLength(0)-1; col >= 0; col--)
        { 
            for(lin = matrizTabuleiroObj.GetLength(1)-1; lin >= 0; lin--)
            {
                matrizTabuleiroObj[col, lin] = posicoesAgrupamento.transform.GetChild(contador).gameObject;
                contador++;
            }
        }
    }

	private void inicializaMatrizPecas(){
		preencheVazio();
		preenchePecasJogador1();
		preenchePecasJogador2();
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
