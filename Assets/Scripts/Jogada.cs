using System.Collections;
using System.Collections.Generic;
public class Jogada
{
    public List<int[]> movimentos;
    public List<int[]> pecasComidas;
    bool virouDama;

    public Jogada()
    {
        this.movimentos = new List<int[]>();
        this.pecasComidas = new List<int[]>();
        this.virouDama = false;
    }
}