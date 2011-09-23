<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ActiveDiscussionsSettings.ascx.cs"
    Inherits="YAF.Mojo.ActiveDiscussions.UI.Controls.ActiveDiscussionsSettings" %>  
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="settingrow">
        <asp:Label CssClass="settinglabel" id="SelectBoardLbl" runat="server"/>    
            <asp:DropDownList ID="BoardDropDownList" runat="server" CssClass="forminput" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:PlaceHolder ID="ModulePlaceHolder" runat="server">
    <div class="settingrow">
    <asp:Label CssClass="settinglabel" id="FeatureName" runat="server" />        
    </div>
    <div class="settingrow">  
         <asp:Label CssClass="settinglabel" id="SettingsLbl" runat="server"/>        
         <asp:TextBox ID="NumberToShow" runat="server" Style="width: 100%" CssClass="forminput" />
    </div>
        <div class="settingrow">  
         <asp:Label CssClass="settinglabel" id="Label1" runat="server"/>        
         <asp:TextBox ID="YAFFeatureGuide" runat="server" Style="width: 100%" CssClass="forminput" />
    </div>  
</asp:PlaceHolder>
