using System;
using System.Windows.Forms;
using RTCV.Common;
using RTCV.CorruptCore;
using RTCV.NetCore;
using static RTCV.UI.UI_Extensions;

namespace RTCV.UI
{
    public partial class RTC_GeneralParameters_Form : ComponentForm, IAutoColorize, IBlockable
    {
        public new void HandleMouseDown(object s, MouseEventArgs e) => base.HandleMouseDown(s, e);
        public new void HandleFormClosing(object s, FormClosingEventArgs e) => base.HandleFormClosing(s, e);

        public RTC_GeneralParameters_Form()
        {
            InitializeComponent();
            multiTB_Intensity.ValueChanged += (sender, args) => CorruptCore.RtcCore.Intensity = multiTB_Intensity.Value;
            multiTB_Intensity.registerSlave(S.GET<RTC_GlitchHarvesterIntensity_Form>().multiTB_Intensity);

            multiTB_ErrorDelay.ValueChanged += (sender, args) => CorruptCore.RtcCore.ErrorDelay = multiTB_ErrorDelay.Value;
        }

        private void RTC_GeneralParameters_Form_Load(object sender, EventArgs e)
        {
            cbBlastRadius.SelectedIndex = 0;
        }

        //Guid? errorDelayToken = null;
        //Guid? intensityToken = null;

        private void cbBlastRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbBlastRadius.SelectedItem.ToString())
            {
                case "SPREAD":
                    CorruptCore.RtcCore.Radius = BlastRadius.SPREAD;
                    break;

                case "CHUNK":
                    CorruptCore.RtcCore.Radius = BlastRadius.CHUNK;
                    break;

                case "BURST":
                    CorruptCore.RtcCore.Radius = BlastRadius.BURST;
                    break;

                case "NORMALIZED":
                    CorruptCore.RtcCore.Radius = BlastRadius.NORMALIZED;
                    break;

                case "PROPORTIONAL":
                    CorruptCore.RtcCore.Radius = BlastRadius.PROPORTIONAL;
                    break;

                case "EVEN":
                    CorruptCore.RtcCore.Radius = BlastRadius.EVEN;
                    break;
            }
        }

        private void RTC_GeneralParameters_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.FormOwnerClosing)
            {
                e.Cancel = true;
                this.RestoreToPreviousPanel();
                return;
            }
        }

        private void nmErrorDelay_ValueChanged(object sender, KeyPressEventArgs e)
        {
        }

        private void nmErrorDelay_ValueChanged(object sender, KeyEventArgs e)
        {
        }

        private void nmIntensity_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void nmIntensity_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void track_Intensity_MouseUp(object sender, KeyPressEventArgs e)
        {
        }

        private void track_Intensity_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void RTC_GeneralParameters_Form_Shown(object sender, EventArgs e)
        {
            object paramValue = AllSpec.VanguardSpec[VSPEC.OVERRIDE_DEFAULTMAXINTENSITY];

            if (paramValue != null && paramValue is int maxintensity)
            {
                var prevState = multiTB_Intensity.FirstLoadDone;
                multiTB_Intensity.FirstLoadDone = false;
                multiTB_Intensity.Maximum = maxintensity;
                multiTB_Intensity.FirstLoadDone = prevState;
            }
        }
    }
}
