using SAPbobsCOM;
using System;
using System.Configuration;
using System.Data;

namespace ABROWN_DREAMS
{
    class SAPAccess
    {
        public static BoDataServerTypes oSapServerType;
        public static Company oCompany;
        public static Recordset oRecordset;
        public static int lRetCode, lErrCode;
        public static string sErrMsg;
        public static SAPbobsCOM.Documents oOrders, oInvoices, oQuotation;
        public static StockTransfer oTransferRequest;
        public static Payments oPayments;
        public static BusinessPartners oBP;
        public static Boolean InitializeCompany(out string ErrMsg)
        {
            try
            {
                string LicenseServer = ConfigurationManager.AppSettings["LicenseServer"].ToString();
                string LicensePort = ConfigurationManager.AppSettings["LicenseServerPort"].ToString();
                string SAPServer = ConfigurationManager.AppSettings["SAPServer"].ToString();
                string DBUsername = DataAccess.GetconDetails("SAP", "User ID");
                string DBPassword = DataAccess.GetconDetails("SAP", "Password");

                oCompany = new Company()
                {
                    LicenseServer = $"{LicenseServer}:{LicensePort}",
                    Server = SAPServer,
                    language = BoSuppLangs.ln_English,
                    UseTrusted = true,
                    DbUserName = DBUsername,
                    DbPassword = DBPassword,
                    DbServerType = BoDataServerTypes.dst_MSSQL2014
                };
                ErrMsg = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }

        public static Company ConnectToSAPDI(out string ErrMsg)
        {
            try
            {
                string ServerType = ConfigurationManager.AppSettings["ServerType"].ToString();
                string LicenseServer = ConfigurationManager.AppSettings["LicenseServer"].ToString();
                string LicensePort = ConfigurationManager.AppSettings["LicenseServerPort"].ToString();
                string SAPServer = ConfigurationManager.AppSettings["SAPServer"].ToString();
                string SAPServerPort = ConfigurationManager.AppSettings["SAPServerPort"].ToString();
                string LicenseID = ConfigurationManager.AppSettings["LicenseID"].ToString();
                string LicensePassword = ConfigurationManager.AppSettings["LicensePassword"].ToString();
                string SapDatabase = DataAccess.GetconDetails("SAP", "Initial Catalog");
                string DBUsername = DataAccess.GetconDetails("SAP", "User ID");
                string DBPassword = DataAccess.GetconDetails("SAP", "Password");
                //CONNECTING TO SAP DATABASE
                oCompany = new Company()
                {
                    LicenseServer = $"{LicenseServer}:{LicensePort}",
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
                lRetCode = oCompany.Connect();
                if (lRetCode != 0) // if the connection failed
                {
                    oCompany.GetLastError(out lErrCode, out sErrMsg);

                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return null;
                }
                else
                {
                    ErrMsg = null;
                    return oCompany;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return null;
            }
        }
        public static Boolean UploadBusinessPartner(string CardName, Company company, out string CardCode, out string ErrMsg)
        {
            try
            {
                string cardCode = null;
                oBP = company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                oBP.Series = int.Parse(ConfigSettings.BPSeries);
                oBP.CardType = BoCardTypes.cCustomer;
                oBP.CardName = CardName;
                lRetCode = oBP.Add();

                if (lRetCode != 0)
                {
                    company.GetLastError(out lErrCode, out sErrMsg);
                    oBP.GetByKey(cardCode);
                    ErrMsg = lErrCode + "-" + sErrMsg;
                    CardCode = cardCode;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    CardCode = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                CardCode = null;
                return false;
            }
        }
        public static Boolean UploadSalesQuotation(string CardCode, DataTable dtItems, Company company, out string ErrMsg)
        {
            try
            {
                oQuotation = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                oQuotation.CardCode = CardCode;

                foreach (DataRow row in dtItems.Rows)
                {
                    //** adding of line Items **//
                    oQuotation.Lines.ItemCode = "";
                    oQuotation.Lines.Quantity = 1;
                    oQuotation.Lines.Add();
                }

                lRetCode = oQuotation.Add();

                if (lRetCode != 0)
                {
                    company.GetLastError(out lErrCode, out sErrMsg);

                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public static Boolean UploadARDownPayment(string CardCode, DataTable dtItems, Company company, out string ErrMsg)
        {
            try
            {
                oInvoices = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                oInvoices.DocType = BoDocumentTypes.dDocument_Items;
                oInvoices.DownPaymentType = DownPaymentTypeEnum.dptRequest;
                oInvoices.CardCode = CardCode;

                foreach (DataRow row in dtItems.Rows)
                {
                    //** adding of line Items **//
                    oInvoices.Lines.ItemCode = "";
                    oInvoices.Lines.Quantity = 1;
                    oInvoices.Lines.Add();
                }

                lRetCode = oInvoices.Add();

                if (lRetCode != 0)
                {
                    company.GetLastError(out lErrCode, out sErrMsg);

                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public static Boolean UploadIncomingPayments(string CardCode, DataTable dtItems, Company company, out string ErrMsg)
        {
            try
            {
                oPayments = company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);
                oPayments.DocDate = DateTime.Now;
                oPayments.DocTypte = SAPbobsCOM.BoRcptTypes.rAccount;
                oPayments.AccountPayments.AccountCode = "_SYS00000000026";
                oPayments.AccountPayments.SumPaid = 100;
                oPayments.AccountPayments.Add();

                lRetCode = oPayments.Add();

                foreach (DataRow row in dtItems.Rows)
                {
                    //** adding of line Items **//
                    oInvoices.Lines.ItemCode = "";
                    oInvoices.Lines.Quantity = 1;
                    oInvoices.Lines.Add();
                }

                lRetCode = oInvoices.Add();

                if (lRetCode != 0)
                {
                    company.GetLastError(out lErrCode, out sErrMsg);

                    ErrMsg = lErrCode + "-" + sErrMsg;
                    return false;
                }
                else
                {
                    ErrMsg = null;
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
        }
        public static void UploadHeader(string xmlFileName, string SINo,
                                    string oCardCode, DateTime oDocDate,
                                    string oProject, string oCashierCode,
                                    string oTerminalID, string oCashierName,
                                    SAPbobsCOM.Documents oDoc, BoObjectTypes oObj,
                                    string oDocSeries, string ObjectCode, out string ErrMsg)
        {
            try
            {
                oDoc.DocObjectCode = oObj;

                //===============UPLOADING HEADER==============//
                DataTable dtDocSeries = new DataTable();
                string _sdata;

                oDoc.CardCode = oCardCode;
                oDoc.DocDate = oDocDate;
                oDoc.DocDueDate = oDocDate;
                oDoc.TaxDate = oDocDate;
                oDoc.Comments = "POSTED BY LINKBOX" + " | " + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt");
                oDoc.UserFields.Fields.Item("U_Remarks").Value = xmlFileName;
                oDoc.UserFields.Fields.Item("U_SINo").Value = SINo;
                oDoc.UserFields.Fields.Item("U_PONo").Value = SINo;
                oDoc.UserFields.Fields.Item("U_TransCat").Value = "Declared";
                oDoc.Project = oProject;
                oDoc.UserFields.Fields.Item("U_Cashier_Codes").Value = oCashierCode;
                oDoc.UserFields.Fields.Item("U_TerminalID").Value = oTerminalID;
                oDoc.UserFields.Fields.Item("U_Cashier_Name").Value = oCashierName;
                oDoc.UserFields.Fields.Item("U_CheckBy").Value = xmlFileName;
                oDoc.UserFields.Fields.Item("U_PrepBy").Value = xmlFileName;
                oDoc.UserFields.Fields.Item("U_AppBy").Value = xmlFileName;
                //U_CheckBy
                oDoc.DocType = BoDocumentTypes.dDocument_Items;
                //=======================================//
                ErrMsg = null;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
        }

        public static void UploadRows(string oDocument, string oProject, string oEmployee, string oItemCode,
                                    double oUnitPrice, double oDiscount, double oQuantity, int oUoM,
                                    string oCustType, string oORnumber, string oVatGroup, string oWhsCode,
                                    SAPbobsCOM.Documents oDoc, out string ErrMsg)
        {
            try
            {
                ErrMsg = null;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
            }
        }

        public static void UploadPayHeader(Payments oPay, string oCardCode, DateTime oDocDate, string oRemarks, out string ErrMsg)
        {
            try
            {

                //=============UPLOADING PAYMENT HEADER===============//
                oPay.CardCode = oCardCode;
                oPay.DocDate = oDocDate;
                oPay.TaxDate = oDocDate;
                oPay.DocType = BoRcptTypes.rCustomer;
                oPay.Remarks = oRemarks;
                //switch (oPayment)
                //{
                //    case "CHECK":
                //        oPay.Checks.CheckAccount = oAccount;
                //        oPay.Checks.CheckSum = oAmount;
                //        //'10011003'
                //        break;
                //    case "BANK":
                //        oPay.BankChargeAmount = oAmount;
                //        oPay.BankAccount = oAccount;
                //        break;
                //    case "CASH":
                //        oPay.CashSum = oAmount;
                //        oPay.CashAccount = oAccount;
                //        break;
                //}
                //=======================================//
                ErrMsg = "";
            }
            catch (Exception ex)
            { ErrMsg = ex.Message; }
        }

        public static void UploadPayRows(int oInvoice, string oPaymentMode, double oCheckSum,
                                        string oActCode, string oBranch, Payments oPay)
        {
            try
            {

                switch (oPaymentMode)
                {
                    case "CHECK":
                        oPay.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                        oPay.Invoices.DocEntry = oInvoice;
                        oPay.Checks.DueDate = DateTime.Now;
                        oPay.Checks.CheckSum = oCheckSum;
                        oPay.Checks.AccounttNum = oActCode;
                        oPay.Checks.Branch = oBranch;
                        oPay.Checks.Trnsfrable = SAPbobsCOM.BoYesNoEnum.tNO;
                        oPay.ApplyVAT = SAPbobsCOM.BoYesNoEnum.tNO;
                        oPay.Checks.Add();
                        break;
                }

                //=============UPLOADING PAYMENT ROWS===============//
                //oPay.Invoices.InvoiceType = BoRcptInvTypes.it_Invoice;
                //oPay.Invoices.DocEntry = oInvoice;
                //oPay.Invoices.Add();
                //=======================================//
            }
            catch (Exception ex)
            {

            }
        }


        public static void UploadPayMeans(string oPayment, string oAccount, double oAmount,
                                  Payments oPay)
        {
            try
            {

                //=============UPLOADING PAYMENT MEANS===============//
                switch (oPayment)
                {
                    case "CHECK":
                        oPay.Checks.DueDate = DateTime.Now;
                        oPay.Checks.CheckSum = oAmount;
                        oPay.Checks.CountryCode = "PH";
                        oPay.Checks.BankCode = "EWB";
                        oPay.Checks.AccounttNum = "";
                        oPay.Checks.Add();
                        break;
                    case "BANK":
                        oPay.BankChargeAmount = oAmount;
                        oPay.BankAccount = oAccount;
                        break;
                    case "CREDIT":
                        oPay.CreditCards.CreditSum = oAmount;
                        oPay.CreditCards.CreditAcct = oAccount;
                        oPay.CreditCards.Add();
                        break;
                    case "CASH":
                        oPay.CashSum = oAmount;
                        oPay.CashAccount = "10011003";
                        break;
                }
                //=======================================//
            }
            catch (Exception ex)
            {

            }
        }
    }
}
