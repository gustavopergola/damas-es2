using UnityEngine;


namespace TiposNS
{
	public class Tipos : MonoBehaviour {

		public const int vazio = 0;
		public const int pecaComum = 1;
		public const int dama = 2;
		private const int mod = 2;

		public static int getPecaJogador1(){
			return Tipos.pecaComum;
		}
		
		public static int getDamaJogador1(){
			return Tipos.dama;
		}
		public static int getPecaJogador2(){
			return Tipos.pecaComum + mod;
		}

		public static int getDamaJogador2(){
			return Tipos.dama + mod;
		}

		public static bool isJogador1(int peca){
			return peca == Tipos.getPecaJogador1() || peca == Tipos.getDamaJogador1();
		}

		public static bool isJogador2(int peca){
			return peca == Tipos.getPecaJogador2() || peca == Tipos.getDamaJogador2();
		}

		public static bool isJogadorX(int peca, int jogador){
			return jogador == 1 ? isJogador1(peca) : isJogador2(peca);
		}

		public static bool isPecaComum(int peca){
			return peca == Tipos.getPecaJogador1() || peca == Tipos.getPecaJogador2();
		}

		public static bool isDama(int peca){
			return peca == Tipos.getDamaJogador1() || peca == Tipos.getDamaJogador2();
		}

		public static int getPecaJogadorX(int peca, int jogador){
			if(peca == dama){
				return jogador == 1 ? getDamaJogador1() : getDamaJogador2();
			}
			return jogador == 1 ? getPecaJogador1() : getPecaJogador2();
		}

		public static bool isVazio(int peca){
			return peca == Tipos.vazio;
		}
	}
}