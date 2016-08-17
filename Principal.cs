using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Baiana20
{
    public partial class Baiana20 : Form
    {
        private string diretorioSubversion;
        private string diretorioBsversion;

        private List<string> eventoLog = new List<string>();

        BarraDeProgresso pbProgresso = new BarraDeProgresso();
        public Baiana20()
        {
            InitializeComponent();
            CarregarPredefinicoes();
            
            diretorioBsversion = PathBsversion.Text;
            diretorioSubversion = PathSubversion.Text;

            this.Controls.Add(pbProgresso);
            pbProgresso.AccessibleRole = System.Windows.Forms.AccessibleRole.ProgressBar;
            pbProgresso.Location = new System.Drawing.Point(11, 331);
            pbProgresso.Name = "PbProgresso";
            pbProgresso.Size = new System.Drawing.Size(694, 23);
            pbProgresso.Style = ProgressBarStyle.Blocks;

        }

        private void CarregarPredefinicoes()
        {
            var currentDirectory = Application.StartupPath.Replace("\\bin\\Debug", "\\");
            var configurationFile = currentDirectory + "\\configuracoes.xml";
            if (File.Exists(configurationFile))
            {
                XmlSerializer reader = new XmlSerializer(typeof(Configuracoes));
                using (FileStream input = File.OpenRead(configurationFile))
                {
                    Configuracoes configuracao = reader.Deserialize(input) as Configuracoes;
                    PathSubversion.Text = configuracao.DiretorioSubversion;
                    PathBsversion.Text = configuracao.DiretorioBsversion;
                    CbFecharTerminar.Checked = configuracao.FecharTerminar;
                    CbCompilarTodos.Checked = configuracao.CompilarTodos;
                }
            }
            BtnCopiaDLLS.Enabled = false;
            CbFecharTerminar.Enabled = false;
        }

        #region Selecionar o caminho

        private void BtnSubversion_Click(object sender, EventArgs e)
        {
            PathSubversion.Text = SelecionarPasta(diretorioSubversion);
        }

        private void BtnBsversion_Click(object sender, EventArgs e)
        {
            PathBsversion.Text = SelecionarPasta(diretorioBsversion);
        }

        private string SelecionarPasta(string diretorio)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            DialogResult resultado = dialog.ShowDialog();
            if (resultado == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return diretorio;
        }

        #endregion

        private void BtnSalvarDiretorios_Click(object sender, EventArgs e)
        {
            var currentDirectory = Application.StartupPath.Replace("\\bin\\Debug", "\\");
            var configuracao = new Configuracoes();
            configuracao.DiretorioSubversion = PathSubversion.Text;
            configuracao.DiretorioBsversion = PathBsversion.Text;
            configuracao.FecharTerminar = CbFecharTerminar.Checked;
            configuracao.CompilarTodos = CbCompilarTodos.Checked;
            var configurationFile = currentDirectory + "\\configuracoes.xml";
            if (File.Exists(configurationFile))
            {
                File.Delete(configurationFile);
                File.CreateText(configurationFile).Close();
            }

            XmlSerializer writer = new XmlSerializer(typeof(Configuracoes));
            using (FileStream file = File.OpenWrite(configurationFile))
            {
                writer.Serialize(file, configuracao);
            }
            LbMensagemAlerta.Text = "Diretórios salvos!";
        }

        private void MensagemProcesso(string mensagem, Color corTexto)
        {
            LbMensagemAlerta.Text = mensagem;
            LbMensagemAlerta.ForeColor = corTexto;
            eventoLog.Add(LbMensagemAlerta.Text);
            
            boxLog.Text = string.Join("\r\n", eventoLog);
            boxLog.SelectionStart = boxLog.TextLength;
            boxLog.ScrollToCaret();
            boxLog.Refresh();
        }

        private string ValorParametroSelecionado()
        {
            var especifico = RbEspecifico;
            var sisAtualizacao = RbSisAtualizacao;
            var sisVerificacao = RbSisVerificacao;
            var bsWebService = RbBSWebService;
            var selecionado = string.Empty;

            if (especifico.Checked == true)
            {
                selecionado = "Especifico";
            }
            else if (sisAtualizacao.Checked == true)
            {
                selecionado = "Sisatualizacao";
            }
            else if (sisVerificacao.Checked == true)
            {
                selecionado = "Sisverificacao";
            }
            else if (bsWebService.Checked == true)
            {
                selecionado = "Bswebservice";
            }

            return selecionado;
        }

        private bool ValidarDiretoriosSelecionados()
        {
            bool valido = false;

            if (!Directory.Exists(diretorioSubversion) || !Directory.Exists(diretorioBsversion))
                return false;
            var subDiretoriosSubVersion = Directory.GetDirectories(diretorioSubversion);
            var subDiretoriosBsVersion = Directory.GetDirectories(diretorioBsversion);
            var contemPastasSV = subDiretoriosSubVersion.Any(x => x.Contains("ESPECIFICOCLIENTES"));
            var comtemPastasBs = subDiretoriosBsVersion.Any(x => x.Contains("ESPECIFICOCLIENTES"));

            if (contemPastasSV)
                valido = true;
            else
                return false;
            if (comtemPastasBs)
                valido = true;
            else
                return false;

            return valido;
        }

        private void ValidarBtnCopiar()
        {
            BtnSalvarDiretorios.Enabled = false;
            if (!string.IsNullOrEmpty(PathSubversion.Text) && !string.IsNullOrEmpty(PathBsversion.Text))
            {
                BtnCopiaDLLS.Enabled = false;
                BtnSalvarDiretorios.Enabled = true;
            }

        }

        private void PathSubversion_TextChanged(object sender, EventArgs e)
        {
            ValidarBtnCopiar();
            var caminhoSubversion = PathSubversion.Text;

            var aplicacoesDisponiveis = DetectarAplicacoesDisponiveis(caminhoSubversion);
            if (aplicacoesDisponiveis.Any())
            {
                BoxListaClientes_Enter(caminhoSubversion);
            }
            else
            {
                DeletarCheckBox();
            }
        }

        private void PathBsversion_TextChanged(object sender, EventArgs e)
        {
            ValidarBtnCopiar();
        }

        private List<string> DetectarAplicacoesDisponiveis(string caminhoSubversion)
        {
            var aplicacoesSubversion = new List<string>();

            aplicacoesSubversion = EncontrarAplicacoes(caminhoSubversion);
            var aplicacoesDisponiveis = aplicacoesSubversion.Select(x => x.ToString()).Intersect(aplicacoesSubversion);
            return aplicacoesDisponiveis.ToList();
        }

        private static List<string> EncontrarAplicacoes(string caminho)
        {
            var aplicacoes = new List<string>();
            if (Directory.Exists(caminho))
            {
                var subDiretoriosSubversion =
                    Directory.GetDirectories(caminho).Where(x => x.Contains("ESPECIFICOCLIENTES"));
                if (subDiretoriosSubversion.Any())
                {
                    DirectoryInfo Dir = new DirectoryInfo(caminho + "\\Delphi\\ESPECIFICOCLIENTES");
                    DirectoryInfo[] Files = Dir.GetDirectories();

                    foreach (DirectoryInfo item in Files)
                    {
                        aplicacoes.Add(item.Name);
                    }
                }
            }
            return aplicacoes;
        }

        private void BoxListaClientes_Enter(string caminhoSubversion)
        {
            if (!string.IsNullOrEmpty(caminhoSubversion))
            {
                var aplicacoesDisponiveis = DetectarAplicacoesDisponiveis(caminhoSubversion);

                GerarCheckBox(aplicacoesDisponiveis);

            }
        }

        private void GerarCheckBox(List<string> aplicacoesDisponiveis)
        {
            CheckBox box;
            int innitialOffset = 25;
            int xDistance = 150;
            int yDistance = 30;

            for (int i = 0; i < aplicacoesDisponiveis.Count; i++)
            {

                box = new CheckBox { Padding = new Padding { Top = 90 } };
                box.Name = "CheckBox";
                box.Tag = aplicacoesDisponiveis[i];
                box.Text = aplicacoesDisponiveis[i];
                box.AutoSize = true;
                box.Location = new Point(innitialOffset + i % 4 * xDistance, innitialOffset + i / 4 * yDistance);

                this.Controls.Add(box);
            }
        }

        private void DeletarCheckBox()
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (Control c in this.Controls)
                {
                    if (c.GetType().Name == "CheckBox")
                    {
                        this.Controls.Remove(c);
                    }
                }
            }
        }

        private List<string> DetectarAplicacoesSelecionadas()
        {
            List<string> resultados = new List<string>();
            foreach (Control c in this.Controls)
            {
                if (c.GetType().Name == "CheckBox")
                {
                    if (((CheckBox)c).Checked == true)
                    {
                        if (!resultados.Contains(c.Text))
                        {
                            resultados.Add(c.Text);
                        }

                    }
                    if (((CheckBox)c).Text == "Compilar Todos" || ((CheckBox)c).Text == "Fechar ao Terminar")
                    {
                        resultados.Remove(c.Text);
                    }
                }

            }
            return resultados;
        }

        private void CbCompilarTodos_CheckedChanged(object sender, EventArgs e)
        {
            if (CbCompilarTodos.Checked == true)
            {
                foreach (Control c in this.Controls)
                {
                    if (c.GetType().Name == "CheckBox")
                    {
                        ((CheckBox)c).Checked = true;
                    }
                }
            }
            else
            {
                foreach (Control c in this.Controls)
                {
                    if (c.GetType().Name == "CheckBox")
                    {
                        ((CheckBox)c).Checked = false;
                    }
                }

            }
        }

        private void BtnCompilar_Click(object sender, EventArgs e)
        {
            int error = 0;
            Brush cor = Brushes.Black;
            boxLog.Clear();
            eventoLog.Clear();
            LbMensagemAlerta.Text = string.Empty;
            pbProgresso.Color = cor;
            pbProgresso.Value = 0;
            
            DirectoryInfo DirDll = new DirectoryInfo(PathSubversion.Text + "\\ESPECIFICOCLIENTES");
            DirectoryInfo DirDproj = new DirectoryInfo(PathSubversion.Text + "\\Delphi\\");

            var caminhoDllOperadora = Directory.GetDirectories(DirDll.ToString()).ToList();
            var diretorioACompilar = Directory.GetDirectories(DirDproj.ToString()).ToList();

            if (!ValidarDiretoriosSelecionados())
            {
                MensagemProcesso("Diretório invalido detectado, por favor revise os caminhos selecionados!", Color.Red);
                new ArgumentNullException();
            }
            else
            {
                MensagemProcesso("Compilando dproj da(s) operadora(s) selecionada(s)...\r\n", Color.Blue);
                pbProgresso.Visible = true;
                pbProgresso.Minimum = 0;

                var aplicacoesSelecionadas = DetectarAplicacoesSelecionadas();
                var rbSelecionado = ValorParametroSelecionado();

                if (aplicacoesSelecionadas.Any())
                {
                    if (ValorParametroSelecionado().Any())
                    {
                        var diretorioFiltrado = diretorioACompilar.First(x => x.Contains(rbSelecionado.ToUpper()));

                        pbProgresso.Maximum = aplicacoesSelecionadas.Count;

                        if (!string.IsNullOrEmpty(diretorioFiltrado))
                        {
                            foreach (var operadoraSelecionada in aplicacoesSelecionadas)
                            {
                                var dirDllOperadora = caminhoDllOperadora.First(x => x.Contains(operadoraSelecionada));
                                var contemPastaDLLL = Path.Combine(dirDllOperadora + "\\DLLS");

                                if (!Directory.Exists(contemPastaDLLL))
                                    Directory.CreateDirectory(contemPastaDLLL);

                                AlterarParaReleaseEAlterarOutputDproj(operadoraSelecionada, rbSelecionado, diretorioFiltrado);
                                error = AlterarBatEProcessar(rbSelecionado, diretorioFiltrado, operadoraSelecionada, aplicacoesSelecionadas.Count);
                            }

                            if (error == 0)
                            {
                                MensagemProcesso("\r\n" + rbSelecionado + ".DLL compilada com sucesso para as aplicações!", Color.Blue);
                            }
                            else if (error == 1)
                            {
                                MensagemProcesso("\r\n Erro ao compilar " + rbSelecionado + "Dll!", Color.Red);
                                cor = Brushes.Red;
                            }
                            else
                            {
                                MensagemProcesso("\r\n" + rbSelecionado + ".DLL Não foi compilada para uma ou mais aplicações!", Color.Red);
                                cor = Brushes.Yellow;
                            }
                        }

                        pbProgresso.Color = cor;
                        pbProgresso.PerformStep();
                        DesabilitaHabilitarOpcoesAposCompilar();
                    }
                    else
                    {
                        MensagemProcesso("Selecione ao menos uma DLL para compilar e copiar!", Color.Red);
                        new ArgumentNullException();
                    }
                }
                else
                {
                    MensagemProcesso("Selecione ao menos uma Operadora!", Color.Red);
                    new ArgumentNullException();
                }
            }
        }

        private void DesabilitaHabilitarOpcoesAposCompilar()
        {
            BtnCopiaDLLS.Enabled = true;
            CbFecharTerminar.Enabled = true;
        }

        private void AlterarParaReleaseEAlterarOutputDproj(string operadora, string rbSelecionado, string diretorioFiltrado)
        {
            var diretorioTmp = diretorioFiltrado + "\\tmp";
            if (!Directory.Exists(diretorioTmp))
                Directory.CreateDirectory(diretorioTmp);



            var arqDproj = rbSelecionado + ".dproj";

            var origemDproj = Path.Combine(diretorioFiltrado, arqDproj);
            var destinoDprojTmp = Path.Combine(diretorioTmp, arqDproj);

            File.Copy(origemDproj, destinoDprojTmp, true);

            using (StreamReader lendo = new StreamReader(origemDproj))
            {
                using (StreamWriter gravando = new StreamWriter(destinoDprojTmp))
                {
                    int linha = 0;
                    string line;

                    while ((line = lendo.ReadLine()) != null)
                    {
                        if (line.TrimStart().StartsWith("<DCC_ExeOutput>..\\") || line.TrimStart().StartsWith("<DCC_UnitSearchPath>$"))
                        {
                            if (line.Contains("<DCC_ExeOutput>..\\"))
                            {
                                line = String.Format(@"<DCC_ExeOutput>..\..\ESPECIFICOCLIENTES\{0}\DLLS\</DCC_ExeOutput>", operadora);
                            }
                            else if (line.Contains("$(DCC_UnitSearchPath"))
                            {
                                line = String.Format(@"<DCC_UnitSearchPath>$(DELPHI)\Lib\Debug;..\COMUNS;c:\benner\bin170;..\ESPECIFICOCLIENTES\{0};$(DCC_UnitSearchPath)</DCC_UnitSearchPath>", operadora);
                            }
                            else
                            {
                                line = String.Format(@"<DCC_UnitSearchPath>$(DELPHI)\Lib\Debug;..\COMUNS;c:\benner\bin170;..\ESPECIFICOCLIENTES\{0}</DCC_UnitSearchPath>", operadora);
                            }
                        }

                        if (line.Contains("<Config Condition=\"'$(Config)'==''\">Debug</Config>"))
                        {
                            line = "<Config Condition=\"'$(Config)'==''\">Release</Config>";
                        }

                        gravando.WriteLine(line);
                        linha++;
                    }

                    lendo.Close();
                    gravando.Flush();
                    gravando.Close();

                    File.Copy(destinoDprojTmp, origemDproj, true);
                    File.Delete(destinoDprojTmp);
                    Directory.Delete(diretorioTmp);
                }
            }

        }

        private int AlterarBatEProcessar(string rbSelecionado, string aplicacao, string operadora, int quantAplicacoes)
        {
            int linha = 0;
            string line;
            string caminho = "";
            int error;


            var arqBat = "Baiana20.bat";
            var diretorioBat = new DirectoryInfo(Directory.GetCurrentDirectory());
            var diretorioTmp = Path.Combine(diretorioBat.ToString(), "tmp");

            if (!Directory.Exists(diretorioTmp))
                Directory.CreateDirectory(diretorioTmp);

            var origemBat = Path.Combine(diretorioBat.ToString(), arqBat);
            var destinoTmp = Path.Combine(diretorioTmp, arqBat);

            File.Copy(origemBat, destinoTmp, true);

            using (StreamReader lendo = new StreamReader(origemBat))
            {
                using (StreamWriter gravando = new StreamWriter(destinoTmp))
                {
                    while ((line = lendo.ReadLine()) != null)
                    {
                        if (line.StartsWith("MSBuild"))
                        {
                            var caminhoProjeto = aplicacao + "\\" + rbSelecionado;
                            line = line.Replace("[CAMINHO][PROJETO]", caminhoProjeto + ".dproj").Replace("[ERRORSLOG]", aplicacao + "\\" + "saida.txt");
                            caminho = caminhoProjeto;
                        }

                        gravando.WriteLine(line);
                        linha++;
                    }

                    lendo.Close();
                    gravando.Flush();
                    gravando.Close();
                }

                error = ProcessarBat(destinoTmp, rbSelecionado, operadora, quantAplicacoes);

                File.Delete(destinoTmp);
                Directory.Delete(diretorioTmp);
            }
            return error;
        }

        private int ProcessarBat(string diretorioBat, string rbSelecionado, string operadora, int quantAplicacoes)
        {
            int error = 0;

            Process process = new Process();
            process.StartInfo.FileName = diretorioBat;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardError = true;

            process.Start();

            var erro = process.StandardError.ReadToEnd();

            if (erro.Length < 0)
            {
                MensagemProcesso(String.Format("Arquivo {0} compilado com Sucesso para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Blue);
                pbProgresso.Step = 1;
                pbProgresso.PerformStep();
            }
            else if (quantAplicacoes <= 1)
            {
                MensagemProcesso(String.Format("Falha ao compilar {0} para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Red);
                error = 1;
                pbProgresso.Color = Brushes.Red;
                pbProgresso.Step = 1;
                pbProgresso.PerformStep();
            }
            else
            {
                MensagemProcesso(String.Format("Falha ao compilar {0} para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Red);

                error = 2;
                pbProgresso.Color = Brushes.Yellow;
                pbProgresso.Step = 1;
                pbProgresso.PerformStep();
            }

            process.WaitForExit();

            return error;
        }

        private void BtnCopiaDLLS_Click(object sender, EventArgs e)
        {
            boxLog.Clear();
            eventoLog.Clear();
            LbMensagemAlerta.Text = string.Empty;
            pbProgresso.Color = Brushes.Green;
            pbProgresso.Value = 0;

            try
            {
                if (!ValidarDiretoriosSelecionados())
                {
                    MensagemProcesso("Diretório invalido detectado, por favor revise os caminhos selecionados!", Color.Red);
                    new ArgumentNullException();
                }
                else
                {
                    MensagemProcesso("Copiando DLLS...\r\n", Color.Blue);

                    var aplicacoesBsversion =
                        Directory.GetDirectories(diretorioBsversion)
                            .Where(x => x.Contains("ESPECIFICOCLIENTES"))
                            .ToList();

                    pbProgresso.Visible = true;
                    pbProgresso.Minimum = 0;

                    pbProgresso.Maximum = aplicacoesBsversion.Count;
                    pbProgresso.Step = 1;

                    var aplicacoesSelecionadas = DetectarAplicacoesSelecionadas();
                    try
                    {
                        if (aplicacoesSelecionadas.Any())
                        {
                            try
                            {
                                if (ValorParametroSelecionado().Any())
                                {
                                    Copiar(aplicacoesSelecionadas);

                                    pbProgresso.PerformStep();
                                    MensagemProcesso("Arquivos copiados com sucesso!", Color.Blue);

                                    if (CbFecharTerminar.Checked)
                                        this.Close();
                                }
                                else
                                {
                                    MensagemProcesso("Selecione ao menos uma DLL para compilar e copiar!", Color.Red);
                                    new ArgumentNullException();
                                }
                            }
                            catch (ArgumentNullException)
                            {
                                boxLog.Controls.Owner.Text = eventoLog.ToString();
                                throw;
                            }
                            catch (UnauthorizedAccessException)
                            {
                                boxLog.Controls.Owner.Text =
                                    "Usuário sem permissão de acesso, execute em modo administrador";
                            }

                        }
                        else
                        {
                            MensagemProcesso("Selecione ao menos uma Operadora!", Color.Red);
                            new ArgumentNullException();
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        boxLog.Controls.Owner.Text = eventoLog.ToString();
                    }
                }
            }
            catch (ArgumentNullException)
            {
                boxLog.Controls.Owner.Text = eventoLog.ToString();
                throw;
            }

        }

        private static void RetornarOperadoraFiltrada(List<string> aplicacoesSelecionadas, List<string> aplicacoesSubversion,
           List<string> aplicacoesSubversionFiltrada, List<string> aplicacoesBsversion, List<string> aplicacoesBsversionFiltrada)
        {
            foreach (var item in aplicacoesSelecionadas)
            {
                var itemFiltradoVS = aplicacoesSubversion.First(x => x.Contains(item));
                if (!string.IsNullOrEmpty(itemFiltradoVS))
                    aplicacoesSubversionFiltrada.Add(itemFiltradoVS);

                var itemFiltradoBS = aplicacoesBsversion.First(x => x.Contains(item));
                if (!string.IsNullOrEmpty(itemFiltradoBS))
                    aplicacoesBsversionFiltrada.Add(itemFiltradoBS);
            }
        }

        private void Copiar(List<string> aplicacoesSelecionadas)
        {
            DirectoryInfo DirVS = new DirectoryInfo(diretorioSubversion + "\\ESPECIFICOCLIENTES");
            DirectoryInfo DirBS = new DirectoryInfo(diretorioBsversion + "\\ESPECIFICOCLIENTES");

            var aplicacoesSubversion =
                Directory.GetDirectories(DirVS.ToString()).Where(x => x.Contains("ESPECIFICOCLIENTES")).ToList();
            var aplicacoesBsversion =
                Directory.GetDirectories(DirBS.ToString()).Where(x => x.Contains("ESPECIFICOCLIENTES")).ToList();

            List<string> aplicacoesSubversionFiltrada = new List<string>();
            List<string> aplicacoesBsversionFiltrada = new List<string>();

            try
            {
                var dllACopiar = ValorParametroSelecionado();

                if (dllACopiar == String.Empty)
                {
                    MensagemProcesso("Nenhuma Opção de DLL selecionada", Color.Red);
                    new ArgumentNullException();
                }
                else
                {
                    RetornarOperadoraFiltrada(aplicacoesSelecionadas, aplicacoesSubversion, aplicacoesSubversionFiltrada,
                        aplicacoesBsversion, aplicacoesBsversionFiltrada);

                    foreach (var aplicacaoSubVersion in aplicacoesSubversionFiltrada)
                    {

                        foreach (var aplicacaoBsVersion in aplicacoesBsversionFiltrada)
                        {
                            var operadoraNoSub =
                                aplicacaoSubVersion.Substring(aplicacaoSubVersion.IndexOf("ESPECIFICOCLIENTES\\"));
                            var operadoNoBs =
                                aplicacaoBsVersion.Substring(aplicacaoSubVersion.IndexOf("ESPECIFICOCLIENTES\\"));
                            if (operadoraNoSub.EndsWith(operadoNoBs))
                            {
                                var caminhoDllSV = aplicacaoSubVersion + "\\DLLS";
                                var arquivoOrigem = Path.Combine(caminhoDllSV, dllACopiar + ".dll");
                                var arquivoDestino = Path.Combine(aplicacaoBsVersion, dllACopiar.ToUpper() + ".dll");

                                if (!File.Exists(arquivoOrigem))
                                {
                                    MensagemProcesso(String.Format("{0}.dll não existe na pasta de origem, tente compilar novamente. {1}!",
                                     dllACopiar.ToUpper(), arquivoOrigem), Color.Red);
                                }
                                else
                                {
                                    File.Copy(arquivoOrigem, arquivoDestino, true);
                                    MensagemProcesso(String.Format("{0}.dll copiada com sucesso para o destino {1}!",
                                        dllACopiar.ToUpper(), aplicacaoBsVersion), Color.Red);
                                }
                            }
                            boxLog.Controls.Owner.Text = eventoLog.ToString();
                        }
                    }

                    pbProgresso.Maximum = 1;
                    pbProgresso.Minimum = 0;
                    pbProgresso.PerformStep();
                    MensagemProcesso("Dlls copiados com sucesso!", Color.Blue);
                    if (CbFecharTerminar.Checked)
                        this.Close();
                }
            }
            catch (ArgumentNullException)
            {
                boxLog.Controls.Owner.Text = eventoLog.ToString();
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                boxLog.Controls.Owner.Text =
                    "Usuário sem permissão de acesso, execute a Baiana2.0 em modo administrador";
                throw;
            }
        }

    }
}
