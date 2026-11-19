using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace SWPCSharpIntegration
{
    public partial class SampleCSharpCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            if (!IsPostBack)
            {
                pnlTranscation.Visible = false;
                populateTranscationTypes();
                hdnPostSWP.Value = "0";
            }

            pg_utc_time.Value = DateTime.UtcNow.Ticks.ToString();
        }

        public string CalculateHMACMD5(string input, string strSecureTransKey)
        {
            StringBuilder sb = new StringBuilder();
            byte[] data = Encoding.Default.GetBytes(input);
            byte[] secretkey = Encoding.Default.GetBytes(strSecureTransKey);
            HMACMD5 hmacMD5 = new HMACMD5(secretkey);
            byte[] macSender = hmacMD5.ComputeHash(data);

            for (int i = 0; i < macSender.Length; i++)
            {
                sb.Append(macSender[i].ToString("x2"));
            }

            hmacMD5.Clear();
            return sb.ToString();
        }

        private void GenerateHashCode()
        {
            string versionNumber = pg_version_number.Value;
            string totalAmount = pg_total_amount.Text;
            string apiLoginId = pg_api_login_id.Text;
            string transactionType = pg_transaction_type.SelectedValue;
            string payload = apiLoginId + "|" + transactionType + "|" + versionNumber + "|" + totalAmount + "|" + pg_utc_time.Value + "|" + pg_transaction_order_number.Text;

            pg_ts_hash.Value = CalculateHMACMD5(payload, txtSecTranKey.Text);
            hdnPostSWP.Value = "1";
        }

        private void populateTranscationTypes()
        {
            if (rdnbtnCCeCheck.SelectedValue == "cc")
            {
                pg_transaction_type.Items.Clear();
                pg_transaction_type.Items.Insert(0, new ListItem("Sale", "10"));
                pg_transaction_type.Items.Insert(1, new ListItem("Auth", "11"));
                pg_transaction_type.Items.Insert(2, new ListItem("Credit", "13"));
            }
            if (rdnbtnCCeCheck.SelectedValue == "ec")
            {
                pg_transaction_type.Items.Clear();
                pg_transaction_type.Items.Insert(0, new ListItem("Sale", "20"));
                pg_transaction_type.Items.Insert(1, new ListItem("Auth", "21"));
                pg_transaction_type.Items.Insert(2, new ListItem("Credit", "23"));
            }
        }

        protected void rdnbtnCCeCheck_SelectedIndexChanged1(object sender, EventArgs e)
        {
            populateTranscationTypes();
        }

        protected void rdnHTMLTranscation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdnHTMLTranscation.SelectedValue == "html")
            {
                pnlTranscation.Visible = false;
            }
            else
            {
                pnlTranscation.Visible = true;
                populateTranscationTypes();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (pnlTranscation.Visible)
                GenerateHashCode();
            else
                hdnPostSWP.Value = "1";
        }
    }
}
