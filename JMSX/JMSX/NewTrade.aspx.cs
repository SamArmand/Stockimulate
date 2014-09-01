using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace JMSX
{
    public partial class _default : System.Web.UI.Page
    {

        private DAO Dao;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Dao = DAO.GetSessionInstance();
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

            Player Buyer = Dao.GetPlayer(Convert.ToInt32(BuyerID.Value));
            Player Seller = Dao.GetPlayer(Convert.ToInt32(SellerID.Value));

            if (Buyer.GetID() == -1)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer does not exist.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Seller.GetID() == -1)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Seller does not exist.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Buyer.GetTeamID() == Seller.GetTeamID())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer and Seller must be on different teams.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "SEC1" && (Buyer.GetPositionIndex1() + Convert.ToInt32(Quantity.Value)) > 100)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the buyer's SEC1 position at over 100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "SEC1" && (Seller.GetPositionIndex1() - Convert.ToInt32(Quantity.Value)) < -100)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the seller's SEC1 position at below -100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "SEC2" && (Buyer.GetPositionIndex2() + Convert.ToInt32(Quantity.Value)) > 100)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the buyer's SEC2 position at over 100.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                WarningDiv.Style.Value = "display: none";
                return;
            }

            if (Security.Value == "SEC2" && (Seller.GetPositionIndex2() - Convert.ToInt32(Quantity.Value)) < -100)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> This trade puts the seller's SEC2 position at below -100.";
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

            Dao.InsertTrade(Convert.ToInt32(BuyerID.Value), Convert.ToInt32(SellerID.Value), Security.Value, Convert.ToInt32(Quantity.Value), Convert.ToInt32(Price.Value));
            
            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: inline";
            WarningDiv.Style.Value = "display: none";



        }
    }
}