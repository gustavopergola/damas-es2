using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TiposNS;
using UnityEngine;

namespace TabuleiroNS
{
    public class Tabuleiro : MonoBehaviour
    {
        public static Tabuleiro instance { get; private set; }
        public GameObject pedrasPretas;
        public GameObject pedrasVermelhas;
        public GameObject posicoesAgrupamento;

        public GameObject[,] matrizTabuleiroPosicoes = new GameObject[8, 8];
        public int[,] matrizTabuleiroInt = new int[8, 8];
        private List<GameObject> posicoes;

        // Use this for initialization
        void Awake()
        {
            instance = this;
            inicalizaPecas();
            inicializaMatrizPosicoes();
            inicializaMatrizInt();
            MatrizPecasParaMatrizPosicoes();
        }

        bool makeMove()
        {
            //TODO Consulta máquina de regras para ver se jogada é válida
            // return false se não for
            //TODO callMakeValidMove()
            return true;
        }


        private bool makeValidMove(GameObject dama, int posx, int posy)
        {
            return true;
        }

        private void inicializaMatrizPosicoes()
        {//matriz das posições
            int col, lin, contador = 0;
            int tamanho = getTamanhoTabuleiro();
            for (lin = tamanho - 1; lin >= 0; lin--)
            {
                for (col = tamanho - 1; col >= 0; col--)
                {
                    matrizTabuleiroPosicoes[lin, col] = posicoesAgrupamento.transform.GetChild(contador).gameObject;
                    contador++;
                }
            }
        }

        private void inicializaMatrizInt()
        {
            preencheVazio();
            preenchePecasJogador1();
            preenchePecasJogador2();
        }

        private void MatrizPecasParaMatrizPosicoes()
        {
            int lin, col, atual;
            int pecasPretas = 0;
            int pecasVermelhas = 0;
            Posicao posicaoAtual;
            int tamanho = getTamanhoTabuleiro();
            //TODO verificar se é dama ou não
            //se essa função for ser utilizada para outro propósito sem ser a inicialização isso será necessário
            for (lin = 0; lin < tamanho; lin++)
            {
                for (col = 0; col < tamanho; col++)
                {
                    atual = matrizTabuleiroInt[lin, col];
                    posicaoAtual = matrizTabuleiroPosicoes[lin, col].gameObject.GetComponent<Posicao>();
                    posicaoAtual.lin = lin;
                    posicaoAtual.col = col;
                    if (Tipos.isJogador1(atual))
                    {
                        posicaoAtual.peca = pedrasPretas.transform.GetChild(pecasPretas).gameObject;
                        pecasPretas++;
                    }
                    else if (Tipos.isJogador2(atual))
                    {
                        posicaoAtual.peca = pedrasVermelhas.transform.GetChild(pecasVermelhas).gameObject;
                        pecasVermelhas++;
                    }
                }
            }
        }

        public List<int[]> posicoesJogadorX(int jogador){
            List<int[]> posicoes = new List<int[]>();
            int[] posicao = new int[2];
            Posicao posicaoAtual;
            Transform pecas = jogador == 1 ? pedrasPretas.transform : pedrasVermelhas.transform;
            for(int i=0; i<pecas.childCount; i++){
                posicaoAtual = pecas.GetChild(i).gameObject.GetComponent<Peca>().posicao.GetComponent<Posicao>();
                posicao[0] = posicaoAtual.lin;
                posicao[1] = posicaoAtual.col;
                posicoes.Add((int[])posicao.Clone());
            }
            return posicoes;
        }


        public void inicalizaPecas()
        {
            foreach (Transform peca in pedrasPretas.transform)
            {
                peca.gameObject.GetComponent<Peca>().jogador = GameController.getJogador1();
                peca.gameObject.GetComponent<Peca>().tipo = Tipos.getPecaJogador1();
            }
            foreach (Transform peca in pedrasVermelhas.transform)
            {
                peca.gameObject.GetComponent<Peca>().jogador = GameController.getJogador2();
                peca.gameObject.GetComponent<Peca>().tipo = Tipos.getPecaJogador2();
            }
        }

        private void preencheVazio()
        {
            int lin, col;
            int tamanho = getTamanhoTabuleiro();
            for (lin = 0; lin < tamanho; lin++)
            {
                for (col = 0; col < tamanho; col++)
                {
                    matrizTabuleiroInt[lin, col] = Tipos.vazio;
                }
            }
        }

        private void preenchePecasJogador1()
        {
            matrizTabuleiroInt[7, 1] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[7, 3] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[7, 5] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[7, 7] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[6, 0] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[6, 2] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[6, 4] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[6, 6] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[5, 1] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[5, 3] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[5, 5] = Tipos.getPecaJogador1();
            matrizTabuleiroInt[5, 7] = Tipos.getPecaJogador1();
        }

        private void preenchePecasJogador2()
        {
            matrizTabuleiroInt[0, 0] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[0, 2] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[0, 4] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[0, 6] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[1, 1] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[1, 3] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[1, 5] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[1, 7] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[2, 0] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[2, 2] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[2, 4] = Tipos.getPecaJogador2();
            matrizTabuleiroInt[2, 6] = Tipos.getPecaJogador2();
        }

        public void mostraTabuleiro()
        {
            int size = getTamanhoTabuleiro();
            string resp = "";
			Debug.Log("=====================");
            for (int lin = 0; lin < size; lin++)
            {
                for (int col = 0; col < size; col++)
                {
                    resp = resp + " " + matrizTabuleiroInt[lin, col];
                }
				Debug.Log(resp);
                resp = "";
            }
			Debug.Log("=====================");
        }

        public int getTamanhoTabuleiro(){
            return matrizTabuleiroInt.GetLength(0);
        }

        /*private List<GameObject> preenche_lista_posicoes(){
            int qtd_posicoes = posicoesAgrupamento.transform.childCount, i = 0;
            List<GameObject> posicoes_new = new List<GameObject> ();
            for (i = 0; i < qtd_posicoes; i++)
                posicoes_new.Add(posicoesAgrupamento.transform.GetChild(i).gameObject);

            return posicoes_new;
        }*/

    }
}
