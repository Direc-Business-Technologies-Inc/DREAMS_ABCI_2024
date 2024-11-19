<%@ Page Title="DREAMS | Document Requirements" Language="C#" MasterPageFile="~/master/Main.Master" AutoEventWireup="true" CodeBehind="Documents.aspx.cs" Inherits="ABROWN_DREAMS.Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .elipsis {
            width: 120px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
    </style>
    <%--MODALS--%>
    <script type="text/javascript">
        function showBuyersList() {
            $('#modalBuyers').modal('show');
        }
        function hideBuyersList() {
            $('#modalBuyers').modal('hide');
        }
        function showAlert() {
            $('#modalAlert').modal('show');
        }
        function showPreview() {
            $('#modalPreview').modal('show');
        }
        function showMsgBox() {
            $('#MsgBox').modal('show');
        }
        function hideMsgBox() {
            $('#MsgBox').modal('hide');
        }
        function showMsgConfirm() {
            $('#MsgConfirm').modal('show');
        }
        function MsgLoanType_Show() {
            $('#MsgLoanType').modal('show');
        }
        function MsgLoanType_Hide() {
            $('#MsgLoanType').modal('hide');
        }
        function MsgAccreditedBanks_Show() {
            $('#MsgAccreditedBanks').modal('show');
        }
        function MsgAccreditedBanks_Hide() {
            $('#MsgAccreditedBanks').modal('hide');
        }

        function MsgWRAStatus_Show() {
            $('#MsgWRAStatus').modal('show');
        }
        function MsgWRAStatus_Hide() {
            $('#MsgWRAStatus').modal('hide');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><span>Documents</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-default panel-trans" id="form_buyer">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-7 col-sm-6 col-xs-12">
                    <h4 class="trans-title">Document Requirements</h4>
                </div>
                <div class="col-md-5 col-sm-6 col-xs-12">
                    <div class="trans-command">
                        <div class="btn-group" role="group">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelButton">
                                <ContentTemplate>
                                    <asp:Button runat="server" ID="Button1" Text="Reservation Details" CssClass="btn btn-info hidden" OnClick="bSearch_ServerClick" />
                                    <%--                                    <button class="btn btn-success" type="button" runat="server" id="btnSave" onserverclick="btnSave_ServerClick">
                                        Ready for
                                        <label runat="server" id="lblSave" style="height: 5px; font-weight: normal;" class="lblSave text-uppercase" />
                                        <i class="glyphicon glyphicon-check"></i>
                                    </button>--%>
                                    <button class="btn btn-danger btn-width" type="button" visible="false" runat="server" id="btnCancel" onserverclick="btnCancel_ServerClick">Cancel <i class="glyphicon glyphicon-remove"></i></button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>





        <div class="panel-body">




            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">Account Details</div>
                        <div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>



                                    <div class="row" style="float: right;">


                                        <div class="col-lg-6">
                                            <asp:Button runat="server" ID="btnSummary" Text="Reservation Details" CssClass="btn btn-info" data-toggle="modal" data-target="#modalSummary" />
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:Button runat="server" ID="btnSaveQuotation" Text="Save Quotation Details" OnClick="btnSaveQuotation_Click" CssClass="btn btn-success" />
                                        </div>
                                    </div>



                                    <div class="row">
                                        <div class="col-lg-12">




                                            <div class="col-lg-4">
                                                <div class="row" style="display: none;">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Document Entry:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <input type="text" style="z-index: auto;" class="form-control txtDocEntry" runat="server" id="txtDocEntry" disabled />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Document Number:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <input type="text" style="z-index: auto;" class="form-control txtDocNum" runat="server" id="txtDocNum" disabled /><span class="input-group-btn">
                                                                <button style="width: 50px; z-index: auto;" runat="server" id="bCustCode" onserverclick="bCustCode_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Document Date:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" runat="server" id="txtDocumentDate" disabled />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Customer Code:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" runat="server" id="txtCustCode" disabled />
                                                    </div>
                                                </div>



                                                <div runat="server" id="divNames">
                                                    <div class="row">
                                                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                            <h5>Last Name:</h5>
                                                        </div>
                                                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                            <input type="text" class="form-control" id="txtLName" runat="server" disabled />
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                            <h5>First Name:</h5>
                                                        </div>
                                                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                            <input type="text" class="form-control" id="txtFName" runat="server" disabled />
                                                        </div>
                                                    </div>

                                                    <div class="row" hidden>
                                                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                            <h5>Middle Name:</h5>
                                                        </div>
                                                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                            <input type="text" class="form-control" id="txtMName" runat="server" disabled />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div runat="server" id="divCompanyName">
                                                    <div class="row">
                                                        <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                            <h5>Company Name:</h5>
                                                        </div>
                                                        <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                            <input type="text" class="form-control" id="txtCompanyName" runat="server" disabled />
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>






                                            <%--project details--%>
                                            <%--<hr />--%>

                                            <div class="col-lg-4">
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Project:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtProj" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Phase:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtPhase" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Block:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtBlock" runat="server" disabled />
                                                    </div>
                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Lot:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtLot" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Model:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtModel" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row hidden" visibility="false" runat="server">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Adjacent Lot:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="lblAdjacentLot" runat="server" disabled />
                                                    </div>
                                                </div>

                                            </div>






                                            <div class="col-lg-4">
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Product Type:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtProdType" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>NET TCP:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtAcctDetailsNetTCP" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Fin. Scheme:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <input type="text" class="form-control" id="txtFinScheme" runat="server" disabled />
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Loan Type:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <input type="text" class="form-control" id="txtLoanType" runat="server" disabled />
                                                            <span class="input-group-btn" style="visibility: hidden;">
                                                                <button id="btnLoanType" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnLoanType_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row" runat="server" id="divBank">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Bank:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <input type="text" class="form-control" id="txtBank" runat="server" disabled /><span class="input-group-btn">
                                                                <button id="btnBank" runat="server" style="width: 50px; z-index: auto;" onserverclick="btnBank_ServerClick" class="btn btn-secondary" type="button"><i class="fa fa-bars"></i></button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>






                                            </div>



                                        </div>
                                    </div>




                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>




            <div class="row">
                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">WRA Status</div>
                        <div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>



                                    <div class="row">
                                        <div class="col-lg-12">


                                            <div class="col-lg-4">
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>WRA Status:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <div class="input-group">
                                                            <asp:Label runat="server" Visible="false" ID="lblWRACode"></asp:Label>
                                                            <input type="text" style="z-index: auto;" class="form-control txtWRAStatus" runat="server" id="txtWRAStatus" disabled /><span class="input-group-btn">
                                                                <button style="width: 50px; z-index: auto;" runat="server" id="btnWRAStatus" onserverclick="btnWRAStatus_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-lg-4">
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Update Date:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <asp:TextBox runat="server" AutoPostBack="true" ID="txtWRADate" type="date" class="form-control" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-lg-4">
                                                <div class="row">
                                                    <div class="col-lg-5 col-md-5 col-sm-12 col-xs-12">
                                                        <h5>Remarks:</h5>
                                                    </div>
                                                    <div class="col-lg-7 col-md-7 col-sm-12 col-xs-12">
                                                        <asp:TextBox ID="txtWRARemarks" Style="resize: none;" TextMode="MultiLine" Rows="3" class="form-control" runat="server" />
                                                    </div>
                                                </div>
                                            </div>



                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>


























































            <div class="row">

                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                    <%--document list--%>
                    <div class="panel panel-default">
                        <div class="panel-heading">Document List</div>
                        <div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <asp:Button runat="server" CssClass="btn btn-info" Text="Contract to Sell" Visible="False" ID="btnContractSell" OnClick="btnContractSell_Click" />
                                            <asp:Button runat="server" CssClass="btn btn-info" Text="Deed of Absolute Sale" Visible="False" ID="btnDAS" OnClick="btnDAS_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>



                            <div class="row">
                                <div class="col-lg-12" style="overflow-x: auto">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView runat="server" ID="gvDocList"
                                                AllowPaging="true"
                                                AllowSorting="true"
                                                CssClass="table table-hover table-responsive"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                OnRowDataBound="gvDocList_RowDataBound"
                                                OnRowCommand="gvDocList_RowCommand"
                                                OnPageIndexChanging="gvDocList_PageIndexChanging1">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>

                                                    <asp:TemplateField Visible="true">
                                                        <ItemTemplate>
                                                            <asp:Image runat="server" ID="docStatus" ImageUrl="~/assets/img/cancel.png" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:BoundField DataField="DocId" HeaderText="DocId" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:TemplateField HeaderText="ID Type" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="hidden">
                                                        <ItemTemplate>
                                                            <div style="width: 200px; overflow: hidden; white-space: nowrap;">
                                                                <asp:DropDownList ID="ddIDtype" runat="server" CssClass="form-control">
                                                                    <asp:ListItem Value="---SELECT ID Type---" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Value="TIN" Text="TIN"> </asp:ListItem>
                                                                    <asp:ListItem Value="SSS" Text="SSS"> </asp:ListItem>
                                                                    <asp:ListItem Value="PagIbig" Text="PagIbig"> </asp:ListItem>
                                                                    <asp:ListItem Value="Passport" Text="Passport"> </asp:ListItem>
                                                                    <asp:ListItem Value="Others" Text="Others"> </asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:BoundField DataField="Document" HeaderText="Document" />

                                                    <%--<asp:BoundField DataField="Attachment" HeaderText="Attachment" NullDisplayText=" " ItemStyle-CssClass="elipsis" />--%>
                                                    <asp:TemplateField HeaderText="Reference No">
                                                        <ItemTemplate>
                                                            <div style="width: 200px; overflow: hidden; white-space: nowrap;">
                                                                <asp:TextBox ID="lblReferenceNo" class="form-control" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Issue Date">
                                                        <ItemTemplate>
                                                            <div style="overflow: hidden; white-space: nowrap;">
                                                                <asp:TextBox ID="lblIssueDate" class="form-control" type="date" runat="server" />
                                                            </div>
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Expiration Date">
                                                        <ItemTemplate>
                                                            <div style="overflow: hidden; white-space: nowrap;">
                                                                <asp:TextBox ID="lblExpirationDate" class="form-control" type="date" runat="server" />
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="5px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" HeaderText="Attachment">
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <div class="form-group  center-block text-center ">
                                                                        <asp:Label runat="server" ID="lblFileName" Text=""></asp:Label>
                                                                        <asp:FileUpload ID="FileUpload1" CssClass="btn btn-default" CommandName="FileUpload1" runat="server" EnableViewState="true" />
                                                                        <asp:LinkButton ID="btnUpload" CssClass="btn btn-primary" runat="server" CommandName="Upload" Text="Upload" />
                                                                        <asp:LinkButton ID="btnPreview" CssClass="btn btn-info" Visible="false" runat="server" CommandName="Preview" Text="Preview" />
                                                                        <asp:LinkButton ID="btnRemove" CssClass="btn btn-danger" Visible="false" runat="server" CommandName="Remove" Text="Remove" />
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="btnUpload" />
                                                                    <asp:PostBackTrigger ControlID="btnPreview" />
                                                                    <asp:PostBackTrigger ControlID="btnRemove" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>


                                                    <asp:TemplateField HeaderText="Attachment" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                                        <ItemTemplate>
                                                            <div style="overflow: hidden; text-overflow: ellipsis; white-space: nowrap;">
                                                                <asp:Label ID="lblAttachment" runat="server"></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-picture" HeaderStyle-Width="10" CommandName="Sel" ItemStyle-HorizontalAlign="Center" HeaderText="Preview" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:TemplateField ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                                        <ItemTemplate>
                                                            <asp:FileUpload runat="server" ID="docImage" accept="image/*" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-upload" HeaderStyle-Width="10" CommandName="Upload" ItemStyle-HorizontalAlign="Center" HeaderText="Upload" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-trash" HeaderStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" HeaderText="Remove" ItemStyle-ForeColor="#e2584d" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Image runat="server" ID="Status" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cancel" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chk" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Cancel Remarks" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide">
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control input-sm" Width="200px" Text='<%# Eval("Remarks") %>' ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                </Columns>

                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="gvDocList" />
                                        </Triggers>
                                    </asp:UpdatePanel>






























                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <asp:GridView runat="server" ID="gvDocList2"
                                                AllowPaging="true"
                                                AllowSorting="true"
                                                CssClass="table table-hover table-responsive"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                OnRowDataBound="gvDocList2_RowDataBound"
                                                OnRowCommand="gvDocList2_RowCommand">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>
                                                    <asp:BoundField DataField="DocId" HeaderText="DocId" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="Document" HeaderText="Document" />
                                                    <asp:BoundField DataField="Attachment" HeaderText="Attachment" />
                                                    <asp:BoundField DataField="Status" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-picture" HeaderStyle-Width="10" CommandName="Sel" ItemStyle-HorizontalAlign="Center" HeaderText="Preview" />
                                                    <asp:ButtonField ControlStyle-CssClass="btn btn-success btn-xs" HeaderStyle-Width="10" CommandName="Accept" Text="Accept" ItemStyle-HorizontalAlign="Center" Visible="true" />
                                                    <asp:ButtonField ControlStyle-CssClass="btn btn-danger btn-xs" HeaderStyle-Width="10" CommandName="Decline" Text="Decline" ItemStyle-HorizontalAlign="Center" Visible="true" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control input-sm" placeholder="Remarks"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <asp:Image runat="server" ID="docStatus" Width="20" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>




        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">
    <div class="modal fade" id="modalBuyers" role="dialog" tabindex="-1">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg" role="document">
                    <div class="modal-content">
                        <div class="modal-header bg-color">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                            <h4 class="modal-title">Buyer List</h4>
                        </div>

                        <div class="modal-body">
                            <%--                            <div class="row">
                                <div class="col-lg-12">
                                    <div class="col-lg-3">
                                        <label>Select Business Type</label>
                                    </div>
                                    <div class="col-lg-6 col-md-4 col-sm-6 col-xs-8 " style="left:-65px;">
                                        <asp:DropDownList ID="txtBusinessType" AutoPostBack="true" OnSelectedIndexChanged="txtBusinessType_OnSelect" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="Individual','Corporation" Text="All" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="Individual" Text="Individual"></asp:ListItem>
                                            <asp:ListItem Value="Corporation" Text="Corporation"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>--%>

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                            <h5>Search: </h5>
                                        </div>
                                        <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                            <div class="input-group">
                                                <input runat="server" id="txtSearch" placeholder="Search here" class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                    <asp:LinkButton runat="server" ID="bSearch" Style="width: 50px;" CssClass="btn btn-default" OnClick="bSearch_ServerClick"><i class="fa fa-search"></i></asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <fieldset>
                                                <asp:GridView runat="server" ID="gvBuyers"
                                                    AllowPaging="true"
                                                    AllowSorting="true"
                                                    EmptyDataText="No Records Found"
                                                    Width="100%"
                                                    CssClass="table table-hover table-responsive"
                                                    AutoGenerateColumns="false"
                                                    GridLines="None"
                                                    PageSize="8"
                                                    OnRowDataBound="gvBuyers_RowDataBound"
                                                    OnPageIndexChanging="gvBuyers_PageIndexChanging"
                                                    OnRowCommand="gvBuyers_RowCommand"
                                                    OnRowCreated="gvBuyers_RowCreated"
                                                    OnSorting="gvBuyers_Sorting">
                                                    <HeaderStyle BackColor="#66ccff" />
                                                    <Columns>
                                                        <asp:BoundField DataField="DocEntry" ItemStyle-Width="0px" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="ProjCode" HeaderText="Project" SortExpression="ProjCode" />
                                                        <asp:BoundField DataField="Block" HeaderStyle-Width="75px" HeaderText="Block" SortExpression="Block" />
                                                        <asp:BoundField DataField="Lot" HeaderStyle-Width="65px" HeaderText="Lot" SortExpression="Lot" />
                                                        <asp:BoundField DataField="CardCode" HeaderText="Buyer's Code" />
                                                        <asp:BoundField DataField="DocNum" HeaderText="Document Number" />

                                                        <%--5--%>
                                                        <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="Phase" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="Model" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" NullDisplayText=" " />
                                                        <%--10--%>
                                                        <asp:BoundField DataField="FinancingScheme" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="ForwardedStatus" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="SapDocEntry" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="BusinessType" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="LoanType" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <%--15--%>
                                                        <asp:BoundField DataField="CivilStatus" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="Bank" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" NullDisplayText=" " />
                                                        <asp:BoundField DataField="DocumentDate" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="ProductType" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="NetTCP" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <%--20--%>
                                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                                        <asp:BoundField DataField="BusinessType" HeaderText="BusinessType" />
                                                        <asp:BoundField DataField="WRAStatus" HeaderText="WRAStatus" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="WRAUpdateDate" HeaderText="WRAUpdateDate" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:BoundField DataField="WRARemarks" HeaderText="WRARemarks" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                                        <asp:ButtonField ControlStyle-CssClass="btn btn-success fa fa-hand-o-up" CommandName="Select" CausesValidation="false" />

                                                    </Columns>
                                                    <PagerStyle CssClass="pagination-ys" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                                    <SortedAscendingHeaderStyle BackColor="#808080" />
                                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                                    <SortedDescendingHeaderStyle BackColor="#383838" />
                                                </asp:GridView>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-default btn-facebook" type="button" style="width: 100px;" data-dismiss="modal">OK</button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="modalPreview" class="modal fade">
        <div class="modal-dialog modal-default modal-lg" id="Div1" runat="server">
            <div class="modal-content">
                <div class="modal-header">
                    Image Preview
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Image runat="server" ID="imgPreview" Width="100%" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAlert" class="modal fade ">
        <div class="modal-dialog" id="alertModal" runat="server">
            <div class="modal-content">
                <div class="modal-body">
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alertIcon" Width="90" Height="90" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <h4>
                                        <asp:Label runat="server" ID="lblMessageAlert"></asp:Label></h4>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <div class="col-lg-12">
                            <button type="button" class="btn btn-primary" data-dismiss="modal" style="width: 90px;">OK</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalConfirmation" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/question.png" runat="server" ID="imgQuestion" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblConfirmationInfo"></asp:Label></h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--<div class="modal fade" id="MsgBox" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Document Requirements</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <p>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        Are you sure you want to set the selected transaction as ready for
                                        <label runat="server" id="lblMsg" style="height: 5px; font-weight: normal;" class="lblSave" />?
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnYes" style="width: 100px;" class="btn btn-primary" onserverclick="btnYes_ServerClick" type="button" data-dismiss="modal">Yes</button>
                            <button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">No</span></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>--%>


    <div class="modal fade" id="MsgConfirm" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Document Requirements</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <p>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        Are you sure you want to remove the image of the selected item?
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <button runat="server" id="btnConfirm" style="width: 100px;" class="btn btn-primary" onserverclick="btnConfirm_ServerClick" type="button" data-dismiss="modal">Yes</button>
                            <button type="button" style="width: 100px;" class="btn btn-default" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">No</span></button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="MsgLoanType" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Loan Type List</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvLoanType" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvLoanType_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Loan Type Code" />
                                            <asp:BoundField DataField="Name" HeaderText="Loan Type Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bLoanType" OnClick="bLoanType_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div id="MsgAccreditedBanks" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">List of Accredited Banks</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvBanks" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvBanks_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="Bank Code" />
                                            <asp:BoundField DataField="Name" HeaderText="Bank Name" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="bBanks" OnClick="bBanks_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>



    <div id="MsgWRAStatus" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">WRA Status</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvWRAStatus" runat="server" AllowPaging="true" AutoGenerateColumns="false" EmptyDataText="No Records Found"
                                        CssClass="table table-bordered table-hover " Width="100%" ShowHeader="True" PageSize="5" OnPageIndexChanging="gvWRAStatus_PageIndexChanging">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="Code" HeaderText="WRA Status Code" />
                                            <asp:BoundField DataField="Name" HeaderText="WRA Status" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="btnWRAStatus" OnClick="btnWRAStatus_Click" CssClass="btn btn-default btn-success" Width="100%" Height="100%" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>





    <div class="modal fade" id="modalSummary" role="dialog" tabindex="-1" style="z-index: 1051;">
        <div class="modal-dialog" role="document" style="width: 90%;">
            <div class="modal-content">
                <div class="modal-header bg-color">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">Reservation Details</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-4">
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                            <asp:UpdatePanel runat="server">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="lblDocEntry" CssClass="hide" />
                                                    <asp:Panel runat="server" ID="pnlBuyerInfo">
                                                        <div class="row">
                                                            <div class="col-lg-10">
                                                                <div class="customer-badge" style="background-color: white;">
                                                                    <%--#f1ffec--%>
                                                                    <%--<asp:LinkButton runat="server" ID="removebuyer" OnClick="removebuyer_Click" CssClass="pull-right"><span class="fa fa-times-circle pull-right" style="color: #E2584D;"></span></asp:LinkButton>--%>
                                                                    <div class="avatar">
                                                                        <img src="../assets/img/user.png" alt="" width="50" height="50">
                                                                    </div>
                                                                    <div class="details">
                                                                        <asp:Label runat="server" ID="lblName" CssClass="text-uppercase" Style="font-size: 16px;" Text="" />
                                                                        <div class="text-success balance">
                                                                            <asp:Label runat="server" ID="lblID" CssClass="small text-uppercase" Text="" /><br />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #f1ffec;">
                                                                <div class="row">
                                                                    <div class="col-lg-4">
                                                                        <h5 style="color: #67676C;">Quotation No.:</h5>
                                                                    </div>
                                                                    <div class="col-lg-8" style="text-align: right;">
                                                                        <asp:Label runat="server" ID="lblQuotationNum" CssClass="" Text="" Font-Size="14" ForeColor="#3333cc" Font-Bold="true" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="comment-block">
                                                            <div class="side-heading">
                                                                <div class="table-responsive">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <table class="table">
                                                                                <tr>
                                                                                    <th style="width: 50%">Last Name:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblLName" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>First Name:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblFName" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Project:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="txtProjId" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Block:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblBlock" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Lot:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblLot" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Model:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblModel" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Lot Area:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="txtLotArea" Text="" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Floor Area:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="txtFloorArea" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Financing Scheme:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblFinScheme" Text="" /></td>
                                                                                </tr>
                                                                                <%--<tr>
                                                                                    <th>Account Type:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="txtAcctType" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Sales Type:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="txtSalesType" Text="" /></td>
                                                                                </tr>--%>
                                                                                <tr>
                                                                                    <th>Product Type:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblProdType" CssClass="" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Loan Type:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblLoanType" CssClass="" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Bank:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblBank" CssClass="" Text="" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>Retitling Type:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblRetitlingType" /></td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <th>LOI:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblLoi" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <th>LTS No:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblLTSNo" /></td>
                                                                                </tr>

                                                                                <tr>
                                                                                    <th>Co-Borrower:</th>
                                                                                    <td>
                                                                                        <asp:Label runat="server" ID="lblCoBorrower" /></td>
                                                                                </tr>















                                                                            </table>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-8">
                                    <div class="panel panel-primary clearfix" style="margin-left: 10px;">
                                        <div class="panel-body">
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <asp:UpdatePanel runat="server">
                                                        <ContentTemplate>
                                                            <%--    <div class="row">
                                    <div class="col-lg-12">
                                        <button id="btnSchedule" runat="server" data-toggle="modal" data-target="#modalSchedule" class="btn btn-secondary btn-dropbox pull-right" type="button">Payment Schedule</button>
                                    </div>
                                </div>--%>
                                                            <div class="table-responsive">
                                                                <table class="table">
                                                                    <caption>Summary</caption>
                                                                    <tr>
                                                                        <th style="width: 50%">TCP Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDAS" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr runat="server" visible="false">
                                                                        <th>Miscellaneous:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMisc" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr runat="server" visible="false">
                                                                        <th>VAT:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblVAT" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Net Total Contract Price:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblNetTCP" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Total Discount Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDiscountAmount" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Total Discount Percent:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDiscountPercent" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                </table>
                                                                <table class="table">
                                                                    <caption>Down Payment</caption>
                                                                    <tr>
                                                                        <th style="width: 50%">Down Payment Percent:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDPPercent" Text="0" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Down Payment Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDPAmount" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Reservation Fee:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblRsvFee" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr runat="server" visible="false">
                                                                        <th>First Down Payment:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="txtFrstDP" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Net Down Payment:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblNetDP" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Terms(mos):</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDPTerms" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Due Date:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblDPDueDate" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Monthly DP:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMonthlyDP" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                </table>
                                                                <table class="table">
                                                                    <caption>Loanable</caption>
                                                                    <tr runat="server" visible="false">
                                                                        <th style="width: 50%">Loanable Percent:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblLPercent" Text="0" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Loanable Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblLAmount" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Terms(mos):</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblLTerms" Text="" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Interest Rate:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblRate" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Monthly Amortization:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMonthlyAmort" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                </table>


                                                                <table class="table">
                                                                    <caption>Miscellaneous</caption>
                                                                    <tr>
                                                                        <th>Misc Financing Scheme:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscFinancingScheme" Text="-" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Due Date:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscDueDate" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50%">Miscellaneous Fees:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscFees" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50%">Misc DP Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscDPAmount" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50%">Misc Monthly DP:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscDPMonthly" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Misc DP Terms(mos):</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscDPTerms" Text="" CssClass="pull-right" /></td>
                                                                    </tr>

                                                                    <tr>
                                                                        <th style="width: 50%">Misc LB Amount:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscLBAmount" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50%">Misc Monthly LB:</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscLBMonthly" Text="0.00" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <th>Misc LB Terms(mos):</th>
                                                                        <td>
                                                                            <asp:Label runat="server" ID="lblMiscLBTerms" Text="" CssClass="pull-right" /></td>
                                                                    </tr>
                                                                </table>

                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <asp:Panel ID="divMonthlyDP" runat="server">
                                        <div class="row">
                                            <h4 style="text-align: center;" id="dDown" runat="server">MONTHLY DOWNPAYMENT SCHEDULE</h4>
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView runat="server" ID="gvDownPayment"
                                                            CssClass="table table-hover table-responsive"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            PageSize="12"
                                                            OnPageIndexChanging="gvDownPayment_PageIndexChanging"
                                                            GridLines="None"
                                                            ShowHeader="true">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                                <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                                <asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                                <asp:BoundField DataField="InterestAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />
                                                                <asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />
                                                                <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
                                                                <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Balance" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="divMonthlyAmort" runat="server">
                                        <div class="row">
                                            <h4 style="text-align: center;" id="dAmort" runat="server">MONTHLY AMORTIZATION SCHEDULE</h4>
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView runat="server" ID="gvAmortization"
                                                            CssClass="table table-hover table-responsive"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            PageSize="12"
                                                            OnPageIndexChanging="gvAmortization_PageIndexChanging"
                                                            GridLines="None"
                                                            ShowHeader="true">
                                                            <HeaderStyle BackColor="#66ccff" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                                <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />
                                                                <asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                                <asp:BoundField DataField="InterestAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />
                                                                <asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />
                                                                <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
                                                                <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Balance" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </asp:Panel>







                                    <asp:Panel ID="divMonthlyDPMiscMiscDP" runat="server">
                                        <div class="row">
                                            <h4 style="text-align: center;" id="dMiscDP" runat="server">MONTHLY MISCELLANEOUS DOWNPAYMENT SCHEDULE</h4>
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView runat="server" ID="gvMiscellaneousDP"
                                                            CssClass="table table-hover table-responsive"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            PageSize="12"
                                                            OnPageIndexChanging="gvMiscellaneousDP_PageIndexChanging"
                                                            GridLines="None"
                                                            ShowHeader="true">
                                                            <HeaderStyle BackColor="#00cc66" />
                                                            <Columns>
                                                                <%--<asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                        <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                        <asp:BoundField DataField="PaymentAmount" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Payment Amount" />--%>
                                                                <%--<asp:BoundField DataField="Penalty" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />--%>
                                                                <%--<asp:BoundField DataField="Misc" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />--%>
                                                                <%--<asp:BoundField DataField="InterestRate" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Interest" />--%>
                                                                <%--<asp:BoundField DataField="Principal" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Principal" />--%>
                                                                <%--<asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="UnAllocated" />--%>
                                                                <%--<asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Balance" />--%>
                                                                <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd MMM yyyy}" />
                                                                <asp:BoundField DataField="PaymentAmount" DataFormatString="{0:###,###,##0.00}" HeaderText="Payment Amount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                <asp:BoundField DataField="Principal" DataFormatString="{0:###,###,##0.00}" HeaderText="Principal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                <asp:BoundField DataField="InterestAmount" DataFormatString="{0:###,###,##0.00}" HeaderText="Interest" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                <asp:BoundField DataField="Balance" DataFormatString="{0:###,###,##0.00}" HeaderText="Balance" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </asp:Panel>



                                    <asp:Panel ID="divMonthlyAmortMisc" runat="server">
                                        <div class="row">
                                            <h4 style="text-align: center;">MONTHLY MISCELLANEOUS AMORTIZATION SCHEDULE (LOANABLE BALANCE)</h4>
                                            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView runat="server" ID="gvMiscellaneousAmort"
                                                            CssClass="table table-hover table-responsive"
                                                            AutoGenerateColumns="false"
                                                            AllowPaging="true"
                                                            PageSize="12"
                                                            OnPageIndexChanging="gvMiscellaneousAmort_PageIndexChanging"
                                                            GridLines="None"
                                                            ShowHeader="true">
                                                            <HeaderStyle BackColor="#00cc66" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />

                                                                <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Payment Amount (PHP)" />
                                                                <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" DataFormatString="PHP {0:###,###,##0.##}" HeaderStyle-CssClass="text-center" HeaderText="Principal" />

                                                                <asp:BoundField DataField="InterestAmount" Visible="false" HeaderText="Interest" DataFormatString="PHP {0:###,###,##0.##}" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" />
                                                                <asp:BoundField DataField="Balance" DataFormatString="PHP {0:###,###,##0.00}" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-right" HeaderText="Balance (PHP)" />


                                                                <%--   <asp:BoundField DataField="Penalty" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Penalty" />
                                                                                    <asp:BoundField DataField="Misc" ControlStyle-CssClass="hidden" DataFormatString="PHP {0:###,###,##0.##}" HeaderText="Misc" />
                                                                                    <asp:BoundField DataField="UnAll" DataFormatString="PHP {0:###,###,##0.##}"
                                                                                        HeaderText="UnAllocated" Visible="false" />--%>
                                                            </Columns>
                                                            <PagerStyle CssClass="pagination-ys" />
                                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </asp:Panel>




                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
