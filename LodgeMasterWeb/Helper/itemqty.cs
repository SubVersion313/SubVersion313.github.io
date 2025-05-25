namespace LodgeMasterWeb.Helper
{
    public class itemqty
    {

        public int _Qty;
        public string _ItemID;
        public int Qty
        {
            get
            {
                return _Qty;
            }
        }

        public string ItemID
        {
            get
            {
                return _ItemID;
            }
        }
        public itemqty(int NewQty, string NewItemID)
        {
            _Qty = NewQty;
            _ItemID = NewItemID;
        }

        public itemqty()
        {
        }

        public override string ToString()
        {
            return ItemID;
        }
    }
}
