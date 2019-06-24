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

/*
 * Have default text from text file
*/

namespace WpfApp
{

    public partial class MainWindow : Window
    {
        string outputText;

        // variables
        // array of machine code (to pass into run)
        

        // constructor
        public MainWindow()
        {
            outputText = "";

            InitializeComponent();
        }
        
        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            // initialize lexer, pass InputBox.Text
            
            outputText += InputBox.Text;

            OutputBox.Text = outputText;
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
