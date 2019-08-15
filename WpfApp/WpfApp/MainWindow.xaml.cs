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
        string inputCodeText;
        string input;
        string outputText;
        List<Instruction> instructions;
        
        public MainWindow()
        {
            inputCodeText = File.ReadAllText("Code.txt");
            input = "";
            outputText = "";

            InitializeComponent();

            InputCodeBox.Text = inputCodeText;
        }

        ~MainWindow()
        {
            File.WriteAllText("Code.txt", inputCodeText);
        }

        private void CompileButton_Click(object sender, RoutedEventArgs e)
        {
            Lexer lexer = new Lexer();

            TokenList tokenList = lexer.Run(inputCodeText);
            
            if (lexer.HasError())
            {
                UpdateOutputBox(lexer.GetError());
                return;
            }
            
            Generator generator = new Generator();

            instructions = generator.Run(tokenList);
            
            if (generator.HasError())
            {
                UpdateOutputBox(generator.GetError());
                return;
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            string[] tokens = input.Split(' ');
            List<int> machineInput = new List<int>();

            foreach(var str in tokens)
            {
                machineInput.Add(int.Parse(str));
            }
            
            Machine machine = new Machine();

            List<int> machineOutput = machine.Run(instructions, machineInput);

            string output = "";
            foreach(var i in machineOutput)
            {
                output += (i + " ");
            }

            UpdateOutputBox(output);
        }

        private void UpdateOutputBox(string text)
        {
            outputText += text + "\n";

            OutputBox.Text = outputText;
        }

        private void InputCodeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            inputCodeText = InputCodeBox.Text;
        }

        private void InputMachineBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            input = InputMachineBox.Text;
        }
    }
}
