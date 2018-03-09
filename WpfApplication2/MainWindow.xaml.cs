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
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        static string getString(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        static void setString(RichTextBox rtb, String value)
        {
            TextRange textRange = new TextRange(
                rtb.Document.ContentStart, rtb.Document.ContentEnd);
            textRange.Text = value;
        }

        // Eval > Evaluates C# sourcelanguage
        public object Eval(string code, Type outType = null, string[] includeNamespaces = null, string[] includeAssemblies = null)
        {
            StringBuilder namespaces = null;
            object methodResult = null;
            using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
            {
                ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
                CompilerParameters compileParams = new CompilerParameters();
                compileParams.CompilerOptions = "/t:library";
                compileParams.GenerateInMemory = true;
                if (includeAssemblies != null && includeAssemblies.Any())
                {
                    foreach (string _assembly in includeAssemblies)
                    {
                        compileParams.ReferencedAssemblies.Add(_assembly);
                    }
                }

                if (includeNamespaces != null && includeNamespaces.Any())
                {
                    foreach (string _namespace in includeNamespaces)
                    {
                        namespaces = new StringBuilder();
                        namespaces.Append(string.Format("using {0};\n", _namespace));
                    }
                }
                code = string.Format(
                    @"{1}  
                using System;  
                namespace CSharpCode{{  
                    public class Parser{{  
                        public {2} Eval(){{  
                            {3} {0};  
                        }}  
                    }}  
                }}",
                    code,
                    namespaces != null ? namespaces.ToString() : null,
                    outType != null ? outType.FullName : "void",
                    outType != null ? "return" : string.Empty
                    );
                CompilerResults compileResult = codeCompiler.CompileAssemblyFromSource(compileParams, code);

                if (compileResult.Errors.Count > 0)
                {

                    //throw new Exception(compileResult.Errors[0].ErrorText);
                    //RichTextBox inputBox = inputBox;
                    setString(inputBox, "Syntax Error");
                    return " ";
                }
                System.Reflection.Assembly assembly = compileResult.CompiledAssembly;
                object classInstance = assembly.CreateInstance("CSharpCode.Parser");
                Type type = classInstance.GetType();
                System.Reflection.MethodInfo methodInfo = type.GetMethod("Eval");
                methodResult = methodInfo.Invoke(classInstance, null);
            }
            
            return methodResult.ToString();
        }

        String results = "";
        bool enter_value = false;
       
        private void num_Click(object sender, RoutedEventArgs e)
        {
            Button num = (Button)sender;
            string numVal = (string)num.Content;
            if (enter_value )
            {
                if (numVal == "+" || numVal == "*" || numVal == "/" || numVal == "-")
                {
                    setString(inputBox, results);
                }
                else
                {
                    setString(inputBox, "");
                }
                
            }

            enter_value = false;
            string currentInput = getString(inputBox);
            
            inputBox.AppendText(numVal);
            
           
        }

       

       
        private void ce_Click(object sender, RoutedEventArgs e)
        {
            setString(inputBox, "");
            results = "";
            enter_value = true;
        }

        private void del_Click(object sender, RoutedEventArgs e)
        {
            string currentInput = getString(inputBox);
            int sI = (currentInput.Length - 3);
            currentInput  = currentInput.Remove(sI, 1);
            setString(inputBox, currentInput);
        }

        private void factorial_Click(object sender, RoutedEventArgs e)
        {
            try {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                int baseNo = Convert.ToInt32(results);
                long fact = baseNo;
                for (int i = baseNo - 1; i > 0; i--)
                {
                    fact = fact * i;
                }
                string show = currentInput + '!';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + fact);
            }
            catch
            {
                setString(inputBox, "Syntax Error");


            }
            enter_value = true;
        }

        private void tan_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            try {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Tan(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "tan(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            enter_value = true;
        }

        private void cos_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }
            try
            {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Cos(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "cos(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            enter_value = true;
        }

        private void sin_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }
            try
            {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Sin(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "sin(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            enter_value = true;
        }

        private void equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                inputBox.AppendText(Environment.NewLine + "" + results);
                enter_value = true;
            }
            catch
            {
                setString(inputBox, "Syntax Error");
                enter_value = true;
            }
        }

        private void sinh_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            try {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Sinh(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "sinh(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            enter_value = true;
        }

        private void cosh_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            try
            {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Cosh(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "cosh(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
                
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            enter_value = true;

        }

        private void tanh_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            string currentInput = getString(inputBox);
            results = (string)Eval(currentInput, typeof(double));
            results = Math.Tanh(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
            int sI = (currentInput.Length - 2);
            currentInput = currentInput.Remove(sI, 1);
            setString(inputBox, currentInput);
            string show = "tanh(" + currentInput + ')';
            show = show.Replace("\n", "");
            show = show.Replace("\t", "");
            show = show.Replace("\r", "");
            setString(inputBox, show);
            inputBox.AppendText("\n\n" + results);
            enter_value = true;
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            try
            {
                string currentInput = getString(inputBox);
                results = (string)Eval(currentInput, typeof(double));
                results = Math.Log10(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
                int sI = (currentInput.Length - 2);
                currentInput = currentInput.Remove(sI, 1);
                setString(inputBox, currentInput);
                string show = "log(" + currentInput + ')';
                show = show.Replace("\n", "");
                show = show.Replace("\t", "");
                show = show.Replace("\r", "");
                setString(inputBox, show);
                inputBox.AppendText("\n\n" + results);
                enter_value = true;
            }
            catch
            {
                setString(inputBox, "Syntax Error");
            }
            
        }

        private void ln_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, results);
            }

            string currentInput = getString(inputBox);
            results = (string)Eval(currentInput, typeof(double));
            results = Math.Log(Convert.ToDouble(results) * (Math.PI / 180)).ToString();
            int sI = (currentInput.Length - 2);
            currentInput = currentInput.Remove(sI, 1);
            setString(inputBox, currentInput);
            string show = "ln(" + currentInput + ')';
            show = show.Replace("\n", "");
            show = show.Replace("\t", "");
            show = show.Replace("\r", "");
            setString(inputBox, show);
            inputBox.AppendText("\n\n" + results);
            enter_value = true;
        }

        private void alog_Click(object sender, RoutedEventArgs e)
        {
            if (enter_value)
            {
                setString(inputBox, "");
            }
            //enter_value = false;
            inputBox.AppendText(Math.PI.ToString());

        }

        private void pow_Click(object sender, RoutedEventArgs e)
        {
            string currentInput = getString(inputBox);
            results = (string)Eval(currentInput, typeof(double));
            Button num = (Button)sender;
            string numVal = (string)num.Content;
            double baseNo = 0;
            if (enter_value)
            {
                setString(inputBox, results);
            }
            enter_value = false;

            inputBox.AppendText(numVal);

            if(numVal == "^2")
            {
                baseNo = Convert.ToDouble(results);
                baseNo = Math.Pow(baseNo, 2);
            }
            else
            {
                baseNo = Convert.ToDouble(results);
                baseNo = Math.Pow(baseNo, 3);
            }
            inputBox.AppendText("\n\n" + baseNo.ToString());
            results = baseNo.ToString();
            enter_value = true;
        }
    }
}
