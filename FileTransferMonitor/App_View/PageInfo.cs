namespace FileTransferMonitor.App_View
{
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            //get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
            get { return (TotalItems % PageSize == 0) ? (TotalItems / PageSize) : (TotalItems / PageSize + 1); }
        }
    }
}