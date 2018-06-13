using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Damastest;


namespace Damastest.UnitTest
{
    [TestClass]
    public class JogadorTests
    {
        [TestMethod]
        public void DeveRetornarJogadorCorretamenteInicializado()
        {
            var jogador = new Jogador("nome");
            int lmv = jogador.layerMaskValue;
            int numero = jogador.getNumeroJogador();
            string nome = jogador.getNomeJogador();
            bool notia = jogador.isPlayer();

            Assert.AreEqual(999, lmv);
            Assert.AreEqual(true, notia);
            Assert.AreEqual(1, numero);
            Assert.AreEqual("nome", nome);  
        }
        [TestMethod]
        public void DeveRetornarQueEPlayer()
        {
            var jogador = new Jogador("");
            bool notia = jogador.isPlayer();

            Assert.AreEqual(true, notia);
        }
        [TestMethod]
        public void DeveRetornarQueEIA()
        {

        }
        [TestMethod]
        public void DeveRetornarNomeCorreto()
        {
            var jogador = new Jogador("nome");
            string nome = jogador.getNomeJogador();

            Assert.AreEqual("nome", nome);
        }
        [TestMethod]
        public void DeveRetornarNumeroDoJogadorCorreto()
        {
            var jogador = new Jogador("nome");
            int numero = jogador.getNumeroJogador();

            Assert.AreEqual(1, numero);
        }
    }
}
