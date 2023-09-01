using System;
using System.Collections;

namespace Paradigma
{
    internal class Program
    {
        public char[,] array;
        public arvore arvore;

        static void Main(string[] args)
        {
            new Program().tarefa1();

        }

        public void tarefa1()
        {
            array = new char[,]{
                { 'A','B' } ,
                { 'A','C' },
                { 'B','G' },
                { 'C','H' },
                { 'E','F' },
                { 'B','D' },
                { 'C','E' },
            };

            //array = new char[,]{
            //    { 'B','D' } ,
            //    { 'D','E' },
            //    { 'A','B' },
            //    { 'C','F' },
            //    { 'E','G' },
            //    { 'A','C' },
            //};

            //array = new char[,]{
            //    { 'A','B' } ,
            //    { 'A','C' },
            //    { 'B','D' },
            //    { 'D','C' },
            //};

            arvore = new arvore();
            var folhasSoltas = new ArrayList();

            for (int i = 0; i < array.Length / 2; i++)
            {
                var no = array[i, 0];
                var folhaFilho = new folha(array[i, 1]);

                if (arvore.temFolha(arvore.folha, new folha(no)))
                    arvore.folha = arvore.inserir(arvore.folha, no, folhaFilho);
                else
                {
                    folhasSoltas.Add(new folha(no, folhaFilho));
                }
            }

            int contador = 5;//evitar loop infinito
            verificaSeTemFolhasSoltas(folhasSoltas, contador);

            MostraResultado(arvore.folha);

            Console.WriteLine();
            Console.WriteLine("-------------------------------");
            Console.WriteLine("Aperte qualquer tecla para sair");
            Console.ReadKey();

        }

        private void MostraResultado(folha folha)
        {
            Console.Write(folha.no);
            if (folha.filhoEsq != null)
            {
                Console.Write("[");
                MostraResultado(folha.filhoEsq);
                Console.Write("]");
            }
            if (folha.filhoDir != null)
            {
                Console.Write("[");
                MostraResultado(folha.filhoDir);
                Console.Write("]");
            }

        }

        private void verificaSeTemFolhasSoltas(ArrayList folhasSoltas, int contador)
        {
            //se tem folhas soltas
            var arrayTemp = new ArrayList();
            contador--;

            foreach (folha item in folhasSoltas)
            {
                if (arvore.temFolha(arvore.folha, new folha(item.no)) || arvore.temFolha(arvore.folha, new folha(item.filhoEsq.no)))
                {
                    arvore.folha = arvore.inserir(arvore.folha, item.no, item.filhoEsq);
                }
                else
                {
                    arrayTemp.Add(item);
                }
            }
            if (arrayTemp.Count > 0 && contador > 0)
                verificaSeTemFolhasSoltas(arrayTemp, contador);
        }

    }
    public class arvore
    {
        public folha folha;
        public arvore()
        {
            this.folha = new folha();
        }

        internal folha inserir(folha arvoreFolha, char no, folha folhaFilho)
        {

            if (no == folhaFilho.no)
            {
                throw new Exception("E2 - Ciclo presente");
            }
            if (arvoreFolha.no == 0)//raiz
            {
                return new folha(no, folhaFilho);
            }
            else if (arvoreFolha.no == no)//encontrou o nó
            {
                if (!temPai(this.folha, folhaFilho.no))
                {
                    return new folha(arvoreFolha, folhaFilho);
                }
                else//ja tem pai
                    throw new Exception("E3 - Raízes múltiplas");

            }
            else if (arvoreFolha.no == folhaFilho.no)//o nó filho tem um novo pai
            {
                var arvoreTemp = new arvore();
                arvoreTemp.folha = new folha(no, arvoreFolha);
                arvoreFolha = arvoreTemp.folha;
                return arvoreFolha;
            }

            if (arvoreFolha.filhoEsq != null)
            {
                arvoreFolha.filhoEsq = inserir(arvoreFolha.filhoEsq, no, folhaFilho);
            }
            if (arvoreFolha.filhoDir != null)
            {
                arvoreFolha.filhoDir = inserir(arvoreFolha.filhoDir, no, folhaFilho);
            }
            return arvoreFolha;

        }


        public bool temPai(folha folhaArvore, char no)
        {
            if (this.folha.no == no || folhaArvore == null)//raiz
            {
                return false;
            }

            if (this.folha.filhoEsq?.no == no || this.folha.filhoDir?.no == no)
            {
                return true;
            }
            else
            {
                folhaArvore = folhaArvore?.filhoEsq;
                var tempTemPai = temPai(folhaArvore, no);
                if (tempTemPai)
                    return true;

                folhaArvore = folhaArvore?.filhoDir;
                tempTemPai = temPai(folhaArvore, no);
                if (tempTemPai)
                    return true;
            }
            return false;
        }

        public bool temFolha(folha folhaArvore, folha folha)
        {
            try
            {
                if (folhaArvore.no == 0)
                {
                    return true;
                }
                else if (folhaArvore.no == folha.no)
                {
                    return true;
                }

                if (folhaArvore.filhoEsq != null)
                {
                    var encontrou = temFolha(folhaArvore.filhoEsq, folha);
                    if (encontrou)
                        return true;
                }
                if (folhaArvore.filhoDir != null)
                {
                    var encontrou = temFolha(folhaArvore.filhoDir, folha);
                    if (encontrou)
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw new Exception("E4 - Qualquer outro erro");
            }
        }
    }
    public class folha
    {
        public char no;
        public folha filhoEsq;
        public folha filhoDir;

        public folha() { }

        public folha(char no, folha filho)
        {
            try
            {
                this.no = no;
                if (this.filhoEsq == null)
                    this.filhoEsq = filho;
                else if (this.filhoDir == null)
                    this.filhoDir = filho;
                else
                    throw new Exception("E1 - Mais de 2 filhos");
            }
            catch (Exception)
            {
                throw new Exception("E4 - Qualquer outro erro");
            }
        }
        public folha(folha pai, folha filho)
        {
            this.no = pai.no;
            this.filhoEsq = pai.filhoEsq;
            this.filhoDir = pai.filhoDir;

            if (pai.filhoEsq == null)
                this.filhoEsq = filho;
            else if (this.filhoDir == null)
                this.filhoDir = filho;
            else
                throw new Exception("E1 - Mais de 2 filhos");

        }
        public folha(char no)
        {
            this.no = no;
            this.filhoEsq = null;
            this.filhoDir = null;
        }


    }
}
