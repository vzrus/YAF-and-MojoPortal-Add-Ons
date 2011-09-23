<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="ActiveDiscussion.ascx.cs" Inherits="YAF.Mojo.ActiveDiscussions.UI.ActiveDiscussion" %>
<%@ Register TagPrefix="YAF" Namespace="YAF.Controls" Assembly="YAF.Controls" %>
<%@ Register TagPrefix="mp" Namespace="mojoPortal.Web.Controls" Assembly="mojoPortal.Web.Controls" %>
<%@ Register TagPrefix="portal" Namespace="mojoPortal.Web.UI" Assembly="mojoPortal.Web" %>
<portal:OuterWrapperPanel ID="pnlOuterWrap" runat="server">
<mp:CornerRounderTop id="ctop1" runat="server" />
<portal:InnerWrapperPanel ID="pnlInnerWrap" runat="server" CssClass="panelwrapper contactform">
<portal:ModuleTitleControl id="Title1" runat="server"  />
<portal:OuterBodyPanel ID="pnlOuterBody" runat="server">
<portal:InnerBodyPanel ID="pnlInnerBody" runat="server" CssClass="modulecontent">
<asp:UpdatePanel ID="UpdateStatsPanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table border="0" class="content" cellspacing="1" cellpadding="0" width="100%">
             <asp:PlaceHolder runat="server" ID="ActiveDiscussionPlaceHolder">               
                 <tr id="noifo_Tr" runat="server" Visible="false" >
                    <td class="post" style="padding-left:10px">
                        <asp:Label ID="NoDataReceivedLbl" runat="server" />
                    </td>
                </tr>
                <asp:Repeater runat="server" ID="LatestPosts" OnItemDataBound="LatestPosts_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td class="post" style="padding-left:10px">
                                <asp:Image ID="NewPostIcon" runat="server" style="border: 0;width:16px;height:16px" />
                                &nbsp;<strong><asp:HyperLink ID="TextMessageLink" runat="server" /></strong>
                                &nbsp;<YAF:LocalizedLabel ID="ByLabel" runat="server" LocalizedTag="BY" />                                
                                &nbsp;<a id="LastUserLink" href="" runat="server" />&nbsp;(<asp:HyperLink ID="ForumLink" runat="server" />)
                            </td>
                            <td class="post" style="width: 30em; text-align: right;">      
                                
                                <YAF:DisplayDateTime ID="LastPostDate" runat="server" Format="BothTopic" />
                                <asp:HyperLink ID="ImageMessageLink" runat="server">
                                    <YAF:ThemeImage ID="LastPostedImage" runat="server" Style="border: 0" />
                                </asp:HyperLink>
                                <asp:HyperLink ID="ImageLastUnreadMessageLink" runat="server">
                                 <YAF:ThemeImage ID="LastUnreadImage" runat="server"  Style="border: 0" />
                                </asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:PlaceHolder>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
 </portal:InnerBodyPanel>
</portal:OuterBodyPanel>
</portal:InnerWrapperPanel>
<mp:CornerRounderBottom id="cbottom1" runat="server" />
</portal:OuterWrapperPanel>        
