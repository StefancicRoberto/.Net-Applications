using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlCenter.Applications.Convertor
{
    /// <summary>
    /// Interaction logic for Convertor.xaml
    /// </summary>
    public partial class Convertor : Window
    {
        public Convertor()
        {
            InitializeComponent();
            txtOutput.IsReadOnly = true;

            ControlCenter.MainWindow.isClosing += (result) => { if (result) this.Close(); };
        }

        private void cbFrom_Loaded(object sender, RoutedEventArgs e)
        {
            cbFrom.Items.Add("Decimal");
            cbFrom.Items.Add("Hexa");
            cbFrom.Items.Add("Binary");
        }

        private void cbTo_Loaded(object sender, RoutedEventArgs e)
        {
            cbTo.Items.Add("Decimal");
            cbTo.Items.Add("Hexa");
            cbTo.Items.Add("Binary");
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (txtInput.Text != String.Empty)
            {
                Conversion(cbFrom.SelectedIndex, cbTo.SelectedIndex);
            }
        }

        private void Conversion(int from, int to)
        {
            switch (from)
            {    
                case 0:
                    { //converting Decimal
                        long decimalVal=0;
                        if(Int64.TryParse(txtInput.Text, out decimalVal))
                        {
                            decimalVal=Int64.Parse(txtInput.Text);
                        switch (to)
                        { 
                            case 0: //Decimal to Decimal
                                txtOutput.Text = txtInput.Text;
                                break;
                            case 1: //Decimal to Hexa
                                txtOutput.Text = decimalVal.ToString("X");
                                break;
                            case 2: //Decimal to Binary
                                txtOutput.Text = Convert.ToString(decimalVal, 2);
                                break;
                            default: break;
                        }
                        }
                        else
                        { MessageBox.Show("Wrong input value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                            
                    }
                    break;
                case 1:
                    { //converting Hexa
                        switch (to)
                        {
                            case 0: //Hexa to Decimal
                                try
                                {
                                    txtOutput.Text = Convert.ToInt64(txtInput.Text, 16).ToString();
                                }
                                catch
                                { MessageBox.Show("Wrong input value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }

                                break;
                            case 1: //Hexa to Hexa
                                txtOutput.Text = txtInput.Text;
                                break;
                            case 2: //Hexa to Binary
                                try
                                {
                                    txtOutput.Text = Convert.ToString(Convert.ToInt64(txtInput.Text, 16), 2);
                                }
                                catch
                                { MessageBox.Show("Wrong input value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                                break;
                            default: break;
                        }
                    }
                    break;
                case 2:
                    { //converting Binary
                        switch (to)
                        {
                            case 0: //Binary to Decimal
                                try
                                {
                                    txtOutput.Text = Convert.ToInt64(txtInput.Text, 2).ToString();
                                }
                                catch
                                { MessageBox.Show("Wrong input value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                                break;
                            case 1: //Binary to Hexa
                                try
                                {
                                    txtOutput.Text = Convert.ToInt64(txtInput.Text, 2).ToString("X");
                                }
                                catch
                                { MessageBox.Show("Wrong input value!", "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
                                break;
                            case 2: //Binary to Binary
                                txtOutput.Text = txtInput.Text;
                                break;
                            default: break;
                        }
                    }
                    break;
                default: break;
            }
        }
    }
}
