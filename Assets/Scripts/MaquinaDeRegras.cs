using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TiposNS;

public class MaquinaDeRegras : MonoBehaviour
{
    
    private int[] SetValoresPecas(int jogador)
    {
        int[] valoresPecas = new int[3];
        if (jogador == 1)
        {
            valoresPecas[0] = Tipos.getDamaJogador1();
            valoresPecas[1] = Tipos.getPecaJogador2();
            valoresPecas[2] = Tipos.getDamaJogador2();
        }
        else
        {
            valoresPecas[0] = Tipos.getDamaJogador2();
            valoresPecas[1] = Tipos.getPecaJogador1();
            valoresPecas[2] = Tipos.getDamaJogador1();
        }
        return valoresPecas;
    }

    public List<List<int>> HasToComer(int[][] tabuleiro, List<List<int>> posPecas, int jogador)
    {
        List<List<int>> pecasQueComem = new List<List<int>>();
        int[] valoresPecas = SetValoresPecas(jogador);
        int x, y;
        foreach (List<int> peca in posPecas)
        {
            x = peca[0];
            y = peca[1];
            if((x + 1 < 8 && y + 1 < 8) && (tabuleiro[x+1][y+1] == valoresPecas[1] || tabuleiro[x+1][y+1] == valoresPecas[2]))
            {
                pecasQueComem.Add(new List<int>{x,y});
            }
            if((x + 1 < 8 && y - 1 >= 0) && (tabuleiro[x+1][y-1] == valoresPecas[1] || tabuleiro[x+1][y-1] == valoresPecas[2]))
            {
                pecasQueComem.Add(new List<int>{x,y});
            }
            if((x - 1 >= 0 && y + 1 < 8) && (tabuleiro[x-1][y+1] == valoresPecas[1] || tabuleiro[x-1][y+1] == valoresPecas[2]))
            {
                pecasQueComem.Add(new List<int>{x,y});
            }
            if((x - 1 >= 0 && y - 1 >= 0) && (tabuleiro[x-1][y-1] == valoresPecas[1] || tabuleiro[x-1][y-1] == valoresPecas[2]))
            {
                pecasQueComem.Add(new List<int>{x,y});
            }
        }
        return pecasQueComem;
    }

    public List<List<int>> PossiveisMovimentos(int[,] tabuleiro, int jogador, List<List<int>> posPecas)
    {
        int[] valoresPecas = SetValoresPecas(jogador);
        int dama = valoresPecas[0], pecaInimiga = valoresPecas[1], damaInimiga = valoresPecas[2];
        List<List<int>> possiveisMovimentos = new List<List<int>>();
        List<List<int>> movimentosAuxiliares = new List<List<int>>();
        bool comer = false;
        foreach (List<int> peca in posPecas)
        {
            movimentosAuxiliares = Highlight(tabuleiro, peca[0], peca[1], jogador, dama, pecaInimiga, damaInimiga);
            
            if (HaveComido(tabuleiro,movimentosAuxiliares[0], pecaInimiga, damaInimiga))
            {
                comer = true;
                possiveisMovimentos = new List<List<int>>();
            }
            if (comer == true)
            {
                if (movimentosAuxiliares.Count > possiveisMovimentos.Count)
                    possiveisMovimentos = movimentosAuxiliares;
            }
            else {
                foreach (List<int> pecaAuxiliar in movimentosAuxiliares)
                    possiveisMovimentos.Add(pecaAuxiliar);
            }
        }
        return possiveisMovimentos;
    }

    private bool HaveComido(int [,] tabuleiro, List<int> posicao, int pecaInimiga, int damaInimiga)
    {
        // TODO isso aqui só funciona se a peça não for dama
        
        int x = posicao[0], y = posicao[1];

        if((x - 2 >= 0 && y - 2 >= 0) && (tabuleiro[x-2,y-2] == pecaInimiga || tabuleiro[x-2,y-2] == damaInimiga))
            return true;
        if((x - 2 >= 0 && y + 2 < 8) && (tabuleiro[x-2,y+2] == pecaInimiga || tabuleiro[x-2,y-2] == damaInimiga))
            return true;
        if((x + 2 < 8 && y - 2 >= 0) && (tabuleiro[x+2,y-2] == pecaInimiga || tabuleiro[x-2,y-2] == damaInimiga))
            return true;
        if((x + 2 < 8 && y + 2 < 8) && (tabuleiro[x+2,y+2] == pecaInimiga || tabuleiro[x-2,y-2] == damaInimiga))
            return true;
        return false;
    }
    
    public List<List<int>> Highlight(int[,] tabuleiro, int x, int y, int jogador, int dama, int pecaInimiga, int damaInimiga)
    {
        // TODO função para checar se alguma peça qlq deve comer
        // comer é obrigatório
        if (tabuleiro[x, y] == dama)
            return HighlightDamas(tabuleiro, x, y, pecaInimiga, damaInimiga);
        List<List<int>> posicoes = new List<List<int>>();
        posicoes = LeiDaMaioria(tabuleiro, x, y, pecaInimiga, damaInimiga);
        if (posicoes.Count > 0)
            return posicoes;
        if(jogador == 1) {
            if (x + 1 < 8 && y + 1 < 8 && Tipos.isVazio(tabuleiro[x + 1, y + 1]))
                posicoes.Add(new List<int> { x + 1, y + 1 });
            if (x - 1 >= 0 && y + 1 < 8 && tabuleiro[x - 1, y + 1] == 0)
                posicoes.Add(new List<int> { x - 1, y + 1 });
        }
        else {
            if (x + 1 < 8 && y - 1 >= 0 && Tipos.isVazio(tabuleiro[x + 1, y - 1]))
                posicoes.Add(new List<int> { x + 1, y - 1 });
            if (x - 1 >= 0 && y - 1 >= 0 && tabuleiro[x - 1, y - 1] == 0)
                posicoes.Add(new List<int> { x - 1, y - 1 });
        }
        return posicoes;
    }

    private List<List<int>> HighlightDamas(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga)
    {
        List<List<int>> posicoesCimaDireita = new List<List<int>>();
        List<List<int>> posicoesCimaEsquerda = new List<List<int>>();
        List<List<int>> posicoesBaixoDireita = new List<List<int>>();
        List<List<int>> posicoesBaixoEsquerda = new List<List<int>>();
        int i, j;
        bool[] controle = {false, true}; // comer, podeMover
        for(i = x, j = y; i < 8 && j < 8; i++, j++){
            controle = checkFor(posicoesCimaDireita, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i < 8 && j >= 0; i++, j--){
            controle = checkFor(posicoesBaixoDireita, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i >= 0 && j < 8; i--, j++){
            controle = checkFor(posicoesCimaEsquerda, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        for(i = x, j = y; i >= 0 && j >= 0; i--, j--){
            controle = checkFor(posicoesBaixoEsquerda, tabuleiro, i, j, pecaInimiga, damaInimiga, controle[0]);
            if(!controle[1]) // não há movimentação nessa diagonal
                break;
        }
        if (controle[0]) // comer == true?
            return max(posicoesBaixoDireita, posicoesBaixoEsquerda, posicoesCimaDireita, posicoesCimaEsquerda);
        else
        {
            List<List<int>> posicoesTotais = new List<List<int>>();
            foreach (List<int> posicao in posicoesBaixoDireita)
                posicoesTotais.Add(posicao);
            foreach (List<int> posicao in posicoesBaixoEsquerda)
                posicoesTotais.Add(posicao);
            foreach (List<int> posicao in posicoesCimaDireita)
                posicoesTotais.Add(posicao);
            foreach (List<int> posicao in posicoesCimaEsquerda)
                posicoesTotais.Add(posicao);
            return posicoesTotais;
        }
    }

    private bool[] checkFor(List<List<int>> posicoes, int[,] tabuleiro, int i, int j, int pecaInimiga, int damaInimiga, bool comer){
        bool[] retorno = {comer, true};
        if (!comer && tabuleiro[i, j] == 0){
            posicoes.Add(new List<int> { i, j });
            return retorno;
        }
        else if(tabuleiro[i, j] == damaInimiga || tabuleiro[i, j] == pecaInimiga){
            retorno[0] = true; // comer = true
            posicoes.Add(new List<int> { i, j });
            List<List<int>> maioria = LeiDaMaioria(tabuleiro, i, j, pecaInimiga, damaInimiga);
            if (maioria.Count > 0)
                foreach (List<int> item in maioria)
                    posicoes.Add(item);
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

