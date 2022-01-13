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
using AEHL = ICSharpCode.AvalonEdit.Highlighting;

namespace WpfApp24TextViewCore2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string loadFilePath;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextEditor_OnDrop(object sender, DragEventArgs e)
        {
            loadFilePath = "";
            
            // Get files that a user dropped
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files == null)
            {
                textView.Clear();
                return;
            }

            string filePath = files.First();

            try
            {
                textView.SyntaxHighlighting = GetSyntaxHilight(filePath);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                textView.Load(filePath);
                loadFilePath = filePath;
            }
            catch (NotSupportedException)
            {
                MessageBox.Show("This file is invalid.");
                return;
            }
        }

        private AEHL.IHighlightingDefinition GetSyntaxHilight(string filePath)
        {
            string ext = System.IO.Path.GetExtension(filePath).ToLower();

            switch (ext)
            {
                case ".cs":
                    return AEHL.HighlightingManager.Instance.GetDefinition("C#");
                case ".md":
                    return AEHL.HighlightingManager.Instance.GetDefinition("Markdown");
                case ".cpp":
                    return AEHL.HighlightingManager.Instance.GetDefinition("C++");
                case ".js":
                    return AEHL.HighlightingManager.Instance.GetDefinition("JavaScript");
                case ".json":
                    return AEHL.HighlightingManager.Instance.GetDefinition("Json");
                case ".htm":
                case ".html":
                    return AEHL.HighlightingManager.Instance.GetDefinition("HTML");
                case ".css":
                    return AEHL.HighlightingManager.Instance.GetDefinition("CSS");
                case ".xml":
                case ".xaml":
                case ".config":
                case ".csproj":
                    return AEHL.HighlightingManager.Instance.GetDefinition("XML");
                case ".ps1":
                case ".psm1":
                    return AEHL.HighlightingManager.Instance.GetDefinition("PowerShell");
                case ".java":
                    return AEHL.HighlightingManager.Instance.GetDefinition("Java");
                case ".sql":
                    return AEHL.HighlightingManager.Instance.GetDefinition("TSQL");
                case ".vb":
                    return AEHL.HighlightingManager.Instance.GetDefinition("VB");
                case ".py":
                    return AEHL.HighlightingManager.Instance.GetDefinition("Python");
                case ".php":
                    return AEHL.HighlightingManager.Instance.GetDefinition("PHP");
                case ".txt":
                case ".log":
                case ".bat":
                case ".ini":
                case ".csv":
                    return null;
                default:
                    throw new NotSupportedException();
            }
        }

        private void TextView_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            bool changeEncoding = false;
            string tag = (sender as MenuItem).Tag.ToString();
            if (tag == "sjis")
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (textView.Encoding != Encoding.GetEncoding("Shift-JIS"))
                {
                    textView.Encoding = Encoding.GetEncoding("Shift-JIS");
                    changeEncoding = true;
                }
            }
            else
            {
                if (textView.Encoding != Encoding.UTF8)
                {
                    textView.Encoding = Encoding.UTF8;
                    changeEncoding = true;
                }
            }

            if (changeEncoding && string.IsNullOrEmpty(loadFilePath) == false)
            {
                textView.Load(loadFilePath);
            }
        }
    }
}
