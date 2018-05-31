using System;
using System.Collections;
using System.Collections.Generic;
using EstadoNS;

namespace IANS
{
    public class IA
    {
        int jogadorId;
        int tipo;
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

            List<Jogada> jogadas = MaquinaDeRegras.instance.PossiveisMovimentos(estado);
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
            List<Jogada> jogadas = MaquinaDeRegras.instance.PossiveisMovimentos(estado);
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
            List<Jogada> jogadas = MaquinaDeRegras.instance.PossiveisMovimentos(estado);
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

        /*
        //função de avaliação sobre a quantidade de movimentos possiveis
        //retorna a qtd de movimentos possiveis
        public static int evalMobility(State state)
        {
            LinkedList<int[]> moves = RuleMachine.possible_moves(state);
            return moves.Count;
        }
        //função de avaliação do controle do centro do tabuleiro
        public static int evalCenterControl(State state)
        {
            int count = 0;
            char piece;
            for (int i = 2; i <= 3; i++)
            {
                for (int j = 2; j <= 3; j++)
                {
                    if (Program.getCurrentPlayer() == 1)
                    {
                        piece = state.board[i, j];
                        if (!(piece == '0'))
                        {
                            if (Char.IsUpper(piece))
                            {
                                count += 1;
                            }
                            else
                            {
                                count -= 1;
                            }
                        }
                    }
                    else
                    {
                        piece = state.board[i, j];
                        if (!(piece == '0'))
                        {
                            if (Char.IsLower(piece))
                            {
                                count += 1;
                            }
                            else
                            {
                                count -= 1;
                            }
                        }
                    }
                }
            }
            return count;
        }

        //função de avaliação simples
        public static int evalMaterial(State state)
        {
            int p = 0, b = 0, t = 0, q = 0, eval = 0;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    switch (state.board[i, j])
                    {
                        case Types.PAWN2:
                            p -= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.PAWN:
                            p += Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.BISHOP2:
                            b -= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.BISHOP:
                            b += Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.ROOK2:
                            t -= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.ROOK:
                            t += Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.QUEEN2:
                            q -= Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                        case Types.QUEEN:
                            q += Program.getCurrentPlayer() == 1 ? 1 : -1;
                            break;
                    }
                }
            }
            eval = p + 3 * b + 5 * t + 9 * q;
            return eval;
        }
        */

        public bool cutoff_test(Estado estado)
        {
            //return estado.contaJogadas - Program.currentState.playsCount > 4;
            return true;
        }
        //funcao de avaliacao usando material e quantidade de jogadas
        public static int eval1(Estado estado)
        {
            /*
            if (state.checkDraw())
            {
                return 0;
            }

            int p = 0, b = 0, t = 0, q = 0, util = 0, winner = -1;

            util = evalMaterial(state);
            winner = Program.getWinner(state);

            if ((Program.getCurrentPlayer()) == 1) util += winner < 2 ? -100 : 100;
            else util += winner == 2 ? 100 : -100;

            util += p + 3 * b + 5 * t + 9 * q;
            util = (int)util / (state.playsCount);
            return util;
            */
            return 1;
        }

        public static int eval2(Estado estado)
        {
            /*
            int c1 = 25, c2 = 35, c3 = 40;
            return (int)((c1 * evalCenterControl(state)) + (c2 * evalMaterial(state)) + (c3 * evalMobility(state))) / 100;
            */
            return 2;
        }
    }
}