using System;
using System.Collections.Generic;
using ATCsharpAniversario.Dominio;
using System.Text;

namespace ATCsharpAniversario.Cli
{
    interface IBancoDeDados
    {
        Pessoa BuscarPessoaPeloId(string Id);
        void Salvar(Pessoa pessoa);
        PessoasComMesmoNome ObterPessoasComMesmoNome();
        Pessoa BuscarPessoaMaisNova();
        int BuscarTotalDePessoa();
        string GerarId();
        List<Pessoa> BuscarTodasPessoas();
        void Editar(Pessoa pessoa);

        void Excluir(Pessoa pessoa);
        public IEnumerable<Pessoa> BuscarTodosOsAniversariantes(DateTime dataNascimento);
    }
}
