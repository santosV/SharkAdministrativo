using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SharkAdministrativo.Modelo;

namespace SharkAdministrativo.Vista
{

    /// <summary>
    /// Lógica de interacción para GestionDeProductos.xaml
    /// </summary>
    public partial class GestionDeProductos : Window
    {
        Insumo insumo = new Insumo();
        public GestionDeProductos()
        {
            InitializeComponent();
        }
        public void selectInsumo(Insumo insumo) {
            this.insumo = insumo;
        }
    }
}
