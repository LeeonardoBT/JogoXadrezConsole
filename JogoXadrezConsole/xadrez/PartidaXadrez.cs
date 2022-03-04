using System.Collections.Generic;
using tabuleiro;

namespace xadrez
{
    class PartidaXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> pecasCapturadas;
        public bool xeque { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Vermelho;
            terminada = false;
            pecas = new HashSet<Peca>();
            pecasCapturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQtdeMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);

            if(pecaCapturada != null)
            {
                pecasCapturadas.Add(pecaCapturada);
            }

            return pecaCapturada;
        }

        public void defazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdeMovimentos();

            if(pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                pecasCapturadas.Remove(pecaCapturada);
            }

            tab.colocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                defazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            turno++;
            alteraJogador();
        }

        public void validarPosicaoOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }

            if(tab.peca(pos).cor != jogadorAtual)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }

            if (!tab.peca(pos).existeMovimentosPossíveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void validarPosicaoDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        public void alteraJogador()
        {
            if (jogadorAtual == Cor.Vermelho)
            {
                jogadorAtual = Cor.Azul;
            }
            else
            {
                jogadorAtual = Cor.Vermelho;
            }
        }

        public HashSet<Peca> pecasCapturadasCor(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach(Peca x in pecasCapturadas)
            {
                if(x.cor == cor)
                {
                    aux.Add(x);
                }
            }

            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }

            aux.ExceptWith(pecasCapturadasCor(cor));

            return aux;
        }

        private Cor adversaria(Cor cor)
        {
            return cor == Cor.Vermelho ? Cor.Azul : Cor.Vermelho;
        }

        private Peca rei(Cor cor)
        {
            foreach(Peca x in pecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            
            return null;
        }

        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);

            if(R == null)
            {
                throw new TabuleiroException($"Não tem rei da cor {cor} no tabuleiro!");
            }

            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();

                if(mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }

            return false;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('b', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('d', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Vermelho));
            colocarNovaPeca('f', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('g', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Vermelho));

            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('a', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('b', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('c', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('d', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('e', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('f', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('g', 7).toPosicao());
            //tab.colocarPeca(new Peao(tab, Cor.Vermelho), new PosicaoXadrez('h', 7).toPosicao());

            colocarNovaPeca('a', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('b', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('c', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('d', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Azul));
            colocarNovaPeca('f', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('g', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Azul));

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
