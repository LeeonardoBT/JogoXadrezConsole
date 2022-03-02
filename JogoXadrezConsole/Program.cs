using System;
using tabuleiro;
using xadrez;

namespace JogoXadrezConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab = new Tabuleiro(8, 8);

            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 5));
            tab.colocarPeca(new Rei(tab, Cor.Preta), new Posicao(3, 4));

            Tela.imprimirTabuleiro(tab);
        }
    }
}
