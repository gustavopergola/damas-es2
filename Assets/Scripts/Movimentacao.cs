using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentacao : MonoBehaviour {

	public GameObject pedraSelecionada;
	public Transform transformInicialPedra;

	public LayerMask layerPosicao;
	public LayerMask layerPedras;

	public float timeToMove = 1f;
	public float speed = 1f;

	private Transform transformPedraEmMovimento;
	private Vector3 startPosition;
	private Vector3 finalPosition;
	private float timeSpent = 9999f;

    private GameController gameController;

    void Start () {
        gameController = gameObject.GetComponent<GameController>();
        pedraSelecionada = null;
	}
		
	void Update () {
		processaClique ();
	}

	void FixedUpdate(){
		controlaMovimento ();
	}

	private void processaClique(){
        if (Input.GetMouseButtonDown(0)) {
			if (!gameController.getTurnoJogador()) return; //turno IA
		
			GameObject objeto_resposta = checaClique ();
			if (objeto_resposta != null) {
				if (comparaLayerMaskValue (objeto_resposta.layer, this.layerPedras.value)) {
					seleciona_pedra (objeto_resposta);
				} else if (comparaLayerMaskValue (objeto_resposta.layer, this.layerPosicao.value)) {
					movimenta (this.pedraSelecionada, objeto_resposta);
					gameController.passarTurno();
				}
			} else {
				descelecionar_pedra_atual ();
			}

		}
	}

	private GameObject checaClique(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast (ray, out hit)) {
			Transform objectHit = hit.transform;
			//if (comparaLayerMaskValue(objectHit.gameObject.layer, layerPedras.value)){
				//TODO adicionar condição se for a peça do jogador do turno correto
			//}

			return objectHit.gameObject;
		}

		return null;
	}

	private void seleciona_pedra(GameObject pedraSelecionada){
		this.pedraSelecionada = pedraSelecionada;
		//TODO melhorar highlight de seleção da peça
		Vector3 aux = this.transformInicialPedra.transform.localScale;
		aux.x += 0.3f;
		aux.y += 0.3f;
		this.pedraSelecionada.transform.localScale = aux;
	}

	private void descelecionar_pedra_atual(){
		if (this.pedraSelecionada != null) {
			this.pedraSelecionada.transform.localScale = this.transformInicialPedra.localScale;
			this.pedraSelecionada = null;		
		}
	}

	private bool comparaLayerMaskValue(int layer, int layerMaskValue){
		return 1 << layer == layerMaskValue;
	}

	private void movimenta(GameObject go_pedra_selecionada, GameObject go_posicao_alvo){
		if (go_pedra_selecionada == null)
			return;
		
		this.startPosition = go_pedra_selecionada.transform.position;
		this.finalPosition = go_posicao_alvo.transform.position;
		this.transformPedraEmMovimento = go_pedra_selecionada.transform;
		this.timeSpent = 0f;
	}

	private void controlaMovimento(){
		if (this.timeSpent <= this.timeToMove) {
			this.timeSpent += Time.deltaTime / this.timeToMove;	
			Vector3 aux = Vector3.Lerp (this.startPosition, this.finalPosition, this.timeSpent * this.speed);
			this.transformPedraEmMovimento.position = new Vector3 (aux.x, aux.y, this.transformInicialPedra.position.z);
		}


	}
}
