using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleTurnos : MonoBehaviour {

	public bool turnoJogador;
	public Text textIndicator;
    public Button passarTurnoBtn;

	// called before Start()
	void Awake (){
        textIndicator = GameObject.Find("Turno").GetComponent<Text>();
        passarTurnoBtn = GameObject.Find("PassarTurno").GetComponent<Button>();
		turnoJogador = false;
		passarTurno();
	}

	// Use this for initialization
	void Start () {
        passarTurnoBtn.onClick.AddListener(passarTurno);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void passarTurno(){
		Debug.Log ("Passou Turno");
		this.turnoJogador = !this.turnoJogador;
        if (this.turnoJogador)
            setTextoTurno("Turno: Jogador");
        else
        {
            setTextoTurno("Turno: IA");
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
        return turnoJogador;
    }
}
