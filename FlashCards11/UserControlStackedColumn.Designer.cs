using System.Windows.Controls;

namespace FlashCards11
{
    partial class UserControlStackedColumn
        {
    <UserControl x:Class="Wpf.CartesianChart.Basic_Stacked_Bar.BasicStackedColumnExample"
                 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                 xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
                 xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                 xmlns:lvc="clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf"
                 mc:Ignorable="d" 
                 d:DesignHeight="300" d:DesignWidth="300">
        <Grid>
            <lvc:CartesianChart Series = "{Binding SeriesCollection}" LegendLocation="Bottom">
                <lvc:CartesianChart.AxisX>
                    <lvc:Axis Title = "Browser"
                              Labels="{Binding Labels}" 
                              Separator="{x:Static lvc:DefaultAxes.CleanSeparator}" />
                </lvc:CartesianChart.AxisX>
                <lvc:CartesianChart.AxisY>
                    <lvc:Axis Title = "Usage" LabelFormatter="{Binding Formatter}"></lvc:Axis>
                </lvc:CartesianChart.AxisY>
            </lvc:CartesianChart>
        </Grid>
    </UserControl>

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        }

        #endregion
    }
}
