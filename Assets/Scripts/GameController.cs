using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public Jogador jogadorAtual;
    private Jogador jogador1;
    private Jogador jogador2;
	private Text textIndicator;
    private Button passarTurnoBtn;

    public const int GAME_MODE_PLAYER_VS_IA = 1;
    public const int GAME_MODE_IA_VS_IA = 2;

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

    public void switchScene(bool IAvsIA){
        if (IAvsIA) PlayerPrefs.SetInt("GameMode", GAME_MODE_IA_VS_IA);
        else PlayerPrefs.SetInt("GameMode", GAME_MODE_PLAYER_VS_IA);
        loadGameScene();
    }

    public void loadPlayervsIAGame(){
        Jogador player = new Jogador("Jogador"); 
        Jogador ia = new Jogador("IA 1", new IA());
        defineJogadores(player, ia);
    }

    public void loadIAvsIAGame(){
        Jogador ia1 = new Jogador("IA 1", new IA());
        Jogador ia2 = new Jogador("IA 2", new IA());
        defineJogadores(ia1, ia2);
    }
    
    private void loadGameScene(){
        SceneManager.LoadScene("initial", LoadSceneMode.Single);
    }

    private void defineJogadores(Jogador jogador1, Jogador jogador2){        
        this.jogador1 = jogador1;
        this.jogador2 = jogador2;
        this.jogadorAtual = jogador1;
    }

    private void loadGameMode(){
        int gameMode = PlayerPrefs.GetInt("GameMode");
        Debug.Log(gameMode);
        if (gameMode == GAME_MODE_PLAYER_VS_IA) loadPlayervsIAGame();
        else loadIAvsIAGame();
        setTextoTurno("Turno: " + jogadorAtual.getNomeJogador());
    }


}
