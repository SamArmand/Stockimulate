using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//       ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> No such Team ID/Code combination exists.";

namespace JMSX
{
    public partial class Admin : System.Web.UI.Page
    {

        private DAO dao;
        private Simulator simulator;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            dao = DAO.SessionInstance;
            simulator = Simulator.Instance;
        }

        protected void PlayPractice_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";
                        
            if (!Verify.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();
            
            if (simulator.IsPlaying() || simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Another simulation is in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            if (simulator.IsStopped())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Please reset the current simulation data.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            simulator.SetPracticeMode();
            simulator.Play();

        }

        protected void PlayCompetition_Click(object sender, EventArgs e)
        {
            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";

            if (!Verify.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            if (simulator.IsPlaying() || simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Another simulation is in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            if (simulator.IsStopped())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Please reset the current simulation data.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            simulator.SetCompetitionMode();
            simulator.Play();
        }

        protected void ResetTrades_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";
            
            if (!Verify.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            simulator.Reset();

        }

        protected void Continue_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";

            if (!Verify.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            if (!simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> There is no PAUSED simulation in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            simulator.Play();

        }

        protected void ClearForm()
        {
            
            Verify.Checked = false;
            //Clear other form fields
        }

    }
}