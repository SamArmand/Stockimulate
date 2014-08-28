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
            
            if (BuyerID.Value == "" || SellerID.Value == "" || Quantity.Value == "" || Price.Value == "")
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> All fields are required.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;

            }

            if (Convert.ToInt32(BuyerID.Value) < 0 || Convert.ToInt32(SellerID.Value) < 0)
            {

                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> IDs cannot be negative.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            if (BuyerID.Value == SellerID.Value)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Buyer ID and Seller ID must be different.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            if (Convert.ToInt32(Quantity.Value) < 0)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Quantity cannot be negative.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            if (Convert.ToInt32(Price.Value) < 0)
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Price cannot be negative.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            if (!Dao.IDExists(BuyerID.Value))
            {

                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Invalid Buyer ID.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            if (!Dao.IDExists(SellerID.Value))
            {

                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Invalid Seller ID.";
                ErrorDiv.Style.Value = "display: inline";
                SuccessDiv.Style.Value = "display: none";
                return;
            }

            Dao.InsertTrade(Convert.ToInt32(BuyerID.Value), Convert.ToInt32(SellerID.Value), Security.Value, Convert.ToInt32(Quantity.Value), Convert.ToInt32(Price.Value));
            
            ErrorDiv.Style.Value = "display: none";
            SuccessDiv.Style.Value = "display: inline";



        }
    }
}