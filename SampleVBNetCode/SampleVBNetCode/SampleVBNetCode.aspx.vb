Imports System.Text
Imports System.Security.Cryptography

Partial Public Class SampleVBNetCode
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetExpires(DateTime.Now)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Not IsPostBack Then
            pnlTranscation.Visible = False
            populateTranscationTypes()
            hdnPostSWP.Value = "0"
        End If

        pg_utc_time.Value = DateTime.UtcNow.Ticks.ToString()
    End Sub

    Private Sub rdnHTMLTranscation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdnHTMLTranscation.SelectedIndexChanged
        If rdnHTMLTranscation.SelectedValue = "html" Then
            pnlTranscation.Visible = False
        Else
            pnlTranscation.Visible = True
            populateTranscationTypes()
        End If
    End Sub

    Private Sub GenerateHashCode()
        Dim versionNumber As String = pg_version_number.Value
        Dim totalAmount As String = pg_total_amount.Text
        Dim apiLoginId As String = pg_api_login_id.Text
        Dim transactionType As String = pg_transaction_type.SelectedValue
        Dim payload As String = apiLoginId & "|" & transactionType & "|" & versionNumber & "|" & totalAmount & "|" & pg_utc_time.Value & "|" & pg_transaction_order_number.Text

        pg_ts_hash.Value = CalculateHMACMD5(payload, txtSecTranKey.Text)
        hdnPostSWP.Value = "1"
    End Sub

    Public Function CalculateHMACMD5(ByVal input As String, ByVal strSecureTransKey As String) As String
        Dim sb As New StringBuilder()
        Dim data As Byte() = Encoding.Default.GetBytes(input)
        Dim secretkey As Byte() = Encoding.Default.GetBytes(strSecureTransKey)
        Dim hmacMD5 As New HMACMD5(secretkey)
        Dim macSender As Byte() = hmacMD5.ComputeHash(data)

        For i As Integer = 0 To macSender.Length - 1
            sb.Append(macSender(i).ToString("x2"))
        Next

        hmacMD5.Clear()
        Return sb.ToString()
    End Function

    Private Sub rdnbtnCCeCheck_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdnbtnCCeCheck.SelectedIndexChanged
        populateTranscationTypes()
    End Sub

    Private Sub populateTranscationTypes()
        If rdnbtnCCeCheck.SelectedValue = "cc" Then
            pg_transaction_type.Items.Clear()
            pg_transaction_type.Items.Insert(0, New ListItem("Sale", "10"))
            pg_transaction_type.Items.Insert(1, New ListItem("Auth", "11"))
            pg_transaction_type.Items.Insert(2, New ListItem("Credit", "13"))
        End If
        If rdnbtnCCeCheck.SelectedValue = "ec" Then
            pg_transaction_type.Items.Clear()
            pg_transaction_type.Items.Insert(0, New ListItem("Sale", "20"))
            pg_transaction_type.Items.Insert(1, New ListItem("Auth", "21"))
            pg_transaction_type.Items.Insert(2, New ListItem("Credit", "23"))
        End If
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If pnlTranscation.Visible Then
            GenerateHashCode()
        Else
            hdnPostSWP.Value = "1"
        End If
    End Sub
End Class
