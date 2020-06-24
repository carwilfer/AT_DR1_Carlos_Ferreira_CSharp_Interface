using System;
using System.Collections.Generic;
using ATCsharpAniversario.Dominio;
using System.Linq;
using static System.String;

namespace ATCsharpAniversario.Cli
{
    public class BancoDeDadosEmMemoria : IBancoDeDados
    {
        private static readonly List<Pessoa> Pessoas = new List<Pessoa>();

        public string GerarId()
        {
            var hoje = DateTime.Today;
            var id = (Pessoas.Count + 1);
            return $"{hoje.Month}-{hoje.Day}-{Pessoas.Count + id}";
        }

        public List<Pessoa> BuscarTodasPessoas()
        {
            return Pessoas;
        }

        public int BuscarTotalDePessoa()
        {
            return Pessoas.Count;
        }

        public Pessoa BuscarPessoaMaisNova()
        {
            var pessoaMaisNova = ConsultasSobrePessoas.BuscarPessoaMaisNova(Pessoas);
            return pessoaMaisNova;
        }

        public PessoasComMesmoNome ObterPessoasComMesmoNome()
        {
            return ConsultasSobrePessoas.ObterPessoasComMesmoNome(Pessoas);
        }

        public void Salvar(Pessoa pessoa)
        {
            if (IsNullOrWhiteSpace(pessoa.Id))
                pessoa.Id = GerarId();

            var p = Pessoas.FirstOrDefault(x => x.Id == pessoa.Id);

            if (p != null)
                Pessoas.Remove(p);

            Pessoas.Add(pessoa);
        }

        public Pessoa BuscarPessoaPeloId(string id)
        {
            return Pessoas.FirstOrDefault(x => x.Id == id);
        }

        public void Editar(Pessoa pessoaNova)
        {

        }

        public void Excluir(Pessoa pessoa)
        {

        }

        public IEnumerable<Pessoa> BuscarTodosOsAniversariantes(DateTime dataNascimento)
        {
            var aniversariantes = new List<Pessoa>();
            return aniversariantes;
        }
    }
}
