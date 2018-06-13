using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TabuleiroNS;
using TiposNS;
using MaquinaDeRegrasNS;
using EstadoNS;
public class Movimentacao : MonoBehaviour {

	public GameObject pedraSelecionada;
	public GameObject selectorParticleSystem;
	public GameObject highlightParticleSystem;
	public GameObject preFabXVermelho;
	private GameObject selectorParticleSystemAtual;
	public Transform transformInicialPedra;
	
	public LayerMask layerPosicao;

	public float timeToMove = 1f;
	public float speed = 1f;

	private Transform transformPedraEmMovimento;
	private Vector3 startPosition;
	private Vector3 finalPosition;
	private float timeSpent = 9999f;
	private bool clickFlag = false;
	private List<GameObject> listaHighlight;

    int jogo = 0;

    void Start () {
        pedraSelecionada = null;
		listaHighlight = new List<GameObject>();
	}
		
	void Update () {
		processaClique ();
	}

	void FixedUpdate(){
		controlaMovimento ();
	}

    public void test_result(){
        // int[,] tabuleiro = { 
		// 	{0,1,0,1,0,1,0,1},
        //     {1,0,1,0,1,0,1,0},
        //     {0,1,0,1,0,1,0,1},
        //     {0,0,0,0,0,0,0,0},
        //     {0,0,0,0,0,0,0,0},
        //     {3,0,3,0,3,0,3,0},
        //     {0,3,0,3,0,3,0,3},
        //     {3,0,3,0,3,0,3,0}
        // };

        // //Estado atual = new Estado(tabuleiro, 1, null);
        // //atual.print();

        // Jogada mock_acao = new Jogada();
        // mock_acao.posInicial = new int[] { 2, 1 };
        // mock_acao.movimentos.Add(new int[] { 3, 2 });
        // mock_acao.movimentos.Add(new int[] { 4, 1 });

        // //Estado novo = Estado.result(atual, mock_acao);
		// //novo.print();
    }

	private void processaClique(){
        if(jogo != 0)
        {
            return;
        }
        if (Input.GetMouseButtonUp(0) && !clickFlag) {
			clickFlag = true;
			if (!GameController.instance.getTurnoJogador()) return; //turno IA
			GameObject objeto_resposta = checaClique();
			if (objeto_resposta != null) {
				if (isPeca(objeto_resposta)) {
					seleciona_pedra (objeto_resposta);
					mostraHighlight (objeto_resposta);
				} else if (isPosicao(objeto_resposta) && this.pedraSelecionada) {
                    Jogada jogadaASerExecutada = null;
                    Peca pecaSelecionada = pedraSelecionada.GetComponent<Peca>();
                    List<int[]> posicoesPecasJogadorAtual = GameController.instance.posicoes_jogador_atual();

					// otimizar para chamar a máquina de regras uma vez apenas quando mudar o turno, pegando todos os movimentos possiveis do jogador atual
					List<List<Jogada>> jogadas = MaquinaDeRegras.PossiveisMovimentosUmJogador(
						GameController.instance.estadoAtual.tabuleiro,
						posicoesPecasJogadorAtual);
					foreach (List<Jogada> lista in jogadas)
					{
						// verifica se lista sendo avaliada neste momento é a lista de jogadas da peça que eu quero movimentar agora
						if(lista[0].posInicial[0] == pecaSelecionada.posicao.lin 
							&& lista[0].posInicial[1] == pecaSelecionada.posicao.col)
						{
							// se for a lista de jogadas da peça que eu quero mover tenho que achar a Jogada que tem como ultimo movimento
							// a posicao que quero mover a peca
							int linFinalAtual, colFinalAtual, linFinalDestino, colFinalDestino;
							foreach(Jogada jogada in lista) // as jogadas para encontrar qual é a jogada que quero fazer
							{
								linFinalAtual = jogada.ultimoMovimento()[0];
								colFinalAtual = jogada.ultimoMovimento()[1];
								Posicao posicaoDestino = objeto_resposta.GetComponent<Posicao>();
								linFinalDestino = posicaoDestino.lin;
								colFinalDestino = posicaoDestino.col;
								if (linFinalAtual == linFinalDestino && colFinalAtual == colFinalDestino)
								// Encontrando a jogada procurada temos que a jogada que queríamos fazer é válida, portando mudamos a variavel jogadaASerExecutada
								{
									jogadaASerExecutada = jogada;
                                    if (jogadaASerExecutada.pecasComidas.Count == 0 && Tipos.isDama(pecaSelecionada.tipo))
                                    {
                                        GameController.instance.estadoAtual.jogadasCondicaoEmpate++;
                                    }else
                                    {
                                        GameController.instance.estadoAtual.jogadasCondicaoEmpate = 0;
                                    }
								}
							}
							// TODO 
							//OK 1-verificar se movimentod que estou querendo fazer se encontra nessa lista de jogadas
							//OK 2.1-impedir movimentação caso não esteja nesta lista de jogadas
							//OK 2.2 - mostrar indicação visual de movimento inválido
							//OK 3-mostrar highlight no tabuleiro ==> Assim que selecionar uma peca mostrar o highlight
							//4-executar movimento visual seguindo as multiplas ações da Jogada
							//5-Acao usa result de Estado e não manipulsa estado atual manualmente
						}
					}
                    
                    if (jogadaASerExecutada != null)//se a jogada for valida posso movimentar, alterar matriz, passar turno e descelecionar e atualizar o estadoAtual
                    {
                        // executar movimento visual
                        if(jogadaASerExecutada.pecasComidas.Count == 0)
                        {
                            movimenta(this.pedraSelecionada, objeto_resposta);
                        }
                        else
                        {
                            comePecasGraphical(this.pedraSelecionada, jogadaASerExecutada);
                        }

						descelecionarPedraAtual();
                        GameController.instance.estadoAtual.tabuleiro = alteraMatriz(GameController.instance.estadoAtual.tabuleiro, jogadaASerExecutada);
                        GameController.instance.estadoAtual.ultimaJogada = jogadaASerExecutada; // VERIFICAR
                        GameController.instance.passarTurno();
                    }else {
						marcaXVermelhoNoTransform(objeto_resposta.transform);
					}
                    int jogo = GameController.instance.verificaVitoriaEmpate(GameController.instance.estadoAtual);
				}
			} else {
				descelecionarPedraAtual();
			}
			clickFlag = false;
		}
	}

    private void comePecasGraphical(GameObject pedraSelecionada, Jogada jogada)
    {
        StartCoroutine(comePecasGraphicalAsync(pedraSelecionada, jogada));
    }


    private IEnumerator comePecasGraphicalAsync(GameObject pedraSelecionada, Jogada jogada){
        int movimentosRealizados = 0;
        int[] posFinal;

        foreach (int[] peca in jogada.pecasComidas){
            Debug.Log("TESTE");
			GameObject posicao_go = Tabuleiro.instance.matrizTabuleiroPosicoes[peca[0], peca[1]];
			Posicao posicao_script = posicao_go.GetComponent<Posicao>();//posicao destino
			GameObject peca_go = posicao_script.peca;
			Peca peca_script = peca_go.GetComponent<Peca>();//peca do destino
			posicao_script.peca = null; // apaga referencia
			peca_script.destruir(); // apaga game object com fade out

            posFinal = jogada.movimentos[movimentosRealizados];
            GameObject posicaoFinalObj = Tabuleiro.instance.matrizTabuleiroPosicoes[posFinal[0], posFinal[1]];
            movimenta(pedraSelecionada, posicaoFinalObj);
            movimentosRealizados++;
            yield return new WaitForSeconds(0.5f);
        }
	}

    private void marcaXVermelhoNoTransform(Transform transformTarget){
		Instantiate(this.preFabXVermelho, transformTarget); // Mostra X de jogada inválida
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
		if (this.pedraSelecionada != null) descelecionarPedraAtual();

		this.pedraSelecionada = pedraSelecionada;
		this.selectorParticleSystemAtual = Instantiate(this.selectorParticleSystem, this.pedraSelecionada.transform) as GameObject;
		this.selectorParticleSystemAtual.transform.parent = this.pedraSelecionada.transform;
	}

	private void descelecionarPedraAtual(){
		if (this.pedraSelecionada != null) {
			apagaHighlight();
			this.pedraSelecionada = null;
			if (this.selectorParticleSystemAtual != null)
				Destroy(this.selectorParticleSystemAtual);
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

	private int[,] alteraMatriz(int[,] matrizTabuleiroInt, Jogada jogada){
        int linInicio = jogada.posInicial[0];
        int colInicio = jogada.posInicial[1];
        int[] ultimoMovimento = jogada.ultimoMovimento();
        int linFim = ultimoMovimento[0];
        int colFim = ultimoMovimento[1];

        Posicao posInicio = Tabuleiro.instance.matrizTabuleiroPosicoes[linInicio, colInicio].GetComponent<Posicao>();
        GameObject pecaSelecionada = posInicio.peca;
        Peca _pecaSelecionada = pecaSelecionada.GetComponent<Peca>();
        
        Posicao posFim = Tabuleiro.instance.matrizTabuleiroPosicoes[linFim, colFim].GetComponent<Posicao>();

		//Atualiza matriz de inteiros
		matrizTabuleiroInt[linInicio, colInicio] = Tipos.vazio;
		matrizTabuleiroInt[linFim, colFim] = _pecaSelecionada.tipo;
		//atualiza objetos
		posInicio.peca = null;
		posFim.peca = pecaSelecionada;
        _pecaSelecionada.posicao = posFim;

        for(int i=0; i < jogada.pecasComidas.Count; i++)
        {
            int linComida = jogada.pecasComidas[i][0];
            int colComida = jogada.pecasComidas[i][1];

            matrizTabuleiroInt[linComida, colComida] = Tipos.vazio;
        }

		if(jogada.virouDama){
            int jogador = Tipos.jogador(_pecaSelecionada.tipo);
            _pecaSelecionada.tipo = Tipos.getPecaJogadorX(Tipos.dama, jogador);
            matrizTabuleiroInt[linFim, colFim] = _pecaSelecionada.tipo;
        }

        return matrizTabuleiroInt;
	}
	private void controlaMovimento(){
		if (this.timeSpent <= this.timeToMove) {
			this.timeSpent += Time.deltaTime / this.timeToMove;	
			Vector3 aux = Vector3.Lerp (this.startPosition, this.finalPosition, this.timeSpent * this.speed);
			this.transformPedraEmMovimento.position = new Vector3 (aux.x, aux.y, this.transformInicialPedra.position.z);
		}
	}

	private void mostraHighlight(GameObject peca_go){
		List<int[]> posicoesPecasJogadorAtual = GameController.instance.posicoes_jogador_atual();
		List<List<Jogada>> jogadas = MaquinaDeRegras.PossiveisMovimentosUmJogador(
						GameController.instance.estadoAtual.tabuleiro,
						posicoesPecasJogadorAtual);
		Peca pecaSelecionada = peca_go.GetComponent<Peca>();

		foreach(List<Jogada> jogadas_peca in jogadas){
			// verifica se lista sendo avaliada neste momento é a lista de jogadas da peça que eu quero movimentar agora
			if(jogadas_peca[0].posInicial[0] == pecaSelecionada.posicao.lin 
				&& jogadas_peca[0].posInicial[1] == pecaSelecionada.posicao.col)
			{
				int linFinalAtual, colFinalAtual, linFinalDestino, colFinalDestino;
				foreach(Jogada jogada in jogadas_peca)
				{
					linFinalAtual = jogada.ultimoMovimento()[0];
					colFinalAtual = jogada.ultimoMovimento()[1];
					GameObject posicao = Tabuleiro.instance.matrizTabuleiroPosicoes[linFinalAtual, colFinalAtual];
					GameObject new_highlight = Instantiate(highlightParticleSystem, posicao.transform) as GameObject;
					this.listaHighlight.Add(new_highlight);
				}
			}
		}
	}

	private void apagaHighlight(){
		foreach(GameObject highlight in listaHighlight)
			Destroy(highlight);
		
		this.listaHighlight = new List<GameObject>();
	}
}
