using System;
using System.Collections.Generic;
using System.Text;

namespace ATCsharpAniversario.Dominio
{
    public class Pessoa
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataDeCadastro { get; set; }
    }
}
