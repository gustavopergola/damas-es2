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
            return HighlightDamas(tabuleiro, x, y, dama, pecaInimiga, damaInimiga);
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

    private List<List<int>> HighlightDamas(int[,] tabuleiro, int x, int y, int dama, int pecaInimiga, int damaInimiga)
    {
        // TODO verificação comer
        List<List<int>> posicoes = new List<List<int>>();
        int i, j;
        for(i = x, j = y; i < 8 && j < 8; i++, j++){
            if (tabuleiro[i, j] == 0)
                posicoes.Add(new List<int> { i, j });
            else break;
        }
        for(i = x, j = y; i < 8 && j >= 0; i++, j--){
            if (tabuleiro[i, j] == 0)
                posicoes.Add(new List<int> { i, j });
            else break;
        }
        for(i = x, j = y; i >= 0 && j < 8; i--, j++){
            if (tabuleiro[i, j] == 0)
                posicoes.Add(new List<int> { i, j });
            else break;
        }
        for(i = x, j = y; i >= 0 && j >= 0; i--, j--){
            if (tabuleiro[i, j] == 0)
                posicoes.Add(new List<int> { i, j });
            else break;
        }
        return posicoes;
    }
}

