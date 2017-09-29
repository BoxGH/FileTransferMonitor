using FileTransferMonitor.Models;
using System.Collections.Generic;

namespace FileTransferMonitor.App_View
{
    public class VMUploadFile
    {
        public PageInfo pageInfo { get; set; }
        public IEnumerable<FilesUploadNote> FilesUploadNote { get; set; }
    }
}