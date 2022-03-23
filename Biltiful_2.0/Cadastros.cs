using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biltiful_2._0
{

    public class MenuCadastros
    {

        public static void SubMenuClientes()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== CLIENTES ===============");
                Console.WriteLine("1. Cadastar cliente");
                Console.WriteLine("2. Listar clientes");
                Console.WriteLine("3. Editar registro de cliente");
                Console.WriteLine("4. Bloquear/Desbloqueia cliente (Inadimplente)");
                Console.WriteLine("5. Localizar cliente");
                Console.WriteLine("6. Localizar cliente bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "0":
                        break;

                    case "1":
                        NovoCliente();
                        break;

                    case "2":
                        ListarClientes();
                        break;

                    case "3":
                        EditarCliente();
                        break;

                    case "4":
                        BloqueiaCliente();

                        break;

                    case "5":
                        LocalizaCliente();
                        break;

                    case "6":
                        LocalizaClienteBloqueado();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                        Console.ReadKey();
                        break;
                }

            } while (escolha != "0");

        }
        public static void NovoCliente()
        {
            Console.Clear();


            Console.WriteLine("Informe a data de nascimento:");
            DateTime dNascimento = DateTime.Parse(Console.ReadLine());
            DateTime DataAtual = DateTime.Now;

            var idade = DateTime.Today.Year - dNascimento.Year;
            if (DateTime.Today.DayOfYear < dNascimento.DayOfYear)
                idade = idade - 1;

            if (idade >= 18)
            {
                string cpf, nome;
                char situacao, sexo;

                do
                {
                    Console.Write("CPF: ");
                    cpf = Console.ReadLine();
                    cpf = cpf.Trim();
                    cpf = cpf.Replace(".", "").Replace("-", "");

                    if (Validacoes.ValidarCpf(cpf) == false)
                        Console.WriteLine("CPF Inválido. Digite um CPF Válido, ou 0 para sair.");
                } while (Validacoes.ValidarCpf(cpf) == false && cpf != "0");
                if (cpf == "0")
                    return;
                else
                {
                    Console.Write("Nome: ");
                    nome = Console.ReadLine();
                    Console.Write("Genero (M - Masculino/ F - Feminino): ");
                    sexo = char.Parse(Console.ReadLine().ToUpper());
                    Console.Write("Situacao (A - Ativo/ I - Inativo): ");
                    situacao = char.Parse(Console.ReadLine().ToUpper());
                }

                try
                {
                    var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                    SqlConnection cnn = new SqlConnection(connetionString);
                    using (cnn)
                    {

                        cnn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("IncluiCliente", cnn);

                        sql_cmnd.CommandType = CommandType.StoredProcedure;

                        sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                        sql_cmnd.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = nome;
                        sql_cmnd.Parameters.AddWithValue("@DataNasc", SqlDbType.Date).Value = dNascimento;
                        sql_cmnd.Parameters.AddWithValue("@sexo", SqlDbType.Char).Value = sexo;
                        sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                        sql_cmnd.ExecuteNonQuery();
                        cnn.Close();
                        Console.WriteLine("Cliente incluido no banco de dados.");
                        Console.ReadKey();

                    }
                }
                catch (SqlException erro)
                {
                    Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                    Console.ReadKey();
                }
                return;

            }
            else
            {
                Console.WriteLine("Menor de 18 anos nao pode ser cadastrado. Presione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
        }
        public static void ListarClientes()
        {
            try
            {

                List<string> list = new List<string>();

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();

                string Select = $"SELECT Nome, CPF, format(DataNasc,'dd/MM/yyyy'), sexo, Situacao FROM dbo.Cliente";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                list.Add($"Nome: {Ler.GetString(0)}\nCPF: {Ler.GetString(1)}\nData de Nascimento: {Ler.GetString(2)}\nSexo:{Ler.GetString(3)}\nSituação: {Ler.GetString(4)}\n");
                            }
                            cnn.Close();
                            list.Sort();
                            int i = 0;
                            string opcao;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine(list[i]);
                                Console.WriteLine("\n\n1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Sair");
                                opcao = Console.ReadLine();
                                switch (opcao)
                                {
                                    case "1":
                                        if (i == list.Count() - 1)
                                            i = list.Count() - 1;
                                        else
                                            i++;
                                        break;

                                    case "2":
                                        if (i == 0)
                                            i = 0;
                                        else
                                            i--;
                                        break;

                                    case "3":
                                        i = 0;
                                        break;

                                    case "4":
                                        i = list.Count() - 1;
                                        break;

                                    case "0":
                                        break;

                                    default:
                                        break;
                                }
                            }
                            while (opcao != "0");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Não existem clientes cadastrados.");
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void BloqueiaCliente()
        {

            Console.WriteLine("Insira o CPF para bloqueio: ");
            string cpf = Console.ReadLine();
            cpf = cpf.Replace(".", "").Replace("-", "");
            Console.Write("Informe a situação do cliente (A - Ativo/ I - Inativo): ");
            char situacao = char.Parse(Console.ReadLine().ToUpper());
            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand sql_cmnd = new SqlCommand("ClienteSituacao", cnn);

                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                    sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                    sql_cmnd.ExecuteNonQuery();
                    cnn.Close();
                    Console.WriteLine("Situação do cliente alterada para .'" + situacao + "'\nPressione ENTER para voltar ao menu...");
                    Console.ReadKey();


                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }




        }
        public static void EditarCliente()
        {
            Console.WriteLine("Informe o CPF do cliente que deseja alterar:");
            var cpf = Console.ReadLine().Replace(".", "").Replace("-", "");

            Console.Clear();
            Console.WriteLine("Informe qual dado deseja alterar do cliente:");
            Console.WriteLine("1. Nome\n2. Data de Nascimento\n3. Sexo");

            var escolha = Console.ReadLine();
            string novoNome;
            DateTime novaData;
            char novoSexo;

            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                SqlCommand sql_cmnd;
                using (cnn)
                {
                    do
                    {

                        switch (escolha)
                        {
                            case "1":
                                Console.WriteLine("Digite o Nome para alterar");
                                novoNome = Console.ReadLine();
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("EditaNomeCliente", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = novoNome;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "2":
                                Console.WriteLine("Digite a data de Nascimento para alterar");
                                novaData = DateTime.Parse(Console.ReadLine());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("EditaDataNascCliente", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@DataNasc", SqlDbType.Date).Value = novaData;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "3":
                                Console.WriteLine("Digite para qual sexo deseja alterar (M - Masculino ou F - Feminino)");
                                novoSexo = char.Parse(Console.ReadLine().ToUpper());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("EditaSexoCliente", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CPF", SqlDbType.VarChar).Value = cpf;
                                sql_cmnd.Parameters.AddWithValue("@sexo", SqlDbType.Char).Value = novoSexo;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();


                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Nome\n2. Data de Nascimento\n3. Sexo\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "0":
                                break;

                            default:
                                Console.WriteLine("Opção incorreta. Tente novamente.");
                                break;
                        }

                    } while (escolha != "0");
                }
                Console.WriteLine("Dados do cliente alterados com sucesso.\nPressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaCliente()
        {
            try
            {

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);

                Console.WriteLine("Digite o nome OU o CPF do cliente que deseja buscar:");
                var Busca = Console.ReadLine();

                cnn.Open();
                string Select = $"SELECT Nome, CPF, format(DataNasc,'dd/MM/yyyy'), sexo, Situacao FROM dbo.Cliente WHERE Nome like '%{Busca}%' OR CPF like '%{Busca}%'";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                Console.Clear();
                                Console.WriteLine("Cliente encontrado: \n");
                                Console.WriteLine($"Nome: {Ler.GetString(0)}\nCPF: {Ler.GetString(1)}\nData de Nascimento: {Ler.GetString(2)}\nSexo:{Ler.GetString(3)}\nSituação: {Ler.GetString(4)}\n");
                                Console.WriteLine("Pressione ENTER para continuar...");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{Busca}\nCliente não encontrado. Pressione ENTER para voltar...");
                            Console.ReadKey();
                        }
                    }
                }
                cnn.Close();

            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaClienteBloqueado()
        {
            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();

                Console.WriteLine("Digite o nome OU o CPF do cliente que deseja saber a situação:");
                var Busca = Console.ReadLine();

                string Select = $"SELECT Nome, CPF, Situacao FROM dbo.Cliente WHERE Nome like '%{Busca}%' OR CPF like '%{Busca}%'";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                Console.Clear();
                                if (Ler.GetString(2) == "I")
                                {
                                    Console.Clear();
                                    Console.WriteLine($"O cliente {Ler.GetString(0)} encontra-se BLOQUEADO\nPressione ENTER para voltar...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine($"O cliente {Ler.GetString(0)} encontra-se DESBLOQUEADO\nPressione ENTER para voltar...");
                                    Console.ReadKey();
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{Busca}\nCliente não encontrado. Pressione ENTER para voltar...");
                            Console.ReadKey();
                        }
                    }
                }
                cnn.Close();

            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }


        public static void SubMenuFornecedores()
        {
            string escolha;

            do
            {
                Console.Clear();

                Console.WriteLine("=============== FORNECEDORES ===============");
                Console.WriteLine("1. Cadastar fornecedor");
                Console.WriteLine("2. Listar fornecedores");
                Console.WriteLine("3. Editar registro de fornecedor");
                Console.WriteLine("4. Bloquear/Desbloqueia fornecedor");
                Console.WriteLine("5. Localizar fornecedor");
                Console.WriteLine("6. Localizar fornecedor bloqueado");
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {

                    case "1":
                        NovoFornecedor();
                        break;

                    case "2":
                        ListarFornecedores();
                        break;

                    case "3":
                        EditarFornecedor();
                        break;

                    case "4":
                        BloqueiaFornecedor();
                        break;

                    case "5":
                        LocalizaFornecedor();
                        break;

                    case "6":
                        LocalizaFornecedorBloqueado();
                        break;

                    case "0":
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Opção inválida");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
                        break;
                }

            } while (escolha != "0");
        }
        public static void NovoFornecedor()
        {
            Console.Clear();

            Console.Write("Digite a data de criacao da empresa:");
            DateTime dCriacao = DateTime.Parse(Console.ReadLine());
            DateTime DataAtual = DateTime.Now;
            TimeSpan date = DataAtual - dCriacao;
            var totalDias = date.Days;

            if (totalDias >= 180)
            {
                string rSocial, cnpj;
                char situacao;

                do
                {
                    Console.Write("CNPJ: ");
                    cnpj = Console.ReadLine();
                    //cnpj = cnpj.Trim();
                    cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                    if (Validacoes.ValidarCnpj(cnpj) == false)
                        Console.WriteLine("CNPJ Inválido. Digite um CNPJ Válido, ou 0 para sair.");
                } while (Validacoes.ValidarCnpj(cnpj) == false && cnpj != "0");
                if (cnpj == "0")
                    return;
                else
                {
                    Console.Write("Razao social: ");
                    rSocial = Console.ReadLine();
                    Console.Write("Situacao (A - Ativo/ I - Inativo): ");
                    situacao = char.Parse(Console.ReadLine().ToUpper());
                }
                try
                {
                    var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                    SqlConnection cnn = new SqlConnection(connetionString);
                    using (cnn)
                    {

                        cnn.Open();
                        SqlCommand sql_cmnd = new SqlCommand("IncluiFornecedor", cnn);

                        sql_cmnd.CommandType = CommandType.StoredProcedure;

                        sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                        sql_cmnd.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = rSocial;
                        sql_cmnd.Parameters.AddWithValue("@DataAbertura", SqlDbType.Date).Value = dCriacao;
                        sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                        sql_cmnd.ExecuteNonQuery();
                        cnn.Close();
                        Console.WriteLine("Fornecedor incluido no banco de dados.");
                        Console.ReadKey();
                    }
                }
                catch (SqlException erro)
                {
                    Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                    Console.ReadKey();
                }
                return;

            }
            else
            {
                Console.WriteLine("Empresa com menos de 6 meses nao pode ser cadastrada. Pressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
        }
        public static void ListarFornecedores()
        {
            try
            {

                List<string> list = new List<string>();

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();

                string Select = $"Select RazaoSocial, CNPJ, Format(DataAbertura,'dd/MM/yyyy'), Situacao From dbo.Fornecedor";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                list.Add($"Razão Social: {Ler.GetString(0)}\nCNPJ: {Ler.GetString(1)}\nData de Abertura: {Ler.GetString(2)}\nSituação: {Ler.GetString(3)}\n");
                            }
                            cnn.Close();
                            list.Sort();
                            int i = 0;
                            string opcao;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine(list[i]);
                                Console.WriteLine("\n\n1 - Proximo\t2 - Anterior\t3 - Primeiro\t4 - Ultimo\t0 - Sair");
                                opcao = Console.ReadLine();
                                switch (opcao)
                                {
                                    case "1":
                                        if (i == list.Count() - 1)
                                            i = list.Count() - 1;
                                        else
                                            i++;
                                        break;

                                    case "2":
                                        if (i == 0)
                                            i = 0;
                                        else
                                            i--;
                                        break;

                                    case "3":
                                        i = 0;
                                        break;

                                    case "4":
                                        i = list.Count() - 1;
                                        break;

                                    case "0":
                                        break;

                                    default:
                                        break;
                                }
                            }
                            while (opcao != "0");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"Não existem Fornecedores cadastrados.");
                            Console.ReadKey();
                        }
                    }
                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void BloqueiaFornecedor()
        {

            Console.WriteLine("Insira o CNPJ para bloqueio: ");
            string cnpj = Console.ReadLine();
            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            Console.Write("Informe a situação do fornecedor (A - Ativo/ I - Inativo): ");
            char situacao = char.Parse(Console.ReadLine().ToUpper());
            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand sql_cmnd = new SqlCommand("FornecedorSituacao", cnn);

                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                    sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;
                    sql_cmnd.ExecuteNonQuery();
                    cnn.Close();
                    Console.WriteLine("Situação do fornecedor alterada para .'" + situacao + "'\nPressione ENTER para voltar ao menu...");
                    Console.ReadKey();


                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }




        }
        public static void EditarFornecedor()
        {
            Console.WriteLine("Informe o CNPJ do Fornecedor que deseja alterar:");
            var cnpj = Console.ReadLine().Replace(".", "").Replace("/", "").Replace("-", "");

            Console.Clear();
            Console.WriteLine("Informe qual dado deseja alterar do Fornecedor:");
            Console.WriteLine("1. Razão Social\n2. Data de Abertura");

            var escolha = Console.ReadLine();
            string novaRSocial;
            DateTime novaData;


            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                SqlCommand sql_cmnd;
                using (cnn)
                {
                    do
                    {

                        switch (escolha)
                        {
                            case "1":
                                Console.WriteLine("Digite a Razão Social para alterar");
                                novaRSocial = Console.ReadLine();
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("EditaRazaoSocial", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                                sql_cmnd.Parameters.AddWithValue("@RazaoSocial", SqlDbType.VarChar).Value = novaRSocial;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Cliente?:");
                                Console.WriteLine("\n1. Razão Social\n2. Data de Abertura\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "2":
                                Console.WriteLine("Digite a Data de Abertura para alterar");
                                novaData = DateTime.Parse(Console.ReadLine());
                                Console.Clear();

                                cnn.Open();
                                sql_cmnd = new SqlCommand("EditaDataAbertura", cnn);
                                sql_cmnd.CommandType = CommandType.StoredProcedure;
                                sql_cmnd.Parameters.AddWithValue("@CNPJ", SqlDbType.VarChar).Value = cnpj;
                                sql_cmnd.Parameters.AddWithValue("@DataAbertura", SqlDbType.Date).Value = novaData;
                                sql_cmnd.ExecuteNonQuery();
                                cnn.Close();

                                Console.WriteLine("Deseja alterar mais alguma informação do Fornecedor?:");
                                Console.WriteLine("\n1. Razão Social\n2. Data de Abertura\n0. Finalizar Alterações");
                                escolha = Console.ReadLine();
                                break;

                            case "0":
                                break;

                            default:
                                Console.WriteLine("Opção incorreta. Tente novamente.");
                                break;
                        }

                    } while (escolha != "0");
                }
                Console.WriteLine("Dados do Fornecedor alterados com sucesso.\nPressione ENTER para voltar ao menu...");
                Console.ReadKey();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaFornecedor()
        {
            try
            {

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);

                Console.WriteLine("Digite A Razão Social OU o CNPJ do cliente que deseja buscar:");
                var Busca = Console.ReadLine();

                cnn.Open();
                string Select = $"Select RazaoSocial, CNPJ, Format(DataAbertura,'dd/MM/yyyy'), Situacao From dbo.Fornecedor WHERE RazaoSocial like '%{Busca}%' OR CNPJ like '%{Busca}%'";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                Console.Clear();
                                Console.WriteLine("Empresa encontrado: \n");
                                Console.WriteLine($"Razão Social: {Ler.GetString(0)}\nCNPJ: {Ler.GetString(1)}\nData de Abertura: {Ler.GetString(2)}\nSituação: {Ler.GetString(3)}\n");
                                Console.WriteLine("Pressione ENTER para continuar...");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{Busca}\nRegistro não encontrado. Pressione ENTER para voltar...");
                            Console.ReadKey();
                        }
                    }
                }
                cnn.Close();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static void LocalizaFornecedorBloqueado()
        {
            try
            {

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();

                Console.WriteLine("Digite A Razão Social OU o CNPJ do cliente que deseja saber a situação:");
                var Busca = Console.ReadLine();

                string Select = $"Select RazaoSocial, CNPJ, Situacao From dbo.Fornecedor WHERE RazaoSocial like '%{Busca}%' OR CNPJ like '%{Busca}%'";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                if (Ler.GetString(2) == "I")
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Empresa {Ler.GetString(0)} encontra-se BLOQUEADA.\nPressione ENTER para voltar...");
                                    Console.ReadKey();
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine($"Empresa {Ler.GetString(0)} encontra-se DESBLOQUEADA.\nPressione ENTER para voltar...");
                                    Console.ReadKey();
                                }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{Busca}\nRegistro não encontrado. Pressione ENTER para voltar...");
                            Console.ReadKey();
                        }
                    }
                }
                cnn.Close();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }



        public static void SubMenuMateriaPrima()
        {
            string escolha;

            do
            {
                Console.Clear();
                Console.WriteLine("\n=============== MATÉRIA-PRIMA ===============");
                Console.WriteLine("1. Cadastrar Matéria-Prima");
                Console.WriteLine("2. Localizar Matéria-Prima");
                Console.WriteLine("3. Imprimir Matérias-Primas");
                Console.WriteLine("4. Alterar Situação da Matéria-Prima");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("0. Voltar ao menu anterior");
                Console.Write("\nEscolha: ");

                switch (escolha = Console.ReadLine())
                {
                    case "1":
                        CadastrarMP();
                        break;
                    case "2":
                        LocalizaMP();
                        break;
                    case "3":
                        // ImprimirMP();
                        break;
                    case "4":
                        // AlterarSituacaoMP();
                        break;
                    case "0":
                        break;

                    default:
                        Console.WriteLine("\n Opção inválida.");
                        Console.WriteLine("\n Pressione ENTER para voltar ao menu.");
                        Console.ReadKey();
                        break;
                }
            } while (escolha != "0");
        }

        public static void CadastrarMP()
        {
            Console.Clear();
            Console.WriteLine("\n Cadastro de Materia-Prima\n");
            Console.Write(" Nome: ");
            var nome = Console.ReadLine();
            char situacao;
            do
            {
                Console.Write(" Situacao (A - Ativo / I - Inativo): ");
                situacao = char.Parse(Console.ReadLine().ToUpper());

                if (situacao != 'A' && situacao != 'I')
                    Console.WriteLine(" Situacao invalida.");

            } while (situacao != 'A' && situacao != 'I');

         

            int cont = ContarLinhaMP()+1; 
            string id = $"MP{cont.ToString().PadLeft(4, '0')}";

            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                using (cnn)
                {
                    cnn.Open();
                    SqlCommand sql_cmnd = new SqlCommand("IncluiMP", cnn);

                    sql_cmnd.CommandType = CommandType.StoredProcedure;

                    sql_cmnd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = id;
                    sql_cmnd.Parameters.AddWithValue("@Nome", SqlDbType.VarChar).Value = nome;
                    sql_cmnd.Parameters.AddWithValue("@Situacao", SqlDbType.Char).Value = situacao;

                    sql_cmnd.ExecuteNonQuery();
                    cnn.Close();
                    Console.WriteLine("Materia Prima incluida no banco de dados.");
                    Console.WriteLine("Pressione ENTER para voltar ao menu...");
                    Console.ReadKey();
                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }
        public static int ContarLinhaMP()
        {

            int cont = 0;
            try
            {
                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);
                cnn.Open();

                string Select = $"Select Id From dbo.MateriaPrima";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                       
                            while (Ler.Read())
                            {
                                cont++;
                            }
                            cnn.Close();
                       
                    }
                }
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
            return cont;
        }
        public static void LocalizaMP()
        {
            try
            {

                var connetionString = @"Data Source=Everton;Initial Catalog=Biltiful;User ID=sa;Password=007410";
                SqlConnection cnn = new SqlConnection(connetionString);

                Console.WriteLine("Digite o ID ou o Nome da Materia Prima que deseja buscar:");
                var Busca = Console.ReadLine();

                cnn.Open();
                string Select = $"select Id, Nome, format(DataCadastro,'dd/MM/yyyy'), format(UltimaCompra,'dd/MM/yyyy'), Situacao From dbo.MateriaPrima WHERE Id like '%{Busca}%' OR Nome like '%{Busca}%'";
                using (SqlCommand comando = new SqlCommand(Select, cnn))
                {
                    using (SqlDataReader Ler = comando.ExecuteReader())
                    {
                        if (Ler.HasRows == true)
                        {
                            while (Ler.Read())
                            {
                                Console.Clear();
                                Console.WriteLine("Resultado Encontrado: \n");
                                Console.WriteLine($"ID: {Ler.GetString(0)}\nMateria Prima: {Ler.GetString(1)}\nCadastrada em: {Ler.GetString(2)}\nUltima Compra em: {Ler.GetString(3)}\nSituação: {Ler.GetString(4)}\n");
                                Console.WriteLine("Pressione ENTER para continuar...");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine($"{Busca}\nRegistro não encontrado. Pressione ENTER para voltar...");
                            Console.ReadKey();
                        }
                    }
                }
                cnn.Close();
            }
            catch (SqlException erro)
            {
                Console.WriteLine("Erro ao se conectar no banco de dados \n" + erro);
                Console.ReadKey();
            }
        }

        

        //public void AlterarSituacao()
        //{
        //    string cod, mPrima, situacao;
        //    bool flag = true;

        //    Console.Clear();
        //    Console.WriteLine("\n Alterar Materia-prima");
        //    Console.Write("\n Digite o codigo da materia-prima: ");
        //    cod = Console.ReadLine();

        //    mPrima = Buscar(cod);

        //    if (mPrima == null)
        //    {
        //        Console.WriteLine("\n A materia-prima nao existe.");
        //        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
        //        Console.ReadKey();
        //    }
        //    else
        //    {
        //        situacao = mPrima.Substring(42, 1);
        //        if (situacao == "A")
        //            situacao = "Ativo";
        //        else if (situacao == "I")
        //            situacao = "Inativo";

        //        Console.WriteLine("\n A materia-prima foi encontrada.\n");
        //        Console.WriteLine($" Codigo: {mPrima.Substring(0, 6)}");
        //        Console.WriteLine($" Nome: {mPrima.Substring(6, 20)}");
        //        Console.WriteLine($" Data ultima compra: {mPrima.Substring(26, 8).Insert(2, "/").Insert(5, "/")}");
        //        Console.WriteLine($" Data do cadastro: {mPrima.Substring(34, 8).Insert(2, "/").Insert(5, "/")}");
        //        Console.WriteLine($" Situacao: {situacao}");

        //        do
        //        {
        //            Console.Write("\n Qual a nova situacao da materia-prima (A / I): ");
        //            situacao = Console.ReadLine().ToUpper();

        //            if ((situacao != "A") && (situacao != "I"))
        //            {
        //                Console.WriteLine("\n Situacao invalida.");
        //                Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
        //                Console.ReadKey();
        //            }
        //            else
        //            {
        //                flag = false;
        //            }

        //        } while (flag);

        //        Atualizar(cod, null, situacao);
        //    }
        //}

        //public void Atualizar(string cod, string dataUltimaCompra = null, string situacaoAtualizada = null)
        //{


        //}

        //public string Impressao(MPrima mPrima)
        //{
        //    string situacao = "";
        //    if (mPrima.Situacao == 'A')
        //        situacao = "Ativo";
        //    else if (mPrima.Situacao == 'I')
        //        situacao = "Inativo";

        //    return "\n"
        //        + "\n Codigo: \t" + mPrima.Id
        //        + "\n Nome: \t" + mPrima.Nome
        //        + "\n Ultima Venda: \t" + mPrima.UltimaCompra.ToString("dd/MM/yyyy")
        //        + "\n Data Cadastro: " + mPrima.DataCadastro.ToString("dd/MM/yyyy")
        //        + "\n Situacao: \t" + situacao
        //        + "\n";
        //}

        //public void ImprimirMPrimas()
        //{



        //}


    }











    //public class Produto
    //{
    //    public string CodigoBarras { get; set; }
    //    public string Nome { get; set; }
    //    public decimal ValorVenda { get; set; }
    //    public DateTime UltimaVenda { get; set; }
    //    public DateTime DataCadastro { get; set; }
    //    public char Situacao { get; set; }

    //    public override string ToString()
    //    {
    //        return CodigoBarras
    //            + Nome.PadLeft(20, ' ')
    //            + ValorVenda.ToString("000.#0").Replace(",", "")
    //            + UltimaVenda.ToString("dd/MM/yyyy").Replace("/", "")
    //            + DataCadastro.ToString("dd/MM/yyyy").Replace("/", "")
    //            + Situacao;
    //    }

    //    public Produto()
    //    {

    //    }

    //    public Produto(string cBarras, string nome, decimal vVenda, DateTime uVenda, DateTime dCadastro, char situacao)
    //    {
    //        CodigoBarras = cBarras;
    //        Nome = nome;
    //        ValorVenda = vVenda;
    //        UltimaVenda = uVenda;
    //        DataCadastro = dCadastro;
    //        Situacao = situacao;
    //    }

    //    public void Menu()
    //    {
    //        string escolha;

    //        do
    //        {
    //            Console.Clear();
    //            Console.WriteLine("\n=============== PRODUTO ===============");
    //            Console.WriteLine("1. Cadastrar Produto");
    //            Console.WriteLine("2. Localizar Produto");
    //            Console.WriteLine("3. Imprimir Produtos");
    //            Console.WriteLine("4. Alterar Situacao do Produto");
    //            Console.WriteLine("---------------------------------------");
    //            Console.WriteLine("0. Voltar ao menu anterior");
    //            Console.Write("\nEscolha: ");

    //            switch (escolha = Console.ReadLine())
    //            {
    //                case "0":
    //                    break;
    //                case "1":
    //                    Cadastrar();
    //                    break;
    //                case "2":
    //                    Localizar();
    //                    break;
    //                case "3":
    //                    ImprimirProdutos();
    //                    break;
    //                case "4":
    //                    AlterarSituacao();
    //                    break;

    //                default:
    //                    Console.WriteLine("\n Opção inválida.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao menu.");
    //                    Console.ReadKey();
    //                    break;
    //            }

    //        } while (escolha != "0");
    //    }


    //    public void Cadastrar()
    //    {
    //        Produto produto = new Produto();

    //        char sit = 'A';
    //        string cod, nomeTemp, verificaProduto = null;
    //        decimal valorVenda = 0;
    //        bool flag = true;

    //        do
    //        {
    //            Console.Clear();
    //            Console.WriteLine("\n Cadastro de Produto\n");

    //            do
    //            {
    //                Console.Clear();

    //                Console.Write(" Cod. Barras: 789");
    //                cod = Console.ReadLine();

    //                if (string.IsNullOrEmpty(cod))
    //                {
    //                    Console.WriteLine("\n Nenhum campo podera ser vazio.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                    continue;

    //                }

    //                cod = "789" + cod;

    //                if (cod.Length != 13)
    //                {
    //                    Console.WriteLine("\n Codigo inválido. Digite um código de 13 digitos, informando apenas os ultimos 10 numeros.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                    continue;
    //                }

    //                verificaProduto = Buscar(cod);

    //                if (!string.IsNullOrEmpty(verificaProduto))
    //                {
    //                    Console.WriteLine("\n Ja existe um produto cadastrado com esse codigo.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar...");
    //                    Console.ReadKey();
    //                }

    //            } while (cod.Length != 13 || !string.IsNullOrEmpty(verificaProduto));


    //            do
    //            {
    //                Console.Write(" Nome: ");
    //                nomeTemp = Console.ReadLine();

    //                if (nomeTemp.Length > 20 || string.IsNullOrEmpty(nomeTemp))
    //                {
    //                    Console.WriteLine("\n Nome invalido. Digite apenas 20 caracteres.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                }

    //            } while (nomeTemp.Length > 20 || string.IsNullOrEmpty(nomeTemp));

    //            do
    //            {
    //                Console.Write(" Valor da Venda: ");
    //                valorVenda = Convert.ToDecimal(Console.ReadLine());

    //                if ((valorVenda < 1) || (valorVenda > (decimal)999.99))
    //                {
    //                    Console.WriteLine("\n Valor invalido. Apenas valores maior que 0 e menor que 999,99.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                }

    //            } while ((valorVenda < 1) || (valorVenda > (decimal)999.99));


    //            do
    //            {
    //                Console.Write(" Situacao (A / I): ");
    //                sit = char.Parse(Console.ReadLine().ToUpper());

    //                if ((sit != 'A') && (sit != 'I') || string.IsNullOrEmpty(sit.ToString()))
    //                {
    //                    Console.WriteLine("\n Situacao invalida.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                    continue;
    //                }

    //            } while ((sit != 'A') && (sit != 'I'));


    //            flag = false;

    //            produto.CodigoBarras = cod;
    //            produto.Nome = nomeTemp;
    //            produto.ValorVenda = valorVenda;
    //            produto.UltimaVenda = DateTime.Now.Date;
    //            produto.DataCadastro = DateTime.Now.Date;
    //            produto.Situacao = sit;

    //            GravarProduto(produto);

    //            Console.WriteLine("\n Cadastro do Produto concluido com sucesso!\n");
    //            Console.WriteLine("\n Pressione ENTER para voltar ao menu");
    //            Console.ReadKey();

    //        } while (flag);
    //    }

    //    public void GravarProduto(Produto produto)
    //    {
    //        string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
    //        Directory.CreateDirectory(caminhoFinal);

    //        string arquivoFinal = Path.Combine(caminhoFinal, "Cosmetico.dat");

    //        try
    //        {
    //            if (!File.Exists(arquivoFinal))
    //            {
    //                using (StreamWriter sw = new StreamWriter(arquivoFinal))
    //                {
    //                    sw.WriteLine(produto.ToString());
    //                }
    //            }
    //            else
    //            {
    //                using (StreamWriter sw = new StreamWriter(arquivoFinal, append: true))
    //                {
    //                    sw.WriteLine(produto.ToString());
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Ex -> " + ex.Message);
    //        }
    //    }

    //    public void Localizar()
    //    {
    //        string cod, produto;

    //        Console.Clear();
    //        Console.WriteLine("\n Localizar Produto");
    //        Console.Write("\n Digite o codigo do produto: ");
    //        cod = Console.ReadLine();

    //        produto = Buscar(cod);

    //        if (produto == null)
    //        {
    //            Console.WriteLine("\n O produto nao existe.");
    //            Console.WriteLine("\n Pressione ENTER para voltar ao menu");
    //            Console.ReadKey();
    //        }
    //        else
    //        {
    //            string situacao = produto.Substring(54, 1);
    //            if (situacao == "A")
    //                situacao = "Ativo";
    //            else if (situacao == "I")
    //                situacao = "Inativo";

    //            Console.WriteLine("\n O produto foi encontrado.\n");
    //            Console.WriteLine($" Codigo: {produto.Substring(0, 13)}");
    //            Console.WriteLine($" Nome: {produto.Substring(13, 20)}");
    //            Console.WriteLine($" Valor da venda: {produto.Substring(33, 5).Insert(3, ",")}");
    //            Console.WriteLine($" Data ultima venda: {produto.Substring(38, 8).Insert(2, "/").Insert(5, "/")}");
    //            Console.WriteLine($" Data do cadastro: {produto.Substring(46, 8).Insert(2, "/").Insert(5, "/")}");
    //            Console.WriteLine($" Situacao: {situacao}");
    //            Console.WriteLine("\n Pressione ENTER para voltar ao menu");
    //            Console.ReadKey();
    //        }
    //    }

    //    public void AlterarSituacao()
    //    {
    //        string cod, produto, situacao;
    //        bool flag = true;

    //        Console.Clear();
    //        Console.WriteLine("\n Alterar Produto");
    //        Console.Write("\n Digite o codigo do produto: ");
    //        cod = Console.ReadLine();

    //        produto = Buscar(cod);

    //        if (produto == null)
    //        {
    //            Console.WriteLine("\n O produto nao existe.");
    //            Console.WriteLine("\n Pressione ENTER para voltar ao menu");
    //            Console.ReadKey();
    //        }
    //        else
    //        {
    //            situacao = produto.Substring(54, 1);
    //            if (situacao == "A")
    //                situacao = "Ativo";
    //            else if (situacao == "I")
    //                situacao = "Inativo";

    //            Console.WriteLine("\n O produto foi encontrado.\n");
    //            Console.WriteLine($" Codigo: {produto.Substring(0, 13)}");
    //            Console.WriteLine($" Nome: {produto.Substring(13, 20)}");
    //            Console.WriteLine($" Valor da venda: {produto.Substring(33, 5).Insert(3, ",")}");
    //            Console.WriteLine($" Data ultima venda: {produto.Substring(38, 8).Insert(2, "/").Insert(5, "/")}");
    //            Console.WriteLine($" Data do cadastro: {produto.Substring(46, 8).Insert(2, "/").Insert(5, "/")}");
    //            Console.WriteLine($" Situacao: {situacao}");

    //            do
    //            {
    //                Console.Write("\n Qual a nova situacao do produto (A / I): ");
    //                situacao = Console.ReadLine().ToUpper();

    //                if ((situacao != "A") && (situacao != "I"))
    //                {
    //                    Console.WriteLine("\n Situacao invalida.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar ao cadastro.");
    //                    Console.ReadKey();
    //                }
    //                else
    //                {
    //                    flag = false;
    //                }

    //            } while (flag);

    //            Atualizar(cod, null, situacao);
    //        }
    //    }

    //    public void ImprimirProdutos()
    //    {
    //        string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
    //        Directory.CreateDirectory(caminhoFinal);

    //        string arquivoFinal = Path.Combine(caminhoFinal, "Cosmetico.dat");

    //        List<Produto> Produtos = new List<Produto>();

    //        if (File.Exists(arquivoFinal))
    //        {
    //            try
    //            {
    //                using (StreamReader sr = new StreamReader(arquivoFinal))
    //                {
    //                    string line = sr.ReadLine();
    //                    do
    //                    {
    //                        if (line.Substring(54, 1) != "I")
    //                        {
    //                            Produtos.Add(
    //                                new Produto(
    //                                    line.Substring(0, 13),
    //                                    line.Substring(13, 20),
    //                                    Convert.ToDecimal(line.Substring(33, 5).Insert(3, ",")),
    //                                    Convert.ToDateTime(line.Substring(38, 8).Insert(2, "/").Insert(5, "/")).Date,
    //                                    Convert.ToDateTime(line.Substring(46, 8).Insert(2, "/").Insert(5, "/")).Date,
    //                                    Convert.ToChar(line.Substring(54, 1))
    //                                    )
    //                                );
    //                        }
    //                        line = sr.ReadLine();

    //                    } while (line != null);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine("Ex ->" + ex.Message);
    //            }


    //            string escolha;
    //            int opcao = 1, posicao = 0;
    //            bool flag = true;

    //            do
    //            {
    //                if ((opcao < 1) || (opcao > 5))
    //                {
    //                    Console.WriteLine("\n Opcao invalida.");
    //                    Console.WriteLine("\n Pressione ENTER para voltar.");
    //                    Console.ReadKey();
    //                    opcao = 1;
    //                }
    //                else
    //                {
    //                    if (opcao == 5)
    //                    {
    //                        flag = false;
    //                        return;
    //                    }
    //                    else if (opcao == 1)
    //                    {
    //                        Console.Clear();
    //                        Console.WriteLine("\n Impressao de Produtos");
    //                        Console.WriteLine(" --------------------------- ");
    //                        posicao = Produtos.IndexOf(Produtos.First());
    //                        Console.WriteLine($"\n Produto {posicao + 1}");
    //                        Console.WriteLine(Impressao(Produtos.First()));
    //                    }
    //                    else if (opcao == 4)
    //                    {
    //                        Console.Clear();
    //                        Console.WriteLine("\n Impressao de Produtos");
    //                        Console.WriteLine(" --------------------------- ");
    //                        posicao = Produtos.IndexOf(Produtos.Last());
    //                        Console.WriteLine($"\n Produto {posicao + 1}");
    //                        Console.WriteLine(Impressao(Produtos.Last()));
    //                    }
    //                    else if (opcao == 2)
    //                    {
    //                        if (posicao == 0)
    //                        {
    //                            Console.Clear();
    //                            Console.WriteLine("\n Impressao de Produtos");
    //                            Console.WriteLine(" --------------------------- ");
    //                            Console.WriteLine("\n Nao ha produto anterior.\n");
    //                            Console.WriteLine(" --------------------------- ");
    //                            posicao = Produtos.IndexOf(Produtos.First());
    //                            Console.WriteLine($"\n Produto {posicao + 1}");
    //                            Console.WriteLine(Impressao(Produtos.First()));
    //                        }
    //                        else
    //                        {
    //                            Console.Clear();
    //                            Console.WriteLine("\n Impressao de Produtos");
    //                            Console.WriteLine(" --------------------------- ");
    //                            posicao--;
    //                            Console.WriteLine($"\n Produto {posicao + 1}");
    //                            Console.WriteLine(Impressao(Produtos[posicao]));
    //                            posicao = Produtos.IndexOf(Produtos[posicao]);
    //                        }
    //                    }
    //                    else if (opcao == 3)
    //                    {
    //                        if (posicao == Produtos.IndexOf(Produtos.Last()))
    //                        {
    //                            Console.Clear();
    //                            Console.WriteLine("\n Impressao de Produtos");
    //                            Console.WriteLine(" --------------------------- ");
    //                            Console.WriteLine("\n Nao ha proximo produto.\n");
    //                            Console.WriteLine(" --------------------------- ");
    //                            Console.WriteLine($"\n Produto {posicao + 1}");
    //                            Console.WriteLine(Impressao(Produtos.Last()));
    //                            posicao = Produtos.IndexOf(Produtos.Last());
    //                        }
    //                        else
    //                        {
    //                            Console.Clear();
    //                            Console.WriteLine("\n Impressao de Produtos");
    //                            Console.WriteLine(" --------------------------- ");
    //                            posicao++;
    //                            Console.WriteLine($"\n Produto {posicao + 1}");
    //                            Console.WriteLine(Impressao(Produtos[posicao]));
    //                            posicao = Produtos.IndexOf(Produtos[posicao]);
    //                        }
    //                    }

    //                    Console.WriteLine(" ------------------------------------------------------------------ ");
    //                    Console.WriteLine("\n Navegacao\n");
    //                    Console.WriteLine(" 1 - Primeiro / 2 - Anterior / 3 - Proximo / 4 - Ultimo / 5 - Sair");
    //                    Console.Write("\n Escolha: ");
    //                    escolha = Console.ReadLine();
    //                    int.TryParse(escolha, out opcao);
    //                }

    //            } while (flag);
    //        }
    //        else
    //        {
    //            Console.WriteLine("\n Nao ha produtos cadastrados\n");
    //            Console.WriteLine("\n Pressione ENTER para voltar");
    //            Console.ReadKey();
    //        }
    //    }

    //    public string Impressao(Produto produto)
    //    {
    //        string situacao = "";
    //        if (produto.Situacao == 'A')
    //            situacao = "Ativo";
    //        else if (produto.Situacao == 'I')
    //            situacao = "Inativo";

    //        return "\n"
    //            + "\n Codigo: \t" + produto.CodigoBarras
    //            + "\n Nome: \t" + produto.Nome
    //            + "\n Valor Venda: \t" + produto.ValorVenda.ToString("000.#0")
    //            + "\n Ultima Venda: \t" + produto.UltimaVenda.ToString("dd/MM/yyyy")
    //            + "\n Data Cadastro: " + produto.DataCadastro.ToString("dd/MM/yyyy")
    //            + "\n Situacao: \t" + situacao
    //            + "\n";
    //    }

    //    public void Atualizar(string cod, string dataUltimaVenda = null, string situacaoAtualizada = null)
    //    {
    //        string produto;
    //        produto = Buscar(cod);

    //        if (produto == null)
    //        {
    //            Console.WriteLine("\n O produto nao existe.");
    //            Console.WriteLine("\n Pressione ENTER para voltar");
    //            Console.ReadKey();
    //        }
    //        else
    //        {
    //            string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
    //            Directory.CreateDirectory(caminhoFinal);

    //            string arquivoFinal = Path.Combine(caminhoFinal, "Cosmetico.dat");

    //            List<string> Produtos = new List<string>();
    //            string novoProduto = null;

    //            if (File.Exists(arquivoFinal))
    //            {
    //                try
    //                {
    //                    using (StreamReader sr = new StreamReader(arquivoFinal))
    //                    {
    //                        string line = sr.ReadLine();
    //                        do
    //                        {
    //                            if (line.Substring(0, 13) != cod)
    //                                Produtos.Add(line);

    //                            line = sr.ReadLine();

    //                        } while (line != null);
    //                    }

    //                    File.Delete(arquivoFinal);

    //                    if (dataUltimaVenda != null)
    //                    {
    //                        novoProduto = produto.Substring(0, 13)
    //                            + produto.Substring(13, 20)
    //                            + produto.Substring(33, 5)
    //                            + dataUltimaVenda.Replace("/", "")
    //                            + produto.Substring(46, 8)
    //                            + produto.Substring(54, 1);
    //                    }
    //                    else if (situacaoAtualizada != null)
    //                    {
    //                        novoProduto = produto.Substring(0, 13)
    //                            + produto.Substring(13, 20)
    //                            + produto.Substring(33, 5)
    //                            + produto.Substring(38, 8)
    //                            + produto.Substring(46, 8)
    //                            + situacaoAtualizada;
    //                    }

    //                    using (StreamWriter sw = new StreamWriter(arquivoFinal))
    //                    {
    //                        Produtos.ForEach(prod => sw.WriteLine(prod));
    //                        sw.WriteLine(novoProduto);
    //                    }

    //                    if (situacaoAtualizada != null)
    //                    {
    //                        Console.WriteLine("\n Produto alterado.");
    //                        Console.WriteLine("\n Pressione ENTER para voltar ao menu");
    //                        Console.ReadKey();
    //                    }
    //                }
    //                catch (Exception ex)
    //                {
    //                    Console.WriteLine("Ex ->" + ex.Message);
    //                }
    //            }
    //        }
    //    }

    //    public string Buscar(string cod, bool remover = false)
    //    {
    //        string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
    //        Directory.CreateDirectory(caminhoFinal);

    //        string arquivoFinal = Path.Combine(caminhoFinal, "Cosmetico.dat");

    //        string produto = null;

    //        if (File.Exists(arquivoFinal))
    //        {
    //            try
    //            {
    //                using (StreamReader sr = new StreamReader(arquivoFinal))
    //                {
    //                    string line = sr.ReadLine();
    //                    do
    //                    {
    //                        if (line.Substring(0, 13) == cod)
    //                            produto = line;

    //                        line = sr.ReadLine();

    //                    } while (line != null);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine("Ex ->" + ex.Message);
    //            }
    //        }
    //        return produto;
    //    }

    //    public Produto RetornaProduto(string cod)
    //    {
    //        string caminhoFinal = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
    //        Directory.CreateDirectory(caminhoFinal);

    //        string arquivoFinal = Path.Combine(caminhoFinal, "Cosmetico.dat");

    //        Produto produto = null;

    //        if (File.Exists(arquivoFinal))
    //        {
    //            try
    //            {
    //                using (StreamReader sr = new StreamReader(arquivoFinal))
    //                {
    //                    string line = sr.ReadLine();
    //                    do
    //                    {
    //                        if (line.Substring(0, 13) == cod)
    //                            produto =
    //                                new Produto(
    //                                    line.Substring(0, 13),
    //                                    line.Substring(13, 20),
    //                                    Convert.ToDecimal(line.Substring(33, 5).Insert(3, ",")),
    //                                    Convert.ToDateTime(line.Substring(38, 8).Insert(2, "/").Insert(5, "/")).Date,
    //                                    Convert.ToDateTime(line.Substring(46, 8).Insert(2, "/").Insert(5, "/")).Date,
    //                                    Convert.ToChar(line.Substring(54, 1))
    //                                    );

    //                        line = sr.ReadLine();

    //                    } while (line != null);
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine("Ex ->" + ex.Message);
    //            }
    //        }
    //        return produto;
    //    }
    //}

    public class Validacoes
    {


        public static bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        public static bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }


}
