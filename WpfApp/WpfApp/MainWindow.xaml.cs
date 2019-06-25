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
            
            Lexer lexer = new Lexer();

            TokenList tokenList = lexer.Run(InputBox.Text);
            
            if (lexer.HasError())
            {
                UpdateOutputBox(Constants.LexerErrors[(int)lexer.error]);
                return;
            }

            UpdateOutputBox(tokenList.ToString());

            // initialize generator
            // check if error code

            /*
            Generator generator = new Generator(tokenList);
            if (generator.HasError())
            {
                // print error
                return; // terminate
            }*/

        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            // take array of machine code, run and print output
        }

        private void UpdateOutputBox(string text)
        {
            outputText += text + "\n";

            OutputBox.Text = outputText;
        }
    }
}
