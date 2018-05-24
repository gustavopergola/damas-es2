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
        this.virouDama = false;
    }

    public void Comeu(int x, int y)
    {
        if (this.pecasComidas == null)
            this.pecasComidas = new List<int[]>();
        this.pecasComidas.Add(new int[2] {x,y});
    }
}