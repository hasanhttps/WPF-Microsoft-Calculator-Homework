using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

namespace WPF_Calculator_Homework {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        Brush? brush = null;
        string? firstoperand = null, secondoperand = null;
        string? _operator = null;
        bool isOperation = false;

        private void Border_GotFocus(object sender, RoutedEventArgs e) {
            Border? border = sender as Border;
            if (brush == null) brush = border!.Background;
            border!.Background = new SolidColorBrush() {
                Color = Color.FromRgb((byte)249, (byte)244, (byte)248)
            };
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e) {
            Border? border = sender as Border;
            border!.Background = brush;
            brush = null;
        }

        private void Border_MouseUp(object sender, MouseEventArgs e) {
            Border? border = sender as Border;
            border!.Background = brush;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            Border? border = sender as Border;
            border!.Background = new SolidColorBrush() {
                Color = Color.FromRgb((byte)250, (byte)242, (byte)245)
            };
            string? command = (border.Child as TextBlock)!.Text;
            if (command != null) {
                if (command.All(c => Char.IsDigit(c))) {
                    
                    if (number.Content.ToString() == "0" || isOperation && firstoperand == number.Content.ToString()) 
                        number.Content = command;
                    else 
                        number.Content += command;

                    if (isOperation == false) 
                        firstoperand = number.Content.ToString();
                    else 
                        secondoperand = number.Content.ToString();
                }
                else {
                    if (isOperation) {
                        if (command == "+" || command == "-" || command == "x" || command == "÷") {
                            secondoperand = number.Content.ToString();
                            string? op = (operation.Content.ToString()[operation.Content.ToString().Length - 2]).ToString();
                            if (op == "x") op = "*";
                            else if (op == "÷") op = "/";
                            double result = EvaluateExpression(firstoperand + op + secondoperand);
                            number.Content = firstoperand = result.ToString();
                            secondoperand = null;
                        }
                    }
                    _operator = command;
                    operate();
                } 
            }
        }

        private void operate() {
            if (_operator == "+" || _operator == "-" || _operator == "x" || _operator == "÷") {
                isOperation = true;
                operation.Content = firstoperand + " " + _operator + " ";
            }
            else if (_operator == ",") {
                if (!number.Content.ToString().Contains(",")) number.Content += ",";
            }
            else if (_operator == "C") {
                operation.Content = "";
                number.Content = "0";
                firstoperand = secondoperand = _operator = null;
                isOperation = false;
            }
            else if (_operator == "CE") { 
                if (isOperation) {
                    number.Content = "0";
                }
            }
            else if (_operator == "⌫") {
                if (number.Content.ToString().Length > 1) {
                    number.Content = number.Content.ToString().Substring(0, number.Content.ToString().Length - 1);
                }else number.Content = 0;
            }
            else if (_operator == "√x") {
                double result = Math.Sqrt(double.Parse(firstoperand));
                operation.Content = $"√({firstoperand})";
                number.Content = firstoperand = result.ToString();
            }
            else if (_operator == "x²") {
                double result = double.Parse(firstoperand) * double.Parse(firstoperand);
                operation.Content = $"sqr({firstoperand})";
                number.Content = firstoperand = result.ToString();
            }
        }

        private void EqualBorder_MouseDown(object sender, MouseButtonEventArgs e) {
            Border? border = sender as Border;
            border!.Background = new SolidColorBrush() {
                Color = Color.FromRgb((byte)204, (byte)71, (byte)65)
            };

            if (isOperation) {
                secondoperand = number.Content.ToString();  
                string? op = (operation.Content.ToString()[operation.Content.ToString().Length - 2]).ToString();
                if (op == "x") op = "*";
                else if (op == "÷") op = "/";
                double result = EvaluateExpression(firstoperand + op + secondoperand);
                operation.Content += secondoperand + " =";

                number.Content = firstoperand  = result.ToString();
                isOperation = false;
            }else operation.Content = number.Content + " =";
        }

        public double EvaluateExpression(string expression) {
            double result = double.Parse(number.Content.ToString());
            DataTable table = new DataTable();
            expression = expression.Replace(',', '.');
            table.Columns.Add("expression", typeof(string), expression);
            DataRow row = table.NewRow();
            table.Rows.Add(row);
            result = double.Parse((string)row["expression"]);
            return result;
        }
    }
}