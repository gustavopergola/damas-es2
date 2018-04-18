using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour {

	public GameObject pedraSelecionada;
	public Transform transformInicialPedra;

	public LayerMask layerPedras;

	// Use this for initialization
	void Start () {
		pedraSelecionada = null;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject pedraClicada = checaCliqueEmPedra ();
		if (pedraClicada != null) {
			descelecionar_pedra_atual ();
			seleciona_pedra (pedraClicada);
		}
			
	}

	private GameObject checaCliqueEmPedra(){
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				Transform objectHit = hit.transform;
				if (1<<objectHit.gameObject.layer == layerPedras.value){
					//TODO adicionar condição se for a peça do jogador do turno correto
					return objectHit.gameObject;
				}
			}
		}

		return null;
	}

	private void seleciona_pedra(GameObject pedraSelecionada){
		this.pedraSelecionada = pedraSelecionada;
		//TODO melhorar highlight de seleção da peça
		Vector3 aux = new Vector3(0.33f, 0.33f, 1);
		pedraSelecionada.transform.localScale = aux;
	}

	private void descelecionar_pedra_atual(){
		if (this.pedraSelecionada != null) {
			this.pedraSelecionada.transform.localScale = this.transformInicialPedra.localScale;
			this.pedraSelecionada = null;		
		}
	}
}
