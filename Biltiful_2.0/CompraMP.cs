using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Biltiful_2._0
//{
//    public class Compra
//    {
//        public int Id { get; set; } // 5 00000
//        public DateTime DataCompra { get; set; } //8 00/00/0000
//        public string Fornecedor { get; set; } //12 00.000.000/0001-00
//        public float ValorTotal { get; set; } // 7 00.000,00
//        public List<ItemCompra> ListaDeItens { get; set; }

//        public Compra()
//        {
//            Id = new ManipulaArquivosCompraMP().PegarUltimoId();
//            DataCompra = DateTime.Now;
//        }

//        public Compra(int id, DateTime dCompra, string cnpjFornecedor, float vTotal)
//        {
//            Id = id;
//            DataCompra = dCompra;
//            Fornecedor = cnpjFornecedor;
//            ValorTotal = vTotal;
//        }

//        public static void SubMenu()
//        {
//            new ManipulaArquivosCompraMP();

//            int option = -1;
//            while (option != 0)
//            {
//                Console.Clear();

//                Console.WriteLine("=============== COMPRAS ===============");
//                Console.WriteLine("1. Nova Compra");
//                Console.WriteLine("2. Consultar Compra");
//                Console.WriteLine("3. Imprimir Registros de Compra");
//                Console.WriteLine("--------------------------------------");
//                Console.WriteLine("0. Voltar");
//                Console.Write("\nEscolha: ");

//                option = int.Parse(Console.ReadLine());
//                switch (option)
//                {
//                    // ---------- CADASTRAR COMPRA -----------
//                    case 1:
//                        CadastraNovaCompra();
//                        break;

//                    // ---------- LOCALIZAR COMPRA -----------
//                    case 2:
//                       break;
                       
//                        break;

//                    // ---------- IMPRESSÃO POR REGISTRO -----------
//                    case 3:
                       
//                        break;
//                }
//                Console.ReadKey();
//                Console.Clear();
//            }
//        }

//        private static void CadastraNovaCompra()
//        {
//            Compra compra = new();
//            string cnpjFornecedor;
           
//                Console.Write("CNPJ do Fornecedor: ");
//                cnpjFornecedor = Console.ReadLine();
//                cnpjFornecedor = cnpjFornecedor.Replace(".", "").Replace("/", "").Replace("-", "");

               
                    
               
           
            

//            int count = 1;
//            List<ItemCompra> itens = new();
//            Console.WriteLine("\nItens da Compra\n");
//            Console.Write("Quantidade de itens a comprar (limite de 3 itens por compra): ");
//            int qtdd;
//            do
//            {
//                Console.WriteLine("Você só pode comprar 3 itens por compra!");
//                qtdd = int.Parse(Console.ReadLine());
//            } while (qtdd > 3 || qtdd < 0);
//            do
//            {
//                string idMP;
//                Console.WriteLine($"Item {count}");
//                do
//                {
//                    Console.Write("- Id da Matéria Prima: ");
//                    idMP = Console.ReadLine();
//                } while (new MPrima().RetornaMateriaPrima(idMP) == null);
//                float valorUnitario;
//                float quantidade;
//                do
//                {
//                    Console.Write("- Valor unitário do item: ");
//                    valorUnitario = float.Parse(Console.ReadLine());
//                    Console.Write("- Quantidade do item que deseja comprar: ");
//                    quantidade = float.Parse(Console.ReadLine());
//                    if ((valorUnitario * quantidade) > 99999.99f) Console.WriteLine("O valor total do Item ultrapassou o limite de 99.999,99");
//                } while ((valorUnitario * quantidade) > 99999.99f);

//                ItemCompra item = new(compra.Id,
//                                        compra.DataCompra,
//                                        idMP,
//                                        quantidade,
//                                        valorUnitario);
//                itens.Add(item);
//                count++;
//            } while (count <= qtdd);
//            compra.Fornecedor = cnpjFornecedor;
//            itens.ForEach(item => compra.ValorTotal += item.TotalItem);
//            ItemCompra.Cadastrar(itens);
           
//        }

//        public void ImprimirCompra()
//        {
//            Console.WriteLine("---------------------------------");
//            Console.WriteLine("Id: " + Compra.FormatarId(Id));
//            Console.WriteLine("Data: " + DataCompra);
//            Console.WriteLine("CNPJ Fornecedor: " + Fornecedor);
//            Console.WriteLine("Valor total: " + ValorTotal.ToString("0000000").Insert(5, ",").TrimStart('0'));
//            Console.WriteLine("---------------------------------");
//        }

//        public void ImprimirCompraEmLinha()
//        {
//            Console.Write("Id: " + Compra.FormatarId(Id) + " / ");
//            Console.Write("Data: " + DataCompra + " / ");
//            Console.Write("CNPJ Fornecedor: " + Fornecedor + " / ");
//            Console.Write("Valor total: " + ValorTotal);
//        }

//        public override string ToString() => FormatarId(Id) + FormatarData(DataCompra) + FormatarCNPJ(Fornecedor) + FormatarValorTotal(ValorTotal);

//        public static string FormatarId(int id) => id.ToString().PadLeft(5, '0');

//        public static string FormatarData(DateTime data) => data.ToString("dd/MM/yyyy").Replace("/", "");

//        public static string FormatarCNPJ(string cnpj) => cnpj.Replace(".", "").Replace("/", "").Replace("-", "");

//        public static string FormatarValorTotal(float total) => string.Join("", total.ToString("0.00").PadLeft(8, '0').Split(','));

//        public static Compra ExtrairCompra(string linhaDoArquivo) => new Compra(
//                                                                        ExtrairId(linhaDoArquivo),
//                                                                        ExtrairDataCompra(linhaDoArquivo),
//                                                                        ExtrairCNPJ(linhaDoArquivo),
//                                                                        ExtrairValorTotal(linhaDoArquivo));

//        public static int ExtrairId(string linhaDoArquivo) => int.Parse(linhaDoArquivo.Substring(0, 5).TrimStart('0'));

//        public static DateTime ExtrairDataCompra(string linhaDoArquivo)
//        {
//            string data = linhaDoArquivo.Substring(5, 8);
//            int dia = int.Parse(data.Substring(0, 2));
//            int mes = int.Parse(data.Substring(2, 2));
//            int ano = int.Parse(data.Substring(4, 4));

//            return new DateTime(ano, mes, dia);
//        }

//        public static string ExtrairCNPJ(string linhaDoArquivo) => linhaDoArquivo.Substring(13, 14)
//                                                                    .Insert(2, ".")
//                                                                    .Insert(6, ".")
//                                                                    .Insert(10, "/")
//                                                                    .Insert(15, "-");

//        public static float ExtrairValorTotal(string linhaDoArquivo) => float.Parse(linhaDoArquivo.Substring(26, 7));
//    }

//    public class ItemCompra
//    {
//        public ItemCompra(int id, DateTime dataCompra, string materiaPrima, float quantidade, float valorUnitario)
//        {
//            Id = id;
//            DataCompra = dataCompra;
//            MateriaPrima = materiaPrima;
//            Quantidade = quantidade;
//            ValorUnitario = valorUnitario;
//            TotalItem = quantidade * valorUnitario;
//        }

//        public int Id { get; set; } //5 campos
//        public DateTime DataCompra { get; set; } //8 campos
//        public string MateriaPrima { get; set; } //6 campos
//        public float Quantidade { get; set; } //5 campos
//        public float ValorUnitario { get; set; } //5 campos
//        public float TotalItem { get; set; } //6 campos

//        public static void Cadastrar(List<ItemCompra> itensCompra) => new ManipulaArquivosCompraMP().Salvar(itensCompra);

//        public override string ToString() => $"{Id.ToString().PadLeft(5, '0')}" +
//                                            $"{DataCompra.ToString("dd/MM/yyyy").Replace("/", "")}" +
//                                            $"{MateriaPrima}" +
//                                            $"{Quantidade.ToString().Replace(".", "").PadLeft(5, '0')}" +
//                                            $"{ValorUnitario.ToString().Replace(".", "").PadLeft(5, '0')}" +
//                                            $"{TotalItem.ToString().Replace(".", "").PadLeft(6, '0')}";

//        public static void Imprimir()
//        {
//            bool sair = false;
//            int indice = 0;
//            string[] dados = File.ReadAllLines(new ManipulaArquivosCompraMP().CaminhoItemCompra);
//            while (!sair)
//            {
//                Console.WriteLine("1 - Inicio\n2 - Fim\n3 - Anterior\n4 - Proximo");
//                Console.WriteLine("Escolha a opção que deseja: ");
//                int opcao = int.Parse(Console.ReadLine());

//                if (dados.Length <= 0)
//                {
//                    Console.WriteLine("Nenhum Arquivo encontrado!");
//                }

//                switch (opcao)
//                {
//                    case 1:
//                        indice = 0;
//                        Console.WriteLine(dados[indice]);
//                        break;
//                    case 2:
//                        indice = dados.Length - 1;
//                        Console.WriteLine(dados[indice]);
//                        break;
//                    case 3:
//                        if (indice == 0)
//                        {
//                            Console.WriteLine("Não há opções anteriores.");
//                        }
//                        else
//                        {
//                            indice--;
//                            Console.WriteLine(dados[indice]);
//                        }
//                        break;
//                    case 4:
//                        if (indice == dados.Length - 1)
//                        {
//                            Console.WriteLine("Não há opções posteriores.");
//                        }
//                        else
//                        {
//                            indice++;
//                            Console.WriteLine(dados[indice]);
//                        }
//                        break;
//                }
//            }
//        }

//        public bool TotalMaximo() => TotalItem > 99999.99f;
//    }

//    public class ManipulaArquivosCompraMP
//    {
//        public string Diretorio { get; set; }
//        public string CaminhoCompra { get; set; }
//        public string CaminhoItemCompra { get; set; }

//        public ManipulaArquivosCompraMP()
//        {
//            Diretorio = SetCaminhoDiretorio();
//            CaminhoCompra = SetCaminhoArquivo("Compra.dat");
//            CaminhoItemCompra = SetCaminhoArquivo("ItemCompra.dat");
//        }

//        public int PegarUltimoId() => ProcuraUltimo();

//        public void Salvar(Compra compra)
//        {
//            try
//            {
//                using (StreamWriter sw = new(CaminhoCompra, append: true))
//                {
//                    sw.WriteLine(compra);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Não foi possível escrever no arquivo de Compras: " + ex.Message);
//            }
//        }

//        public void Salvar(List<ItemCompra> itensCompra)
//        {
//            try
//            {
//                using (StreamWriter sw = File.AppendText(CaminhoItemCompra))
//                {
//                    itensCompra.ForEach(item => sw.WriteLine(item));
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Não foi possível escrever no arquivo ItemCompra.dat: " + ex.Message);
//            }
//        }

//        public Compra Procura(int idProcura)
//        {
//            Compra procura = null;
//            string linha;
//            try
//            {
//                using (StreamReader sr = new(CaminhoCompra))
//                {
//                    if (File.ReadAllLines(CaminhoCompra).Length != 0)
//                    {
//                        while ((linha = sr.ReadLine()) != null)
//                        {
//                            if (Compra.ExtrairId(linha) == idProcura && !new Read().ProcurarCNPJBloqueado(Compra.ExtrairCNPJ(linha)))
//                            {
//                                procura = Compra.ExtrairCompra(linha);
//                                return procura;
//                            }
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("Arquivo vazio");
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Não foi possivel ler o arquivo Compra.dat: " + ex.Message);
//            }
//            return procura;
//        }

//        public int ProcuraUltimo()
//        {
//            try
//            {
//                return File.ReadAllLines(CaminhoCompra).Length + 1;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine("Não foi possivel ler o arquivo Compra.dat: " + ex.Message);
//            }

//            return 1;
//        }

//        public List<Compra> PegarTodasAsCompras()
//        {
//            var arquivoCompleto = new List<Compra>();
//            var dadosArquivo = File.ReadAllLines(CaminhoCompra).ToList();
//            if (dadosArquivo.Count != 0 || dadosArquivo != null) dadosArquivo.ForEach(linha => arquivoCompleto.Add(Compra.ExtrairCompra(linha)));
//            else Console.WriteLine("Arquivo nao encontrado ou vazio!");
//            return arquivoCompleto;
//        }

//        static string SetCaminhoDiretorio()
//        {
//            string caminho = Path.Combine(Directory.GetCurrentDirectory(), "DataBase");
//            if (!Directory.Exists(caminho)) Directory.CreateDirectory(caminho);
//            return caminho;
//        }

//        string SetCaminhoArquivo(string arquivo)
//        {
//            string arquivoDat = Path.Combine(Diretorio, arquivo);
//            if (!File.Exists(arquivoDat))
//                File.Create(arquivoDat).Close();
//            return arquivoDat;
//        }

//    }
//}
