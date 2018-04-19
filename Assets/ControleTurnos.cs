using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleTurnos : MonoBehaviour {

	public bool turnoJogador;
	public Text textIndicator;

	// called before Start()
	void Awake (){
		turnoJogador = false;
		passarTurno ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void passarTurno(){
		Debug.Log ("Passou Turno");
		this.turnoJogador = !this.turnoJogador;
		if (this.turnoJogador)
			setTextoTurno ("Turno: Jogador");
		else
			setTextoTurno ("Turno: IA");
	}

	private void setTextoTurno(string new_texto){
		textIndicator.text = new_texto;
	}
}
