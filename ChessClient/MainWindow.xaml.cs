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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using System.IO;


namespace ChessClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Table table;
        public int NumberClient;
        private ServerConnection serverConnection;
        public MainWindow()
        {
            InitializeComponent();
            table = new Table(ref GridTable, ref UniformGridTable);
            serverConnection = new ServerConnection(IPserver.Text, table);

        }

        private void BtSend_Click(object sender, RoutedEventArgs e)
        {
            if (!table.MoveFigure(StepBegin.Text, StepEnd.Text))
            {
                MessageBox.Show("Incorrect move!");
            }
            else
            {
                ListSteps.Items.Add((ListSteps.Items.Count + 1).ToString() + ". " 
                    + table.GetFigure(StepEnd.Text) + "  " 
                    + StepBegin.Text + " - " + StepEnd.Text);
                StepBegin.Text = "";
                StepEnd.Text = "";
               // BtSend.IsEnabled = false;
                var mes = NumberClient.ToString() + "-" + StepBegin.Text + "-" + StepEnd.Text;
                serverConnection.SendMessage(mes);
                
            }
        }
    }
}
