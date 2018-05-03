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
        List<List<int>> posicoes = new List<List<int>>();
        posicoes = LeiDaMaioria(tabuleiro, x, y, pecaInimiga, damaInimiga);
        if (posicoes.Count > 0)
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
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i < 8 && j >= 0; i++, j--){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i >= 0 && j < 8; i--, j++){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i >= 0 && j >= 0; i--, j--){
            controle = checkFor(posicoes, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        return posicoes;
    }

    private bool[] checkFor(List<List<int>> posicoes, int[,] tabuleiro, int i, int j, int pecaInimiga, int damaInimiga, bool comer){
        bool[] retorno = {comer, true};
        if (!comer && tabuleiro[i, j] == 0){
            posicoes.Add(new List<int> { i, j });
            return retorno;
        }
        else if(tabuleiro[i, j] == damaInimiga || tabuleiro[i, j] == pecaInimiga){
            retorno[0] = true; // comer = true
            // chamar LeiDaMaioria aqui??
            posicoes.Add(new List<int> { i, j });
            return retorno;
        }
        else{
            retorno[1] = false; // não há movimentação
            return retorno;
        }
    }

    private List<List<int>> LeiDaMaioria(int [,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga) {
        List<List<int>> pos1 = new List<List<int>>();
        List<List<int>> pos2 = new List<List<int>>();
        List<List<int>> pos3 = new List<List<int>>();
        List<List<int>> pos4 = new List<List<int>>();
        List<List<int>> posAux = new List<List<int>>();
        if (x + 2 < 8 && y + 2 < 8 && (tabuleiro[x + 1, y + 1] == pecaInimiga || tabuleiro[x + 1, y + 1] == damaInimiga))
        {
            pos1.Add(new List<int> { x + 2, y + 2 });
            posAux = LeiDaMaioria(tabuleiro, x+2, y+2, pecaInimiga, damaInimiga);
            if(posAux.Count != 0){
                foreach (List<int> pos in posAux)
                {
                    pos1.Add(pos);
                }
            }
        }
        if (x - 2 >= 0 && y + 2 < 8 && (tabuleiro[x - 1, y + 1] == pecaInimiga || tabuleiro[x + 1, y + 1] == damaInimiga))
        {
            pos2.Add(new List<int> { x - 2, y + 2 });
            posAux = LeiDaMaioria(tabuleiro, x-2, y+2, pecaInimiga, damaInimiga);
            if(posAux.Count != 0){
                foreach (List<int> pos in posAux)
                {
                    pos2.Add(pos);
                }
            }
        }
        if (x + 2 < 8 && y - 2 >= 0 && (tabuleiro[x + 1, y - 1] == pecaInimiga || tabuleiro[x + 1, y - 1] == damaInimiga)){
            pos3.Add(new List<int> { x + 2, y - 2 });
            posAux = LeiDaMaioria(tabuleiro, x+2, y-2, pecaInimiga, damaInimiga);
            if(posAux.Count != 0){
                foreach (List<int> pos in posAux)
                {
                    pos3.Add(pos);
                }
            }
        }
        if (x - 2 < 8 && y - 2 >= 0 && (tabuleiro[x - 1, y - 1] == pecaInimiga || tabuleiro[x - 1, y - 1] == damaInimiga))
        {
            pos4.Add(new List<int> { x - 2, y - 2 });
            posAux = LeiDaMaioria(tabuleiro, x-2, y-2, pecaInimiga, damaInimiga);
            if(posAux.Count != 0){
                foreach (List<int> pos in posAux)
                {
                    pos4.Add(pos);
                }
            }
        }
        return max(pos1, pos2, pos3, pos4);
    }

    private List<List<int>> max(List<List<int>> lista1, List<List<int>> lista2, List<List<int>> lista3, List<List<int>> lista4) {
        if(lista1.Count >= lista2.Count && lista1.Count >= lista3.Count && lista1.Count >= lista4.Count)
            return lista1;
        if(lista2.Count >= lista1.Count && lista2.Count >= lista3.Count && lista2.Count >= lista4.Count)
            return lista2;
        if(lista3.Count >= lista1.Count && lista3.Count >= lista2.Count && lista3.Count >= lista4.Count)
            return lista3;
        else return lista4;
    }
}

