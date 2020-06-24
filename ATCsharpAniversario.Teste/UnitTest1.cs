using System;
using Xunit;
using System.Collections.Generic;
using ATCsharpAniversario.Dominio;

namespace ATCsharpAniversario.Teste
{
    public class UnitTest1
    {
        [Fact]
        public void ConsultarPessoaMaisNova()
        {
            var pessoa1 = new Pessoa() { DataNascimento = new DateTime(2016, 11, 5) };
            var pessoa2 = new Pessoa() { DataNascimento = new DateTime(1981, 05, 14) };
            var pessoa3 = new Pessoa() { DataNascimento = new DateTime(1980, 09, 5) };

            var pessoas = new List<Pessoa>
            {
                pessoa1,
                pessoa2,
                pessoa3

            };

            var pessoaEncontrada = ConsultasSobrePessoas.BuscarPessoaMaisNova(pessoas);
            Assert.Same(pessoa1, pessoaEncontrada);
        }
    }
}
