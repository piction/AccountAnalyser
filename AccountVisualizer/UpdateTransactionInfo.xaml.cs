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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Globalization;

namespace AccountVisualizer
{

    public class TagViewModel
    {
        public string Name { get; set; } = "";
        public double Amount { get; set; } = 0;
        public TagViewModel(string name, double amount = 0)
        {
            this.Name = name;
            this.Amount = amount;
        }
    }
    /// <summary>
    /// Interaction logic for UpdateTransactionInfo.xaml
    /// </summary>
    public partial class UpdateTransactionInfo : Window
    {
        private Dictionary<string, TextBox> _textBoxes = new Dictionary<string, TextBox>();
        private List<TagViewModel> _tagsViewModelList = new List<TagViewModel>();
        private TransactionInfoViewModel _trans;
        public bool saveDone = false;
        public UpdateTransactionInfo(TransactionInfoViewModel trans, HashSet<string> availibleTags, bool isLooping=false)
        {
            _trans = trans;
            InitializeComponent();
            this.SaveBttn.Content = isLooping ? "Next" : "Save";
            this.titleLabel.Content = $"{trans.Amount} euro=> {trans.AccountName} @ {trans.Date.ToShortDateString()}";
            this.descriptionLabel.Content = String.IsNullOrWhiteSpace(trans.Message) ? "(No description)": $"({trans.Message})";
            this.SurplasDescTextBlock.Text = trans.SurplasInfo;
      

            this.InvoiceDate_dd.Text = trans?.InvoiceDate?.Day.ToString() ?? "";
            this.InvoiceDate_mm.Text = trans?.InvoiceDate?.Month.ToString() ?? "";
            this.InvoiceDate_yy.Text = trans?.InvoiceDate?.Year.ToString() ?? trans.Date.Year.ToString() ;
            this.IsProcessedCheckbox.IsChecked = trans.IsProcessed;

            this.FileNameTextBox.Text = String.IsNullOrWhiteSpace(trans.LinkedFile) ? "" : trans.LinkedFile;

           
            foreach ( var t in availibleTags)
            {
               double Amount = trans.info.tags.ContainsKey(t) ? trans.info.tags[t] : 0;
                _tagsViewModelList.Add(new TagViewModel(t, Amount));
            }
            int half = (int)(_tagsViewModelList.Count / 2);
            ListBoxTags1.ItemsSource = _tagsViewModelList.Take(half);
            ListBoxTags2.ItemsSource = _tagsViewModelList.Skip(half);
        }

        private void BrowseFileNameBttn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if ( openFileDialog.ShowDialog().Value)
            {
               this.FileNameTextBox.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
            }
        }

        private void SaveBttn_Click(object sender, RoutedEventArgs e)
        {
            // validation 
            CultureInfo eng = new CultureInfo("en-En"); // use this to have the dot as decimal seperator
            Dictionary<string, double> receivedTags = new Dictionary<string, double>();
            double sum = 0;
            foreach (var b in _tagsViewModelList)
            {
                if (b.Amount != 0)
                {
                    sum += b.Amount;
                    receivedTags.Add(b.Name, b.Amount);
                }
            }
            if (Math.Abs(sum - _trans.Amount) > 1e-4)
            {
                MessageBox.Show("The sum of the categories are not correct!", "caption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                _trans.info.tags = new Dictionary<string, double>(receivedTags);
            }
            _trans.info.linkedFile = this.FileNameTextBox.Text;
            _trans.info.surplasDescription = this.SurplasDescTextBlock.Text.Replace(',', '&');

            try
            {
                if (string.IsNullOrEmpty(this.InvoiceDate_dd.Text) ||
                    string.IsNullOrEmpty(this.InvoiceDate_mm.Text) ||
                    string.IsNullOrEmpty(this.InvoiceDate_yy.Text))
                {
                    _trans.info.invoiceDate = null;
                }
                else
                {
                    _trans.info.invoiceDate = new DateTime(Convert.ToInt32(this.InvoiceDate_yy.Text), Convert.ToInt32(this.InvoiceDate_mm.Text), Convert.ToInt32(this.InvoiceDate_dd.Text));
                }
            }
            catch
            {
                MessageBox.Show("De factuur datum is niet correct of in verkeerde formaat", "caption", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            _trans.info.IsProcessed = this.IsProcessedCheckbox.IsChecked.Value ;
            saveDone = true;
            this.Close();
        }

        private void OpenDocBtn_Click(object sender, RoutedEventArgs e)
        {
            string f = this.FileNameTextBox.Text;
            if (string.IsNullOrWhiteSpace(f))
                return;
            foreach (var folder in Properties.Settings.Default.SourceFolders)
            {
                foreach ( var file in System.IO.Directory.GetFiles(folder))
                {
                    if (System.IO.Path.GetFileName(file).Equals(f))
                    {
                        System.Diagnostics.Process.Start(file);
                        return;
                    }
                }
            }
            MessageBox.Show("File not found");
        }

        private void TextBoxAmountChanged (object sender,TextCompositionEventArgs e)
        {
            if (e.Text.Equals("-"))
                return;
            if (e.Text.Last().Equals('.'))
                return;
            decimal t;
            if (!Decimal.TryParse(e.Text, out t))
                e.Handled = true;
        }

        private void click_applyFullAmount(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if ( btn?.DataContext !=null)
            {
                var tagviewModel = btn.DataContext as TagViewModel;
                if ( tagviewModel !=null)
                {
                    foreach ( var v in _tagsViewModelList)
                    {
                        v.Amount = 0;
                    }
                    tagviewModel.Amount = _trans.Amount;


                    int half = (int)(_tagsViewModelList.Count / 2);
                    ListBoxTags1.ItemsSource = null;
                    ListBoxTags1.ItemsSource = _tagsViewModelList.Take(half);
                    ListBoxTags2.ItemsSource = null;
                    ListBoxTags2.ItemsSource = _tagsViewModelList.Skip(half);
                }
            }
        }
    }
}
