Imports System.Security.Cryptography

Partial Public Class GenerateHash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim strKey As String = txtHashString.Text.Substring(txtHashString.Text.LastIndexOf(",") + 1)
        Dim strString As String = txtHashString.Text.Substring(0, txtHashString.Text.LastIndexOf(","))
        If strString.Length > 1 Then
            txtResult.Text = CalculateHMACMD5(strString, strKey)
        Else
            txtResult.Text = "Error check input string"
        End If

    End Sub

    Public Function CalculateHMACMD5(ByVal input As String, ByVal strSecureTransKey As String) As String
        Dim sb As New StringBuilder()
        Dim data As Byte() = Encoding.[Default].GetBytes(input)
        Dim secretkey As Byte() = New Byte(63) {}
        secretkey = Encoding.[Default].GetBytes(strSecureTransKey)
        Dim hmacMD5 As New HMACMD5(secretkey)
        Dim macSender As Byte() = hmacMD5.ComputeHash(data)

        For i As Integer = 0 To macSender.Length - 1
            sb.Append(macSender(i).ToString("x2"))
        Next

        hmacMD5.Clear()
        Return sb.ToString()

    End Function

End Class