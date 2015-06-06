using System;

namespace Stockimulate.Views.BrokerViews
{
    public partial class NewTrade : System.Web.UI.Page
    {

        private DataAccess _dataAccess;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _dataAccess = DataAccess.SessionInstance;
        }

        protected void Submit_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: none";
            WarningDiv.Style.Value = "display: none";
            
            if (BuyerID.Value == "" || SellerID.Value == "" || Quantity.Value == "" || Price.Value == "")
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> All fields are required.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;

            }

            if (Convert.ToInt32(BuyerID.Value) < 0 || Convert.ToInt32(SellerID.Value) < 0)
            {

                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> IDs cannot be negative.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (BuyerID.Value == SellerID.Value)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer ID and Seller ID must be different.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Convert.ToInt32(Quantity.Value) <= 0)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Quantity must be at least 1.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Convert.ToInt32(Price.Value) <= 0)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Price must be at least 1.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            Player buyer = _dataAccess.GetPlayer(Convert.ToInt32(BuyerID.Value));
            Player seller = _dataAccess.GetPlayer(Convert.ToInt32(SellerID.Value));

            if (buyer.Id == -1)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer does not exist.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (seller.Id == -1)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Seller does not exist.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (buyer.TeamId == seller.TeamId)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer and Seller must be on different teams.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "OIL" && ((buyer.PositionIndex1 + Convert.ToInt32(Quantity.Value)) > 100 && buyer.Id != 0))
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the buyer's OIL position at over 100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "OIL" && ((seller.PositionIndex1 - Convert.ToInt32(Quantity.Value)) < -100 && seller.TeamId != 0))
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the seller's OIL position at below -100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "IND" && ((buyer.PositionIndex2 + Convert.ToInt32(Quantity.Value)) > 100 && buyer.TeamId != 0))
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the buyer's IND position at over 100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "IND" && ((seller.PositionIndex2 - Convert.ToInt32(Quantity.Value)) < -100 && seller.Id != 0))
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the seller's IND position at below -100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (!Verify.Checked)
            {
                ErrorDiv.Style.Value = "display: none";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: inline";
                return;
            }

            _dataAccess.InsertTrade(Convert.ToInt32(BuyerID.Value), Convert.ToInt32(SellerID.Value), Security.Value, Convert.ToInt32(Quantity.Value), Convert.ToInt32(Price.Value));
            
            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: inline";
            WarningDiv.Style.Value = "display: none";

            ClearForm();

        }

        protected void ClearForm()
        {
            BuyerID.Value = string.Empty;
            SellerID.Value = string.Empty;
            Quantity.Value = string.Empty;
            Price.Value = string.Empty;

            Verify.Checked = false;
            //Clear other form fields
        }
    }
}