using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaquinaDeRegras : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<List<int>> Highlight(int[,] tabuleiro, int x, int y, int jogador)
    {
        // TODO função para checar se alguma peça qlq deve comer
        // comer é obrigatório
        int dama, pecaInimiga, damaInimiga;
        if (jogador == 1)
        {
            dama = 2;
            pecaInimiga = 3;
            damaInimiga = 4;
        }
        else
        {
            dama = 4;
            pecaInimiga = 1;
            damaInimiga = 2;
        }
        if (tabuleiro[x, y] == dama)
            return HighlightDamas(tabuleiro, x, y, pecaInimiga, damaInimiga);
        bool comer = false;
        List<List<int>> posicoes = new List<List<int>>();
        if (x + 2 < 8 && y + 2 < 8 && (tabuleiro[x + 1, y + 1] == pecaInimiga || tabuleiro[x + 1, y + 1] == damaInimiga))
        {
            posicoes.Add(new List<int> { x + 2, y + 2 });
            comer = true;
        }
        if (x - 2 >= 0 && y + 2 < 8 && (tabuleiro[x - 1, y + 1] == pecaInimiga || tabuleiro[x + 1, y + 1] == damaInimiga))
        {
            posicoes.Add(new List<int> { x - 2, y + 2 });
            comer = true;
        }
        if (x + 2 < 8 && y - 2 >= 0 && (tabuleiro[x + 1, y - 1] == pecaInimiga || tabuleiro[x + 1, y - 1] == damaInimiga)){
            posicoes.Add(new List<int> { x + 2, y - 2 });
            comer = true;
        }
        if (x - 2 < 8 && y - 2 >= 0 && (tabuleiro[x - 1, y - 1] == pecaInimiga || tabuleiro[x - 1, y - 1] == damaInimiga))
        {
            posicoes.Add(new List<int> { x - 2, y - 2 });
            comer = true;
        }
        if (comer == true)
            return posicoes;
        if (x + 1 < 8 && y + 1 < 8 && tabuleiro[x + 1, y + 1] == 0)
            posicoes.Add(new List<int> { x + 1, y + 1 });
        if (x - 1 >= 0 && y + 1 < 8 && tabuleiro[x - 1, y + 1] == 0)
            posicoes.Add(new List<int> { x - 1, y + 1 });
        return posicoes;
    }

    private List<List<int>> HighlightDamas(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga)
    {
        List<List<int>> posicoes = new List<List<int>>();
        int i, j;
        bool[] controle = {false, true};
        for(i = x, j = y; i < 8 && j < 8; i++, j++){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) 
                break;
        }
        for(i = x, j = y; i < 8 && j >= 0; i++, j--){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) 
                break;
        }
        for(i = x, j = y; i >= 0 && j < 8; i--, j++){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) 
                break;
        }
        for(i = x, j = y; i >= 0 && j >= 0; i--, j--){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) 
                break;
        }
        return posicoes;
    }

    private bool[] checkFor(List<List<int>> posicoes, int[,] tabuleiro, int i, int j, int pecaInimiga, int damaInimiga, bool comer){
        bool[] retorno = {comer, true};
        if (!retorno[0] && tabuleiro[i, j] == 0){
            posicoes.Add(new List<int> { i, j });
            return retorno;
        }
        else if(tabuleiro[i, j] == damaInimiga || tabuleiro[i, j] == pecaInimiga){
            retorno[0] = true;
            posicoes.Add(new List<int> { i, j });
            return retorno;
        }
        else{
            retorno[1] = false;
            return retorno;
        }
    }
}

