using System.Collections;
using System.Collections.Generic;

public class Jogador {

    private string nome;
    private IA ia;
    
    public Jogador(string nome, IA ia = null){
        this.nome = nome;
        this.ia = ia;
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
