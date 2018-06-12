using System;
using System.Collections;
using System.Collections.Generic;
using EstadoNS;
using MaquinaDeRegrasNS;
using TiposNS;

namespace IANS
{
    public class IA
    {
        int jogadorId;
        int tipo;

        static int[,] valoracao = { {0,4,0,4,0,4,0,4},
                                    {4,0,3,0,3,0,3,0},
                                    {0,3,0,2,0,2,0,4},
                                    {4,0,2,0,1,0,3,0},
                                    {0,3,0,1,0,2,0,4},
                                    {4,0,2,0,2,0,3,0},
                                    {0,3,0,3,0,3,0,4},
                                    {4,0,4,0,4,0,4,0}
        };

        public IA(int jogadorId, int tipo)
        {
            this.jogadorId = jogadorId;
            this.tipo = tipo;
        }

        public IA(int jogadorId)
        {
            this.jogadorId = jogadorId;
            this.tipo = 1;
        }

        public IA()
        {
            this.jogadorId = 2;
            this.tipo = 1;
        }

        public Jogada getAction(){
            //return alpha_beta_search(estado atual);
            return null;
        }

        public Jogada alpha_beta_search(Estado estado)
        {

            int v = Int32.MinValue;
            int alfa = Int32.MinValue;
            int beta = Int32.MaxValue;

            List<Jogada> jogadas = MaquinaDeRegras.PossiveisMovimentos(estado);
            int i = 0, temp, jogadaIndex = 0;
            foreach (var jogada in jogadas)
            {
                //MAX(v,min_value(result(s,a),alfa,beta))
                temp = min_value(Estado.result(estado, jogada), alfa, beta);
                if (v < temp)
                {
                    v = temp;
                    jogadaIndex = i;
                }

                if (v >= beta)
                {
                    return jogada;
                }

                //MAX(alfa,v)
                if (alfa < v)
                {
                    alfa = v;
                }

                i++;
            }

            List<Jogada>.Enumerator e = jogadas.GetEnumerator();
            for (i = 0; i < jogadaIndex + 1; i++)
            {
                e.MoveNext();
            }
            return e.Current;
        }

        public int max_value(Estado estado, int alfa, int beta)
        {
            if (cut(estado) || estado.gameIsOver())
            {
                return eval(estado);
            }

            int v = Int32.MinValue;
            int temp;
            List<Jogada> jogadas = MaquinaDeRegras.PossiveisMovimentos(estado);
            foreach (var jogada in jogadas)
            {
                //MAX(v,min_value(result(s,a),alfa,beta))
                temp = min_value(Estado.result(estado, jogada), alfa, beta);
                if (v < temp)
                {
                    v = temp;
                }

                if (v >= beta)
                {
                    return v;
                }

                //MAX(alfa,v)
                if (alfa < v)
                {
                    alfa = v;
                }
            }
            return v;
        }

        public int min_value(Estado estado, int alfa, int beta)
        {
            if (cut(estado) || estado.gameIsOver())
            {
                return eval(estado);
            }

            int v = Int32.MaxValue;
            int temp;
            List<Jogada> jogadas = MaquinaDeRegras.PossiveisMovimentos(estado);
            foreach (var jogada in jogadas)
            {
                //MIN(v,max_value(result(s,a),alfa,beta))
                temp = max_value(Estado.result(estado, jogada), alfa, beta);
                if (v > temp)
                {
                    v = temp;
                }

                if (v <= alfa)
                {
                    return v;
                }

                //MIN(beta,v)
                if (beta > v)
                {
                    beta = v;
                }
            }
            return v;
        }

        public int eval(Estado estado)
        {
            switch (this.tipo)
            {
                case 1:
                    return eval1(estado);
                case 2:
                    return eval2(estado);
                default:
                    return eval1(estado);
            }
        }

        public bool cut(Estado estado)
        {
            switch (this.tipo)
            {
                case 1:
                    return cutoff_test(estado);
                case 2:
                    return cutoff_test(estado);
                default:
                    return false;
            }
        }

        
        //fun��o de avaliacao sobre a quantidade de movimentos possiveis
        //retorna a qtd de movimentos possiveis
        public static int evalMobility(Estado estado)
        {
            List<Jogada> jogadas = MaquinaDeRegras.PossiveisMovimentos(estado);
            return jogadas.Count;
        }

        //funcao de avaliacao que considera a quantidade de pecas e o lugar onde estao
        //cada peca tem um valor associado peao, peao perto de virar dama e dama
        //cada posicao do tabuleiro tem um valor associado
        public static int evalMaterial(Estado estado)
        {
            int i, j, peca, valorPeca, valorPos, sum1 = 0, sum2 = 0;
            for(i=0; i<8; i++){
                for(j=0; j<8; j++){
                    peca = estado.tabuleiro[i, j];
                    valorPos = valoracao[i, j];
                    
                    if(Tipos.isDama(peca)){
                        valorPeca = 10;
                    }
                    else if(((i == 1) && Tipos.isJogador1(peca)) || ((i == 6) && Tipos.isJogador2(peca))){
                        valorPeca = 7;
                    }
                    else{
                        valorPeca = 5;
                    }

                    if (Tipos.isJogador1(peca)){ 
                        sum1 += valorPeca * valorPos;
                    }
                    else if(Tipos.isJogador2(peca)){
                        sum2 += valorPeca * valorPos;
                    }
                }
            }

            if(estado.jogadorAtual == 1){
                return sum1 - sum2;
            }
            else{
                return sum2 - sum1;
            }
        }

        public bool cutoff_test(Estado estado)
        {
            return (estado.contaJogadas - GameController.instance.estadoAtual.contaJogadas) > 4;
        }

        //funcao de avaliacao usando material e quantidade de jogadas
        public static int eval1(Estado estado)
        {
            int c1 = 50, c2 = 50;
            return (int)( (c1*evalMaterial(estado)) + (c2*evalMobility(estado)) );
        }

        public static int eval2(Estado estado)
        {
            int c1 = 70, c2 = 30;
            return (int)( (c1 * evalMaterial(estado)) + (c2 * evalMobility(estado)) );
        }
    }
}