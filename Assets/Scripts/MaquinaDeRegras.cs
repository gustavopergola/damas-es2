using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TiposNS;
using TabuleiroNS;

public class MaquinaDeRegras : MonoBehaviour
{
    // inicializa valores para verificação de movimentos de acordo com o jogador atual
    private int[] GetValoresPecas(int jogador)
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

    // verifica dentro de todas as peças do jogador se existe alguma que deve comer
    public List<List<int>> HasToComer(int[,] tabuleiro, List<List<int>> pecasJogador, int jogador)
    {
        List<List<int>> pecasQueComem = new List<List<int>>();
        int[] valoresPecas = GetValoresPecas(jogador);
        int pecaInimiga = valoresPecas[1], damaInimiga = valoresPecas[2];
        int x, y;
        foreach (List<int> peca in pecasJogador)
        {
            x = peca[0];
            y = peca[1];
            if(Tipos.isDama(tabuleiro[x,y])) {
                // TODO verificar comer como dama
            }
            // TODO verificar lei da maioria
            else {
                if(x + 2 < 8 && y + 2 < 8) // verifica se posição final da peça após comer está nos limites do tabuleiro
                    // verifica se na diagonal existe uma peça inimiga para ser comida e ser é possível mover para a próxima casa
                    if((tabuleiro[x+1, y+1] == pecaInimiga || tabuleiro[x+1, y+1] == damaInimiga) && (Tipos.isVazio(tabuleiro[x+2, y+2])))
                    {
                        pecasQueComem.Add(new List<int>{x,y});
                        continue;
                    }
                if(x + 2 < 8 && y - 2 >= 0)
                    if((tabuleiro[x+1, y-1] == pecaInimiga || tabuleiro[x+1, y-1] == damaInimiga) && (Tipos.isVazio(tabuleiro[x+2, y-2])))
                    {
                        pecasQueComem.Add(new List<int>{x,y});
                        continue;
                    }
                if(x - 2 >= 0 && y + 2 < 8)
                    if((tabuleiro[x-1, y+1] == pecaInimiga || tabuleiro[x-1, y+1] == damaInimiga) && (Tipos.isVazio(tabuleiro[x-2, y+2])))
                    {
                        pecasQueComem.Add(new List<int>{x,y});
                        continue;
                    }
                if(x - 2 >= 0 && y - 2 >= 0)
                    if((tabuleiro[x-1, y-1] == pecaInimiga || tabuleiro[x-1, y-1] == damaInimiga) && (Tipos.isVazio(tabuleiro[x-2, y-2])))
                    {
                        pecasQueComem.Add(new List<int>{x,y});
                        continue;
                    }
            }
        }
        return pecasQueComem;
    }

    
    public List<List<int>> TodosPossiveisMovimentos(int[,] tabuleiro, int jogador, List<List<int>> pecasJogador)
    {
        int[] valoresPecas = GetValoresPecas(jogador);
        int dama = valoresPecas[0], pecaInimiga = valoresPecas[1], damaInimiga = valoresPecas[2];
        List<List<int>> possiveisMovimentos = new List<List<int>>();
        List<List<int>> movimentosAuxiliares = new List<List<int>>();
        bool comer = false;
        foreach (List<int> peca in pecasJogador)
        {
            movimentosAuxiliares = PossiveisMovimentos(tabuleiro, peca[0], peca[1]);
            
            if (HaveComido(tabuleiro,movimentosAuxiliares[0], pecaInimiga, damaInimiga))
            {
                comer = true;
                possiveisMovimentos = new List<List<int>>(); // limpa a lista de possíveis movimentos para poder receber apenas movimentos de captura
            }
            if (comer == true) // se comeu
            {
                if (movimentosAuxiliares.Count > possiveisMovimentos.Count) // verifica qual caminho que mais come peças
                    possiveisMovimentos = movimentosAuxiliares;
            }
            else { // se não comeu
                foreach (List<int> pecaAuxiliar in movimentosAuxiliares)
                    possiveisMovimentos.Add(pecaAuxiliar);
            }
        }
        return possiveisMovimentos;
    }

    private bool HaveComido(int[,] tabuleiro, List<int> posicao, int pecaInimiga, int damaInimiga)
    {
        // TODO isso aqui só funciona se a peça não for dama
        
        int x = posicao[0], y = posicao[1];

        if((x - 2 >= 0 && y - 2 >= 0) && (tabuleiro[x-2, y-2] == pecaInimiga || tabuleiro[x-2, y-2] == damaInimiga))
            return true;
        if((x - 2 >= 0 && y + 2 < 8) && (tabuleiro[x-2, y+2] == pecaInimiga || tabuleiro[x-2, y-2] == damaInimiga))
            return true;
        if((x + 2 < 8 && y - 2 >= 0) && (tabuleiro[x+2, y-2] == pecaInimiga || tabuleiro[x-2, y-2] == damaInimiga))
            return true;
        if((x + 2 < 8 && y + 2 < 8) && (tabuleiro[x+2, y+2] == pecaInimiga || tabuleiro[x-2, y-2] == damaInimiga))
            return true;
        return false;
    }
    
    // retorna os possíveis movimentos de apenas uma peça
    public List<Jogada> PossiveisMovimentos(int[,] tabuleiro, int x, int y)
    {
        int jogador;
        if(Tipos.isJogador1(tabuleiro[x,y]))
            jogador = 1;
        else
            jogador = 2;
        int[] valoresPecas = GetValoresPecas(jogador);
        int dama = valoresPecas[0], pecaInimiga = valoresPecas[1], damaInimiga = valoresPecas[2];
        if (tabuleiro[x,y] == dama)
            return MovimentosDama(tabuleiro, x, y, pecaInimiga, damaInimiga);
        List<Jogada> jogadas = new List<Jogada>();
        // TODO chamar lei da maioria pra verificar qual movimento que mais come peça e...
        if (jogadas.Count > 0)
            return jogadas;
        // ...retornar ele
        if(jogador == 1)
        {
            if (x + 1 < 8 && y + 1 < 8 && Tipos.isVazio(tabuleiro[x + 1, y + 1]))
            {
                Jogada novaJogada = new Jogada();
                novaJogada.movimentos.Add(new int[2] { x + 1, y + 1 });
                jogadas.Add(novaJogada);
            }
            if (x - 1 >= 0 && y + 1 < 8 && tabuleiro[x - 1, y + 1] == 0)
            {
                Jogada novaJogada = new Jogada();
                novaJogada.movimentos.Add(new int[2] { x - 1, y + 1 });
                jogadas.Add(novaJogada);
            }
        }
        else {
            if (x + 1 < 8 && y - 1 >= 0 && Tipos.isVazio(tabuleiro[x + 1, y - 1]))
            {
                Jogada novaJogada = new Jogada();
                novaJogada.movimentos.Add(new int[2] { x + 1, y - 1 });
                jogadas.Add(novaJogada);
            }
            if (x - 1 >= 0 && y - 1 >= 0 && tabuleiro[x - 1, y - 1] == 0)
            {
                Jogada novaJogada = new Jogada();
                novaJogada.movimentos.Add(new int[2] { x - 1, y - 1 });
                jogadas.Add(novaJogada);
            }
        }
        return jogadas;
    }

    // retorna os possíveis movimentos para dama
    private List<Jogada> MovimentosDama(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga)
    {
        Jogada jogadaCimaDireita = new Jogada();
        Jogada jogadaCimaEsquerda = new Jogada();
        Jogada jogadaBaixoDireita = new Jogada();
        Jogada jogadaBaixoEsquerda = new Jogada();
        int i, j;
        // TODO chamar LeiDaMaioria aqui
        for(i = x+1, j = y+1; i < Tabuleiro.getTamanhoTabuleiro() && j < Tabuleiro.getTamanhoTabuleiro(); i++, j++)
        {
            if(Tipos.isVazio(tabuleiro[i,j]))
            {
                jogadaCimaDireita.movimentos.Add(new int[2] {i,j});
                // chamar LeiDaMaioria aqui
            }
            else if(tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
            {
                // chamar LeiDaMaioria aqui
            }
            // caso onde achou uma peça aliada
            else break;

        }
        for(i = x+1, j = y-1; i < Tabuleiro.getTamanhoTabuleiro() && j >= 0; i++, j--)
        {
            if(Tipos.isVazio(tabuleiro[i,j]))
            {
                jogadaCimaDireita.movimentos.Add(new int[2] {i,j});
                // chamar LeiDaMaioria aqui
            }
            else if(tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
            {
                // chamar LeiDaMaioria aqui
            }
            // caso onde achou uma peça aliada
            else break;
        }
        for(i = x-1, j = y+1; i >= 0 && j < Tabuleiro.getTamanhoTabuleiro(); i--, j++)
        {
            if(Tipos.isVazio(tabuleiro[i,j]))
            {
                jogadaCimaDireita.movimentos.Add(new int[2] {i,j});
                // chamar LeiDaMaioria aqui
            }
            else if(tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
            {
                // chamar LeiDaMaioria aqui
            }
            // caso onde achou uma peça aliada
            else break;
        }
        for(i = x-1, j = y-1; i >= 0 && j >= 0; i--, j--)
        {
            if(Tipos.isVazio(tabuleiro[i,j]))
            {
                jogadaCimaDireita.movimentos.Add(new int[2] {i,j});
                // chamar LeiDaMaioria aqui
            }
            else if(tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
            {
                // chamar LeiDaMaioria aqui
            }
            // caso onde achou uma peça aliada
            else break;
        }
    }

    // 
    private List<List<int>> LeiDaMaioria(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga) {
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
        return Max(pos1, pos2, pos3, pos4);
    }

    // retorna qual lista tem mais elementos
    private List<List<int>> Max(List<List<int>> lista1, List<List<int>> lista2, List<List<int>> lista3, List<List<int>> lista4) {
        if(lista1.Count >= lista2.Count && lista1.Count >= lista3.Count && lista1.Count >= lista4.Count)
            return lista1;
        if(lista2.Count >= lista1.Count && lista2.Count >= lista3.Count && lista2.Count >= lista4.Count)
            return lista2;
        if(lista3.Count >= lista1.Count && lista3.Count >= lista2.Count && lista3.Count >= lista4.Count)
            return lista3;
        else return lista4;
    }
}

