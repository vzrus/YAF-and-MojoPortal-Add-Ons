<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Forum.ascx.cs" Inherits="YAF.Mojo.UI.YAFModule.YafForum" %>
<%@ Register TagPrefix="YAF" Assembly="YAF" Namespace="YAF" %>
<%@ Register TagPrefix="portal" Namespace="mojoPortal.Web.UI" %>
<%@ Register TagPrefix="mp" Namespace="mojoPortal.Web.Controls" %>

<portal:modulepanel id="pnlContainer" runat="server">
    <portal:mojoPanel ID="mp1" runat="server" ArtisteerCssClass="art-Post" RenderArtisteerBlockContentDivs="true">
      <mp:CornerRounderTop ID="ctop1" runat="server" />
        <asp:Panel ID="pnlWrapper" runat="server" CssClass="art-Post-inner panelwrapper blogmodule">
            <portal:ModuleTitleControl ID="Title1" runat="server" />
            <portal:mojoPanel ID="MojoPanel1" runat="server" ArtisteerCssClass="art-PostContent">
                <div class="modulecontent">
                     <YAF:Forum ID="forum1" runat="server"  Visible="true"></YAF:Forum>
                </div>
            </portal:mojoPanel>
            <div class="cleared">
            </div>
        </asp:Panel>
        <mp:CornerRounderBottom ID="cbottom1" runat="server" />
    </portal:mojoPanel>
</portal:modulepanel>

