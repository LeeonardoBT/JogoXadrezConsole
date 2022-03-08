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
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Vermelho;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
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

            if (pecaCapturada != null)
            {
                pecasCapturadas.Add(pecaCapturada);
            }

            // #JogadaEspecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);

                Peca T = tab.retirarPeca(origemTorre);
                T.incrementarQtdeMovimentos();
                tab.colocarPeca(T, destinoTorre);
            }

            // #JogadaEspecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);

                Peca T = tab.retirarPeca(origemTorre);
                T.incrementarQtdeMovimentos();
                tab.colocarPeca(T, destinoTorre);
            }

            // #JogadaEspecial en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;

                    if (p.cor == Cor.Vermelho)
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }

                    pecaCapturada = tab.retirarPeca(posP);
                    pecasCapturadas.Add(pecaCapturada);
                }
            }

            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decrementarQtdeMovimentos();

            if(pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                pecasCapturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #JogadaEspecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);

                Peca T = tab.retirarPeca(destinoTorre);
                T.decrementarQtdeMovimentos();
                tab.colocarPeca(T, origemTorre);
            }

            // #JogadaEspecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);

                Peca T = tab.retirarPeca(destinoTorre);
                T.decrementarQtdeMovimentos();
                tab.colocarPeca(T, origemTorre);
            }

            // #JogadaEspecial en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tab.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Vermelho)
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    tab.colocarPeca(peao, posP);
                }
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);

            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            Peca p = tab.peca(destino);

            // #JogadaEspecial promocao
            if (p is Peao)
            {
                if ((p.cor == Cor.Vermelho && destino.linha == 7) || (p.cor == Cor.Azul && destino.linha == 0))
                {
                    p = tab.retirarPeca(destino);
                    pecas.Remove(p);

                    Peca dama = new Dama(tab, p.cor);
                    tab.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                alteraJogador();
            }

            // #JogadaEspecial en passant
            if (p is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vulneravelEnPassant = p;
            }
            else
            {
                vulneravelEnPassant = null;
            }
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
            if (!tab.peca(origem).movimentoPossivel(destino))
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

            if (R == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro!");
            }

            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();

                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }

            foreach(Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();

                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.linhas; j++)
                    {
                        if(mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(x.posicao, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);

                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Vermelho));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Vermelho));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Vermelho));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Vermelho));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Vermelho, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Vermelho));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Vermelho));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Vermelho));

            //colocarNovaPeca('a', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('b', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('c', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('d', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('e', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('f', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('g', 7, new Peao(tab, Cor.Vermelho, this));
            //colocarNovaPeca('h', 7, new Peao(tab, Cor.Vermelho, this));

            colocarNovaPeca('a', 1, new Torre(tab, Cor.Azul));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Azul));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Azul));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Azul));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Azul, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Azul));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Azul));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Azul));

            //colocarNovaPeca('a', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('b', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('c', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('d', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('e', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('f', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('g', 2, new Peao(tab, Cor.Azul, this));
            //colocarNovaPeca('h', 2, new Peao(tab, Cor.Azul, this));
        }
    }
}
