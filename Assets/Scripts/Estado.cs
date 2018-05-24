using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TiposNS;

public class Estado : MonoBehaviour {

	public int[,] board;
	public int currentPlayer;
	public int[] lastMove;
	public int playsCount;
	public int playsWithoutCapture;
	public bool gameOver = false;

	public Estado(int[,] board, int currentPlayer, int[] lastMove, int playsCount, int playsWithoutCapture = 0){
		this.board = (int[,])board.Clone();
		this.currentPlayer = currentPlayer;
		this.lastMove = lastMove;
		this.playsCount = playsCount;
		this.playsWithoutCapture = playsWithoutCapture;
	}

	public void print(){
		//Debug.Log("Current Player:"+currentPlayer);
		//Debug.Log("Board:");
		//string resp = "";
		for(int i=0; i<6; i++){
			for(int j=0; j<6; j++){
				//resp += board[i,j]+" ";
				//Console.Write($"{board[i,j]} "); ANTIGO
			}
			//Debug.Log(resp);
			//resp = "";
			//Console.Write("\n"); ANTIGO
		}
		if(lastMove != null){
			//Debug.Log("LastMove: ["+lastMove[0]+"]["+lastMove[1]+"] to ["+lastMove[2]+"]["+lastMove[3]+"]");
		}
	}

	private void registerGameIsOver(){
		int num_player1_pieces = 0;
		int num_player2_pieces = 0;

		foreach (int piece in this.board){
			if (Tipos.isJogador1(piece)) num_player1_pieces++;
			if (Tipos.isJogador2(piece)) num_player2_pieces++;

			//if((num_player1_pieces!=0) && (num_player2_pieces!=0)) break;	
		}

		if((num_player1_pieces==0) || (num_player2_pieces==0)){
			this.gameOver = true;
		}
		else{
			this.gameOver = false || this.checkDraw();
		}

	}

	public bool gameIsOver(){
		return this.gameOver;
	}

	public bool checkDraw(){
		return this.playsWithoutCapture >= 20;
	}

	public int getOpponent(){            
		if (this.currentPlayer == 1) return 2;
		else return 1;
	}

	public static Estado result(Estado oldState, int[] action){
		Estado newState = new Estado(oldState.board, oldState.currentPlayer, oldState.lastMove, oldState.playsCount, oldState.playsWithoutCapture);
		
		//TODO Concertar quando tiver a função na máquina de regras
		/*if(RuleMachine.isAttackMove(action, newState.board)){
			newState.playsWithoutCapture = 0;
		}else {
			newState.playsWithoutCapture++;
		}*/

		int piece = newState.board[action[0], action[1]];
		newState.board[action[0], action[1]] = Tipos.vazio;
		newState.board[action[2], action[3]] = piece;
		newState.currentPlayer = newState.currentPlayer == 1 ? 2 : 1;
		newState.lastMove = action;
		newState.playsCount++;
		
		newState.registerGameIsOver();

		return newState;
	}

}
