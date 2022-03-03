﻿using System;
using tabuleiro;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Vermelho;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdeMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
        }

        private void colocarPecas()
        {
            tab.colocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('a', 8).toPosicao());
            tab.colocarPeca(new Cavalo(tab, Cor.Vermelho), new PosicaoXadrez('b', 8).toPosicao());
            tab.colocarPeca(new Bispo(tab, Cor.Vermelho), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new Rainha(tab, Cor.Vermelho), new PosicaoXadrez('d', 8).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Vermelho), new PosicaoXadrez('e', 8).toPosicao());
            tab.colocarPeca(new Bispo(tab, Cor.Vermelho), new PosicaoXadrez('f', 8).toPosicao());
            tab.colocarPeca(new Cavalo(tab, Cor.Vermelho), new PosicaoXadrez('g', 8).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Vermelho), new PosicaoXadrez('h', 8).toPosicao());

            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('a', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('b', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('c', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('d', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('e', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('f', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('g', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('h', 7).toPosicao());

            tab.colocarPeca(new Torre(tab, Cor.Azul), new PosicaoXadrez('a', 1).toPosicao());
            tab.colocarPeca(new Cavalo(tab, Cor.Azul), new PosicaoXadrez('b', 1).toPosicao());
            tab.colocarPeca(new Bispo(tab, Cor.Azul), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new Rainha(tab, Cor.Azul), new PosicaoXadrez('d', 1).toPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Azul), new PosicaoXadrez('e', 1).toPosicao());
            tab.colocarPeca(new Bispo(tab, Cor.Azul), new PosicaoXadrez('f', 1).toPosicao());
            tab.colocarPeca(new Cavalo(tab, Cor.Azul), new PosicaoXadrez('g', 1).toPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Azul), new PosicaoXadrez('h', 1).toPosicao());

            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('a', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('b', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('c', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('d', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('e', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('f', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('g', 2).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Azul), new PosicaoXadrez('h', 2).toPosicao());
        }
    }
}
