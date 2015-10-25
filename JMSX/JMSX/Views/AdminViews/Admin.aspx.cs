using System;

namespace Stockimulate.Views.AdminViews
{
    public partial class Admin : System.Web.UI.Page
    {
        private Simulator _simulator;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            _simulator = Simulator.Instance;
        }

        protected void PlayPractice_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";
                        
            if (!VerifyInput.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();
            
            if (_simulator.IsPlaying() || _simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Another simulation is in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            if (_simulator.IsStopped())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Please reset the current simulation data.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            _simulator.SetPracticeMode();
            _simulator.Play();

        }

        protected void PlayCompetition_Click(object sender, EventArgs e)
        {
            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";

            if (!VerifyInput.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            if (_simulator.IsPlaying() || _simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Another simulation is in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            if (_simulator.IsStopped())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> Simulator is not READY to play another simulation. <br/> Please reset the current simulation data.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            _simulator.SetCompetitionMode();
            _simulator.Play();
        }

        protected void ResetTrades_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";
            
            if (!VerifyInput.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            _simulator.Reset();

        }

        protected void Continue_Click(object sender, EventArgs e)
        {

            ErrorDiv.Style.Value = "display: none;";
            WarningDiv.Style.Value = "display: none;";

            if (!VerifyInput.Checked)
            {
                WarningDiv.Style.Value = "display: inline;";
                return;
            }

            ClearForm();

            if (!_simulator.IsPaused())
            {
                ErrorDiv.InnerHtml = "<a href='#' class='close' data-dismiss='alert'>&times;</a><strong>Error!</strong> There is no PAUSED simulation in progress.";
                ErrorDiv.Style.Value = "display: inline;";
                return;
            }

            _simulator.Play();

        }

        protected void ClearForm()
        {
            
            VerifyInput.Checked = false;
            //Clear other form fields
        }

    }
}