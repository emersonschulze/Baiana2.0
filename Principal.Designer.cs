using System.Windows.Forms;

namespace Baiana20
{
    partial class Baiana20
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnCompilar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.RbSisAtualizacao = new System.Windows.Forms.RadioButton();
            this.RbSisVerificacao = new System.Windows.Forms.RadioButton();
            this.RbEspecifico = new System.Windows.Forms.RadioButton();
            this.RbBSWebService = new System.Windows.Forms.RadioButton();
            this.PbProgresso = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.PathSubversion = new System.Windows.Forms.TextBox();
            this.PathBsversion = new System.Windows.Forms.TextBox();
            this.CbCompilarTodos = new System.Windows.Forms.CheckBox();
            this.BtnCopiaDLLS = new System.Windows.Forms.Button();
            this.BtnSubversion = new System.Windows.Forms.Button();
            this.BtnBsversion = new System.Windows.Forms.Button();
            this.BtnSalvarDiretorios = new System.Windows.Forms.Button();
            this.LbMensagemAlerta = new System.Windows.Forms.Label();
            this.CbFecharTerminar = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.boxLog = new System.Windows.Forms.TextBox();
            this.btPararProcesso = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnCompilar
            // 
            this.BtnCompilar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnCompilar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCompilar.ForeColor = System.Drawing.Color.Red;
            this.BtnCompilar.Location = new System.Drawing.Point(531, 37);
            this.BtnCompilar.Name = "BtnCompilar";
            this.BtnCompilar.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.BtnCompilar.Size = new System.Drawing.Size(175, 23);
            this.BtnCompilar.TabIndex = 0;
            this.BtnCompilar.Text = "Compilar";
            this.BtnCompilar.UseVisualStyleBackColor = true;
            this.BtnCompilar.Click += new System.EventHandler(this.BtnCompilar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Path SubVersion:";
            // 
            // RbSisAtualizacao
            // 
            this.RbSisAtualizacao.AutoSize = true;
            this.RbSisAtualizacao.Location = new System.Drawing.Point(16, 72);
            this.RbSisAtualizacao.Name = "RbSisAtualizacao";
            this.RbSisAtualizacao.Size = new System.Drawing.Size(94, 17);
            this.RbSisAtualizacao.TabIndex = 3;
            this.RbSisAtualizacao.TabStop = true;
            this.RbSisAtualizacao.Text = "SisAtualização";
            this.RbSisAtualizacao.UseVisualStyleBackColor = true;
            // 
            // RbSisVerificacao
            // 
            this.RbSisVerificacao.AutoSize = true;
            this.RbSisVerificacao.Location = new System.Drawing.Point(119, 72);
            this.RbSisVerificacao.Name = "RbSisVerificacao";
            this.RbSisVerificacao.Size = new System.Drawing.Size(92, 17);
            this.RbSisVerificacao.TabIndex = 4;
            this.RbSisVerificacao.TabStop = true;
            this.RbSisVerificacao.Text = "SisVerificação";
            this.RbSisVerificacao.UseVisualStyleBackColor = true;
            // 
            // RbEspecifico
            // 
            this.RbEspecifico.AutoSize = true;
            this.RbEspecifico.Location = new System.Drawing.Point(220, 72);
            this.RbEspecifico.Name = "RbEspecifico";
            this.RbEspecifico.Size = new System.Drawing.Size(76, 17);
            this.RbEspecifico.TabIndex = 5;
            this.RbEspecifico.TabStop = true;
            this.RbEspecifico.Text = "Específico";
            this.RbEspecifico.UseVisualStyleBackColor = true;
            // 
            // RbBSWebService
            // 
            this.RbBSWebService.AutoSize = true;
            this.RbBSWebService.Location = new System.Drawing.Point(302, 72);
            this.RbBSWebService.Name = "RbBSWebService";
            this.RbBSWebService.Size = new System.Drawing.Size(98, 17);
            this.RbBSWebService.TabIndex = 6;
            this.RbBSWebService.TabStop = true;
            this.RbBSWebService.Text = "BSWebService";
            this.RbBSWebService.UseVisualStyleBackColor = true;
            // 
            // PbProgresso
            // 
            this.PbProgresso.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.PbProgresso.Location = new System.Drawing.Point(11, 331);
            this.PbProgresso.Name = "PbProgresso";
            this.PbProgresso.Size = new System.Drawing.Size(694, 23);
            this.PbProgresso.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Path BSVERSION:";
            // 
            // PathSubversion
            // 
            this.PathSubversion.Location = new System.Drawing.Point(124, 13);
            this.PathSubversion.Name = "PathSubversion";
            this.PathSubversion.Size = new System.Drawing.Size(261, 20);
            this.PathSubversion.TabIndex = 10;
            this.PathSubversion.TextChanged += new System.EventHandler(this.PathSubversion_TextChanged);
            // 
            // PathBsversion
            // 
            this.PathBsversion.Location = new System.Drawing.Point(124, 38);
            this.PathBsversion.Name = "PathBsversion";
            this.PathBsversion.Size = new System.Drawing.Size(261, 20);
            this.PathBsversion.TabIndex = 11;
            this.PathBsversion.TextChanged += new System.EventHandler(this.PathBsversion_TextChanged);
            // 
            // CbCompilarTodos
            // 
            this.CbCompilarTodos.AutoSize = true;
            this.CbCompilarTodos.Location = new System.Drawing.Point(423, 41);
            this.CbCompilarTodos.Name = "CbCompilarTodos";
            this.CbCompilarTodos.Size = new System.Drawing.Size(99, 17);
            this.CbCompilarTodos.TabIndex = 14;
            this.CbCompilarTodos.Text = "Compilar Todos";
            this.CbCompilarTodos.UseVisualStyleBackColor = true;
            this.CbCompilarTodos.CheckedChanged += new System.EventHandler(this.CbCompilarTodos_CheckedChanged);
            // 
            // BtnCopiaDLLS
            // 
            this.BtnCopiaDLLS.Location = new System.Drawing.Point(531, 65);
            this.BtnCopiaDLLS.Name = "BtnCopiaDLLS";
            this.BtnCopiaDLLS.Size = new System.Drawing.Size(175, 23);
            this.BtnCopiaDLLS.TabIndex = 16;
            this.BtnCopiaDLLS.Text = "Copiar DLLS para o BSVERSION";
            this.BtnCopiaDLLS.UseVisualStyleBackColor = true;
            this.BtnCopiaDLLS.Click += new System.EventHandler(this.BtnCopiaDLLS_Click);
            // 
            // BtnSubversion
            // 
            this.BtnSubversion.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSubversion.Location = new System.Drawing.Point(391, 11);
            this.BtnSubversion.Name = "BtnSubversion";
            this.BtnSubversion.Size = new System.Drawing.Size(26, 22);
            this.BtnSubversion.TabIndex = 18;
            this.BtnSubversion.Text = "...";
            this.BtnSubversion.UseVisualStyleBackColor = true;
            this.BtnSubversion.Click += new System.EventHandler(this.BtnSubversion_Click);
            // 
            // BtnBsversion
            // 
            this.BtnBsversion.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnBsversion.Location = new System.Drawing.Point(391, 35);
            this.BtnBsversion.Name = "BtnBsversion";
            this.BtnBsversion.Size = new System.Drawing.Size(26, 23);
            this.BtnBsversion.TabIndex = 19;
            this.BtnBsversion.Text = "...";
            this.BtnBsversion.UseVisualStyleBackColor = true;
            this.BtnBsversion.Click += new System.EventHandler(this.BtnBsversion_Click);
            // 
            // BtnSalvarDiretorios
            // 
            this.BtnSalvarDiretorios.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnSalvarDiretorios.Enabled = false;
            this.BtnSalvarDiretorios.Location = new System.Drawing.Point(423, 12);
            this.BtnSalvarDiretorios.Name = "BtnSalvarDiretorios";
            this.BtnSalvarDiretorios.Size = new System.Drawing.Size(105, 23);
            this.BtnSalvarDiretorios.TabIndex = 20;
            this.BtnSalvarDiretorios.Text = "Salvar Diretorios";
            this.BtnSalvarDiretorios.UseVisualStyleBackColor = true;
            this.BtnSalvarDiretorios.Click += new System.EventHandler(this.BtnSalvarDiretorios_Click);
            // 
            // LbMensagemAlerta
            // 
            this.LbMensagemAlerta.AccessibleRole = System.Windows.Forms.AccessibleRole.Alert;
            this.LbMensagemAlerta.AutoSize = true;
            this.LbMensagemAlerta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.LbMensagemAlerta.ForeColor = System.Drawing.Color.Blue;
            this.LbMensagemAlerta.Location = new System.Drawing.Point(282, 493);
            this.LbMensagemAlerta.Name = "LbMensagemAlerta";
            this.LbMensagemAlerta.Size = new System.Drawing.Size(0, 13);
            this.LbMensagemAlerta.TabIndex = 22;
            this.LbMensagemAlerta.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CbFecharTerminar
            // 
            this.CbFecharTerminar.AutoSize = true;
            this.CbFecharTerminar.Location = new System.Drawing.Point(406, 71);
            this.CbFecharTerminar.Name = "CbFecharTerminar";
            this.CbFecharTerminar.Size = new System.Drawing.Size(118, 17);
            this.CbFecharTerminar.TabIndex = 24;
            this.CbFecharTerminar.Text = "Fechar ao Terminar";
            this.CbFecharTerminar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Selecionar Operadoras";
            // 
            // boxLog
            // 
            this.boxLog.Location = new System.Drawing.Point(11, 360);
            this.boxLog.Multiline = true;
            this.boxLog.Name = "boxLog";
            this.boxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.boxLog.Size = new System.Drawing.Size(694, 130);
            this.boxLog.TabIndex = 27;
            // 
            // btPararProcesso
            // 
            this.btPararProcesso.Location = new System.Drawing.Point(531, 12);
            this.btPararProcesso.Name = "btPararProcesso";
            this.btPararProcesso.Size = new System.Drawing.Size(175, 22);
            this.btPararProcesso.TabIndex = 28;
            this.btPararProcesso.Text = "Parar Processo";
            this.btPararProcesso.UseVisualStyleBackColor = true;
            this.btPararProcesso.Click += new System.EventHandler(this.btPararProcesso_Click);
            // 
            // Baiana20
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(717, 509);
            this.Controls.Add(this.btPararProcesso);
            this.Controls.Add(this.boxLog);
            this.Controls.Add(this.PbProgresso);
            this.Controls.Add(this.CbFecharTerminar);
            this.Controls.Add(this.CbCompilarTodos);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LbMensagemAlerta);
            this.Controls.Add(this.BtnSalvarDiretorios);
            this.Controls.Add(this.BtnBsversion);
            this.Controls.Add(this.BtnSubversion);
            this.Controls.Add(this.BtnCopiaDLLS);
            this.Controls.Add(this.PathBsversion);
            this.Controls.Add(this.PathSubversion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RbBSWebService);
            this.Controls.Add(this.RbEspecifico);
            this.Controls.Add(this.RbSisVerificacao);
            this.Controls.Add(this.RbSisAtualizacao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnCompilar);
            this.Name = "Baiana20";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Baiana 2.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCompilar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton RbSisAtualizacao;
        private System.Windows.Forms.RadioButton RbSisVerificacao;
        private System.Windows.Forms.RadioButton RbEspecifico;
        private System.Windows.Forms.RadioButton RbBSWebService;
        private System.Windows.Forms.ProgressBar PbProgresso;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PathSubversion;
        private System.Windows.Forms.TextBox PathBsversion;
        private System.Windows.Forms.CheckBox CbCompilarTodos;
        private System.Windows.Forms.Button BtnCopiaDLLS;
        private System.Windows.Forms.Button BtnSubversion;
        private System.Windows.Forms.Button BtnBsversion;
        private System.Windows.Forms.Button BtnSalvarDiretorios;
        private System.Windows.Forms.Label LbMensagemAlerta;
        public System.Windows.Forms.CheckBox CbFecharTerminar;
        private System.Windows.Forms.Label label3;
        private TextBox boxLog;
        private Button btPararProcesso;
    }
}

