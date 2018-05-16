﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public Jogador jogadorAtual;
    private static Jogador jogador1;
    private static Jogador jogador2;
	private Text textIndicator;
    private Button passarTurnoBtn;

    public const int GAME_MODE_PLAYER_VS_IA = 1;
    public const int GAME_MODE_IA_VS_IA = 2;
    public const int GAME_MODE_PLAYER_VS_PLAYER = 3;

    private static bool created = false;

	void Awake (){
        
        if (!created){
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }else {
            // vai entrar aqui quando for uma scene diferente do menu inicial
            textIndicator = GameObject.Find("TurnoText").GetComponent<Text>();
            passarTurnoBtn = GameObject.Find("PassarTurno").GetComponent<Button>();
            loadGameMode();
        }
    }

	public void passarTurno(){
        jogadorAtual = isJogadorAtual(jogador1) ? jogador2 : jogador1;

        setTextoTurno("Turno: " + jogadorAtual.getNomeJogador());

        if (this.jogadorAtual.isIA()){
            jogadorAtual.callAIAction();
            disablePassarTurnoBtn();
        }
    }

    public void disablePassarTurnoBtn()
    {
        passarTurnoBtn.interactable = false;
    }

	private void setTextoTurno(string new_texto){
		textIndicator.text = new_texto;
	}

    public bool getTurnoJogador()
    {
        if (this.jogadorAtual == null) return false;
        return this.jogadorAtual.isPlayer();
    }

    public bool isJogadorAtual(Jogador jogador){
        return jogador == this.jogadorAtual;
    }

    public void switchScene(int gameMode){
        PlayerPrefs.SetInt("GameMode", gameMode);
        loadGameScene();
    }

    private void loadPlayervsPlayerGame(){
        Jogador player1 = new Jogador("Jogador1"); 
        Jogador player2 = new Jogador("Jogador2"); 
        defineJogadores(player1, player2);
    }

    private void loadPlayervsIAGame(){
        Jogador player = new Jogador("Jogador"); 
        Jogador ia = new Jogador("IA 1", new IA());
        defineJogadores(player, ia);
    }

    private void loadIAvsIAGame(){
        Jogador ia1 = new Jogador("IA 1", new IA());
        Jogador ia2 = new Jogador("IA 2", new IA());
        defineJogadores(ia1, ia2);
    }
    
    private void loadGameScene(){
        SceneManager.LoadScene("initial", LoadSceneMode.Single);
    }

    private void defineJogadores(Jogador player1, Jogador player2){        
        jogador1 = player1;
        jogador2 = player2;
        this.jogadorAtual = jogador1;
    }

    private void loadGameMode(){
        int gameMode = PlayerPrefs.GetInt("GameMode");
        Debug.Log("GameMode loaded = " + gameMode);
        if (gameMode == GAME_MODE_PLAYER_VS_IA) loadPlayervsIAGame();
        else if (gameMode == GAME_MODE_IA_VS_IA) loadIAvsIAGame();
        else loadPlayervsPlayerGame();
        Debug.Log("Turno: " + jogadorAtual.getNomeJogador());
        setTextoTurno("Turno: " + jogadorAtual.getNomeJogador());
    }

    public static Jogador getJogador1(){
        return jogador1;
    }
    public static Jogador getJogador2(){
        return jogador2;
    }


}
