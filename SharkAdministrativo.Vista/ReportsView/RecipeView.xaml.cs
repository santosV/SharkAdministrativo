﻿using System;
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

namespace SharkAdministrativo.Vista.ReportsView
{
    /// <summary>
    /// Lógica de interacción para RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Window
    {
        public RecipeView()
        {
            InitializeComponent();
            loadReport();
        }

        /// <summary>
        /// Carga el reporte de la receta especificada.
        /// </summary>
        public void loadReport()
        {
            DataReports.RecipeData report = new DataReports.RecipeData();
            recipeViewer.DocumentSource = report;
            report.CreateDocument();
        }
    }
}
