using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiposNS;

namespace EstadoNS{
    public class Estado{

        public int[,] tabuleiro;
        public Jogador jogadorAtual;
	    public Jogada ultimaJogada;
	    public bool gameOver = false;

	    public Estado(int[,] tabuleiro, Jogador jogadorAtual, Jogada ultimaJogada){
            this.tabuleiro = (int[,])tabuleiro.Clone();
            this.jogadorAtual = jogadorAtual;
            this.ultimaJogada = ultimaJogada;
	    }

	    public void print(){
            string resp = "";

            resp += ("Jogador Atual:"+jogadorAtual+"\n");
		    resp += ("Tabuleiro:\n");
		    for(int i=0; i<8; i++){
		    	for(int j=0; j<8; j++){
		    		resp += tabuleiro[i,j]+" ";
		    		//Debug.Log(tabuleiro[i,j]+" ");
		    	}
		    	resp += "\n";
		    }

            Debug.Log(resp);

            //if (ultimaJogada != null){
	        //	Debug.Log("Ultima jogada: ["+ultimaJogada[0]+"]["+ultimaJogada[1]+"] to ["+lastMove[2]+"]["+lastMove[3]+"]");
		    //}
	    }

        public bool gameIsOver(){
		    return this.gameOver;
	    }

	    public int getOpponent(){            
		    if (this.jogadorAtual.getNumeroJogador() == 1) return 2;
		    else return 1;
	    }

	    public static Estado result(Estado antigo, Jogada acao){
		    Estado novo = new Estado(antigo.tabuleiro, antigo.jogadorAtual, antigo.ultimaJogada);

            novo.jogadorAtual = GameController.instance.getOponente(novo.jogadorAtual.getNumeroJogador());
            novo.ultimaJogada = acao;

            int size = acao.movimentos.Count;

            novo.tabuleiro[acao.movimentos[size-1][0], acao.movimentos[size-1][1]] = novo.tabuleiro[acao.posInicial[0], acao.posInicial[1]];
            novo.tabuleiro[acao.posInicial[0], acao.posInicial[1]] = 0;

            foreach (var peca in acao.pecasComidas){
                novo.tabuleiro[peca[0], peca[1]] = 0;
            }
    	
		    return novo;
	    }

    }
}