using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManagment.Bol.ViewModels.UserAreaViewModels.ViewModels
{
    public class FileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FileEncodingTypes { get; set; }
        public string FileStoreMode { get; set; }
        public string Path { get; set; }
        public Byte[] Data { get; set; }
    }
}
