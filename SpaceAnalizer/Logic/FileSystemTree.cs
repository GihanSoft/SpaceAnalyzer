using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gihan.SpaceAnalizer.Logic
{
    public class FileSystemTreeNode : TreeNode<FileSystemInfo>
    {
        public event EventHandler SizeSetted;

        public long? Size { get; private set; }

        private Task SetChildTask { get; set; }

        public FileSystemTreeNode()
        {
            SetChildTask = new Task(SetChildren);
        }
        private void SetChildren()
        {
            try
            {
                if (!(Data is DirectoryInfo folder))
                    throw new Exception("must be folder");

                var fileSystemInfos = folder.GetFileSystemInfos("*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => !f.Attributes.HasFlag(FileAttributes.System));
                _children.Clear();
                foreach (var fileSystemInfo in fileSystemInfos)
                    AddChild(new FileSystemTreeNode { Data = fileSystemInfo });

                foreach (var fileSystemInfoChild in Children)
                {
                    var child = fileSystemInfoChild as FileSystemTreeNode;
                    switch (child.Data)
                    {
                        case FileInfo file:
                            child.Size = file.Length;
                            break;
                        case DirectoryInfo _:
                            child.SetChildren();
                            break;
                        default:
                            break;
                    }
                }
                Size = Children.Sum(c => (c as FileSystemTreeNode).Size);
                SizeSetted?.Invoke(this, null);
            }
            catch(UnauthorizedAccessException err)
            {

            }
            catch (Exception err)
            {

            }
            SetChildTask = new Task(SetChildren);
        }

        public async void Refresh()
        {
            try
            {
                if (SetChildTask.Status == TaskStatus.Created)
                {
                    SetChildTask.Start();
                    await SetChildTask;
                }
            }
            catch (Exception err)
            {

            }
        }
    }
}
