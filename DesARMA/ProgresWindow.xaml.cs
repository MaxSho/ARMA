using DesARMA.Automation;
using DesARMA.Models;
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

namespace DesARMA
{
    /// <summary>
    /// Interaction logic for ProgresWindow.xaml
    /// </summary>
    public partial class ProgresWindow : Window
    {
        public ProgresWindow()
        {
            InitializeComponent();
            Closing += ProgresWindow_Closing;
            Loaded += ProgresWindow_Loaded;
            Deactivated += ProgressWindow_Deactivated;
            Activated += ProgressWindow_Activated;

        }
        public void CreateEDR()
        {
            
        }

        private void ProgresWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Встановлення властивості Topmost на true для того, щоб вікно було поверх інших вікон
            Topmost = true;

            this.Show();
            //var dsd = new SearchEDR(item.Code, item.Name, null, 500, SearchType.Base, path + "\\auto");
            //dsd.CreatePDF();

        }

        private void ProgresWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //// Відміна закриття вікна
            //e.Cancel = true;
            //// Приховування вікна замість закривання
            //Hide();
        }
        private void ProgressWindow_Deactivated(object sender, EventArgs e)
        {
            //// Приховуємо вікно при деактивації
            //Hide();
        }

        private void ProgressWindow_Activated(object sender, EventArgs e)
        {
            //// Показуємо вікно та приводимо його на передній план при активації
            //ShowDialog();
            //Activate();
        }
    }
}
