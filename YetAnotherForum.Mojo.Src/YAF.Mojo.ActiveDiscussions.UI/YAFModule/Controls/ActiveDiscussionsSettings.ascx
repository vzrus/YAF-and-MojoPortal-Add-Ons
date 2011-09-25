<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveDiscussionsSettings.ascx.cs"
    Inherits="YAF.Mojo.ActiveDiscussions.UI.Controls.ActiveDiscussionsSettings" %>
<%@ Register TagPrefix="mp" Namespace="mojoPortal.Web.Controls" Assembly="mojoPortal.Web.Controls" %>  
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="settingrow">        
            <asp:DropDownList ID="ModuleDropDownList" runat="server" CssClass="forminput" />
        </div>
        <asp:PlaceHolder ID="ModulePlaceHolder" runat="server">    
</asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>

