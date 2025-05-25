namespace LodgeMasterWeb.Core.Models
{
    public class ItemService
    {
        public int ID { get; set; }
        [MaxLength(250)]
        public string ItemID { get; set; } = string.Empty;
        [MaxLength(250)]
        public string ItemIDSub { get; set; } = string.Empty;
        [MaxLength(250)]
        public string CompanyID { get; set; } = string.Empty;
        public int Qty { get; set; } = 1;
        public int iSorted { get; set; } = 0;

        public int IsDeleted { get; set; } = 0;

    }
}
