using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        public Rei(Tabuleiro tab, Cor cor) : base(tab, cor)
        {
        }

        private bool podeMover(Posicao pos)
        {
            Peca p = tab.peca(pos);

            return p == null || p.cor != cor;
        }

        public override bool[,] movimentosPossiveis()
        {
            bool[,] mat = new bool[tab.linhas, tab.colunas];

            Posicao pos = new Posicao(0, 0);

            // o Rei apenas pode movimentar uma casa, para qualquer lado
            for(int i = posicao.linha - 1; i <= posicao.linha + 1; i++)
            {
                for (int j = posicao.coluna - 1; j <= posicao.coluna + 1; j++)
                {
                    if (!(posicao.linha == i && posicao.coluna == j))
                    {
                        pos.definirValores(i, j);

                        if (tab.posicaoValida(pos) && podeMover(pos))
                        {
                            mat[pos.linha, pos.coluna] = true;
                        }
                    }
                }
            }

            return mat;
        }

        public override string ToString()
        {
            return "R";
        }
    }
}
