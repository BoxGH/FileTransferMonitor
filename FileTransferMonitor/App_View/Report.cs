using System;

namespace FileTransferMonitor.App_View
{
    public class Report
    {
        public string FileName { get; set; }
        public decimal FileFileSize { get; set; }
        public string FileType { get; set; }
        public DateTime DataUploaded { get; set; }
        public string WhoUploaded { get; set; }
        public string TypeOperations { get; set; }
        public DateTime? DataOperations { get; set; }
        public string WhoOperations { get; set; }
    }
}