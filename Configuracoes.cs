using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Baiana20
{
    [Serializable]
    public class Configuracoes
    {
        public string DiretorioSubversion { get; set; }
        public string DiretorioBsversion { get; set; }
        public bool CompilarTodos { get; set; }
        public bool FecharTerminar { get; set; }
    }
}
