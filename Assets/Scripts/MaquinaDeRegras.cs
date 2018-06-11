﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TiposNS;
using EstadoNS;

namespace MaquinaDeRegrasNS
{
    public class MaquinaDeRegras : MonoBehaviour
    {
        // inicializa valores para verificação de movimentos de acordo com o jogador atual
        private static int[] GetValoresPecas(int jogador)
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

        public static List<List<Jogada>>[] TodosPossiveisMovimentos(int[,] tabuleiro, List<int[]> pecasJogador1, List<int[]> pecasJogador2)
        {
            List<List<Jogada>>[] possiveisJogadas = new List<List<Jogada>>[2];
            possiveisJogadas[0] = PossiveisMovimentosUmJogador(tabuleiro, pecasJogador1);
            possiveisJogadas[1] = PossiveisMovimentosUmJogador(tabuleiro, pecasJogador2);
            return possiveisJogadas;
        }

        // cada item da lista está relacionado com uma peça
        // cada peça possui a própria lista de jogadas válidas
        public static List<List<Jogada>> PossiveisMovimentosUmJogador(int[,] tabuleiro, List<int[]> pecasJogador)
        {   
            int maiorNumeroPecasComidas = 0;
            List<List<Jogada>> possiveisJogadas = new List<List<Jogada>>();

            foreach (int[] peca in pecasJogador)
            {
                List<Jogada> jogadas_peca = PossiveisMovimentosUmaPeca(tabuleiro, peca[0], peca[1]);
                if (jogadas_peca.Count > 0){
                    int pecasComidasPecaAtual = jogadas_peca.First().pecasComidas.Count;
                    // se foi a jogada analisada que mais comeu peça (lei da maioria)
                    if (maiorNumeroPecasComidas < pecasComidasPecaAtual){
                        possiveisJogadas = new List<List<Jogada>>();
                        maiorNumeroPecasComidas = pecasComidasPecaAtual;
                        List<Jogada> lista_unica_jogada = new List<Jogada>();
                        lista_unica_jogada.Add(jogadas_peca.First());
                        possiveisJogadas.Add(lista_unica_jogada);
                    }else if (pecasComidasPecaAtual == maiorNumeroPecasComidas){
                        possiveisJogadas.Add(jogadas_peca);
                    }
                }
            }
            return possiveisJogadas;
            
        }

        // retorna os possíveis movimentos de apenas uma peça
        public static List<Jogada> PossiveisMovimentosUmaPeca(int[,] tabuleiro, int x, int y)
        {
            int jogador = Tipos.pegaJogador(tabuleiro[x, y]);

            int[] valoresPecas = GetValoresPecas(jogador);

            // se for dama
            if (tabuleiro[x, y] == valoresPecas[0])
                return MovimentosDama(tabuleiro, x, y, valoresPecas[1], valoresPecas[2]);

            List<Jogada> jogadas = new List<Jogada>();
            Jogada captura = LeiDaMaioria(tabuleiro, x, y, valoresPecas[1], valoresPecas[2], null);
            // se teve alguma jogada com captura / peças comida
            if (captura != null)
            {
                jogadas.Add(captura);
                return jogadas;
            }

            Jogada novaJogada;
            int[] posPeca = new int[2] { x, y };
            if (jogador == 1)
            {
                if ((x + 1 < 8 && y + 1 < 8) && Tipos.isVazio(tabuleiro[x + 1, y + 1]))
                {
                    novaJogada = new Jogada(posPeca);
                    novaJogada.movimentos.Add(new int[2] { x + 1, y + 1 });
                    jogadas.Add(novaJogada);
                }
                if ((x + 1 < 8 && y - 1 >= 0) && Tipos.isVazio(tabuleiro[x + 1, y - 1]))
                {
                    novaJogada = new Jogada(posPeca);
                    novaJogada.movimentos.Add(new int[2] { x + 1, y - 1 });
                    jogadas.Add(novaJogada);
                }
            }
            else
            {                    
                if ((x - 1 >= 0 && y + 1 < 8) && Tipos.isVazio(tabuleiro[x - 1, y + 1]))
                {
                    novaJogada = new Jogada(posPeca);
                    novaJogada.movimentos.Add(new int[2] { x - 1, y + 1 });
                    jogadas.Add(novaJogada);
                }
                if ((x - 1 >= 0 && y - 1 >= 0) && Tipos.isVazio(tabuleiro[x - 1, y - 1]))
                {
                    novaJogada = new Jogada(posPeca);
                    novaJogada.movimentos.Add(new int[2] { x - 1, y - 1 });
                    jogadas.Add(novaJogada);
                }
            }
            
            return jogadas;
        }

        private static Jogada LeiDaMaioria(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga, List<int[]> pecasComidas)
        {
            
            Jogada cimaDireita = null;
            Jogada cimaEsquerda = null;
            Jogada baixoDireita = null;
            Jogada baixoEsquerda = null;
            int[] posPeca = new int[2] { x, y };
            // inicializa a lista de peças comidas, caso seja a primeira chamada ao método
            if (pecasComidas == null)
                pecasComidas = new List<int[]>();
            // se tem uma peça ou dama inimiga na vizinhança
            if ((x + 1 < 8 && y + 1 < 8) && (tabuleiro[x + 1, y + 1] == pecaInimiga || tabuleiro[x + 1, y + 1] == damaInimiga))
            {
                
                // e se ela ainda não foi comida numa jogada em cadeia
                if (!contemPeca(pecasComidas,new int[2] { x + 1, y + 1 }))
                {
                    // e se tem uma casa vazia logo em seguida
                    if ((x + 2 < 8 && y + 2 < 8) && Tipos.isVazio(tabuleiro[x + 2, y + 2]))
                    {
                        // então é uma jogada que come
                        if (cimaDireita == null)
                            cimaDireita = new Jogada(posPeca);
                        cimaDireita.movimentos.Add(new int[2] { x + 2, y + 2 });
                        cimaDireita.pecasComidas.Add(new int[2] { x + 1, y + 1 });
                        // atualiza o "estado" do tabuleiro
                        int pecaAtual = tabuleiro[x, y];
                        tabuleiro[x, y] = 0;
                        tabuleiro[x + 2, y + 2] = pecaAtual;
                        int indicePecasComidas = pecasComidas.Count();
                        pecasComidas.Add(new int[2] { x + 1, y + 1 });
                        // chamada recursiva para olhar as próximas jogadas
                        Jogada futura = LeiDaMaioria(tabuleiro, x + 2, y + 2, pecaInimiga, damaInimiga, pecasComidas);
                        // adiciona o resultado da jogada à jogada anterior
                        if (futura != null){
                            cimaDireita.movimentos.Concat(futura.movimentos);
                            cimaDireita.pecasComidas.Concat(futura.pecasComidas);
                        }
                        // retorna os valores originais do "estado" do tabuleiro
                        tabuleiro[x, y] = pecaAtual;
                        tabuleiro[x + 2, y + 2] = 0;
                        pecasComidas.RemoveAt(indicePecasComidas);
                    }
                }
            }
            if ((x + 1 < 8 && y - 1 >= 0) && (tabuleiro[x + 1, y - 1] == pecaInimiga || tabuleiro[x + 1, y - 1] == damaInimiga))
            {
                
                if (!contemPeca(pecasComidas,new int[2] { x + 1, y - 1 }))
                {
                    if ((x + 2 < 8 && y - 2 >= 0) && Tipos.isVazio(tabuleiro[x + 2, y - 2]))
                    {
                        
                        if (cimaEsquerda == null)
                            cimaEsquerda = new Jogada(posPeca);
                        cimaEsquerda.movimentos.Add(new int[2] { x + 2, y - 2 });
                        cimaEsquerda.pecasComidas.Add(new int[2] { x + 1, y - 1 });
                        int pecaAtual = tabuleiro[x, y];
                        tabuleiro[x, y] = 0;
                        tabuleiro[x + 2, y - 2] = pecaAtual;
                        int indicePecasComidas = pecasComidas.Count();
                        pecasComidas.Add(new int[2] { x + 1, y - 1 });
                        Jogada futura = LeiDaMaioria(tabuleiro, x + 2, y - 2, pecaInimiga, damaInimiga, pecasComidas);
                        if (futura != null){
                            cimaEsquerda.movimentos.Concat(futura.movimentos);
                            cimaEsquerda.pecasComidas.Concat(futura.pecasComidas);
                        }
                        tabuleiro[x, y] = pecaAtual;
                        tabuleiro[x + 2, y - 2] = 0;
                        pecasComidas.RemoveAt(indicePecasComidas);
                    }
                }
            }
            if ((x - 1 >= 0 && y + 1 < 8) && (tabuleiro[x - 1, y + 1] == pecaInimiga || tabuleiro[x - 1, y + 1] == damaInimiga))
            {
                
                if (!contemPeca(pecasComidas, new int[2] { x - 1, y + 1 }))
                {
                    if ((x - 2 >= 0 && y + 2 > 8) && Tipos.isVazio(tabuleiro[x - 2, y + 2]))
                    {
                        if (baixoDireita == null)
                            baixoDireita = new Jogada(posPeca);
                        baixoDireita.movimentos.Add(new int[2] { x - 2, y + 2 });
                        baixoDireita.pecasComidas.Add(new int[2] { x - 1, y + 1 });
                        int pecaAtual = tabuleiro[x, y];
                        tabuleiro[x, y] = 0;
                        tabuleiro[x - 2, y + 2] = pecaAtual;
                        int indicePecasComidas = pecasComidas.Count();
                        pecasComidas.Add(new int[2] { x - 1, y + 1 });
                        Jogada futura = LeiDaMaioria(tabuleiro, x - 2, y + 2, pecaInimiga, damaInimiga, pecasComidas);
                        if (futura != null){
                            baixoDireita.movimentos.Concat(futura.movimentos);
                            baixoDireita.pecasComidas.Concat(futura.pecasComidas);
                        }
                        tabuleiro[x, y] = pecaAtual;
                        tabuleiro[x - 2, y + 2] = 0;
                        pecasComidas.RemoveAt(indicePecasComidas);
                    }
                }
            }
            if ((x - 1 >= 0 && y - 1 >= 0) && (tabuleiro[x - 1, y - 1] == pecaInimiga || tabuleiro[x - 1, y - 1] == damaInimiga))
            {
                if (!contemPeca(pecasComidas, new int[2] { x - 1, y - 1 }))
                {
                    if ((x - 2 >= 0 && y - 2 >= 0) && Tipos.isVazio(tabuleiro[x - 2, y - 2]))
                    {
                         
                        if (baixoEsquerda == null)
                            baixoEsquerda = new Jogada(posPeca);
                        baixoEsquerda.movimentos.Add(new int[2] { x - 2, y - 2 });
                        baixoEsquerda.pecasComidas.Add(new int[2] { x - 1, y - 1 });
                        int pecaAtual = tabuleiro[x, y];
                        tabuleiro[x, y] = 0;
                        tabuleiro[x - 2, y - 2] = pecaAtual;
                        int indicePecasComidas = pecasComidas.Count();
                        pecasComidas.Add(new int[2] { x - 1, y - 1 });
                        Jogada futura = LeiDaMaioria(tabuleiro, x - 2, y - 2, pecaInimiga, damaInimiga, pecasComidas);
                        if (futura != null){
                            baixoEsquerda.movimentos.Concat(futura.movimentos);
                            baixoEsquerda.pecasComidas.Concat(futura.pecasComidas);
                        }
                        tabuleiro[x, y] = pecaAtual;
                        tabuleiro[x - 2, y - 2] = 0;
                        pecasComidas.RemoveAt(indicePecasComidas);
                    }
                }
            }
            Jogada melhor = cimaDireita;
            if (melhor == null || (cimaEsquerda != null && cimaEsquerda.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = cimaEsquerda;
            if (melhor == null || (baixoDireita != null && baixoDireita.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = baixoDireita;
            if (melhor == null || (baixoEsquerda != null && baixoEsquerda.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = baixoEsquerda;

            return melhor;
        }

        private static Jogada LeiDaMaioriaDamas(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga, List<int[]> pecasComidas)
        {
            Jogada cimaDireita = null;
            Jogada cimaEsquerda = null;
            Jogada baixoDireita = null;
            Jogada baixoEsquerda = null;
            int[] posPeca = new int[2] { x, y };
            // lista de jogadas que podem surgir de uma casa vazia
            // olhar if (Tipos.isVazio(tabuleiro[i,j]))
            List<Jogada> jogadasEmCasasVazias = new List<Jogada>();
            // inicializa a lista de peças comidas, caso seja a primeira chamada ao método
            if (pecasComidas == null)
            {
                pecasComidas = new List<int[]>();
            }

            // percorrer cada diagonal a procura da peças para comer
            int i , j;
            for (i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
            {
                // se encontrou uma peça pra comer
                if (tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
                {
                    // e se ela ainda não foi comida numa jogada em cadeia
                    if (!contemPeca(pecasComidas,new int[2] { i, j }))
                    {
                        // e se tem uma casa vazia logo em seguida
                        if ((i + 2 < 8 && j + 2 < 8) && Tipos.isVazio(tabuleiro[i + 2, j + 2]))
                        {
                            // então é uma jogada que come
                            if (cimaDireita == null)
                                cimaDireita = new Jogada(posPeca);
                            cimaDireita.movimentos.Add(new int[2] { i + 2, j + 2 });
                            cimaDireita.pecasComidas.Add(new int[2] { i + 1, j + 1 });
                            // atualiza o "estado" do tabuleiro
                            int pecaAtual = tabuleiro[i, j];
                            tabuleiro[i, j] = 0;
                            tabuleiro[i + 2, j + 2] = pecaAtual;
                            int indicePecasComidas = pecasComidas.Count();
                            pecasComidas.Add(new int[2] { i + 1, j + 1 });
                            // chamada recursiva para olhar as próximas jogadas
                            Jogada futura = LeiDaMaioria(tabuleiro, i + 2, j + 2, pecaInimiga, damaInimiga, pecasComidas);
                            // adiciona o resultado da jogada à jogada anterior
                            if (futura != null){
                                cimaDireita.movimentos.Concat(futura.movimentos);
                                cimaDireita.pecasComidas.Concat(futura.pecasComidas);
                            }
                            // retorna os valores originais do "estado" do tabuleiro
                            tabuleiro[i, j] = pecaAtual;
                            tabuleiro[i + 2, j + 2] = 0;
                            pecasComidas.RemoveAt(indicePecasComidas);
                        }
                    }
                }
                // em um espaço vazio verifica a melhor jogada a ser fazer a partir dai
                // pode acarretar em um nova jogada comendo peças * jogadasCasasVazias *
                else if (Tipos.isVazio(tabuleiro[i,j]))
                {
                    // a ideia aqui é simular como seria seguir a jogada a partir desse caminho
                    // com o intuito de analisar qual o melhor caminho
                    int[] novaPos = new int[2] { i + 1, j + 1 };

                    // variavel para controlar se pode haver captura numa vizinhança a esta casa vazia
                    bool capturaVizinhanca = false;

                    // se encontrou uma peça pra comer
                    if (i + 1 < 8 && j + 1 < 8 && (tabuleiro[i+1,j+1] == pecaInimiga || tabuleiro[i+1,j+1] == damaInimiga))
                    {
                        // e se ela ainda não foi comida numa jogada em cadeia
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            // e se tem uma casa vazia logo em seguida
                            if ((i + 2 < 8 && j + 2 < 8) && Tipos.isVazio(tabuleiro[i + 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i + 1 < 8 && j - 1 >= 0 && (tabuleiro[i+1,j-1] == pecaInimiga || tabuleiro[i+1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i + 2, j - 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j + 1 < 8 && (tabuleiro[i-1,j+1] == pecaInimiga || tabuleiro[i-1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j + 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && (tabuleiro[i-1,j-1] == pecaInimiga || tabuleiro[i-1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j - 2]))
                            capturaVizinhanca = true;
                        }
                    }
                    // caso possa haver a captura
                    if (capturaVizinhanca){
                        // faz uma chamada recursiva aqui para analisar o possivel caminho
                        tabuleiro[i,j] = tabuleiro[x,y];
                        tabuleiro[x,y] = 0;
                        Jogada possivel = LeiDaMaioriaDamas(tabuleiro, i, j, pecaInimiga, damaInimiga, pecasComidas);
                        if (possivel != null){                                    
                            Jogada casaVazia = new Jogada(posPeca);
                            copiaJogada(cimaDireita, casaVazia);
                            casaVazia.movimentos.Add(new int[2] {i, j});
                            casaVazia.movimentos.Concat(possivel.movimentos);
                            casaVazia.pecasComidas.Concat(possivel.pecasComidas);
                            jogadasEmCasasVazias.Add(casaVazia);
                        }
                        tabuleiro[x,y] = tabuleiro[i,j];
                        tabuleiro[i,j] = 0;
                    }
                }
                // caso onde achou uma peça aliada
                else break;
            }
            for (i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--)
            {
                if (tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
                {
                    if (!contemPeca(pecasComidas,new int[2] { i, j }))
                    {
                        if ((i + 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i + 2, j - 2]))
                        {
                            if (cimaEsquerda == null)
                                cimaEsquerda = new Jogada(posPeca);
                            cimaEsquerda.movimentos.Add(new int[2] { i + 2, j - 2 });
                            cimaEsquerda.pecasComidas.Add(new int[2] { i + 1, j - 1 });
                            int pecaAtual = tabuleiro[i, j];
                            tabuleiro[i, j] = 0;
                            tabuleiro[i + 2, j - 2] = pecaAtual;
                            int indicePecasComidas = pecasComidas.Count();
                            pecasComidas.Add(new int[2] { i + 1, j - 1 });
                            Jogada futura = LeiDaMaioria(tabuleiro, i + 2, j - 2, pecaInimiga, damaInimiga, pecasComidas);
                            if (futura != null){
                                cimaEsquerda.movimentos.Concat(futura.movimentos);
                                cimaEsquerda.pecasComidas.Concat(futura.pecasComidas);
                            }
                            tabuleiro[i, j] = pecaAtual;
                            tabuleiro[i + 2, j - 2] = 0;
                            pecasComidas.RemoveAt(indicePecasComidas);
                        }
                    }
                }
                else if (Tipos.isVazio(tabuleiro[i,j]))
                {
                    int[] novaPos = new int[2] { i + 1, j + 1 };
                    bool capturaVizinhanca = false;
                    if (i + 1 < 8 && j + 1 < 8 && (tabuleiro[i+1,j+1] == pecaInimiga || tabuleiro[i+1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j + 2 < 8) && Tipos.isVazio(tabuleiro[i + 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i + 1 < 8 && j - 1 >= 0 && (tabuleiro[i+1,j-1] == pecaInimiga || tabuleiro[i+1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i + 2, j - 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j + 1 < 8 && (tabuleiro[i-1,j+1] == pecaInimiga || tabuleiro[i-1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j + 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && (tabuleiro[i-1,j-1] == pecaInimiga || tabuleiro[i-1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j - 2]))
                            capturaVizinhanca = true;
                        }
                    }
                    if (capturaVizinhanca){
                        tabuleiro[i,j] = tabuleiro[x,y];
                        tabuleiro[x,y] = 0;
                        Jogada possivel = LeiDaMaioriaDamas(tabuleiro, i, j, pecaInimiga, damaInimiga, pecasComidas);
                        if (possivel != null){                                    
                            Jogada casaVazia = new Jogada(posPeca);
                            copiaJogada(cimaEsquerda, casaVazia);
                            casaVazia.movimentos.Add(new int[2] {i, j});
                            casaVazia.movimentos.Concat(possivel.movimentos);
                            casaVazia.pecasComidas.Concat(possivel.pecasComidas);
                            jogadasEmCasasVazias.Add(casaVazia);
                        }
                        tabuleiro[x,y] = tabuleiro[i,j];
                        tabuleiro[i,j] = 0;
                    }
                }
                // caso onde achou uma peça aliada
                else break;
            }
            for (i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)
            {
                if (tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
                {
                    if (!contemPeca(pecasComidas,new int[2] { i, j }))
                    {
                        if ((i - 2 < 8 && j + 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j + 2]))
                        {
                            if (baixoDireita == null)
                                baixoDireita = new Jogada(posPeca);
                            baixoDireita.movimentos.Add(new int[2] { i - 2, j + 2 });
                            baixoDireita.pecasComidas.Add(new int[2] { i - 1, j + 1 });
                            int pecaAtual = tabuleiro[i, j];
                            tabuleiro[i, j] = 0;
                            tabuleiro[i - 2, j + 2] = pecaAtual;
                            int indicePecasComidas = pecasComidas.Count();
                            pecasComidas.Add(new int[2] { i - 1, j + 1 });
                            Jogada futura = LeiDaMaioria(tabuleiro, i - 2, j + 2, pecaInimiga, damaInimiga, pecasComidas);
                            if (futura != null){
                                baixoDireita.movimentos.Concat(futura.movimentos);
                                baixoDireita.pecasComidas.Concat(futura.pecasComidas);
                            }
                            tabuleiro[i, j] = pecaAtual;
                            tabuleiro[i - 2, j + 2] = 0;
                            pecasComidas.RemoveAt(indicePecasComidas);
                        }
                    }
                }
                else if (Tipos.isVazio(tabuleiro[i,j]))
                {
                    int[] novaPos = new int[2] { i + 1, j + 1 };
                    bool capturaVizinhanca = false;
                    if (i + 1 < 8 && j + 1 < 8 && (tabuleiro[i+1,j+1] == pecaInimiga || tabuleiro[i+1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j + 2 < 8) && Tipos.isVazio(tabuleiro[i + 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i + 1 < 8 && j - 1 >= 0 && (tabuleiro[i+1,j-1] == pecaInimiga || tabuleiro[i+1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i + 2, j - 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j + 1 < 8 && (tabuleiro[i-1,j+1] == pecaInimiga || tabuleiro[i-1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j + 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && (tabuleiro[i-1,j-1] == pecaInimiga || tabuleiro[i-1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j - 2]))
                            capturaVizinhanca = true;
                        }
                    }
                    if (capturaVizinhanca){
                        tabuleiro[i,j] = tabuleiro[x,y];
                        tabuleiro[x,y] = 0;
                        Jogada possivel = LeiDaMaioriaDamas(tabuleiro, i, j, pecaInimiga, damaInimiga, pecasComidas);
                        if (possivel != null){                                    
                            Jogada casaVazia = new Jogada(posPeca);
                            copiaJogada(baixoDireita, casaVazia);
                            casaVazia.movimentos.Add(new int[2] {i, j});
                            casaVazia.movimentos.Concat(possivel.movimentos);
                            casaVazia.pecasComidas.Concat(possivel.pecasComidas);
                            jogadasEmCasasVazias.Add(casaVazia);
                        }
                        tabuleiro[x,y] = tabuleiro[i,j];
                        tabuleiro[i,j] = 0;
                    }
                }
                // caso onde achou uma peça aliada
                else break;
            }
            for (i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (tabuleiro[i,j] == pecaInimiga || tabuleiro[i,j] == damaInimiga)
                {
                    if (!contemPeca(pecasComidas,new int[2] { i, j }))
                    {
                        if ((i - 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j - 2]))
                        {
                            if (baixoEsquerda == null)
                                baixoEsquerda = new Jogada(posPeca);
                            baixoEsquerda.movimentos.Add(new int[2] { i - 2, j - 2 });
                            baixoEsquerda.pecasComidas.Add(new int[2] { i - 1, j - 1 });
                            int pecaAtual = tabuleiro[i, j];
                            tabuleiro[i, j] = 0;
                            tabuleiro[i - 2, j - 2] = pecaAtual;
                            int indicePecasComidas = pecasComidas.Count();
                            pecasComidas.Add(new int[2] { i - 1, j - 1 });
                            Jogada futura = LeiDaMaioria(tabuleiro, i - 2, j - 2, pecaInimiga, damaInimiga, pecasComidas);
                            if (futura != null){
                                baixoEsquerda.movimentos.Concat(futura.movimentos);
                                baixoEsquerda.pecasComidas.Concat(futura.pecasComidas);
                            }
                            tabuleiro[i, j] = pecaAtual;
                            tabuleiro[i - 2, j - 2] = 0;
                            pecasComidas.RemoveAt(indicePecasComidas);
                        }
                    }
                }
                else if (Tipos.isVazio(tabuleiro[i,j]))
                {
                    int[] novaPos = new int[2] { i + 1, j + 1 };
                    bool capturaVizinhanca = false;
                    if (i + 1 < 8 && j + 1 < 8 && (tabuleiro[i+1,j+1] == pecaInimiga || tabuleiro[i+1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j + 2 < 8) && Tipos.isVazio(tabuleiro[i + 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i + 1 < 8 && j - 1 >= 0 && (tabuleiro[i+1,j-1] == pecaInimiga || tabuleiro[i+1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i + 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i + 2, j - 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j + 1 < 8 && (tabuleiro[i-1,j+1] == pecaInimiga || tabuleiro[i-1,j+1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j + 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j + 2]))
                                capturaVizinhanca = true;
                        }
                    }
                    if (i - 1 >= 0 && j - 1 >= 0 && (tabuleiro[i-1,j-1] == pecaInimiga || tabuleiro[i-1,j-1] == damaInimiga))
                    {
                        if (!contemPeca(pecasComidas,novaPos))
                        {
                            if ((i - 2 < 8 && j - 2 >= 0) && Tipos.isVazio(tabuleiro[i - 2, j - 2]))
                            capturaVizinhanca = true;
                        }
                    }
                    if (capturaVizinhanca){
                        tabuleiro[i,j] = tabuleiro[x,y];
                        tabuleiro[x,y] = 0;
                        Jogada possivel = LeiDaMaioriaDamas(tabuleiro, i, j, pecaInimiga, damaInimiga, pecasComidas);
                        if (possivel != null){                                    
                            Jogada casaVazia = new Jogada(posPeca);
                            copiaJogada(baixoEsquerda, casaVazia);
                            casaVazia.movimentos.Add(new int[2] {i, j});
                            casaVazia.movimentos.Concat(possivel.movimentos);
                            casaVazia.pecasComidas.Concat(possivel.pecasComidas);
                            jogadasEmCasasVazias.Add(casaVazia);
                        }
                        tabuleiro[x,y] = tabuleiro[i,j];
                        tabuleiro[i,j] = 0;
                    }
                }
                // caso onde achou uma peça aliada
                else break;
            }
            Jogada melhor = cimaDireita;
            if (melhor == null || (cimaEsquerda != null && cimaEsquerda.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = cimaEsquerda;
            if (melhor == null || (baixoDireita != null && baixoDireita.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = baixoDireita;
            if (melhor == null || (baixoEsquerda != null && baixoEsquerda.pecasComidas.Count() > melhor.pecasComidas.Count()))
                melhor = baixoEsquerda;
            if (jogadasEmCasasVazias != null){
                foreach (Jogada casaVazia in jogadasEmCasasVazias){
                    if (melhor == null || (casaVazia.pecasComidas.Count() > melhor.pecasComidas.Count()))
                        melhor = casaVazia;
                }
            }
            return melhor;
        }

        // retorna os possíveis movimentos para dama
        private static List<Jogada> MovimentosDama(int[,] tabuleiro, int x, int y, int pecaInimiga, int damaInimiga)
        {
            // TODO chamar LeiDaMaioria aqui
            List<Jogada> jogadas = new List<Jogada>();
            Jogada captura = LeiDaMaioriaDamas(tabuleiro, x, y, pecaInimiga, damaInimiga, null);
            // se teve alguma jogada com captura / peças comida
            if (captura != null)
            {
                jogadas.Add(captura);
                return jogadas;
            }
            int[] posPeca = new int[2] { x, y };
            Jogada jogadaCimaDireita = null;
            Jogada jogadaCimaEsquerda = null;
            Jogada jogadaBaixoDireita = null;
            Jogada jogadaBaixoEsquerda = null;
            int i, j;
            for (i = x + 1, j = y + 1; i < 8 && j < 8; i++, j++)
            {
                if (Tipos.isVazio(tabuleiro[i, j]))
                {
                    if (jogadaCimaDireita == null)
                        jogadaCimaDireita = new Jogada(posPeca);
                    jogadaCimaDireita.movimentos.Add(new int[2] { i, j });
                }
                // caso onde achou uma peça aliada
                else break;

            }
            for (i = x + 1, j = y - 1; i < 8 && j >= 0; i++, j--)
            {
                if (Tipos.isVazio(tabuleiro[i, j]))
                {
                    if (jogadaCimaEsquerda == null)
                        jogadaCimaEsquerda = new Jogada(posPeca);
                    jogadaCimaEsquerda.movimentos.Add(new int[2] { i, j });
                }
                // caso onde achou uma peça aliada
                else break;
            }
            for (i = x - 1, j = y + 1; i >= 0 && j < 8; i--, j++)
            {
                if (Tipos.isVazio(tabuleiro[i, j]))
                {
                    if (jogadaBaixoDireita == null)
                        jogadaBaixoDireita = new Jogada(posPeca);
                    jogadaBaixoDireita.movimentos.Add(new int[2] { i, j });
                }
                // caso onde achou uma peça aliada
                else break;
            }
            for (i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (Tipos.isVazio(tabuleiro[i, j]))
                {
                    if (jogadaBaixoEsquerda == null)
                        jogadaBaixoEsquerda = new Jogada(posPeca);
                    jogadaBaixoEsquerda.movimentos.Add(new int[2] { i, j });
                }
                // caso onde achou uma peça aliada
                else break;
            }
            List<Jogada> jogadasPossiveis = new List<Jogada>();
            jogadasPossiveis.Add(jogadaCimaDireita);
            jogadasPossiveis.Add(jogadaCimaEsquerda);
            jogadasPossiveis.Add(jogadaBaixoDireita);
            jogadasPossiveis.Add(jogadaBaixoEsquerda);
            return jogadasPossiveis;
        }
        
        private static  bool contemPeca(List<int[]> lista, int[] peca){
            foreach(int[] peca_lista in lista)
                if(peca_lista[0] == peca[0] && peca_lista[1] == peca[1])
                    return true;
            return false;
        }

        private static void copiaJogada(Jogada origem, Jogada destino){
            origem.posInicial = destino.posInicial;
            foreach(int[] movimento in origem.movimentos)
                destino.movimentos.Add(new int[2] {movimento[0], movimento[1]});
            foreach(int[] pecaComida in destino.pecasComidas)
                destino.pecasComidas.Add(new int[2] {pecaComida[0], pecaComida[1]});
        }

    }

    
}
