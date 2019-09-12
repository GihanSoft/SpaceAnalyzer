using Gihan.SpaceAnalizer.Logic;
using System;
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

namespace Gihan.SpaceAnalizer.Views.Customs
{
    /// <summary>
    /// Interaction logic for DriveItem.xaml
    /// </summary>
    public partial class DriveItem : UserControl
    {
        public event RoutedEventHandler Click;

        public StorageTreeNode Node { get; }

        public DriveItem(string driveName)
        {
            InitializeComponent();
            TbPath.Text = driveName;
            var drive = new DriveInfo(driveName);
            Prg.Maximum = drive.TotalSize;
            Prg.Value = drive.TotalSize - drive.AvailableFreeSpace;
            Prg.IsIndeterminate = false;
            Node = new StorageTreeNode { Data = drive.RootDirectory };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, e);
        }
    }
}
