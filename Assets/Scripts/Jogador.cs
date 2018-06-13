using System.Collections;
using System.Collections.Generic;
using IANS;
using EstadoNS;

public class Jogador {

    public int layerMaskValue;
    private string nome;
    private int numero;
    private IA ia;

    private static int jogadores = 1;
    
    public Jogador(string nome, IA ia = null, int layerMaskValue = 999){
        this.nome = nome;
        this.numero = jogadores;
        this.ia = ia;
        this.layerMaskValue = layerMaskValue;
        jogadores = 2;//máximo de 2 jogadores
    }

    public bool isPlayer(){
        return this.ia == null;
    }

    public bool isIA(){
        return !this.isPlayer();
    }

    public string getNomeJogador(){
        return this.nome;
    }

    public Jogada callAIAction(Estado estado){
        return this.ia.alpha_beta_search(estado);
    }
    public int getNumeroJogador(){
        return this.numero;
    }
}
