using System;
using SAPbobsCOM;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using MSXML2;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using MDC_REALITY;
using ABROWN_DREAMS.wcf;
using DirecLayer;
using System.Text;
using ABROWN_DREAMS.Services;
using ABROWN_DREAMS;

namespace ABROWN_DREAMS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DirecService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DirecService.svc or DirecService.svc.cs at the Solution Explorer and start debugging.
    public class DirecService : IDirecService
    {
        public static BusinessPartners oBP;
        public static int lRetCode, lErrCode;
        public static string sErrMsg;
        public static Company oCompany;
        public static SAPbobsCOM.Documents oQuotation, oOrder, oInvoice;
        public static SAPbobsCOM.JournalEntries oJE;
        public static Payments oPayments;

        DirecWebService ws = new DirecWebService();
        SAPHanaAccess hana = new SAPHanaAccess();
        public string ErrMsg { get; set; }

        public static XMLHTTP60 ServiceLayer { get; set; }

        #region ServiceLayer

        public bool ConnectWithServiceLayer()
        {
            var output = false;
            try
            {
                var model = new List<dynamic>();
                model.Add(new
                {
                    CompanyDB = ConfigurationManager.AppSettings["Database"].ToString(),
                    UserName = ConfigurationManager.AppSettings["LicenseID"].ToString(),
                    Password = ConfigurationManager.AppSettings["LicensePassword"].ToString()
                });

                string url = ServiceURL_Update(ConfigurationManager.AppSettings["ServiceLayerURL"].ToString());
                ServiceLayer.open("POST", $@"{url}/Login");

                string json = new JavaScriptSerializer().Serialize(model).ToString();
                if (json.Length > 2)
                {
                    json = json.Substring(1, json.Length - 2);
                }

                ServiceLayer.send(json);
                ErrMsg = JsonHelper.GetJsonValue(ServiceLayer.responseText, "SessionId");

                output = IsLoginSuccess(ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = $"Error : Service Layer Access Return Login {ex.Message}";
                throw new Exception(ErrMsg);
            }
            return output;
        }


        public static string ServiceURL_Update(string url)
        {
            var output = string.Empty;
            try
            {
                const string httpStr = "http://";
                const string httpsStr = "https://";

                if (!url.StartsWith(httpStr, true, null) &&
                        !url.StartsWith(httpsStr, true, null))
                {
                    output = $"{httpStr}{url}/b1s/v1/";
                }
                else
                {
                    output = $"{url}/b1s/v1/";
                }

                if (ServiceLayer == null)
                { ServiceLayer = new XMLHTTP60(); }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Service Layer Access Return Service URL {ex.Message}"); }
            return output;
        }

        bool IsLoginSuccess(string sResponse)
        {
            var output = false;
            try
            {
                if (string.IsNullOrEmpty(sResponse))
                { output = false; }
                else
                {
                    //Logout();
                    output = sResponse.Contains("-");
                }
            }
            catch (Exception ex)
            { throw new Exception($"Error : Service Layer Access Return IsLoginSuccess {ex.Message}"); }

            return output;
        }



        #endregion

        #region STANDARD

        //DI API
        bool ConnectToSAPDI(out Company company, out string ErrMsg)
        {
            try
            {
                string ServerType = ConfigurationManager.AppSettings["ServerType"].ToString();
                string LicenseServer = ConfigurationManager.AppSettings["LicenseServer"].ToString();
                string LicensePort = ConfigurationManager.AppSettings["LicenseServerPort"].ToString();
                string SAPServer = DataAccess.GetconDetails("SAP", "Data Source");
                string SAPServerPort = ConfigurationManager.AppSettings["SAPServerPort"].ToString();
                string LicenseID = ConfigurationManager.AppSettings["LicenseID"].ToString();
                string LicensePassword = ConfigurationManager.AppSettings["LicensePassword"].ToString();
                string SapDatabase = DataAccess.GetconDetails("SAP", "Initial Catalog");
                string DBUsername = DataAccess.GetconDetails("SAP", "User ID");
                string DBPassword = DataAccess.GetconDetails("SAP", "Password");

                string licenseWithPort = LicenseServer + ":" + LicensePort;
                //CONNECTING TO SAP DATABASE
                oCompany = new Company
                {
                    LicenseServer = licenseWithPort,
                    Server = SAPServer,
                    language = BoSuppLangs.ln_English,
                    UseTrusted = false,
                    DbUserName = DBUsername,
                    DbPassword = DBPassword,
                    DbServerType = BoDataServerTypes.dst_MSSQL2014,
                    CompanyDB = SapDatabase,
                    UserName = LicenseID,
                    Password = LicensePassword
                };

                if (oCompany.Connected == false)
                {
                    lRetCode = oCompany.Connect();
                }
                else { lRetCode = 0; }

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    company = null;
                    return false;
                }
                else
                {
                    ErrMsg = "Connected to SAP";
                    company = oCompany;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                company = null;
                return false;
            }
        }

        bool AddToSAP(SAPbobsCOM.Documents oDoc, out string ErrMsg)
        {
            lRetCode = oDoc.Add();

            if (lRetCode != 0)
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                ErrMsg = lErrCode + "-" + sErrMsg;
                return false;
            }
            else
            {
                ErrMsg = null;
                return true;
            }
        }
        #endregion

        #region RESERVATION
        bool CreateSalesQuotation(string CardCode, string ProjCode,  //2
                                  string Model, string Block,  //4
                                  string Lot, string Feat,  //6
                                  string PriceCat, string DAS,  //8
                                  string Discount, string AddtlCharge, //10
                                  string VAT, string RsvDate,  //12
                                  string ItemCode, string ItemCodeOC,  //14
                                  string threshold, string misc, //16
                                  string netDas, string promoDisc,
                                  out int DocEntry, out string ErrMsg) //18
        {
            try
            {
                // ** quotation header **//
                string module = $"Quotations";
                IDictionary<string, object> Quotations = new Dictionary<string, object>()
                {
                    {"CardCode", CardCode},
                    {"DocDate", Convert.ToDateTime(RsvDate).ToString("yyyy-MM-dd")},
                    {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                    {"U_Project", ProjCode},
                    {"U_qsBlock", Block},
                    {"U_qsLotNo", Lot},
                    {"U_qsModel", Model},
                    {"U_qsHouseFeat", Feat},
                    {"U_qsPriceCat", PriceCat}
                };

                string vatGrp = "";
                if (Convert.ToDouble(netDas) <= Convert.ToDouble(threshold))
                    vatGrp = "OTE";
                else
                    vatGrp = "OT1";

                //** freight **//
                IList<IDictionary<string, object>> DocumentAdditionalExpenses = new List<IDictionary<string, object>>();
                var ExpensesLines = new Dictionary<string, object>();
                if ((double.Parse(Discount) + double.Parse(promoDisc)) > 0)
                {
                    ExpensesLines.Add("ExpenseCode", 1);
                    ExpensesLines.Add("LineTotal", -double.Parse(Discount) + -double.Parse(promoDisc));
                    ExpensesLines.Add("VatGroup", vatGrp);
                }

                if (double.Parse(AddtlCharge) > 0)
                {
                    ExpensesLines.Add("ExpenseCode", 2);
                    ExpensesLines.Add("LineTotal", -double.Parse(AddtlCharge));
                    ExpensesLines.Add("VatGroup", "OT1");
                }
                DocumentAdditionalExpenses.Add(ExpensesLines);

                //** adding of line Items **//
                IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
                var Lines = new Dictionary<string, object>();

                Lines.Add("ItemCode", ItemCode);
                Lines.Add("Quantity", 1);
                Lines.Add("ProjectCode", ProjCode);
                Lines.Add("UnitPrice", double.Parse(DAS));

                //if (!string.IsNullOrEmpty(VAT) || double.Parse(VAT) != 0) 
                Lines.Add("VatGroup", vatGrp);

                ////** freight discount **//
                //oQuotation.Lines.Expenses.ExpenseCode = 1;
                //oQuotation.Lines.Expenses.LineTotal = double.Parse(Discount);
                //oQuotation.Lines.Expenses.VatGroup = "OTE";
                //oQuotation.Lines.Expenses.Add();

                DocumentLines.Add(Lines);

                //Other Charges aka Miscellaneous Charges
                DataTable dtVal;
                dtVal = hana.GetData($@"SELECT TOP 1 ""ItemCode"" FROM OITM WHERE ""U_Project"" = '{ProjCode}' AND ""ItmsGrpCod"" = {ConfigSettings.TitleInventoryItemGroup}", hana.GetConnection("SAPHana"));
                string miscItem = (string)DataAccess.GetData(dtVal, 0, "ItemCode", "");

                if (dtVal.Rows.Count != 0)
                {
                    Lines = new Dictionary<string, object>();
                    Lines.Add("ItemCode", miscItem);
                    Lines.Add("Quantity", 1);
                    Lines.Add("UnitPrice", double.Parse(misc));
                    Lines.Add("VatGroup", "OTNA");
                    DocumentLines.Add(Lines);
                }

                // ** for exclusive only ** //
                //if (!string.IsNullOrEmpty(ItemCodeOC))
                //{
                //    //** adding of line Items **//
                //    oQuotation.Lines.ItemCode = ItemCodeOC;
                //    oQuotation.Lines.Quantity = 1;
                //    oQuotation.Lines.VatGroup = "OTE";
                //    oQuotation.Lines.Add();
                //}

                //############################## [POSTING HERE] ##############################
                SapHanaLayer company = new SapHanaLayer();
                bool result = company.Connect();

                StringBuilder DocumentAdditionalExpensesLines = DataHelper.JsonLinesBuilder("DocumentAdditionalExpenses", DocumentAdditionalExpenses);
                StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                StringBuilder strLines = DataHelper.JsonLinesCombine(DocumentAdditionalExpensesLines, DocumentLine);
                var json = DataHelper.JsonBuilder(Quotations, strLines);

                if (DocumentAdditionalExpenses.Count > 0 || DocumentLines.Count > 0)
                {
                    if (company.POST(module, json))
                    {
                        ErrMsg = $"{module} successfully created.";
                        DocEntry = int.Parse(company.ResultDescription);
                        result = true;
                    }
                    else
                    {
                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                        DocEntry = 0;
                        result = false;
                    }
                }
                else
                {
                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                    DocEntry = 0;
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }
        bool CreateSalesQuotationRestructure(string CardCode, string ProjCode, //2
                                             string Model, string Block,  //4
                                             string Lot, string Feat,  //6
                                             string PriceCat, string DAS,  //8
                                             string Discount, string VAT,  //10
                                             string ItemCode, string ItemCodeOC, //12 
                                             out int DocEntry, out string ErrMsg) //14
        {
            try
            {
                int Entry = 0;
                // ** quotation header **//
                oQuotation = oCompany.GetBusinessObject(BoObjectTypes.oQuotations);

                oQuotation.CardCode = CardCode;
                oQuotation.DocDate = DateTime.Now;
                oQuotation.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";
                oQuotation.UserFields.Fields.Item("U_Project").Value = ProjCode;
                oQuotation.UserFields.Fields.Item("U_qsBlock").Value = Block;
                oQuotation.UserFields.Fields.Item("U_qsLotNo").Value = Lot;
                oQuotation.UserFields.Fields.Item("U_qsModel").Value = Model;
                oQuotation.UserFields.Fields.Item("U_qsHouseFeat").Value = Feat;
                oQuotation.UserFields.Fields.Item("U_qsPriceCat").Value = PriceCat;

                //** adding of line Items **//
                oQuotation.Lines.ItemCode = ItemCode;
                oQuotation.Lines.Quantity = 1;
                oQuotation.Lines.UnitPrice = double.Parse(DAS);

                if (!string.IsNullOrEmpty(VAT) || double.Parse(VAT) != 0)
                    oQuotation.Lines.VatGroup = "OT1";
                else
                    oQuotation.Lines.VatGroup = "OTE";

                //** freight discount **//
                oQuotation.Lines.Expenses.ExpenseCode = 1;
                oQuotation.Lines.Expenses.LineTotal = double.Parse(Discount);
                oQuotation.Lines.Expenses.VatGroup = "OTE";
                oQuotation.Lines.Expenses.Add();

                oQuotation.Lines.Add();

                // ** for exclusive only ** //
                if (!string.IsNullOrEmpty(ItemCodeOC))
                {
                    //** adding of line Items **//
                    oQuotation.Lines.ItemCode = ItemCodeOC;
                    oQuotation.Lines.Quantity = 1;
                    oQuotation.Lines.VatGroup = "OTE";
                    oQuotation.Lines.Add();
                }

                lRetCode = oQuotation.Add();

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    DocEntry = 0;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    Entry = int.Parse(oCompany.GetNewObjectKey());
                    DocEntry = Entry;
                    return true;
                }

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }
        bool CreateDownPaymentRequest(string CardCode, int SQEntry, double payment, string RsvDate, out int DocEntry, out string ErrMsg)
        {
            try
            {
                // ** quotation header **//
                string module = $"DownPayments";
                IDictionary<string, object> DownPayments = new Dictionary<string, object>()
                {
                    {"DownPaymentType", "dptRequest"},
                    {"CardCode", CardCode},
                    {"DocDate", Convert.ToDateTime(RsvDate).ToString("yyyy-MM-dd")},
                    {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                    {"DocTotal", payment}
                };

                IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

                DataTable dtItems = hana.GetData($@"SELECT A.""LineNum"",A.""ItemCode"" FROM QUT1 A Where A.""DocEntry"" = '{SQEntry}'", hana.GetConnection("SAPHana"));
                if (DataAccess.Exist(dtItems))
                {
                    foreach (DataRow dr in dtItems.Rows)
                    {
                        var Lines = new Dictionary<string, object>();

                        string itemCode = string.Format("{0}", dr[1]);
                        //** set base entry **//
                        Lines.Add("BaseEntry", SQEntry);
                        Lines.Add("BaseLine", int.Parse(dr[0].ToString()));
                        Lines.Add("BaseType", 23);
                        //** adding of line Items **//
                        //oInvoice.Lines.ItemCode = itemCode;
                        Lines.Add("Quantity", 1);

                        DocumentLines.Add(Lines);
                    }
                }

                //############################## [POSTING HERE] ##############################
                SapHanaLayer company = new SapHanaLayer();
                bool result = company.Connect();

                StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                var json = DataHelper.JsonBuilder(DownPayments, DocumentLine);


                if (DocumentLines.Count > 0)
                {
                    if (company.POST(module, json))
                    {
                        ErrMsg = $"{module} successfully created.";
                        DocEntry = int.Parse(company.ResultDescription);
                        result = true;
                    }
                    else
                    {
                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                        DocEntry = 0;
                        result = false;
                    }
                }
                else
                {
                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                    DocEntry = 0;
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }
        bool CreateDownPaymentRequestRestructure(string CardCode, int SQEntry, int SOEntry, double TotalSum, out int DocEntry, out string ErrMsg)
        {
            try
            {
                int Entry = 0;
                // ** quotation header **//
                oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oDownPayments);
                oInvoice.DownPaymentType = DownPaymentTypeEnum.dptRequest;

                var oBaseDoc = oOrder;

                oInvoice.CardCode = CardCode;
                oInvoice.DocDate = DateTime.Now;
                oInvoice.DocTotal = TotalSum;
                oInvoice.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";

                //DataTable dtItems = DataAccess.Select("SAP", $"SELECT A.LineNum,A.ItemCode FROM QUT1 A (NOLOCK) INNER JOIN OITT B ON A.ItemCode = B.Code Where A.DocEntry = '{SQEntry}'");

                var oBaseLines = oBaseDoc.Lines;

                if (oBaseLines.Count > 0)
                {
                    for (int i = 0; i < oBaseLines.Count; i++)
                    {
                        //** set base entry **//
                        oInvoice.Lines.BaseEntry = SOEntry;
                        oInvoice.Lines.BaseLine = oBaseLines.LineNum;
                        oInvoice.Lines.BaseType = int.Parse(oBaseDoc.DocObjectCodeEx);
                        //** adding of line Items **//
                        oInvoice.Lines.ItemCode = oBaseLines.ItemCode;
                        oInvoice.Lines.Quantity = oBaseLines.Quantity;
                        oInvoice.Lines.Add();
                    }

                }

                //if (dtItems.Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dtItems.Rows)
                //    {
                //        string itemCode = string.Format("{0}", dr[1]);
                //        //** set base entry **//
                //        oInvoice.Lines.BaseEntry = SOEntry;
                //        oInvoice.Lines.BaseLine = int.Parse(dr[0].ToString());
                //        oInvoice.Lines.BaseType = 17;
                //        //** adding of line Items **//
                //        oInvoice.Lines.ItemCode = itemCode;
                //        oInvoice.Lines.Quantity = 1;
                //        oInvoice.Lines.Add();
                //    }
                //}



                lRetCode = oInvoice.Add();

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    DocEntry = 0;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    Entry = int.Parse(oCompany.GetNewObjectKey());
                    DocEntry = Entry;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }
        bool CreateDownPayment(int aDocEntry, string CardCode, string RsvDate, double TotalCash, int DPEntry, string ORNumber, string Comments, DataTable dtPayments
             , out int DocEntry, out string ErrMsg)
        {
            try
            {
                int Entry = 0;
                // ** quotation header **//
                string module = $"IncomingPayments";


                //** select existing partial payments **//
                DataTable dtPartialPayments = new DataTable();
                dtPartialPayments = ws.GetPartialPayments(aDocEntry).Tables[0];
                string _remarks = "";
                foreach (DataRow dr in dtPartialPayments.Rows)
                {
                    _remarks += dr["Comments"].ToString() + " ";
                }

                IDictionary<string, object> IncomingPayments = new Dictionary<string, object>()
                {
                    {"CardCode", CardCode},
                    {"DocDate", Convert.ToDateTime(RsvDate).ToString("yyyy-MM-dd")},
                    {"Remarks", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                    {"U_ORNo", ORNumber},
                    {"U_Remarks", _remarks + Comments},
                    {"CashAccount", ConfigSettings.CashAccount},
                    {"CashSum", TotalCash}
                };

                IList<IDictionary<string, object>> PaymentInvoices = new List<IDictionary<string, object>>();

                var PaymentInvoicesLines = new Dictionary<string, object>();

                PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
                PaymentInvoicesLines.Add("DocEntry", DPEntry);

                PaymentInvoices.Add(PaymentInvoicesLines);

                IList<IDictionary<string, object>> PaymentChecks = new List<IDictionary<string, object>>();
                IList<IDictionary<string, object>> PaymentCreditCards = new List<IDictionary<string, object>>();

                foreach (DataRow row in dtPayments.Rows)
                {
                    var lines = new Dictionary<string, object>();
                    //** checks **//
                    if (!string.IsNullOrWhiteSpace(row["CheckNo"].ToString()))
                    {
                        if (int.Parse(row["CheckNo"].ToString()) != 0)
                        {
                            lines.Add("DueDate", Convert.ToDateTime(row["DueDate"].ToString()).ToString("yyyy-MM-dd"));
                            lines.Add("CheckSum", double.Parse(row["Amount"].ToString()));
                            lines.Add("BankCode", row["BankCode"].ToString());
                            lines.Add("Branch", row["Branch"].ToString());
                            lines.Add("AccounttNum", row["AccountNum"].ToString());
                            lines.Add("CheckNumber", int.Parse(row["CheckNo"].ToString()));
                            lines.Add("CheckAccount", ConfigSettings.CheckAccount);

                            PaymentChecks.Add(lines);
                        }
                    }
                    //** credit **//
                    else if (row["Type"].ToString() == "Credit")
                    {
                        lines.Add("DueDate", Convert.ToDateTime(row["DueDate"].ToString()).ToString("yyyy-MM-dd"));

                        //** get code of creditcard
                        DataTable dtCredit = hana.GetData($@"SELECT ""CreditCard"" FROM OCRC Where ""CardName"" ='{row["CreditCard"].ToString()}'", hana.GetConnection("SAPHana"));

                        string creditcard = dtCredit.Rows[0][0].ToString();
                        lines.Add("CreditCard", int.Parse(creditcard));
                        lines.Add("CreditAcct", row["CreditAcctCode"].ToString());
                        lines.Add("CreditCardNumber", row["CreditCardNumber"].ToString());
                        lines.Add("CreditSum", double.Parse(row["Amount"].ToString()));
                        lines.Add("CardValidUntil", Convert.ToDateTime(row["ValidUntil"].ToString()).ToString("yyyy-MM-dd"));
                        lines.Add("PaymentMethodCode", int.Parse(row["PymtTypeCode"].ToString()));
                        lines.Add("NumOfPayments", int.Parse(row["NumOfPymts"].ToString()));
                        lines.Add("VoucherNum", row["VoucherNum"].ToString());
                        lines.Add("OwnerIdNum", row["IdNum"].ToString());
                        lines.Add("OwnerPhone", row["TelNum"].ToString());
                        lines.Add("CreditType", "cr_Regular");

                        PaymentCreditCards.Add(lines);
                    }
                }


                //############################## [POSTING HERE] ##############################
                SapHanaLayer company = new SapHanaLayer();
                bool result = company.Connect();

                StringBuilder PaymentInvoicesLine = DataHelper.JsonLinesBuilder("PaymentInvoices", PaymentInvoices);
                StringBuilder PaymentChecksLine = DataHelper.JsonLinesBuilder("PaymentChecks", PaymentChecks);
                StringBuilder PaymentCreditCardsLine = DataHelper.JsonLinesBuilder("PaymentCreditCards", PaymentCreditCards);
                StringBuilder strLines = DataHelper.JsonLinesCombine(PaymentInvoicesLine, PaymentChecksLine, PaymentCreditCardsLine);
                var json = DataHelper.JsonBuilder(IncomingPayments, strLines);

                if (PaymentInvoices.Count > 0 || PaymentChecks.Count > 0 || PaymentCreditCards.Count > 0)
                {
                    if (company.POST(module, json))
                    {
                        ErrMsg = $"{module} successfully created.";
                        DocEntry = int.Parse(company.ResultDescription);
                        result = true;
                    }
                    else
                    {
                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                        DocEntry = 0;
                        result = false;
                    }
                }
                else
                {
                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                    DocEntry = 0;
                    result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }
        bool CreateDownPaymentRestructure(string CardCode, double TotalPayments, double DPAmount, int DPEntry, int LBEntry, out int DocEntry, out string ErrMsg)
        {
            try
            {
                int Entry = 0;
                oPayments = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                oPayments.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                oPayments.Invoices.DocEntry = DPEntry;
                oPayments.Invoices.SumApplied = TotalPayments;
                oPayments.Invoices.Add();

                oPayments.CardCode = CardCode;
                oPayments.DocDate = DateTime.Now;
                oPayments.Remarks = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";

                if (LBEntry != 0)
                {
                    oPayments.Invoices.DocEntry = LBEntry;
                    oPayments.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                    oPayments.Invoices.SumApplied = DPAmount - TotalPayments;
                    oPayments.Invoices.Add();
                }

                oPayments.CashAccount = ConfigSettings.CashAccount;
                oPayments.CashSum = TotalPayments;

                lRetCode = oPayments.Add();

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    DocEntry = 0;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    Entry = int.Parse(oCompany.GetNewObjectKey());
                    DocEntry = Entry;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                DocEntry = 0;
                return false;
            }
        }

        public bool CreateReservation(int DocEntry, string projCode,  //2
                                      string Block, string Lot,  //4
                                      string LName, string FName,  //6
                                      string MName, string AddonCardCode, //8 
                                      string Model, string Feature,  //10
                                      string SalesType, string Size,  //12
                                      double ReservationPayment, double ReservationFee,  //14
                                      string LotArea, string Das,  //16
                                      string Vat, string Discount,  //18
                                      string AddtlCharges, string RsvDate,  //20 
                                      string ItemCode, string ItemCodeOC,   //22
                                      string ORNumber, string Comments,     //24 
                                      string threshold, string misc, //26
                                      string netDAS, string promoDisc, //28
                                      string TIN, //29
                                      GridView gvPayments, out int SapEntry     //31
                                    , out string SapCardCode, out string Message)   //33
        {
            try
            {
                bool isErr = false;
                string module = "";
                //** POSTING TO SAP **//
                SapHanaLayer company = new SapHanaLayer();
                bool result = company.Connect();
                Message = $"({company.ResultCode}) {company.ResultDescription}";
                if (result)
                {
                    //** SAP START TRANSACTION **//
                    string CardCode = null;
                    int SQEntry = 0, DPREntry = 0, PymtEntry = 0;

                    // ** create business partner **//
                    module = "BusinessPartners";
                    int series = int.Parse(ConfigSettings.BPSeries);


                    DataTable dt = hana.GetData($@"SELECT TOP 1 ""CardCode"",""CardName"",""CardFName"" FROM ""OCRD"" Where ""CardFName"" = '{AddonCardCode}'", hana.GetConnection("SAPHana"));

                    if (result = DataAccess.Exist(dt))
                    {
                        //return existing cardcode
                        ErrMsg = "Business Partner successfully created.";
                        CardCode = dt.Rows[0][0].ToString();
                    }
                    else
                    {
                        IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
                        {
                            { "Series" , series },
                            { "CardType" , "C" },
                            { "CardName" , $"{LName},{FName}" },
                            { "CardForeignName" , AddonCardCode },
                            { "PriceListNum" , int.Parse(ConfigSettings.BPPriceList) },
                            { "DownPaymentClearAct" , ConfigSettings.CashAccount },
                            { "DownPaymentInterimAccount" , ConfigSettings.CashAccount },
                            { "SubjectToWithholdingTax" , "N" },
                            { "DeferredTax" , "N" },
                            { "U_LName" , LName},
                            { "U_FName" , FName},
                            { "U_MName" , MName},
                            { "FederalTaxID" , TIN}
                        };

                        var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

                        if (company.POST(module, json))
                        {
                            ErrMsg = "Business Partner successfully created.";
                            CardCode = company.ResultDescription;
                            result = true;
                        }
                        else
                        {
                            ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                            Message = "BP Posting: " + ErrMsg;
                            CardCode = null;
                            result = false;
                        }
                    }

                    if (result)
                    {
                        DataTable dtPayments = ws.GetAllReservationPayments(DocEntry).Tables["GetAllReservationPayments"];

                        ////get paid sum cash
                        double totalCash = 0;
                        DataTable dtCash = hana.GetData($@"SELECT IFNULL(SUM(""CashSum""),0) ""Cash"" FROM ""QUT2"" WHERE ""DocEntry"" = {DocEntry}", hana.GetConnection("SAOHana"));

                        if (double.Parse(dtCash.Rows[0]["Cash"].ToString()) != 0)
                        {
                            totalCash = double.Parse(dtCash.Rows[0]["Cash"].ToString());
                        }

                        //** create sales quotation **//

                        if (CreateSalesQuotation(CardCode, projCode, Model, Block, Lot, Feature, SalesType, Das, Discount, AddtlCharges, Vat, RsvDate, ItemCode, ItemCodeOC, threshold, misc, netDAS, promoDisc, out SQEntry, out Message))
                        {
                            module = "Quotations";
                            //** create downpayment request **//
                            if (CreateDownPaymentRequest(CardCode, SQEntry, ReservationFee, RsvDate, out DPREntry, out Message))
                            {
                                module = "DownPayments";
                                //** create incoming payments **//
                                if (CreateDownPayment(DocEntry, CardCode, RsvDate, totalCash, DPREntry, ORNumber, Comments, dtPayments
                                    , out PymtEntry, out Message))
                                {
                                    module = "IncomingPayments";
                                    //**create opyt table amountpaid **//
                                    if (ws.CreateDownPayment(DocEntry, PymtEntry, ORNumber, Comments, ReservationPayment, 0, 0))
                                    {
                                        //** update reservation date **//
                                        hana.Execute($@"Update PRJ2 SET ""datersv"" = CURRENT_DATE WHERE ""PrjCode"" = '{projCode}' and ""Block"" = '{Block}' and ""Lot"" = '{Lot}'", hana.GetConnection("SAOHana"));
                                        hana.Execute($@"Update OCRD SET ""SAPCardCode"" = '{CardCode}' WHERE ""CardCode"" = '{AddonCardCode}'", hana.GetConnection("SAOHana"));

                                        SapEntry = SQEntry;
                                        SapCardCode = CardCode;
                                        return true;
                                    }
                                    else
                                    {
                                        StringBuilder model = new StringBuilder();

                                        //CANCEL INCOMING PAYMENT
                                        model.Append(company.GET($"{module}('{PymtEntry}')"));
                                        cancelSAPTransactions(model, company, module, PymtEntry);

                                        //CANCEL AR DOWNPAYMENT
                                        model = new StringBuilder();
                                        model.Append(company.GET($"{module}('{DPREntry}')"));
                                        cancelSAPTransactions(model, company, module, DPREntry);

                                        //CANCEL QUOTATION
                                        model = new StringBuilder();
                                        model.Append(company.GET($"{module}('{SQEntry}')"));
                                        cancelSAPTransactions(model, company, module, SQEntry);

                                        SapEntry = 0;
                                        SapCardCode = CardCode;
                                        return false;
                                    }
                                }
                                else
                                {
                                    StringBuilder model = new StringBuilder();

                                    //CANCEL AR DOWNPAYMENT
                                    model.Append(company.GET($"{module}('{DPREntry}')"));
                                    cancelSAPTransactions(model, company, module, DPREntry);

                                    //CANCEL QUOTATION
                                    model = new StringBuilder();
                                    model.Append(company.GET($"{module}('{SQEntry}')"));
                                    cancelSAPTransactions(model, company, module, SQEntry);


                                    SapEntry = 0;
                                    SapCardCode = CardCode;
                                    return false;
                                }
                            }
                            else
                            {
                                //CANCEL QUOTATION
                                StringBuilder model = new StringBuilder();
                                model.Append(company.GET($"{module}('{SQEntry}')"));
                                cancelSAPTransactions(model, company, module, SQEntry);

                                SapEntry = 0;
                                SapCardCode = CardCode;
                                return false;
                            }
                        }
                        else
                        {
                            SapEntry = 0;
                            SapCardCode = CardCode;
                            return false;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(CardCode))
                        {
                            StringBuilder model = new StringBuilder();
                            model.Append(company.GET($"{module}('{CardCode}')"));

                            if (model.Length > 0)
                            {
                                company.DELETE(module);
                            }
                        }

                        SapEntry = 0;
                        SapCardCode = CardCode;
                        return false;
                    }
                }
                else
                {
                    SapEntry = 0;
                    SapCardCode = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                //DELETE PAYMENT IF ERROR IN POSTING
                hana.Execute($@"DELETE FROM QUT2 Where ""DocEntry"" = '{DocEntry}' and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
                hana.Execute($@"DELETE FROM QUT3 Where ""DocEntry"" = '{DocEntry}' and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
                hana.Execute($@"DELETE FROM QUT4 Where ""DocEntry"" = '{DocEntry}' and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
                Message = ex.Message;
                SapEntry = 0;
                SapCardCode = null;
                return false;
            }
        }
        #endregion

        #region SALES ORDER

        bool CreateSalesOrder(int aDocEntry, int aUserID, string Block, string Lot, string LotArea, out int DocEntry, out string CardCode, out string ErrMsg)
        {
            bool ret = false;
            int Entry = 0;
            string oCardCode = "";
            try
            {
                oOrder = oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                oOrder.DocObjectCode = BoObjectTypes.oOrders;
                DateTime oDocDate = DateTime.Now;
                int oDocEntry = 0;
                DataTable dt = new DataTable();

                dt = ws.Select($"SELECT SAPDocEntry FROM OQUT WHERE DocEntry = {aDocEntry}", "Select", "Addon").Tables["Select"];

                oDocEntry = int.Parse((string)DataAccess.GetData(dt, 0, "SAPDocEntry", "0"));
                dt = ws.Select($"SELECT CardCode,DocDate FROM OQUT WHERE DocEntry = {oDocEntry}", "Select", "SAP").Tables["Select"];

                oDocDate = DateTime.Parse((string)DataAccess.GetData(dt, 0, "DocDate", DateTime.Now.ToString()));
                oCardCode = (string)DataAccess.GetData(dt, 0, "CardCode", "0");
                string UserName = ws.WebUsername(aUserID);

                oOrder.DocDate = oDocDate;
                oOrder.DocDueDate = oDocDate;
                oOrder.TaxDate = oDocDate;
                oOrder.CardCode = oCardCode;
                oOrder.UserFields.Fields.Item("U_PrepBy").Value = UserName;
                oOrder.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";
                oOrder.DocType = BoDocumentTypes.dDocument_Items;
                dt = ws.GetSQDetails(oDocEntry).Tables[0];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var oLines = oOrder.Lines;

                    oLines.BaseEntry = oDocEntry;
                    oLines.BaseLine = int.Parse((string)DataAccess.GetData(dt, i, "LineNum", "0")); ;
                    oLines.BaseType = 23;
                    oLines.ItemCode = (string)DataAccess.GetData(dt, i, "ItemCode", "");
                    //oLines.Quantity = 1;

                    //** check if item is manage by batch **//
                    //DataTable dtBatch = DataAccess.Select("SAP", $"SELECT ItemCode FROM OITM Where ItemCode = '{(string)DataAccess.GetData(dt, i, "ItemCode", "")}' And ManBtchNum = 'Y'");
                    //if (dtBatch.Rows.Count > 0)
                    //{
                    //    //** add batch **//
                    //    oLines.BatchNumbers.BatchNumber = string.Format("BL{0}LT{1}", Block, Lot);
                    //    oLines.BatchNumbers.Quantity = double.Parse(LotArea);
                    //    oLines.BatchNumbers.Add();
                    //}
                    oLines.Add();
                }

                ret = AddToSAP(oOrder, out ErrMsg);

                if (ret == true)
                { Entry = int.Parse(oCompany.GetNewObjectKey()); }


            }
            catch (Exception ex)
            { ErrMsg = ex.Message; }
            DocEntry = Entry;
            CardCode = oCardCode;
            return ret;
        }

        bool CreateARDPR(int aUserID, int SQEntry, int DPEntry, string oCardCode, out int oEntry, out string ErrMsg)
        {
            bool ret = false;
            string Err = "";
            try
            {
                DataTable dt = new DataTable();
                dt = ws.Select($"SELECT SAPDocEntry,NetDP FROM OQUT WHERE DocEntry = {SQEntry}", "Select", "Addon").Tables["Select"];

                oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oDownPayments);
                oInvoice.DownPaymentType = DownPaymentTypeEnum.dptRequest;
                oInvoice.CardCode = oCardCode;
                oInvoice.DocDate = DateTime.Now;
                oInvoice.DocTotal = double.Parse((string)DataAccess.GetData(dt, 0, "NetDP", "0"));

                string UserName = ws.WebUsername(aUserID);
                oInvoice.UserFields.Fields.Item("U_PrepBy").Value = UserName;
                oInvoice.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";

                dt = ws.Select($"SELECT ItemCode,Quantity,LineNum FROM QUT1 (NOLOCK) WHERE DocEntry = {(string)DataAccess.GetData(dt, 0, "SAPDocEntry", "0")} AND TreeType IN ('S','T') AND UseBaseUn = 'N'", "Select", "SAP").Tables["Select"];

                foreach (DataRow dr in dt.Rows)
                {
                    string itemCode = dr["ItemCode"].ToString();
                    oInvoice.Lines.BaseEntry = DPEntry;
                    oInvoice.Lines.BaseLine = int.Parse(dr["LineNum"].ToString());
                    oInvoice.Lines.BaseType = 17;


                    oInvoice.Lines.ItemCode = itemCode;
                    oInvoice.Lines.Quantity = double.Parse(dr["Quantity"].ToString());
                    oInvoice.Lines.Add();
                }

                ret = AddToSAP(oInvoice, out Err);

                if (ret == true)
                { oEntry = int.Parse(oCompany.GetNewObjectKey()); }
                else
                    oEntry = 0;
            }
            catch (Exception ex)
            {
                Err = ex.Message;
                oEntry = 0;
            }

            ErrMsg = Err;
            return ret;
        }

        public bool CreateSO(int DocEntry, int UserID, string Block, string Lot, string LotArea, string DocType, out string Msg)
        {
            bool ret = true;
            string Err = "", ret2 = "";
            int oEntry;
            try
            {
                //** POSTING TO SAP **//
                if (ConnectToSAPDI(out oCompany, out Err))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    int SODocEntry = 0;
                    string SOCardCode = "";
                    if (CreateSalesOrder(DocEntry, UserID, Block, Lot, LotArea, out SODocEntry, out SOCardCode, out Err) == true)
                    {
                        if (CreateARDPR(UserID, DocEntry, SODocEntry, SOCardCode, out oEntry, out Err) == true)
                        {
                            //** create downpayment **//
                            ret2 = ws.SetDownpayment(DocEntry, DocType, UserID);

                            if (ret2 == "Operation completed successfully.")
                            {
                                if (oCompany.InTransaction)
                                    oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                                ws.Execute($"UPDATE OQUT SET DPEntry = '{oEntry}' WHERE DocEntry = {DocEntry}", "Addon");

                            }
                            else
                            {
                                if (oCompany.InTransaction)
                                    oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            }
                        }
                        else
                        {
                            if (oCompany.InTransaction)
                            { oCompany.EndTransaction(BoWfTransOpt.wf_RollBack); }
                            ret = false;
                        }
                    }
                    else
                    {
                        if (oCompany.InTransaction)
                        { oCompany.EndTransaction(BoWfTransOpt.wf_RollBack); }
                        ret = false;
                    }
                }
                else
                { ret = false; }

            }
            catch (Exception ex)
            { Err = ex.Message; }

            Msg = Err;
            return ret;
        }


        public bool CreateDPTerms(int DocEntry, string aDocEntry, double CAmount, out string ErrMsg, out int oDocEntry)
        {
            bool ret = false;
            string Err = "";
            int Entry = 0;
            try
            {
                if (ConnectToSAPDI(out oCompany, out Err))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    DataTable dt = new DataTable();
                    dt = ws.Select($"SELECT CardCode FROM ODPI WHERE DocEntry = {DocEntry}", "ODPI", "SAP").Tables["ODPI"];
                    oPayments = oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                    oPayments.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                    oPayments.Invoices.SumApplied = CAmount;
                    oPayments.Invoices.DocEntry = DocEntry;
                    oPayments.CardCode = (string)DataAccess.GetData(dt, 0, "CardCode", "0");

                    oPayments.DocDate = DateTime.Now;
                    oPayments.Remarks = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";

                    oPayments.CashAccount = ConfigSettings.CashAccount;
                    oPayments.CashSum = CAmount;

                    lRetCode = oPayments.Add();

                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        Entry = 0;
                        Err = lErrCode + "-" + sErrMsg;
                        ret = false;
                    }
                    else
                    {
                        Entry = int.Parse(oCompany.GetNewObjectKey());
                        Err = null;
                        ret = true;
                    }

                    if (ret == true)
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                    }
                    else
                    {
                        if (oCompany.InTransaction)
                        { oCompany.EndTransaction(BoWfTransOpt.wf_RollBack); }
                    }
                }
                else
                { ret = false; }
            }
            catch (Exception ex)
            {
                Entry = 0;
                Err = ex.Message;
                ret = false;
            }
            ErrMsg = Err;
            oDocEntry = Entry;
            return ret;
        }
        public bool CreateARforPenalty(int SODocEntry, string CardCode, double TotalPenalties, out string ErrMsg)
        {
            if (ConnectToSAPDI(out oCompany, out ErrMsg))
            {
                if (!oCompany.InTransaction) oCompany.StartTransaction();

                //** update sales order **//
                //** insert new line items (penalties) **//
                oOrder = oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                oOrder.DocObjectCode = BoObjectTypes.oOrders;
                oOrder.GetByKey(SODocEntry);
                //** sales order lines - penalties **//
                oOrder.Lines.Add();
                oOrder.Lines.ItemCode = ConfigSettings.PenaltyItemCode;
                oOrder.Lines.Quantity = 1;
                oOrder.Lines.VatGroup = "OTE";
                oOrder.Lines.UnitPrice = TotalPenalties;
                oOrder.Lines.Add();

                lRetCode = oOrder.Update();

                if (lRetCode == 0)
                {
                    //** create ar invoice for penalties **//
                    oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                    oInvoice.CardCode = CardCode;
                    oInvoice.DocDate = DateTime.Now;
                    //oInvoice.DocTotal = TotalPenalties;
                    oInvoice.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";

                    DataTable dtItems = DataAccess.Select("SAP", $"SELECT LineNum,ItemCode FROM RDR1 (NOLOCK) Where DocEntry = '{SODocEntry}' and ItemCode = '{ConfigSettings.PenaltyItemCode}' and LineStatus = 'O'");
                    if (dtItems.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtItems.Rows)
                        {
                            string itemCode = string.Format("{0}", dr[1]);
                            //** set base entry **//
                            oInvoice.Lines.BaseEntry = SODocEntry;
                            oInvoice.Lines.BaseLine = int.Parse(dr[0].ToString());
                            oInvoice.Lines.BaseType = 17;
                            //** adding of line Items **//
                            oInvoice.Lines.ItemCode = itemCode;
                            oInvoice.Lines.Quantity = 1;
                            oInvoice.Lines.Add();
                        }
                    }
                    else
                    {
                        ErrMsg = "Error in fetching data from sales order";
                        return false;
                    }
                    lRetCode = oInvoice.Add();

                    if (lRetCode == 0)
                    {
                        ErrMsg = null;
                        return true;
                    }
                    else
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        ErrMsg = lErrCode + "-" + sErrMsg;
                        return false;
                    }
                }
                else
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
            }
            else
            {
                oCompany.GetLastError(out lErrCode, out sErrMsg);
                ErrMsg = lErrCode + "-" + sErrMsg;
                return false;
            }
        }


        public bool CreateDPIncomingPayments(int AOEntry,
            int SQEntry,                //2
            int SODocEntry,
            int ARDPEntry,               //4
            int LBEntry,
            string CardCode,             //6
            double DPBalance,
            bool firstLB,               //8
            double TotalCash,
            double TotalPayment,        //10
            double TotalPenalties,
            double TotalInterest,       //12
            double TotalLoanable,
            string OR,                  //14
            string Remarks,
            DataTable dtPayments,       //16
            GridView gv,
            GridView gvPayments,        //18
            double AmountPaid,
            double TotalDP,             //20
            string PrjCode,
            string HouseModel,
            out string ErrMsg,          //22
            out int oDocEntry)
        {
            try
            {
                SapHanaLayer company = new SapHanaLayer();
                bool result = company.Connect();
                ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                oDocEntry = 0;
                if (result)
                {
                    //bool firstLoanable = false;
                    int ARDPLoanable = 0;
                    int dpentry = 0;
                    //** get addon dp entry **//
                    //ws.Select($@"SELECT ""DocEntry"" FROM ODPI Where ""BaseDocEntry"" = '{AOEntry}'", "dtDPEntry", "SAP").Tables[0];
                    DataTable dtdpentry = ws.Select($@"SELECT ""DocEntry"" FROM ODPI Where ""BaseDocEntry"" = '{AOEntry}'", "dtdpentry", "SAPHana").Tables[0];
                    if (dtdpentry.Rows.Count > 0)
                        dpentry = int.Parse(dtdpentry.Rows[0][0].ToString());


                    if (firstLB == true)
                    {
                        //** create ARDP for LB **//
                        string module = $"DownPayments";
                        IDictionary<string, object> DownPayments = new Dictionary<string, object>()
                        {
                            {"DownPaymentType", "dptRequest"},
                            {"CardCode", CardCode},
                            {"DocDate", DateTime.Now.ToString("yyyy-MM-dd")},
                            {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                            {"DocTotal", Math.Round(TotalLoanable, 4)}
                        };

                        IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
                        DataTable dtItems = hana.GetData($@"SELECT A.""LineNum"",A.""ItemCode"" FROM RDR1 A Where A.""DocEntry"" = '{SODocEntry}'", hana.GetConnection("SAPHana"));

                        if (DataAccess.Exist(dtItems))
                        {
                            foreach (DataRow dr in dtItems.Rows)
                            {
                                var lines = new Dictionary<string, object>();
                                string itemCode = string.Format("{0}", dr[1]);
                                //** set base entry **//
                                lines.Add("BaseEntry", SODocEntry);
                                lines.Add("BaseLine", int.Parse(dr[0].ToString()));
                                lines.Add("BaseType", 17);
                                //** adding of line Items **//
                                lines.Add("ItemCode", itemCode);
                                lines.Add("Quantity", 1);
                                DocumentLines.Add(lines);
                            }
                        }


                        //############################## [POSTING HERE] ##############################
                        StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                        var json = DataHelper.JsonBuilder(DownPayments, DocumentLine);

                        if (DocumentLines.Count > 0)
                        {
                            if (company.POST(module, json))
                            {
                                ErrMsg = $"{module} successfully created.";
                                oDocEntry = int.Parse(company.ResultDescription);
                                ARDPLoanable = int.Parse(company.ResultDescription);
                                result = true;
                            }
                            else
                            {
                                ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                oDocEntry = 0;
                                result = false;
                            }
                        }
                        else
                        {
                            ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                            oDocEntry = 0;
                            result = false;
                        }
                    }

                    //** check if penalty > 0 **//
                    int ARPenaltyEntry = 0;

                    if (TotalPenalties > 0 && AmountPaid < TotalPenalties)
                    {

                        //** update sales order **//
                        //** insert new line items (penalties) **//
                        string module = $"Orders({SODocEntry})";
                        IDictionary<string, object> Orders = new Dictionary<string, object>();

                        IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
                        var lines = new Dictionary<string, object>()
                        {
                            { "ItemCode",ConfigSettings.PenaltyItemCode },
                            { "Quantity",1 },
                            { "VatGroup","OTE" },
                            { "UnitPrice",TotalPenalties }
                        };

                        DocumentLines.Add(lines);

                        lRetCode = oOrder.Update();

                        //############################## [POSTING HERE] ##############################
                        StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                        var json = DataHelper.JsonBuilder(Orders, DocumentLine);

                        if (DocumentLines.Count > 0)
                        {
                            if (company.PATCH(module, json))
                            {

                                module = $"Invoices";
                                IDictionary<string, object> Invoices = new Dictionary<string, object>()
                                {
                                    {"CardCode",CardCode},
                                    {"DocDate",DateTime.Now.ToString("yyyy-MM-dd")},
                                    {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"}

                                };

                                DocumentLines = new List<IDictionary<string, object>>();
                                DataTable dtItems = hana.GetData($@"SELECT A.""LineNum"",A.""ItemCode"" FROM RDR1 A Where A.""DocEntry"" = '{SODocEntry}' and ""ItemCode"" = '{ConfigSettings.PenaltyItemCode}'", hana.GetConnection("SAPHana"));

                                if (DataAccess.Exist(dtItems))
                                {
                                    foreach (DataRow dr in dtItems.Rows)
                                    {
                                        string itemCode = string.Format("{0}", dr[1]);
                                        //** set base entry **//
                                        var line = new Dictionary<string, object>()
                                        {
                                            { "BaseEntry",SODocEntry },
                                            { "BaseLine",int.Parse(dr[0].ToString()) },
                                            { "BaseType",17 },
                                            { "ItemCode",itemCode },
                                            { "Quantity",1 }
                                        };
                                        DocumentLines.Add(lines);
                                    }
                                    ErrMsg = "Error in fetching data from sales order";
                                    oDocEntry = 0;
                                }
                                else
                                {
                                    ErrMsg = "Error in fetching data from sales order";
                                    oDocEntry = 0;
                                    return false;
                                }

                                //############################## [POSTING HERE] ##############################
                                DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                                json = DataHelper.JsonBuilder(Invoices, DocumentLine);

                                if (DocumentLines.Count > 0)
                                {
                                    if (company.POST(module, json))
                                    {
                                        ErrMsg = $"{module} successfully created.";
                                        oDocEntry = int.Parse(company.ResultDescription);
                                        ARPenaltyEntry = int.Parse(company.ResultDescription);
                                        result = true;
                                    }
                                    else
                                    {
                                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                        oDocEntry = 0;
                                        result = false;
                                    }
                                }
                                else
                                {
                                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                    oDocEntry = 0;
                                    result = false;
                                }

                            }
                            else
                            {
                                ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                oDocEntry = 0;
                                result = false;
                            }
                        }
                        else
                        {
                            ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                            oDocEntry = 0;
                            result = false;
                        }
                    }

                    //** check if interest > 0 **//
                    int ARInterestEntry = 0;
                    if (TotalInterest > 0)
                    {
                        //** update sales order **//
                        //** insert new line items (interest) **//
                        string module = $"Orders({SODocEntry})";
                        IDictionary<string, object> Orders = new Dictionary<string, object>();

                        IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
                        DataTable dtItems = hana.GetData($@"SELECT A.""LineNum"",A.""ItemCode"" FROM RDR1 A Where A.""DocEntry"" = '{SODocEntry}' and ""ItemCode"" = '{ConfigSettings.PenaltyItemCode}'", hana.GetConnection("SAPHana"));

                        if (DataAccess.Exist(dtItems))
                        {
                            foreach (DataRow dr in dtItems.Rows)
                            {
                                string itemCode = string.Format("{0}", dr[1]);
                                //** set base entry **//
                                var line = new Dictionary<string, object>()
                                        {
                                            { "ItemCode",ConfigSettings.InterestItemCode },
                                            { "Quantity", 1},
                                            { "VatGroup","OTE" },
                                            { "UnitPrice",TotalInterest }
                                        };
                                DocumentLines.Add(line);
                            }
                        }
                        else
                        {
                            ErrMsg = "Error in fetching data from sales order";
                            oDocEntry = 0;
                            return false;
                        }

                        //############################## [POSTING HERE] ##############################
                        var DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                        var json = DataHelper.JsonBuilder(Orders, DocumentLine);

                        if (DocumentLines.Count > 0)
                        {
                            if (company.PATCH(module, json))
                            {
                                module = $"Invoices";
                                IDictionary<string, object> Invoices = new Dictionary<string, object>()
                                {
                                    {"CardCode",CardCode},
                                    {"DocDate",DateTime.Now.ToString("yyyy-MM-dd")},
                                    {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"}

                                };

                                DocumentLines = new List<IDictionary<string, object>>();
                                dtItems = hana.GetData($@"SELECT A.""LineNum"",A.""ItemCode"" FROM RDR1 A Where A.""DocEntry"" = '{SODocEntry}' and ""ItemCode"" = '{ConfigSettings.PenaltyItemCode}' and LineStatus = 'O'", hana.GetConnection("SAPHana"));

                                if (DataAccess.Exist(dtItems))
                                {
                                    foreach (DataRow dr in dtItems.Rows)
                                    {
                                        string itemCode = string.Format("{0}", dr[1]);
                                        //** set base entry **//
                                        var line = new Dictionary<string, object>()
                                        {
                                            { "BaseEntry",SODocEntry },
                                            { "BaseLine",int.Parse(dr[0].ToString()) },
                                            { "BaseType",17 },
                                            { "ItemCode",itemCode },
                                            { "Quantity",1 }
                                        };
                                        DocumentLines.Add(line);
                                    }
                                }
                                else
                                {
                                    ErrMsg = "Error in fetching data from sales order";
                                    oDocEntry = 0;
                                    return false;
                                }

                                //############################## [POSTING HERE] ##############################
                                DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                                json = DataHelper.JsonBuilder(Invoices, DocumentLine);

                                if (DocumentLines.Count > 0)
                                {
                                    if (company.POST(module, json))
                                    {
                                        oDocEntry = int.Parse(company.ResultDescription);
                                        ARInterestEntry = int.Parse(company.ResultDescription);
                                    }
                                    else
                                    {
                                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                        oDocEntry = 0;
                                        result = false;
                                    }
                                }
                                else
                                {
                                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                    oDocEntry = 0;
                                    result = false;
                                }
                            }
                            else
                            {
                                ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                oDocEntry = 0;
                                result = false;
                            }
                        }
                        else
                        {
                            ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                            oDocEntry = 0;
                            result = false;
                        }

                        //** create payments **//

                        module = $"IncomingPayments";
                        IDictionary<string, object> IncomingPayments = new Dictionary<string, object>()
                        {
                            {"CardCode",CardCode},
                            {"DocDate",DateTime.Now.ToString("yyyy-MM-dd")},
                            {"Remarks", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                            {"U_ORNo", OR},
                            {"U_Remarks", Remarks}

                        };


                        IList<IDictionary<string, object>> PaymentInvoices = new List<IDictionary<string, object>>();

                        var PaymentInvoicesLines = new Dictionary<string, object>();
                        PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");

                        PaymentInvoices.Add(PaymentInvoicesLines);
                        if (ARPenaltyEntry != 0)
                        {
                            PaymentInvoicesLines.Add("DocEntry", ARPenaltyEntry);
                            PaymentInvoicesLines.Add("SumApplied", TotalPenalties);
                        }
                        if (ARInterestEntry != 0)
                        {
                            PaymentInvoicesLines.Add("DocEntry", ARInterestEntry);
                            PaymentInvoicesLines.Add("SumApplied", TotalInterest);
                        }
                        PaymentInvoices.Add(PaymentInvoicesLines);

                        if (ARDPEntry != 0)
                        {
                            PaymentInvoicesLines.Add("DocEntry", ARDPEntry);
                            PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
                            if (AmountPaid > 0 && AmountPaid == TotalPenalties)
                            {
                                PaymentInvoicesLines.Add("SumApplied", TotalPayment);
                            }
                            else if (TotalPayment > TotalPenalties && DPBalance > 0 && LBEntry == 0)
                            {
                                PaymentInvoicesLines.Add("SumApplied", TotalPayment - (TotalPenalties));
                            }
                            PaymentInvoices.Add(PaymentInvoicesLines);
                        }

                        if (firstLB)
                        {
                            PaymentInvoicesLines.Add("DocEntry", ARDPLoanable);
                            PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
                            PaymentInvoicesLines.Add("SumApplied", TotalPayment - TotalInterest);
                        }
                        else
                        {
                            if (LBEntry != 0)
                            {
                                PaymentInvoicesLines.Add("DocEntry", LBEntry);
                                PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
                                PaymentInvoicesLines.Add("SumApplied", TotalPayment - TotalInterest);
                            }
                        }

                        //** cash payment **//
                        IncomingPayments.Add("CashAccount", ConfigSettings.CashAccount);
                        IncomingPayments.Add("CashSum", TotalCash);

                        IList<IDictionary<string, object>> PaymentChecks = new List<IDictionary<string, object>>();
                        IList<IDictionary<string, object>> PaymentCreditCards = new List<IDictionary<string, object>>();
                        foreach (DataRow row in dtPayments.Rows)
                        {
                            var lines = new Dictionary<string, object>();

                            if (!string.IsNullOrWhiteSpace(row["CheckNo"].ToString()))
                            {
                                if (int.Parse(row["CheckNo"].ToString()) != 0)
                                {
                                    lines.Add("DueDate", Convert.ToDateTime(row["DueDate"].ToString()).ToString("yyyy-MM-dd"));
                                    lines.Add("CheckSum", double.Parse(row["Amount"].ToString()));
                                    lines.Add("BankCode", row["BankCode"].ToString());
                                    lines.Add("Branch", row["Branch"].ToString());
                                    lines.Add("AccounttNum", row["AccountNum"].ToString());
                                    lines.Add("CheckNumber", int.Parse(row["CheckNo"].ToString()));
                                    lines.Add("CheckAccount", ConfigSettings.CheckAccount);
                                    PaymentChecks.Add(lines);
                                }
                            }
                            //** credit **//
                            else if (row["Type"].ToString() == "Credit")
                            {
                                lines.Add("DueDate", Convert.ToDateTime(row["DueDate"].ToString()).ToString("yyyy-MM-dd"));

                                //** get code of creditcard
                                DataTable dtCredit = hana.GetData($@"SELECT ""CreditCard"" FROM OCRC Where ""CardName"" ='{row["CreditCard"].ToString()}'", hana.GetConnection("SAPHana"));

                                string creditcard = dtCredit.Rows[0][0].ToString();
                                lines.Add("CreditCard", int.Parse(creditcard));
                                lines.Add("CreditAcct", row["CreditAcctCode"].ToString());
                                lines.Add("CreditCardNumber", row["CreditCardNumber"].ToString());
                                lines.Add("CreditSum", double.Parse(row["Amount"].ToString()));
                                lines.Add("CardValidUntil", Convert.ToDateTime(row["ValidUntil"].ToString()).ToString("yyyy-MM-dd"));
                                lines.Add("PaymentMethodCode", int.Parse(row["PymtTypeCode"].ToString()));
                                lines.Add("NumOfPayments", int.Parse(row["NumOfPymts"].ToString()));
                                lines.Add("VoucherNum", row["VoucherNum"].ToString());
                                lines.Add("OwnerIdNum", row["IdNum"].ToString());
                                lines.Add("OwnerPhone", row["TelNum"].ToString());
                                lines.Add("CreditType", "cr_Regular");

                                PaymentCreditCards.Add(lines);
                            }
                        }

                        //############################## [POSTING HERE] ##############################
                        company = new SapHanaLayer();
                        result = company.Connect();

                        StringBuilder PaymentInvoicesLine = DataHelper.JsonLinesBuilder("PaymentInvoices", PaymentInvoices);
                        StringBuilder PaymentChecksLine = DataHelper.JsonLinesBuilder("PaymentChecks", PaymentChecks);
                        StringBuilder PaymentCreditCardsLine = DataHelper.JsonLinesBuilder("PaymentCreditCards", PaymentCreditCards);
                        StringBuilder strLines = DataHelper.JsonLinesCombine(PaymentInvoicesLine, PaymentChecksLine, PaymentCreditCardsLine);
                        json = DataHelper.JsonBuilder(IncomingPayments, strLines);

                        if (PaymentInvoices.Count > 0 || PaymentChecks.Count > 0 || PaymentCreditCards.Count > 0)
                        {
                            if (company.POST(module, json))
                            {
                                oDocEntry = int.Parse(company.ResultDescription);

                                // ** JournalEntries **//
                                DataTable dtRGP = new DataTable();
                                double RGPrate = 0;
                                dtRGP = hana.GetData($@"SELECT DISTINCT IFNULL(""U_RGPRate"",0) ""U_RGPRate"" FROM ""@HOUSE_MOD"" Where ""Code"" ='{HouseModel}'", hana.GetConnection("SAPHana"));
                                //CHECK IF MODEL HAS RATE 
                                RGPrate = Convert.ToDouble(string.IsNullOrEmpty((string)DataAccess.GetData(dtRGP, 0, "U_RGPRate", "0")) ? "0" : (string)DataAccess.GetData(dtRGP, 0, "U_RGPRate", "0"));

                                if (RGPrate == 0)
                                {
                                    dtRGP = hana.GetData($@"SELECT DISTINCT IFNULL(""U_prRGPRate"",0) ""U_prRGPRate"" FROM OPRJ Where ""PrjCode"" ='{PrjCode}'", hana.GetConnection("SAPHana"));
                                }
                                RGPrate = double.Parse(dtRGP.Rows[0][0].ToString());

                                //if (DPBalance > 0)
                                //{
                                //    //JournalEntries(CardCode, PrjCode, SQEntry.ToString(), TotalDP, Rate, out ErrMsg);
                                //    JournalEntries(CardCode, PrjCode, SQEntry.ToString(), TotalPayment, RGPrate, UGPRate, out ErrMsg);
                                //}
                                //else
                                //{
                                JournalEntries(CardCode, PrjCode, SQEntry.ToString(), TotalPayment, RGPrate, out ErrMsg);
                                //    JournalEntries(CardCode, PrjCode, SQEntry.ToString(), TotalPayment, RGPrate, UGPRate, out ErrMsg);
                                //}



                                bool isErr = false;

                                //**create opyt table amountpaid **//
                                if (!ws.CreateDownPayment(AOEntry, oDocEntry, OR, Remarks, TotalPayment, TotalPenalties, TotalInterest))
                                {
                                    isErr = true;
                                }

                                if (isErr == false)
                                {
                                    foreach (GridViewRow row in gv.Rows)
                                    {
                                        int _entry = int.Parse(row.Cells[0].Text);
                                        int _terms = int.Parse(row.Cells[1].Text);
                                        string _type = row.Cells[2].Text;
                                        double penalty = double.Parse(row.Cells[7].Text);
                                        double payment = double.Parse(row.Cells[9].Text);
                                        //** update dpi1 amountpaid **//
                                        if (payment > 0)
                                        {
                                            if (!ws.UpdateDPPayment(_entry, _terms, _type, payment, penalty))
                                            {
                                                isErr = true;
                                            }

                                        }
                                    }
                                    //** create payment breakdown
                                    foreach (GridViewRow _row in gvPayments.Rows)
                                    {
                                        string type = _row.Cells[1].Text;
                                        double amount = double.Parse(_row.Cells[2].Text);
                                        string checkno = _row.Cells[3].Text;
                                        string bank = _row.Cells[4].Text;
                                        string bankname = _row.Cells[5].Text;
                                        string branch = _row.Cells[6].Text;
                                        string checkdate = _row.Cells[7].Text;
                                        string account = _row.Cells[8].Text;
                                        //credit
                                        string CreditCard = _row.Cells[9].Text;
                                        string CreditAcctCode = _row.Cells[10].Text;
                                        string CreditAcct = _row.Cells[11].Text;
                                        string CreditCardNumber = _row.Cells[12].Text;
                                        string ValidUntil = _row.Cells[13].Text;
                                        string IdNum = _row.Cells[14].Text.Trim();
                                        string TelNum = _row.Cells[15].Text.Trim();
                                        string PymtTypeCode = _row.Cells[16].Text;
                                        string PymtType = _row.Cells[17].Text;
                                        string NumOfPymts = _row.Cells[18].Text;
                                        string VoucherNum = _row.Cells[19].Text;
                                        int Id = int.Parse(_row.Cells[20].Text);

                                        if (type == "Cash")
                                        {
                                            if (!ws.AddDownPayments(
                                                 type,
                                                 oDocEntry,
                                                 amount,
                                                 OR,
                                                 Remarks
                                                 ))
                                            {
                                                isErr = true;
                                            }
                                            else
                                            {
                                                // ** loop included terms **//
                                                foreach (GridViewRow row in gv.Rows)
                                                {
                                                    int _terms = int.Parse(row.Cells[1].Text);
                                                    double payment = double.Parse(row.Cells[9].Text);
                                                    string _type = row.Cells[2].Text;

                                                    if (payment > 0)
                                                        if (!ws.CreateDownPaymentTerms(AOEntry, oDocEntry, _type, _terms))
                                                            isErr = true;
                                                }
                                            }
                                        }
                                        else if (!string.IsNullOrWhiteSpace(checkno))
                                        {

                                            if (!ws.AddDownPayments(
                                                type,
                                                oDocEntry,
                                                amount,
                                                OR,
                                                Remarks,
                                                checkdate,
                                                checkno,
                                                bank,
                                                bankname,
                                                branch,
                                                account
                                                ))
                                            {
                                                isErr = true;
                                            }
                                            else
                                            {
                                                // ** loop included terms **//
                                                foreach (GridViewRow row in gv.Rows)
                                                {
                                                    int _terms = int.Parse(row.Cells[1].Text);
                                                    double payment = double.Parse(row.Cells[9].Text);
                                                    string _type = row.Cells[2].Text;

                                                    if (payment > 0)
                                                        if (!ws.CreateDownPaymentTerms(AOEntry, oDocEntry, _type, _terms))
                                                            isErr = true;
                                                }
                                            }
                                        }
                                        else if (type == "Credit")
                                        {
                                            if (!ws.AddDownPayments(
                                                 type,
                                                oDocEntry,
                                                amount,
                                                OR,
                                                Remarks,
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                string.Empty,
                                                CreditCard,
                                                CreditAcctCode,
                                                CreditAcct,
                                                CreditCardNumber,
                                                ValidUntil,
                                                IdNum,
                                                TelNum,
                                                PymtTypeCode,
                                                PymtType,
                                                NumOfPymts,
                                                VoucherNum,
                                                "S"
                                                ))
                                            {
                                                isErr = true;
                                            }
                                            else
                                            {
                                                // ** loop included terms **//
                                                foreach (GridViewRow row in gv.Rows)
                                                {
                                                    int _terms = int.Parse(row.Cells[1].Text);
                                                    double payment = double.Parse(row.Cells[9].Text);
                                                    string _type = row.Cells[2].Text;
                                                    if (payment > 0)
                                                        if (!ws.CreateDownPaymentTerms(AOEntry, oDocEntry, _type, _terms))
                                                            isErr = true;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (isErr == false)
                                {
                                    if (ARDPLoanable != 0)
                                        hana.Execute($@"Update OQUT SET ""LBEntry"" = '{ARDPLoanable}' WHERE ""DocEntry"" = '{AOEntry}'", hana.GetConnection("SAOHana"));
                                    oDocEntry = int.Parse(company.ResultDescription);
                                    result = true;
                                }
                                else
                                {

                                    oDocEntry = 0;
                                    result = false;
                                }
                            }
                            else
                            {
                                ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                                oDocEntry = 0;
                                result = false;
                            }
                        }
                        else
                        {
                            ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                            oDocEntry = 0;
                            result = false;
                        }
                        return result;
                    }
                    else
                    {
                        ErrMsg = "Error in connecting to SAP";
                        oDocEntry = 0;
                        return false;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                oDocEntry = 0;
                return false;
            }
        }
        #endregion

        void cancelSAPTransactions(StringBuilder model, SapHanaLayer company, string module, int entry)
        {
            if (model.Length > 0)
            {
                company.POST($"{module}('{entry}')/Cancel", new StringBuilder());
            }
        }

        bool cancelSAPTransactionsNew(SapHanaLayer company, string module, int entry, string table, string field, out string errorDesc)
        {
            errorDesc = "";
            bool return1 = true;

            string qry = "";

            if (module != "Deposits")
            {
                qry = $@"SELECT ""DocEntry"" FROM {table} WHERE ""DocEntry"" = {entry} AND ""{field}"" = 'N'";

                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    return1 = company.POST($"{module}({entry})/Cancel", new StringBuilder());
                    errorDesc = company.ResultDescription;
                    return return1;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                qry = $@"SELECT ""DeposId"" FROM {table} WHERE ""DeposId"" = {entry} AND ""{field}"" = 'N' AND ""CnclDps"" = -1";

                DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    return1 = company.POST($"{module}({entry})/CancelDepositbyCurrentSystemDate", new StringBuilder());
                    errorDesc = company.ResultDescription;
                    return return1;
                }
                else
                {
                    return true;
                }
            }
        }

        bool patchSAPQuotation(SapHanaLayer company, string module, int entry, string ContractStatus, out string errorDesc)
        {
            //2024-06-13 : CONSIDERED INCOMINGPAYMENTS WHEN UPDATING CONTRACTSTATUS AND ISSUETYPE
            //return company.PATCH($@"{module}({entry})", $@"{{""U_IssueType"": ""CANCELED"", ""U_ContractStatus"":""Restructured""}}", out errorDesc);

            if (module == "IncomingPayments")
            {
                return company.PATCH($@"{module}({entry})", $@"{{""U_ContractStatus"":""{ContractStatus}""}}", out errorDesc);
            }
            else
            {
                return company.PATCH($@"{module}({entry})", $@"{{""U_IssueType"": ""CANCELED"", ""U_ContractStatus"":""{ContractStatus}""}}", out errorDesc);
            }
        }



        bool CreateSalesOrderRestructure(int DocEntry, string oCardCode, double Interest, double Penalty, string Block, string Lot, string LotArea, out int SODocEntry, out string ErrMsg)
        {
            int Entry = 0;
            try
            {
                oOrder = oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                oOrder.DocObjectCode = BoObjectTypes.oOrders;
                //string UserName = ws.WebUsername(aUserID);
                var oBaseDoc = oQuotation;
                oOrder.DocDate = DateTime.Now;
                oOrder.DocDueDate = DateTime.Now;
                oOrder.TaxDate = DateTime.Now;
                oOrder.CardCode = oCardCode;
                //oOrder.UserFields.Fields.Item("U_PrepBy").Value = UserName;
                oOrder.Comments = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";
                oOrder.DocType = BoDocumentTypes.dDocument_Items;

                DataTable dt = new DataTable();
                //                dt = ws.GetSQDetails(DocEntry).Tables[0];

                var oLines = oOrder.Lines;
                var oBaseLines = oBaseDoc.Lines;

                for (int i = 0; i < oBaseLines.Count; i++)
                {
                    oBaseLines.SetCurrentLine(i);
                    oLines.BaseType = int.Parse(oBaseDoc.DocObjectCodeEx);
                    var sam = int.Parse(oBaseDoc.DocObjectCodeEx);
                    oLines.ItemCode = oBaseLines.ItemCode;
                    oLines.BaseEntry = DocEntry;
                    oLines.BaseLine = i;
                    oLines.Quantity = oBaseLines.Quantity;

                    DataTable dtBatch = DataAccess.Select("SAP", $"SELECT ItemCode FROM OITM Where ItemCode = '{(string)DataAccess.GetData(dt, i, "ItemCode", "")}' And ManBtchNum = 'Y'");
                    if (dtBatch.Rows.Count > 0)
                    {
                        //** add batch **//
                        oLines.BatchNumbers.BatchNumber = string.Format("BL{0}LT{1}", Block, Lot);
                        oLines.BatchNumbers.Quantity = double.Parse(LotArea);
                        oLines.BatchNumbers.Add();
                    }
                    oLines.Add();
                }

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    oLines.BaseEntry = DocEntry;
                //    oLines.BaseLine = int.Parse((string)DataAccess.GetData(dt, i, "LineNum", "0"));
                //    oLines.BaseType = int.Parse(oBaseDoc.DocObjectCodeEx); //23
                //    oLines.ItemCode = (string)DataAccess.GetData(dt, i, "ItemCode", "");
                //    //oLines.Quantity = 1;

                //    //** check if item is manage by batch **//


                //    oLines.Add();
                //}

                if (Interest > 0)
                {
                    oLines.ItemCode = ConfigSettings.InterestItemCode;
                    oOrder.Lines.Quantity = 1;
                    oOrder.Lines.VatGroup = "OTE";
                    oOrder.Lines.UnitPrice = Interest;
                    oOrder.Lines.Add();
                }
                if (Penalty > 0)
                {
                    oLines.ItemCode = ConfigSettings.PenaltyItemCode;
                    oOrder.Lines.Quantity = 1;
                    oOrder.Lines.VatGroup = "OTE";
                    oOrder.Lines.UnitPrice = Penalty;
                    oOrder.Lines.Add();
                }


                lRetCode = oOrder.Add();

                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    SODocEntry = 0;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    Entry = int.Parse(oCompany.GetNewObjectKey());
                    SODocEntry = Entry;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                SODocEntry = 0;
                return false;
            }
        }





        bool CancelSAPDocuments(SapHanaLayer company, CashRegisterService cashRegister, string oldBlock, string oldLot, string oldPrjCode,
                                string newPrjCode, string newBlock, string newLot, string DocNum, string oSAPDB, string SQEntry,
                                string oldCardCode, string DocDate, string oldModel, string oldFinancingScheme, string oldProductType, double oldReservationFee,
                                double oldDPAmount, double AmountDue,

                                string CancellationTagging, string RestructuringDate,
                                out string errorDesc, out string RestructuringID)
        {
            bool isSuccess = true;
            DataTable dt;
            errorDesc = "";


            //var ticks = DateTime.Now.Ticks;
            //var guid = Guid.NewGuid().ToString();
            //var uniqueSessionId = ticks.ToString() + guid.ToString();
            //string[] agentCode = uniqueSessionId.Split('-');
            //RestructuringID = agentCode.Last();
            RestructuringID = DateTime.Now.ToString("yyyyMMddHHmmssffff");


            //CANCEL QUOTATIONS
            #region CANCEL QUOTATIONS
            string qry = $@"SELECT ""DocEntry"",""CardCode"" FROM OQUT WHERE IFNULL(""U_BlockNo"",'') = '{oldBlock}' AND IFNULL(""U_LotNo"",'') = '{oldLot}' AND 
                                IFNULL(""Project"",'') = '{oldPrjCode}' AND IFNULL(""U_IssueType"",'') <> 'CANCELED' AND IFNULL(""U_ContractStatus"",'') = 'Open'";
            DataTable dtQuotation = hana.GetData(qry, hana.GetConnection("SAPHana"));
            foreach (DataRow dr in dtQuotation.Rows)
            {
                //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                //if (isSuccess = patchSAPQuotation(company, "Quotations", Convert.ToInt32(dr["DocEntry"]),  out errorDesc))
                if (isSuccess = patchSAPQuotation(company, "Quotations", Convert.ToInt32(dr["DocEntry"]), CancellationTagging, out errorDesc))
                {
                    qry = $@"SELECT TOP 1 ""DocEntry"" FROM RDR1 WHERE ""BaseEntry"" = {dr["DocEntry"]} and ""BaseType"" = 23";
                    DataTable dtOrders = hana.GetData(qry, hana.GetConnection("SAPHana"));

                    foreach (DataRow dr1 in dtOrders.Rows)
                    {
                        //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                        //if (isSuccess = patchSAPQuotation(company, "Orders", Convert.ToInt32(dr1["DocEntry"]), out errorDesc))
                        if (isSuccess = patchSAPQuotation(company, "Orders", Convert.ToInt32(dr1["DocEntry"]), CancellationTagging, out errorDesc))
                        {

                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion



            //CANCEL AR RESERVE INVOICE AND AR INVOICE OF MISCELLANEOUS
            #region CANCEL AR RESERVE INVOICE
            if (isSuccess)
            {
                //CANCEL AR RESERVE INVOICE
                qry = $@"select ""DocEntry"", ""DocNum"" from OINV WHERE IFNULL(""U_BlockNo"",'') = '{oldBlock}' AND  IFNULL(""U_LotNo"",'') = '{oldLot}' AND 
                                    IFNULL(""Project"",'') = '{oldPrjCode}' AND IFNULL(""U_SalesType"",'') = 'Real Estate' AND
                                    ""U_DreamsQuotationNo"" = '{DocNum}' AND ""CANCELED"" NOT IN ('Y','C') 
                                    AND ""isIns"" = 'Y'  ";
                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        //CANCEL INCOMING PAYMENT
                        qry = $@"SELECT A.""DocEntry"" FROM ORCT A INNER JOIN RCT2 B ON A.""DocEntry"" = B.""DocNum"" WHERE B.""DocEntry"" = {dr["DocEntry"]} AND A.""Canceled"" <> 'Y' ";
                        DataTable dt2 = hana.GetData(qry, hana.GetConnection("SAPHana"));


                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            if (isSuccess = hana.Execute($@"UPDATE ORCT SET ""U_RestructCode"" = '{RestructuringID}'  WHERE ""DocEntry"" = {dr2["DocEntry"]}", hana.GetConnection("SAPHana")))
                            {

                                if (!cancelSAPTransactionsNew(company, "IncomingPayments", Convert.ToInt32(dr2["DocEntry"]), "ORCT", "Canceled", out errorDesc))
                                {
                                    isSuccess = false;
                                }
                                else
                                {
                                    //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                                    if (isSuccess = patchSAPQuotation(company, "IncomingPayments", Convert.ToInt32(dr2["DocEntry"]), CancellationTagging, out errorDesc))
                                    {

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }


                        //CANCEL AR INVOICE
                        if (isSuccess = cancelSAPTransactionsNew(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), "OINV", "CANCELED", out errorDesc))
                        {
                            //UPDATE CONTRACT STATUS AND ISSUE TYPE
                            //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                            //if (isSuccess = patchSAPQuotation(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), out errorDesc))
                            if (isSuccess = patchSAPQuotation(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), CancellationTagging, out errorDesc))
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion



            //CANCEL AR INVOICE OF MISCELLANEOUS
            #region CANCEL AR INVOICE
            if (isSuccess)
            {
                //CANCEL AR RESERVE INVOICE
                qry = $@"select ""DocEntry"", ""DocNum"" from OINV WHERE IFNULL(""U_BlockNo"",'') = '{oldBlock}' AND  IFNULL(""U_LotNo"",'') = '{oldLot}' AND 
                                    IFNULL(""Project"",'') = '{oldPrjCode}' AND IFNULL(""U_SalesType"",'') = 'Real Estate' AND
                                    ""U_DreamsQuotationNo"" = '{DocNum}' AND ""CANCELED"" NOT IN ('Y','C')  
                                    -- AND IFNULL(""U_Type"",'') = 'MISC' AND ""isIns"" = 'Y'  ";
                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        //CANCEL INCOMING PAYMENT
                        qry = $@"SELECT A.""DocEntry"" FROM ORCT A INNER JOIN RCT2 B ON A.""DocEntry"" = B.""DocNum"" WHERE B.""DocEntry"" = {dr["DocEntry"]} AND A.""Canceled"" <> 'Y' ";
                        DataTable dt2 = hana.GetData(qry, hana.GetConnection("SAPHana"));


                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            if (isSuccess = hana.Execute($@"UPDATE ORCT SET ""U_RestructCode"" = '{RestructuringID}'  WHERE ""DocEntry"" = {dr2["DocEntry"]}", hana.GetConnection("SAPHana")))
                            {

                                if (!cancelSAPTransactionsNew(company, "IncomingPayments", Convert.ToInt32(dr2["DocEntry"]), "ORCT", "Canceled", out errorDesc))
                                {
                                    isSuccess = false;
                                }
                                else
                                {
                                    //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                                    if (isSuccess = patchSAPQuotation(company, "IncomingPayments", Convert.ToInt32(dr2["DocEntry"]), CancellationTagging, out errorDesc))
                                    {

                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                        }


                        //CANCEL AR INVOICE
                        if (isSuccess = cancelSAPTransactionsNew(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), "OINV", "CANCELED", out errorDesc))
                        {
                            //UPDATE CONTRACT STATUS AND ISSUE TYPE
                            //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                            //if (isSuccess = patchSAPQuotation(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), out errorDesc))
                            if (isSuccess = patchSAPQuotation(company, "Invoices", Convert.ToInt32(dr["DocEntry"]), CancellationTagging, out errorDesc))
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion







            //CANCEL DEPOSITS
            #region CANCEL DEPOSITS
            if (isSuccess)
            {
                //CANCEL DEPOSITS
                qry = $@"SELECT 
                            E.* 
                        FROM 
                            ORCT A INNER JOIN 
                            rct3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN 
                            ocrh C ON B.""CrCardNum"" = C.""CrdCardNum"" AND  B.""DocNum"" = C.""RctAbs"" INNER JOIN 
                            ODPS E ON E.""DeposId"" = C.""DepNum""
                        WHERE 
                            A.""U_Project"" = '{oldPrjCode}' AND 
                            A.""U_BlockNo"" = '{oldBlock}' AND 
                            A.""U_LotNo"" = '{oldLot}' AND 
                            E.""Canceled"" ='N' and 
                            E.""CnclDps"" = -1";
                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (isSuccess = cancelSAPTransactionsNew(company, "Deposits", Convert.ToInt32(dr["DeposId"]), "ODPS", "Canceled", out errorDesc))
                        {
                            if (!hana.Execute($@"UPDATE ODPS SET ""U_ResCode"" = '{RestructuringID}'  WHERE ""DeposId"" = {dr["DeposId"]}", hana.GetConnection("SAPHana")))
                            {
                                isSuccess = false;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion





            //CANCEL INCOMING PAYMENTS
            #region NEW CANCEL INCOMING PAYMENTS
            if (isSuccess)
            {
                qry = $@" SELECT DISTINCT A.""DocEntry"", A.""DocNum"" FROM ORCT A INNER JOIN  
                           RCT2 B ON A.""DocEntry"" = B.""DocNum""  LEFT JOIN(SELECT x. ""U_DreamsQuotationNo"",	x.""U_BlockNo"",  
                            x.""U_LotNo"", x.""Project"", x.""DocEntry"", x.""DocNum"",	x.""ObjType"", x.""U_SalesType"", x.""CANCELED"" FROM
                           ODPI x UNION ALL SELECT y. ""U_DreamsQuotationNo"", y.""U_BlockNo"", y.""U_LotNo"",	y.""Project"", y.""DocEntry"",
                     y.""DocNum"", y.""ObjType"", y.""U_SalesType"", y.""CANCELED""  FROM OINV y 
                     --WHERE y.""isIns"" = 'Y'
                     ) C ON C.""DocEntry"" = B.""DocEntry"" AND C.""ObjType"" = B.""InvType"" WHERE IFNULL(C.""U_DreamsQuotationNo"",'') = '{DocNum}'
                     AND IFNULL(C.""U_BlockNo"",'') = '{oldBlock}' AND IFNULL(C.""U_LotNo"",'') = '{oldLot}' AND IFNULL(C.""Project"",'') = '{oldPrjCode}' AND
                     IFNULL(C.""U_SalesType"", '') = 'Real Estate' AND C.""CANCELED"" NOT IN('Y','C') AND A.""Canceled"" = 'N'";

                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                foreach (DataRow dr in dt.Rows)
                {
                    if (isSuccess = hana.Execute($@"UPDATE ORCT SET ""U_RestructCode"" = '{RestructuringID}'  WHERE ""DocEntry"" = {dr["DocEntry"]}", hana.GetConnection("SAPHana")))
                    {
                        if (!cancelSAPTransactionsNew(company, "IncomingPayments", Convert.ToInt32(dr["DocEntry"]), "ORCT", "Canceled", out errorDesc))
                        {
                            isSuccess = false;
                        }
                        else
                        {
                            //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                            if (isSuccess = patchSAPQuotation(company, "IncomingPayments", Convert.ToInt32(dr["DocEntry"]), CancellationTagging, out errorDesc))
                            {

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion

            //CANCEL AR DOWNPAYMENT INVOICES
            #region CANCEL AR DOWNPAYMENT INVOICES
            if (isSuccess)
            {
                string qryBPCode = $@"SELECT ""CardCode"" FROM OCRD WHERE ""U_DreamsCustCode"" = '{oldCardCode}'";
                DataTable dtCardCode = hana.GetData(qryBPCode, hana.GetConnection("SAPHana"));
                string SAPCardCode = DataAccess.GetData(dtCardCode, 0, "CardCode", "").ToString();

                //CANCEL AR DOWNPAYMENT INVOICES
                //qry = $@"select ""SapDocEntry"" from QUT1 WHERE IFNULL(""SapDocEntry"",0) <> 0 AND ""DocEntry"" = {SQEntry} AND ""PaymentType"" IN ('RES','DP')";
                qry = $@" select DISTINCT A.""DocEntry"",A.""U_Type"" from ODPI A INNER JOIN DPI1 B ON A.""DocEntry"" = B.""DocEntry"" WHERE 
                                    IFNULL(A.""U_BlockNo"", '') = '{oldBlock}' AND IFNULL(A.""U_LotNo"",'') = '{oldLot}' AND IFNULL(A.""Project"",'') = '{oldPrjCode}' AND
                                    IFNULL(A.""U_SalesType"", '') = 'Real Estate' AND A.""U_DreamsQuotationNo"" = '{DocNum}' AND
                                    A.""CardCode"" = '{SAPCardCode}' AND A.""CANCELED"" <> 'Y' AND B.""TargetType"" = -1 AND 
                                    A.""DocEntry"" NOT IN(SELECT DISTINCT x.""DocEntry"" FROM ORCT x INNER JOIN RCT2 y ON x.""DocEntry"" = y.""DocNum"" 
                                    WHERE y.""DocEntry"" = A.""DocEntry"" AND y.""InvType"" = 203 AND x.""Canceled"" = 'N' );";
                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                int docEntry = 0;



                foreach (DataRow dr in dt.Rows)
                {
                    if (isSuccess = cashRegister.CreateARCreditMemo(company, SAPCardCode,
                                                                    //DateTime.Now.ToShortDateString(), 0,
                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), 0,
                                                                    oldPrjCode, oldBlock,
                                                                    oldLot, oldModel,
                                                                    oldFinancingScheme, oldProductType,
                                                                    oldReservationFee, oldDPAmount,
                                                                    AmountDue, Convert.ToInt32(dr["DocEntry"]),
                                                                    out docEntry, out errorDesc))
                    {
                        //UPDATE CONTRACT STATUS AND ISSUE TYPE
                        //2024-06-13: ADDED CANCELLATION TAGGING DYNAMICALLY
                        //if (isSuccess = patchSAPQuotation(company, "DownPayments", Convert.ToInt32(dr["DocEntry"]), out errorDesc))
                        if (isSuccess = patchSAPQuotation(company, "DownPayments", Convert.ToInt32(dr["DocEntry"]), CancellationTagging, out errorDesc))
                        {

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            #endregion



            //CANCEL SALES ORDER
            #region CANCEL SALES ORDER
            if (isSuccess)
            {

                //CANCEL SALES ORDER
                qry = $@"select ""DocEntry"" from ORDR WHERE IFNULL(""U_BlockNo"",'') = '{oldBlock}' AND  IFNULL(""U_LotNo"",'') = '{oldLot}' AND 
                                    IFNULL(""Project"",'') = '{oldPrjCode}' AND IFNULL(""U_SalesType"",'') = 'Real Estate' AND
                                    ""U_DreamsQuotationNo"" = '{DocNum}' AND ""CANCELED"" <> 'Y'  ";
                dt = hana.GetData(qry, hana.GetConnection("SAPHana"));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (isSuccess = cancelSAPTransactionsNew(company, "Orders", Convert.ToInt32(dr["DocEntry"]), "ORDR", "CANCELED", out errorDesc))
                        {
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            #endregion



            //CANCEL PAYMENTS IN ADDON DATABASE
            #region CANCEL PAYMENTS
            if (isSuccess)
            {

                //CANCEL PAYMENTS IN ADDON DATABASE
                qry = $@" UPDATE QUT2 SET ""Cancelled"" = 'Y' WHERE ""DocEntry"" = {SQEntry};";
                string qry2 = $@"UPDATE QUT3 SET ""Cancelled"" = 'Y' WHERE ""DocEntry"" = {SQEntry}; ";
                string qry3 = $@"UPDATE QUT4 SET ""Cancelled"" = 'Y' WHERE ""DocEntry"" = {SQEntry}; ";
                string qry4 = $@"UPDATE QUT7 SET ""Cancelled"" = 'Y' WHERE ""DocEntry"" = {SQEntry}; ";
                //string qry5 = $@"UPDATE QUT13 SET ""Status"" = 'R' WHERE ""DocEntry"" = {SQEntry}; ";
                hana.Execute(qry, hana.GetConnection("SAOHana"));
                hana.Execute(qry2, hana.GetConnection("SAOHana"));
                hana.Execute(qry3, hana.GetConnection("SAOHana"));
                hana.Execute(qry4, hana.GetConnection("SAOHana"));
                //hana.Execute(qry5, hana.GetConnection("SAOHana"));

            }
            #endregion





            return isSuccess;
        }








        public bool CancelTransactionsNew(string SQEntry, SapHanaLayer company, //2

                                          //OLD
                                          string DocDate, string oldPrjCode, //4
                                          string oldBlock, string oldLot, //6
                                          double oldDPAmount, string oldModel, //8
                                          string oldFinancingScheme, double oldReservationFee,  //10
                                           string oldProductType, string oldCardCode, //12

                                          //NEW 
                                          string newModel, string newFinancingScheme,  //14 
                                          string newProductType, double newReservationFee, //16 
                                          double newDPAmount, double AmountDue,//18
                                          string newCardCode, double newMiscFee, //20
                                          double TotalNetPayments, string DocNum, //22
                                           string newBlock, string newLot,  //24 
                                           double TCPDownpayment, double TCPLoanableBalance, //26
                                           double newNETTCP, string Vatable,  //28
                                           double DiscountAmount, double MiscAmount,  //30
                                           string BatchNum, string EmpName,  //32
                                           string SoldWithAdjacentLot, string AdjacentLotQuotationNo, //34
                                           string newPrjCode, double MiscMonthly, //36 
                                           int DPTerms, string LOI, //38
                                           string UserCode, string RestructureAction, //40
                                           double TotalNetPaymentsMisc, double oldNetTCP,//42
                                           string RestructuringDate, string UserId, //44
                                           string UpdateAmortBalance, string OQUTLTSNo, //46 
                                           int AdvancePayment, int MiscTerms, //48
                                           double OriginalNETDAS, string CancellationTagging, //2024-06-13 KUNG ADVANCE PAYMENT OR RESTRUCTURING //50
                                           out string errorDesc, out string outQuotationDocEntry, //52
                                           out string outDPEntry, out string outMiscEntry //54
                                          )
        {
            errorDesc = "";
            try
            {

                string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];

                CashRegisterService cashRegister = new CashRegisterService();

                bool isSuccess = true;

                int PymtEntry1 = 0;
                int DPEntry1 = 0;
                int MiscEntry = 0;
                string RestructuringID = "";



                //2024-06-13 : CONSIDERED IF THE CANCELLATION IS FOR ADVANCE PAYMENT OR RESTRUCTURING (CancellationTagging)
                //isSuccess = CancelSAPDocuments(company, cashRegister, oldBlock, oldLot, oldPrjCode, newPrjCode, newBlock, newLot, DocNum, oSAPDB, SQEntry,
                //                                oldCardCode, DocDate, oldModel, oldFinancingScheme, oldProductType, oldReservationFee, oldDPAmount,
                //                                AmountDue, "CANCEL", RestructuringDate, out errorDesc, out RestructuringID);
                isSuccess = CancelSAPDocuments(company, cashRegister, oldBlock, oldLot, oldPrjCode, newPrjCode, newBlock, newLot, DocNum, oSAPDB, SQEntry,
                                                oldCardCode, DocDate, oldModel, oldFinancingScheme, oldProductType, oldReservationFee, oldDPAmount,
                                                AmountDue, CancellationTagging, RestructuringDate, out errorDesc, out RestructuringID);





                //################################
                //POSTING NEW DOCUMENTS TO SAP
                //################################



                if (isSuccess)
                {

                    int SapEntry = 0;
                    int DPEntry;
                    string SapCardCode = "";
                    string Message = "";
                    int JournalEntryNo;

                    string DPEntries = "";
                    string InvoiceEntries = "";

                    string qryForUpdates = "";

                    double _CashAmount = 0;
                    double _CheckAmount = 0;
                    double _CreditAmount = 0;
                    double _TransferAmount = 0;
                    double _OthersAmount = 0;



                    //GL ACCOUNT CODES
                    string CreditableWithholdingTaxAccount = ConfigSettings.CreditableWithholdingTaxAccount;
                    string ARAccount = ConfigSettings.ARClearingAccount;
                    string APOthersAccount = ConfigSettings.APOthersAccount;
                    string ARClearingAccount = ConfigSettings.ARClearingAccount;
                    string ContractReceivablesDeferredAccount = ConfigSettings.ContractReceivablesDeferredAccount;
                    string DepositFromCustomers = ConfigSettings.ClearingAccount;
                    string SalesCollectedAccount = ConfigSettings.SalesCollectedAccount;
                    string OutputVATAccount = ConfigSettings.OutputVATAccount;
                    string SalesUncollectedAccount = ConfigSettings.SalesUncollectedAccount;


                    string ControlAccount = "";

                    //GET DATA FROM ADDON QUOTATION
                    DataTable generalData = new DataTable();
                    generalData = ws.GetGeneralData(Convert.ToInt32(SQEntry), ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];


                    //GET TCP TOTAL AMOUNT PAID FROM QUOTATION ROWS
                    //DataTable dtTotalPaid1 = hana.GetData($@"SELECT SUM(IFNULL(""AmountPaid"",0)) ""TotalAmountPaid"" from QUT1 where ""DocEntry"" = {SQEntry} AND ""PaymentType"" <> 'MISC' AND IFNULL(""Cancelled"",'N') <> 'Y'", hana.GetConnection("SAOHana"));
                    //double TotalAmoundPaid1 = Convert.ToDouble(DataHelper.DataTableRet(dtTotalPaid1, 0, "TotalAmountPaid", "0"));
                    double TotalAmoundPaid1 = TotalNetPayments;





                    var ARReserveInvoiceCondition1 = ConfigSettings.ARReserveInvoiceCondition;

                    //CHECK IF TOTAL PAID AMOUNT IS LESS THAN OR EQUAL 25%

                    //if (TotalAmoundPaid1 < (newNETTCP * Convert.ToDouble(ARReserveInvoiceCondition1)))
                    //2023-06-02: USE ORIGINAL NETDAS INSTEAD OF NETTCP BALANCE
                    if (TotalAmoundPaid1 < (OriginalNETDAS * Convert.ToDouble(ARReserveInvoiceCondition1)))
                    {
                        ControlAccount = ARAccount;
                    }
                    else
                    {
                        ControlAccount = ContractReceivablesDeferredAccount;
                    }

                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    //CHECK FOR AR RESERVE INVOICE
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                    //GET PAYMENT SCHEME
                    //DataTable dtPaymentScheme = hana.GetData($@"SELECT ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{newFinancingScheme}'", hana.GetConnection("SAPHana"));
                    //string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "0").ToString();
                    string qryPaymentScheme = $@" SELECT TOP 1 ""U_PaymentType"" FROM OINV WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND 
                                                ""CANCELED"" = 'Y' AND ""isIns"" = 'Y' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured') ORDER BY ""DocEntry"" DESC";
                    DataTable dtPaymentScheme = hana.GetData(qryPaymentScheme, hana.GetConnection("SAPHana"));
                    string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PaymentType", "").ToString();


                    //GET PRODUCTTYPE AND TAX CLASSIFICATION
                    string ProdType = DataHelper.DataTableRet(generalData, 0, "ProductType", "");
                    string TaxClassification = DataHelper.DataTableRet(generalData, 0, "TaxClassification", "");



                    //GET LOCATION
                    string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{newPrjCode}'";
                    DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
                    string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
                    string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();









                    //** RESERVATION POSTING**//
                    #region RESERVATION POSTING

                    if (isSuccess)
                    {

                        //GET TAXDATE RES
                        string qryReservation = $@" SELECT TOP 1 ""DocDate"",""TaxDate"" FROM OQUT WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND 
                                                IFNULL(""U_IssueType"",'') = 'CANCELED' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Advance') ORDER BY ""DocEntry"" DESC";

                        DataTable dtReservation = hana.GetData(qryReservation, hana.GetConnection("SAPHana"));
                        // string ReservationDocDate = DataAccess.GetData(dtReservation, 0, "DocDate", "").ToString();

                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                        //string ReservationTaxDate = DataAccess.GetData(dtReservation, 0, "TaxDate", RestructuringDate).ToString();



                        if (isSuccess = cashRegister.CreateReservation(int.Parse(SQEntry),
                                                                   Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                   "RST",
                                                                   AmountDue, //4 Balance
                                                                   0, // Payment
                                                                   DocNum, //6 DocNum
                                                                   "RESTRUCTURING",
                                                                   DPTerms,  //8
                                                                   MiscTerms,
                                                                   out SapEntry,
                                                                   out DPEntry, //10
                                                                   out SapCardCode,
                                                                   out errorDesc, //12

                                                                   newBlock,
                                                                   newLot,
                                                                   newPrjCode,
                                                                   newModel,
                                                                   newFinancingScheme,
                                                                   newProductType,
                                                                   newReservationFee,
                                                                   TCPDownpayment,
                                                                   TCPLoanableBalance,
                                                                   newNETTCP,
                                                                   Vatable,
                                                                   DiscountAmount,
                                                                   MiscAmount,
                                                                   MiscMonthly,

                                                                   BatchNum,
                                                                   EmpName,
                                                                   SoldWithAdjacentLot,
                                                                   AdjacentLotQuotationNo,

                                                                   newCardCode,

                                                                   //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                   //Convert.ToDateTime(ReservationTaxDate).ToString("yyyy-MM-dd") //2
                                                                   Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd") //2
                                                                   )) //10
                        {
                            //update forwarded status and sap entry
                            qryForUpdates = $@"UPDATE ""OQUT"" SET ""SAPDocEntry"" = '{SapEntry}' WHERE ""DocEntry"" = '{SQEntry}';";
                            //qryForUpdates = $@"UPDATE ""QUT1"" SET ""SapDocEntry"" = '{DPEntry}', ""AmountPaid"" = 
                            //        '{double.Parse(paymentAmount)}'{(double.Parse(_row.Cells[10].Text) == 0 ? @",""LineStatus"" = 'C'" : "")} WHERE ""DocEntry"" = '{SQEntry}' 
                            ////        AND ""Terms"" = '{Terms}' AND ""PaymentType"" = '{paymentdue_type}'";
                            //qryForUpdates += $@" INSERT INTO QUT13 VALUES ({SQEntry}, 0, '', 0, 'RES', 1, {newReservationFee}, 0, 0, '{Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd")}', 
                            //                                                       '{Location}', {UserId}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
                            DPEntries = $"{DPEntries}{DPEntry};";


                        }
                        else
                        {

                            isSuccess = false;
                        }
                    }
                    #endregion








                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                    //SALES ORDER POSTING
                    //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                    #region SALES ORDER POSTING

                    int SOEntry = 0;
                    InvoiceEntries = "";

                    if (isSuccess)
                    {

                        //GET TAXDATE SALESORDER
                        string qrySalesOrder = $@" SELECT TOP 1 ""DocDate"",""TaxDate"" FROM ORDR WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND 
                                                ""CANCELED"" = 'Y' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured', 'Advance') ORDER BY ""DocEntry"" DESC";

                        DataTable dtSalesOrder = hana.GetData(qrySalesOrder, hana.GetConnection("SAPHana"));
                        // string ReservationDocDate = DataAccess.GetData(dtReservation, 0, "DocDate", "").ToString();

                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                        //string SalesOrderTaxDate = DataAccess.GetData(dtSalesOrder, 0, "TaxDate", RestructuringDate).ToString();


                        if (isSuccess = cashRegister.CreateSalesOrder(null,
                                                                      SapCardCode, //2
                                                                      Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                      newPrjCode, //4
                                                                      newBlock,
                                                                      newLot, //6
                                                                      0,
                                                                      0, //8
                                                                      SapEntry,
                                                                      "LB", //10
                                                                      DocNum,
                                                                      LOI,
                                                                      //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                      //Convert.ToDateTime(SalesOrderTaxDate).ToString("yyyyMMdd"),
                                                                      Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                      Convert.ToInt32(SQEntry),
                                                                      out SOEntry, //12
                                                                      out errorDesc))
                        {
                            if (isSuccess)
                            {



                                double temp_TotalNetPayments = TotalNetPayments;
                                string ORNumber = DocNum + Convert.ToDateTime(DocDate).ToString("yyyyMMdd") + "-1";

                                //CHECK IF TRANSACTION HAS AR RESERVE INVOICE
                                //if (TotalAmoundPaid1 >= (newNETTCP * Convert.ToDouble(ARReserveInvoiceCondition1)))
                                //2023-06-02: USE ORIGINAL NETDAS INSTEAD OF NETTCP BALANCE
                                if (TotalAmoundPaid1 >= (OriginalNETDAS * Convert.ToDouble(ARReserveInvoiceCondition1)))
                                {
                                    temp_TotalNetPayments = newDPAmount + newReservationFee;
                                }






                                //** MISCELLANEOUS POSTING**//
                                #region MISCELLANEOUS POSTING

                                //check if misc payment exists
                                if (TotalNetPaymentsMisc > 0)
                                {

                                    //GET TAXDATE OINV
                                    string qryInvoice = $@" SELECT TOP 1 ""DocDate"",""TaxDate"" FROM OINV WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND 
                                                ""CANCELED"" = 'Y' AND IFNULL(""U_Type"",'') = 'MISC' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ORDER BY ""DocEntry""";

                                    DataTable dtInvoice = hana.GetData(qryInvoice, hana.GetConnection("SAPHana"));
                                    // string ReservationDocDate = DataAccess.GetData(dtReservation, 0, "DocDate", "").ToString();

                                    //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                    //string InvoiceTaxDate = DataAccess.GetData(dtInvoice, 0, "TaxDate", RestructuringDate).ToString();


                                    if (isSuccess)
                                    {
                                        if (isSuccess = cashRegister.CreateARInvoice(null,
                                                                                    SapCardCode, //2
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                    TotalNetPaymentsMisc, //4
                                                                                    "MISC",
                                                                                    "QryGroup1", //6    
                                                                                    "1",
                                                                                    newPrjCode, //8
                                                                                    newBlock,
                                                                                    newLot, //10
                                                                                    SOEntry,
                                                                                    "Y", //12
                                                                                         //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                         //Convert.ToDateTime(InvoiceTaxDate).ToString("yyyyMMdd"),
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                    MiscTerms,
                                                                                    out MiscEntry,
                                                                                    out Message)) //14
                                        {
                                            outMiscEntry = MiscEntry.ToString();

                                            if (isSuccess)
                                            {
                                                //DataTable dtGetRestructureCode = hana.GetData($@"SELECT TOP 1 ""U_RestructCode"" FROM ORCT A WHERE 
                                                //                                            IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured') ORDER BY ""U_RestructCode"" DESC", hana.GetConnection("SAPHana"));
                                                //string GetRestructureCode = DataAccess.GetData(dtGetRestructureCode, 0, "U_RestructCode", "").ToString();

                                                //GET EXISTING INCOMING PAYMENT
                                                string qryExistingPayments = $@"SELECT A.* FROM ORCT A  WHERE ""U_Project"" = '{oldPrjCode}' AND 
                                                                          A.""U_BlockNo"" = '{oldBlock}' and A.""U_LotNo"" = '{oldLot}' AND
                                                                          IFNULL(A.""U_RestructCode"",'') = '{RestructuringID}' AND
                                                                          IFNULL(A.""U_PaymentType"",'') = 'MISC' AND 
                                                                          IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                                DataTable dtExistingPayments = hana.GetData(qryExistingPayments, hana.GetConnection("SAPHana"));

                                                foreach (DataRow dr in dtExistingPayments.Rows)
                                                {

                                                    string PaymentDocDate = dr["DocDate"].ToString();
                                                    string PaymentRemarks = dr["U_Remarks"].ToString();
                                                    string PaymentORNo = dr["U_ORNo"].ToString();
                                                    string PaymentORDate = dr["U_ORDate"].ToString();
                                                    string PaymentPRNo = dr["U_PRNo"].ToString();
                                                    string PaymentPRDate = dr["U_PRDate"].ToString();
                                                    string PaymentARNo = dr["U_ARNo"].ToString();
                                                    string PaymentARDate = dr["U_ARDate"].ToString();
                                                    double PaymentDocTotal = Convert.ToDouble(dr["DocTotal"]);
                                                    int PaymentPaymentOrder = Convert.ToInt32(dr["U_PaymentOrder"]);
                                                    int PaymentDocEntry = Convert.ToInt32(dr["DocEntry"]);
                                                    string InterBankDate = dr["TrsfrDate"].ToString();
                                                    string InterBankAcct = dr["TrsfrAcct"].ToString();
                                                    string PaymentDocNum = dr["DocNum"].ToString();

                                                    int PymtEntry0 = 0;
                                                    string AccountNo = "";


                                                    string withDeposit = "N";

                                                    //CHECK IF CANCELLED DEPOSIT
                                                    string qryCancelledDeposit = $@"SELECT 
                                                                                    A.""DocNum"", E.""Canceled"",E.""CnclDps""
                                                                                FROM 
                                                                                    ORCT A INNER JOIN 
                                                                                    rct3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN 
                                                                                    ocrh C ON B.""CrCardNum"" = C.""CrdCardNum"" AND B.""DocNum"" = C.""RctAbs"" INNER JOIN 
                                                                                    ODPS E ON E.""DeposId"" = C.""DepNum"" 
                                                                                WHERE 
	                                                                                A.""DocNum"" = {PaymentDocNum} AND
	                                                                                E.""Canceled"" = 'N' AND
	                                                                                E.""CnclDps"" <> -1 AND
                                                                                    IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                                    DataTable dtCancelledDeposit = hana.GetData(qryCancelledDeposit, hana.GetConnection("SAPHana"));

                                                    if (DataAccess.Exist(dtCancelledDeposit))
                                                    {
                                                        withDeposit = "Y";
                                                    }

                                                    string qryGetSAPCardCodeOfOldCardCode = $@"select ""CardCode"" from ocrd where ""U_DreamsCustCode"" = '{oldCardCode}' ";
                                                    DataTable dtGetSAPCardCodeOfOldCardCode = hana.GetData(qryGetSAPCardCodeOfOldCardCode, hana.GetConnection("SAPHana"));
                                                    string GetSAPCardCodeOfOldCardCode = DataHelper.DataTableRet(dtGetSAPCardCodeOfOldCardCode, 0, "CardCode", "0");

                                                    // AR RESERVE INVOICE OF MISCELLANEOUS
                                                    double ARInvoiceMiscSumApplied = 0;
                                                    string qryARInvoiceMiscSumApplied = $@" SELECT 
                                                                                                 B.""SumApplied""
                                                                                              FROM 
                                                                                                OINV A INNER JOIN
                                                                                                RCT2 B ON A.""DocEntry"" = B.""DocEntry"" INNER JOIN
                                                                                                ORCT C ON B.""DocNum"" = C.""DocEntry""
                                                                                              WHERE
                                                                                                  A.""Project"" = '{oldPrjCode}' AND
                                                                                                  A.""U_BlockNo"" = '{oldBlock}' AND
                                                                                                  A.""U_LotNo"" = '{oldLot}' AND
                                                                                                  A.""CardCode"" = '{GetSAPCardCodeOfOldCardCode}' AND
                                                                                                  A.""CANCELED"" <> 'C' AND 
	                                                                                              C.""U_ORNo"" = '{PaymentORNo}' AND
                                                                                                  C.""U_ARNo"" = '{PaymentARNo}' AND
                                                                                                  C.""U_PRNo"" = '{PaymentPRNo}' AND 
                                                                                                  IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured','Advance') AND
                                                                                                  IFNULL(A.""U_RestructureTag"",'OLD') <> 'NEW' AND
                                                                                              A.""U_Type"" = 'MISC'
                                                                                            ";
                                                    DataTable dtARInvoiceMiscSumApplied = hana.GetData(qryARInvoiceMiscSumApplied, hana.GetConnection("SAPHana"));
                                                    if (dtARInvoiceMiscSumApplied.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow rowARInvoiceMiscSumApplied in dtARInvoiceMiscSumApplied.Rows)
                                                        {
                                                            ARInvoiceMiscSumApplied += double.Parse(rowARInvoiceMiscSumApplied["SumApplied"].ToString());
                                                        }
                                                    }


                                                    if (isSuccess = cashRegister.CreateIncomingPayment(SapCardCode,
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                                        PaymentORNo, //OR
                                                                                        PaymentRemarks, //4
                                                                                        0,
                                                                                        "", //6
                                                                                        MiscEntry.ToString(),
                                                                                        null, //8
                                                                                        PaymentORDate,
                                                                                        PaymentPRNo, //10
                                                                                        PaymentPRDate,
                                                                                        PaymentARNo, //12
                                                                                        PaymentARDate,
                                                                                        ARInvoiceMiscSumApplied, //14
                                                                                        0,
                                                                                        0, //16
                                                                                        DocNum,
                                                                                        PaymentPaymentOrder, //18
                                                                                        "LB",
                                                                                        0, //20
                                                                                        0,
                                                                                        UserCode, //22
                                                                                        newBlock,
                                                                                        newLot, //24
                                                                                        newPrjCode,
                                                                                        "N", //26
                                                                                        "Y",
                                                                                        PaymentDocEntry, //28
                                                                                        "MISC",
                                                                                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                        //Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd"), //2
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                                        withDeposit,
                                                                                        "",
                                                                                        null,
                                                                                        0,
                                                                                        MiscEntry,

                                                                                        //2023-09-21 : ADD SURCHARGE DATE ON POSTING
                                                                                        "",

                                                                                        //2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
                                                                                        CancellationTagging,

                                                                                        out PymtEntry0,
                                                                                        out errorDesc, //30
                                                                                        out AccountNo,
                                                                                        out _CashAmount, //32
                                                                                        out _CheckAmount,
                                                                                        out _CreditAmount, //34
                                                                                        out _TransferAmount,
                                                                                        out _OthersAmount //36
                                                                                        ))
                                                    {

                                                        //qryForUpdates += $@" INSERT INTO QUT13 VALUES ({SQEntry}, 0, '{PaymentORNo}', {PymtEntry0}, 'MISC', {PaymentPaymentOrder}, {PaymentDocTotal}, 0, 0, '{ Convert.ToDateTime(PaymentORDate).ToString("yyyyMMdd")}', 
                                                        //                         '{Location}',{UserId}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";

                                                        //if (UpdateAmortBalance == "YES")
                                                        //{
                                                        string receiptNo = string.IsNullOrWhiteSpace(PaymentORNo) ? (string.IsNullOrWhiteSpace(PaymentARNo) ? PaymentPRNo : PaymentARNo) : PaymentORNo;

                                                        qryForUpdates += $@" UPDATE QUT13 SET ""SAPDocEntry"" = {PymtEntry0} WHERE ""DocEntry"" = {SQEntry} AND ""ReceiptNo"" = '{receiptNo}' AND ""Location"" = '{Location}'; ";
                                                        //}

                                                        if (_CashAmount > 0)
                                                        {
                                                            if (!ws.PostQuotation(
                                                                         0,
                                                                         "Cash",
                                                                         int.Parse(SQEntry),
                                                                         PymtEntry0,
                                                                         //amount,
                                                                         _CashAmount,
                                                                         PaymentORNo,

                                                                        //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                        //  PaymentRemarks + " - Restructured",
                                                                        AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                         string.Empty, //8
                                                                        "0",
                                                                        string.Empty, //10
                                                                        string.Empty,
                                                                        string.Empty,//12
                                                                        string.Empty,
                                                                        string.Empty, //14
                                                                        string.Empty,
                                                                        string.Empty, //16
                                                                        string.Empty,
                                                                        string.Empty, //18
                                                                        string.Empty,
                                                                        string.Empty, //20
                                                                        string.Empty,
                                                                        string.Empty, //22
                                                                        0,
                                                                        string.Empty, //24
                                                                        string.Empty,
                                                                        AccountNo, //26
                                                                        Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                        PaymentARNo, //28
                                                                        Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                        PaymentPRNo, //30
                                                                        Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                        string.Empty, //32
                                                                        string.Empty,
                                                                        0, //34
                                                                        string.Empty,
                                                                        string.Empty, //36
                                                                        string.Empty,
                                                                        string.Empty, //38
                                                                        Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                                 ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_TransferAmount > 0)
                                                        {
                                                            //GET INTERBANK BANK 
                                                            string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                            DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                            string InterBankBank = DataAccess.GetData(dtGetTransferBank, 0, "BankCode", "").ToString();

                                                            if (!ws.PostQuotation(
                                                                               0,
                                                                               "Interbank", //2
                                                                               int.Parse(SQEntry),
                                                                               PymtEntry0, //4
                                                                                           //amount,
                                                                               _TransferAmount,
                                                                               PaymentORNo,

                                                                                //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                                //  PaymentRemarks + " - Restructured",
                                                                                AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                               Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                                               InterBankAcct,
                                                                               string.Empty, //10
                                                                               InterBankBank,
                                                                               string.Empty, //12
                                                                               string.Empty,
                                                                               string.Empty, //14
                                                                               string.Empty,
                                                                               string.Empty, //16
                                                                               string.Empty,
                                                                               string.Empty, //18
                                                                               string.Empty,
                                                                               string.Empty, //20
                                                                               string.Empty,
                                                                               string.Empty, //22
                                                                               0,
                                                                               string.Empty, //24
                                                                               string.Empty,
                                                                               AccountNo, //26
                                                                               Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                               PaymentARNo, //28
                                                                               Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                               PaymentPRNo, //30
                                                                               Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),
                                                                               string.Empty, //32
                                                                               string.Empty,
                                                                               0, //34
                                                                               string.Empty,
                                                                               string.Empty, //36
                                                                               string.Empty,
                                                                               string.Empty, //38
                                                                               Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")
                                                                                    ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_CheckAmount > 0)
                                                        {
                                                            //GET CREDIT CARD DETAILS FROM EXISTING 
                                                            string qryCheck = $@"SELECT DISTINCT B.""DueDate"",B.""CheckNum"",B.""BankCode"",B.""Branch"",B.""CheckAct""
                                                                              FROM  RCT1 B INNER JOIN OCHH C ON B.""CheckNum"" = C.""CheckNum""
                                                                              WHERE B.""DocNum"" = {PaymentDocEntry} AND C.""CheckKey"" NOT IN        
                                                                              (SELECT x.""CheckKey"" FROM DPS1 x INNER JOIN ODPS y ON x.""DepositId"" = y.""DeposId"" 
                                                                              WHERE y.""Canceled"" ='N' and y.""CnclDps"" = -1) ";
                                                            DataTable dtCheck = hana.GetData(qryCheck, hana.GetConnection("SAPHana"));

                                                            foreach (DataRow dr_ in dtCheck.Rows)
                                                            {

                                                                if (!ws.PostQuotation(
                                                                0,
                                                                "Check", //2    
                                                                int.Parse(SQEntry),
                                                                PymtEntry0, //4
                                                                _CheckAmount,
                                                                PaymentORNo, //6

                                                                //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                //  PaymentRemarks + " - Restructured",
                                                                AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                Convert.ToDateTime(dr_["DueDate"].ToString()).ToString("yyyy-MM-dd"), //8
                                                                dr_["CheckNum"].ToString(),
                                                                dr_["BankCode"].ToString(),
                                                                dr_["BankCode"].ToString(),
                                                                dr_["Branch"].ToString(),
                                                                dr_["CheckAct"].ToString(),
                                                                string.Empty, //14
                                                                string.Empty,
                                                                string.Empty, //16
                                                                string.Empty,
                                                                string.Empty, //18
                                                                string.Empty,
                                                                string.Empty, //20
                                                                string.Empty,
                                                                string.Empty, //22
                                                                0,
                                                                string.Empty, //24
                                                                string.Empty,
                                                                AccountNo, //26
                                                                Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                PaymentARNo, //28
                                                                Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                PaymentPRNo, //30
                                                                Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                string.Empty,
                                                                string.Empty,
                                                                0,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")
                                                                ))
                                                                {
                                                                    isSuccess = false;
                                                                }
                                                            }
                                                        }

                                                        if (_CreditAmount > 0)
                                                        {
                                                            //GET CREDIT CARD DETAILS FROM EXISTING 
                                                            DataTable dtCredit = hana.GetData($@"select * from rct3 where ""DocNum"" = {PaymentDocEntry}", hana.GetConnection("SAPHana"));
                                                            DataTable dtCreditAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString()}'", hana.GetConnection("SAPHana"));
                                                            DataTable dtPaymentTypeName = hana.GetData($@"select ""CrTypeName"" from ocrp where ""CrTypeCode"" = {DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString()}", hana.GetConnection("SAPHana"));

                                                            if (!ws.PostQuotation(
                                                               0,
                                                                "Credit", //2
                                                                int.Parse(SQEntry),
                                                                PymtEntry0, //4
                                                                _CreditAmount,
                                                                PaymentORNo, //6

                                                                //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                //  PaymentRemarks + " - Restructured",
                                                                AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                string.Empty, //8
                                                                "0",
                                                                string.Empty, //10
                                                                DataAccess.GetData(dtCredit, 0, "U_CCBrand", "0").ToString(),
                                                                string.Empty,//12
                                                                string.Empty,
                                                                DataAccess.GetData(dtCredit, 0, "CreditCard", "0").ToString(), //14
                                                                DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString(),
                                                                DataAccess.GetData(dtCreditAccountName, 0, "AcctName", "").ToString(), //16
                                                                DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(),
                                                                Convert.ToDateTime(DataAccess.GetData(dtCredit, 0, "CardValid", "")).ToString("yyyy-MM-dd"), //18
                                                                DataAccess.GetData(dtCredit, 0, "OwnerIdNum", "").ToString(),
                                                                DataAccess.GetData(dtCredit, 0, "OwnerPhone", "").ToString(), //20
                                                                DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString(),
                                                                DataAccess.GetData(dtPaymentTypeName, 0, "CrTypeName", "").ToString(), //22
                                                                int.Parse(DataAccess.GetData(dtCredit, 0, "NumOfPmnts", "0").ToString()),
                                                                DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(), //24
                                                                "S",
                                                                AccountNo, //26 
                                                                Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                PaymentARNo, //28
                                                                Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                PaymentPRNo, //30
                                                                Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                string.Empty,
                                                                string.Empty,
                                                                0,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                                ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_OthersAmount > 0)
                                                        {
                                                            //GET INTERBANK BANK 
                                                            string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                            DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                            string InterBankBank = DataAccess.GetData(dtPaymentScheme, 0, "BankCode", "").ToString();

                                                            DataTable dtTransferAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));

                                                            //2024-09-14: CHANGE BASIS OF GETTING OTHER PAYMENT MEANS FROM ACCOUNT CODE TO BANK CODE 
                                                            //DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""U_GLAccountCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));
                                                            DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""Code"" = '{dr["U_OthPayment"].ToString()}'", hana.GetConnection("SAPHana"));


                                                            if (!ws.PostQuotation(
                                                             0,
                                                             "Others", //2
                                                             int.Parse(SQEntry),
                                                             PymtEntry0, //4
                                                                         //amount,
                                                             _OthersAmount,
                                                             PaymentORNo, //6

                                                            //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                            //  PaymentRemarks + " - Restructured",
                                                            AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                             Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                             "0",

                                                             string.Empty, //10
                                                             string.Empty,
                                                             string.Empty, //12
                                                             string.Empty,

                                                             string.Empty, //14
                                                             string.Empty,
                                                             string.Empty, //16
                                                             string.Empty,
                                                             string.Empty, //18
                                                             string.Empty,
                                                             string.Empty, //20
                                                             string.Empty,
                                                             string.Empty, //22
                                                             0,
                                                             string.Empty, //24
                                                             string.Empty,
                                                             AccountNo, //26
                                                             Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                             PaymentARNo, //28
                                                             Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                             PaymentPRNo, //30
                                                             Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                             DataAccess.GetData(dtOthersPaymentTypeName, 0, "Code", "").ToString(),
                                                             DataAccess.GetData(dtOthersPaymentTypeName, 0, "Name", "").ToString(),
                                                             _OthersAmount,
                                                             DataAccess.GetData(dtExistingPayments, 0, "TrsfrRef", "0").ToString(),
                                                             DataAccess.GetData(dtExistingPayments, 0, "TrsfrAcct", "0").ToString(),
                                                             DataAccess.GetData(dtTransferAccountName, 0, "AcctName", "").ToString(),
                                                             Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"),
                                                             Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")
                                                             ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                    }

                                                }


                                            }
                                        }
                                    }

                                }

                                #endregion































                                string booked = "N";
                                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                //AR RESERVE INVOICE POSTING
                                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@


                                if (isSuccess)
                                {
                                    // WITH AR RESERVE INVOICE
                                    //if (TotalAmoundPaid1 >= (newNETTCP * Convert.ToDouble(ARReserveInvoiceCondition1)) && !string.IsNullOrWhiteSpace(OQUTLTSNo))
                                    //2023-06-02: USE ORIGINAL NETDAS INSTEAD OF NETTCP BALANCE
                                    if (TotalAmoundPaid1 >= (OriginalNETDAS * Convert.ToDouble(ARReserveInvoiceCondition1)) && !string.IsNullOrWhiteSpace(OQUTLTSNo))
                                    {
                                        #region AR RESERVE POSTING



                                        //GET TAXDATE AR RESERVE INVOICE
                                        string qryARRes = $@" SELECT TOP 1 ""DocDate"",""TaxDate"", ""DocEntry"" FROM OINV WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND 
                                                ""CANCELED"" = 'Y' AND ""isIns"" = 'Y' AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured','Advance') ORDER BY ""DocEntry"" DESC";

                                        DataTable dtARRes = hana.GetData(qryARRes, hana.GetConnection("SAPHana"));
                                        // string ReservationDocDate = DataAccess.GetData(dtReservation, 0, "DocDate", "").ToString();

                                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                        //string ARResTaxDate = DataAccess.GetData(dtARRes, 0, "TaxDate", RestructuringDate).ToString();

                                        string ARResDocEntry = DataAccess.GetData(dtARRes, 0, "DocEntry", RestructuringDate).ToString();

                                        //double DiscountAmount = double.Parse(DataHelper.DataTableRet(generalData, 0, "DiscountAmount", "0"));

                                        int ARReserveEntry = 0;
                                        if (isSuccess = cashRegister.CreateReserveInvoice(null,
                                                                                        SapCardCode, //2
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                        newPrjCode, //4
                                                                                        newBlock,
                                                                                        newLot, //6
                                                                                        0,
                                                                                        0, //8
                                                                                        SOEntry,
                                                                                        newNETTCP, //10
                                                                                        newFinancingScheme,
                                                                                        TaxClassification, //12
                                                                                        PaymentScheme,
                                                                                        TotalAmoundPaid1, //14
                                                                                        TotalNetPayments,
                                                                                        0, //16
                                                                                        System.Drawing.ColorTranslator.FromHtml("#FFFFFF"),
                                                                                        DocNum, //18
                                                                                        Vatable,
                                                                                        "0",
                                                                                        "LB",
                                                                                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                        //Convert.ToDateTime(ARResTaxDate).ToString("yyyyMMdd"),
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                        DiscountAmount,
                                                                                        0,

                                                                                        //2023-07-31: ADD RECEIPT NUMBER
                                                                                        "",

                                                                                        //2023-08-07 : ADD TAGGING OF RESTRUCTURING TYPE
                                                                                        AdvancePayment,

                                                                                        out ARReserveEntry,
                                                                                        out errorDesc)) //20
                                        {
                                            qryForUpdates += $@" UPDATE ""OQUT"" SET ""BookStatus"" = 'Y' WHERE ""DocEntry"" = '{SQEntry}'; ";
                                            booked = "Y";
                                            //InvoiceEntries = $"{InvoiceEntries}{ARReserveEntry};";

                                            //UPDATE THE AR RESERVE INVOICE WITH RESTRUCTURE TAG = OLD
                                            string qryUpdateRestructureTag = $@"UPDATE OINV SET ""U_RestructureTag"" = '' WHERE ""DocEntry"" = {ARResDocEntry} AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                            hana.Execute(qryUpdateRestructureTag, hana.GetConnection("SAPHana"));

                                            if (PaymentScheme == "Installment")
                                            {
                                                double amount = (newNETTCP - oldNetTCP) / 1.12;

                                                if (newNETTCP > oldNetTCP)
                                                {
                                                    //POST ON JOURNAL ENTRY 
                                                    if (isSuccess = cashRegister.CreateJournalEntry(null, newPrjCode, SapCardCode, amount, SOEntry, newFinancingScheme, TaxClassification,
                                                                            newBlock, newLot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedAccount, "Restructure1", DocNum, "", "", "Y", Convert.ToDateTime(DocDate).ToString("yyyyMMdd"), "RN>O", out JournalEntryNo, out errorDesc, ""))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                    }
                                                }
                                                else if (newNETTCP < oldNetTCP)
                                                {
                                                    //POST ON JOURNAL ENTRY 
                                                    if (isSuccess = cashRegister.CreateJournalEntry(null, newPrjCode, SapCardCode, amount, SOEntry, newFinancingScheme, TaxClassification,
                                                                            newBlock, newLot, SalesUncollectedAccount, SalesCollectedAccount, OutputVATAccount, "Restructure2", DocNum, "", "", "Y", Convert.ToDateTime(DocDate).ToString("yyyyMMdd"), "RN<O", out JournalEntryNo, out errorDesc, ""))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                    }
                                                }
                                            }





                                        }



                                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                        //INCOMING PAYMENT POSTING
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

                                        if (isSuccess)
                                        {
                                            string AccountNo = "";
                                            if (booked == "Y")
                                            {
                                                if (temp_TotalNetPayments <= 0)
                                                {

                                                }


                                                //GET EXISTING INCOMING PAYMENT
                                                //DataTable dtGetRestructureCode = hana.GetData($@"SELECT TOP 1 ""U_RestructCode"" FROM ORCT A WHERE A.""PrjCode"" = '{oldPrjCode}' AND 
                                                //                                         A.""U_BlockNo"" = '{oldBlock}' AND A.""U_LotNo"" = '{oldLot}' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured')
                                                //                                        ORDER BY ""U_RestructCode"" DESC", hana.GetConnection("SAPHana"));
                                                //string GetRestructureCode = DataAccess.GetData(dtGetRestructureCode, 0, "U_RestructCode", "").ToString();

                                                //GET EXISTING INCOMING PAYMENT
                                                string qryExistingPayments = $@"SELECT * FROM ORCT A  WHERE ""U_Project"" = '{oldPrjCode}' AND 
                                                                            ""U_BlockNo"" = '{oldBlock}' and ""U_LotNo"" = '{oldLot}' AND
                                                                            IFNULL(""U_RestructCode"",'') = '{RestructuringID}'  AND
                                                                            IFNULL(""U_PaymentType"",'') = 'TCP' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance')";
                                                DataTable dtExistingPayments = hana.GetData(qryExistingPayments, hana.GetConnection("SAPHana"));




                                                foreach (DataRow dr in dtExistingPayments.Rows)
                                                {
                                                    //InvoiceEntries = $"{InvoiceEntries}{ARReserveEntry};";

                                                    string PaymentDocDate = dr["DocDate"].ToString();
                                                    string PaymentRemarks = dr["U_Remarks"].ToString();
                                                    string PaymentORNo = dr["U_ORNo"].ToString();
                                                    string PaymentORDate = dr["U_ORDate"].ToString();
                                                    string PaymentPRNo = dr["U_PRNo"].ToString();
                                                    string PaymentPRDate = dr["U_PRDate"].ToString();
                                                    string PaymentARNo = dr["U_ARNo"].ToString();
                                                    string PaymentARDate = dr["U_ARDate"].ToString();
                                                    double PaymentDocTotal = Convert.ToDouble(dr["DocTotal"]);
                                                    int PaymentPaymentOrder = Convert.ToInt32(dr["U_PaymentOrder"]);
                                                    int PaymentDocEntry = Convert.ToInt32(dr["DocEntry"]);
                                                    string InterBankDate = dr["TrsfrDate"].ToString();
                                                    string InterBankAcct = dr["TrsfrAcct"].ToString();
                                                    string PaymentDocNum = dr["DocNum"].ToString();

                                                    string withDeposit = "N";

                                                    //CHECK IF CANCELLED DEPOSIT
                                                    string qryCancelledDeposit = $@"SELECT 
                                                                                    A.""DocNum"", E.""Canceled"",E.""CnclDps""
                                                                                FROM 
                                                                                    ORCT A INNER JOIN 
                                                                                    rct3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN 
                                                                                    ocrh C ON B.""CrCardNum"" = C.""CrdCardNum"" AND B.""DocNum"" = C.""RctAbs"" INNER JOIN 
                                                                                    ODPS E ON E.""DeposId"" = C.""DepNum"" 
                                                                                WHERE 
	                                                                                A.""DocNum"" = {PaymentDocNum} AND
	                                                                                E.""Canceled"" = 'N' AND
	                                                                                E.""CnclDps"" <> -1 AND 
                                                                                    IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                                    DataTable dtCancelledDeposit = hana.GetData(qryCancelledDeposit, hana.GetConnection("SAPHana"));

                                                    if (DataAccess.Exist(dtCancelledDeposit))
                                                    {
                                                        withDeposit = "Y";
                                                    }






                                                    //GET ALL STANDALONE INVOICES EXCLUDING AR RESERVE INVOICE
                                                    int InvoiceEntry = 0;
                                                    string qryGetSAPCardCodeOfOldCardCode = $@"select ""CardCode"" from ocrd where ""U_DreamsCustCode"" = '{oldCardCode}' ";
                                                    DataTable dtGetSAPCardCodeOfOldCardCode = hana.GetData(qryGetSAPCardCodeOfOldCardCode, hana.GetConnection("SAPHana"));
                                                    string GetSAPCardCodeOfOldCardCode = DataHelper.DataTableRet(dtGetSAPCardCodeOfOldCardCode, 0, "CardCode", "0");

                                                    string qryStandaloneInvoices = $@"SELECT 
                                                                          A.""DocEntry"", A.""DocTotal"",A.""U_HouseModel"", A.""U_ReservationAmount"", 
                                                                          ""U_DPAmount"", A.""U_LBAmount"", A.""U_TransactionType"",
                                                                          A.""U_Waive"",  A.""U_Partial"", A.""U_PaymentOrder"", A.""U_Type""
                                                                      FROM 
                                                                        OINV A INNER JOIN
                                                                        RCT2 B ON A.""DocEntry"" = B.""DocEntry"" INNER JOIN
                                                                        ORCT C ON B.""DocNum"" = C.""DocEntry""
                                                                      WHERE
                                                                          A.""Project"" = '{oldPrjCode}' AND
                                                                          A.""U_BlockNo"" = '{oldBlock}' AND
                                                                          A.""U_LotNo"" = '{oldLot}' AND
                                                                          A.""CardCode"" = '{GetSAPCardCodeOfOldCardCode}' AND
                                                                          A.""CANCELED"" <> 'C' AND
                                                                          A.""U_TransactionType"" IN('RE - SUR', 'INT INC', 'IP&S') AND
                                                                          C.""U_ORNo"" = '{PaymentORNo}' AND
                                                                          C.""U_ARNo"" = '{PaymentARNo}' AND
                                                                          C.""U_PRNo"" = '{PaymentPRNo}' AND
                                                                          A.""isIns"" = 'N' AND
                                                                          IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured','Advance') AND
                                                                          IFNULL(A.""U_RestructureTag"",'NEW') = 'NEW'  ";


                                                    DataTable dtStandaloneInvoices = hana.GetData(qryStandaloneInvoices, hana.GetConnection("SAPHana"));
                                                    if (dtStandaloneInvoices.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow drInvoices in dtStandaloneInvoices.Rows)
                                                        {

                                                            int InvoiceDocEntry = int.Parse(drInvoices["DocEntry"].ToString());
                                                            double InvoiceDocTotal = double.Parse(drInvoices["DocTotal"].ToString());
                                                            string InvoiceHouseModel = drInvoices["U_HouseModel"].ToString();
                                                            double InvoiceReservationAmount = double.Parse(drInvoices["U_ReservationAmount"].ToString());
                                                            double InvoiceDPAmount = double.Parse(drInvoices["U_DPAmount"].ToString());
                                                            double InvoiceLBAmount = double.Parse(drInvoices["U_LBAmount"].ToString());
                                                            string InvoiceWaive = drInvoices["U_Waive"].ToString();
                                                            string InvoicePartial = drInvoices["U_Partial"].ToString();
                                                            string InvoicePaymentOrder = drInvoices["U_PaymentOrder"].ToString();
                                                            string InvoiceType = drInvoices["U_Type"].ToString();

                                                            string TransType = "";
                                                            string InvoiceTransType = drInvoices["U_TransactionType"].ToString();
                                                            if (InvoiceTransType == "RE - SUR")
                                                            {
                                                                TransType = "Surcharge";
                                                            }
                                                            else if (InvoiceTransType == "INT INC")
                                                            {
                                                                TransType = "Interest";
                                                            }
                                                            else if (InvoiceTransType == "IP&S")
                                                            {
                                                                TransType = "IPS";
                                                            }


                                                            if (InvoiceDocTotal > 0)
                                                            {

                                                                //REECREATE INVOICE FOR SURCHARGE, INTEREST AND IP&S
                                                                if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
                                                                                  SapCardCode, //2
                                                                                  Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                  InvoiceDocTotal, //4
                                                                                  InvoiceType,
                                                                                  InvoicePaymentOrder, //6
                                                                                  oldPrjCode,
                                                                                  oldBlock, //8
                                                                                  oldLot,
                                                                                  InvoiceHouseModel, //10
                                                                                  oldFinancingScheme,
                                                                                  oldProductType, //12
                                                                                  InvoiceReservationAmount,
                                                                                  InvoiceDPAmount, //14
                                                                                  InvoiceLBAmount,
                                                                                  TransType, //16
                                                                                  DocNum,
                                                                                   ConfigSettings.ExcessControlAccount, //18
                                                                                  Vatable,
                                                                                  0, //20
                                                                                  InvoicePartial,
                                                                                  InvoiceWaive,
                                                                                  //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                  //Convert.ToDateTime(ARResTaxDate).ToString("yyyyMMdd"),
                                                                                  Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                  out InvoiceEntry,
                                                                                  out Message)) //22
                                                                {


                                                                    //UPDATE THE INVOICE WITH RESTRUCTURE TAG = OLD
                                                                    string qryUpdateRestructureTag = $@"UPDATE OINV SET ""U_RestructureTag"" = 'OLD' WHERE ""DocEntry"" = {InvoiceDocEntry} AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                                                    hana.Execute(qryUpdateRestructureTag, hana.GetConnection("SAPHana"));

                                                                    if (AdvancePayment == 0)
                                                                    {
                                                                        int tagExistEntry = 0;
                                                                        foreach (var docEntry in Convert.ToString(InvoiceEntry).Split(';'))
                                                                        {
                                                                            if (docEntry != "")
                                                                            {
                                                                                if (docEntry == Convert.ToString(InvoiceEntry))
                                                                                {
                                                                                    tagExistEntry++;
                                                                                }
                                                                            }
                                                                        }

                                                                        if (tagExistEntry == 0)
                                                                        {
                                                                            InvoiceEntries = $"{InvoiceEntries}{InvoiceEntry};";
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        InvoiceEntries = $"{InvoiceEntries}{InvoiceEntry};";
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }










                                                    // AR RESERVE INVOICE PART
                                                    double ARReserveInvoiceSumApplied = 0;
                                                    string qryARReserveInvoiceSumApplied = $@"SELECT 
                                                                                     B.""SumApplied""
                                                                                  FROM 
                                                                                    OINV A INNER JOIN
                                                                                    RCT2 B ON A.""DocEntry"" = B.""DocEntry"" INNER JOIN
                                                                                    ORCT C ON B.""DocNum"" = C.""DocEntry""
                                                                                  WHERE
                                                                                      A.""Project"" = '{oldPrjCode}' AND
                                                                                      A.""U_BlockNo"" = '{oldBlock}' AND
                                                                                      A.""U_LotNo"" = '{oldLot}' AND
                                                                                      A.""CardCode"" = '{GetSAPCardCodeOfOldCardCode}' AND
                                                                                      A.""CANCELED"" <> 'C' AND
                                                                                      --A.""U_TransactionType"" IN('RE - SUR', 'INT INC', 'IP&S') AND
                                                                                      C.""U_ORNo"" = '{PaymentORNo}' AND
                                                                                      C.""U_ARNo"" = '{PaymentARNo}' AND
                                                                                      C.""U_PRNo"" = '{PaymentPRNo}' AND
                                                                                      A.""isIns"" = 'Y' AND
                                                                                      IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured','Advance') 
                                                                                     AND IFNULL(A.""U_RestructureTag"",'') = ''  ";
                                                    DataTable dtARReserveInvoiceSumApplied = hana.GetData(qryARReserveInvoiceSumApplied, hana.GetConnection("SAPHana"));
                                                    if (dtARReserveInvoiceSumApplied.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow rowARReserveInvoiceSumApplied in dtARReserveInvoiceSumApplied.Rows)
                                                        {
                                                            ARReserveInvoiceSumApplied += double.Parse(rowARReserveInvoiceSumApplied["SumApplied"].ToString());
                                                        }
                                                    }






                                                    // AR DOWNPAYMENT INVOICES ATTACHED IN INCOMING PAYMENTS
                                                    double ARDPISumApplied = 0;
                                                    string qryARDPISumApplied = $@"SELECT 
                                                                                     B.""SumApplied""
                                                                                  FROM 
                                                                                    ODPI A INNER JOIN
                                                                                    RCT2 B ON A.""DocEntry"" = B.""DocEntry"" INNER JOIN
                                                                                    ORCT C ON B.""DocNum"" = C.""DocEntry""
                                                                                  WHERE
                                                                                      A.""Project"" = '{oldPrjCode}' AND
                                                                                      A.""U_BlockNo"" = '{oldBlock}' AND
                                                                                      A.""U_LotNo"" = '{oldLot}' AND
                                                                                      A.""CardCode"" = '{GetSAPCardCodeOfOldCardCode}' AND
                                                                                      A.""CANCELED"" <> 'C' AND
                                                                                      --A.""U_TransactionType"" IN('RE - SUR', 'INT INC', 'IP&S') AND
                                                                                      C.""U_ORNo"" = '{PaymentORNo}' AND
                                                                                      C.""U_ARNo"" = '{PaymentARNo}' AND
                                                                                      C.""U_PRNo"" = '{PaymentPRNo}' AND
                                                                                      --A.""isIns"" = 'Y' AND
                                                                                      IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed', 'Restructured','Advance') AND
                                                                                      IFNULL(A.""U_RestructureTag"",'NEW') = 'NEW'  ";
                                                    DataTable dtARDPISumApplied = hana.GetData(qryARDPISumApplied, hana.GetConnection("SAPHana"));
                                                    if (dtARDPISumApplied.Rows.Count > 0)
                                                    {
                                                        foreach (DataRow rowARDPISumApplied in dtARDPISumApplied.Rows)
                                                        {
                                                            ARDPISumApplied += double.Parse(rowARDPISumApplied["SumApplied"].ToString());
                                                        }
                                                    }




                                                    if (isSuccess = cashRegister.CreateIncomingPayment(SapCardCode,
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                                    PaymentORNo, //OR
                                                                                    PaymentRemarks, //4
                                                                                    0,
                                                                                    "", //6
                                                                                    InvoiceEntries,
                                                                                    null, //8
                                                                                    PaymentORDate,
                                                                                    PaymentPRNo, //10
                                                                                    PaymentPRDate,
                                                                                    PaymentARNo, //12
                                                                                    PaymentARDate,
                                                                                    ARReserveInvoiceSumApplied + ARDPISumApplied, //14 //AR RESERVE INVOICE TOTAL SUM APPLIED
                                                                                    0,
                                                                                    0, //16
                                                                                    DocNum,
                                                                                    PaymentPaymentOrder, //18
                                                                                    "LB",
                                                                                    0, //20
                                                                                    temp_TotalNetPayments + newReservationFee,
                                                                                    UserCode,
                                                                                    newBlock,
                                                                                    newLot,
                                                                                    newPrjCode,
                                                                                    "N",
                                                                                    "Y",
                                                                                    PaymentDocEntry,
                                                                                    "TCP",
                                                                                    //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                    //Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd"), //2
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                                    withDeposit,
                                                                                    "",
                                                                                    null,
                                                                                    ARReserveEntry,
                                                                                    0,

                                                                                    //2023-09-21 : ADD SURCHARGE DATE ON POSTING
                                                                                    "",

                                                                                    //2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
                                                                                    CancellationTagging,

                                                                                    out PymtEntry1, //22
                                                                                    out errorDesc,
                                                                                    out AccountNo, //24
                                                                                    out _CashAmount,
                                                                                    out _CheckAmount,
                                                                                    out _CreditAmount,
                                                                                    out _TransferAmount,
                                                                                    out _OthersAmount
                                                                                    ))
                                                    {
                                                        //if (UpdateAmortBalance == "YES")
                                                        //{
                                                        string receiptNo = string.IsNullOrWhiteSpace(PaymentORNo) ? (string.IsNullOrWhiteSpace(PaymentARNo) ? PaymentPRNo : PaymentARNo) : PaymentORNo;

                                                        qryForUpdates += $@" UPDATE QUT13 SET ""SAPDocEntry"" = {PymtEntry1} WHERE ""DocEntry"" = {SQEntry} AND ""ReceiptNo"" = '{receiptNo}' AND ""Location"" = '{Location}'; ";

                                                        InvoiceEntries = "";


                                                        //}

                                                        if (_CashAmount > 0)
                                                        {
                                                            if (!ws.PostQuotation(
                                                                         0,
                                                                         "Cash",
                                                                         int.Parse(SQEntry),
                                                                         PymtEntry1,
                                                                         //amount,
                                                                         _CashAmount,
                                                                         PaymentORNo,

                                                                        //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                        //  PaymentRemarks + " - Restructured",
                                                                        AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                         string.Empty, //8
                                                                        "0",
                                                                        string.Empty, //10
                                                                        string.Empty,
                                                                        string.Empty,//12
                                                                        string.Empty,
                                                                        string.Empty, //14
                                                                        string.Empty,
                                                                        string.Empty, //16
                                                                        string.Empty,
                                                                        string.Empty, //18
                                                                        string.Empty,
                                                                        string.Empty, //20
                                                                        string.Empty,
                                                                        string.Empty, //22
                                                                        0,
                                                                        string.Empty, //24
                                                                        string.Empty,
                                                                        AccountNo, //26
                                                                        Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                        PaymentARNo, //28
                                                                        Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                        PaymentPRNo, //30
                                                                        Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                        string.Empty, //32
                                                                        string.Empty,
                                                                        0, //34
                                                                        string.Empty,
                                                                        string.Empty, //36
                                                                        string.Empty,
                                                                        string.Empty, //38
                                                                        Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd") //2

                                                                 ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_TransferAmount > 0)
                                                        {
                                                            //GET INTERBANK BANK 
                                                            string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                            DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                            string InterBankBank = DataAccess.GetData(dtPaymentScheme, 0, "BankCode", "").ToString();

                                                            if (!ws.PostQuotation(
                                                                               0,
                                                                               "Interbank", //2
                                                                               int.Parse(SQEntry),
                                                                               PymtEntry1, //4
                                                                                           //amount,
                                                                               _TransferAmount,
                                                                               PaymentORNo,

                                                                               //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                               //  PaymentRemarks + " - Restructured",
                                                                               AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                               Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                                               InterBankAcct,
                                                                               string.Empty, //10
                                                                               InterBankBank,
                                                                               string.Empty, //12
                                                                               string.Empty,
                                                                               string.Empty, //14
                                                                               string.Empty,
                                                                               string.Empty, //16
                                                                               string.Empty,
                                                                               string.Empty, //18
                                                                               string.Empty,
                                                                               string.Empty, //20
                                                                               string.Empty,
                                                                               string.Empty, //22
                                                                               0,
                                                                               string.Empty, //24
                                                                               string.Empty,
                                                                               AccountNo, //26
                                                                               Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                               PaymentARNo, //28
                                                                               Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                               PaymentPRNo, //30
                                                                               Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),
                                                                               string.Empty, //32
                                                                               string.Empty,
                                                                               0, //34
                                                                               string.Empty,
                                                                               string.Empty, //36
                                                                               string.Empty,
                                                                               string.Empty, //38
                                                                               Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd") //2

                                                                                    ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_CheckAmount > 0)
                                                        {

                                                            //GET CREDIT CARD DETAILS FROM EXISTING 
                                                            string qryCheck = $@"SELECT DISTINCT B.""DueDate"",B.""CheckNum"",B.""BankCode"",B.""Branch"",B.""CheckAct"" FROM  RCT1 B INNER JOIN OCHH C ON B.""CheckNum"" = C.""CheckNum""
                                                                          WHERE B.""DocNum"" = {PaymentDocEntry} AND C.""CheckKey"" NOT IN        
                                                                          (SELECT x.""CheckKey"" FROM DPS1 x INNER JOIN ODPS y ON x.""DepositId"" = y.""DeposId"" 
                                                                          WHERE y.""Canceled"" ='N' and y.""CnclDps"" = -1) ";
                                                            DataTable dtCheck = hana.GetData(qryCheck, hana.GetConnection("SAPHana"));

                                                            foreach (DataRow dr_ in dtCheck.Rows)
                                                            {

                                                                if (!ws.PostQuotation(
                                                                0,
                                                                "Check", //2    
                                                                int.Parse(SQEntry),
                                                                PymtEntry1, //4
                                                                _CheckAmount,
                                                                PaymentORNo, //6

                                                                //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                //  PaymentRemarks + " - Restructured",
                                                                AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                Convert.ToDateTime(dr_["DueDate"].ToString()).ToString("yyyy-MM-dd"), //8
                                                                dr_["CheckNum"].ToString(),
                                                                dr_["BankCode"].ToString(),
                                                                dr_["BankCode"].ToString(),
                                                                dr_["Branch"].ToString(),
                                                                dr_["CheckAct"].ToString(),
                                                                //DataAccess.GetData(dtCheck, 0, "CheckNum", "0").ToString(),
                                                                //DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString(), //10
                                                                //DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString(),
                                                                //DataAccess.GetData(dtCheck, 0, "Branch", "").ToString(), //12
                                                                //DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString(),
                                                                string.Empty, //14
                                                                string.Empty,
                                                                string.Empty, //16
                                                                string.Empty,
                                                                string.Empty, //18
                                                                string.Empty,
                                                                string.Empty, //20
                                                                string.Empty,
                                                                string.Empty, //22
                                                                0,
                                                                string.Empty, //24
                                                                string.Empty,
                                                                AccountNo, //26
                                                                Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                PaymentARNo, //28
                                                                Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                PaymentPRNo, //30
                                                                Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                string.Empty,
                                                                string.Empty,
                                                                0,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd") //2

                                                                ))
                                                                {
                                                                    isSuccess = false;
                                                                }
                                                            }
                                                        }


                                                        if (_CreditAmount > 0)
                                                        {
                                                            //GET CREDIT CARD DETAILS FROM EXISTING 
                                                            DataTable dtCredit = hana.GetData($@"select * from rct3 where ""DocNum"" = {PaymentDocEntry}", hana.GetConnection("SAPHana"));
                                                            DataTable dtCreditAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString()}'", hana.GetConnection("SAPHana"));
                                                            DataTable dtPaymentTypeName = hana.GetData($@"select ""CrTypeName"" from ocrp where ""CrTypeCode"" = {DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString()}", hana.GetConnection("SAPHana"));

                                                            if (!ws.PostQuotation(
                                                               0,
                                                                "Credit", //2
                                                                int.Parse(SQEntry),
                                                                PymtEntry1, //4
                                                                _CreditAmount,
                                                                PaymentORNo, //6

                                                                //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                //  PaymentRemarks + " - Restructured",
                                                                AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                string.Empty, //8
                                                                "0",
                                                                string.Empty, //10
                                                                DataAccess.GetData(dtCredit, 0, "U_CCBrand", "0").ToString(),
                                                                string.Empty,//12
                                                                string.Empty,
                                                                DataAccess.GetData(dtCredit, 0, "CreditCard", "0").ToString(), //14
                                                                DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString(),
                                                                DataAccess.GetData(dtCreditAccountName, 0, "AcctName", "").ToString(), //16
                                                                DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(),
                                                                Convert.ToDateTime(DataAccess.GetData(dtCredit, 0, "CardValid", "")).ToString("yyyy-MM-dd"), //18
                                                                DataAccess.GetData(dtCredit, 0, "OwnerIdNum", "").ToString(),
                                                                DataAccess.GetData(dtCredit, 0, "OwnerPhone", "").ToString(), //20
                                                                DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString(),
                                                                DataAccess.GetData(dtPaymentTypeName, 0, "CrTypeName", "").ToString(), //22
                                                                int.Parse(DataAccess.GetData(dtCredit, 0, "NumOfPmnts", "0").ToString()),
                                                                DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(), //24
                                                                "S",
                                                                AccountNo, //26 
                                                                Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                PaymentARNo, //28
                                                                Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                PaymentPRNo, //30
                                                                Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                string.Empty,
                                                                string.Empty,
                                                                0,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                string.Empty,
                                                                Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd") //2

                                                                ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }

                                                        if (_OthersAmount > 0)
                                                        {
                                                            //GET INTERBANK BANK 
                                                            string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                            DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                            string InterBankBank = DataAccess.GetData(dtPaymentScheme, 0, "BankCode", "").ToString();

                                                            DataTable dtTransferAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));

                                                            //2024-09-14: CHANGE BASIS OF GETTING OTHER PAYMENT MEANS FROM ACCOUNT CODE TO BANK CODE 
                                                            //DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""U_GLAccountCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));
                                                            DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""Code"" = '{dr["U_OthPayment"].ToString()}'", hana.GetConnection("SAPHana"));


                                                            if (!ws.PostQuotation(
                                                             0,
                                                             "Others", //2
                                                             int.Parse(SQEntry),
                                                             PymtEntry1, //4
                                                                         //amount,
                                                             _OthersAmount,
                                                             PaymentORNo, //6

                                                            //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                            //  PaymentRemarks + " - Restructured",
                                                            AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                             Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                             "0",

                                                             string.Empty, //10
                                                             string.Empty,
                                                             string.Empty, //12
                                                             string.Empty,

                                                             string.Empty, //14
                                                             string.Empty,
                                                             string.Empty, //16
                                                             string.Empty,
                                                             string.Empty, //18
                                                             string.Empty,
                                                             string.Empty, //20
                                                             string.Empty,
                                                             string.Empty, //22
                                                             0,
                                                             string.Empty, //24
                                                             string.Empty,
                                                             AccountNo, //26
                                                             Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                             PaymentARNo, //28
                                                             Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                             PaymentPRNo, //30
                                                             Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                             DataAccess.GetData(dtOthersPaymentTypeName, 0, "Code", "").ToString(),
                                                             DataAccess.GetData(dtOthersPaymentTypeName, 0, "Name", "").ToString(),
                                                             _OthersAmount,
                                                             DataAccess.GetData(dtExistingPayments, 0, "TrsfrRef", "0").ToString(),
                                                             DataAccess.GetData(dtExistingPayments, 0, "TrsfrAcct", "0").ToString(),
                                                             DataAccess.GetData(dtTransferAccountName, 0, "AcctName", "").ToString(),
                                                             Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"),
                                                             Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")//2

                                                             ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }


                                                        //InvoiceEntries = "";


                                                    }



                                                }






                                                //CHECK IF PAYMENT IS DEPOSITED
                                                string qryDeposit = $@" SELECT 
                                                                        ""DeposNum"",
	                                                                    IFNULL(""ComissAct"",'601003') ""CommissAct"",
	                                                                    IFNULL(""Comission"", 0) ""Comission"",
	                                                                    IFNULL(""ComissDate"",CURRENT_TIMESTAMP) ""ComissDate"",
	                                                                    IFNULL(""Project"", 'GENERAL') ""Project"",
	                                                                    IFNULL(""CrdBankAct"", '100410') ""CrdBankAct"",
	                                                                    IFNULL(""CommisVat"", 'IT1') ""CommisVat"",
	                                                                    IFNULL(""VatTotal"", 0) ""VatTotal""
                                                                    FROM ODPS WHERE ""U_ResCode"" = '{RestructuringID}'";
                                                DataTable dtDeposit = hana.GetData(qryDeposit, hana.GetConnection("SAPHana"));

                                                int depoDocEntry = 0;

                                                if (DataAccess.Exist(dtDeposit))
                                                {

                                                    foreach (DataRow dr in dtDeposit.Rows)
                                                    {
                                                        if (isSuccess = cashRegister.CreateDeposit(null,
                                                                                                dr["CommissAct"].ToString(),
                                                                                                Convert.ToDouble(dr["Comission"].ToString()),
                                                                                                Convert.ToDateTime(dr["ComissDate"].ToString()).ToString("yyyy-MM-dd"), //2
                                                                                                dr["Project"].ToString(),
                                                                                                "dtCredit",
                                                                                                dr["CrdBankAct"].ToString(),
                                                                                                dr["CrdBankAct"].ToString(),
                                                                                                dr["CommisVat"].ToString(),
                                                                                                Convert.ToDouble(dr["VatTotal"].ToString()),

                                                                                                dr["DeposNum"].ToString(),
                                                                                                RestructuringID,

                                                                                                newPrjCode,
                                                                                                newBlock,
                                                                                                newLot,

                                                                                                out depoDocEntry,
                                                                                                out errorDesc //36
                                                                                                ))
                                                        {


                                                        }



                                                    }
                                                }

                                            }

                                        }











                                        #endregion


                                    }









































                                    // WITHOUT AR RESERVE INVOICE
                                    else
                                    {


                                        //GET TAXDATE ODPI
                                        string qryODPI = $@" SELECT TOP 1 ""DocDate"",""TaxDate"" FROM ODPI WHERE ""U_BlockNo"" = '{oldBlock}' AND ""U_LotNo"" = '{oldLot}' AND 
                                                IFNULL(""U_SalesType"",'') = 'Real Estate' AND ""Project"" = '{oldPrjCode}' AND IFNULL(""U_Type"",'') IN ('DP','LB') AND
                                                                            IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance')   ORDER BY ""DocEntry""";

                                        DataTable dtODPI = hana.GetData(qryODPI, hana.GetConnection("SAPHana"));
                                        // string ReservationDocDate = DataAccess.GetData(dtReservation, 0, "DocDate", "").ToString();

                                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                        //string ODPITaxDate = DataAccess.GetData(dtODPI, 0, "TaxDate", RestructuringDate).ToString();


                                        //POST DOWNPAYMENT FOR SALES ORDER
                                        if (isSuccess = cashRegister.CreateDownPayment(null,
                                                                                    SapCardCode, //2
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                    //AmountDue, //4
                                                                                    TotalNetPayments, //4
                                                                                    "LB",
                                                                                    "0", //6
                                                                                    newPrjCode,
                                                                                    newBlock, //8
                                                                                    newLot,
                                                                                    SOEntry, //10
                                                                                    AmountDue,
                                                                                    0, //12
                                                                                    "RESTRUCTURING",
                                                                                    Convert.ToInt32(SQEntry),
                                                                                    //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                    //Convert.ToDateTime(ODPITaxDate).ToString("yyyyMMdd"),
                                                                                    Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                                    out DPEntry,
                                                                                    out errorDesc)) //14
                                        {
                                            DPEntries = $"{DPEntries}{DPEntry};";
                                            DPEntry1 = DPEntry;

                                            //qryForUpdates += $@" INSERT INTO QUT13 VALUES ({SQEntry}, 0, '', '', 'LB', 0, {TotalNetPayments},  0, 0, '{Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd")}', 
                                            //                                     '{Location}',{UserId}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";
                                        }




                                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                        //INCOMING PAYMENTS FOR DOWNPAYMENTS
                                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                                        if (isSuccess)
                                        {

                                            //DataTable dtGetRestructureCode = hana.GetData($@"SELECT TOP 1 ""U_RestructCode"" FROM ORCT A WHERE A.""PrjCode"" = '{oldPrjCode}' AND 
                                            //                                             A.""U_BlockNo"" = '{oldBlock}' AND A.""U_LotNo"" = '{oldLot}' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured')
                                            //                                                    ORDER BY ""U_RestructCode"" DESC", hana.GetConnection("SAPHana"));
                                            //string GetRestructureCode = DataAccess.GetData(dtGetRestructureCode, 0, "U_RestructCode", "").ToString();

                                            //GET EXISTING INCOMING PAYMENT
                                            string qryExistingPayments = $@"SELECT A.* FROM ORCT A  WHERE ""U_Project"" = '{oldPrjCode}' AND 
                                                                          ""U_BlockNo"" = '{oldBlock}' and ""U_LotNo"" = '{oldLot}' AND
                                                                          IFNULL(""U_RestructCode"",'') = '{RestructuringID}' AND
                                                                          IFNULL(""U_PaymentType"",'') = 'TCP' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                            DataTable dtExistingPayments = hana.GetData(qryExistingPayments, hana.GetConnection("SAPHana"));

                                            foreach (DataRow dr in dtExistingPayments.Rows)
                                            {
                                                string PaymentDocDate = dr["DocDate"].ToString();
                                                string PaymentRemarks = dr["U_Remarks"].ToString();
                                                string PaymentORNo = dr["U_ORNo"].ToString();
                                                string PaymentORDate = dr["U_ORDate"].ToString();
                                                string PaymentPRNo = dr["U_PRNo"].ToString();
                                                string PaymentPRDate = dr["U_PRDate"].ToString();
                                                string PaymentARNo = dr["U_ARNo"].ToString();
                                                string PaymentARDate = dr["U_ARDate"].ToString();
                                                double PaymentDocTotal = Convert.ToDouble(dr["DocTotal"]);
                                                int PaymentPaymentOrder = Convert.ToInt32(dr["U_PaymentOrder"]);
                                                int PaymentDocEntry = Convert.ToInt32(dr["DocEntry"]);
                                                string InterBankDate = dr["TrsfrDate"].ToString();
                                                string InterBankAcct = dr["TrsfrAcct"].ToString();
                                                string PaymentDocNum = dr["DocNum"].ToString();


                                                int PymtEntry0 = 0;
                                                string AccountNo = "";

                                                string withDeposit = "N";

                                                //CHECK IF CANCELLED DEPOSIT
                                                string qryCancelledDeposit = $@"SELECT 
                                                                                    A.""DocNum"", E.""Canceled"",E.""CnclDps""
                                                                                FROM 
                                                                                    ORCT A INNER JOIN 
                                                                                    rct3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN 
                                                                                    ocrh C ON B.""CrCardNum"" = C.""CrdCardNum"" AND B.""DocNum"" = C.""RctAbs"" INNER JOIN 
                                                                                    ODPS E ON E.""DeposId"" = C.""DepNum"" 
                                                                                WHERE 
	                                                                                A.""DocNum"" = {PaymentDocNum} AND
	                                                                                E.""Canceled"" = 'N' AND
	                                                                                E.""CnclDps"" <> -1 AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance')";
                                                DataTable dtCancelledDeposit = hana.GetData(qryCancelledDeposit, hana.GetConnection("SAPHana"));

                                                if (DataAccess.Exist(dtCancelledDeposit))
                                                {
                                                    withDeposit = "Y";
                                                }



                                                //GET ALL STANDALONE INVOICES
                                                int InvoiceEntry = 0;
                                                string qryGetSAPCardCodeOfOldCardCode = $@"select ""CardCode"" from ocrd where ""U_DreamsCustCode"" = '{oldCardCode}' ";
                                                DataTable dtGetSAPCardCodeOfOldCardCode = hana.GetData(qryGetSAPCardCodeOfOldCardCode, hana.GetConnection("SAPHana"));
                                                string GetSAPCardCodeOfOldCardCode = DataHelper.DataTableRet(dtGetSAPCardCodeOfOldCardCode, 0, "CardCode", "0");

                                                string qryStandaloneInvoices = $@"SELECT 
                                                                          A.""DocEntry"", A.""DocTotal"",A.""U_HouseModel"", A.""U_ReservationAmount"", 
                                                                          ""U_DPAmount"", A.""U_LBAmount"", A.""U_TransactionType"",
                                                                          A.""U_Waive"",  A.""U_Partial"", A.""U_PaymentOrder"", A.""U_Type""
                                                                      FROM 
                                                                        OINV A INNER JOIN
                                                                        RCT2 B ON A.""DocEntry"" = B.""DocEntry"" INNER JOIN
                                                                        ORCT C ON B.""DocNum"" = C.""DocEntry""
                                                                      WHERE
                                                                          A.""Project"" = '{oldPrjCode}' AND
                                                                          A.""U_BlockNo"" = '{oldBlock}' AND
                                                                          A.""U_LotNo"" = '{oldLot}' AND
                                                                          A.""CardCode"" = '{GetSAPCardCodeOfOldCardCode}' AND
                                                                          A.""CANCELED"" <> 'C' AND
                                                                          A.""U_TransactionType"" IN('RE - SUR', 'INT INC', 'IP&S') AND
                                                                          IFNULL(C.""U_ORNo"",'') = '{PaymentORNo}' AND
                                                                          IFNULL(C.""U_ARNo"",'') = '{PaymentARNo}' AND
                                                                          IFNULL(C.""U_PRNo"",'') = '{PaymentPRNo}' AND
                                                                          IFNULL(A.""U_RestructureTag"",'OLD') <> 'NEW' AND IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";

                                                DataTable dtStandaloneInvoices = hana.GetData(qryStandaloneInvoices, hana.GetConnection("SAPHana"));
                                                if (dtStandaloneInvoices.Rows.Count > 0)
                                                {
                                                    foreach (DataRow drInvoices in dtStandaloneInvoices.Rows)
                                                    {
                                                        int InvoiceDocEntry = int.Parse(drInvoices["DocEntry"].ToString());
                                                        double InvoiceDocTotal = double.Parse(drInvoices["DocTotal"].ToString());
                                                        string InvoiceHouseModel = drInvoices["U_HouseModel"].ToString();
                                                        double InvoiceReservationAmount = double.Parse(drInvoices["U_ReservationAmount"].ToString());
                                                        double InvoiceDPAmount = double.Parse(drInvoices["U_DPAmount"].ToString());
                                                        double InvoiceLBAmount = double.Parse(drInvoices["U_LBAmount"].ToString());
                                                        string InvoiceWaive = drInvoices["U_Waive"].ToString();
                                                        string InvoicePartial = drInvoices["U_Partial"].ToString();
                                                        string InvoicePaymentOrder = drInvoices["U_PaymentOrder"].ToString();
                                                        string InvoiceType = drInvoices["U_Type"].ToString();

                                                        string TransType = "";
                                                        string InvoiceTransType = drInvoices["U_TransactionType"].ToString();
                                                        if (InvoiceTransType == "RE - SUR")
                                                        {
                                                            TransType = "Surcharge";

                                                            //qryForUpdates += $@" INSERT INTO QUT13 VALUES ({SQEntry}, 0, '{PaymentORNo}', {PymtEntry0}, '{InvoiceType}', {InvoicePaymentOrder}, 0, 0, {InvoiceDocTotal}, '{ Convert.ToDateTime(InvoiceDocTotal).ToString("yyyyMMdd")}', 
                                                            //                     '{Location}',{UserId}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";

                                                        }
                                                        else if (InvoiceTransType == "INT INC")
                                                        {
                                                            TransType = "Interest";

                                                            //qryForUpdates += $@" INSERT INTO QUT13 VALUES ({SQEntry}, 0, '{PaymentORNo}', {PymtEntry0}, '{InvoiceType}', {InvoicePaymentOrder}, 0,  {InvoiceDocTotal}, 0, '{ Convert.ToDateTime(InvoiceDocTotal).ToString("yyyyMMdd")}', 
                                                            //                     '{Location}',{UserId}, '{DateTime.Now.ToString("yyyyMMdd")}'); ";

                                                        }
                                                        else if (InvoiceTransType == "IP&S")
                                                        {
                                                            TransType = "IPS";
                                                        }


                                                        if (InvoiceDocTotal > 0)
                                                        {
                                                            //REECREATE INVOICE FOR SURCHARGE, INTEREST AND IP&S
                                                            if (isSuccess = cashRegister.CreateARInvoiceStandalone(null,
                                                                              SapCardCode, //2
                                                                              Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                              InvoiceDocTotal, //4
                                                                              InvoiceType,
                                                                              InvoicePaymentOrder, //6
                                                                              oldPrjCode,
                                                                              oldBlock, //8
                                                                              oldLot,
                                                                              InvoiceHouseModel, //10
                                                                              oldFinancingScheme,
                                                                              oldProductType, //12
                                                                              InvoiceReservationAmount,
                                                                              InvoiceDPAmount, //14
                                                                              InvoiceLBAmount,
                                                                              TransType, //16
                                                                              DocNum,
                                                                               ConfigSettings.ExcessControlAccount, //18
                                                                              Vatable,
                                                                              0, //20
                                                                              InvoicePartial,
                                                                              InvoiceWaive,
                                                                              //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                              //Convert.ToDateTime(ODPITaxDate).ToString("yyyyMMdd"),
                                                                              Convert.ToDateTime(RestructuringDate).ToString("yyyyMMdd"),
                                                                              out InvoiceEntry,
                                                                              out Message)) //22
                                                            {


                                                                //UPDATE THE INVOICE WITH RESTRUCTURE TAG = OLD
                                                                string qryUpdateRestructureTag = $@"UPDATE OINV SET ""U_RestructureTag"" = 'OLD' WHERE ""DocEntry"" = {InvoiceDocEntry} AND IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Restructured','Advance') ";
                                                                hana.Execute(qryUpdateRestructureTag, hana.GetConnection("SAPHana"));

                                                                if (AdvancePayment == 0)
                                                                {
                                                                    int tagExistEntry = 0;
                                                                    foreach (var docEntry in Convert.ToString(InvoiceEntry).Split(';'))
                                                                    {
                                                                        if (docEntry != "")
                                                                        {
                                                                            if (docEntry == Convert.ToString(InvoiceEntry))
                                                                            {
                                                                                tagExistEntry++;
                                                                            }
                                                                        }
                                                                    }

                                                                    if (tagExistEntry == 0)
                                                                    {
                                                                        InvoiceEntries = $"{InvoiceEntries}{InvoiceEntry};";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    InvoiceEntries = $"{InvoiceEntries}{InvoiceEntry};";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }



                                                if (isSuccess = cashRegister.CreateIncomingPayment(SapCardCode,
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2
                                                                                        PaymentORNo, //OR
                                                                                        PaymentRemarks, //4
                                                                                        0,
                                                                                        DPEntries, //6
                                                                                        InvoiceEntries,
                                                                                        null, //8
                                                                                        PaymentORDate,
                                                                                        PaymentPRNo, //10
                                                                                        PaymentPRDate,
                                                                                        PaymentARNo, //12
                                                                                        PaymentARDate,
                                                                                        PaymentDocTotal, //14
                                                                                        0,
                                                                                        0, //16
                                                                                        DocNum,
                                                                                        PaymentPaymentOrder, //18
                                                                                        "LB",
                                                                                        0, //20
                                                                                        0,
                                                                                        UserCode, //22
                                                                                        newBlock,
                                                                                        newLot, //24
                                                                                        newPrjCode,
                                                                                        "N", //26
                                                                                        "Y",
                                                                                        PaymentDocEntry, //28
                                                                                        "TCP",
                                                                                        //2023-05-17 : COMMENTED BY DHEZA - RESTRUCTURING DATE NALANG DAPAT ANG TAXDATE POSTING
                                                                                        //Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd"), //2
                                                                                        Convert.ToDateTime(RestructuringDate).ToString("yyyy-MM-dd"), //2

                                                                                        withDeposit,
                                                                                        "",
                                                                                        null,
                                                                                        0,
                                                                                        0,

                                                                                        //2023-09-21 : ADD SURCHARGE DATE ON POSTING
                                                                                        "",

                                                                                        //2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
                                                                                        CancellationTagging,

                                                                                        out PymtEntry0,
                                                                                        out errorDesc, //30
                                                                                        out AccountNo,
                                                                                        out _CashAmount, //32
                                                                                        out _CheckAmount,
                                                                                        out _CreditAmount, //34
                                                                                        out _TransferAmount,
                                                                                        out _OthersAmount //36
                                                                                        ))
                                                {
                                                    //if (UpdateAmortBalance == "YES")
                                                    //{
                                                    string receiptNo = string.IsNullOrWhiteSpace(PaymentORNo) ? (string.IsNullOrWhiteSpace(PaymentARNo) ? PaymentPRNo : PaymentARNo) : PaymentORNo;

                                                    qryForUpdates += $@" UPDATE QUT13 SET ""SAPDocEntry"" = {PymtEntry0} WHERE ""DocEntry"" = {SQEntry} AND ""ReceiptNo"" = '{receiptNo}' AND ""Location"" = '{Location}'; ";
                                                    //}


                                                    if (_CashAmount > 0)
                                                    {
                                                        if (!ws.PostQuotation(
                                                                     0,
                                                                     "Cash",
                                                                     int.Parse(SQEntry),
                                                                     PymtEntry0,
                                                                     //amount,
                                                                     _CashAmount,
                                                                     PaymentORNo,

                                                                    //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                    //  PaymentRemarks + " - Restructured",
                                                                    AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",


                                                                     string.Empty, //8
                                                                    "0",
                                                                    string.Empty, //10
                                                                    string.Empty,
                                                                    string.Empty,//12
                                                                    string.Empty,
                                                                    string.Empty, //14
                                                                    string.Empty,
                                                                    string.Empty, //16
                                                                    string.Empty,
                                                                    string.Empty, //18
                                                                    string.Empty,
                                                                    string.Empty, //20
                                                                    string.Empty,
                                                                    string.Empty, //22
                                                                    0,
                                                                    string.Empty, //24
                                                                    string.Empty,
                                                                    AccountNo, //26
                                                                    Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                    PaymentARNo, //28
                                                                    Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                    PaymentPRNo, //30
                                                                    Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                                    string.Empty, //32
                                                                    string.Empty,
                                                                    0, //34
                                                                    string.Empty,
                                                                    string.Empty, //36
                                                                    string.Empty,
                                                                    string.Empty, //38
                                                                    Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                             ))
                                                        {
                                                            isSuccess = false;
                                                        }
                                                    }

                                                    if (_TransferAmount > 0)
                                                    {
                                                        //GET INTERBANK BANK 
                                                        string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                        DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                        string InterBankBank = DataAccess.GetData(dtGetTransferBank, 0, "BankCode", "").ToString();

                                                        if (!ws.PostQuotation(
                                                                           0,
                                                                           "Interbank", //2
                                                                           int.Parse(SQEntry),
                                                                           PymtEntry0, //4
                                                                                       //amount,
                                                                           _TransferAmount,
                                                                           PaymentORNo,

                                                                            //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                                            //  PaymentRemarks + " - Restructured",
                                                                            AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                                           Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                                           InterBankAcct,
                                                                           string.Empty, //10
                                                                           InterBankBank,
                                                                           string.Empty, //12
                                                                           string.Empty,
                                                                           string.Empty, //14
                                                                           string.Empty,
                                                                           string.Empty, //16
                                                                           string.Empty,
                                                                           string.Empty, //18
                                                                           string.Empty,
                                                                           string.Empty, //20
                                                                           string.Empty,
                                                                           string.Empty, //22
                                                                           0,
                                                                           string.Empty, //24
                                                                           string.Empty,
                                                                           AccountNo, //26
                                                                           Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                                           PaymentARNo, //28
                                                                           Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                                           PaymentPRNo, //30
                                                                           Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),
                                                                           string.Empty, //32
                                                                           string.Empty,
                                                                           0, //34
                                                                           string.Empty,
                                                                           string.Empty, //36
                                                                           string.Empty,
                                                                           string.Empty, //38
                                                                           Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                                                ))
                                                        {
                                                            isSuccess = false;
                                                        }
                                                    }

                                                    if (_CheckAmount > 0)
                                                    {

                                                        //GET CREDIT CARD DETAILS FROM EXISTING 
                                                        string qryCheck = $@"SELECT DISTINCT B.""DueDate"",B.""CheckNum"",B.""BankCode"",B.""Branch"",B.""CheckAct""  FROM  RCT1 B INNER JOIN OCHH C ON B.""CheckNum"" = C.""CheckNum""
                                                                          WHERE B.""DocNum"" = {PaymentDocEntry} AND C.""CheckKey"" NOT IN        
                                                                          (SELECT x.""CheckKey"" FROM DPS1 x INNER JOIN ODPS y ON x.""DepositId"" = y.""DeposId"" 
                                                                          WHERE y.""Canceled"" ='N' and y.""CnclDps"" = -1) ";
                                                        DataTable dtCheck = hana.GetData(qryCheck, hana.GetConnection("SAPHana"));



                                                        foreach (DataRow dr_ in dtCheck.Rows)
                                                        {

                                                            if (!ws.PostQuotation(
                                                            0,
                                                            "Check", //2    
                                                            int.Parse(SQEntry),
                                                            PymtEntry0, //4
                                                            _CheckAmount,
                                                            PaymentORNo, //6

                                                            //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                            //  PaymentRemarks + " - Restructured",
                                                            AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                            Convert.ToDateTime(dr_["DueDate"].ToString()).ToString("yyyy-MM-dd"), //8
                                                            dr_["CheckNum"].ToString(),
                                                            dr_["BankCode"].ToString(),
                                                            dr_["BankCode"].ToString(),
                                                            dr_["Branch"].ToString(),
                                                            dr_["CheckAct"].ToString(),

                                                            //Convert.ToDateTime(DataAccess.GetData(dtCheck, 0, "DueDate", "0")).ToString("yyyy-MM-dd"), //8
                                                            //DataAccess.GetData(dtCheck, 0, "CheckNum", "0").ToString(),
                                                            //DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString(), //10
                                                            //DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString(),
                                                            //DataAccess.GetData(dtCheck, 0, "Branch", "").ToString(), //12
                                                            //DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString(),
                                                            string.Empty, //14
                                                            string.Empty,
                                                            string.Empty, //16
                                                            string.Empty,
                                                            string.Empty, //18
                                                            string.Empty,
                                                            string.Empty, //20
                                                            string.Empty,
                                                            string.Empty, //22
                                                            0,
                                                            string.Empty, //24
                                                            string.Empty,
                                                            AccountNo, //26
                                                            Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                            PaymentARNo, //28
                                                            Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                            PaymentPRNo, //30
                                                            Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                            string.Empty,
                                                            string.Empty,
                                                            0,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                            ))
                                                            {
                                                                isSuccess = false;
                                                            }
                                                        }
                                                    }

                                                    if (_CreditAmount > 0)
                                                    {
                                                        //GET CREDIT CARD DETAILS FROM EXISTING 
                                                        DataTable dtCredit = hana.GetData($@"select * from rct3 where ""DocNum"" = {PaymentDocEntry}", hana.GetConnection("SAPHana"));
                                                        DataTable dtCreditAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString()}'", hana.GetConnection("SAPHana"));
                                                        DataTable dtPaymentTypeName = hana.GetData($@"select ""CrTypeName"" from ocrp where ""CrTypeCode"" = {DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString()}", hana.GetConnection("SAPHana"));

                                                        if (!ws.PostQuotation(
                                                           0,
                                                            "Credit", //2
                                                            int.Parse(SQEntry),
                                                            PymtEntry0, //4
                                                            _CreditAmount,
                                                            PaymentORNo, //6

                                                            //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                            //  PaymentRemarks + " - Restructured",
                                                            AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",


                                                            string.Empty, //8
                                                            "0",
                                                            string.Empty, //10
                                                            DataAccess.GetData(dtCredit, 0, "U_CCBrand", "0").ToString(),
                                                            string.Empty,//12
                                                            string.Empty,
                                                            DataAccess.GetData(dtCredit, 0, "CreditCard", "0").ToString(), //14
                                                            DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString(),
                                                            DataAccess.GetData(dtCreditAccountName, 0, "AcctName", "").ToString(), //16
                                                            DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(),
                                                            Convert.ToDateTime(DataAccess.GetData(dtCredit, 0, "CardValid", "")).ToString("yyyy-MM-dd"), //18
                                                            DataAccess.GetData(dtCredit, 0, "OwnerIdNum", "").ToString(),
                                                            DataAccess.GetData(dtCredit, 0, "OwnerPhone", "").ToString(), //20
                                                            DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString(),
                                                            DataAccess.GetData(dtPaymentTypeName, 0, "CrTypeName", "").ToString(), //22
                                                            int.Parse(DataAccess.GetData(dtCredit, 0, "NumOfPmnts", "0").ToString()),
                                                            DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString(), //24
                                                            "S",
                                                            AccountNo, //26 
                                                            Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                            PaymentARNo, //28
                                                            Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                            PaymentPRNo, //30
                                                            Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                            string.Empty,
                                                            string.Empty,
                                                            0,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            string.Empty,
                                                            Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")
                                                            ))
                                                        {
                                                            isSuccess = false;
                                                        }
                                                    }

                                                    if (_OthersAmount > 0)
                                                    {
                                                        //GET INTERBANK BANK 
                                                        string qryGetTransferBank = $@"select * from dsc1 where ""GLAccount"" = '{InterBankAcct}'";
                                                        DataTable dtGetTransferBank = hana.GetData(qryGetTransferBank, hana.GetConnection("SAPHana"));
                                                        string InterBankBank = DataAccess.GetData(dtGetTransferBank, 0, "BankCode", "").ToString();

                                                        DataTable dtTransferAccountName = hana.GetData($@"select ""AcctName"" from oact where ""AcctCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));

                                                        //2024-09-14: CHANGE BASIS OF GETTING OTHER PAYMENT MEANS FROM ACCOUNT CODE TO BANK CODE 
                                                        //DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""U_GLAccountCode"" = '{dr["TrsfrAcct"].ToString()}'", hana.GetConnection("SAPHana"));
                                                        DataTable dtOthersPaymentTypeName = hana.GetData($@"SELECT * FROM ""@OTHERPAYMENTMEANS"" WHERE ""Code"" = '{dr["U_OthPayment"].ToString()}'", hana.GetConnection("SAPHana"));


                                                        if (!ws.PostQuotation(
                                                         0,
                                                         "Others", //2
                                                         int.Parse(SQEntry),
                                                         PymtEntry0, //4
                                                                     //amount,
                                                         _OthersAmount,
                                                         PaymentORNo, //6

                                                        //2023-07-03 : REMOVE IF PROCESS IS ADVANCE PAYMENT
                                                        //  PaymentRemarks + " - Restructured",
                                                        AdvancePayment == 1 ? PaymentRemarks : PaymentRemarks + " - Restructured",

                                                         Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"), //8
                                                         "0",

                                                         string.Empty, //10
                                                         string.Empty,
                                                         string.Empty, //12
                                                         string.Empty,

                                                         string.Empty, //14
                                                         string.Empty,
                                                         string.Empty, //16
                                                         string.Empty,
                                                         string.Empty, //18
                                                         string.Empty,
                                                         string.Empty, //20
                                                         string.Empty,
                                                         string.Empty, //22
                                                         0,
                                                         string.Empty, //24
                                                         string.Empty,
                                                         AccountNo, //26
                                                         Convert.ToDateTime(PaymentORDate).ToString("yyyy-MM-dd"),
                                                         PaymentARNo, //28
                                                         Convert.ToDateTime(PaymentARDate).ToString("yyyy-MM-dd"),
                                                         PaymentPRNo, //30
                                                         Convert.ToDateTime(PaymentPRDate).ToString("yyyy-MM-dd"),

                                                         DataAccess.GetData(dtOthersPaymentTypeName, 0, "Code", "").ToString(),
                                                         DataAccess.GetData(dtOthersPaymentTypeName, 0, "Name", "").ToString(),
                                                         _OthersAmount,
                                                         DataAccess.GetData(dtExistingPayments, 0, "TrsfrRef", "0").ToString(),
                                                         DataAccess.GetData(dtExistingPayments, 0, "TrsfrAcct", "0").ToString(),
                                                         DataAccess.GetData(dtTransferAccountName, 0, "AcctName", "").ToString(),
                                                         Convert.ToDateTime(InterBankDate).ToString("yyyy-MM-dd"),
                                                         Convert.ToDateTime(PaymentDocDate).ToString("yyyy-MM-dd")

                                                         ))
                                                        {
                                                            isSuccess = false;
                                                        }
                                                    }


                                                    InvoiceEntries = "";


                                                    temp_TotalNetPayments = TotalNetPayments - (newDPAmount + newReservationFee);
                                                }

                                                booked = "N";
                                                qryForUpdates += $@" UPDATE ""OQUT"" SET ""BookStatus"" = 'N' WHERE ""DocEntry"" = '{SQEntry}'; ";



                                            }






                                            //CHECK IF PAYMENT IS DEPOSITED
                                            string qryDeposit = $@" SELECT 
                                                                        ""DeposNum"",
	                                                                    IFNULL(""ComissAct"",'601003') ""CommissAct"",
	                                                                    IFNULL(""Comission"", 0) ""Comission"",
	                                                                    IFNULL(""ComissDate"",CURRENT_TIMESTAMP) ""ComissDate"",
	                                                                    IFNULL(""Project"", 'GENERAL') ""Project"",
	                                                                    IFNULL(""CrdBankAct"", '100410') ""CrdBankAct"",
	                                                                    IFNULL(""CommisVat"", 'IT1') ""CommisVat"",
	                                                                    IFNULL(""VatTotal"", 0) ""VatTotal""
                                                                    FROM ODPS WHERE ""U_ResCode"" = '{RestructuringID}'";
                                            DataTable dtDeposit = hana.GetData(qryDeposit, hana.GetConnection("SAPHana"));

                                            int depoDocEntry = 0;

                                            if (DataAccess.Exist(dtDeposit))
                                            {

                                                foreach (DataRow dr in dtDeposit.Rows)
                                                {
                                                    if (isSuccess = cashRegister.CreateDeposit(null,
                                                                                            dr["CommissAct"].ToString(),
                                                                                            Convert.ToDouble(dr["Comission"].ToString()),
                                                                                            Convert.ToDateTime(dr["ComissDate"].ToString()).ToString("yyyy-MM-dd"), //2
                                                                                            dr["Project"].ToString(),
                                                                                            "dtCredit",
                                                                                            dr["CrdBankAct"].ToString(),
                                                                                            dr["CrdBankAct"].ToString(),
                                                                                            dr["CommisVat"].ToString(),
                                                                                            Convert.ToDouble(dr["VatTotal"].ToString()),

                                                                                            dr["DeposNum"].ToString(),
                                                                                            RestructuringID,

                                                                                            newPrjCode,
                                                                                            newBlock,
                                                                                            newLot,

                                                                                            out depoDocEntry,
                                                                                            out errorDesc //36
                                                                                            ))
                                                    {


                                                    }



                                                }
                                            }



                                        }




                                    }
                                }






                            }
                        }
                        else
                        {
                            isSuccess = false;
                        }
                    }
                    #endregion





























                    #region UPDATE BATCH STATUS

                    //UPDATE BATCH IN SAP : U_LOT STATUS

                    if (isSuccess)
                    {
                        if (oldPrjCode == newPrjCode && oldBlock == newBlock && oldLot == newLot)
                        {

                        }
                        else
                        {
                            isSuccess = cashRegister.UpdateBatchDetails(null, oldPrjCode, oldBlock, oldLot, "AVAILABLE", out errorDesc);
                            isSuccess = cashRegister.UpdateBatchDetails(null, newPrjCode, newBlock, newLot, "SOLD", out errorDesc);
                        }

                    }

                    #endregion













                    //UPDATE ALL DATA FOR PAYMENTS
                    if (isSuccess)
                    {
                        if (!string.IsNullOrWhiteSpace(qryForUpdates))
                        {
                            foreach (var qryForUpdatesRow in qryForUpdates.Split(';'))
                            {
                                hana.Execute(qryForUpdatesRow, hana.GetConnection("SAOHana"));
                            }
                        }
                    }








                    if (isSuccess)
                    {
                        outQuotationDocEntry = SapEntry.ToString();
                        outDPEntry = DPEntry1.ToString();
                        outMiscEntry = MiscEntry.ToString();
                        return isSuccess;
                    }
                    else
                    {
                        string errorDesc2 = "";
                        //CancelSAPDocuments(company, cashRegister, oldBlock, oldLot, oldPrjCode, newPrjCode, newBlock, newLot, DocNum, oSAPDB, SQEntry,
                        //    oldCardCode, DocDate, oldModel, oldFinancingScheme, oldProductType, oldReservationFee, oldDPAmount,
                        //    AmountDue, "ERROR", RestructuringDate, out errorDesc2, out RestructuringID);


                        outQuotationDocEntry = "0";
                        outDPEntry = "0";
                        outMiscEntry = "0";
                        return false;
                    }



                }
                else
                {
                    outQuotationDocEntry = "0";
                    outDPEntry = "0";
                    outMiscEntry = "0";
                    return false;
                }

            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                outQuotationDocEntry = "0";
                outDPEntry = "0";
                outMiscEntry = "0";
                return false;
            }
        }





        void JEForNoLTSNumber(CashRegisterService cashRegister, string projectCode, string SapCardCode, string payment, int SODocEntry, string FinancingScheme
            , string TaxClassification, string Block, string Lot, string ARAccount, string APOthersAccount, string JEType, string SQEntry, string DocNum, out bool isSuccess, out string Message)
        {
            isSuccess = true;
            Message = "";
            int JournalEntryNo = 0;
            //JOURNAL ENTRY WHEN PROJECT HAS NO LTS NO.
            //DataTable dtLTSNo = hana.GetData($@"SELECT ""U_LTSno"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{projectCode}'", hana.GetConnection("SAPHana"));
            DataTable dtLTSNo = hana.GetData($@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocEntry"" = {SQEntry}", hana.GetConnection("SAOHana"));
            if (dtLTSNo.Rows.Count > 0)
            {
                string LTSNo = DataAccess.GetData(dtLTSNo, 0, "LTSNo", "").ToString();

                if (string.IsNullOrWhiteSpace(LTSNo))
                {
                    if (isSuccess = cashRegister.CreateJournalEntry(null, projectCode, SapCardCode, Convert.ToDouble(payment), SODocEntry, FinancingScheme,
                                                    TaxClassification, Block, Lot, ARAccount, APOthersAccount, "", JEType, DocNum, "", "", "Y", Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd"), "", out JournalEntryNo, out Message, ""))
                    {

                    }
                }
            }
        }







        public bool CancelTransactions(int SQEntry, string CardCode,  //2
                                       string ProjCode, string Model, //4
                                       string Block, string Lot,  //6
                                       string LotArea, string Feat, //8
                                       string PriceCat, string DAS, //10
                                       string Discount, string VAT,  //12
                                       string ItemCode, string ItemCodeOC, //14 
                                       double DPAmount, double LBAmount,  //16
                                       string Stage, double TotalPaid,  //18
                                       int AOEntry, out string ErrMsg) //20
        {
            try
            {
                if (ConnectToSAPDI(out oCompany, out ErrMsg))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    bool isError = false;
                    double TotalInterest = 0;
                    double TotalPenalty = 0;
                    DataTable dt = new DataTable();
                    //** Loop I.Payments **//
                    dt = ws.Cancellation(SQEntry, "IP").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel i.payments **//
                        oPayments = oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                        oPayments.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oPayments.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                    }
                    //** Loop AR Invoice **//
                    dt = ws.Cancellation(SQEntry, "AR").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel ar invoice **//
                        oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                        oInvoice.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oInvoice.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                    }
                    //** Loop ARDP Invoice **//
                    dt = ws.Cancellation(SQEntry, "ARDP").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel ardp **//
                        oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oDownPayments);
                        oInvoice.DownPaymentType = DownPaymentTypeEnum.dptRequest;
                        oInvoice.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oInvoice.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                    }
                    //** Loop Sales Order **//
                    dt = ws.Cancellation(SQEntry, "SO").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel sales order **//
                        oOrder = oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                        oOrder.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oOrder.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                        else
                        {
                            DataTable dt1 = new DataTable();
                            dt1 = DataAccess.Select("SAP", $"SELECT ItemCode,Price FROM QUT1 Where DocEntry = '{row[0].ToString()}' and ItemCode IN  ('{ConfigSettings.InterestItemCode}','{ConfigSettings.PenaltyItemCode}')");
                            if (dt1.Rows.Count > 0)
                            {
                                //** interest **//
                                if (dt1.Rows[0][0].ToString() == ConfigSettings.InterestItemCode)
                                    TotalInterest += double.Parse(dt1.Rows[0][1].ToString());
                                //** penalty **//
                                if (dt1.Rows[0][0].ToString() == ConfigSettings.PenaltyItemCode)
                                    TotalPenalty += double.Parse(dt1.Rows[0][1].ToString());
                            }
                        }
                    }
                    //** Loop Reservation Payments **//
                    dt = ws.Cancellation(SQEntry, "IPRSV").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel rsv i.payments **//
                        oPayments = oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                        oPayments.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oPayments.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                    }
                    //** Loop Reservation ARDP **//
                    dt = ws.Cancellation(SQEntry, "ARRSV").Tables[0];
                    foreach (DataRow row in dt.Rows)
                    {
                        //** cancel rsv ardp **//
                        oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oDownPayments);
                        oInvoice.DownPaymentType = DownPaymentTypeEnum.dptRequest;
                        oInvoice.GetByKey(int.Parse(row[0].ToString()));
                        lRetCode = oInvoice.Cancel();
                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lErrCode, out sErrMsg);
                            ErrMsg = lErrCode + "-" + sErrMsg;
                            isError = true;
                            break;
                        }
                    }

                    //** Cancel Base SalesQuotation **//
                    //if (SQEntry != 0)
                    //{
                    //    oQuotation = oCompany.GetBusinessObject(BoObjectTypes.oQuotations);
                    //    oQuotation.GetByKey(SQEntry);
                    //    lRetCode = oQuotation.Cancel();
                    //    if (lRetCode != 0)
                    //    {
                    //        oCompany.GetLastError(out lErrCode, out sErrMsg);
                    //        ErrMsg = lErrCode + "-" + sErrMsg;
                    //        isError = true;
                    //    }
                    //}

                    //** CREATE NEW TRANSACTION **//
                    if (!isError)
                    {
                        int SOEntry = 0;
                        int DPEntry = 0;
                        int LBEntry = 0;
                        int PymtEntry = 0;
                        bool isErr = false;
                        //** create sales quotation **//
                        if (CreateSalesQuotationRestructure(CardCode, ProjCode, Model, Block, Lot, Feat, PriceCat, DAS, Discount, VAT, ItemCode, ItemCodeOC, out SQEntry, out ErrMsg))
                        {
                            //** create quotation **//
                            if (CreateSalesOrderRestructure(SQEntry, CardCode, TotalInterest, TotalPenalty, Block, Lot, LotArea, out SOEntry, out ErrMsg))
                            {
                                //** create downpayment request for DP **//
                                if (CreateDownPaymentRequestRestructure(CardCode, SQEntry, SOEntry, DPAmount, out DPEntry, out ErrMsg))
                                {
                                    //** If stage = LB Then Create downpayment request for LB **//
                                    if (Stage == "LB")
                                    {
                                        if (!CreateDownPaymentRequestRestructure(CardCode, SQEntry, SOEntry, LBAmount, out LBEntry, out ErrMsg))
                                        {
                                            isErr = true;
                                        }
                                    }
                                    //** Create incoming payments **//
                                    if (IncomingPaymentsRestructure(DPEntry, LBEntry, CardCode, DPAmount, LBAmount, TotalPaid, out ErrMsg))
                                    {
                                        //** UPDATE DocEntry ADDON **//
                                        if (!DataAccess.Execute("Addon", $"UPDATE OQUT SET SapDocEntry = '{SQEntry}',DPEntry='{DPEntry}',LBEntry='{LBEntry}' Where DocEntry = '{AOEntry}'"))
                                        {
                                            isErr = true;
                                        }
                                    }
                                    else
                                    {
                                        isErr = true;
                                    }
                                }
                                else
                                    isErr = true;
                            }
                            else
                                isErr = true;
                        }

                        if (!isErr)
                        {
                            if (oCompany.InTransaction)
                                oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                            ErrMsg = "";
                            return true;
                        }
                        else
                        {
                            if (oCompany.InTransaction)
                                oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                            ErrMsg = "Error in creating new transaction";
                            return false;
                        }
                    }
                    else
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                        ErrMsg = "";
                        return false;
                    }
                }
                else
                {
                    if (oCompany.InTransaction)
                        oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);
                    ErrMsg = "Error in connecting to SAP";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public bool IncomingPaymentsRestructure(int DPEntry, int LBEntry, string CardCode, double DPAmount, double LBAmount, double TotalPayment, out string ErrMsg)
        {
            try
            {
                //** create payments **//
                oPayments = oCompany.GetBusinessObject(BoObjectTypes.oIncomingPayments);
                oPayments.CardCode = CardCode;
                oPayments.DocDate = DateTime.Now;
                oPayments.Remarks = $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}";


                if (DPEntry != 0)
                {
                    oPayments.Invoices.DocEntry = DPEntry;
                    oPayments.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;

                    if (TotalPayment > DPAmount)
                        oPayments.Invoices.SumApplied = DPAmount;
                    else
                        oPayments.Invoices.SumApplied = TotalPayment;

                    oPayments.Invoices.Add();
                }
                if (LBEntry != 0)
                {
                    oPayments.Invoices.DocEntry = LBEntry;
                    oPayments.Invoices.InvoiceType = BoRcptInvTypes.it_DownPayment;
                    oPayments.Invoices.SumApplied = TotalPayment - DPAmount;
                    oPayments.Invoices.Add();
                }

                //** cash payment **//
                oPayments.CashAccount = ConfigSettings.CashAccount;
                oPayments.CashSum = TotalPayment;

                lRetCode = oPayments.Add();
                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
                else
                {
                    ErrMsg = "";
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public bool ChangeOwnership(string CardCode, string BPCode, string LName, string FName, string MName, out string ErrMsg)
        {
            try
            {
                if (ConnectToSAPDI(out oCompany, out ErrMsg))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();


                    oBP = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                    oBP.GetByKey(CardCode);
                    oBP.CardName = LName + FName + MName;
                    oBP.CardForeignName = BPCode;
                    oBP.UserFields.Fields.Item("U_U_API_FName").Value = FName;
                    oBP.UserFields.Fields.Item("U_U_API_LName").Value = LName;
                    oBP.UserFields.Fields.Item("U_U_API_MName").Value = MName;

                    lRetCode = oBP.Update();

                    if (lRetCode != 0)
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);

                        ws.Execute($"UPDATE FROM OCRD SET SAPCardCode = '{CardCode}' Where CardCode = '{BPCode}'", "Addon");
                        ws.Execute($"DELETE FROM QUT3 Where DocEntry = '{CardCode}' and ORNumber = '{BPCode}'", "Addon");

                        ErrMsg = "";
                        return true;
                    }
                    else
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        ErrMsg = lErrCode + "-" + sErrMsg;
                        return false;
                    }
                }
                else
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public bool ChangeLocation(int SOEntry, string ProjCode, string Model, string Feat, string PriceCat, string Block, string Lot, string LotArea, out string ErrMsg)
        {
            try
            {
                if (ConnectToSAPDI(out oCompany, out ErrMsg))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    oOrder = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    oOrder.GetByKey(SOEntry);
                    oOrder.UserFields.Fields.Item("U_Project").Value = ProjCode;
                    oOrder.UserFields.Fields.Item("U_qsBlock").Value = Block;
                    oOrder.UserFields.Fields.Item("U_qsLotNo").Value = Lot;
                    oOrder.UserFields.Fields.Item("U_qsModel").Value = Model;
                    oOrder.UserFields.Fields.Item("U_qsHouseFeat").Value = Feat;
                    oOrder.UserFields.Fields.Item("U_qsPriceCat").Value = PriceCat;

                    //** update batch details **//
                    //DataTable dt = DataAccess.Select("SAP", $"SELECT A.ItemCode FROM RDR1 A INNER JOIN OITM B ON A.ItemCode = B.ItemCode Where A.DocEntry = '{SOEntry}' and B.ManBtchNum = 'Y'");

                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    var oLines = oOrder.Lines;
                    //    //** check if item is manage by batch **//
                    //    //** add batch **//
                    //    oLines.BatchNumbers.BatchNumber = string.Format("BL{0}LT{1}", Block, Lot);
                    //    oLines.BatchNumbers.Quantity = double.Parse(LotArea);
                    //    oLines.BatchNumbers.Add();
                    //}


                    lRetCode = oOrder.Update();

                    if (lRetCode == 0)
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_Commit);
                        ErrMsg = "";
                        return true;
                    }
                    else
                    {
                        if (oCompany.InTransaction)
                            oCompany.EndTransaction(BoWfTransOpt.wf_RollBack);

                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        ErrMsg = lErrCode + "-" + sErrMsg;
                        return false;
                    }
                }
                else
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public bool UpdateTotalRestructure(int DocEntry, double Amount, out Company oCmp)
        {
            try
            {
                string ErrMsg;
                if (ConnectToSAPDI(out oCompany, out ErrMsg))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oDownPayments);
                    oInvoice.DownPaymentType = DownPaymentTypeEnum.dptRequest;
                    oInvoice.GetByKey(DocEntry);
                    oInvoice.DocTotal = Amount;

                    lRetCode = oInvoice.Update();

                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        oCmp = oCompany;
                        return false;
                    }
                    else
                    {
                        oCmp = oCompany;
                        return true;
                    }
                }
                else
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    oCmp = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                oCmp = null;
                return false;
            }
        }

        public bool CreateReserveInvoice(int DocEntry, double Amount, out Company oCmp)
        {
            try
            {
                string ErrMsg;
                if (ConnectToSAPDI(out oCompany, out ErrMsg))
                {
                    if (!oCompany.InTransaction) oCompany.StartTransaction();

                    oInvoice = oCompany.GetBusinessObject(BoObjectTypes.oInvoices);
                    oInvoice.ReserveInvoice = BoYesNoEnum.tYES;
                    oInvoice.DocTotal = Amount;

                    var oLines = oOrder.Lines;

                    //oLines.BaseEntry = DocEntry;
                    //oLines.BaseLine = int.Parse((string)DataAccess.GetData(dt, i, "LineNum", "0")); ;
                    //oLines.BaseType = 23;
                    //oLines.ItemCode = (string)DataAccess.GetData(dt, i, "ItemCode", "");


                    lRetCode = oInvoice.Update();

                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lErrCode, out sErrMsg);
                        oCmp = oCompany;
                        return false;
                    }
                    else
                    {
                        oCmp = oCompany;
                        return true;
                    }
                }
                else
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    oCmp = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                oCmp = null;
                return false;
            }
        }
        public bool JournalEntries(string CardName, string ProjCode, string SQEntry, double Amount, double RGPRate, out string ErrMsg)
        {
            try
            {
                bool result = false;
                string module = $"JournalEntries";
                IDictionary<string, object> JournalEntries = new Dictionary<string, object>()
                {
                    {"Memo", CardName},
                    {"ProjectCode", ProjCode},
                    {"Reference", SQEntry}
                };

                //** realized gross as credit **//
                IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

                IDictionary<string, object> Lines = new Dictionary<string, object>();
                Lines.Add("AccountCode", ConfigSettings.RGP);
                Lines.Add("Credit", Amount * (RGPRate / 100));
                DocumentLines.Add(Lines);

                Lines.Add("AccountCode", ConfigSettings.UGP);
                Lines.Add("Credit", Amount * (RGPRate / 100));
                DocumentLines.Add(Lines);

                SapHanaLayer company = new SapHanaLayer();

                StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                var json = DataHelper.JsonBuilder(JournalEntries, DocumentLine);

                if (DocumentLines.Count > 0)
                {
                    if (company.POST(module, json))
                    {
                        ErrMsg = "";
                        result = true;
                    }
                    else
                    {
                        ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                        result = false;
                    }
                }
                else
                {
                    ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public string PostAP(string Method
                    , string SAODocEntry
                    , string oPosition
                    , string oRelease
                    , string oCardCode
                    , string oItemProp
                    , string CashReplenishment = ""
                    , string DocDate = "1990-01-01"
                    //, DataTable dt
                    )
        {
            try
            {

                string oSAPDB = ConfigurationManager.AppSettings["HANADatabase"];

                //dt = dt.Select($"DocEntry = '{SAODocEntry}'").CopyToDataTable();




                //string qryComs = $@"SELECT T0.""DocEntry"", T0.""DocNum"" , T0.""AcctType""
                //                             , B2.""PositionSharedDetails"" as ""Position""
                //                             , B2.""SalesPersonNameSharedDetails""
                //                             , S1.""U_Release"" as ""Release""
                //                             , C1.""FirstName"" || ' ' || C1.""LastName"" as ""CardName"" 
                //                             , T0.""ProjCode"", T0.""Block"", T0.""Lot"", T0.""Model""
                //                             , B2.""OslaID"" as ""Id""
                //                             , S2.""CardCode"" as ""SAPCardCode""
                //                             , T0.""NetTcp""
                //                             , T1.""PaidAmnt""
                //                             , S1.""U_CollectedTCP""
                //                             , S1.""U_CommissionRelease""
                //                             , (T1.""PaidAmnt"" / T0.""NetTcp"") *100 as ""CollectedPerct""
                //                             , case when(T1.""PaidAmnt"" / T0.""Das"") * 100 >= S1.""U_CollectedTCP"" Then((S1.""U_CommissionRelease"" / 100) * (T0.""Das""/1.12)) *(B2.""PercentageSharedDetails"" / S3.""U_Commission"") else 0 End as ""Amount""
                //                            FROM OQUT T0 INNER JOIN CRD1 C1 ON T0.""CardCode"" = C1.""CardCode"" AND C1.""CardType"" = 'Buyer'
                //                            /**--RESERV AND DP BREAKDOWN--**/  INNER JOIN(SELECT ""DocEntry"", SUM(""AmountPaid"") as ""PaidAmnt"" FROM QUT1 WHERE ""PaymentType"" <> 'MISC' group by ""DocEntry"") AS T1 ON T0.""DocEntry"" = T1.""DocEntry""
                //                            /**--AGENT TAG--**/                INNER JOIN QUT5 T5 ON T0.""DocEntry"" = T5.""DocEntry""
                //                            /**--AGENT DATA--**/               INNER JOIN OSLA A1 ON T5.""EmpCode"" = CAST(A1.""Id"" AS NVARCHAR(30))
                //                            /**--BROKER TAG--**/               INNER JOIN BRK1 BR ON A1.""Id"" = BR.""Id""
                //                            /**--SHARING INFO--**/			   INNER JOIN BRK2 B2 ON BR.""Id"" = B2.""SalesPersonId""
                //                            /**--SAP SCHEME--**/            INNER JOIN ""{oSAPDB}"".""@COMMISSION"" S1 ON T0.""ProjCode"" || '-' || UPPER(T0.""AcctType"") = UPPER(S1.""Code"") 
                //                            /**--SAP BP--**/                INNER JOIN ""{oSAPDB}"".""OCRD"" S2 ON B2.""OslaID"" = S2.""U_SalesAgentCode""
                //                            /** -- COMMISSION HEADER --**/  LEFT JOIN ""{oSAPDB}"".""@COMMINCSCHEME"" S3 ON S1.""Code"" = S3.""Code""
                //                            WHERE T0.""DocEntry"" = '{SAODocEntry}'
                //                                    AND B2.""PositionSharedDetails"" = '{oPosition}'
                //                                    AND S1.""U_Release"" = '{oRelease}'
                //                                    AND S2.""CardCode"" = '{oCardCode}'; ";

                //string qryInce = $@"SELECT T0.""DocEntry"", T0.""DocNum"" , T0.""AcctType""
                //                             , A1.""Position"" as ""Position""
                //                             , A1.""SalesPerson"" 
                //                             , S1.""U_Release"" as ""Release""
                //                             , C1.""FirstName"" || ' ' || C1.""LastName"" as ""CardName"" 
                //                             , S1.""U_Amount"" as ""Amount""
                //                             , T0.""ProjCode""
                //                             , T0.""Block""
                //                             , T0.""Lot""
                //                             , A1.""SAPCardCode""
                //                             , A1.""Id""
                //                             , T0.""Model""
                //                             , A1.""VATCode""
                //                             , A1.""WTaxCode""
                //                            FROM OQUT T0 INNER JOIN CRD1 C1 ON T0.""CardCode"" = C1.""CardCode"" AND C1.""CardType"" = 'Buyer'
                //                            /**--RESERV AND DP BREAKDOWN--**/  INNER JOIN QUT1 T1 ON T0.""DocEntry"" = T1.""DocEntry"" AND T1.""PaymentType"" = 'RES'
                //                            /**--AGENT TAG--**/                INNER JOIN QUT5 T5 ON T0.""DocEntry"" = T5.""DocEntry""
                //                            --/**--AGENT DATA--**/             INNER JOIN OSLA A1 ON T5.""EmpCode"" = CAST(A1.""TransID"" AS NVARCHAR(30))
                //                            --/**--BROKER TAG--**/             LEFT JOIN BRK1 BR ON A1.""Id"" = BR.""Id""
                //                            --/**--BROKER INFO--**/			   LEFT JOIN OBRK BK ON BR.""BrokerId"" = BK.""BrokerId""

                //                                                            INNER JOIN(
                //                                                                select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Sales Agent' as ""Position"", A.""Id"", A.""SalesPerson"", B.""SAPCardCode"" , A.""VATCode"", A.""WTaxCode""
                //                                                                FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id""
                //                                                                union all
                //                                                                select CAST(A.""TransID"" AS NVARCHAR(30)) as ""TransID"", 'Broker' as ""Position"", A.""Id"", C.""FirstName"" || ' ' || C.""MiddleName"" || ' ' || C.""LastName"", C.""SAPCardCode"", J.""VATCode"", J.""WTaxCode""
                //                                                                FROM OSLA A inner join BRK1 B on A.""Id"" = B.""Id"" inner join OBRK C on B.""BrokerId"" = C.""BrokerId"" inner join BRK1 K on C.""BrokerId"" = K.""BrokerId"" AND B.""Id"" = K.""Id"" inner join OSLA J ON K.""Id"" = J.""Id""
                //                                                                ) AS A1 ON T5.""EmpCode"" = CAST(A1.""Id"" AS NVARCHAR(30))
                //                            /**--SAP INCENTIVE--**/            LEFT JOIN ""{ oSAPDB}"".""@INCENTIVE"" S1 ON UPPER(T0.""ProjCode"") || '-' || UPPER(T0.""AcctType"") = CAST(UPPER(S1.""Code"") AS NVARCHAR(100)) AND A1.""Position"" = S1.""U_Position""
                //                             WHERE T0.""DocEntry"" = '{SAODocEntry}'
                //                                    AND A1.""Position"" = '{oPosition}'
                //                                    AND S1.""U_Release"" = '{oRelease}'
                //                                    AND A1.""SAPCardCode"" = '{oCardCode}'; ";

                //string qry = "";
                //qry = (Method == "Incentives") ? qryInce : qryComs;

                //DataTable dt = hana.GetData($@"{qry}", hana.GetConnection("SAOHana"));

                DataTable dt = new DataTable();

                if (Method == "Incentives")
                {
                    dt = ws.GetIncentivePosting(oSAPDB, SAODocEntry, oPosition, oRelease, oCardCode).Tables["GetIncentivePosting"];
                }
                else
                {
                    dt = ws.GetCommissionPosting(oSAPDB, SAODocEntry, oPosition, oRelease, oCardCode).Tables["GetCommissionPosting"];
                }




                bool result = false;
                string msg = "error";

                int SAPDocEntry = 0;

                foreach (DataRow item in dt.Rows)
                {
                    // ** header **//
                    string module = $"PurchaseInvoices";
                    IDictionary<string, object> DownPayments = new Dictionary<string, object>();
                    IList<IDictionary<string, object>> DPWTCollection = new List<IDictionary<string, object>>();

                    string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{item["ProjCode"].ToString()}'";
                    DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
                    string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
                    string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


                    string CardCode = "";
                    if (CashReplenishment == "Cash")
                    {
                        string qryGetCashierPerLocation = $@"SELECT * FROM ""@CASHPAYEE"" WHERE ""Name"" = '{Location}'";
                        DataTable dtGetGetCashierPerLocation = hana.GetData(qryGetCashierPerLocation, hana.GetConnection("SAPHana"));

                        //CardCode = ConfigSettings.ReplenishmentBP"].ToString();
                        if (Method == "Incentives")
                        {
                            CardCode = DataAccess.GetData(dtGetGetCashierPerLocation, 0, "U_BPCodeIncentive", ConfigSettings.ReplenishmentBP).ToString();
                        }
                        else
                        {
                            CardCode = DataAccess.GetData(dtGetGetCashierPerLocation, 0, "U_BPCodeCommission", ConfigSettings.ReplenishmentBP).ToString();
                        }
                    }
                    else
                    {
                        CardCode = oCardCode;
                    }

                    if (Method == "Incentives")
                    {


                        DownPayments = new Dictionary<string, object>()
                        {
                        //{"CardCode", $"{item["SAPCardCode"].ToString()}"},
                        {"CardCode", $"{CardCode}"},

                        //2023-11-16 : ADD DATE FROM DATE TODAY
                        {"DocDate", Convert.ToDateTime(DocDate).ToString("yyyyMMdd")},
                        {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                        {"Project", $"{item["ProjCode"].ToString()}"},
                        {"NumAtCard", $"{item["DocNum"].ToString()}"},
                        {"U_BlockNo", $"{item["Block"].ToString()}"},
                        {"U_LotNo", $"{item["Lot"].ToString()}"},
                        {"U_HouseModel", $"{item["Model"].ToString()}" },
                        {"U_PaymentOrder", $"{item["Release"].ToString()}"},
                        {"U_Remarks", $"{item["CardName"].ToString()} Project {item["ProjCode"].ToString()} Block no. {item["Block"].ToString()} Lot no. {item["Lot"].ToString()} - Release stage no. {item["Release"].ToString()} "},
                        {"JournalMemo", $"{item["CardName"].ToString()} Project {item["ProjCode"].ToString()} Block no. {item["Block"].ToString()} Lot no. {item["Lot"].ToString()} - Release stage no. {item["Release"].ToString()} "},
                        {"U_DreamsQuotationNo", $"{item["DocNum"].ToString()}"},
                        {"U_TransactionType", $"INCENTIVE"},
                        {"U_Branch", Location},
                        {"U_SortCode", SortCode},
                        {"U_SalesAgent",oCardCode}
                        };
                    }
                    else
                    {
                        DownPayments = new Dictionary<string, object>()
                        {
                        //{"CardCode", $"{item["SAPCardCode"].ToString()}"},
                        {"CardCode", $"{CardCode}"},

                        //2023-11-16 : ADD DATE FROM DATE TODAY
                        {"DocDate", Convert.ToDateTime(DocDate).ToString("yyyyMMdd")},
                        {"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
                        {"Project", $"{item["ProjCode"].ToString()}"},
                        {"NumAtCard", $"{item["DocNum"].ToString()}"},
                        {"U_BlockNo", $"{item["Block"].ToString()}"},
                        {"U_LotNo", $"{item["Lot"].ToString()}"},
                        {"U_HouseModel", $"{item["Model"].ToString()}" },
                        {"U_PaymentOrder", $"{item["Release"].ToString()}"},
                        {"U_Remarks", $"{item["CardName"].ToString()} Project {item["ProjCode"].ToString()} Block no. {item["Block"].ToString()} Lot no. {item["Lot"].ToString()} - Release stage no. {item["Release"].ToString()} "},
                        {"JournalMemo", $"{item["CardName"].ToString()} Project {item["ProjCode"].ToString()} Block no. {item["Block"].ToString()} Lot no. {item["Lot"].ToString()} - Release stage no. {item["Release"].ToString()} "},
                        //{"U_Remarks", $"Release stage no. {item["Release"].ToString()} for the account of {item["CardName"].ToString()} Project {item["ProjCode"].ToString()} Block no. {item["Block"].ToString()} Lot no. {item["Lot"].ToString()} "},
                        {"U_DreamsQuotationNo", $"{item["DocNum"].ToString()}"},
                        {"U_TransactionType", $"COMMI"},
                        {"U_Branch", Location},
                        {"U_SortCode", SortCode},
                        {"U_SalesAgent",oCardCode}
                        };


                        DataTable dtSAPBPWtax = hana.GetData($@"SELECT ""WTCode"" FROM OCRD WHERE ""CardCode"" = '{CardCode}' ", hana.GetConnection("SAPHana"));
                        if (DataAccess.Exist(dtSAPBPWtax))
                        {
                            foreach (DataRow items in dtSAPBPWtax.Rows)
                            {
                                var WTLines = new Dictionary<string, object>();
                                WTLines.Add("WTCode", $"{items["WTCode"]}");
                                DPWTCollection.Add(WTLines);
                                break;
                            }
                        }

                    }


                    //DownPayments.Add("WithholdingTaxDataCollection", DPWTCollection);

                    IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
                    oItemProp = (string.IsNullOrEmpty(oItemProp) ? ConfigSettings.IncentiveProperty : oItemProp);
                    DataTable dtItems = hana.GetData($@"SELECT TOP 1 A.""ItemCode"" FROM OITM A Where A.""{oItemProp}"" = 'Y'; ", hana.GetConnection("SAPHana"));
                    DataTable dtSAPBPVat = hana.GetData($@"SELECT T0.""ECVatGroup"" FROM OCRD T0 WHERE T0.""CardCode""='{oCardCode}' ", hana.GetConnection("SAPHana"));
                    if (DataAccess.Exist(dtItems))
                    {
                        var Lines = new Dictionary<string, object>();
                        string itemCode = string.Format("{0}", dtItems.Rows[0]["ItemCode"].ToString());
                        //** adding of line Items **//
                        Lines.Add("ItemCode", itemCode);
                        Lines.Add("Quantity", 1);
                        Lines.Add("CostingCode", ConfigSettings.IncentiveCommissionDepartment);
                        Lines.Add("CostingCode2", ConfigSettings.IncentiveCommissionBranch);
                        Lines.Add("ProjectCode", item["ProjCode"].ToString());

                        //Added 20220805_1540
                        Lines.Add("PriceAfterVAT", item["Amount"].ToString());


                        if (Method == "Incentives")
                        {
                            Lines.Add("VatGroup", ConfigSettings.IncentiveVATGroup);
                            //Lines.Add("PriceAfterVAT", item["Amount"].ToString());
                            Lines.Add("WTLiable", "tNO");
                        }
                        else
                        {
                            if (DataAccess.Exist(dtSAPBPVat))
                            {
                                Lines.Add("VatGroup", $"{dtSAPBPVat.Rows[0]["ECVatGroup"]}");
                            }
                            //Lines.Add("LineTotal", item["Amount"].ToString());
                        }
                        DocumentLines.Add(Lines);
                    }

                    //############################## [POSTING HERE] ##############################
                    SapHanaLayer company = new SapHanaLayer();
                    result = company.Connect();

                    StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
                    StringBuilder WithholdingTaxDataCollection = DataHelper.JsonLinesBuilder("WithholdingTaxDataCollection", DPWTCollection);
                    var jsonlines = DataHelper.JsonLinesCombine(DocumentLine, WithholdingTaxDataCollection);
                    var json = DataHelper.JsonBuilder(DownPayments, jsonlines);


                    if (DocumentLines.Count > 0)
                    {
                        if (company.POST(module, json))
                        {
                            msg = $"{module} successfully created.";
                            SAPDocEntry = int.Parse(company.ResultDescription);
                            result = true;

                            hana.Execute($@"INSERT INTO ""OPCH"" 
                                            (""DocEntry"",
                                                ""SAPDocEntry"",
                                                ""TransType"",
                                                ""SalesAgentID"",
                                                ""CreateDate"",
                                                ""UpdateDate"",
                                                ""Position"",
                                                ""Release"") 
                                            VALUES
                                               ('{SAODocEntry}'
		                                        ,'{SAPDocEntry}'
                                                ,'{Method}'
                                                ,'{item["Id"].ToString()}'
                                                ,'{DateTime.Now.ToString("yyyy/MM/dd")}'
                                                ,'{DateTime.Now.ToString("yyyy/MM/dd")}'
                                                ,'{item["Position"].ToString()}'
                                                ,'{item["Release"].ToString()}'
                                               ); ", hana.GetConnection("SAOHana"));
                        }
                        else
                        {
                            msg = $"({company.ResultCode}) {company.ResultDescription}";
                            result = false;
                        }
                    }
                    else
                    {
                        msg = $"({company.ResultCode}) {company.ResultDescription}";
                        result = false;
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {

                return $@"error: {ex.Message}";
            }
        }

    }
}
