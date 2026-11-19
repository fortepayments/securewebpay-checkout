<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SampleVBNetCode.aspx.vb" Inherits="SampleVBNetCode.SampleVBNetCode" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SWP ASP.NET Web Forms sample</title>
    <link rel="stylesheet" type="text/css" href="css/site.css" />
    <script language="javascript" type="text/javascript">
        function val(id) {
            var el = document.getElementById(id);
            return el ? el.value : "";
        }

        function addField(form, name, value, always) {
            if (!always && (value == null || value === "")) {
                return;
            }
            var input = document.createElement("input");
            input.type = "hidden";
            input.name = name;
            input.value = value || "";
            form.appendChild(input);
        }

        function postSWP() {
            var flag = document.getElementById("hdnPostSWP");
            if (!flag || flag.value != "1") {
                return;
            }
            flag.value = "0";

            var form = document.createElement("form");
            form.method = "post";
            form.action = val("txtURL");

            addField(form, "pg_api_login_id", val("pg_api_login_id"), true);
            addField(form, "pg_template_id", "200", true);
            addField(form, "pg_billto_postal_name_company", val("pg_billto_postal_name_company"), true);
            addField(form, "pg_billto_postal_name_first", val("pg_billto_postal_name_first"), true);
            addField(form, "pg_billto_postal_name_last", val("pg_billto_postal_name_last"), true);
            addField(form, "pg_return_url", val("pg_return_url"), false);

            if (val("pg_ts_hash")) {
                addField(form, "pg_ts_hash", val("pg_ts_hash"), true);
                addField(form, "pg_transaction_type", val("pg_transaction_type"), true);
                addField(form, "pg_version_number", val("pg_version_number"), true);
                addField(form, "pg_utc_time", val("pg_utc_time"), true);
                addField(form, "pg_transaction_order_number", val("pg_transaction_order_number"), true);
                addField(form, "pg_total_amount", val("pg_total_amount"), true);
            }

            document.body.appendChild(form);
            form.submit();
        }
    </script>
</head>
<body onload="postSWP();">
    <form id="form1" runat="server">
        <div class="wrap">
            <h1>SWP hosted checkout sample</h1>
            <p class="lede">ASP.NET Web Forms (VB.NET). HMAC-MD5 is created on the server, then the browser posts to SWP.</p>

            <input type="hidden" name="hdnPostSWP" id="hdnPostSWP" runat="server" value="0" />
            <input type="hidden" name="pg_ts_hash" id="pg_ts_hash" runat="server" /> 
            <input type="hidden" name="pg_version_number" id="pg_version_number" runat="server" value="1.0" />
            <input type="hidden" name="pg_utc_time" id="pg_utc_time" runat="server" />

            <div class="row">
                <asp:Label ID="lblApiLoginId" runat="server" AssociatedControlID="pg_api_login_id" Text="API Login ID"></asp:Label>
                <asp:TextBox ID="pg_api_login_id" runat="server" CssClass="field" Text="YOUR_API_LOGIN_ID"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblSecTranKey" runat="server" AssociatedControlID="txtSecTranKey" Text="Secure Transaction Key"></asp:Label>
                <asp:TextBox ID="txtSecTranKey" runat="server" CssClass="field" TextMode="Password" autocomplete="off" Text="YOUR_SECURE_TRANSACTION_KEY"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblCheckoutUrl" runat="server" AssociatedControlID="txtURL" Text="Checkout URL"></asp:Label>
                <asp:TextBox ID="txtURL" runat="server" CssClass="field" Text="https://swp.paymentsgateway.net/co/default.aspx"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblReturnUrl" runat="server" AssociatedControlID="pg_return_url" Text="Return URL"></asp:Label>
                <asp:TextBox ID="pg_return_url" runat="server" CssClass="field"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblCompany" runat="server" AssociatedControlID="pg_billto_postal_name_company" Text="Company Name"></asp:Label>
                <asp:TextBox ID="pg_billto_postal_name_company" runat="server" CssClass="field"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblFirstName" runat="server" AssociatedControlID="pg_billto_postal_name_first" Text="First Name"></asp:Label>
                <asp:TextBox ID="pg_billto_postal_name_first" runat="server" CssClass="field"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="lblLastName" runat="server" AssociatedControlID="pg_billto_postal_name_last" Text="Last Name"></asp:Label>
                <asp:TextBox ID="pg_billto_postal_name_last" runat="server" CssClass="field"></asp:TextBox>
            </div>
            <div class="row">
                <span>Mode</span>
                <asp:RadioButtonList ID="rdnHTMLTranscation" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="radios" AutoPostBack="true">
                    <asp:ListItem Value="html" Selected="True" Text="HTML"></asp:ListItem>
                    <asp:ListItem Value="trans" Text="Secure Transaction"></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <asp:Panel ID="pnlTranscation" runat="server" Visible="false">
                <div class="row">
                    <span>Payment method</span>
                    <asp:RadioButtonList ID="rdnbtnCCeCheck" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="radios" AutoPostBack="true">
                        <asp:ListItem Text="Credit Card" Value="cc" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="eCheck" Value="ec"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="row">
                    <asp:Label ID="lblTransactionType" runat="server" AssociatedControlID="pg_transaction_type" Text="Transaction Type"></asp:Label>
                    <asp:DropDownList ID="pg_transaction_type" runat="server" CssClass="field"></asp:DropDownList>
                </div>
                <div class="row">
                    <asp:Label ID="lblOrderNumber" runat="server" AssociatedControlID="pg_transaction_order_number" Text="Order Number"></asp:Label>
                    <span>
                        <asp:TextBox ID="pg_transaction_order_number" runat="server" CssClass="field" Text="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqdTranscationOrderNum" runat="server" ControlToValidate="pg_transaction_order_number" ErrorMessage="*" CssClass="req" Display="Dynamic"></asp:RequiredFieldValidator>
                    </span>
                </div>
                <div class="row">
                    <asp:Label ID="lblTotalAmount" runat="server" AssociatedControlID="pg_total_amount" Text="Total Amount ($)"></asp:Label>
                    <span>
                        <asp:TextBox ID="pg_total_amount" runat="server" CssClass="field"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqdTotalAmount" runat="server" ControlToValidate="pg_total_amount" ErrorMessage="*" CssClass="req" Display="Dynamic"></asp:RequiredFieldValidator>
                    </span>
                </div>
            </asp:Panel>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="primary" />
            <p class="note">Replace the placeholder credentials on this form. The secret key is used only on the server and is never posted to SWP.</p>
        </div>
    </form>
</body>
</html>
