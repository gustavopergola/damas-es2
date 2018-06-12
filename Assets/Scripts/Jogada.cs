using System.Collections;
using System.Collections.Generic;
public class Jogada
{
    public List<int[]> movimentos;
    public List<int[]> pecasComidas;
    public int[] posInicial;
    public bool virouDama;

    public Jogada(int[] posInicial = null)
    {
        this.movimentos = new List<int[]>();
        this.pecasComidas = new List<int[]>();
        this.posInicial = posInicial;
        this.virouDama = false;
    }

    public int[] ultimoMovimento()
    {
        return movimentos[movimentos.Count - 1];
    }
}