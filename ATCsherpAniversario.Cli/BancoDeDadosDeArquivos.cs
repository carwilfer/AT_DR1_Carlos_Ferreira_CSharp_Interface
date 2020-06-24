using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.String;
using System.Text;
using ATCsharpAniversario.Dominio;

namespace ATCsharpAniversario.Cli
{
    public class BancoDeDadosDeArquivos : IBancoDeDados
    {
        public string GerarId()
        {
            var hoje = DateTime.Today;
            var id = (BuscarTodasPessoas().Count + 1);
            return $"{hoje.Month}-{hoje.Day}-{BuscarTodasPessoas().Count + id}";
        }

        public int BuscarTotalDePessoa()
        {
            return BuscarTodasPessoas().Count;
        }

        public Pessoa BuscarPessoaMaisNova()
        {
            return ConsultasSobrePessoas.BuscarPessoaMaisNova(BuscarTodasPessoas());
        }

        public PessoasComMesmoNome ObterPessoasComMesmoNome()
        {
            return ConsultasSobrePessoas.ObterPessoasComMesmoNome(BuscarTodasPessoas());
        }

        public void Salvar(Pessoa pessoa)
        {
            if (IsNullOrWhiteSpace(pessoa.Id))
                pessoa.Id = GerarId();

            var p = BuscarTodasPessoas().FirstOrDefault(x => x.Id == pessoa.Id);

            if (p != null)
                BuscarTodasPessoas().Remove(p);

            string nomeDoArquivo = ObterNomeArquivo();

            string formato = $"{pessoa.Id},{pessoa.Cpf},{pessoa.Nome},{pessoa.SobreNome},{pessoa.DataNascimento};";

            File.AppendAllText(nomeDoArquivo, formato);
        }

        public Pessoa BuscarPessoaPeloId(string id)
        {
            return BuscarTodasPessoas().FirstOrDefault(x => x.Id == id);
        }

        public List<Pessoa> BuscarTodasPessoas()
        {
            string nomeDoArquivo = ObterNomeArquivo();

            FileStream arquivo;
            if (!File.Exists(nomeDoArquivo))
            {
                arquivo = File.Create(nomeDoArquivo);
                arquivo.Close();
            }

            string resultado = File.ReadAllText(nomeDoArquivo);

            string[] pessoas = resultado.Split(';');

            var pessoasList = new List<Pessoa>();

            for (int i = 0; i < pessoas.Length - 1; i++)
            {
                string[] dados = pessoas[i].Split(',');

                var id = dados[0];
                var cpf = dados[1];
                var nome = dados[2];
                var sobreNome = dados[3];
                var dataDeNascimento = Convert.ToDateTime(dados[4]);

                var pessoa = new Pessoa
                {
                    Nome = nome,
                    SobreNome = sobreNome,
                    Cpf = cpf,
                    DataNascimento = dataDeNascimento,
                    Id = id
                };

                pessoasList.Add(pessoa);
            }

            return pessoasList;
        }

        private string ObterNomeArquivo()
        {
            var pastaDesktop = Environment.SpecialFolder.Desktop;

            string localDaPastaDesktop = Environment.GetFolderPath(pastaDesktop);
            string nomeDoArquivo = @"\repositorio.txt";
            if (!(File.Exists(localDaPastaDesktop + nomeDoArquivo)))
            {
                File.Create(localDaPastaDesktop + nomeDoArquivo).Close();
            }

            return localDaPastaDesktop + nomeDoArquivo;
        }

        public void Editar(Pessoa pessoaNova)
        {
            var todasPessoas = BuscarTodasPessoas();
            List<Pessoa> listaNova = new List<Pessoa>();

            foreach (var pessoa in todasPessoas)
            {
                if (pessoaNova.Id == pessoa.Id)
                {
                    listaNova.Add(pessoaNova);
                }
                else
                {
                    listaNova.Add(pessoa);
                }
            }

            RecriarArquivo(listaNova);
        }

        public void Excluir(Pessoa pessoa)
        {
            {
                var todasPessoas = BuscarTodasPessoas();
                List<Pessoa> listaNova = new List<Pessoa>();

                foreach (var pessoaExcluindo in todasPessoas)
                {
                    if (pessoa.Nome != pessoaExcluindo.Nome)
                    {
                        listaNova.Add(pessoaExcluindo);
                    }
                }

                RecriarArquivo(listaNova);
            }

        }

        public void RecriarArquivo(List<Pessoa> listaNova)
        {
            string nomeDoArquivo = ObterNomeArquivo();
            File.Delete(nomeDoArquivo);
            FileStream arquivo;
            if (!File.Exists(nomeDoArquivo))
            {
                arquivo = File.Create(nomeDoArquivo);
                arquivo.Close();
            }

            foreach (var pessoa in listaNova)
            {
                Salvar(pessoa);
            }
        }

        public IEnumerable<Pessoa> BuscarTodosOsAniversariantes(DateTime dataNascimento) 
        {
            var todasPessoas = BuscarTodasPessoas();
            List<Pessoa> pessoasEncontradas = new List<Pessoa>();
            return todasPessoas.ToList()
                .Where(
                    gente => gente.DataNascimento.Day == dataNascimento.Day
                    && gente.DataNascimento.Month == dataNascimento.Month
                );
        }

    }
}
