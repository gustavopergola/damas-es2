using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Damastest;

namespace Damastest.UnitTest
{
    [TestClass]
    public class TiposTests
    {
        [TestMethod]
        public void DeveRetornarPecaDoJogador1()
        {

            int peca = Tipos.getPecaJogador1();

            Assert.AreEqual(1, peca);
        }
        [TestMethod]
        public void DeveRetornarPecaDoJogador2()
        {

            int peca = Tipos.getPecaJogador2();

            Assert.AreEqual(3, peca);
        }
        [TestMethod]
        public void DeveRetornarDamaDoJogador1()
        {

            int peca = Tipos.getDamaJogador1();

            Assert.AreEqual(2, peca);
        }
        [TestMethod]
        public void DeveRetornarDamaDoJogador2()
        {

            int peca = Tipos.getDamaJogador2();

            Assert.AreEqual(4, peca);
        }
        [TestMethod]
        public void DeveRetornarJogador1()
        {

            int peca = Tipos.getDamaJogador1();
            bool j1 = Tipos.isJogador1(peca);

            Assert.AreEqual(true, j1);
        }
        [TestMethod]
        public void DeveRetornarFalseJogador1()
        {

            int peca = Tipos.getDamaJogador2();
            bool j1 = Tipos.isJogador1(peca);

            Assert.AreEqual(false, j1);
        }
        [TestMethod]
        public void DeveRetornarJogador2()
        {

            int peca = Tipos.getDamaJogador2();
            bool j1 = Tipos.isJogador2(peca);

            Assert.AreEqual(true, j1);
        }
        [TestMethod]
        public void DeveRetornarFalseJogador2()
        {

            int peca = Tipos.getDamaJogador1();
            bool j1 = Tipos.isJogador2(peca);

            Assert.AreEqual(false, j1);
        }
        [TestMethod]
        public void DeveRetornarJogadorXIgual1()
        {
           
            int peca = Tipos.getPecaJogador1();
            int isj = Tipos.jogador(peca);
            bool isJ1 = Tipos.isJogadorX(peca, isj);

            Assert.AreEqual(true, isJ1);
        }
        [TestMethod]
        public void DeveRetornarJogadorXIgual2()
        {
            
            int peca = Tipos.getPecaJogador2();
            int isj = Tipos.jogador(peca);
            bool isJ2 = Tipos.isJogadorX(peca, isj);

            Assert.AreEqual(true, isJ2);
        }
        [TestMethod]
        public void DeveRetornarQueAPecaEComum()
        {
            int peca1 = Tipos.getPecaJogador1();
            bool comum1 = Tipos.isPecaComum(peca1);
            int peca2 = Tipos.getPecaJogador2();
            bool comum2 = Tipos.isPecaComum(peca2);

            Assert.AreEqual(true, comum1);
            Assert.AreEqual(true, comum2);
        }
        [TestMethod]
        public void DeveRetornarQueAPecaNaoEComum()
        {
            int peca1 = Tipos.getDamaJogador1();
            bool comum1 = Tipos.isPecaComum(peca1);
            int peca2 = Tipos.getDamaJogador2();
            bool comum2 = Tipos.isPecaComum(peca2);

            Assert.AreEqual(false, comum1);
            Assert.AreEqual(false, comum2);
        }
        [TestMethod]
        public void DeveRetornarQueAPecaEDama()
        {
            int peca1 = Tipos.getDamaJogador1();
            bool comum1 = Tipos.isDama(peca1);
            int peca2 = Tipos.getDamaJogador2();
            bool comum2 = Tipos.isDama(peca2);

            Assert.AreEqual(true, comum1);
            Assert.AreEqual(true, comum2);
        }
        [TestMethod]
        public void DeveRetornarQueAPecaNaoEDama()
        {
            int peca1 = Tipos.getPecaJogador1();
            bool comum1 = Tipos.isDama(peca1);
            int peca2 = Tipos.getPecaJogador2();
            bool comum2 = Tipos.isDama(peca2);

            Assert.AreEqual(false, comum1);
            Assert.AreEqual(false, comum2);
        }
        [TestMethod]
        public void DeveRetornarPecaComumDoJogador1()
        {
            int peca = Tipos.getPecaJogador1();
            int isj = Tipos.jogador(peca);
            int pecaselecionada = Tipos.getPecaJogadorX(1, isj);

            Assert.AreEqual(1, pecaselecionada);
        }
        [TestMethod]
        public void DeveRetornarPecaComumDoJogador2()
        {
            int peca = Tipos.getPecaJogador2();
            int isj = Tipos.jogador(peca);
            int pecaselecionada = Tipos.getPecaJogadorX(1, isj);

            Assert.AreEqual(3, pecaselecionada);
        }
        [TestMethod]
        public void DeveRetornarADamaDoJogador1()
        {
            int peca = Tipos.getDamaJogador1();
            int isj = Tipos.jogador(peca);
            int pecaselecionada = Tipos.getPecaJogadorX(2, isj);

            Assert.AreEqual(2, pecaselecionada);
        }
        [TestMethod]
        public void DeveRetornarADamaDoJogador2()
        {
            int peca = Tipos.getDamaJogador2();
            int isj = Tipos.jogador(peca);
            int pecaselecionada = Tipos.getPecaJogadorX(2, isj);

            Assert.AreEqual(4, pecaselecionada);
        }
        [TestMethod]
        public void DeveRetornarQueEVazio()
        {
            int peca = Tipos.vazio;
            bool vazio = Tipos.isVazio(peca);

            Assert.AreEqual(true, vazio);
        }
        [TestMethod]
        public void DevePegarJogador1()
        {
            int peca = Tipos.getPecaJogador1();
            int jogador = Tipos.pegaJogador(peca);

            Assert.AreEqual(1, jogador);
        }
        [TestMethod]
        public void DevePegarJogador2()
        {
            int peca = Tipos.getPecaJogador2();
            int jogador = Tipos.pegaJogador(peca);

            Assert.AreEqual(2, jogador);
        }

    }
}
