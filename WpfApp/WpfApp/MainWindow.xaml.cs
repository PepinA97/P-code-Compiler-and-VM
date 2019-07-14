using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp
{

    public partial class MainWindow : Window
    {
        string inputText;
        string outputText;
        List<Instruction> instructions;
        
        public MainWindow()
        {
            outputText = "";
            inputText = File.ReadAllText("Code.txt");

            InitializeComponent();

            InputCodeBox.Text = inputText;
        }

        ~MainWindow()
        {
            File.WriteAllText("Code.txt", inputText);
        }

        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            Lexer lexer = new Lexer();

            TokenList tokenList = lexer.Run(inputText);
            
            if (lexer.HasError())
            {
                UpdateOutputBox(lexer.GetError());
                return;
            }
            
            Generator generator = new Generator();

            instructions = generator.Run(tokenList);

            foreach (Instruction instr in instructions)
                UpdateOutputBox(instr.ToString());

            if (generator.HasError())
            {
                UpdateOutputBox(generator.GetError());
                return;
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            Machine machine = new Machine();

            //List<int> machineOutput = machine:Run(machineInput);

            // take array of machine code, run and print output
        }

        private void UpdateOutputBox(string text)
        {
            outputText += text + "\n";

            OutputBox.Text = outputText;
        }

        private void InputCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputText = InputCodeBox.Text;
        }

        private void InputMachineBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
