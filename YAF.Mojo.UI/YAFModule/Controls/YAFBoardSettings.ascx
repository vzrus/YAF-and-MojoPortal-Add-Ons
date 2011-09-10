<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YAFBoardSettings.ascx.cs"
    Inherits="YAF.Mojo.UI.YAFModule.Controls.YafBoardSettings" %>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="settingrow">
        <asp:Label CssClass="settinglabel" id="SelectBoardLbl" runat="server"/>    
            <asp:DropDownList ID="BoardDropDownList" runat="server" CssClass="forminput" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:PlaceHolder ID="BoardPlaceHolder" runat="server">
    <div class="settingrow">
    <asp:Label CssClass="settinglabel" id="NewBoardLbl" runat="server" />        
    </div>
    <div class="settingrow">  
         <asp:Label CssClass="settinglabel" id="SettingsLbl" runat="server"/>        
         <asp:TextBox ID="BoardName" runat="server" Style="width: 100%" CssClass="forminput" />
    </div>
    <div class="settingrow">
         <asp:Label CssClass="settinglabel" id="BoardCultureLbl" runat="server"></asp:Label>        
         <asp:DropDownList ID="Culture" runat="server" CssClass="forminput" />
    </div>
    <div class="settingrow">
          <asp:Label CssClass="settinglabel" id="AllowThreadedLabel" runat="server" />      
          <asp:CheckBox runat="server" ID="AllowThreaded" CssClass="forminput" />
    </div>
    <div class="settingrow">
        <asp:Button ID="SaveBtn" runat="server" />
    </div>
</asp:PlaceHolder>
