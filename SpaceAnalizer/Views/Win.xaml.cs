using Gihan.SpaceAnalizer.Logic;
using Gihan.SpaceAnalizer.Views.Customs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Gihan.SpaceAnalizer.Views
{
    /// <summary>
    /// Interaction logic for Win.xaml
    /// </summary>
    public partial class Win
    {
        List<DriveItem> _driveItems;

        public Win()
        {
            InitializeComponent();
            BtnBack.Visibility = Visibility.Collapsed;
            LoadDrives();
        }

        private void LoadDrives()
        {
            var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed);
            _driveItems = new List<DriveItem>();
            foreach (var drive in drives)
            {
                DriveItem driveItem;
                Sp.Children.Add(driveItem = new DriveItem(drive.RootDirectory.FullName));
                _driveItems.Add(driveItem);
                //driveItem.Node.Refresh();
                driveItem.Click += DriveItem_Click;
            }
        }

        private void DriveItem_Click(object sender, RoutedEventArgs e)
        {
            (sender as DriveItem).Node.Refresh();
            TxtPath.Text = (sender as DriveItem).Node.Data.FullName;
            BtnGo_Click(sender, e);
        }

        private FileSystemTreeNode Find(FileSystemTreeNode node, string path)
        {
            if (path == node.Data.FullName) return node;
            var child = node.Children.LastOrDefault(n =>
                path.StartsWith(n.Data.FullName, StringComparison.OrdinalIgnoreCase));
            if (child is null) return null;
            if (string.Compare(child.Data.FullName, path, StringComparison.OrdinalIgnoreCase) == 0)
                return child as FileSystemTreeNode;
            return Find(child as FileSystemTreeNode, path);
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            Sp.Children.Clear();
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
            {
                BtnBack.Visibility = Visibility.Collapsed;
                LoadDrives();
                return;
            }
            else
                BtnBack.Visibility = Visibility.Visible;
            var tree = _driveItems
                .FirstOrDefault(d => TxtPath.Text.StartsWith(d.Node.Data.FullName)).Node;

            if (tree is null)
            {
                TxtPath.Focus();
                TxtPath.SelectAll();
                //to do: danger
                return;
            }
            var node = Find(tree, TxtPath.Text);
            foreach (var item in node.Children)
            {
                switch (item.Data)
                {
                    case DirectoryInfo folder:
                        FolderItem folderItem;
                        Sp.Children.Add(folderItem = new FolderItem(item as FileSystemTreeNode));
                        folderItem.Click += FolderItem_Click;
                        break;
                    case FileInfo file:
                        Sp.Children.Add(new FileItem(item as FileSystemTreeNode));
                        break;
                    default:
                        break;
                }
            }
        }

        private void FolderItem_Click(object sender, RoutedEventArgs e)
        {
            TxtPath.Text = (sender as FolderItem).Node.Data.FullName;
            BtnGo_Click(sender, e);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (TxtPath.Text.EndsWith(@":\"))
                TxtPath.Text = "";
            else
            {
                TxtPath.Text = TxtPath.Text.TrimEnd('/', '\\');
                TxtPath.Text = TxtPath.Text.Substring(0, TxtPath.Text.LastIndexOf('\\'));
                if (TxtPath.Text.EndsWith(":"))
                    TxtPath.Text += '\\';
            }
            BtnGo_Click(sender, e);
        }
    }
}

