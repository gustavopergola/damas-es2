using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TabuleiroNS;
using TiposNS;
using EstadoNS;

public class Movimentacao : MonoBehaviour {

	public GameObject pedraSelecionada;
	public GameObject selectorParticleSystem;
	private GameObject selectorParticleSystemAtual;
	public Transform transformInicialPedra;

	public LayerMask layerPosicao;

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

        test_result();
	}

	void FixedUpdate(){
		controlaMovimento ();
	}

    public void test_result(){
        int[,] tabuleiro = { {0,1,0,1,0,1,0,1},
                             {1,0,1,0,1,0,1,0},
                             {0,1,0,1,0,1,0,1},
                             {0,0,0,0,0,0,0,0},
                             {0,0,0,0,0,0,0,0},
                             {3,0,3,0,3,0,3,0},
                             {0,3,0,3,0,3,0,3},
                             {3,0,3,0,3,0,3,0}
                           };


        Estado atual = new Estado(tabuleiro, 1, null);
        atual.print();


        Jogada acao = new Jogada();
        acao.posInicial = new int[] { 2, 1 };
        acao.movimentos.Add(new int[] { 3, 2 });
        acao.movimentos.Add(new int[] { 4, 1 });


        Estado novo = Estado.result(atual, acao);
        novo.print();
    }

	private void processaClique(){
        if (Input.GetMouseButtonDown(0)) {
			if (!gameController.getTurnoJogador()) return; //turno IA
			GameObject objeto_resposta = checaClique();
			if (objeto_resposta != null) {
				if (isPeca(objeto_resposta)) {
					seleciona_pedra (objeto_resposta);
				} else if (isPosicao(objeto_resposta) && this.pedraSelecionada) {
					movimenta (this.pedraSelecionada, objeto_resposta);
					gameController.passarTurno();
					descelecionar_pedra_atual();
				}
			} else {
				descelecionar_pedra_atual();
			}

		}
	}

	private bool isPeca(GameObject objeto){
		return (
			comparaLayerMaskValue (objeto.layer, this.gameController.layerJogador1.value) ||
			comparaLayerMaskValue (objeto.layer, this.gameController.layerJogador2.value)
		);
	}

	private bool isPecaJogadorAtual(GameObject objeto){
		return comparaLayerMaskValue(objeto.layer, this.gameController.jogadorAtual.layerMaskValue);
	}

	private bool isPosicao(GameObject objeto){
		return comparaLayerMaskValue (objeto.layer, this.layerPosicao.value);
	}

	private GameObject checaClique(){
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast (ray, out hit)) {
			GameObject objHit = hit.transform.gameObject;
			if (isPosicao(objHit) || isPecaJogadorAtual(objHit)) return objHit; else return null;
		}
		
		return null;
	}

	private void seleciona_pedra(GameObject pedraSelecionada){
		if (this.pedraSelecionada != null) descelecionar_pedra_atual();

		this.pedraSelecionada = pedraSelecionada;
		this.selectorParticleSystemAtual = Instantiate(this.selectorParticleSystem, this.pedraSelecionada.transform) as GameObject;
		this.selectorParticleSystemAtual.transform.parent = this.pedraSelecionada.transform;
	}

	private void descelecionar_pedra_atual(){
		if (this.pedraSelecionada != null) {
			this.pedraSelecionada.transform.localScale = this.transformInicialPedra.localScale;
			this.pedraSelecionada = null;
			if (this.selectorParticleSystemAtual != null) Destroy(this.selectorParticleSystemAtual);
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
		//realiza mudanças no tabuleiro
		alteraMatriz(go_pedra_selecionada, go_posicao_alvo);
	}

	private void alteraMatriz(GameObject pedra_selecionada, GameObject posicao_alvo){
		Peca pecaSelecionada = pedra_selecionada.GetComponent<Peca>();
		Posicao posicaoAlvo = posicao_alvo.GetComponent<Posicao>();
		int linInicio = pecaSelecionada.posicao.lin;
		int colInicio = pecaSelecionada.posicao.col;
		int linFim = posicaoAlvo.lin;
		int colFim = posicaoAlvo.col;
		bool ataque = false;//TODO receber resultado da máquina de regras para determinar se movimento é ataque ou não
		//TODO RECEBER POSICAO DA PECA QUE FOI CAPTURADA
		Posicao posInicio = Tabuleiro.instance.matrizTabuleiroPosicoes[linInicio, colInicio].GetComponent<Posicao>();
		Posicao posFim = Tabuleiro.instance.matrizTabuleiroPosicoes[linFim, colFim].GetComponent<Posicao>();
		if(!ataque){
			//MOVIMENTACAO SIMPLES
			//Atualiza matriz de inteiros
			Tabuleiro.instance.matrizTabuleiroInt[linInicio, colInicio] = Tipos.vazio;
			Tabuleiro.instance.matrizTabuleiroInt[linFim, colFim] = pecaSelecionada.tipo;
			//atualiza objetos
			posInicio.peca = null;
			posFim.peca = pedra_selecionada;
			pecaSelecionada.posicao = posicaoAlvo;
		}else{
			//MOVIMENTO DE ATAQUE
			//TODO alterar matriz em caso de movimento de ataque
		}
		bool dama = false; //TODO receber resultado da máquina de regras para determinar se virou dama ou não
		if(dama){
			//TODO verificar se virou dama
		}
		
	}

	private void controlaMovimento(){
		if (this.timeSpent <= this.timeToMove) {
			this.timeSpent += Time.deltaTime / this.timeToMove;	
			Vector3 aux = Vector3.Lerp (this.startPosition, this.finalPosition, this.timeSpent * this.speed);
			this.transformPedraEmMovimento.position = new Vector3 (aux.x, aux.y, this.transformInicialPedra.position.z);
		}
	}
}
