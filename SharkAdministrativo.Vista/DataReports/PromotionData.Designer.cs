namespace SharkAdministrativo.Vista.DataReports
{
    partial class PromotionData
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PromotionData));
            DevExpress.DataAccess.EntityFramework.EFConnectionParameters efConnectionParameters1 = new DevExpress.DataAccess.EntityFramework.EFConnectionParameters();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.efDataSource1 = new DevExpress.DataAccess.EntityFramework.EFDataSource(this.components);
            this.dateS = new DevExpress.XtraReports.Parameters.Parameter();
            this.dateE = new DevExpress.XtraReports.Parameters.Parameter();
            this.name = new DevExpress.XtraReports.Parameters.Parameter();
            this.precio = new DevExpress.XtraReports.Parameters.Parameter();
            this.precioM = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel4,
            this.xrLabel5,
            this.xrLabel6,
            this.xrPictureBox2,
            this.xrLabel7,
            this.xrLabel8,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel14,
            this.xrLabel13,
            this.xrLabel16,
            this.xrLabel15,
            this.xrLabel18,
            this.xrLabel17});
            this.Detail.Dpi = 100F;
            this.Detail.HeightF = 245.8333F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.nombre")});
            this.xrLabel4.Dpi = 100F;
            this.xrLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(51.04163F, 24.72919F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(301.0417F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.descripcion")});
            this.xrLabel5.Dpi = 100F;
            this.xrLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(149.9999F, 62.18751F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.diasDisponibles")});
            this.xrLabel6.Dpi = 100F;
            this.xrLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(149.9999F, 85.1875F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Image", null, "Promociones.imagen")});
            this.xrPictureBox2.Dpi = 100F;
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(372.9167F, 24.72919F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(228.1251F, 198.4583F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Dpi = 100F;
            this.xrLabel7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(51.04163F, 62.18751F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "Descripción:";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 100F;
            this.xrLabel8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(51.04163F, 85.18753F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Días:";
            // 
            // xrLabel10
            // 
            this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.hora_inicio")});
            this.xrLabel10.Dpi = 100F;
            this.xrLabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(149.9998F, 108.1875F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Dpi = 100F;
            this.xrLabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(51.04157F, 108.1876F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Hora / inicio:";
            // 
            // xrLabel12
            // 
            this.xrLabel12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.hora_fin")});
            this.xrLabel12.Dpi = 100F;
            this.xrLabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(149.9999F, 131.1875F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel12.StylePriority.UseFont = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Dpi = 100F;
            this.xrLabel11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(51.0417F, 131.1876F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "Hora / fin:";
            // 
            // xrLabel14
            // 
            this.xrLabel14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.fecha_inicio")});
            this.xrLabel14.Dpi = 100F;
            this.xrLabel14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(149.9998F, 154.1875F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel14.StylePriority.UseFont = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Dpi = 100F;
            this.xrLabel13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(51.04157F, 154.1876F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "Fecha / Inicio:";
            // 
            // xrLabel16
            // 
            this.xrLabel16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.fecha_fin")});
            this.xrLabel16.Dpi = 100F;
            this.xrLabel16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(149.9999F, 177.1875F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel16.StylePriority.UseFont = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Dpi = 100F;
            this.xrLabel15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(51.0417F, 177.1875F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.Text = "Fecha / fin:";
            // 
            // xrLabel18
            // 
            this.xrLabel18.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "Promociones.precioConImpuesto")});
            this.xrLabel18.Dpi = 100F;
            this.xrLabel18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(149.9999F, 200.1875F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(202.0835F, 23F);
            this.xrLabel18.StylePriority.UseFont = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Dpi = 100F;
            this.xrLabel17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(51.0417F, 200.1875F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(98.95834F, 23F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Precio:          $";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPictureBox1,
            this.xrLabel3});
            this.TopMargin.Dpi = 100F;
            this.TopMargin.HeightF = 173.6459F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 100F;
            this.xrLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75.31247F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "REPORTE DE PROMOCIONES";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Dpi = 100F;
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(262.5F, 31.56247F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(127.0833F, 43.75F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.xrLabel3.Dpi = 100F;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 157.9375F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 15.70833F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 100F;
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // efDataSource1
            // 
            efConnectionParameters1.ConnectionString = "";
            efConnectionParameters1.ConnectionStringName = "bdsharkEntities";
            efConnectionParameters1.Source = typeof(SharkAdministrativo.Modelo.bdsharkEntities);
            this.efDataSource1.ConnectionParameters = efConnectionParameters1;
            this.efDataSource1.Name = "efDataSource1";
            // 
            // dateS
            // 
            this.dateS.Description = "Fecha, desde:";
            this.dateS.Name = "dateS";
            this.dateS.Type = typeof(System.DateTime);
            // 
            // dateE
            // 
            this.dateE.Description = "Hasta";
            this.dateE.Name = "dateE";
            this.dateE.Type = typeof(System.DateTime);
            // 
            // name
            // 
            this.name.Description = "filtrar por nombre:";
            this.name.Name = "name";
            // 
            // precio
            // 
            this.precio.Description = "fitrar por precios, mayores de:";
            this.precio.Name = "precio";
            this.precio.Type = typeof(double);
            this.precio.ValueInfo = "0";
            // 
            // precioM
            // 
            this.precioM.Description = "filtrar por precio, menores de:";
            this.precioM.Name = "precioM";
            this.precioM.Type = typeof(double);
            this.precioM.ValueInfo = "0";
            // 
            // PromotionData
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.efDataSource1});
            this.FilterString = "[fecha_inicio] Between(?dateS, ?dateE) Or Contains([nombre], ?dateS) Or [precioCo" +
    "nImpuesto] >= ?precio Or [precioConImpuesto] <= ?precioM";
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 174, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.dateS,
            this.dateE,
            this.name,
            this.precio,
            this.precioM});
            this.Version = "16.1";
            ((System.ComponentModel.ISupportInitialize)(this.efDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel7;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRLabel xrLabel10;
        private DevExpress.XtraReports.UI.XRLabel xrLabel9;
        private DevExpress.XtraReports.UI.XRLabel xrLabel12;
        private DevExpress.XtraReports.UI.XRLabel xrLabel11;
        private DevExpress.XtraReports.UI.XRLabel xrLabel14;
        private DevExpress.XtraReports.UI.XRLabel xrLabel13;
        private DevExpress.XtraReports.UI.XRLabel xrLabel16;
        private DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRLabel xrLabel18;
        private DevExpress.XtraReports.UI.XRLabel xrLabel17;
        private DevExpress.DataAccess.EntityFramework.EFDataSource efDataSource1;
        private DevExpress.XtraReports.Parameters.Parameter dateS;
        private DevExpress.XtraReports.Parameters.Parameter dateE;
        private DevExpress.XtraReports.Parameters.Parameter name;
        private DevExpress.XtraReports.Parameters.Parameter precio;
        private DevExpress.XtraReports.Parameters.Parameter precioM;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
    }
}
