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
        int dama = 2, pecaInimiga = 3, damaInimiga = 4;
        /*
        if (jogador == 1)
        {
            dama = 2;
            pecaInimiga = 3;
            damaInimiga = 4;
        }
        */
        if (jogador == 2)
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
        for(int i = x; i < 8; i++)
        {
            for(int j = y; j < 8; j++)
            {
                if (tabuleiro[i, j] == 0)
                    posicoes.Add(new List<int> { i, j });
            }
        }
        return posicoes;
    }
}

