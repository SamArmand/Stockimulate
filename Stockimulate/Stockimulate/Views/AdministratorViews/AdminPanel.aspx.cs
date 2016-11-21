using System;
using System.Web.UI.WebControls;
using Stockimulate.Architecture;

namespace Stockimulate.Views.AdministratorViews
{
    public partial class AdminPanel : System.Web.UI.Page
    {
        private Simulator _simulator;
        private DataAccess _dataAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            _simulator = Simulator.Instance;

            _dataAccess = DataAccess.SessionInstance;

            ReportsEnabledSpan.InnerHtml = "Reports Page Enabled: " + _dataAccess.IsReportsEnabled();

            var instruments = _dataAccess.Instruments;

            SecurityDropDownList.Items.Clear();

            foreach (var instrument in instruments)
            {
                SecurityDropDownList.Items.Add(new ListItem(instrument.Key, instrument.Key));
                var btn = new Button {UseSubmitBehavior = false, CausesValidation = false, OnClientClick = "return false;"};
                btn.Attributes.Add("href", "../Index.aspx?index='" + instrument.Key + "'");
                IndexButtonGroup.Controls.Add(btn);
            }

        }


        protected void PlayPractice_Click(object sender, EventArgs e)
        {
            Index.Reset();

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
            Index.Reset();

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

        protected void FixTrades_Click(object sender, EventArgs e)
		{

			ErrorDiv.Style.Value = "display: none;";
			WarningDiv.Style.Value = "display: none;";

			if (!VerifyInput.Checked)
			{
				WarningDiv.Style.Value = "display: inline;";
				return;
			}

			ClearForm();

			_simulator.SortaReset();

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

        protected void UpdatePrice_Click(object sender, EventArgs e)
        {
            if (PriceInput.Value != string.Empty && int.Parse(PriceInput.Value) >= 0)
            {
                var instrument = _dataAccess.GetAllInstruments()[SecurityDropDownList.SelectedValue];
                instrument.Price = int.Parse(PriceInput.Value);
                _dataAccess.Update(instrument);
            }

            Response.Redirect("AdminPanel.aspx");

        }

        protected void ToggleReportsEnabled_Click(object sender, EventArgs e)
        {
            _dataAccess.UpdateReportsEnabled(_dataAccess.IsReportsEnabled() ? "False" : "True");

            Response.Redirect("AdminPanel.aspx");
        }


    }
}