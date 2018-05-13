using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public Jogador jogadorAtual;
    private Jogador jogador1;
    private Jogador jogador2;
	private Text textIndicator;
    private Button passarTurnoBtn;

	void Awake (){
        textIndicator = GameObject.Find("TurnoText").GetComponent<Text>();
        passarTurnoBtn = GameObject.Find("PassarTurno").GetComponent<Button>();
    }

	public void passarTurno(){
		Debug.Log ("Passou Turno");

        jogadorAtual = isJogadorAtual(jogador1) ? jogador2 : jogador1;

        if (this.jogadorAtual.isPlayer())
            setTextoTurno("Turno: Jogador");
        else
        {  
            setTextoTurno("Turno: IA " + jogadorAtual.getNomeJogador());
            //TODO get IA input here?
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
        return this.jogadorAtual.isPlayer();
    }

    public bool isJogadorAtual(Jogador jogador){
        jogador == this.jogadorAtual;
    }

    public void defineJogadores(Jogador jogador1, Jogador jogador2){        
        this.jogador1 = jogador1;
        this.jogador2 = jogador2;
        this.jogadorAtual = jogador1;
    }


}
