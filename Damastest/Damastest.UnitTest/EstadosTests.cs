using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Damastest;
using UnityEngine;

namespace Damastest.UnitTest
{
    [TestClass]
    public class EstadosTests : MonoBehaviour
    {
        [TestMethod]
        public void DeveRetornarNaoFimDeJogo()
        {
            
            var tabuleiro = new Tabuleiro();
            var jogada = new Jogada();
            //var estado = new Estado(tabuleiro.matrizTabuleiroInt, );

        }
        [TestMethod]
        public void DeveRetornarOponenteCorreto()
        {

        }
        [TestMethod]
        public void DeveRetornarEstadoResultanteCorreto()
        {

        }
    }
}
