using Gihan.SpaceAnalizer.Logic;
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

namespace Gihan.SpaceAnalizer.Views.Customs
{
    /// <summary>
    /// Interaction logic for FileItem.xaml
    /// </summary>
    public partial class FileItem : UserControl
    {
        public FileSystemTreeNode Node { get; }

        public FileItem(FileSystemTreeNode node)
        {
            Node = node;
            InitializeComponent();
            TbPath.Text = node.Data.FullName;
            if (node.Size.HasValue)
                FileItem_SizeSetted(node, null);
            (Node.Parent as FileSystemTreeNode).SizeSetted += FileItem_SizeSetted;
            (Node.Parent as FileSystemTreeNode).SizeSetted += FileItem_SizeSetted;
        }

        private void FileItem_SizeSetted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                Prg.Maximum = (Node.Parent as FileSystemTreeNode).Size ?? 0;
                Prg.Value = Node.Size ?? 0;
                Prg.IsIndeterminate = false;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
