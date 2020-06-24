using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ATCsharpAniversario.Dominio;

namespace ATCsharpAniversario.Cli
{
    class InteracaoUsuario
    {
        private static void EscreverNaTela(string texto)
        {
            Console.WriteLine(texto);
        }
        private static void LimparTela()
        {
            Console.Clear();
        }

        private static readonly List<Pessoa> Pessoas = new List<Pessoa>();

        private static string ListaDeAniversarianteHoje(Pessoa pessoa)
        {
            string retorno;
            DateTime aniversarioAnoCorrente = new DateTime(DateTime.Now.Year, pessoa.DataNascimento.Month, pessoa.DataNascimento.Day);
            if (aniversarioAnoCorrente.Date > DateTime.Now.Date)
            {
                TimeSpan ts = aniversarioAnoCorrente.Date - DateTime.Now.Date;
                retorno = string.Format("Faltam {0} dias para seu aniversário.", ts.Days);
            }
            else
            {
                DateTime aniversarioProximoAno = new DateTime(DateTime.Now.Year + 1, pessoa.DataNascimento.Month, pessoa.DataNascimento.Day);
                TimeSpan ts = aniversarioProximoAno.Date - DateTime.Now.Date;
                retorno = string.Format("Faltam {0} dias para seu aniversário.", ts.Days);
            }

            return retorno;
        }
        private static void ExibirAniversariantesDoDia()
        {
            var aniversarianteFiltradosPelaDataNascimento = BancoDeDados.BuscarTodosOsAniversariantes(DateTime.Now.Date);

            if (aniversarianteFiltradosPelaDataNascimento.Count() > 0)
            {
                EscreverNaTela("Feliz aniversário!");
                foreach (var gente in aniversarianteFiltradosPelaDataNascimento)
                {
                    EscreverNaTela(gente.Nome + " " + gente.SobreNome);
                }
            }
        }

        private static void Salvar(Pessoa pessoa)
        {
            Pessoas.Add(pessoa);
        }
        public static void MenuPrincipal()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            ExibirAniversariantesDoDia();

            Console.ForegroundColor = ConsoleColor.Red;
            EscreverNaTela("Menu do sistema de gerência de Aniversários:");
            EscreverNaTela("Selecione uma operação");
            EscreverNaTela("1 - Adicionar pessoa");
            EscreverNaTela("2 - Pesquisar pessoa");
            EscreverNaTela("3 - Editar pessoa");
            EscreverNaTela("4 - Operação excluir pessoaa");
            EscreverNaTela("0 - Sair");

            string operacao = Console.ReadLine();

            LimparTela();

            switch (operacao)
            {
                case "1":
                    OperacaoAdicionarPessoa(); break;

                case "2":
                    OperacaoPesquisarPessoa(); break;

                case "3":
                    OperacaoEditarPessoa(); break;

                case "4":
                    OperacaoExcluirPessoa(); break;

                default:
                    EscreverNaTela("Opção inexistente"); break;
            }
            EscreverNaTela("Pressione qualquer tecla para continuar");
            Console.ReadKey();
            LimparTela();
        }
        private static void OperacaoAdicionarPessoa()
        {
            LimparTela();

            EscreverNaTela("Entre com o Nome:");
            string nome = Console.ReadLine();

            EscreverNaTela("Entre com o Sobrenome:");
            string sobreNome = Console.ReadLine();

            DateTime niver;
            EscreverNaTela("Entre com a data de Nascimento no formato: DD/MM/YYYY");
            niver = DateTime.Parse(Console.ReadLine());

            DateTime dataDeCadastro = DateTime.Now;

            TimeSpan resultado;
            resultado = dataDeCadastro - niver;
            //DateTime idade = (New DateTime() + result).AddYears(-1).AddDays(-1);
            //return idade.Year;

            EscreverNaTela($"Você tem {(resultado.Days / 30 / 12) - 1} anos");

            string id = BancoDeDados.GerarId();

            var pessoa = new Pessoa
            {
                Nome = nome,
                SobreNome = sobreNome,
                DataNascimento = niver,
                DataDeCadastro = dataDeCadastro
            };

            BancoDeDados.Salvar(pessoa);

            EscreverNaTela("ID: " + id);

            EscreverNaTela("Cadastrado com sucesso!");
            EscreverNaTela("Pressione qualquer tecla para continuar");
            Console.ReadKey();
            LimparTela();

            MenuPrincipal();
        }
        private static void OperacaoPesquisarPessoa()
        {
            LimparTela();

            foreach (var pessoa in BancoDeDados.BuscarTodasPessoas())
            {
                EscreverNaTela($"id: {pessoa.Id} nome: {pessoa.Nome} sobreNome: {pessoa.SobreNome} cpf: {pessoa.Cpf}");
            }
            StatusAniversariante();
        }
        private static void OperacaoEditarPessoa()
        {
            LimparTela();
            EscreverNaTela("Favor informar o Id: ");
            string id = Console.ReadLine();

            var pessoa = BancoDeDados.BuscarPessoaPeloId(id);

            //remover erro por não encontrar aluno pela matrícula

            EscreverNaTela("Nome?");
            string nome = Console.ReadLine();

            EscreverNaTela("SobreNome?");
            string sobreNome = Console.ReadLine();

            EscreverNaTela("Cpf?");
            string cpf = Console.ReadLine();

            EscreverNaTela("Data de nascimento (dd/mm/aaaa)?");
            DateTime dataDeNascimento = DateTime.Parse(Console.ReadLine());

            pessoa.Id = id;
            pessoa.Nome = nome;
            pessoa.SobreNome = sobreNome;
            pessoa.Cpf = cpf;
            pessoa.DataNascimento = dataDeNascimento;

            BancoDeDados.Editar(pessoa);
            Console.ReadKey();
            LimparTela();
            MenuPrincipal();
        }

        private static void OperacaoExcluirPessoa()
        {
            EscreverNaTela("Favor informar o Id: ");
            string id = Console.ReadLine();

            var pessoa = BancoDeDados.BuscarPessoaPeloId(id);
            
            string excluir;
            if (pessoa == null)
            {
                EscreverNaTela("Cpf digitando incorretamente ou funcionario não encontrado");
                EscreverNaTela("Pressione qualquer tecla para continuar");
                Console.ReadKey();
                LimparTela();
                MenuPrincipal();
            }
            EscreverNaTela($"Nome: {pessoa.Nome} Sobre Nome: {pessoa.SobreNome}");
            BancoDeDados.Excluir(pessoa);

            MenuPrincipal();
        }

    

        static void StatusAniversariante()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            EscreverNaTela("Os dados cadastrados estão corretos? ");
            EscreverNaTela("1 - Operação consultar pessoa mais nova");
            EscreverNaTela("2 - Operação consultar pessoa com mesmo nome");
            EscreverNaTela("3 - Operação pesquisar quantidades de pessoa");
            EscreverNaTela("4 - Consultar pela data");


            string tipoConsulta = Console.ReadLine();


            switch (tipoConsulta)
            {
                case "1":
                    OperacaoPesquisarPessoaMaisNova(); break;

                case "2":
                    OperacaoPesquisarPessoaComMesmoNome(); break;

                case "3":
                    OperacaoPesquisarQuantidadesDePessoa(); break;

                case "4":
                    ConsultarPelaData(); break;

                default:
                    EscreverNaTela("Consulta incorreta");
                    MenuPrincipal();
                    break;
            }
        }
        private static void OperacaoPesquisarPessoaMaisNova()
        {
            Pessoa pessoa = BancoDeDados.BuscarPessoaMaisNova();
            EscreverNaTela("A pessoas mais nova é: " + pessoa.Nome + " " + pessoa.SobreNome);
            LimparTela();
            StatusAniversariante();
        }
        private static void OperacaoPesquisarPessoaComMesmoNome()
        {
            EscreverNaTela("Entre com o nome da pessoa: ");
            string nome = Console.ReadLine();
            EscreverNaTela("Entre com o sobrenome: ");
            string sobreNome = Console.ReadLine();

            var pessoaEncontrada = BancoDeDados.ObterPessoasComMesmoNome().Pessoas
                .Where(pessoa => pessoa.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)
                || pessoa.SobreNome.Contains(sobreNome, StringComparison.OrdinalIgnoreCase));

            int quantidadePessoaEncontrada = pessoaEncontrada.Count();

            if (quantidadePessoaEncontrada > 0)
            {
                EscreverNaTela("Pessoa encontrada");

                foreach (var pessoa in pessoaEncontrada)
                {
                    EscreverNaTela(pessoa.Nome + " " + pessoa.SobreNome);

                    Pessoa pessoaVerificar = new Pessoa();
                    pessoaVerificar.DataNascimento = pessoa.DataNascimento;

                    EscreverNaTela(VerificarProximoAniversario(pessoaVerificar));
                }

            }
            else
            {
                EscreverNaTela("Nenhuma pessoa encontrada para o nome digitado: " + nome + " " + sobreNome);
            }

            MenuPrincipal();
        }
        private static void OperacaoPesquisarQuantidadesDePessoa()
        {
            var totalDePessoas = BancoDeDados.BuscarTotalDePessoa();
            EscreverNaTela("Quantidade de pessoas cadastradas:" + totalDePessoas);
        }
        private static void ConsultarPelaData()
        {
            Console.WriteLine("Entre com a data");
            var dataNascimento = DateTime.Parse(Console.ReadLine());

            var filtroPessoaPelaDataNascimento = BancoDeDados.BuscarTodasPessoas().Where(pessoa => pessoa.DataNascimento.Date == dataNascimento);

            EscreverNaTela("Pessoa encontrada");

            foreach (var pessoa in filtroPessoaPelaDataNascimento)
            {
                Console.WriteLine(pessoa.Nome);
                Console.WriteLine(VerificarProximoAniversario(pessoa));
            }
            MenuPrincipal();
        }

        static string VerificarProximoAniversario(Pessoa pessoa)
        {
            string retorno;
            DateTime aniversarioAnoCorrente = new DateTime(DateTime.Now.Year, pessoa.DataNascimento.Month, pessoa.DataNascimento.Day);
            if (aniversarioAnoCorrente.Date > DateTime.Now.Date)
            {
                TimeSpan ts = aniversarioAnoCorrente.Date - DateTime.Now.Date;
                retorno = string.Format("Faltam {0} dias para seu aniversário.", ts.Days);
            }
            else
            {
                DateTime aniversarioProximoAno = new DateTime(DateTime.Now.Year + 1, pessoa.DataNascimento.Month, pessoa.DataNascimento.Day);
                TimeSpan ts = aniversarioProximoAno.Date - DateTime.Now.Date;
                retorno = string.Format("Faltam {0} dias para seu aniversário.", ts.Days);
            }

            return retorno;
            MenuPrincipal();
        }

        public static IBancoDeDados BancoDeDados = new BancoDeDadosDeArquivos();
    }
}
