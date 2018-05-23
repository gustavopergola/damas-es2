using System.Collections;
using System.Collections.Generic;

public class Jogador {

    public int layerMaskValue;
    private string nome;
    private IA ia;
    
    public Jogador(string nome, IA ia = null, int layerMaskValue = 999){
        this.nome = nome;
        this.ia = ia;
        this.layerMaskValue = layerMaskValue;
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

    public void callAIAction(){
        this.ia.getAction();
    }
}
