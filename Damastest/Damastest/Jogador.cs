using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Damastest
{
    public class Jogador
    {

        public int layerMaskValue;
        private string nome;
        private int numero;
        private IA ia;

        private static int jogadores = 1;

        public Jogador(string nome, IA ia = null, int layerMaskValue = 999)
        {
            this.nome = nome;
            this.numero = jogadores;
            this.ia = ia;
            this.layerMaskValue = layerMaskValue;
            jogadores = 2;//máximo de 2 jogadores
        }

        public bool isPlayer()
        {
            return this.ia == null;
        }

        public bool isIA()
        {
            return !this.isPlayer();
        }

        public string getNomeJogador()
        {
            return this.nome;
        }

        public void callAIAction()
        {
            this.ia.getAction();
        }
        public int getNumeroJogador()
        {
            return this.numero;
        }
    }
}
