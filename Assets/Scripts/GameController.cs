using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EstadoNS;
using TabuleiroNS;

public class GameController : MonoBehaviour {


    public static GameController instance { get; private set; }
    public Estado estadoAtual;
    public Jogador jogadorAtual; 

    private static Jogador jogador1;
    private static Jogador jogador2;

    public LayerMask layerJogador1;
    public LayerMask layerJogador2;

	private Text textIndicator;
    private Button passarTurnoBtn;

    public const int GAME_MODE_PLAYER_VS_IA = 1;
    public const int GAME_MODE_IA_VS_IA = 2;
    public const int GAME_MODE_PLAYER_VS_PLAYER = 3;

    private static bool created = false;

	void Awake (){
        instance = this;
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

    private void Start()
    {
        // inicializando estado
        estadoAtual = new Estado(Tabuleiro.instance.matrizTabuleiroInt, -1, null);
    }

    public void passarTurno(){
        setJogadorAtual(isJogadorAtual(jogador1) ? jogador2 : jogador1);

        setTextoTurno("Turno: " + jogadorAtual.getNomeJogador());

        if (jogadorAtual.isIA()){
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
        if (jogadorAtual == null) return false;
        return jogadorAtual.isPlayer();
    }

    public bool isJogadorAtual(Jogador jogador){
        return jogador == jogadorAtual;
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

        jogador1.layerMaskValue = layerJogador1.value;

        jogador2.layerMaskValue = layerJogador2.value;

        setJogadorAtual(jogador1);
    }

    private void loadGameMode(){
        int gameMode = PlayerPrefs.GetInt("GameMode");
        if (gameMode == GAME_MODE_PLAYER_VS_IA) loadPlayervsIAGame();
        else if (gameMode == GAME_MODE_IA_VS_IA) loadIAvsIAGame();
        else loadPlayervsPlayerGame();        
        setTextoTurno("Turno: " + jogadorAtual.getNomeJogador());
    }

    public Jogador getJogador1(){
        return jogador1;
    }
    public Jogador getJogador2(){
        return jogador2;
    }

    public void setJogadorAtual(Jogador novoJogador)
    {
        this.jogadorAtual = novoJogador;
        this.estadoAtual.setJogadorAtual(novoJogador.getNumeroJogador());
    }

}
