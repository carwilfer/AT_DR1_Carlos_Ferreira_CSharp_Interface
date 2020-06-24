using System;
using System.Linq;
using System.Collections.Generic;

namespace ATCsharpAniversario.Dominio
{
    public static class ConsultasSobrePessoas
    {
        public static Pessoa BuscarPessoaMaisNova(List<Pessoa> pessoas)
        {
            var maiorDataDeNascimento = pessoas.Max(x => x.DataNascimento);
            return pessoas.First(x => x.DataNascimento == maiorDataDeNascimento);
        }

        public static PessoasComMesmoNome ObterPessoasComMesmoNome(List<Pessoa> pessoas)
        {
            var pessoasComMesmoNome = pessoas
                .GroupBy(x => x.Nome )
                .Where(x => x.Count() > 1)
                .Select(x => new PessoaComMesmoNome
                {
                    Nome = x.Key,
                    Quantidade = x.Count()
                });

            return new PessoasComMesmoNome()
            {
                Pessoas = pessoasComMesmoNome,
                Total = pessoasComMesmoNome.Sum(x => x.Quantidade)
            };
        }
    }

    public class PessoasComMesmoNome
    {
        public IEnumerable<PessoaComMesmoNome> Pessoas { get; set; }
        public PessoaComMesmoNome PessoaDeNome(string nome)
        {
            return Pessoas.Single(x => x.Nome == nome);
        }
        public int Total { get; set; }
    }

    public class PessoaComMesmoNome
    {
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public int Quantidade { get; set; }
    }
}

