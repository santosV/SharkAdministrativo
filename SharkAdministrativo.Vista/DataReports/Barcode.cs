using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SharkAdministrativo.Vista.DataReports
{
    public partial class Barcode : DevExpress.XtraReports.UI.XtraReport
    {
        public Barcode()
        {
            InitializeComponent();
        
        }

        public void loadParameters(int id) {
            P_ID.Value = id;
            P_ID.Visible = false;
            

        }

    }
}
