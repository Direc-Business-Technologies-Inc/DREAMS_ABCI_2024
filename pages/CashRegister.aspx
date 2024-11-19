<%@ Page Title="DREAMS | Cash Register" Language="C#" MasterPageFile="~/master/Main.Master" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="CashRegister.aspx.cs" Inherits="ABROWN_DREAMS.CashRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        function showBank() {
            $('#modalBank').modal('show');
        }
        function hideBank() {
            $('#modalBank').modal('hide');
        }
        function showDepositAccounts() {
            $('#modalDepositAccounts').modal('show');
        }
        function hideDepositAccounts() {
            $('#modalDepositAccounts').modal('hide');
        }
        function showBranch() {
            $('#modalBranch').modal('show');
        }
        function hideBranch() {
            $('#modalBranch').modal('hide');
        }
        function showBuyer() {
            $('#modalBuyers').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function showAccounts() {
            $('#modalAccounts').modal('show');
        }
        function closeAccounts() {
            $('#modalAccounts').modal('hide');
        }
        function showCredit() {
            $('#modalCreditCard').modal('show');
        }
        function showHistory() {
            $('#modalHistory').modal('show');
        }
        function showPrint() {
            $('#modalPrint').modal('show');
        }
        function closeCredit() {
            $('#modalCreditCard').modal('hide');
        }
        function showCreditPayment() {
            $('#modalCredit').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeCreditPayment() {
            $('#modalCredit').modal('hide');
        }
        function showPymtMethod() {
            $('#modalCreditPaymentMethod').modal('show');
        }
        function closePymtMethod() {
            $('#modalCreditPaymentMethod').modal('hide');
        }

        function showCardBrandAccount() {
            $('#modalCardBrandAccount').modal('show');
        }
        function closeCardBrandAccount() {
            $('#modalCardBrandAccount').modal('hide');
        }


        function showCash() {
            $('#modalCash').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
        function closeCash() {
            $('#modalCash').modal('hide');
        }
        function showCheck() {
            $('#modalCheck').modal('show');
        }
        function closeCheck() {
            $('#modalCheck').modal('hide');
        }
        function closeAddPDC() {
            $('#modalAddPDC').modal('hide');
        }
        function showAddPDC() {
            $('#modalAddPDC').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function showConfirmation() {
            $('#modalConfirmation').modal({
                backdrop: 'static',
                keyboard: false
            });
        }

        function showListPDC() {
            $('#modalListPDC').modal('show');
        }
        function hideListPDC() {
            $('#modalListPDC').modal('hide');
        }
        function closeBuyer() {
            $('#modalBuyers').modal('hide');
        }
        function hideAlert() {
            $('#modalAlert').modal('hide');
        }
        function hideConfirm() {
            $('#modalConfirmation').modal('hide');
        }
        function showConfirmPDC() {
            $('#modalConfirmationPDC').modal('show');
        }
        function hideConfirmPDC() {
            $('#modalConfirmationPDC').modal('hide');
        }
        //with SetTimeout
        function showAlertSuccess() {
            $('#modalAlertSuccess').modal('show');
            setTimeout(function hideAlertSuccess() {
                $('#modalAlertSuccess').modal('hide');
            }, 2500);
        }
        function showInter() {
            $('#modalInterBranch').modal('show');
        }
        function hideInter() {
            $('#modalInterBranch').modal('hide');
        }
        function showInterBank() {
            $('#modalInterBank').modal('show');
        }
        function hideInterBank() {
            $('#modalInterBank').modal('hide');
        }
        function showOthers() {
            $('#modalOthers').modal('show');
        }
        function hideOthers() {
            $('#modalOthers').modal('hide');
        }
        function showOthersPaymentMean() {
            $('#modalOthersPaymentMean').modal('show');
        }
        function hideOthersPaymentMean() {
            $('#modalOthersPaymentMean').modal('hide');
        }


        debugger;
        var alert = "this is a debugger test";
        alert(alert);

    </script>
    <script type="text/javascript">

        $("a.SRF").on("click", function () {
            window.open('../pages/ReportViewer.aspx', '_blank');
        });
        function newtab() {
            window.open('www.google.com', '_blank');
        }
        //function SetTarget() {
        //    document.forms[0].target = "_blank";
        //}


    </script>
    <script type="text/javascript">  
        $(document).ready(function () {
            SearchText();
        });
        function SearchText() {
            $("#txtSearch").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "~/pages/CashRegister.aspx/TextSearch",
                        data: "{'search':'" + document.getElementById('txtSearch').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            alert("No Data Found");
                        }
                    });
                }
            });
        }
    </script>
    <script>
        $(document).ready(function () {
            // Add minus icon for collapse element which is open by default
            $(".collapse.in").each(function () {
                $(this).siblings(".panel-heading").find(".glyphicon").addClass("glyphicon-minus").removeClass("glyphicon-plus");
            });

            // Toggle plus minus icon on show hide of collapse element
            $(".collapse").on('show.bs.collapse', function () {
                $(this).parent().find(".glyphicon").removeClass("glyphicon-plus").addClass("glyphicon-minus");
            }).on('hide.bs.collapse', function () {
                $(this).parent().find(".glyphicon").removeClass("glyphicon-minus").addClass("glyphicon-plus");
            });
        });

    </script>
    <!-- ############ Number Only ############### -->
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;


            //$('#modalConfirmationPDC').on('shown.bs.modal', function () {
            //    $('#btnConfimPDC').focus();
            //})


            //$('#modalBank').on('shown.bs.modal', function () {
            //    $('#txtCheckBankSearch1').focus();
            //})

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sitemap" runat="server">
    <li><span>Payments</span></li>
    <li><span>Cash Register</span></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">

    <%--###############################--%>
    <%--FIELDS FOR RESTRUCTURING--%>
    <%--###############################--%>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <input type="text" runat="server" id="tResrvFee" class="tResrvFee hide" />
            <input type="text" runat="server" id="lblLoanableBalance" class="lblLoanableBalance hide" />
            <input type="text" runat="server" id="tNetDas" class="tNetDas hide" />
            <input type="text" runat="server" id="txtDiscountAmount" class="txtDiscountAmount hide" />
            <input type="text" runat="server" id="lblMiscMonthly" class="lblMiscMonthly hide" />
            <input type="text" runat="server" id="lblNetTCPn" class="lblNetTCPn hide" />


            <input type="text" runat="server" id="lblDPDueDate" class="lblDPDueDate hide" />
            <input type="text" runat="server" id="lblTCPMonthly2n" class="lblTCPMonthly2n hide" />
            <input type="text" runat="server" id="lblBalance2n" class="lblBalance2n hide" />
            <input type="text" runat="server" id="lblMiscFees2n" class="lblMiscFees2n hide" />

            <input type="text" runat="server" id="lblDueDate2n" class="lblDueDate2n hide" />
            <%--<input type="text" runat="server" id="lblLoanableBalance6n" class="lblLoanableBalance6n hide" />--%>
            <input type="text" runat="server" id="lblAddMiscCharges2n" class="lblAddMiscCharges2n hide" />
            <%--<input type="text" runat="server" id="lblMonthly2n" class="lblMonthly2n hide" />--%>

            <input type="text" runat="server" id="lblDownPaymentn" class="lblDownPaymentn hide" />
            <input type="text" runat="server" id="lblVATn" class="lblVATn hide" />
            <input type="text" runat="server" id="txtMiscMonthly" class="txtMiscMonthly hide" />
            <input type="text" runat="server" id="tPDBalance" class="tPDBalance hide" />
            <input type="text" runat="server" id="lblMiscMonthly2n" class="lblMiscMonthly2n hide" />
            <input type="text" runat="server" id="lblAddMiscFees2n" class="lblAddMiscFees2n hide" />
            <input type="text" runat="server" id="txtMiscFees" class="txtMiscFees hide" />
            <input type="text" runat="server" id="lblDueDate3n" class="lblDueDate3n hide" />

            <input type="text" runat="server" id="dtpDueDate" class="dtpDueDate hide" />

            <input type="text" runat="server" id="txtFinancingMisc" class="txtFinancingMisc hide" />




            <%--            <button runat="server" id="bLot" onserverclick="bLot_ServerClick" class="bLot hide" />--%>
        </ContentTemplate>
    </asp:UpdatePanel>




















    <div class="col-lg-4">
        <div class="panel panel-default">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlBuyerInfo">
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="customer-badge" style="background-color: white;">
                                        <div class="avatar">
                                            <img src="../assets/img/user.png" alt="" width="50" height="50">
                                        </div>
                                        <div class="details">
                                            <%--<a href="#">--%>
                                            <asp:Label runat="server" ID="lblName" CssClass="text-uppercase" Style="font-size: 16px;" Text="" />

                                            <%--</a>--%>
                                            <br />
                                            <div class="text-success balance">
                                                <asp:Label runat="server" ID="lblID" CssClass="small text-uppercase" Text="" />
                                                <br />
                                                <asp:Label runat="server" ID="lblTIN" CssClass="small text-uppercase hidden" Text="" />
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-2">
                                    <asp:LinkButton runat="server" ID="btnFind" CssClass="btn btn-info btn-circle pull-right" OnClick="btnFind_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                </div>
                            </div>
                            <br />
                            <img src="../assets/img/house.png" alt="" width="30" height="30" class="pull-left">
                            <h5 style="color: #67676C;">&nbsp;House/Lot:</h5>



                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server"
                                        ID="gvHouseLot"
                                        CssClass="table table-responsive table-hover"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="false"
                                        UseAccessibleHeader="true"
                                        SelectedRowStyle-BackColor="#A1DCF2"
                                        OnRowCommand="gvHouseLot_RowCommand"
                                        EmptyDataText="No records found">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDocEntry" CssClass="text-uppercase" Style="font-size: 16px;" Visible="false" Text='<%# Eval("DocEntry") %>' />
                                                    <asp:Label runat="server" ID="lblProject" CssClass="text-uppercase" Style="font-size: 16px;" Text='<%# Eval("PrjName") %>' />
                                                    <br />
                                                    <div class="text-success balance">
                                                        <asp:Label runat="server" ID="lblBlock" CssClass="small text-uppercase" Text='<%# "Block:" + Eval("Block") %>' />
                                                        <asp:Label runat="server" ID="lblLot" CssClass="small text-uppercase" Text='<%# "Lot:" + Eval("Lot") %>' />
                                                        <asp:Label runat="server" ID="lblModel" CssClass="small text-uppercase" Text='<%# "Model:" + Eval("Model") %>' />
                                                        <asp:Label runat="server" ID="lblStatus" CssClass="small text-uppercase" Text='<%# Eval("Status") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblPhase" CssClass="small text-uppercase" Text='<%# Eval("Phase") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblDocNum" CssClass="small text-uppercase" Text='<%# Eval("DocNum") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblFinscheme" CssClass="small text-uppercase" Text='<%# Eval("FinancingScheme") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblBlock1" CssClass="small text-uppercase" Text='<%# Eval("Block") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblLot1" CssClass="small text-uppercase" Text='<%# Eval("Lot") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblModel1" CssClass="small text-uppercase" Text='<%# Eval("Model") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblFinschemeCode" CssClass="small text-uppercase" Text='<%# Eval("FinCode") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblProjCode" CssClass="small text-uppercase" Text='<%#  Eval("ProjCode") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblApproved" CssClass="small text-uppercase" Text='<%#  Eval("Approved") %>' Visible="false" />
                                                        <asp:Label runat="server" ID="lblMiscFinSchemeCode" CssClass="small text-uppercase" Text='<%#  Eval("MiscFinancingScheme") %>' Visible="false" />
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success btn-sm fa fa-arrow-right pull-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>











                            <div class="row">
                                <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #f1ffec;">
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <img src="../assets/img/profile.png" alt="" width="30" height="30" class="pull-left">
                                            <h5 style="color: #67676C;">Quotation Details</h5>
                                        </div>
                                        <div class="col-lg-8" style="text-align: right;">
                                            <button class="btn btn-default" type="button" style="width: 40px; height: 30px;" data-toggle="collapse" data-target="#profile"><i class="fa fa-list"></i></button>
                                        </div>
                                    </div>
                                </div>
                            </div>





                            <div class="collapse" id="profile">
                                <div class="table-responsive">
                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>
                                            <table class="table">
                                                <tr>
                                                    <th>Document No:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtDocNum" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th style="width: 50%">Project:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtProj" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Phase:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtPhase" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Block:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtBlock" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Lot:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtLot" Text="" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>House Model:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtModel" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>NET TCP:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblNETTCP" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Total TCP Paid:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTotalPayment" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>% TCP Paid:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblPercentPaid" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>TCP Financing Scheme:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTCPFinScheme" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Total Miscellaneous Fee:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTotalMiscFee" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Total Misc Paid:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTotalMiscPaid" />
                                                    </td>
                                                </tr>

                                                <%--         //2023-05-31 : REQUESTED BY MS KATE : ADD MISC FINANCING SCHEME--%>
                                                <tr>
                                                    <th>Misc Financing Scheme:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblMiscFinScheme" />
                                                    </td>
                                                </tr>


                                                <tr>
                                                    <th>Book Status:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtBookStatus" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>LOI:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblLoi" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>LTS No:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblLTSNo" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Sales Agent:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblSalesAgent" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Co-borrower:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblCoBorrower" />
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <th>Tax Classification:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblTaxClassification" />
                                                    </td>
                                                </tr>











                                                <tr style="display: none;">
                                                    <th>First Name:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtFName" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <th>Last Name:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtLName" />
                                                    </td>
                                                </tr>
                                                <tr style="display: none;">
                                                    <th>Middle Name:</th>
                                                    <td>
                                                        <asp:Label runat="server" ID="txtMName" />
                                                    </td>
                                                </tr>



                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>


        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:Panel runat="server" ID="pnlPayment">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:GridView runat="server"
                                        ID="gvPayments"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        BorderStyle="None"
                                        OnRowCommand="gvPayments_RowCommand">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="LineNum" HeaderText="LineNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Type" ItemStyle-Width="100%" HeaderText="Type" />
                                            <asp:BoundField DataField="Amount" ItemStyle-Width="100%" ItemStyle-HorizontalAlign="right" DataFormatString="{0:#,##0.00}" HeaderText="Amount" />
                                            <%--2--%>

                                            <%--CHECK--%>
                                            <asp:BoundField DataField="CheckNo" ItemStyle-Width="100%" HeaderText="Check No." />
                                            <asp:BoundField DataField="BankCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Bank" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Branch" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="DueDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="AccountNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <%--8--%>


                                            <%--CREDIT--%>
                                            <asp:BoundField DataField="CreditCard" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="CreditAcctCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="CreditAcct" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="CreditCardNumber" ItemStyle-Width="100%" HeaderText="Credit Card No." />
                                            <asp:BoundField DataField="ValidUntil" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="IdNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                            <asp:BoundField DataField="TelNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText=" " />
                                            <%--15--%>


                                            <asp:BoundField DataField="PymtTypeCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="PymtType" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="NumOfPymts" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="VoucherNum" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Id" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OR" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <%--21--%>

                                            <%--DEPOSIT DETAILS--%>
                                            <asp:BoundField DataField="DepositedBankID" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="DepositedBank" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="DepositedBranch" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="CheckAccount" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <%--25--%>

                                            <%--INTERBANK--%>
                                            <asp:BoundField DataField="InterBankDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="InterBankBank" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="InterBankGLAcc" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="InterBankAccNo" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="InterBankAmount" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <%--30--%>

                                            <%--OTHERS--%>
                                            <asp:BoundField DataField="OthersModeOfPaymentCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersModeOfPayment" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersAmount" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersReferenceNo" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersGLAccountCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersGLAccountName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="OthersPaymentDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <%--37--%>

                                            <asp:BoundField DataField="CheckPDCId" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="CreditCardBrandAccountCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />
                                            <asp:BoundField DataField="CreditCardBrandAccountName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" NullDisplayText="0" />



                                            <asp:ButtonField ControlStyle-CssClass="glyphicon glyphicon-edit" ItemStyle-Width="10" CommandName="Edt" ItemStyle-HorizontalAlign="Center" />
                                            <asp:ButtonField ControlStyle-CssClass="fa fa-times-circle" ItemStyle-Width="10" CommandName="Del" ItemStyle-HorizontalAlign="Center" ItemStyle-ForeColor="#e2584d" />
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--<hr style="border-top: dashed 1px #D0D3D8;" />--%>
                            <h5 style="color: #67676C;">Add Payment</h5>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <button runat="server" id="btnCash" class="btn btn-primary" onserverclick="btnCash_Click">Cash</button>
                                    <button runat="server" id="btnCheck" class="btn btn-primary" onserverclick="btnCheck_Click">Check</button>
                                    <button runat="server" id="btnCredit" class="btn btn-primary" onserverclick="btnCredit_Click">Credit</button>
                                    <button runat="server" id="btnInterbranch" class="btn btn-primary" onserverclick="btnInterbranch_Click">Interbranch</button>
                                    <button runat="server" id="btnOthers" class="btn btn-primary" onserverclick="btnOthers_ServerClick">Others</button>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #ffdedc;">
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <h5 style="color: #67676C;">Amount Due:</h5>
                                                </div>
                                                <div class="col-lg-6" style="text-align: right;">
                                                    <asp:Label runat="server" ID="lblDue" Text="0.00" Font-Size="14" ForeColor="#d43f3a" Font-Bold="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row" hidden>
                                        <div class="col-lg-6 col-sm-12 box-container-left">
                                            <div class="col-lg-12">
                                                <h4 style="color: #67676C;">Total</h4>
                                            </div>
                                            <div class="col-lg-12">
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label runat="server" ID="lblAmount" CssClass="" Text="0.00" Font-Size="16" ForeColor="#33cc33" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-6 col-sm-12 box-container-right">
                                            <div class="col-lg-12">
                                                <h4 style="color: #67676C;">Payment</h4>
                                            </div>
                                            <div class="col-lg-12">
                                                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                                                    <asp:Label runat="server" ID="lblAmountDue" CssClass="" Text="0.00" Font-Size="16" ForeColor="#ff9933" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #a0d2db;">
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <h5 style="color: #67676C;">Balance:</h5>
                                                </div>
                                                <div class="col-lg-6" style="text-align: right;">
                                                    <h4>
                                                        <asp:Label runat="server" ID="txtBalance" CssClass="" Text="0.00" Font-Size="14" ForeColor="#d43f3a" Font-Bold="true" />
                                                    </h4>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--2023-07-05 : REMOVE THE FIELD --%>
                                    <%--2023-07-04 : ADD TAX CLASSIFICATION UI--%>
                                    <%--  <div class="row">
                                        <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: floralwhite;">
                                            <div class="row">
                                                <div class="col-lg-6">
                                                    <h5 style="color: #67676C;">Payment:</h5>
                                                </div>
                                                <div class="col-lg-6" style="text-align: right;">
                                                    <asp:Label runat="server" ID="lblTaxClassification" Text=" - " Font-Size="12" ForeColor="#d43f3a" Font-Bold="true" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <div class="comment-block">

                                <div class="side-heading" runat="server" id="divAdvancePrincipal" visible="false">

                                    <div class="row">
                                        <div class="col-lg-12 center-block">
                                            <asp:Button runat="server" CssClass="btn btn-info btn-large btn-block" type="button" ID="btnApplyToPrincipal" OnClick="btnApplyToPrincipal_Click" Text="Apply To Principal Only?" />
                                        </div>
                                        <div class="col-lg-12 center-block text-center">
                                            <asp:Label runat="server" ID="lblApplyToPrincipal" CssClass="" Text="NO" Font-Size="14" Font-Bold="true" />
                                            </h4>
                                        </div>
                                    </div>

                                    <hr />
                                </div>


                                <div class="side-heading">
                                    <label>Posting Date: </label>
                                </div>
                                <input type="date" class="form-control" id="tDocDate" runat="server" />



                                <hr />

                                <div class="side-heading">
                                    <label>Surcharge Date: </label>
                                </div>

                                <div class="row">
                                    <div class="col-lg-11">
                                        <div class="side-heading">
                                            <asp:TextBox runat="server" ID="tSurChargeDate" CssClass="form-control" type="date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-1">
                                        <span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnSurchargeDate1" OnClick="btnSurchargeDate_Click" CssClass="btn btn-secondary btn-circle"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </div>

                                <hr />



                                <div class="row">
                                    <%--<div class="col-lg-6">--%>
                                    <div class="col-lg-11">
                                        <div class="side-heading">
                                            <div class="row">
                                                <div class="col-lg-5 left">
                                                    <label>Enter OR #: </label>
                                                </div>
                                                <div class="col-lg-7 right">
                                                    <label>OR For Miscellaneous Payment: </label>
                                                    <%--//2023-09-28 : CONDITION TO REMOVE BLOCKING IF OR/AR FOR MISC PAYMENT WHEN LOI TO LTS WHEN THE CHECKBOX IS CHECKED--%>
                                                    <asp:CheckBox runat="server" ID="chkRemoveBlocking" />
                                                </div>
                                            </div>
                                            <asp:TextBox runat="server" ID="txtOR" CssClass="form-control disabled" MaxLength="25" placeholder="Enter OR Number"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-lg-1">
                                        <div class="side-heading">
                                            <label>|  </label>
                                        </div>
                                        <span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnOR" CssClass="btn btn-secondary btn-circle" OnClick="btnOR_Click" AccessKey="Z"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                    <%--<div class="col-lg-5">--%>
                                    <div class="col-lg-5 hidden">
                                        <div class="side-heading">
                                            <label>OR Date: </label>
                                        </div>
                                        <asp:TextBox runat="server" ID="tORDate" CssClass="form-control" type="date"></asp:TextBox>
                                    </div>
                                </div>





                                <div class="row">
                                    <%--<div class="col-lg-6">--%>
                                    <div class="col-lg-11">
                                        <div class="side-heading">
                                            <label>Enter AR #: </label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtAR" CssClass="form-control disabled" MaxLength="25" placeholder="Enter AR Number"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-1">
                                        <div class="side-heading">
                                            <label>|  </label>
                                        </div>
                                        <span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnAR" CssClass="btn btn-secondary btn-circle" OnClick="btnAR_Click"><i class="fa fa-search" accesskey="X"></i></asp:LinkButton>
                                        </span>
                                    </div>

                                    <%-- <div class="col-lg-5">--%>
                                    <div class="col-lg-5 hidden">
                                        <div class="side-heading">
                                            <label>AR Date: </label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtARDate" CssClass="form-control" type="date"></asp:TextBox>
                                    </div>
                                </div>






                                <div class="row">
                                    <%--<div class="col-lg-6">--%>
                                    <div class="col-lg-11">
                                        <div class="side-heading">
                                            <label>Enter PR #: </label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtPR" CssClass="form-control disabled" MaxLength="25" placeholder="Enter PR Number"></asp:TextBox>
                                    </div>
                                    <div class="col-lg-1">
                                        <div class="side-heading">
                                            <label>|  </label>
                                        </div>
                                        <span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnPR" CssClass="btn btn-secondary btn-circle" OnClick="btnPR_Click"><i class="fa fa-search" accesskey="C"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                    <%--<div class="col-lg-5">--%>
                                    <div class="col-lg-5 hidden">
                                        <div class="side-heading">
                                            <label>PR Date: </label>
                                        </div>
                                        <asp:TextBox runat="server" ID="txtPRDate" CssClass="form-control" type="date"></asp:TextBox>

                                    </div>
                                </div>


                                <hr />



                                <div class="side-heading">
                                    <label id="comment_label" for="comment">Comments : </label>
                                </div>
                                <asp:TextBox runat="server" ID="txtComment" CssClass="form-control" TextMode="MultiLine" Style="width: 100%; max-width: 100%; min-width: 100%; max-height: 100px; min-height: 100px;"></asp:TextBox>
                                <%--<textarea name="comment" cols="40" rows="2" id="comment" class="form-control" data-title="Comments" style="width: 100%; max-width: 100%; min-width:100%; max-height:100px;min-height:100px;"></textarea>--%>
                            </div>
                            <div class="finish-sale">
                                <asp:Button runat="server" ID="btnFinish" CssClass="btn btn-success btn-large btn-block" Text="Save" AccessKey="A" OnClick="btnFinish_Click" />
                                <%--<button runat="server" class="btn btn-success btn-large btn-block">Finish</button>--%>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="col-lg-8">
        <div class="panel panel-default clearfix" style="margin-left: 10px;">
            <div class="panel-body">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="col-lg-12">
                            <div class="row">
                                <div style="float: right;">
                                    <asp:Button runat="server" ID="btnPymtHistory" CssClass="btn btn-info btn-sm" Text="Payment History" OnClick="btnPymtHistory_Click" />
                                </div>
                                <div style="float: right;">
                                    <asp:Button runat="server" ID="btnPrintSurcharge" CssClass="btn btn-success btn-sm" Text="Surcharge Form" OnClick="btnPrintSurcharge_Click" />
                                </div>
                                <div style="float: right;" hidden>
                                    <asp:Button runat="server" ID="btnBLedger" CssClass="btn btn-success btn-sm" Text="Buyer's Ledger" OnClick="btnBLedger_Click" OnClientClick="SetTarget();" />
                                </div>
                                <div style="float: right;">
                                    <asp:Button runat="server" ID="btnDemandLetter" CssClass="btn btn-success btn-sm" Text="Demand Letter" OnClick="btnDemandLetter_Click" />
                                </div>
                            </div>
                            <div class="row" style="overflow: auto;">
                                <div class="col-lg-12">
                                    <asp:Label runat="server" ID="pymntTitle" CssClass="" Text="Equity / Down Payments" Visible="false"></asp:Label>
                                    <asp:GridView runat="server"
                                        ID="gvDuePayments"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        ShowHeader="true"
                                        SelectedRowStyle-BackColor="#A1DCF2"
                                        UseAccessibleHeader="true"
                                        OnRowCommand="gvDuePayments_RowCommand"
                                        OnPageIndexChanging="gvDuePayments_PageIndexChanging"
                                        EmptyDataText="No records found">
                                        <HeaderStyle BackColor="#d7dce5" />
                                        <Columns>
                                            <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="DocNum" HeaderText="Doc No." />
                                            <asp:BoundField DataField="CardCode" HeaderText="CardCode" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-CssClass="" />
                                            <asp:BoundField DataField="DocDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="DocStatus" HeaderText="DocStatus" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField DataField="Payment" HeaderText="Payment" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:#,##0.00}" />
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>







                                    <%--TEMPORARY TABLES FOR GENERATION OF SCHEDULES--%>
                                    <%--MONTHLY DOWNPAYMENT SCHEDULE--%>

                                    <div class="row" id="divAdvancePaymentscheduleAccordion" runat="server" visible="false">
                                        <div class="col-lg-12 col-sm-12 box-container" style="height: 50px; background-color: #f1ffec;">
                                            <div class="row">
                                                <div class="col-lg-4">
                                                    <%--<img src="../assets/img/profile.png" alt="" width="30" height="30" class="pull-left">--%>
                                                    <h5 style="color: #67676C;">Advance Payment Schedule Preview</h5>
                                                </div>
                                                <div class="col-lg-8" style="text-align: right;">
                                                    <button class="btn btn-default" type="button" style="width: 40px; height: 30px;" data-toggle="collapse" data-target="#AdvancePaymentschedule"><i class="fa fa-list"></i></button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>



                                    <div class="collapse" id="AdvancePaymentschedule" style="background-color: #f1ffec;">
                                        <div class="row">
                                            <div class="col-lg-12">

                                                <br />


                                                Total LB Amount: 
                                                <asp:Label runat="server" ID="lblLoanableBalance6nNew"></asp:Label>
                                                <br />

                                                LB Monthly Amount:
                                                <asp:Label runat="server" ID="lblMonthly2nNew"></asp:Label>
                                                <%--<asp:Label runat="server" ID="lblAddMiscCharges2n"></asp:Label>--%>



                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="divMonthlyDP" Visible="false" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY DOWNPAYMENT SCHEDULE</h4>
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
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Principal" />
                                                                                    <asp:BoundField DataField="InterestAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Interest" />
                                                                                    <asp:BoundField DataField="IPS" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="IP&S" />
                                                                                    <asp:BoundField DataField="Balance" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="Balance" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>

                                                        <%--MONTHLY AMORTIZATION SCHEDULE--%>
                                                        <asp:Panel ID="divMonthlyAmort" runat="server">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY AMORTIZATION SCHEDULE</h4>
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
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Principal" />
                                                                                    <asp:BoundField DataField="InterestAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Interest" />
                                                                                    <asp:BoundField DataField="IPS" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="IP&S" />
                                                                                    <asp:BoundField DataField="Balance" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="Balance" />

                                                                                    <%--     <asp:BoundField DataField="Penalty" Visible="false" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="Penalty (PHP)" />
                                                                                    <asp:BoundField DataField="Misc" Visible="false" HeaderStyle-CssClass="text-center hidden" ItemStyle-CssClass="text-r hiddendden" DataFormatString="{0:###,###,##0.00}" HeaderText="Misc (PHP)" />
                                                                                    <asp:BoundField DataField="UnAll" ControlStyle-CssClass="hidden" DataFormatString="{0:###,###,##0.##}" HeaderText="UnAllocated" Visible="false" />--%>
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>


                                                        <%--MONTHLY MISC SCHEDULE--%>
                                                        <asp:Panel ID="divMonthlyDPMisc" runat="server" Visible="false">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY MISCELLANEOUS DOWNPAYMENT SCHEDULE</h4>
                                                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                                                    <asp:UpdatePanel runat="server">
                                                                        <ContentTemplate>
                                                                            <asp:GridView runat="server" ID="gvMiscellaneous"
                                                                                CssClass="table table-hover table-responsive"
                                                                                AutoGenerateColumns="false"
                                                                                AllowPaging="true"
                                                                                PageSize="12"
                                                                                OnPageIndexChanging="gvMiscellaneous_PageIndexChanging"
                                                                                GridLines="None"
                                                                                ShowHeader="true">
                                                                                <HeaderStyle BackColor="#00cc66" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="Terms" HeaderText="Line Item No." />
                                                                                    <asp:BoundField DataField="PaymentType" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" HeaderText="Payment Type" />
                                                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                                                    <asp:BoundField DataField="PaymentAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Payment Amount (PHP)" />
                                                                                    <asp:BoundField DataField="Principal" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Principal" />
                                                                                    <asp:BoundField DataField="InterestAmount" ItemStyle-CssClass="text-right" HeaderStyle-CssClass="text-center" DataFormatString="PHP {0:###,###,##0.00}" HeaderText="Interest" />
                                                                                    <asp:BoundField DataField="IPS" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="IP&S" />
                                                                                    <asp:BoundField DataField="Balance" HeaderStyle-CssClass="text-center" ItemStyle-CssClass="text-center" DataFormatString="{0:###,###,##0.00}" HeaderText="Balance" />
                                                                                </Columns>
                                                                                <PagerStyle CssClass="pagination-ys" />
                                                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#C2D69B" ForeColor="Blue" Font-Bold="true" />
                                                                            </asp:GridView>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                            </div>
                                                        </asp:Panel>




                                                        <asp:Panel ID="divMonthlyAmortMisc" runat="server" Visible="false">
                                                            <div class="row">
                                                                <h4 style="text-align: center;">MONTHLY MISCELLANEOUS AMORTIZATION SCHEDULE</h4>
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
















                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>


                                    <hr />



















                                    <asp:UpdatePanel runat="server">
                                        <ContentTemplate>

                                            <asp:GridView runat="server"
                                                ID="gvDownPayments"
                                                CssClass="table table-responsive"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                ShowHeader="true"
                                                SelectedRowStyle-BackColor="#A1DCF2"
                                                UseAccessibleHeader="true"
                                                EmptyDataText="No records found"
                                                OnRowCommand="gvDownPayments_RowCommand">


                                                <HeaderStyle BackColor="#d7dce5" />
                                                <Columns>

                                                    <asp:BoundField DataField="DocEntry" HeaderText="DocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <asp:CheckBox runat="server" AutoPostBack="true" OnCheckedChanged="chkSel_CheckedChanged" ID="chkSel" CommandName="chk" CausesValidation="false" />
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:PostBackTrigger ControlID="chkSel" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="Terms" HeaderText="Terms" />
                                                    <asp:BoundField DataField="PaymentType" HeaderText="Type" />
                                                    <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                                    <asp:BoundField DataField="PaymentAmount" HeaderText="Amount Due" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="text-right" />
                                                    <%--5--%>
                                                    <asp:BoundField DataField="Principal" HeaderText="Principal" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="text-right" />
                                                    <asp:BoundField DataField="InterestAmount" HeaderText="Interest" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="text-right" />
                                                    <asp:BoundField DataField="Penalty" HeaderText="SurCharges" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-danger text-right" />
                                                    <asp:BoundField DataField="IPS" HeaderText="IP&S" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-danger text-right" />
                                                    <asp:BoundField DataField="CashDiscount" HeaderText="Cash Discount" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" HeaderStyle-CssClass="Hide" ItemStyle-CssClass="text-danger text-right hide" />
                                                    <%--10--%>
                                                    <asp:BoundField DataField="CashDiscountValidDate" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" HeaderText="Discount Date" />
                                                    <asp:BoundField DataField="AmountPaid" HeaderText="Amount Paid" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-right" />
                                                    <asp:BoundField DataField="Payment" HeaderText="Payment" DataFormatString="{0:#,##0.00}" ItemStyle-Font-Bold="true" ItemStyle-CssClass="text-right" />
                                                    <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:#,##0.00}" ItemStyle-CssClass="text-right" />
                                                    <asp:BoundField DataField="DPEntry" HeaderText="DPEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <%--15--%>
                                                    <asp:BoundField DataField="LineStatus" HeaderText="LineStatus" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <asp:BoundField DataField="InterestAmount" HeaderText="OGInterest" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <%--<asp:BoundField DataField="CancelPenalty" HeaderText="CancelPenalty" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />--%>
                                                </Columns>
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>

                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="gvDownPayments" />
                                        </Triggers>
                                    </asp:UpdatePanel>


                                </div>
                            </div>
                        </div>

                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnBLedger" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="modals" runat="server">









    <div id="modalBuyers" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <h4 class="modal-title">Buyer's List</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <%--     <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search: </h5>
                                </div>--%>
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                    <asp:Panel runat="server" DefaultButton="bSearchBuyer">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchBuyer" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="bSearchBuyer" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook"
                                                    OnClick="bSearchBuyer_Click">
                                                    <i class="fa fa-search"></i>
                                                </asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>

                                </div>

                            </div>
                            <br />
                            <asp:GridView runat="server"
                                ID="gvBuyers"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="true"
                                PageSize="10"
                                UseAccessibleHeader="true"
                                OnRowCommand="gvBuyers_RowCommand"
                                OnPageIndexChanging="gvBuyers_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CardCode" HeaderText="Code" />
                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="BirthDay" HeaderText="BirthDay" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="IDNo" HeaderText="TIN" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <%--<asp:BoundField DataField="LTSNo" HeaderText="LTS No"  />--%>
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button runat="server" id="btnClose" type="button" class="btn btn-danger" onserverclick="btnClose_ServerClick">Cancel</button>
                </div>
            </div>
        </div>
    </div>





    <div id="modalCash" class="modal" role="dialog" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Cash Amount</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="bAddCash">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="input-group">
                                    <asp:TextBox runat="server" CssClass="form-control" placeholder="0" TabIndex="0" ID="txtCashAmount" Style="text-align: right;" TextMode="Number"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton runat="server" ID="bAddCash" Style="width: 100px;" CssClass="btn btn-secondary btn-primary btn-facebook" OnClick="bAddCash_Click" Text="Add"></asp:LinkButton>
                                    </span>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>










    <div id="modalHistory" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Payment History</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="height: auto;">

                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>


                                    <div class="row">
                                        <asp:Panel runat="server" DefaultButton="btnSearchHistory">
                                            <div class="input-group">
                                                <input runat="server" id="txtSearchHistory" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                    <asp:LinkButton runat="server" ID="btnSearchHistory" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchHistory_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </span>
                                            </div>
                                        </asp:Panel>
                                    </div>

                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 pre-scrollable" style="height: inherit;">



                                        <div class="row">
                                            <asp:GridView runat="server"
                                                ID="gvHistory"
                                                CssClass="table table-responsive"
                                                AutoGenerateColumns="false"
                                                GridLines="None"
                                                ShowHeader="true"
                                                OnRowCommand="gvHistory_RowCommand"
                                                EmptyDataText="No records found">
                                                <HeaderStyle BackColor="#66ccff" />
                                                <Columns>

                                                    <%--2023-07-02: REMOVE DATA BINDING FOR THESE 3--%>
                                                    <%--<asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="RptName" HeaderText="RptName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField DataField="RptPath" HeaderText="RptPath" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />--%>
                                                    <asp:BoundField HeaderText="Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField HeaderText="RptName" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                    <asp:BoundField HeaderText="RptPath" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />

                                                    <%--2023-07-02: CHANGE DATA BINDING COLUMN NAMES --%>
                                                    <%--<asp:BoundField DataField="DocDate" HeaderText="Date" --%>
                                                    <%--<asp:BoundField DataField="Total" HeaderText="Amount" DataFormatString="{0:#,##0.00}"  --%>
                                                    <%-- <asp:BoundField DataField="ORNumber" HeaderText="Receipt No." / --%>
                                                    <asp:BoundField DataField="RCPTDate" DataFormatString="{0:MM/dd/yyyy}" HeaderText="Date" />
                                                    <asp:BoundField DataField="PaidAmount" HeaderText="Amount" DataFormatString="{0:#,##0.00}" />
                                                    <asp:BoundField DataField="RCPTNo" HeaderText="Receipt No." />


                                                    <%--5--%>


                                                    <asp:BoundField DataField="Cancelled" HeaderText="Cancelled" ItemStyle-CssClass="text-center" />

                                                    <%--2023-07-02: ADDED DAIF TAGGING--%>
                                                    <asp:BoundField DataField="DAIF" HeaderText="DAIF" ItemStyle-CssClass="text-center" />

                                                    <%--2023-07-02: CHANGE DATA BINDING COLUMN NAMES --%>
                                                    <%--<asp:BoundField DataField="Remarks" HeaderText="Remarks" --%>
                                                    <asp:BoundField DataField="Comments" HeaderText="Remarks" />

                                                    <asp:TemplateField HeaderText="OR">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnPrint" Text="Print" runat="server" OnClick="btnPrint_Click" OnClientClick="SetTarget();" CommandArgument='<%# Container.DataItemIndex %>'>
                                                                Print <i class="fa fa-print"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AR">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="btn btn-info btn-sm" ID="btnPrintAR" Text="Print" runat="server" OnClick="btnPrintAR_Click" OnClientClick="SetTarget();" CommandArgument='<%# Container.DataItemIndex %>'>
                                                                Print <i class="fa fa-print"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="PR">
                                                        <ItemTemplate>
                                                            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnPrintPR" Text="Print" runat="server" OnClick="btnPrintPR_Click" OnClientClick="SetTarget();" CommandArgument='<%# Container.DataItemIndex %>'>
                                                                Print <i class="fa fa-print"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--2023-07-02: CHANGE DATA BINDING COLUMN NAMES --%>
                                                    <%--<asp:BoundField DataField="SapDocEntry" HeaderText="SapDocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" --%>
                                                    <asp:BoundField DataField="RCTEntry" HeaderText="SapDocEntry" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                                </Columns>
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                                <PagerStyle CssClass="pagination-ys" />
                                            </asp:GridView>
                                        </div>


                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalPrint" class="modal fade" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content modal-sm">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Demand Letter</h4>
                </div>
                <div class="modal-body">
                    <div class="row ">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                            <%--<div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 pre-scrollable">--%>
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                            <div class="row text-center">
                                                <label>Letter Date</label>
                                            </div>

                                            <div class="input-group">
                                                <label>LVL 1</label><span class="input-group-btn">
                                                    <button id="btnDemand1" onserverclick="btnDemand1_Click" validationgroup="Demand1" runat="server" style="width: 50px;" class="btn btn-info form-control" type="button"><i class="fa fa-print"></i></button>
                                                </span>
                                                <span class="input-group-btn">
                                                    <input type="date" runat="server" id="txtDemand1" class="form-control" />
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator1"
                                                ControlToValidate="txtDemand1"
                                                Text="Need Date for the Form Parameter."
                                                ValidationGroup="Demand1"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <label>LVL 2</label><span class="input-group-btn">
                                                    <button id="btnDemand2" runat="server" validationgroup="Demand2" onserverclick="btnDemand2_Click" style="width: 50px;" class="btn btn-info form-control" type="button"><i class="fa fa-print"></i></button>
                                                </span>
                                                <span class="input-group-btn">
                                                    <input type="date" runat="server" id="txtDemand2" class="form-control" />
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator2"
                                                ControlToValidate="txtDemand2"
                                                Text="Need Date for the Form Parameter."
                                                ValidationGroup="Demand2"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <label>LVL 3</label><span class="input-group-btn">
                                                    <button id="btnDemand3" runat="server" validationgroup="Demand3" onserverclick="btnDemand3_Click" style="width: 50px;" class="btn btn-info form-control" type="button"><i class="fa fa-print"></i></button>
                                                </span>
                                                <span class="input-group-btn">
                                                    <input type="date" runat="server" id="txtDemand3" class="form-control" />
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator3"
                                                ControlToValidate="txtDemand3"
                                                Text="Need Date for the Form Parameter."
                                                ValidationGroup="Demand3"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <label>LVL 4</label><span class="input-group-btn">
                                                    <button id="btnDemand4" runat="server" validationgroup="Demand4" onserverclick="btnDemand4_Click" style="width: 50px;" class="btn btn-info form-control" type="button"><i class="fa fa-print"></i></button>
                                                </span>
                                                <span class="input-group-btn">
                                                    <input type="date" runat="server" id="txtDemand4" class="form-control" />
                                                </span>
                                            </div>
                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator4"
                                                ControlToValidate="txtDemand4"
                                                Text="Need Date for the Form Parameter."
                                                ValidationGroup="Demand4"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                    <div class="row hidden">
                                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                            <div class="input-group">
                                                <label>LVL 5</label><span class="input-group-btn">
                                                    <button id="btnDemand5" validationgroup="Demand5" runat="server" onserverclick="btnDemand5_Click" style="width: 50px;" class="btn btn-info form-control" type="button"><i class="fa fa-print"></i></button>
                                                </span>
                                                <span class="input-group-btn">
                                                    <input type="date" runat="server" id="txtDemand5" class="form-control" />
                                                </span>
                                            </div>

                                            <asp:RequiredFieldValidator
                                                ID="RequiredFieldValidator5"
                                                ControlToValidate="txtDemand5"
                                                Text="Need Date for the Form Parameter."
                                                ValidationGroup="Demand5"
                                                runat="server" Style="color: red" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%--</div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalCheck" class="modal" style="overflow-y: scroll;" role="dialog" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Check Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddCheck">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnListPDC" CssClass="btn btn-info" Text="List of PDCs" Width="100%" OnClick="btnListPDC_Click" />
                                    </div>
                                </div>
                                <div class="row hidden">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnCheckPDC" CssClass="btn btn-info" Text="Add PDC" Width="100%" OnClick="btnCheckPDC_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h4>Check Details</h4>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtCheckNo" onkeypress="return isNumberKey(event)" runat="server" id="txtCheckNo" maxlength="10" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check Date:</h5>
                                    </div>
                                    <div class="col-lg-7">
                                        <asp:TextBox runat="server" CssClass="form-control date form_datetime" ID="txtCheckDate" TextMode="Date" />
                                    </div>
                                    <div class="col-lg-1">
                                        <asp:LinkButton runat="server" ID="btnCheckDate" OnClick="btnCheckDate_Click" CssClass="btn btn-secondary btn-circle"><i class="fa fa-search"></i></asp:LinkButton>
                                    </div>
                                </div>
                                <div class="row" id="divARPDCNo" runat="server" visible="false">
                                    <div class="col-lg-4">
                                        <h5>AR PDC No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtARPDCNo" runat="server"
                                            id="txtARPDCNo" />
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="input-group">
                                                    <span class="input-group-addon">PHP</span>
                                                    <asp:TextBox runat="server" CssClass="form-control txtCheckAmount" ID="txtCheckAmount" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCheckAmount_TextChanged" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank hidden" runat="server" id="txtBankID" disabled />
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtCheckBank" disabled /><span class="input-group-btn">
                                                <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" ID="btnCheckBank" OnClick="btnCheckBank_Click" CssClass="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Branch:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBranch" runat="server" id="txtCheckBranch" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h4>Deposited Details</h4>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank hidden" runat="server" id="txtDepositBankID" disabled />
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtDepositBank" disabled /><span class="input-group-btn">
                                                <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" ID="btnCheckHouseBanks" OnClick="btnCheckHouseBanks_Click" CssClass="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Account:</h5>
                                    </div>
                                    <div class="col-lg-8">

                                        <input type="text" style="z-index: auto;" class="form-control txtBranch hidden" runat="server" id="txtCheckAccount" />
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtAccount" runat="server" id="txtAccount" disabled /><span class="input-group-btn">
                                                <button id="btnDepositAccount" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="btnDepositAccount_ServerClick"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Branch:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBranch" runat="server" id="txtDepositedBranch" disabled />
                                        <button id="branch" runat="server" style="width: 50px; z-index: auto; display: none;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="branch_ServerClick"><i class="fa fa-bars"></i></button>

                                    </div>
                                </div>
                                <div class="row" style="display: none;">
                                    <div class="col-lg-4">
                                        <h5>Bank Code:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBankCode" runat="server" id="txtBankCode" disabled /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddCheck" CssClass="btn btn-info" Text="Add Check" Width="100%" OnClick="btnAddCheck_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>


    <%--    <div id="modalAddPDC" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <asp:Panel runat="server" DefaultButton="btnAddPDC">
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <div class="modal-content">
                            <div class="modal-header">
                                <asp:LinkButton runat="server" ID="btnHideAddPDC" OnClick="btnHideAddPDC_Click" CssClass="close">&times;</asp:LinkButton>
                                <h4 class="modal-title">Add PDC</h4>
                            </div>
                            <div class="modal-body">

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h4>Check Details</h4>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtCheckNo" onkeypress="return isNumberKey(event)" runat="server" id="txtCheckNoPDC" maxlength="10" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check Date:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" CssClass="form-control date form_datetime" ID="txtCheckDatePDC" TextMode="Date" />
                                    </div>


                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Check Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <div class="input-group">
                                                    <span class="input-group-addon">PHP</span>
                                                    <asp:TextBox runat="server" CssClass="form-control txtCheckAmount" ID="txtCheckAmountPDC" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCheckAmount_TextChanged" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank hidden" runat="server" id="txtBankIdPDC" disabled />
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtCheckBankPDC" disabled /><span class="input-group-btn">
                                                <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" ID="btnCheckBankPDC" OnClick="btnCheckBank_Click" CssClass="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Branch:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBranch" runat="server" id="txtCheckBranchPDC" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h4>Deposited Details</h4>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank hidden" runat="server" id="txtDepositBankIdPDC" disabled />
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtDepositBankPDC" disabled /><span class="input-group-btn">
                                                <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" ID="btnCheckHouseBanksPDC" OnClick="btnCheckHouseBanks_Click" CssClass="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Account:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBranch hidden" runat="server" id="txtCheckAccountPDC" />
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtAccount" runat="server" id="txtAccountPDC" disabled /><span class="input-group-btn">
                                                <button id="btnDepositAccountPDC" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="btnDepositAccount_ServerClick"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Branch:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBranch" runat="server" id="txtDepositedBranchPDC" disabled />
                                        <button id="branchPDC" runat="server" style="width: 50px; z-index: auto; display: none;" class="btn btn-secondary btn-dropbox" type="button" onserverclick="branch_ServerClick"><i class="fa fa-bars"></i></button>

                                    </div>
                                </div>
                                <div class="row" style="display: none;">
                                    <div class="col-lg-4">
                                        <h5>Bank Code:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBankCode" runat="server" id="txtBankCodePDC" disabled /><span class="input-group-btn">
                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddPDC" CssClass="btn btn-info" Text="Add PDC" Width="100%" OnClick="btnAddPDC_Click" />
                                    </div>
                                </div>
                                <br />

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>--%>

    <div id="modalListPDC" class="modal fade" role="dialog">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header modal-header-primary">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Select PDC</h4>
                        </div>
                        <div>
                            <div class="modal-body">
                                <asp:GridView runat="server"
                                    ID="gvSelectPDC"
                                    CssClass="table table-responsive"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    AllowPaging="true"
                                    PageSize="10"
                                    OnRowCommand="gvSelectPDC_RowCommand"
                                    OnPageIndexChanging="gvSelectPDC_PageIndexChanging"
                                    EmptyDataText="No records found">
                                    <HeaderStyle BackColor="#66ccff" />
                                    <Columns>
                                        <asp:BoundField DataField="DueDate" HeaderText="Check Date" DataFormatString="{0:MM/dd/yyyy}" />
                                        <asp:BoundField DataField="CheckSum" ItemStyle-HorizontalAlign="right" DataFormatString="{0:#,##0.00}" HeaderText="Amount" />
                                        <asp:BoundField DataField="Bank" HeaderText="Bank" />
                                        <asp:TemplateField HeaderText="Print">
                                            <ItemTemplate>
                                                <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnPDCPrint" Text="Print" runat="server" OnClick="btnPDCPrint_Click" OnClientClick="SetTarget();" CommandArgument='<%# Container.DataItemIndex %>'>
                                                    Print <i class="fa fa-print"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
                                        <asp:BoundField DataField="PostingDate" HeaderText="Date Received" DataFormatString="{0:MM/dd/yyyy}" />
                                        <%--5--%>
                                        <asp:BoundField DataField="ARPDCNo" HeaderText="AR PDC No." />
                                        <asp:BoundField DataField="CheckNum" HeaderText="Check No." />
                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnSelectPDC" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectPDC_Click" CommandArgument='<%# Bind("Id")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:UpdatePanel runat="server">
                                                    <ContentTemplate>
                                                        <asp:CheckBox runat="server" AutoPostBack="true" ID="chkSelPDC" CommandName="chk" CausesValidation="false" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <asp:BoundField DataField="CheckSumOrig" HeaderText="Id" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                        <%--10--%>
                                    </Columns>
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>

                                <hr />

                                <div class="text-right">
                                    <asp:LinkButton CssClass="btn btn-danger btn-sm" ID="btnDeletePDC" Text="Delete" runat="server" OnClick="btnDeletePDC_Click" OnClientClick="SetTarget();">
                                        Delete <i class="fa fa-print"></i>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="modalConfirmationPDC" class="modal" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:Panel runat="server" DefaultButton="btnConfimPDC">
                                <div class="row" style="text-align: center;">
                                    <div class="col-lg-12">
                                        <asp:Image ImageUrl="~/assets/img/info.png" runat="server" ID="Image1" Width="90" Height="90" />
                                    </div>
                                </div>
                                <div class="row" style="text-align: center">
                                    <div class="col-lg-12">
                                        <h4>
                                            <asp:Label runat="server" ID="lblConfirmPDC"></asp:Label>
                                        </h4>
                                    </div>
                                </div>
                                <div class="row" style="text-align: center">
                                    <div class="col-lg-12">
                                        <asp:Button runat="server" ID="btnConfimPDC" CssClass="btn btn-info btn-sm autofocus" Text="Confirm" OnClick="btnConfirmPDC_Click" TabIndex="0" Style="width: 90px;" />
                                        <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirmPDC()">Cancel</button>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>


    <div id="modalBank" class="modal" data-focus-on="input:first">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header modal-header-primary">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Bank</h4>
                        </div>
                        <div class="modal-body">
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">

                                    <asp:Panel runat="server" DefaultButton="btnCheckBankSearch">
                                        <div class="input-group">
                                            <asp:TextBox runat="server" ID="txtCheckBankSearch1" TabIndex="0" placeholder="Search..." class="form-control autofocus" type="text"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnCheckBankSearch" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnCheckBankSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                                    <asp:GridView runat="server"
                                        ID="gvBanks"
                                        CssClass="table table-responsive"
                                        AutoGenerateColumns="false"
                                        GridLines="None"
                                        AllowPaging="true"
                                        PageSize="10"
                                        OnRowCommand="gvBanks_RowCommand"
                                        OnPageIndexChanging="gvBanks_PageIndexChanging"
                                        EmptyDataText="No records found">
                                        <HeaderStyle BackColor="#66ccff" />
                                        <Columns>
                                            <asp:BoundField DataField="BankCode" HeaderText="Code" />
                                            <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                            <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="btnSelectBank" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectBank_Click" CommandArgument='<%# Bind("BankCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="modalDepositAccounts" class="modal fade" role="dialog">
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header modal-header-primary">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Bank Accounts</h4>
                        </div>
                        <div class="modal-body">
                            <asp:GridView runat="server"
                                ID="gvDepositAccounts"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                AllowPaging="true"
                                PageSize="10"
                                OnPageIndexChanging="gvDepositAccounts_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Account" HeaderText="Account" />
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnSelectAccount" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectAccount_Click" CommandArgument='<%# Bind("Account")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <div id="modalBranch" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Branch</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvBranch"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="false"
                                AllowPaging="true"
                                PageSize="10"
                                OnRowCommand="gvBranch_RowCommand"
                                OnPageIndexChanging="gvBranch_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Account" HeaderText="Account" />
                                    <asp:BoundField DataField="CheckAccount" HeaderText="CheckAccount" ItemStyle-CssClass="hidden" HeaderStyle-CssClass="hidden" />
                                    <asp:BoundField DataField="Branch" HeaderText="Bank" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCredit" class="modal" role="dialog" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Credit Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddCredit">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtCreditCardCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Card:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditCard" disabled />
                                            <span class="input-group-btn">
                                                <button runat="server" id="btnShowCredit" style="width: 50px; z-index: auto;" onserverclick="btnShowCredit_ServerClick" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Card No.:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditCardNum" maxlength="4" placeholder="Enter last 4 digit only" /><span class="input-group-btn">
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Expiry Date: (MM/YY)</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <asp:TextBox runat="server" CssClass="form-control " ID="txtValidUntil" placeholder="MM/YY" MaxLength="5" />
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>No. of Payments:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="number" style="z-index: auto;" class="form-control" runat="server" id="txtNoOfPayments" value="1" maxlength="2" /><span class="input-group-btn">
                                    </div>
                                </div>

                                <div class="row hidden">
                                    <div class="col-lg-4">
                                        <h5>Voucher Number:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" maxlength="20" id="txtVoucherNum" /><span class="input-group-btn">
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Approval Code Number:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" maxlength="20" id="txtApprovalNum" /><span class="input-group-btn">
                                    </div>
                                </div>



                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtCardBrandAccountCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Card Brand Account:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCardBrandAccountName" disabled /><span class="input-group-btn">
                                                <button style="width: 50px; z-index: auto;" onclick="showCardBrandAccount();" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>




                                <input type="text" style="z-index: auto; display: none;" class="form-control" runat="server" id="txtCreditAccountCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Account:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditAccount" disabled />
                                        <%--<span class="input-group-btn">
                                            <button runat="server" id="btnAccounts" style="width: 50px; z-index: auto;" onserverclick="btnAccounts_ServerClick" class="btn btn-secondary btn-dropbox"><i class="fa fa-bars"></i></button>
                                        </span>--%>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Credit Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-addon">PHP</span>
                                            <asp:TextBox runat="server" CssClass="form-control" AutoPostBack="true" ID="txtCreditAmount" MaxLength="10" TextMode="Number" Style="text-align: right;" OnTextChanged="txtCheckAmount_TextChanged" />
                                        </div>
                                    </div>
                                </div>

                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="txtCreditMethodCode" disabled />
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Payment Method:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtCreditMethod" disabled /><span class="input-group-btn">
                                                <button style="width: 50px; z-index: auto;" onclick="showPymtMethod();" class="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>


                                <div class="row" style="display: none;">
                                    <div class="col-lg-4">
                                        <h5>ID Number:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" maxlength="15" runat="server" id="txtIDNum" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row" style="display: none;">
                                    <div class="col-lg-4">
                                        <h5>Tel. No:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" maxlength="20" runat="server" id="txtTelNo" /><span class="input-group-btn">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddCredit" CssClass="btn btn-info" Text="Add Credit" Width="100%" OnClick="btnAddCredit_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAccounts" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">G/L Accounts</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                                    <h5>Search:</h5>
                                </div>
                                <div class="col-lg-10 col-md-10 col-sm-6 col-xs-12">
                                    <asp:Panel runat="server" DefaultButton="btnSearchAccounts">
                                        <div class="input-group">
                                            <input runat="server" id="txtSearchAccounts" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                                <asp:LinkButton runat="server" ID="btnSearchAccounts" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchAccounts_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                            <br />
                            <asp:GridView runat="server"
                                ID="gvAccounts"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="true"
                                PageSize="5"
                                OnRowCommand="gvAccounts_RowCommand"
                                OnPageIndexChanging="gvAccounts_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="AcctCode" HeaderText="Code" />
                                    <asp:BoundField DataField="AcctName" HeaderText="Name" />
                                    <%--<asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />--%>
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnSelectCreditGLAccounts" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectCreditGLAccounts_Click" CommandArgument='<%# Bind("AcctCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCreditCard" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Credit Cards</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvCreditCard"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="false"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CreditCard" HeaderText="Code" />
                                    <asp:BoundField DataField="CardName" HeaderText="Name" />
                                    <asp:BoundField DataField="CompanyID" HeaderText="Company" ItemStyle-CssClass="hide" HeaderStyle-CssClass="hide" />
                                    <asp:BoundField DataField="AcctCode" HeaderText="Account Code" />
                                    <asp:BoundField DataField="AcctName" HeaderText="Account Name" />
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnSelectCreditCard" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectCreditCard_Click" CommandArgument='<%# Bind("CreditCard")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalCreditPaymentMethod" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Credit Card Payment Method</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvPymtMethod"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="false"
                                OnRowCommand="gvPymtMethod_RowCommand"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="CrTypeCode" HeaderText="Code" />
                                    <asp:BoundField DataField="CrTypeName" HeaderText="Name" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



    <div id="modalCardBrandAccount" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Card Brand Account</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvCardBrandAccount"
                                CssClass="table table-hover table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="true"
                                AllowPaging="false"
                                OnRowCommand="gvCardBrandAccount_RowCommand"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Code" HeaderText="Code" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:ButtonField ButtonType="Link" ControlStyle-CssClass="btn btn-default btn-success fa fa-arrow-right" ItemStyle-Width="50"
                                        ItemStyle-HorizontalAlign="Center" CommandName="Sel" />
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalConfirmation" class="modal" data-focus-on="input:first">
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
                                        <asp:Label runat="server" ID="lblConfirmationInfo"></asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-12">
                                    <asp:Button runat="server" ID="btnYes" CssClass="btn btn-info btn-sm" Text="Yes" TabIndex="0" AutoFocus="true" OnClick="btnYes_Click" Style="width: 90px;" />
                                    <button type="button" class="btn btn-default btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideConfirm()">No</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <div id="modalAlert" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" ID="alertIcon" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="lblMessageAlert"></asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <button type="button" class="btn btn-primary btn-sm" data-dismiss="modal" style="width: 90px;" onclick="hideAlert()">Ok</button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <%--    Hide/Show automatically --%>
    <div id="modalAlertSuccess" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <asp:Image ImageUrl="~/assets/img/success.png" runat="server" Width="90" Height="90" />
                                </div>
                            </div>
                            <div class="row" style="text-align: center;">
                                <div class="col-lg-12">
                                    <h4>
                                        <asp:Label runat="server" ID="txtAlert"></asp:Label>
                                    </h4>
                                </div>
                            </div>
                            <br />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <div id="modalInterBranch" class="modal" role="dialog" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter Interbranch Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddInter">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="Text3" disabled />

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Deposit Date:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="date" placeholder="MM/DD/YYYY" style="z-index: auto;" class="form-control" runat="server" id="txtInterBranchDate" />
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Bank:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtBank hidden" runat="server" id="txtInterBankGLAcc" />
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtBank" runat="server" id="txtInterBankBank" disabled /><span class="input-group-btn">
                                                <%-- <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" OnClick="btnInterBank_Click" CssClass="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></asp:LinkButton>--%>
                                                <button id="btnInterBank" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="btnInterBank_Click" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Account No:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtInterAccounts" disabled />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-addon">PHP</span>
                                            <asp:TextBox runat="server" CssClass="form-control" ID="txtInterAmount" TextMode="Number" MaxLength="50" Style="text-align: right;" OnTextChanged="txtInterAmount_TextChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddInter" CssClass="btn btn-info" Text="Add InterBranch" Width="100%" OnClick="btnAddInter_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>




    <div id="modalOthers" class="modal" style="overflow-y: scroll;" role="dialog" data-focus-on="input:first">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Other Payment Mean Details</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" DefaultButton="btnAddCredit">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <input type="text" style="z-index: auto;" class="form-control hide" runat="server" id="Text1" disabled />

                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Payment Date:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="date" placeholder="MM/DD/YYYY" style="z-index: auto;" class="form-control" runat="server" id="txtOthersPaymentDate" />
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Mode of Payment:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control txtOthersModeOfPaymentCode hidden" runat="server" id="txtOthersModeOfPaymentCode" />
                                        <div class="input-group">
                                            <input type="text" style="z-index: auto;" class="form-control txtOthersModeOfPayment" runat="server" id="txtOthersModeOfPayment" disabled />
                                            <span class="input-group-btn">
                                                <%-- <asp:LinkButton Style="width: 50px; z-index: auto;" runat="server" OnClick="btnInterBank_Click" CssClass="btn btn-secondary btn-dropbox" type="button"><i class="fa fa-bars"></i></asp:LinkButton>--%>
                                                <button id="btnOthersModeOfPayment" runat="server" style="width: 50px; z-index: auto;" class="btn btn-secondary" onserverclick="btnOthersModeOfPayment_ServerClick" type="button"><i class="fa fa-bars"></i></button>
                                            </span>
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Amount:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <span class="input-group-addon">PHP</span>
                                            <asp:TextBox runat="server" CssClass="form-control" AutoPostBack="true" ID="txtOthersAmount" MaxLength="10" TextMode="Number" Style="text-align: right;" />
                                        </div>
                                    </div>
                                </div>



                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Reference No:</h5>
                                    </div>
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtOthersReferenceNo" />
                                    </div>
                                </div>




                                <div class="row">
                                    <div class="col-lg-4">
                                        <h5>Account:</h5>
                                    </div>
                                    <input type="text" style="z-index: auto;" class="form-control txtOthersGLAccountCode hidden" runat="server" id="txtOthersGLAccountCode" />
                                    <div class="col-lg-8">
                                        <input type="text" style="z-index: auto;" class="form-control" runat="server" id="txtOthersGLAccountName" disabled />
                                    </div>
                                </div>





                                <div class="row">
                                    <div class="col-lg-8 col-lg-offset-4">
                                        <asp:Button runat="server" ID="btnAddOthers" CssClass="btn btn-info" Text="Add Payment" Width="100%" OnClick="btnAddOthers_Click" />
                                    </div>
                                </div>



                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>




    <div id="modalInterBank" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Bank</h4>
                </div>
                <div class="modal-body">

                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <asp:Panel runat="server" DefaultButton="btnSearchInter">
                                    <div class="input-group">
                                        <input runat="server" id="txtSearchInter" placeholder="Search..." class="form-control" type="text" autofocus /><span class="input-group-btn">
                                            <asp:LinkButton runat="server" ID="btnSearchInter" Style="width: 50px;" CssClass="btn btn-secondary btn-default btn-facebook" OnClick="btnSearchInter_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                        </span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="row">
                                <asp:GridView runat="server"
                                    ID="gvInterBanks"
                                    CssClass="table table-responsive"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    ShowHeader="false"
                                    AllowPaging="true"
                                    PageSize="10"
                                    OnRowCommand="gvInterBanks_RowCommand"
                                    OnPageIndexChanging="gvInterBanks_PageIndexChanging"
                                    EmptyDataText="No records found">
                                    <HeaderStyle BackColor="#66ccff" />
                                    <Columns>
                                        <asp:BoundField DataField="BankCode" HeaderText="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                        <asp:BoundField DataField="BankName" HeaderText="Bank" />
                                        <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="btnSelectInterBanks" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectInterBanks_Click" CommandArgument='<%# Bind("BankCode")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                    <PagerStyle CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>




    <div id="modalOthersPaymentMean" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header modal-header-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Other Payment Means </h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server"
                                ID="gvOthersPaymentMean"
                                CssClass="table table-responsive"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                ShowHeader="false"
                                AllowPaging="true"
                                PageSize="10"
                                OnPageIndexChanging="gvOthersPaymentMean_PageIndexChanging"
                                EmptyDataText="No records found">
                                <HeaderStyle BackColor="#66ccff" />
                                <Columns>
                                    <asp:BoundField DataField="Code" HeaderText="Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="U_GLAccountCode" HeaderText="GL Account Code" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:BoundField DataField="U_GLAccountName" HeaderText="GL Account Name" ItemStyle-CssClass="Hide" HeaderStyle-CssClass="Hide" />
                                    <asp:TemplateField HeaderStyle-Width="10px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="btnSelectOthersPaymentMean" CssClass="btn btn-default btn-success" Width="100%" Height="100%" OnClick="btnSelectOthersPaymentMean_Click" CommandArgument='<%# Bind("Code")%>'><i class="fa fa-hand-o-up"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="White" ForeColor="Black" />
                                <PagerStyle CssClass="pagination-ys" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>



</asp:Content>
