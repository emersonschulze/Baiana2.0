using System.Windows;
using System.Windows.Forms;

namespace Baiana20
{
    public class SelecionaPasta
    {
        internal string SelecionarPasta(string diretorio)
        {
            var dialog = new FolderBrowserDialog();
            
            dialog.ShowNewFolderButton = false;
            
            dialog.SelectedPath = diretorio;
            DialogResult resultado = dialog.ShowDialog();
            
            if (resultado == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }
            return diretorio;
        }
    }
}