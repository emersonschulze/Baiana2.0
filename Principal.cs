using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Baiana20
{
    public partial class Baiana20 : Form
    {
        private string diretorioSubversion;
        private string diretorioBsversion;
        private int gerouErro = 0;
        private string caminhoOrigemDproj;
        private string caminhoTmpDproj;
        private string caminhoTmp2;
        private SelecionaPasta selecionaPasta = new SelecionaPasta();

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
                    configuracao.RbEspecifico = true;
                    PathSubversion.Text = configuracao.DiretorioSubversion;
                    PathBsversion.Text = configuracao.DiretorioBsversion;
                    CbFecharTerminar.Checked = configuracao.FecharTerminar;
                    RbEspecifico.Checked = configuracao.RbEspecifico;
                    CbCompilarTodos.Checked = configuracao.CompilarTodos;
                }
            }
            BtnCopiaDLLS.Enabled = false;
            CbFecharTerminar.Enabled = false;
        }

        #region Botão o caminho

        private void BtnSubversion_Click(object sender, EventArgs e)
        {
            PathSubversion.Text = selecionaPasta.SelecionarPasta(diretorioSubversion);
        }

        private void BtnBsversion_Click(object sender, EventArgs e)
        {
            PathBsversion.Text = selecionaPasta.SelecionarPasta(diretorioBsversion);
        }

        private void BtnSalvarDiretorios_Click(object sender, EventArgs e)
        {
            try
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
                    file.Flush();
                }
                LbMensagemAlerta.Text = "Diretórios salvos!";

                diretorioBsversion = PathBsversion.Text;
                diretorioSubversion = PathSubversion.Text;
            }
            catch (UnauthorizedAccessException)
            {
                MensagemProcesso("Usuário sem permissão de acesso, execute em modo administrador", Color.Red);
            }
        }

        #endregion

       
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
            var selecionado = String.Empty;
            IEnumerable<RadioButton> buttons = Controls.OfType<RadioButton>() ;

            foreach (var dll in buttons)
            {
                if (dll.Checked)
                {
                   var text = dll.Text;
                    switch (text)
                    {
                        case "Específico":
                            selecionado = "Especifico";
                            break;
                        case "SisAtualização":
                            selecionado = "SisAtualizacaoEspec";
                            break;
                        case "SisVerificação":
                            selecionado = "SisVerificacaoEspec";
                            break;
                        case "BSWebService":
                            selecionado = "Bswebservice";
                            break; 
                    }
                }
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
            diretorioSubversion = PathSubversion.Text;
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
            diretorioBsversion = PathBsversion.Text;
            ValidarBtnCopiar();
        }

        private List<string> DetectarAplicacoesDisponiveis(string caminhoSubversion)
        {
            var aplicacoesSubversion = new List<string>();

            aplicacoesSubversion = EncontrarAplicacoes(caminhoSubversion);
            var aplicacoesDisponiveis = aplicacoesSubversion.Select(x => x.ToString()).Intersect(aplicacoesSubversion);
            return aplicacoesDisponiveis.ToList();
        }

        private List<string> EncontrarAplicacoes(string caminho)
        {

            var aplicacoes = new List<string>();
            if (Directory.Exists(caminho))
            {
                DirectoryInfo Dir = null;

                var subDiretoriosSubversion =
                    Directory.GetDirectories(caminho).Where(x => x.Contains("ESPECIFICOCLIENTES"));
                if (subDiretoriosSubversion.Any())
                {
                    DirectoryInfo[] Files = new DirectoryInfo[] { };

                    if (diretorioSubversion.Contains("Corrente"))
                    {
                        Dir = new DirectoryInfo(caminho + "\\Delphi\\ESPECIFICOCLIENTES");
                        Files = Dir.GetDirectories();
                    }
                    else
                    {
                        var versao = Regex.Match(diretorioSubversion, @"\b\d\d");
                        if (Convert.ToInt32(versao.Value) > 42)
                        {
                            Dir = new DirectoryInfo(caminho + "\\Delphi\\ESPECIFICOCLIENTES");
                            Files = Dir.GetDirectories();
                        }
                        else
                        {
                            Dir = new DirectoryInfo(caminho + "\\ESPECIFICOCLIENTES");
                            Files = Dir.GetDirectories();
                        }
                    }

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
            int xDistance = 140;
            int yDistance = 30;

            for (int i = 0; i < aplicacoesDisponiveis.Count; i++)
            {

                box = new CheckBox { Padding = new Padding { Top = 2 } };
                box.Name = "CheckBox";
                box.Tag = aplicacoesDisponiveis[i];
                box.Text = aplicacoesDisponiveis[i];
                box.AutoSize = true;
                box.Location = new Point(innitialOffset + i % 5 * xDistance, innitialOffset + i / 5 * yDistance);

                this.gBOperadoras.Controls.Add(box);
            }
        }

        private void DeletarCheckBox()
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (Control c in this.gBOperadoras.Controls)
                {
                    if (c.GetType().Name == "CheckBox")
                    {
                        if (((CheckBox)c).Text != "Compilar Todos" && ((CheckBox)c).Text != "Fechar ao Terminar")
                            this.gBOperadoras.Controls.Remove(c);
                    }
                }
            }
        }

        private List<string> DetectarAplicacoesSelecionadas()
        {
            List<string> resultados = new List<string>();
            foreach (Control c in this.gBOperadoras.Controls)
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
                foreach (Control c in this.gBOperadoras.Controls)
                {
                    if (c.GetType().Name == "CheckBox")
                    {
                        ((CheckBox)c).Checked = true;

                        if (((CheckBox)c).Text == "Fechar ao Terminar")
                            ((CheckBox)c).Checked = false;
                    }
                }
            }
            else
            {
                foreach (Control c in this.gBOperadoras.Controls)
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
            gerouErro = 0;
            try
            {
                if (!ValidarDiretoriosSelecionados())
                {
                    MensagemProcesso("Diretório invalido detectado, por favor revise os caminhos selecionados!", Color.Red);
                    new ArgumentNullException();
                }
                else
                {
                    Brush cor = Brushes.LimeGreen;
                    boxLog.Clear();
                    eventoLog.Clear();
                    LbMensagemAlerta.Text = string.Empty;
                    pbProgresso.Color = cor;
                    pbProgresso.Value = 0;

                    DirectoryInfo DirDll = new DirectoryInfo(PathSubversion.Text + "\\ESPECIFICOCLIENTES");
                    DirectoryInfo DirDproj;

                    if (PathSubversion.Text.Contains("Corrente"))
                    {
                        DirDproj = new DirectoryInfo(PathSubversion.Text + "\\Delphi\\");
                    }
                    else
                    {
                        var versao = Regex.Match(diretorioSubversion, @"\b\d\d");
                        if (Convert.ToInt32(versao.Value) > 42)
                        {
                            DirDproj = new DirectoryInfo(PathSubversion.Text + "\\Delphi\\");
                        }
                        else
                        {
                            DirDproj = new DirectoryInfo(PathSubversion.Text + "\\");
                        }
                    }

                    var caminhoDllOperadora = Directory.GetDirectories(DirDll.ToString()).ToList();
                    var diretorioACompilar = Directory.GetDirectories(DirDproj.ToString()).ToList();

                    MensagemProcesso("Compilando dproj da(s) operadora(s) selecionada(s)...\r\n", Color.Blue);
                    pbProgresso.Visible = true;
                    pbProgresso.Minimum = 0;

                    var aplicacoesSelecionadas = DetectarAplicacoesSelecionadas();
                    var rbSelecionado = ValorParametroSelecionado();

                    if (aplicacoesSelecionadas.Any())
                    {
                        if (ValorParametroSelecionado().Any())
                        {
                            var diretorioFiltrado = diretorioACompilar.Find(x => x.Contains(rbSelecionado.ToUpper()) && !x.Contains("."));

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
                                    AlterarBatEProcessar(rbSelecionado, diretorioFiltrado, operadoraSelecionada, aplicacoesSelecionadas.Count);

                                }

                                if (gerouErro == 0)
                                {
                                    MensagemProcesso("\r\n" + rbSelecionado + ".DLL compilada com sucesso para as aplicações!", Color.Blue);
                                }
                                else if (gerouErro == 1)
                                {
                                    MensagemProcesso("\r\nErro ao compilar " + rbSelecionado + "DLL!", Color.Red);
                                    cor = Brushes.Red;
                                }
                                else
                                {
                                    MensagemProcesso("\r\n" + rbSelecionado + ".DLL Não foi compilada para uma ou mais aplicações!", Color.Red);
                                    cor = Brushes.Yellow;
                                }
                            }

                            File.Copy(caminhoTmpDproj, caminhoOrigemDproj, true);
                            File.Delete(caminhoTmpDproj);
                            Directory.Delete(caminhoTmp2);

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
            catch (UnauthorizedAccessException)
            {
                MensagemProcesso("Usuário sem permissão de acesso, execute em modo administrador", Color.Red);
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

            var diretorioTmpBackup = diretorioFiltrado + "\\tmp2";
            if (!Directory.Exists(diretorioTmpBackup))
                Directory.CreateDirectory(diretorioTmpBackup);

            string arqDproj;
            if (rbSelecionado.Equals("SisAtualizacaoEspec"))
            {
                arqDproj = "SisAtualizacaEspec.dproj"; 
            }
            else
            {
                arqDproj = rbSelecionado + ".dproj";      
            }
            
            var origemDproj = Path.Combine(diretorioFiltrado, arqDproj);
            var destinoDprojTmp = Path.Combine(diretorioTmp, arqDproj);
            var destinoDprojBackup = Path.Combine(diretorioTmpBackup, arqDproj);
            caminhoOrigemDproj = origemDproj;
            caminhoTmpDproj = destinoDprojBackup;
            caminhoTmp2 = diretorioTmpBackup;

            File.Copy(origemDproj, destinoDprojTmp, true);

            if (!File.Exists(caminhoTmpDproj))
                File.Copy(origemDproj, destinoDprojBackup, false);

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
                                if (PathSubversion.Text.Contains("Corrente"))
                                {
                                    line = String.Format(@"<DCC_ExeOutput>..\..\ESPECIFICOCLIENTES\{0}\DLLS\</DCC_ExeOutput>", operadora);
                                }
                                else
                                {
                                    var versao = Regex.Match(diretorioSubversion, @"\b\d\d");
                                    if (Convert.ToInt32(versao.Value) > 42)
                                    {
                                        line = String.Format(@"<DCC_ExeOutput>..\..\ESPECIFICOCLIENTES\{0}\DLLS\</DCC_ExeOutput>", operadora);
                                    }
                                    else
                                    {
                                        line = String.Format(@"<DCC_ExeOutput>..\ESPECIFICOCLIENTES\{0}\DLLS\</DCC_ExeOutput>", operadora);
                                    }
                                }
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

        private void AlterarBatEProcessar(string rbSelecionado, string aplicacao, string operadora, int quantAplicacoes)
        {
            int linha = 0;
            string line;

            var arqBat = "Baiana.bat";
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
                            if (rbSelecionado.Equals("SisAtualizacaoEspec"))
                                rbSelecionado = "SisAtualizacaEspec";

                            var caminhoProjeto = aplicacao + "\\" + rbSelecionado;

                            line = line.Replace("[CAMINHO][PROJETO]", caminhoProjeto + ".dproj").Replace("[ERRORSLOG]", aplicacao + "\\" + "saida.txt");
                        }

                        gravando.WriteLine(line);
                        linha++;
                    }

                    lendo.Close();
                    gravando.Flush();
                    gravando.Close();
                }

                ProcessarBat(destinoTmp, rbSelecionado, operadora, quantAplicacoes, aplicacao);

                File.Delete(destinoTmp);
                Directory.Delete(diretorioTmp);
            }
        }

        private void ProcessarBat(string diretorioBat, string rbSelecionado, string operadora, int quantAplicacoes, string aplicacao)
        {
            Process process = new Process();

            process.StartInfo.FileName = diretorioBat;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            process.WaitForExit();

            var origemSaida = Path.Combine(aplicacao, "saida.txt");

            using (StreamReader lendo = new StreamReader(origemSaida))
            {
                if (String.IsNullOrEmpty(lendo.ReadToEnd()))
                {
                    MensagemProcesso(String.Format("Arquivo {0} compilado com Sucesso para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Blue);
                    pbProgresso.Step = 1;
                    pbProgresso.PerformStep();
                }
                else if (quantAplicacoes <= 1)
                {
                    MensagemProcesso(String.Format("Falha ao compilar {0} para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Red);
                    gerouErro = 1;
                    pbProgresso.Color = Brushes.Red;
                    pbProgresso.Step = 1;
                    pbProgresso.PerformStep();
                }
                else
                {
                    MensagemProcesso(String.Format("Falha ao compilar {0} para Operadora {1}!", rbSelecionado + ".dll", operadora), Color.Red);
                    gerouErro = 2;
                    pbProgresso.Color = Brushes.Yellow;
                    pbProgresso.Step = 1;
                    pbProgresso.PerformStep();
                }
            }

        }

        private void BtnCopiaDLLS_Click(object sender, EventArgs e)
        {
            boxLog.Clear();
            eventoLog.Clear();
            LbMensagemAlerta.Text = string.Empty;
            pbProgresso.Color = Brushes.LimeGreen;
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

                                    if (CbFecharTerminar.Checked && gerouErro == 0)
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
                        MensagemProcesso(eventoLog.ToString(), Color.Red);
                    }
                }
            }
            catch (ArgumentNullException)
            {
                MensagemProcesso(eventoLog.ToString(), Color.Red);
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                MensagemProcesso("Usuário sem permissão de acesso, execute em modo administrador", Color.Red);
            }

        }

        private void Copiar(List<string> aplicacoesSelecionadas)
        {
            Brush cor = Brushes.LimeGreen;
            pbProgresso.Color = cor;

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
                if (dllACopiar.Equals("SisAtualizacaoEspec"))
                    dllACopiar = "SisAtualizacaEspec";
                gerouErro = 0;

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
                                aplicacaoSubVersion.Substring(aplicacaoSubVersion.IndexOf("\\ESPECIFICOCLIENTES\\"));
                            var operadoNoBs =
                                aplicacaoBsVersion.Substring(aplicacaoSubVersion.IndexOf("\\ESPECIFICOCLIENTES\\"));
                            try
                            {
                                if (operadoraNoSub.EndsWith(operadoNoBs))
                                {
                                    var caminhoDllSV = aplicacaoSubVersion + "\\DLLS";
                                    var arquivoOrigem = Path.Combine(caminhoDllSV, dllACopiar + ".dll");
                                    var arquivoDestino = Path.Combine(aplicacaoBsVersion, dllACopiar.ToUpper() + ".dll");

                                    if (!File.Exists(arquivoOrigem))
                                    {
                                        MensagemProcesso(String.Format("{0}.dll não existe na pasta de origem! Tente compilar novamente. {1}.dproj para o caminho {2}!",
                                         dllACopiar.ToUpper(), dllACopiar, caminhoDllSV), Color.Red);
                                        gerouErro = 2;
                                    }
                                    else
                                    {
                                        File.Copy(arquivoOrigem, arquivoDestino, true);
                                        MensagemProcesso(String.Format("{0}.dll copiada com sucesso para o destino {1}!",
                                            dllACopiar.ToUpper(), aplicacaoBsVersion), Color.Red);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                MensagemProcesso(String.Format("Diretório {0} existe no BSVERSION, mas não existe no Subversion!",
                                        operadoNoBs), Color.Red);
                            }

                            boxLog.Controls.Owner.Text = eventoLog.ToString();
                        }
                    }

                    if (gerouErro == 2)
                    {
                        MensagemProcesso("\r\nErro! Uma ou mais DLLS não foram copiadas!", Color.Red);
                        cor = Brushes.Yellow;
                    }
                    else
                    {
                        MensagemProcesso("\r\nDLLS copiadas com sucesso!", Color.Blue);
                    }

                    pbProgresso.Color = cor;
                    pbProgresso.Maximum = 1;
                    pbProgresso.Minimum = 0;
                    pbProgresso.PerformStep();

                    if (CbFecharTerminar.Checked && gerouErro == 0)
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

        private void RetornarOperadoraFiltrada(List<string> aplicacoesSelecionadas, List<string> aplicacoesSubversion,
          List<string> aplicacoesSubversionFiltrada, List<string> aplicacoesBsversion, List<string> aplicacoesBsversionFiltrada)
        {
            foreach (var item in aplicacoesSelecionadas)
            {
                try
                {
                    foreach (var apSubversion in aplicacoesSubversion)
                    {
                        if (apSubversion.ToUpper().Contains(item.ToUpper()))
                            aplicacoesSubversionFiltrada.Add(apSubversion.ToUpper());
                    }
                    if (aplicacoesBsversion.Any())
                    {
                        foreach (var apBsversion in aplicacoesBsversion)
                        {
                            if (apBsversion.ToUpper().Contains(item.ToUpper()))
                                aplicacoesBsversionFiltrada.Add(apBsversion.ToUpper());
                        }
                    }
                    else
                    {
                        gerouErro = 2;
                        new Exception(); MensagemProcesso(String.Format("O diretório da aplicação {0} existe no SUBVERSION mas não existe no BSVERSION!!!", item), Color.Red);
                    }
                }
                catch (Exception)
                {
                    MensagemProcesso(String.Format("O diretório da aplicação {0} existe no SUBVERSION mas não existe no BSVERSION!!!", item), Color.Red);
                }
            }
        }
    }
}
