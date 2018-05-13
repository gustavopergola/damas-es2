using System.Collections;
using System.Collections.Generic;

public class Jogador {

    private string nome;
    private bool is_player;
    

    public bool isPlayer(){
        return this.is_player;
        //TODO substituir por return !ia;
    }

    public string getNomeJogador(){
        return this.nome;
    }

    public void callAIAction(){
        //TODO ia.CallAction();
    }

}
