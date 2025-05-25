namespace LodgeMasterWeb.Core.ViewModels
{
    public class ItemsServiceVM
    {

        // [ID]
        //,[ItemID]
        //,[ItemIDSub]
        //,[CompanyID]
        //,[Qty]
        //,[iSorted]

        public string? ItemID { get; set; } = string.Empty;
        public string ItemIDSub { get; set; } = string.Empty;
        public string? CompanyID { get; set; } = string.Empty;
        public int subitemQty { get; set; } = 1;
        public int iSorted { get; set; } = 0;
        public int IsDeleted { get; set; } = 0;


    }
}
