using DirecLayer;
using System.Configuration;

namespace ABROWN_DREAMS
{

    public static class ConfigSettings
    {
        static SAPHanaAccess _hana = _hana ?? new SAPHanaAccess();

        //========= Master Data =========//
        public static string PRJCode = GetValue("MD", "PRJCode");
        public static string PRJName = GetValue("MD", "PRJName");
        public static string DefaultPaymentTermForVendor = GetValue("MD", "DefaultPaymentTermForVendor");
        public static string PayTermsGrpCode = GetValue("MD", "PayTermsGrpCode");
        public static string BPPriceList = GetValue("MD", "BPPriceList");
        public static string ATCCode = GetValue("MD", "ATCCode");
        public static string BIRCardCode = GetValue("MD", "BIRCardCode");
        public static string CommissionProperty = GetValue("MD", "CommissionProperty");
        public static string IncentiveProperty = GetValue("MD", "IncentiveProperty");
        public static string SurchargeProperty = GetValue("MD", "SurchargeProperty");
        public static string IncentiveCommissionDepartment = GetValue("MD", "IncentiveCommissionDepartment");
        public static string IncentiveCommissionBranch = GetValue("MD", "IncentiveCommissionBranch");
        public static string ReplenishmentBP = GetValue("MD", "ReplenishmentBP");
        public static string ExpenseCode = GetValue("MD", "ExpenseCode");
        public static string PenaltyItemCode = GetValue("MD", "PenaltyItemCode");
        public static string InterestItemCode = GetValue("MD", "InterestItemCode");


        //========= Groups =========//
        public static string TitleInventoryItemGroup = GetValue("Groups", "TitleInventoryItemGroup");
        public static string BrokerBPGroup = GetValue("Groups", "BrokerGroupGroup");
        public static string VendorGroupCode = GetValue("Groups", "VendorGroupCode");
        public static string InterestVatableVATGroup = GetValue("Groups", "InterestVatableVATGroup");
        public static string InterestNONVatableVATGroup = GetValue("Groups", "InterestNONVatableVATGroup");
        public static string IPSVatableVATGroup = GetValue("Groups", "IPSVatableVATGroup");
        public static string IPSNONVatableVATGroup = GetValue("Groups", "IPSNONVatableVATGroup");
        public static string SurchargeVATGroup = GetValue("Groups", "SurchargeVATGroup");
        public static string MiscVATGroup = GetValue("Groups", "MiscVATGroup");
        public static string ExcessVATGroup = GetValue("Groups", "ExcessVATGroup");
        public static string IncentiveVATGroup = GetValue("Groups", "IncentiveVATGroup");




        //2023-09-18 : ADD SURCHARGE VAT GROOUP EX
        public static string SurchargeExVATGroup = GetValue("Groups", "SurchargeExVATGroup");

        //========= Series =========//
        public static string BrokerBPSeries = GetValue("Series", "BrokerBPSeries");
        public static string BPSeries = GetValue("Series", "BPSeries");
        public static string QuotationSeries = GetValue("Series", "QuotationSeries");
        public static string SalesOrderSeries = GetValue("Series", "SalesOrderSeries");
        public static string ARInvoiceSeries = GetValue("Series", "ARInvoiceSeries");
        public static string ARCreditMemoSeries = GetValue("Series", "ARCreditMemoSeries");
        public static string IncomingPaymentSeries = GetValue("Series", "IncomingPaymentSeries");


        //========= General Settings =========//
        public static string Currency = GetValue("GS", "Currency");
        public static string CreditValidDatePrefix = GetValue("GS", "CreditValidDatePrefix");
        public static string TaxClassification1 = GetValue("GS", "TaxClassification1");
        public static string TaxClassification2 = GetValue("GS", "TaxClassification2");
        public static string TaxEndOfYear_Month = GetValue("GS", "TaxEndOfYear_Month");
        public static string TaxEndOfYear_Day = GetValue("GS", "TaxEndOfYear_Day");
        public static string PaymentScheme1 = GetValue("GS", "PaymentScheme1");
        public static string PaymentScheme2 = GetValue("GS", "PaymentScheme2");
        public static string RestructuringWithLTS = GetValue("GS", "RestructuringWithLTS");
        public static string ATPDateExp = GetValue("GS", "ATPDateExp");
        public static string ARReserveInvoiceCondition = GetValue("GS", "ARReserveInvoiceCondition");
        public static string Forwarded = GetValue("GS", "Forwarded");
        public static string AddtlChargesBuffer = GetValue("GS", "AddtlChargesBuffer");


        //========= Account Codes =========//
        public static string ClearingAccount = GetValue("AccountCodes", "ClearingAccount"); //2024-01-16: DEPOSIT FROM CUSTOMER
        public static string CashAccount = GetValue("AccountCodes", "CashAccount");
        public static string CheckAccount = GetValue("AccountCodes", "CheckAccount");
        public static string ARClearingAccount = GetValue("AccountCodes", "ARClearingAccount");
        public static string APOthersAccount = GetValue("AccountCodes", "APOthersAccount");
        public static string CreditableWithholdingTaxAccount = GetValue("AccountCodes", "CreditableWithholdingTaxAccount");
        public static string OutputVATClearingAccount = GetValue("AccountCodes", "OutputVATClearingAccount");
        public static string OutputVATAccount = GetValue("AccountCodes", "OutputVATAccount");
        public static string SalesREVATAccount = GetValue("AccountCodes", "SalesREVATAccount");
        public static string CostOfSalesCollected = GetValue("AccountCodes", "CostOfSalesCollected");
        public static string UnearnedIncomeAccount = GetValue("AccountCodes", "UnearnedIncomeAccount");
        public static string CostOfSalesRE = GetValue("AccountCodes", "CostOfSalesRE");
        public static string CostOfSalesUncollected = GetValue("AccountCodes", "CostOfSalesUncollected");
        public static string SalesCollectedAccount = GetValue("AccountCodes", "SalesCollectedAccount");
        public static string SalesUncollectedAccount = GetValue("AccountCodes", "SalesUncollectedAccount");
        public static string AccumulatedCostOfSalesAccount = GetValue("AccountCodes", "AccumulatedCostOfSalesAccount");
        public static string ContractReceivablesDeferredAccount = GetValue("AccountCodes", "ContractReceivablesDeferredAccount");
        public static string ContractReceivablesInstallmentAccount = GetValue("AccountCodes", "ContractReceivablesInstallmentAccount");
        public static string SalesUncollectedVATAccount = GetValue("AccountCodes", "SalesUncollectedVATAccount");
        public static string CostOfSalesClearingAccount = GetValue("AccountCodes", "CostOfSalesClearingAccount");
        public static string RetitlingPayableLotAccount = GetValue("AccountCodes", "RetitlingPayableLotAccount");
        public static string AccountsPayableAccount = GetValue("AccountCodes", "AccountsPayableAccount");
        public static string CommissionExpenseAccount = GetValue("AccountCodes", "CommissionExpenseAccount");
        public static string PrepaidCommissionAccount = GetValue("AccountCodes", "PrepaidCommissionAccount");
        public static string SalesCollectedNonVAT = GetValue("AccountCodes", "SalesCollectedNonVAT");
        public static string SalesUncollectedNonVAT = GetValue("AccountCodes", "SalesUncollectedNonVAT");
        public static string IPSVATAccount = GetValue("AccountCodes", "IPSVATAccount");
        public static string IPSNonVATAccount = GetValue("AccountCodes", "IPSNonVATAccount");
        public static string IPSControlAccount = GetValue("AccountCodes", "IPSControlAccount"); //2024-01-16: ACCOUNTS RECEIVABLE - CLEARING 
        public static string ExcessControlAccount = GetValue("AccountCodes", "ExcessControlAccount");
        public static string InterestVATAccount = GetValue("AccountCodes", "InterestVATAccount");
        public static string InterestNonVATAccount = GetValue("AccountCodes", "InterestNonVATAccount");
        public static string ExcessVATAccount = GetValue("AccountCodes", "ExcessVATAccount");
        public static string RGP = GetValue("AccountCodes", "RGP");
        public static string UGP = GetValue("AccountCodes", "UGP");

        //2023-12-06 : ADD SURCHARGE ACCOUNT CODE DEPENDS ON VAT GROUP
        public static string SuchargeVATAccount = GetValue("AccountCodes", "SuchargeVATAccount");
        public static string SuchargeNonVATAccount = GetValue("AccountCodes", "SuchargeNonVATAccount");
         
        //2023-06-30 : SALES COLLECTED DISCOUNT
        public static string SalesCollectedDiscount = GetValue("AccountCodes", "SalesCollectedDiscount");

        //2023-06-30 : SALES COLLECTED DISCOUNT
        public static string OutputTaxInterest = GetValue("AccountCodes", "OutputTaxInterest");
        public static string OutputTaxSurcharge = GetValue("AccountCodes", "OutputTaxSurcharge");



        //========= Forms and Reports =========//
        public static string QuotationForm = GetValue("RPT", "QuotationForm");
        public static string ORForm = GetValue("RPT", "ORForm");
        public static string ARForm = GetValue("RPT", "ARForm");
        public static string PRForm = GetValue("RPT", "PRForm");
        public static string SurchargeForm = GetValue("RPT", "SurchargeForm");
        public static string DemandLetterForm1 = GetValue("RPT", "DemandLetterForm1");
        public static string DemandLetterForm2 = GetValue("RPT", "DemandLetterForm2");
        public static string DemandLetterForm3 = GetValue("RPT", "DemandLetterForm3");
        public static string DemandLetterForm4 = GetValue("RPT", "DemandLetterForm4");
        public static string DemandLetterForm5 = GetValue("RPT", "DemandLetterForm5");
        public static string PDCForm = GetValue("RPT", "PDCForm");
        public static string DocumentRequirement = GetValue("RPT", "DocumentRequirement");
        public static string DocumentRequirementDAS = GetValue("RPT", "DocumentRequirementDAS");
        public static string BrokerApplication = GetValue("RPT", "BrokerApplication");
        public static string AccreditationAgreement = GetValue("RPT", "AccreditationAgreement");
        public static string BrokerAccreditationGeneralPolicies = GetValue("RPT", "BrokerAccreditationGeneralPolicies");
        public static string ListofAccreditedSalesPersons = GetValue("RPT", "ListofAccreditedSalesPersons");
        public static string BuyersInfoForm = GetValue("RPT", "BuyersInfoForm");
        public static string MapGenerationForm = GetValue("RPT", "MapGenerationForm");
        public static string SingleMapGenerationForm = GetValue("RPT", "SingleMapGenerationForm");


        static string GetValue(string group, string key)
        {
            string output = "";

            try
            {
                var dt = _hana.GetData($@"SELECT ""U_Value"" FROM ""@DREAMSR"" WHERE ""Code"" = '{group}' AND ""U_Key""='{key}'", _hana.GetConnection("SAPHana"));
                output = DataAccess.GetData(dt, 0, "U_Value", "").ToString();
            }
            catch
            {

            }

            return output;
        }
    }
}