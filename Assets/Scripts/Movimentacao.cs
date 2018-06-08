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

    void Start () {
        pedraSelecionada = null;
	}
		
	void Update () {
		processaClique ();
	}

	void FixedUpdate(){
		controlaMovimento ();
	}

    public void test_result(){
        int[,] tabuleiro = { 
			{0,1,0,1,0,1,0,1},
            {1,0,1,0,1,0,1,0},
            {0,1,0,1,0,1,0,1},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {3,0,3,0,3,0,3,0},
            {0,3,0,3,0,3,0,3},
            {3,0,3,0,3,0,3,0}
        };

        //Estado atual = new Estado(tabuleiro, 1, null);
        //atual.print();

        Jogada mock_acao = new Jogada();
        mock_acao.posInicial = new int[] { 2, 1 };
        mock_acao.movimentos.Add(new int[] { 3, 2 });
        mock_acao.movimentos.Add(new int[] { 4, 1 });

        //Estado novo = Estado.result(atual, mock_acao);
		//novo.print();
    }

	private void processaClique(){
		test_result();
        if (Input.GetMouseButtonDown(0)) {
			if (!GameController.instance.getTurnoJogador()) return; //turno IA
			GameObject objeto_resposta = checaClique();
			if (objeto_resposta != null) {
				if (isPeca(objeto_resposta)) {
					seleciona_pedra (objeto_resposta);
				} else if (isPosicao(objeto_resposta) && this.pedraSelecionada) {
					movimenta (this.pedraSelecionada, objeto_resposta);
                    GameController.instance.passarTurno();
					descelecionar_pedra_atual();
				}
			} else {
				descelecionar_pedra_atual();
			}

		}
	}

	private bool isPeca(GameObject objeto){
		return (
			comparaLayerMaskValue (objeto.layer, GameController.instance.layerJogador1.value) ||
			comparaLayerMaskValue (objeto.layer, GameController.instance.layerJogador2.value)
		);
	}

	private bool isPecaJogadorAtual(GameObject objeto){
		return comparaLayerMaskValue(objeto.layer, GameController.instance.jogadorAtual.layerMaskValue);
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
	}

	private void alteraMatriz(Jogada jogada){
        int linInicio = jogada.posInicial[0];
        int colInicio = jogada.posInicial[1];
        int[] ultimoMovimento = jogada.ultimoMovimento();
        int linFim = ultimoMovimento[0];
        int colFim = ultimoMovimento[1];

        GameObject pecaSelecionada = Tabuleiro.instance.matrizTabuleiroPosicoes[linInicio, colInicio].GetComponent<Posicao>().peca;
        Peca _pecaSelecionada = Tabuleiro.instance.matrizTabuleiroPosicoes[linInicio, colInicio].GetComponent<Posicao>().peca.GetComponent<Peca>();

        Posicao posInicio = Tabuleiro.instance.matrizTabuleiroPosicoes[linInicio, colInicio].GetComponent<Posicao>();
        Posicao posFim = Tabuleiro.instance.matrizTabuleiroPosicoes[linFim, colFim].GetComponent<Posicao>();

		//Atualiza matriz de inteiros
		Tabuleiro.instance.matrizTabuleiroInt[linInicio, colInicio] = Tipos.vazio;
		Tabuleiro.instance.matrizTabuleiroInt[linFim, colFim] = _pecaSelecionada.tipo;
		//atualiza objetos
		posInicio.peca = null;
		posFim.peca = pecaSelecionada;
        _pecaSelecionada.posicao = posFim;

        bool ataque = jogada.pecasComidas.Count > 0 ? true : false;
        if (ataque)
        {
            for(int i=0; i < jogada.pecasComidas.Count; i++)
            {
                int linComida = jogada.pecasComidas[i][0];
                int colComida = jogada.pecasComidas[i][1];

                Tabuleiro.instance.matrizTabuleiroInt[linComida, colComida] = Tipos.vazio;
                Tabuleiro.instance.matrizTabuleiroPosicoes[linComida, colComida].GetComponent<Posicao>().peca = null;
                //objeto da peça não é modificado pois ele será deletado
            }
        }

		if(jogada.virouDama){
            int jogador = Tipos.jogador(_pecaSelecionada.tipo);
            _pecaSelecionada.tipo = Tipos.getPecaJogadorX(Tipos.dama, jogador);
            Tabuleiro.instance.matrizTabuleiroInt[linFim, colFim] = _pecaSelecionada.tipo;
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
