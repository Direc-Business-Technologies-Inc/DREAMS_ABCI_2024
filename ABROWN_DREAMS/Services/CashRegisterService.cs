using ABROWN_DREAMS.Models;
using ABROWN_DREAMS.wcf;
using DirecLayer;
using MSXML2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web.UI.WebControls;

namespace ABROWN_DREAMS.Services
{
	public class CashRegisterService
	{
		public string ErrMsg { get; set; }
		double StandardCostFactorRate = 0;
		double _TotalPayment = 0;
		DirecWebService ws = new DirecWebService();
		SAPHanaAccess hana = new SAPHanaAccess();
		XMLHTTP60 ServiceLayer { get; set; }







		public bool CreateReservation(int DocEntry,
										string DocDate, //2
										string ORNumber,
										double Balance, //4
										double Payment,
										string DocNum, //6
										string Tagging,
										int DPTerms, //8
										int MiscTerms,
										out int SQEntry,
										out int DPEntry, //10
										out string SapCardCode,
										out string Message, //12

										[Optional] string Block,
										[Optional] string Lot,
										[Optional] string ProjectCode,
										[Optional] string HouseModel,
										[Optional] string FinancingScheme,
										[Optional] string ProductType,

										[Optional] double ReservationPayment,
										[Optional] double TCPDownpayment,
										[Optional] double TCPLoanableBalance,
										[Optional] double NetTCP,
										[Optional] string Vatable,
										[Optional] double DiscountAmount,
										[Optional] double MiscAmount,
										[Optional] double MonthlyMiscAmount,

										[Optional] string BatchNum,
										[Optional] string EmpName,
										[Optional] string SoldWithAdjacentLot,
										[Optional] string AdjacentLotQuotationNo,

										[Optional] string newCardCode,
										[Optional] string TaxDate
										) //10
		{
			bool output = false;
			SapHanaLayer company = new SapHanaLayer();
			SQEntry = 0;
			DPEntry = 0;
			SapCardCode = "";
			try
			{
				output = company.Connect();
				Message = $"({company.ResultCode}) {company.ResultDescription}";
				//int PymtEntry = 0;



				// ### GENERAL DATA HERE ### //
				#region General Data
				DataTable generalData = new DataTable();
				//string ProjectCode = "";
				//string Block = "";
				//string Lot = "";
				double totalAmount = 0;
				//double ReservationPayment = 0;
				try
				{
					generalData = ws.GetGeneralData(DocEntry, ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];


					totalAmount = double.Parse(DataHelper.DataTableRet(generalData, 0, "TotalAmount", ""));
					if (!DataHelper.DataTableExist(generalData))
					{
						Message = "Error : No Data Found upon generataing GetGeneralData SP";
						return false;
					}
				}
				catch (Exception ex)
				{
					Message = $"Error : GetGeneralData - {ex.Message}";
					return false;
				}
				#endregion

				// ### CREATION OF BUSINESS PARTNER ### //
				#region Business Partner
				// ### SAP TRANSACTION START HERE ### //
				string AddonCardCode = "";

				string NewCardName = "";
				string NewLastName = "";
				string NewFirstName = "";
				string NewMiddleName = "";
				string NewTIN = "";
				string NewTaxClassification = "";
				string NewSpecialBuyerRole = "";


				if (Tagging == "QUOTATION")
				{
					AddonCardCode = DataHelper.DataTableRet(generalData, 0, "CardCode", "");
					ProjectCode = DataHelper.DataTableRet(generalData, 0, "ProjCode", "");
					NewCardName = DataHelper.DataTableRet(generalData, 0, "CardName", "");
					NewLastName = DataHelper.DataTableRet(generalData, 0, "LastName", "");
					NewFirstName = DataHelper.DataTableRet(generalData, 0, "FirstName", "");
					NewMiddleName = DataHelper.DataTableRet(generalData, 0, "MiddleName", "");
					NewTIN = DataHelper.DataTableRet(generalData, 0, "TIN", "");
					NewTaxClassification = DataHelper.DataTableRet(generalData, 0, "TaxClassification", "");
					NewSpecialBuyerRole = DataHelper.DataTableRet(generalData, 0, "SpecialBuyerRole", "");
				}
				else
				{
					AddonCardCode = newCardCode;
					string qryRestructuringGetData = $@" select CASE WHEN A.""BusinessType"" <> 'Corporation' THEN B.""LastName"" || ', ' || B.""FirstName""
                                                     ELSE B.""CompanyName""	END ""CardName"", B.""LastName"",	B.""FirstName"", B.""MiddleName"",	IFNULL(B.""TIN"",A.""IDNo"") ""TIN"",
                                                     CASE WHEN UPPER(A.""TaxClassification"") = 'CORPORATION' THEN 'Engaged in Business' ELSE A.""TaxClassification"" END ""TaxClassification"", 
                                                     IFNULL(A.""SpecialBuyerRole"",'') ""SpecialBuyerRole"" from OCRD A inner join CRD1 B ON A.""CardCode"" = B.""CardCode"" 
                                                     where 	A.""CardCode"" = '{AddonCardCode}' AND	B.""CardType"" = 'Buyer'  ";
					DataTable dtRestructuringGetData = new DataTable();
					dtRestructuringGetData = hana.GetData(qryRestructuringGetData, hana.GetConnection("SAOHana"));

					if (dtRestructuringGetData.Rows.Count > 0)
					{
						NewCardName = DataHelper.DataTableRet(dtRestructuringGetData, 0, "CardName", "");
						NewLastName = DataHelper.DataTableRet(dtRestructuringGetData, 0, "LastName", "");
						NewFirstName = DataHelper.DataTableRet(dtRestructuringGetData, 0, "FirstName", "");
						NewMiddleName = DataHelper.DataTableRet(dtRestructuringGetData, 0, "MiddleName", "");
						NewTIN = DataHelper.DataTableRet(dtRestructuringGetData, 0, "TIN", "");
						NewTaxClassification = DataHelper.DataTableRet(dtRestructuringGetData, 0, "TaxClassification", "");
						NewSpecialBuyerRole = DataHelper.DataTableRet(dtRestructuringGetData, 0, "SpecialBuyerRole", "");
					}
				}

				output = CreateBusinessPartner(company,
													AddonCardCode,
													NewCardName,
													ProjectCode,
													NewLastName,
													NewFirstName,
													NewMiddleName,
													NewTIN,
													NewTaxClassification,
													NewSpecialBuyerRole,
													out SapCardCode,
													out Message);
				#endregion








				// ### CHECK IF PROCEED TO SALES QUOTATION ### //
				#region Sales Quotation

				if (output)
				{
					if (Tagging == "QUOTATION")
					{

						Block = DataHelper.DataTableRet(generalData, 0, "Block", "");
						Lot = DataHelper.DataTableRet(generalData, 0, "Lot", "");
						ReservationPayment = double.Parse(DataHelper.DataTableRet(generalData, 0, "ReservationAmount", ""));

						output = CreateSalesQuotation(company,
										  DocEntry, //2
										  SapCardCode,
										  DocDate, //4
										  ProjectCode,
										  Block, //6
										  Lot,
										  DataHelper.DataTableRet(generalData, 0, "HouseModel", ""), //8
										  DataHelper.DataTableRet(generalData, 0, "FinancingScheme", ""),
										  DataHelper.DataTableRet(generalData, 0, "ProductType", ""), //10
										  ReservationPayment,
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "TCPDownpayment", "0")), //12
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "TCPLoanableBalance", "0")),
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "NetTCP", "0")), //14
										  DataHelper.DataTableRet(generalData, 0, "Vatable", "No"),
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "DiscountAmount", "0")), //16
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "MiscAmount", "0")),
										  DataHelper.DataTableRet(generalData, 0, "BatchNum", ""), //18
										  DocNum,
										  DataHelper.DataTableRet(generalData, 0, "EmpName", ""), //20
										  DataHelper.DataTableRet(generalData, 0, "SoldWithAdjacentLot", ""), //22
										  DataHelper.DataTableRet(generalData, 0, "AdjacentLotQuotationNo", ""),
										  DPTerms,
										  double.Parse(DataHelper.DataTableRet(generalData, 0, "MiscFeesMonthly", "0")),
										  DocDate, //4
										  MiscTerms,
										  out SQEntry, //24
										  out Message
										  );
					}
					else
					{
						//RESTRUCTURING
						output = CreateSalesQuotation(company,
										DocEntry, //2
										SapCardCode,
										DocDate, //4
										ProjectCode,
										Block, //6
										Lot,
										HouseModel, //8
										FinancingScheme,
										ProductType, //10
										ReservationPayment,
										TCPDownpayment, //12
										TCPLoanableBalance,
										NetTCP, //14
										Vatable,
										DiscountAmount, //16
										MiscAmount,
										BatchNum, //18
										DocNum,
										EmpName,
										SoldWithAdjacentLot,
										AdjacentLotQuotationNo,
										DPTerms,
										MonthlyMiscAmount,
										TaxDate,
										MiscTerms,
										out SQEntry,
										out Message
										);//20
					}

				}
				else
				{
					if (!string.IsNullOrEmpty(SapCardCode))
					{
						StringBuilder model = new StringBuilder();
						model.Append(company.GET($"BusinessPartners('{SapCardCode}')"));

						if (model.Length > 0)
						{
							company.DELETE($"BusinessPartners('{SapCardCode}')");
							deleteAddonTransactions(DocEntry, ORNumber);
						}
					}
				}
				#endregion












				// ### CHECK IF PROCEED TO DOWNPAYMENT ### //
				#region AR Downpayment


				if (Tagging == "QUOTATION")
				{


					//// Check if Quotation already has Sales Order
					//DataTable dt = new DataTable();
					//dt = hana.GetData($@"select ""DocEntry"" from qut1 where ""DocEntry"" = {SQEntry} and  ""TargetType"" = 17", hana.GetConnection("SAPHana"));

					//if (dt.Rows.Count == 0)
					//{ }



					if (output)
					{


						ReservationPayment = double.Parse(DataHelper.DataTableRet(generalData, 0, "ReservationAmount", ""));

						output = CreateDownPayment(company,
													SapCardCode,
													DocDate,
													ReservationPayment,
													"RES",
													"1",
													ProjectCode,
													Block,
													Lot,
													SQEntry,
													Payment,
													Payment,
													"",
													DocEntry,
													DocDate,
													out DPEntry,
													out Message);
					}
					else
					{
						StringBuilder model = new StringBuilder();
						model.Append(company.GET($"Quotations({SQEntry})"));

						if (model.Length > 0)
						{
							company.DELETE($"Quotations({SQEntry})");
							deleteAddonTransactions(DocEntry, ORNumber);
						}
					}
				}
				#endregion



















			}
			catch (Exception ex)
			{
				//DELETE PAYMENT IF ERROR IN POSTING
				deleteAddonTransactions(DocEntry, ORNumber);
				Message = ex.Message;
			}
			return output;
		}

		public bool CreateBusinessPartner(SapHanaLayer company,
											string AddonCardCode,
											string CardName,
											string ProjCode,
											string LastName,
											string FirstName,
											string MiddleName,
											string TIN,
											string TaxClassification,
											string Role,
											out string SapCardCode,
											out string ErrMsg)
		{
			bool output = false;

			try
			{
				// Check if BP is exist in SAP
				DataTable dt = new DataTable();
				dt = hana.GetData($@"SELECT TOP 1 ""DocEntry"",""CardCode"",""CardName"",""CardFName""  FROM ""OCRD"" WHERE ""U_DreamsCustCode"" = '{AddonCardCode}'", hana.GetConnection("SAPHana"));

				if (DataAccess.Exist(dt))
				{
					int bpSeries = int.Parse(ConfigSettings.BPSeries);
					string clearingAccount = ConfigSettings.ClearingAccount;
					int bpListNum = int.Parse(ConfigSettings.BPPriceList);
					string PayTermsGrpCode = ConfigSettings.PayTermsGrpCode;
					IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
					{
					{ "CardName" , CardName },
					{ "CardForeignName" , CardName },
					{ "U_LName" , LastName},
					{ "U_FName" , FirstName},
					{ "U_MName" , MiddleName},
					};

					var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

					if (company.PATCH($@"BusinessPartners('{DataAccess.GetData(dt, 0, "CardCode", "").ToString()}')", json))
					{
						//SapCardCode = company.ResultDescription;
						SapCardCode = DataAccess.GetData(dt, 0, "CardCode", "").ToString();
						ErrMsg = "Business Partner successfully updated.";
						output = true;
						hana.Execute($@"UPDATE ""OCRD"" SET ""SAPCardCode"" = '{SapCardCode}' WHERE ""CardCode"" = '{AddonCardCode}'", hana.GetConnection("SAOHana"));
					}
					else
					{
						SapCardCode = "";
						ErrMsg = $"({company.ResultCode}) {company.ResultDescription}";
					}
				}
				else
				{
					int bpSeries = int.Parse(ConfigSettings.BPSeries);
					string clearingAccount = ConfigSettings.ClearingAccount;
					int bpListNum = int.Parse(ConfigSettings.BPPriceList);
					string PayTermsGrpCode = ConfigSettings.PayTermsGrpCode;
					IDictionary<string, object> BusinessPartners = new Dictionary<string, object>()
					{
					{ "Series" , bpSeries },
					{ "CardType" , "C" },
					{ "CardName" , CardName },
					{ "CardForeignName" , CardName },
					{ "PriceListNum" , bpListNum },
					{ "DownPaymentClearAct" , clearingAccount },
					{ "SubjectToWithholdingTax" , "N" },
					{ "DeferredTax" , "N" },
					{ "ProjectCode" , ProjCode },
					{ "PayTermsGrpCode" , PayTermsGrpCode },
					{ "U_LName" , LastName},
					{ "U_FName" , FirstName},
					{ "U_MName" , MiddleName},
					{ "FederalTaxID" , TIN },
					{ "GlobalLocationNumber" , "DREAMS" },
					{ "U_DreamsCustCode" , AddonCardCode },
					{ "U_TaxClass" , TaxClassification },
					{ "U_Role" , Role}
					};

					var json = DataHelper.JsonBuilder(BusinessPartners, new StringBuilder());

					if (company.POST("BusinessPartners", json))
					{
						SapCardCode = company.ResultDescription;
						ErrMsg = "Business Partner successfully created.";
						output = true;
						hana.Execute($@"UPDATE ""OCRD"" SET ""SAPCardCode"" = '{SapCardCode}' WHERE ""CardCode"" = '{AddonCardCode}'", hana.GetConnection("SAOHana"));
					}
					else
					{
						SapCardCode = "";
						ErrMsg = $"({company.ResultCode}) {company.ResultDescription} - [BP Posting]";
					}
				}
			}
			catch (Exception ex)
			{
				SapCardCode = "";
				ErrMsg = ex.Message;
			}

			return output;
		}

		public bool CreateSalesQuotation(SapHanaLayer company,
											int AddonDocEntry, //2
											string CardCode,
											string RsvDate, //4
											string ProjCode,
											string Block, //6
											string Lot,
											string HouseModel, //8
											string FinancingScheme,
											string ProductType, //10
											double ReservationAmount,
											double DPAmount, //12
											double LBAmount,
											double NetTCP, //14
											string Vatable,
											double DiscountAmount, //16
											double MiscAmount,
											string BatchNum, //18
											string DocNum,
											string EmpName, //20
											string SoldWithAdjacentLot,
											string AdjacentLotQuotationNo, //22
											int DPTerms,
											double MonthlyMisc,
											string TaxDate,
											int MiscTerms,
											out int DocEntry,
											out string ErrMsg
											) //24
		{
			bool output = false;
			try
			{
				// ### CHECKING IF THE QUOTATION IS EXISTING ### //
				DataTable dt = new DataTable();
				dt = ws.IsSalesQuotationExists(ProjCode,
												Block,
												Lot,
												CardCode).Tables[0];
				if (DataHelper.DataTableExist(dt))
				{
					ErrMsg = $"Quotations successfully created.";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
				}
				else
				{


					string qry = $@"select ""U_PmtSchemeType"" from ""@FINANCINGSCHEME"" where ""Code"" = '{FinancingScheme}'";
					DataTable dtGetPmtScheme = hana.GetData(qry, hana.GetConnection("SAPHana"));
					string GetPmtScheme = DataAccess.GetData(dtGetPmtScheme, 0, "U_PmtSchemeType", "").ToString();

					qry = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjCode}'";
					DataTable dtGetLocation = hana.GetData(qry, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();

					DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
					string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();

					IDictionary<string, object> Quotations = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"Series", ConfigSettings.QuotationSeries},
						{"DocDate", RsvDate},
						{"TaxDate", TaxDate},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: 'RES' Pymnt Order: '0' | Project: {ProjCode} BL{Block} LT{Lot}"},
						{"Comments", $" {ProjCode}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"Project", ProjCode},
						{"U_Project", ProjCode},
						{"NumAtCard", DocNum},
						{"U_BlockNo", Block},
						{"U_LotNo", Lot},
						{"U_HouseModel", HouseModel},
						{"U_FinancingScheme", FinancingScheme},
						{"U_ProductType", ProductType},
						{"U_ReservationAmount", ReservationAmount},
						{"U_DPAmount", DPAmount},
						{"U_LBAmount", LBAmount},
						{"U_PrepBy", "DREAMS"},
						{"U_SalesType", "Real Estate"},
						{"U_DreamsQuotationNo", DocNum},
						{"U_ContractStatus", "Open"},
						{"U_SalesAgent", EmpName},
						{"U_SoldWithAdjacentLot", SoldWithAdjacentLot},
						{"U_AdjacentLotQuotationNo", AdjacentLotQuotationNo},
						{"U_PaymentType", GetPmtScheme},
						{"U_Location", Location},
						{"U_Branch", Location},
						{"U_SortCode", SortCode}

					};

					string vatGrp = (Vatable == "N" ? "OTE" : "OT-SP");


					// ### FREIGHT HERE ### //
					#region FREIGHT

					IList<IDictionary<string, object>> DocumentAdditionalExpenses = new List<IDictionary<string, object>>();

					if (DiscountAmount > 0)
					{
						var ExpensesLines = new Dictionary<string, object>();
						ExpensesLines.Add("ExpenseCode", ConfigSettings.ExpenseCode);
						//ExpensesLines.Add("LineTotal", -DiscountAmount);
						ExpensesLines.Add("LineGross", -DiscountAmount);
						ExpensesLines.Add("VatGroup", vatGrp);
						//ExpensesLines.Add("VatGroup", "OTNA");


						//2023-09-07 : ADD BRANCH AND PROJECT ExpensesLines.Add("DistributionRule2",
						ExpensesLines.Add("Project", ProjCode);
						ExpensesLines.Add("DistributionRule", "SLSMKTG");
						ExpensesLines.Add("DistributionRule2", SortCode);


						DocumentAdditionalExpenses.Add(ExpensesLines);
					}
					#endregion

					DataTable dtPrice = ws.GetHouseDetails(ProjCode, Block, Lot, FinancingScheme).Tables["GetHouseDetails"];
					double LandPrice = Convert.ToDouble(DataAccess.GetData(dtPrice, 0, "LandPrice", "0").ToString());
					double HousePrice = Convert.ToDouble(DataAccess.GetData(dtPrice, 0, "HousePrice", "0").ToString());




					// ### DOCUMENT LINES HERE ### //
					#region DOCUMENTLINES

					DataTable itemDetails = new DataTable();
					itemDetails = ws.GetItemDetails(ProjCode,
													Block,
													Lot,
													ConfigurationManager.AppSettings["HANADatabase"]).Tables[0];

					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
					var Lines = new Dictionary<string, object>();

					// ### GET HOUSE HERE
					#region HOUSE
					string houseItem = DataHelper.DataTableRet(itemDetails, 0, "House", "");
					if (!string.IsNullOrEmpty(houseItem))
					{
						Lines.Add("ItemCode", houseItem);
						Lines.Add("Quantity", 1);
						Lines.Add("ProjectCode", ProjCode);
						//1 -- Lines.Add("UnitPrice", double.Parse(DataHelper.DataTableRet(itemDetails, 0, "HousePrice", "")));
						//2 - -Lines.Add("LineTotal", HousePrice);
						Lines.Add("PriceAfterVAT", HousePrice);
						Lines.Add("VatGroup", vatGrp);
						//Lines.Add("VatGroup", "OTNA");
						DocumentLines.Add(Lines);
					}
					#endregion

					// ### GET LOT HERE
					#region LOT
					string lotItem = DataHelper.DataTableRet(itemDetails, 0, "Lot", "");
					double priceAfterVat = LandPrice / double.Parse(DataHelper.DataTableRet(itemDetails, 0, "LotArea", ""));
					if (!string.IsNullOrEmpty(lotItem))
					{
						Lines = new Dictionary<string, object>();
						Lines.Add("ItemCode", lotItem);
						Lines.Add("Quantity", double.Parse(DataHelper.DataTableRet(itemDetails, 0, "LotArea", "")));
						//Lines.Add("Quantity", 1);
						Lines.Add("ProjectCode", ProjCode);
						//1 - Lines.Add("UnitPrice", double.Parse(DataHelper.DataTableRet(itemDetails, 0, "LotPricePerSqm", "")));
						//2 - Lines.Add("LineTotal", LandPrice);
						Lines.Add("PriceAfterVAT", priceAfterVat);
						Lines.Add("VatGroup", vatGrp);
						//Lines.Add("VatGroup", "OTNA");
						DocumentLines.Add(Lines);
					}
					#endregion




					// ### GET MISCELLANEOUS PAYMENTS HERE 
					#region ADDITIONAL CHARGES
					string AdditionalChargesItem = DataHelper.DataTableRet(itemDetails, 0, "AdditionalCharges", "");

					if (!string.IsNullOrEmpty(AdditionalChargesItem))
					{
						Lines = new Dictionary<string, object>();
						Lines.Add("ItemCode", AdditionalChargesItem);
						//Lines.Add("Quantity", DPTerms);
						Lines.Add("Quantity", MiscTerms * 2);
						Lines.Add("ProjectCode", ProjCode);
						//Lines.Add("UnitPrice", MonthlyMisc);
						Lines.Add("LineTotal", MiscAmount);
						//Lines.Add("VatGroup", vatGrp);
						Lines.Add("VatGroup", "OTNA");
						DocumentLines.Add(Lines);
					}
					#endregion





					#endregion

					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();

					StringBuilder DocumentAdditionalExpensesLines = DataHelper.JsonLinesBuilder("DocumentAdditionalExpenses", DocumentAdditionalExpenses);
					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					StringBuilder strLines = new StringBuilder();

					if (DocumentAdditionalExpenses.Count > 0)
					{
						strLines = DataHelper.JsonLinesCombine(DocumentAdditionalExpensesLines, DocumentLine);
					}
					else
					{
						strLines = DocumentLine;
					}
					var json = DataHelper.JsonBuilder(Quotations, strLines);

					if (DocumentAdditionalExpenses.Count > 0 || DocumentLines.Count > 0)
					{
						if (company.POST("Quotations", json))
						{
							ErrMsg = $"Quotations successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							ErrMsg = $"({company.ResultCode}) {company.ResultDescription} - [Quotation Posting]";
							DocEntry = 0;
						}
					}
					else
					{
						ErrMsg = $"({company.ResultCode}) {company.ResultDescription} - [Quotation Posting]";
						DocEntry = 0;
					}
				}
			}
			catch (Exception ex)
			{
				DocEntry = 0;
				ErrMsg = ex.Message;
			}

			return output;
		}

		public bool CreateSalesOrder(SapHanaLayer company,
										string CardCode, //2
										string RsvDate,
										string ProjectCode, //4
										string Block,
										string Lot, //6
										double SurChargesAmount,
										double InterestAmount, //8
										int SQDocEntry,
										string paymentdue_type, //10
										string DocNum,
										string LOI,
										string TaxDate,
										//2023-09-08 : ADDED DOCENTRY
										int OQUTDocEntry,
										out int DocEntry,//12
										out string Message)
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}

				DataTable dt = new DataTable();

				dt = ws.IsSalesOrderExists(ProjectCode,
											Block,
											Lot,
											CardCode,
											DocNum).Tables[0];


				//IF SALES ORDER EXISTS 
				if (DataHelper.DataTableExist(dt))
				{
					//IF SURCHARGE OR INTEREST AMOUNTIS IS MORE THAN 0 
					//if (SurChargesAmount > 0 || InterestAmount > 0)
					//{
					//    //SALES ORDER HEADER
					//    IDictionary<string, object> Orders = new Dictionary<string, object>()
					//    {
					//        {"Series", ConfigSettings.SalesOrderSeries"].ToString()},
					//        {"CardCode", CardCode},
					//        {"U_SalesType", "Real Estate"},
					//        {"U_LOI", LOI},
					//        {"Comments", $"UPDATED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"}
					//    };

					//    //SALES ORDER LINES
					//    IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

					//    DataTable dtItems = new DataTable();

					//    //GET SALES ORDER LINES WHERE STATUS IS STILL OPEN AND ITEM WHICH ARE NOT TAGGED AS SURCHARGED OR INTEREST IN ITEM PROPERTIES
					//    dtItems = ws.GetSalesOrderDetails(int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0")), true).Tables[0];

					//    int lineNum = dtItems.Rows.Count;

					//    //ADD SALES ORDER ROWS FOR ACTUAL ITEMS
					//    if (DataAccess.Exist(dtItems))
					//    {
					//        foreach (DataRow dr in dtItems.Rows)
					//        {
					//            //CHECK IF ITEM IS MISC
					//            string qry = $@"select IFNULL(""QryGroup1"",'N') ""QryGroup1"" from oitm where ""ItemCode"" = '{dr["ItemCode"]}'";
					//            DataTable dtGetMisc = hana.GetData(qry, hana.GetConnection("SAPHana"));
					//            string IsMisc = DataAccess.GetData(dtGetMisc, 0, "QryGroup1", "").ToString();

					//            if (IsMisc != "Y")
					//            {
					//                var Lines = new Dictionary<string, object>();
					//                Lines.Add("ItemCode", dr["ItemCode"].ToString());
					//                Lines.Add("Quantity", dr["Quantity"].ToString());
					//                Lines.Add("ProjectCode", ProjectCode);
					//                Lines.Add("UnitPrice", dr["Price"].ToString());
					//                //Lines.Add("GrossPrice", dr["Price"].ToString());
					//                Lines.Add("VatGroup", dr["VatGroup"].ToString());
					//                DocumentLines.Add(Lines);
					//            }
					//        }
					//    }

					//    DataTable itemDetails = new DataTable();
					//    itemDetails = ws.GetItemDetails(ProjectCode,
					//                                        Block,
					//                                        Lot).Tables[0];
					//    string surChargesItem = DataHelper.DataTableRet(itemDetails, 0, "SurCharges", "");
					//    string interestItem = DataHelper.DataTableRet(itemDetails, 0, "Interest", "");

					//    ////ADD SALES ORDER ROWS FOR SURCHARGES 
					//    //if (SurChargesAmount > 0)
					//    //{
					//    //    if (!string.IsNullOrEmpty(surChargesItem))
					//    //    {
					//    //        var Lines = new Dictionary<string, object>();
					//    //        Lines.Add("ItemCode", surChargesItem);
					//    //        Lines.Add("Quantity", 1);
					//    //        Lines.Add("ProjectCode", ProjectCode);
					//    //        Lines.Add("UnitPrice", SurChargesAmount);
					//    //        //Lines.Add("GrossPrice", SurChargesAmount);
					//    //        Lines.Add("VatGroup", "OT1");
					//    //        DocumentLines.Add(Lines);
					//    //    }
					//    //}

					//    //ADD SALES ORDER ROWS FOR INTEREST
					//    if (InterestAmount > 0)
					//    {

					//        if (!string.IsNullOrEmpty(interestItem))
					//        {
					//            var Lines = new Dictionary<string, object>();
					//            Lines.Add("ItemCode", interestItem);
					//            Lines.Add("Quantity", 1);
					//            Lines.Add("ProjectCode", ProjectCode);
					//            Lines.Add("UnitPrice", InterestAmount);
					//            //Lines.Add("GrossPrice", InterestAmount);
					//            //Lines.Add("VatGroup", "OT1");
					//            Lines.Add("VatGroup", "OTNA");
					//            DocumentLines.Add(Lines);
					//        }
					//    }

					//    //############################## [POSTING HERE] ##############################
					//    bool result = company.Connect();

					//    StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					//    var json = DataHelper.JsonBuilder(Orders, DocumentLine);


					//    if (DocumentLines.Count > 0)
					//    {
					//        DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					//        if (company.PATCH($"Orders({DocEntry})", json))
					//        {
					//            Message = $"Orders successfully created.";
					//            output = true;
					//        }
					//        else
					//        {
					//            Message = $"({company.ResultCode}) {company.ResultDescription}";
					//            DocEntry = 0;
					//        }
					//    }
					//    else
					//    {
					//        Message = $"({company.ResultCode}) {company.ResultDescription}";
					//        DocEntry = 0;
					//    }
					//}
					//else
					//{
					Message = $"Sales Order successfully created. Existing";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
					//}

				}


				//IF SALES ORDER DOES NOT EXIST
				else
				{





					DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
					string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();

					IDictionary<string, object> Orders = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"Series", ConfigSettings.SalesOrderSeries},
						{"DocDate", RsvDate},
						{"DocDueDate", RsvDate},
						{"TaxDate", RsvDate},
						{"U_LOI", LOI},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: 'RES' Pymnt Order: '0' | Project: {ProjectCode} BL{Block} LT{Lot}"},
						{"Comments", $" {ProjectCode}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"}
					};

					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
					DataTable dtItems = new DataTable();

					//GET QUOTATION DETAILS (COPY FROM) 
					dtItems = ws.GetSalesQuotationDetails(SQDocEntry).Tables[0];
					int lineNum = dtItems.Rows.Count;

					//SALES ORDER ROWS FROM SALES QUOTATION
					if (DataAccess.Exist(dtItems))
					{
						foreach (DataRow dr in dtItems.Rows)
						{
							var Lines = new Dictionary<string, object>();
							Lines.Add("BaseEntry", SQDocEntry);
							Lines.Add("BaseLine", int.Parse(dr["LineNum"].ToString()));
							Lines.Add("BaseType", 23);
							Lines.Add("U_UnitPrice", Convert.ToDouble(dr["LineTotal"].ToString()));

							if (paymentdue_type == "MISC")
							{
								Lines.Add("VatGroup", "OTNA");
							}

							DocumentLines.Add(Lines);
						}
					}

					DataTable itemDetails = new DataTable();
					itemDetails = ws.GetItemDetails(ProjectCode,
														Block,
														Lot,
														ConfigurationManager.AppSettings["HANADatabase"]).Tables[0];
					string surChargesItem = DataHelper.DataTableRet(itemDetails, 0, "SurCharges", "");
					string interestItem = DataHelper.DataTableRet(itemDetails, 0, "Interest", "");

					#region comments
					//FOR SURCHARGES
					//if (SurChargesAmount > 0)
					//{
					//    if (!string.IsNullOrEmpty(surChargesItem))
					//    {
					//        var Lines = new Dictionary<string, object>();
					//        Lines.Add("ItemCode", surChargesItem);
					//        Lines.Add("Quantity", 1);
					//        Lines.Add("ProjectCode", ProjectCode);
					//        //Lines.Add("UnitPrice", SurChargesAmount);
					//        Lines.Add("PriceAfterVAT", SurChargesAmount);
					//        Lines.Add("VatGroup", "OT1");
					//        DocumentLines.Add(Lines);
					//    }
					//}

					//// FOR INTERESTS
					//if (InterestAmount > 0)
					//{

					//    if (!string.IsNullOrEmpty(interestItem))
					//    {
					//        var Lines = new Dictionary<string, object>();
					//        Lines.Add("ItemCode", interestItem);
					//        Lines.Add("Quantity", 1);
					//        Lines.Add("ProjectCode", ProjectCode);
					//        //Lines.Add("UnitPrice", InterestAmount);
					//        Lines.Add("PriceAfterVAT", InterestAmount);
					//        Lines.Add("VatGroup", "OT1");
					//        DocumentLines.Add(Lines);
					//    }
					//}
					#endregion


					//2023-09-08 : ADD FREIGHT POSTING IN SALES ORDER
					string qry = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qry, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();

					DataTable generalData = ws.GetGeneralData(OQUTDocEntry, ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
					double DiscountAmount = double.Parse(DataHelper.DataTableRet(generalData, 0, "DiscountAmount", "0")); //16
					string Vatable = DataHelper.DataTableRet(generalData, 0, "Vatable", "No").ToString();
					string vatGrp = (Vatable == "N" ? "OTE" : "OT-SP");


					// ### FREIGHT HERE ### //
					#region FREIGHT

					IList<IDictionary<string, object>> DocumentAdditionalExpenses = new List<IDictionary<string, object>>();

					if (DiscountAmount > 0)
					{
						var ExpensesLines = new Dictionary<string, object>();
						ExpensesLines.Add("ExpenseCode", ConfigSettings.ExpenseCode);
						//ExpensesLines.Add("LineTotal", -DiscountAmount);
						ExpensesLines.Add("LineGross", -DiscountAmount);
						ExpensesLines.Add("VatGroup", vatGrp);
						//ExpensesLines.Add("VatGroup", "OTNA");

						//2023-09-07 : ADD BRANCH AND PROJECT 
						ExpensesLines.Add("Project", ProjectCode);
						ExpensesLines.Add("DistributionRule", "SLSMKTG");
						ExpensesLines.Add("DistributionRule2", SortCode);


						DocumentAdditionalExpenses.Add(ExpensesLines);
					}

					#endregion


					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();

					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					StringBuilder DocumentAdditionalExpensesLines = DataHelper.JsonLinesBuilder("DocumentAdditionalExpenses", DocumentAdditionalExpenses);
					StringBuilder strLines = new StringBuilder();

					if (DocumentAdditionalExpenses.Count > 0)
					{
						strLines = DataHelper.JsonLinesCombine(DocumentLine, DocumentAdditionalExpensesLines);
					}
					else
					{
						strLines = DocumentLine;
					}

					var json = DataHelper.JsonBuilder(Orders, strLines);




					if (DocumentLines.Count > 0)
					{
						if (company.POST("Orders", json))
						{
							Message = $"Orders successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription} - [SO Posting]";
							DocEntry = 0;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription} - [SO Posting]";
						DocEntry = 0;
					}
				}


			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
			}

			return output;
		}




		public bool CreateDownPayment(SapHanaLayer company,
										string CardCode, //2
										string RsvDate,
										double ReservationAmount, //4
										string Type,
										string PaymentOrder, //6
										string ProjectCode,
										string Block, //8
										string Lot,
										int SQDocEntry, //10
										double TCP,
										double Payment, //12
										string TransactionTagging,
										int DreamsDocEntry,
										string TaxDate,
										out int DocEntry,
										out string Message) //14
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}
				DataTable dt = new DataTable();

				//IF DP IS FOR AR RESERVE INVOICE POSTING
				if (PaymentOrder == "0")
				{
					TCP = ReservationAmount;
				}

				dt = ws.IsARDownpaymentExists(Type,
												PaymentOrder,
												ProjectCode,
												Block,
												Lot,
												CardCode,
												RsvDate,
												TCP).Tables[0];

				if (DataHelper.DataTableExist(dt))
				{
					Message = $"DownPayments successfully created. Existing";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
				}
				else
				{
					decimal LineTotal = 1;
					decimal AmountPaid = 0;

					//Check if ARDPI posting is from Restructuring or not (if Blank, then it's not from restructuring)
					if (TransactionTagging == "")
					{
						// Check if Reservation is already fully paid (For Restructured Contracts)
						DataTable dtCheckAmount = new DataTable();
						string qryCheckAmount = $@"select 
                                            IFNULL((IFNULL(B.""PaymentAmount"",0) + IFNULL(B.""Penalty"",0) + IFNULL(B.""InterestAmount"",0) + IFNULL(B.""IPS"",0)),0) ""LineTotal"", 
                                            B.""AmountPaid"",
                                            A.""DocEntry""
                                            from OQUT A INNER JOIN QUT1 B ON A.""DocEntry"" = B.""DocEntry"" where
                                            A.""DocStatus"" = 'O' AND A.""DocEntry"" = '{DreamsDocEntry}' AND A.""ProjCode"" = '{ProjectCode}' and 
                                            A.""Block"" = '{Block}' and	A.""Lot"" = '{Lot}' and B.""PaymentType"" = '{Type}' and B.""Terms"" = '{PaymentOrder}' and 
                                            B.""LineStatus"" <> 'R' ";
						dtCheckAmount = hana.GetData(qryCheckAmount, hana.GetConnection("SAOHana"));
						LineTotal = Convert.ToDecimal(DataAccess.GetData(dtCheckAmount, 0, "LineTotal", "0").ToString());
						AmountPaid = Convert.ToDecimal(DataAccess.GetData(dtCheckAmount, 0, "AmountPaid", "0").ToString());
						int QuotDocEntry = Convert.ToInt32(DataAccess.GetData(dtCheckAmount, 0, "DocEntry", "0").ToString());
					}


					string qryLocation = $@"SELECT B.""Name"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();



					if (LineTotal > AmountPaid)
					{

						string ARAccount = ConfigSettings.ARClearingAccount;

						DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
						string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();

						IDictionary<string, object> DownPayments = new Dictionary<string, object>()
					{
						{"DownPaymentType", "dptInvoice"},
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"TaxDate", TaxDate},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: '{Type}' Pymnt Order: '{PaymentOrder}' | Project: {ProjectCode} BL{Block} LT{Lot}"},
						{"Comments", $" {ProjectCode}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"DocTotal", ReservationAmount},
						{"ControlAccount", ARAccount},
						{"U_Type", Type},
						{"U_PaymentOrder", PaymentOrder},
						{"U_Branch", Location},
						{"U_RestructureTag", "NEW"},
					};

						IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

						DataTable dtItems = new DataTable();
						if (Type != "RES")
						{
							dtItems = ws.GetSalesOrderDetails(SQDocEntry, false).Tables[0];
						}
						else
						{
							dtItems = ws.GetSalesQuotationDetails(SQDocEntry).Tables[0];
						}

						if (DataAccess.Exist(dtItems))
						{
							foreach (DataRow dr in dtItems.Rows)
							{
								var Lines = new Dictionary<string, object>();

								//CHECK IF ITEM IS MISC
								string qry = $@"select IFNULL(""QryGroup1"",'N') ""QryGroup1"" from oitm where ""ItemCode"" = '{dr["ItemCode"]}'";
								DataTable dtGetMisc = hana.GetData(qry, hana.GetConnection("SAPHana"));
								string IsMisc = DataAccess.GetData(dtGetMisc, 0, "QryGroup1", "").ToString();

								if (IsMisc != "Y")
								{

									//string itemCode = dr["ItemCode"].ToString();
									//** set base entry **//
									Lines.Add("BaseEntry", SQDocEntry);
									Lines.Add("BaseLine", int.Parse(dr["LineNum"].ToString()));
									Lines.Add("BaseType", (Type == "RES" ? 23 : 17));
									Lines.Add("VatGroup", "OTNA");

									//2023-06-21 : FOR 0.01 RESERVATION POSTING, ADD LINETOTAL ROWS FOR POSTING
									if (Type == "RES")
									{
										Lines.Add("LineTotal", ReservationAmount);
									}


									DocumentLines.Add(Lines);
								}
							}
						}

						//############################## [POSTING HERE] ##############################
						bool result = company.Connect();

						StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
						var json = DataHelper.JsonBuilder(DownPayments, DocumentLine);


						if (DocumentLines.Count > 0)
						{
							if (company.POST("DownPayments", json))
							{
								Message = $"DownPayments successfully created.";
								DocEntry = int.Parse(company.ResultDescription);
								output = true;
							}
							else
							{
								Message = $"({company.ResultCode}) {company.ResultDescription} - [ARDP Posting]";
								DocEntry = 0;
							}
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription} - [ARDP Posting]";
							DocEntry = 0;
						}
					}


					else
					{
						Message = $"DownPayments successfully created. Existing";
						DocEntry = 0;
						output = true;
					}



				}


			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
			}

			return output;
		}

		public bool CreateARInvoice(SapHanaLayer company,
										string CardCode, //2
										string RsvDate,
										double Amount, //4
										string Type,
										string fieldName, //6
										string PaymentOrder,
										string ProjectCode, //8
										string Block,
										string Lot, //10
										int SODocEntry,
										string Restructured,//12
										string TaxDate,
										int MiscTerms,
										out int DocEntry,
										out string Message) //14
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}
				DataTable dt = new DataTable();

				dt = ws.IsARInvoiceExists(Type,
									   PaymentOrder,
									   ProjectCode,
									   Block,
									   Lot,
									   CardCode,
									   RsvDate,
									   "").Tables[0];

				if (DataHelper.DataTableExist(dt) && Restructured == "N")
				{
					Message = $"Invoices successfully created. Existing";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
				}
				else
				{

					string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


					DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
					string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();


					IDictionary<string, object> Invoices = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"TaxDate", TaxDate},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: '{Type}' Pymnt Order: '{PaymentOrder}' | Project: {ProjectCode} BL{Block} LT{Lot}"},
						{"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"DocTotal", Amount},
						{"U_Type", Type},
						{"U_PaymentOrder", PaymentOrder},
						{"ControlAccount", ConfigSettings.ARClearingAccount},
						{"Series", ConfigSettings.ARInvoiceSeries},
						{"U_Branch", Location },
						{"U_SortCode", SortCode },
						{"U_RestructureTag", Restructured == "Y" ? "NEW" : ""}
                        //{"TotalDiscount", 0}
                    };



					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

					DataTable dtItems = new DataTable();
					//dtItems = ws.GetARInvoiceDetails(SODocEntry, fieldName).Tables[0];
					dtItems = ws.GetSalesOrderDetails(SODocEntry, false).Tables[0];

					if (DataAccess.Exist(dtItems))
					{
						foreach (DataRow dr in dtItems.Rows)
						{
							if (Type == "MISC" && dr["QryGroup1"].ToString() == "Y")
							{
								var Lines = new Dictionary<string, object>();

								//string itemCode = dr["ItemCode"].ToString();
								//** set base entry **//
								Lines.Add("BaseEntry", SODocEntry);
								Lines.Add("BaseLine", int.Parse(dr["LineNum"].ToString()));
								Lines.Add("BaseType", 17);

								//if (Restructured == "Y")
								//{
								//    Lines.Add("Quantity", MiscTerms);
								//}
								//else
								//{
								Lines.Add("Quantity", 1);
								//}

								//Lines.Add("LineTotal", Amount);
								DocumentLines.Add(Lines);
							}
						}
					}

					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();

					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					var json = DataHelper.JsonBuilder(Invoices, DocumentLine);


					if (DocumentLines.Count > 0)
					{
						if (company.POST("Invoices", json))
						{
							Message = $"Invoices successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription} - [AR Inv Posting]";
							DocEntry = 0;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription} -- (No Miscellaneous item available found in Sales Order | Please check Sales Order Document)";
						DocEntry = 0;
					}
				}


			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
			}

			return output;
		}



		public bool CreateARInvoiceStandalone(SapHanaLayer company,
								   string CardCode, //2
								   string RsvDate,
								   double Amount, //4
								   string Type,
								   string PaymentOrder, //6
								   string ProjectCode,
								   string Block, //8
								   string Lot,
								   string HouseModel, //10
								   string FinancingScheme,
								   string ProductType, //12
								   double ReservationAmount,
								   double DPAmount, //14
								   double LBAmount,
								   string transType, //16
								   string QuotationNo,
								   string ControlAccount, //18
								   string Vatable,
								   double AmountCondition,//20
								   string partialPaymentTag,
								   string waive,
								   string TaxDate,
								   out int DocEntry, //22
								   out string Message)
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}

				string Transac = "";
				if (transType == "Surcharge")
				{
					Transac = "RE - SUR";
				}
				else if (transType == "Interest")
				{
					Transac = "INT INC";
				}
				else if (transType == "IPS")
				{
					Transac = "IP&S";
				}
				else if (transType == "Sundry")
				{
					//Transac = "REFUND"; 
					Transac = "EXCESS";
				}

				DataTable dt = new DataTable();
				dt = ws.IsARInvoiceStandaloneExists(Type,
												PaymentOrder,
												ProjectCode,
												Block,
												Lot,
												CardCode,
												AmountCondition,
												Transac,
												RsvDate,
												partialPaymentTag).Tables[0];
				if (DataHelper.DataTableExist(dt))
				{
					Message = $"Invoices successfully created. Existing.";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
				}
				else
				{


					DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
					string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();


					string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


					IDictionary<string, object> Invoices;
					Invoices = new Dictionary<string, object>()
						{
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: '{Type}' Pymnt Order: '{PaymentOrder}' | Project: {ProjectCode} BL{Block} LT{Lot}"},
						{"Comments", $" {ProjectCode}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"Project", ProjectCode},
						{"DocTotal", Amount},
						{"NumAtCard", QuotationNo},
						{"U_Type", Type},
						{"U_PaymentOrder", PaymentOrder},
						{"U_BlockNo", Block},
						{"U_LotNo", Lot},
						{"U_Project", ProjectCode},
						{"U_HouseModel", HouseModel},
						{"U_FinancingScheme", FinancingScheme},
						{"U_ProductType", ProductType},
						{"U_ReservationAmount", ReservationAmount},
						{"U_DPAmount", DPAmount},
						{"U_LBAmount", LBAmount},
						{"U_PrepBy", "DREAMS"},
						{"U_SalesType", "Real Estate"},
						{"U_DreamsQuotationNo", QuotationNo},
						{"U_TransactionType", Transac},
						{"Series", ConfigSettings.ARInvoiceSeries},
						{"ControlAccount", ControlAccount},
						{"U_Waive", waive},
						{"U_Branch", Location},
						{"U_SortCode", SortCode},
						{"TaxDate", TaxDate}
						};

					if (partialPaymentTag == "Y")
					{
						Invoices.Add("U_Partial", "Y");
					}



					string SurchargePropertyFieldName = ConfigSettings.SurchargeProperty;

					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

					DataTable itemDetails = new DataTable();

					if (transType == "Surcharge")
					{
						itemDetails = ws.GetARInvoiceDetails(SurchargePropertyFieldName).Tables[0];
					}
					else
					{
						itemDetails = ws.GetItemDetails(ProjectCode,
														Block,
														Lot,
														ConfigurationManager.AppSettings["HANADatabase"]).Tables[0];
					}
					string item = "";

					var Lines = new Dictionary<string, object>();


					Lines.Add("Quantity", 1);
					Lines.Add("ProjectCode", ProjectCode);


					if (transType == "Misc")
					{
						Lines.Add("UnitPrice", Amount);
						item = DataHelper.DataTableRet(itemDetails, 0, "AdditionalCharges", "");
						Lines.Add("VatGroup", ConfigSettings.MiscVATGroup);
					}
					else if (transType == "Surcharge")
					{
						item = DataHelper.DataTableRet(itemDetails, 0, "ItemCode", "");
						Lines.Add("PriceAfterVAT", Amount);

						//2023-09-18 : ADD CONDITION FOR VAT 
						if (Vatable == "Y")
						{
							Lines.Add("VatGroup", ConfigSettings.SurchargeVATGroup);

							//2023-12-06 : ADD SURCHARGE ACCOUNT CODE DEPENDS ON VAT GROUP
							Lines.Add("AccountCode", ConfigSettings.SuchargeVATAccount);

						}
						else
						{
							Lines.Add("VatGroup", ConfigSettings.SurchargeExVATGroup);

							//2023-12-06 : ADD SURCHARGE ACCOUNT CODE DEPENDS ON VAT GROUP
							Lines.Add("AccountCode", ConfigSettings.SuchargeNonVATAccount);

						}

					}
					else if (transType == "IPS")
					{
						if (Vatable == "Y")
						{
							Lines.Add("VatGroup", ConfigSettings.IPSVatableVATGroup);
							Lines.Add("AccountCode", ConfigSettings.IPSVATAccount);
						}
						else
						{
							Lines.Add("VatGroup", ConfigSettings.IPSNONVatableVATGroup);
							Lines.Add("AccountCode", ConfigSettings.IPSNonVATAccount);
						}

						item = DataHelper.DataTableRet(itemDetails, 0, "IPS", "");
						Lines.Add("PriceAfterVAT", Amount);
					}
					else if (transType == "Sundry")
					{
						Lines.Add("VatGroup", ConfigSettings.ExcessVATGroup);
						Lines.Add("AccountCode", ConfigSettings.ExcessVATAccount);

						Lines.Add("PriceAfterVAT", Amount);
						item = DataHelper.DataTableRet(itemDetails, 0, "EXCESS", "");
					}
					else
					{
						if (Vatable == "Y")
						{
							Lines.Add("VatGroup", ConfigSettings.InterestVatableVATGroup);
							Lines.Add("AccountCode", ConfigSettings.InterestVATAccount);
						}
						else
						{
							Lines.Add("VatGroup", ConfigSettings.InterestNONVatableVATGroup);
							Lines.Add("AccountCode", ConfigSettings.InterestNonVATAccount);
						}

						Lines.Add("PriceAfterVAT", Amount);
						item = DataHelper.DataTableRet(itemDetails, 0, "Interest", "");
					}
					Lines.Add("ItemCode", item);
					DocumentLines.Add(Lines);

					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();

					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					var json = DataHelper.JsonBuilder(Invoices, DocumentLine);


					if (DocumentLines.Count > 0)
					{
						if (company.POST("Invoices", json))
						{
							Message = $"Invoices successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription}";
							DocEntry = 0;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription}";
						DocEntry = 0;
					}
				}


			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
			}

			return output;
		}



		public bool CreateARCreditMemo(SapHanaLayer company,
							 string CardCode, //2
							 string RsvDate,
							 double Amount, //4
							 string ProjectCode,
							 string Block, //8
							 string Lot,
							 string HouseModel, //10
							 string FinancingScheme,
							 string ProductType, //12
							 double ReservationAmount,
							 double DPAmount, //14
							 double LBAmount,
							 int DPIEntry,
							 out int DocEntry, //16
							 out string Message)
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}
				DataTable dt = new DataTable();
				dt = ws.IsARCMExists(ProjectCode,
									 Block,
									 Lot,
									 CardCode,
									 DPIEntry
									).Tables[0];
				if (DataHelper.DataTableExist(dt))
				{
					Message = $"AR Credit Memo successfully created.";
					DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					output = true;
				}
				else
				{

					string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();

					IDictionary<string, object> Invoices = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"Comments", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"Project", ProjectCode},
                        //{"DocTotal", Amount},
                        {"U_BlockNo", Block},
						{"U_LotNo", Lot},
						{"U_HouseModel", HouseModel},
						{"U_FinancingScheme", FinancingScheme},
						{"U_ProductType", ProductType},
						{"U_ReservationAmount", ReservationAmount},
						{"U_DPAmount", DPAmount},
						{"U_LBAmount", LBAmount},
						{"U_PrepBy", "DREAMS"},
						{"U_SalesType", "Real Estate"},
						{"Series", ConfigSettings.ARCreditMemoSeries},
						{"U_Branch", Location},
						{"U_SortCode", SortCode}

					};

					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();

					DataTable itemDetails = new DataTable();
					itemDetails = ws.GetDownpaymentDetails(DPIEntry).Tables[0];


					//ROWS FROM SALES ORDER ROWS 
					if (DataAccess.Exist(itemDetails))
					{
						foreach (DataRow dr in itemDetails.Rows)
						{
							var Lines = new Dictionary<string, object>();
							Lines.Add("BaseEntry", DPIEntry);
							Lines.Add("BaseLine", int.Parse(dr["LineNum"].ToString()));
							Lines.Add("BaseType", 203);
							Lines.Add("U_UnitPrice", Convert.ToDouble(dr["LineTotal"].ToString()));
							DocumentLines.Add(Lines);
						}
					}
					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();

					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					var json = DataHelper.JsonBuilder(Invoices, DocumentLine);


					if (DocumentLines.Count > 0)
					{
						if (company.POST("CreditNotes", json))
						{
							Message = $"AR Credit Memo successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription}";
							DocEntry = 0;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription}";
						DocEntry = 0;
					}
				}


			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
			}

			return output;
		}




		public bool CreateReserveInvoice(SapHanaLayer company,
									 string CardCode, //2
									 string RsvDate,
									 string ProjectCode, //4
									 string Block,
									 string Lot, //6
									 double SurChargesAmount,
									 double InterestAmount, //8
									 int SODocEntry,
									 double NetTCP, //10
									 string FinancingScheme,
									 string TaxClassification, //12
									 string PaymentScheme,
									 double TotalPayment, //14
									 double Payment,
									 double Balance, //16
									 System.Drawing.Color color,
									 string DocNum, //18
									 string Vatable,
									 string Term, //20
									 string PaymentType,
									 string TaxDate, //22
									 double DiscountAmount,
									 double TotalPaymentPlusCurrentPayment, //24

									 //2023-07-31: ADD RECEIPT NUMBER
									 string ReceiptNo,

									 //2023-08-07 : ADD TAGGING OF RESTRUCTURING TYPE
									 int AdvancePaymentPosting,

									 out int DocEntry,
									 out string Message) //26
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}

				DataTable dt = new DataTable();
				int JournalEntryNo;
				DocEntry = 0;
				Message = "";
				dt = ws.IsReserveInvoiceExists(ProjectCode,
											Block,
											Lot,
											CardCode).Tables[0];


				string OutputVATClearingAccount = ConfigSettings.OutputVATClearingAccount;
				string OutputVATAccount = ConfigSettings.OutputVATAccount;

				string SalesREVATAccount = ConfigSettings.SalesREVATAccount;
				string UnearnedIncomeAccount = ConfigSettings.UnearnedIncomeAccount;

				string CostOfSalesCollected = ConfigSettings.CostOfSalesCollected;
				string CostOfSalesRE = ConfigSettings.CostOfSalesRE;
				string CostOfSalesUncollected = ConfigSettings.CostOfSalesUncollected;

				string SalesCollectedAccount = ConfigSettings.SalesCollectedAccount;
				string SalesUncollectedAccount = ConfigSettings.SalesUncollectedAccount;

				string CreditableWithholdingTaxAccount = ConfigSettings.CreditableWithholdingTaxAccount;
				string AccumulatedCostOfSalesAccount = ConfigSettings.AccumulatedCostOfSalesAccount;

				string ContractReceivablesDeferredAccount = ConfigSettings.ContractReceivablesDeferredAccount;
				string ContractReceivablesInstallmentAccount = ConfigSettings.ContractReceivablesInstallmentAccount;

				string SalesUncollectedVATAccount = ConfigSettings.SalesUncollectedVATAccount;
				string CostOfSalesClearingAccount = ConfigSettings.CostOfSalesClearingAccount;

				string AccountsPayableAccount = ConfigSettings.AccountsPayableAccount;

				string SalesCollectedNonVAT = ConfigSettings.SalesCollectedNonVAT;
				string SalesUncollectedNonVAT = ConfigSettings.SalesUncollectedNonVAT;



				DataTable dtStandardCost = hana.GetData($@"SELECT ""U_Standardcost"" FROM ""OPRJ"" WHERE ""PrjCode"" = '{ProjectCode}'", hana.GetConnection("SAPHana"));
				StandardCostFactorRate = Convert.ToDouble(DataAccess.GetData(dtStandardCost, 0, "U_Standardcost", "0").ToString());


				//IF AR RESERVATION INVOICE EXISTS
				if (DataHelper.DataTableExist(dt))
				{
					//Message = $"AR Reserve Invoice successfully created.";
					//DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
					//output = true;


					if (color != System.Drawing.ColorTranslator.FromHtml("#28B463"))
					{


						// GET WITHHOLDING TAX RATE FROM SAP
						double wthRate = 0;
						string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {NetTCP} AND ""U_SellingPriceTo"" >= {NetTCP}";
						DataTable dtWthRate = hana.GetData(qryWthRate, hana.GetConnection("SAPHana"));


						if (dtWthRate.Rows.Count > 0)
						{
							wthRate = (double.Parse(DataAccess.GetData(dtWthRate, 0, "Rate", "0").ToString())) / 100;
						}


						#region SALES PREVIOUS JE
						//SALES UNCOLLECTED
						if (PaymentScheme.ToUpper() == "INSTALLMENT")
						{

							//Payment Subsequent to booking - recognition of sales and VAT
							if (output =
								CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										  //2023-07-31 : ADDED RECEIPT NUMBER
										  Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATAccount, "25TCP11", DocNum, Term, PaymentType, Vatable, RsvDate, "UNSV", out JournalEntryNo, out Message, ReceiptNo)
										  )
							{
								//Payment Subsequent to booking - recognition of cost of sales
								if (output = CreateJournalEntry(null, ProjectCode, CardCode, (Payment * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
									   //2023-07-31 : ADDED RECEIPT NUMBER
									   Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, "", "25TCP13", DocNum, Term, PaymentType, Vatable, RsvDate, "UCOS", out JournalEntryNo, out Message, ReceiptNo))
								{


									//IF ENGAGED IN BUSINESS, ADD 25TCP14
									if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification1.ToUpper())
									{
										double Amount = Payment * wthRate;
										//CREDITABLE WITHHOLDING TAX
										if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
										   //2023-07-31 : ADDED RECEIPT NUMBER
										   Block, Lot, CreditableWithholdingTaxAccount, AccountsPayableAccount, "", "25TCP14", DocNum, Term, PaymentType, Vatable, RsvDate, "UCWT", out JournalEntryNo, out Message, ReceiptNo))
										{
											Message = $"AR Reserve Invoice successfully created. Existing";
											DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
											output = true;
										}
										else
										{
											DocEntry = 0;
											output = false;
										}
									}
									else
									{
										Message = $"AR Reserve Invoice successfully created. Existing";
										DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
										output = true;
									}
								}
								else
								{
									DocEntry = 0;
									output = false;
								}
							}
							else
							{
								DocEntry = 0;
								output = false;
							}





							////FULL PAYMENT
							//if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification2"].ToString().ToUpper())
							//{

							//    double Amount = (NetTCP / 1.12) * wthRate;
							//    //Full payment - recognition of withholding taxes
							//    if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
							//               Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP22", DocNum, "", "", out JournalEntryNo, out Message))
							//    {
							//        Message = $"AR Reserve Invoice successfully created. Existing";
							//        DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
							//        output = true;

							//    }
							//    else
							//    {
							//        DocEntry = 0;
							//        output = false;
							//    }
							//}
							//else
							//{
							//    Message = $"AR Reserve Invoice successfully created. Existing";
							//    DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
							//    output = true;
							//}





							if (TaxClassification != "Not engaged in business")
							{

								////Payment Subsequent to booking - recognition of sales and VAT
								//if (output =

								//    (Vatable == "Y" ? CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
								//              Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATAccount, "25TCP11", DocNum, "", out JournalEntryNo, out Message) : true)

								//              )
								//{
								//OUTPUT VAT CLEARING
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
								//           Block, Lot, OutputVATClearingAccount, OutputVATAccount, "", "25TCP12", out JournalEntryNo, out Message))
								//{


								////Payment Subsequent to booking - recognition of cost of sales
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (Payment * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
								//       Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, "", "25TCP13", DocNum, "", out JournalEntryNo, out Message))
								//{




								//double Amount = Payment * wthRate;
								////CREDITABLE WITHHOLDING TAX
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
								//   Block, Lot, CreditableWithholdingTaxAccount, AccountsPayableAccount, "", "25TCP14", DocNum, "", out JournalEntryNo, out Message))
								//{


								//    if (output =
								//        (Vatable == "N" ? CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
								//       Block, Lot, SalesUncollectedNonVAT, SalesCollectedNonVAT, "", "25TCP26", DocNum, "", out JournalEntryNo, out Message) : true)

								//       )
								//    {

								//        Message = $"AR Reserve Invoice successfully created. Existing";
								//        DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
								//        output = true;
								//    }
								//    else
								//    {
								//        DocEntry = 0;
								//        output = false;
								//    }

								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}


								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}



								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}
								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}
							}
							else
							{
								////SALES UNCOLLLECTED
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (NetTCP - TotalPayment), SODocEntry, FinancingScheme, TaxClassification,
								//           Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATClearingAccount, "25TCP19", out JournalEntryNo, out Message))
								//{

								////Payment Subsequent to booking - recognition of sales and VAT
								//if (output =

								//    (Vatable == "Y" ? CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
								//              Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATAccount, "25TCP19", "", DocNum, out JournalEntryNo, out Message) : true)

								//    )
								//{

								////OUTPUT VAT CLEARING
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (NetTCP - TotalPayment), SODocEntry, FinancingScheme, TaxClassification,
								//           Block, Lot, OutputVATClearingAccount, OutputVATAccount, "", "25TCP20", out JournalEntryNo, out Message))
								//{



								////COST OF SALES COLLECTED (STANDARD COST)
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, NetTCP * (StandardCostFactorRate / 100), SODocEntry, FinancingScheme, TaxClassification,
								//           Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, "", "25TCP21", out JournalEntryNo, out Message))
								//{

								////Payment Subsequent to booking - recognition of cost of sales
								//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (Payment * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
								//       Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, "", "25TCP21", DocNum, "", out JournalEntryNo, out Message))
								//{


								//    if (output =
								//      (Vatable == "N" ? CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
								//     Block, Lot, SalesUncollectedNonVAT, SalesCollectedNonVAT, "", "25TCP26", DocNum, "", out JournalEntryNo, out Message) : true)

								//     )
								//    {





								////CHECK IF CONTRACT IS ALREADY FULLY PAID
								//if ((Payment + TotalPayment) >= NetTCP)
								//{




								//    double Amount = NetTCP * wthRate;
								//    //Full payment - recognition of withholding taxes
								//    if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
								//               Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP22", DocNum, "", out JournalEntryNo, out Message))
								//    {




								//        Message = $"AR Reserve Invoice successfully created.";
								//        DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
								//        output = true;








								//    }
								//    else
								//    {
								//        DocEntry = 0;
								//        output = false;
								//    }
								//}
								//else
								//{
								//    Message = $"AR Reserve Invoice successfully created.";
								//    DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
								//    output = true;
								//}

								//    }

								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}
								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}
								//}
								//else
								//{
								//    DocEntry = 0;
								//    output = false;
								//}
							}

						}
						else
						{
							Message = $"AR Reserve Invoice successfully created. Existing";
							DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
							output = true;
						}
						#endregion










					}
					else
					{
						Message = $"AR Reserve Invoice successfully created. Existing";
						DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
						output = true;
					}
				}




































				//IF AR RESERVATION INVOICE DOES NOT EXIST
				else
				{

					string ControlAccount = "";
					if (PaymentScheme == "Deferred")
					{
						ControlAccount = ContractReceivablesDeferredAccount;
					}
					else
					{
						ControlAccount = ContractReceivablesInstallmentAccount;
					}


					string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
					DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
					string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
					string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


					bool LTS = true;
					//JOURNAL ENTRY WHEN PROJECT HAS NO LTS NO.
					DataTable dtLTSNo = hana.GetData($@"SELECT ""LTSNo"" FROM OQUT WHERE ""DocNum"" = '{DocNum}'", hana.GetConnection("SAOHana"));
					if (dtLTSNo.Rows.Count > 0)
					{
						string LTSNo = DataAccess.GetData(dtLTSNo, 0, "LTSNo", "").ToString();

						if (!string.IsNullOrWhiteSpace(LTSNo))
						{
							LTS = true;
						}
						else
						{
							LTS = false;
						}
					}
					else
					{
						LTS = false;
					}


					DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
					string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();


					IDictionary<string, object> ARReserveInvoice = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"DocDueDate", RsvDate},
						{"TaxDate", TaxDate},
						{"Comments", $" {ProjectCode}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"ReserveInvoice", "tYES"},
						{"ControlAccount", ControlAccount},
						{"U_PaymentType", PaymentScheme},
						{"U_PaymentOrder", Term},
						{"U_Type", PaymentType},
						{"U_Branch", Location},
						{"U_SortCode", SortCode},
						{"JournalMemo", $"Customer Name: {GetCustomerName} | Type: '{PaymentScheme}' Pymnt Order: '{Term}' | Project: {ProjectCode} BL{Block} LT{Lot}"},
						{"U_RestructureTag", "NEW"},
					};

					IList<IDictionary<string, object>> DocumentLines = new List<IDictionary<string, object>>();
					IList<IDictionary<string, object>> DownpaymentDraws = new List<IDictionary<string, object>>();

					DataTable dtItems = new DataTable();

					//GET ORDER DETAILS (COPY FROM) 
					dtItems = ws.GetSalesOrderDetails(SODocEntry, true).Tables[0];
					int lineNum = dtItems.Rows.Count;

					//ROWS FROM SALES ORDER ROWS  
					if (DataAccess.Exist(dtItems))
					{
						foreach (DataRow dr in dtItems.Rows)
						{
							//CHECK IF ITEM IS MISC
							string qry = $@"select IFNULL(""QryGroup1"",'N') ""QryGroup1"" from oitm where ""ItemCode"" = '{dr["ItemCode"]}'";
							DataTable dtGetMisc = hana.GetData(qry, hana.GetConnection("SAPHana"));
							string IsMisc = DataAccess.GetData(dtGetMisc, 0, "QryGroup1", "").ToString();

							if (IsMisc != "Y")
							{
								var Lines = new Dictionary<string, object>();
								Lines.Add("BaseEntry", SODocEntry);
								Lines.Add("BaseLine", int.Parse(dr["LineNum"].ToString()));
								Lines.Add("BaseType", 17);
								Lines.Add("U_UnitPrice", Convert.ToDouble(dr["LineTotal"].ToString()));

								//2023-09-01 : ADDED DIMENSION 2 (DEPARTMENT) AND 
								Lines.Add("CostingCode", "SLSMKTG");
								Lines.Add("CostingCode2", SortCode);

								//2023-08-31 : ADD ACCOUNT CODE (SALES COLLECTED NON VAT) -- ONLY WHEN NON-VAT  
								if (Vatable == "N")
								{
									Lines.Add("AccountCode", SalesCollectedNonVAT);
								}

								DocumentLines.Add(Lines);
							}
						}
					}


					DataTable dtDownpayments = new DataTable();
					string qryDP = $@"select DISTINCT A.""DocEntry"",A.""DocTotal"",A.""PaidToDate"",A.""DocNum"" From ODPI A INNER JOIN DPI1 B ON 
                        A.""DocEntry"" = B.""DocEntry"" where A.""U_DreamsQuotationNo"" = '{DocNum}' AND IFNULL(B.""TargetType"",0) <> 14 --AND A.""DocTotal"" = ""PaidToDate""";
					dtDownpayments = hana.GetData(qryDP, hana.GetConnection("SAPHana"));
					//ROWS FROM DOWNPAYMENT ROWS 
					if (DataAccess.Exist(dtDownpayments))
					{
						foreach (DataRow dr in dtDownpayments.Rows)
						{
							var Lines = new Dictionary<string, object>();
							Lines.Add("DocEntry", dr["DocEntry"].ToString());

							double paidToDate = double.Parse(dr["PaidToDate"].ToString());
							double DocTotal = double.Parse(dr["DocTotal"].ToString());
							if (DocTotal == paidToDate)
							{
								Lines.Add("AmountToDraw", DocTotal);
							}
							else
							{
								Lines.Add("AmountToDraw", paidToDate);
							}
							DownpaymentDraws.Add(Lines);
						}
					}


					// ### FREIGHT HERE ### //
					#region FREIGHT
					IList<IDictionary<string, object>> DocumentAdditionalExpenses = new List<IDictionary<string, object>>();
					string vatGrp = (Vatable == "N" ? "OTE" : "OT-SP");

					if (DiscountAmount > 0)
					{
						var ExpensesLines = new Dictionary<string, object>();
						ExpensesLines.Add("ExpenseCode", ConfigSettings.ExpenseCode);
						//ExpensesLines.Add("LineTotal", -DiscountAmount);
						ExpensesLines.Add("LineGross", -DiscountAmount);
						ExpensesLines.Add("VatGroup", vatGrp);
						//ExpensesLines.Add("VatGroup", "OTNA");

						//2023-09-07 : ADD BRANCH AND PROJECT 
						ExpensesLines.Add("Project", ProjectCode);
						ExpensesLines.Add("DistributionRule", "SLSMKTG");
						ExpensesLines.Add("DistributionRule2", SortCode);


						DocumentAdditionalExpenses.Add(ExpensesLines);
					}
					#endregion



					//############################## [POSTING HERE] ##############################
					bool result = company.Connect();



					StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("DocumentLines", DocumentLines);
					StringBuilder DownpaymentDraw = DataHelper.JsonLinesBuilder("DownPaymentsToDraw", DownpaymentDraws);
					StringBuilder strLines = new StringBuilder();

					StringBuilder DocumentAdditionalExpensesLines = DataHelper.JsonLinesBuilder("DocumentAdditionalExpenses", DocumentAdditionalExpenses);
					if (DocumentAdditionalExpenses.Count > 0)
					{
						strLines = DataHelper.JsonLinesCombine(DocumentLine, DownpaymentDraw, DocumentAdditionalExpensesLines);
					}
					else
					{
						strLines = DataHelper.JsonLinesCombine(DocumentLine, DownpaymentDraw);
					}

					var json = DataHelper.JsonBuilder(ARReserveInvoice, strLines);


					if (DocumentLines.Count > 0)
					{
						if (company.POST("Invoices", json))
						{
							//2023-08-07 : EXCLUDE FROM POSTING WHEN AR RESERVE INVOICE POSTING IS FROM ADVANCE PAYMENT
							if (AdvancePaymentPosting == 0)
							{

								//###########################
								//JOURNAL ENTRY POSTING
								//########################### 

								// GET WITHHOLDING TAX RATE FROM SAP
								double wthRate = 0;
								string qryWthRate = $@"SELECT IFNULL(""Rate"",0) ""Rate"" FROM OWHT WHERE ""U_ATC"" = '{ConfigSettings.ATCCode}' AND ""U_SellingPriceFrom"" <= {NetTCP} AND ""U_SellingPriceTo"" >= {NetTCP}";
								DataTable dtWthRate = hana.GetData(qryWthRate, hana.GetConnection("SAPHana"));


								if (dtWthRate.Rows.Count > 0)
								{
									wthRate = (double.Parse(DataAccess.GetData(dtWthRate, 0, "Rate", "0").ToString())) / 100;
								}


								//OUTPUT VAT CLEARING
								if (PaymentScheme == "Deferred")
								{

									//2023-07-14 : MOVED SO IT WONT BE CONSIDERED FOR ANY TAX CLASSIFICATION
									//2023-07-06 : CHANGED TRANSCODE FROM ITCR TO ISDC : https://docs.google.com/spreadsheets/d/1rbPD_Ml1CoRWerx3nPklB3XrODP5GBUslbJwvHmbuBQ/edit#gid=34173818
									//2023-06-30 : SALES COLLECTED (DISCOUNT) -- DI DAPAT MAG POPOST KAPAG 0 YUNG DISCOUNT AMOUNT 
									double unCollected = Math.Abs((NetTCP) - (TotalPayment + TotalPaymentPlusCurrentPayment));
									if (output = CreateJournalEntry(null, ProjectCode, CardCode, DiscountAmount, SODocEntry, FinancingScheme, TaxClassification,
														 //2023-07-31 : ADDED RECEIPT NUMBER
														 //2023-08-02 : CHANGED TRANSCODE FROM ISDC TO DFSD
														 //Block, Lot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedVATAccount, "25TCP29", DocNum, Term, PaymentType, Vatable, RsvDate, "ISDC", out JournalEntryNo, out Message, ReceiptNo, DiscountAmount))
														 Block, Lot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedVATAccount, "25TCP29", DocNum, Term, PaymentType, Vatable, RsvDate, "DFSD", out JournalEntryNo, out Message, ReceiptNo, DiscountAmount))
									{

										Message = $"AR Reserve Invoice successfully created.";
										DocEntry = int.Parse(company.ResultDescription);
										output = true;

										//COST OF SALES COLLECTED (STANDARD COST DAW DAPAT TO)
										if (output = CreateJournalEntry(null, ProjectCode, CardCode, NetTCP * (StandardCostFactorRate / 100), SODocEntry, FinancingScheme, TaxClassification,
																  //2023-07-31 : ADDED RECEIPT NUMBER
																  Block, Lot, CostOfSalesCollected, CostOfSalesClearingAccount, "", "25TCP3", DocNum, Term, PaymentType, Vatable, RsvDate, "DFCO", out JournalEntryNo, out Message, ReceiptNo))
										{
											//CHECK IF ENGAGED IN BUSINESS
											if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification1.ToUpper())
											{

												double unCollected1 = Math.Abs((NetTCP - TotalPayment));
												//2023-05-23 : CHANGED TOTALPAYMENT TO TOTALPAYMENT + CURRENT PAYMENT
												////double unCollected1 = Math.Abs((NetTCP - TotalPaymentPlusCurrentPayment));
												double amt25TCP28 = 0;
												if (Vatable.ToUpper() == "Y")
												{
													amt25TCP28 = (unCollected1 / 1.12) * wthRate;
												}
												else
												{
													amt25TCP28 = (unCollected1) * wthRate;
												}

												//CREDITABLE WITHHOLDING TAX UPON 25%
												if (LTS)
												{

													//2023-09-20 : GET 
													string qryDocEntry = $@"SELECT ""DocEntry"" FROM OQUT WHERE ""DocNum"" = '{DocNum}'";
													DataTable dtDocEntry = hana.GetData(qryDocEntry, hana.GetConnection("SAOHana"));
													int DREAMSDocEntry = int.Parse(DataAccess.GetData(dtDocEntry, 0, "DocEntry", "0").ToString());

													DataTable generalData = ws.GetGeneralData(DREAMSDocEntry, ConfigurationManager.AppSettings["HANADatabase"].ToString()).Tables[0];
													int DPTerms = int.Parse(DataHelper.DataTableRet(generalData, 0, "DPTerms", ""));
													int LTerms = int.Parse(DataHelper.DataTableRet(generalData, 0, "LTerms", ""));


													//2024-04-11 : COMMENTED TO CONSIDER PARTIAL PAYMENT FOR SPOTCASH
													//if (DPTerms == 1 && LTerms == 0) 
													//{
													//    Message = $"AR Reserve Invoice successfully created.";
													//    DocEntry = int.Parse(company.ResultDescription); 
													//    output = true;
													//}
													//else
													//{
													//if (output = CreateJournalEntry(null, ProjectCode, CardCode, amt25TCP28, SODocEntry, FinancingScheme, TaxClassification,
													//				//2023-07-31 : ADDED RECEIPT NUMBER
													//				Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP28", DocNum, Term, PaymentType, Vatable, RsvDate, "DFCW", out JournalEntryNo, out Message, ReceiptNo))
													//{

													Message = $"AR Reserve Invoice successfully created.";
													DocEntry = int.Parse(company.ResultDescription);
													output = true;

													//}
													//else
													//{
													//	DocEntry = 0;
													//	output = false;
													//}


													//}
												}
												else
												{
													Message = $"AR Reserve Invoice successfully created.";
													DocEntry = int.Parse(company.ResultDescription);
													output = true;
												}
											}
											else
											{
												//2023-07-06 : NOT ENGAGED IN BUSINESS JE POSTING

												if (LTS)
												{
													//2023-07-06 : JE posting - DINW https://docs.google.com/spreadsheets/d/1rbPD_Ml1CoRWerx3nPklB3XrODP5GBUslbJwvHmbuBQ/edit#gid=34173818
													double Amount = 0;
													if (Vatable.ToUpper() == "Y")
													{
														Amount = (NetTCP / 1.12) * wthRate;
													}
													else
													{
														Amount = (NetTCP * wthRate);
													}

													if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
																			  //2023-07-31 : ADDED RECEIPT NUMBER
																			  //2023-08-18 : CHANGE TRANSCODE FROM DINW TO DNEW
																			  Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP30", DocNum, Term, PaymentType, Vatable, RsvDate, "DNEW", out JournalEntryNo, out Message, ReceiptNo))
													{
														Message = $"AR Reserve Invoice successfully created.";
														DocEntry = int.Parse(company.ResultDescription);
														output = true;
													}
													else
													{
														DocEntry = 0;
														output = false;
													}
												}
												else
												{
													Message = $"AR Reserve Invoice successfully created.";
													DocEntry = int.Parse(company.ResultDescription);
													output = true;
												}
											}
										}
										else
										{
											DocEntry = 0;
											output = false;
										}
									}
									else
									{
										DocEntry = 0;
										output = false;
									}


								}
								else if (PaymentScheme.ToUpper() == "INSTALLMENT")
								{

									Message = $"AR Reserve Invoice successfully created.";
									DocEntry = int.Parse(company.ResultDescription);
									output = true;



									//TCP Payment - reversal
									//2023-06-28 : GET TOTAL PAYMENT + CURRENT PAYMENT
									//double unCollected = Math.Abs((NetTCP) - TotalPayment);
									double unCollected = Math.Abs((NetTCP) - (TotalPayment + TotalPaymentPlusCurrentPayment));
									if (output =

										CreateJournalEntry(null, ProjectCode, CardCode, unCollected, SODocEntry, FinancingScheme, TaxClassification,
																 Block, Lot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedVATAccount, "25TCP4", DocNum, Term, PaymentType, Vatable, RsvDate, "ITCR", out JournalEntryNo, out Message, ReceiptNo, DiscountAmount)
										)
									{

										_TotalPayment = TotalPayment;
										//25% TCP Payment - cost of sales
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (NetTCP * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
										if (output = CreateJournalEntry(null, ProjectCode, CardCode, NetTCP, SODocEntry, FinancingScheme, TaxClassification,
												  //2023-07-31 : ADDED RECEIPT NUMBER
												  Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, CostOfSalesClearingAccount, "25TCP9", DocNum, Term, PaymentType, Vatable, RsvDate, "I25C", out JournalEntryNo, out Message, ReceiptNo))
										{

											Message = $"AR Reserve Invoice successfully created.";
											DocEntry = int.Parse(company.ResultDescription);
											output = true;
										}
										else
										{
											DocEntry = 0;
											output = false;
										}

									}
									else
									{
										DocEntry = 0;
										output = false;
									}




									//WALA NA TO
									if (TaxClassification != "Not engaged in business")
									{

										////SALES UNCOLLECTED
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//                       Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATClearingAccount, "25TCP15",
										//                       out JournalEntryNo, out Message))
										//{

										//TCP Payment - reversal 
										//if (output =
										//    (Vatable == "Y" ? CreateJournalEntry(null, ProjectCode, CardCode, unCollected, SODocEntry, FinancingScheme, TaxClassification,
										//                             Block, Lot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedVATAccount, "25TCP4", DocNum, "", out JournalEntryNo, out Message) : true)


										//    )
										//{
										////OUTPUT VAT CLEARING
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//                        Block, Lot, OutputVATClearingAccount, OutputVATAccount, "", "25TCP5", out JournalEntryNo, out Message))
										//{
										//SALES UNCOLLECTED
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//                    Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATClearingAccount, "25TCP6", out JournalEntryNo, out Message))
										//{
										////OUTPUT VAT CLEARING
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//              Block, Lot, OutputVATClearingAccount, OutputVATAccount, "", "25TCP7", out JournalEntryNo, out Message))
										//{
										////SALES RE VAT UNEARNED INCOME CLEARING
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, NetTCP, SODocEntry, FinancingScheme, TaxClassification,
										//              Block, Lot, SalesREVATAccount, UnearnedIncomeAccount, "", "25TCP8", out JournalEntryNo, out Message))
										//{


										////25% TCP Payment - cost of sales
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (NetTCP * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
										//          Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, CostOfSalesClearingAccount, "25TCP9", DocNum, "", out JournalEntryNo, out Message))
										//{



										//    ////25% TCP Payment - recognition of withholding tax
										//    //double Amount = TotalPayment * wthRate;
										//    //if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
										//    //              Block, Lot, CreditableWithholdingTaxAccount, AccountsPayableAccount, "", "25TCP10", DocNum, out JournalEntryNo, out Message))
										//    //{



										//    double unCollected1 = Math.Abs(NetTCP - TotalPayment);
										//    if (output =


										//       (Vatable == "N" ? CreateJournalEntry(null, ProjectCode, CardCode, unCollected1, SODocEntry, FinancingScheme, TaxClassification,
										//            Block, Lot, SalesCollectedNonVAT, SalesUncollectedNonVAT, "", "25TCP25", DocNum, "", out JournalEntryNo, out Message) : true)

										//        )

										//    {
										//        Message = $"AR Reserve Invoice successfully created.";
										//        DocEntry = int.Parse(company.ResultDescription);
										//        output = true;
										//    }




										//    else
										//    {
										//        DocEntry = 0;
										//        output = false;
										//    }
										//    //}
										//    //else
										//    //{
										//    //    DocEntry = 0;
										//    //    output = false;
										//    //}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}


										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
									}
									else
									{

										//25% TCP Payment - reversal
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//                       Block, Lot, SalesUncollectedVATAccount, SalesCollectedAccount, OutputVATClearingAccount, "25TCP15",
										//                       out JournalEntryNo, out Message))



										////25% TCP Payment - reversal
										//unCollected = Math.Abs(NetTCP - TotalPayment);
										//if (output =

										//    (Vatable == "Y" ? CreateJournalEntry(null, ProjectCode, CardCode, unCollected, SODocEntry, FinancingScheme, TaxClassification,
										//                             Block, Lot, SalesCollectedAccount, OutputVATAccount, SalesUncollectedVATAccount, "25TCP15", DocNum, "", out JournalEntryNo, out Message) : true)


										//    )

										//{
										////OUTPUT VAT - CLEARING
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, Payment, SODocEntry, FinancingScheme, TaxClassification,
										//                           Block, Lot, OutputVATClearingAccount, OutputVATAccount, "", "25TCP16",
										//                           out JournalEntryNo, out Message))
										//{
										////SALES RE VAT
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, TotalPayment, SODocEntry, FinancingScheme, TaxClassification,
										//                           Block, Lot, SalesREVATAccount, UnearnedIncomeAccount, "", "25TCP17",
										//                           out JournalEntryNo, out Message))
										//{




										////COST OF SALES (STANDARD COST)
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, NetTCP * (StandardCostFactorRate / 100), SODocEntry, FinancingScheme, TaxClassification,
										//                           Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, CostOfSalesRE, "25TCP18",
										//                           out JournalEntryNo, out Message))
										//{

										////25% TCP Payment - cost of sales
										//if (output = CreateJournalEntry(null, ProjectCode, CardCode, (NetTCP * StandardCostFactorRate) / 100, SODocEntry, FinancingScheme, TaxClassification,
										//          Block, Lot, CostOfSalesCollected, CostOfSalesUncollected, CostOfSalesClearingAccount, "25TCP18", DocNum, "", out JournalEntryNo, out Message))
										//{



										//    double unCollected1 = Math.Abs(NetTCP - TotalPayment);
										//    if (output =


										//       (Vatable == "N" ? CreateJournalEntry(null, ProjectCode, CardCode, unCollected1, SODocEntry, FinancingScheme, TaxClassification,
										//            Block, Lot, SalesCollectedNonVAT, SalesUncollectedNonVAT, "", "25TCP25", DocNum, "", out JournalEntryNo, out Message) : true)

										//        )
										//    {
										//        Message = $"AR Reserve Invoice successfully created.";
										//        DocEntry = int.Parse(company.ResultDescription);
										//        output = true;

										//    }


										//}





										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}
										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
										//}


										//else
										//{
										//    DocEntry = 0;
										//    output = false;
										//}
									}
								}


								#region commented posting of JE


								////FULL PAYMENT
								//if (TaxClassification.ToUpper() == ConfigSettings.TaxClassification2"].ToString().ToUpper())
								//{

								//    double Amount = (NetTCP / 1.12) * wthRate;
								//    //Full payment - recognition of withholding taxes
								//    if (output = CreateJournalEntry(null, ProjectCode, CardCode, Amount, SODocEntry, FinancingScheme, TaxClassification,
								//               Block, Lot, CreditableWithholdingTaxAccount, "", "", "25TCP22", DocNum, "", "", out JournalEntryNo, out Message))
								//    {
								//        Message = $"AR Reserve Invoice successfully created.";
								//        DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
								//        output = true;

								//    }
								//    else
								//    {
								//        DocEntry = 0;
								//        output = false;
								//    }
								//}
								//else
								//{
								//    Message = $"AR Reserve Invoice successfully created.";
								//    DocEntry = int.Parse(DataHelper.DataTableRet(dt, 0, "DocEntry", "0"));
								//    output = true;
								//}

								#endregion

							}
							else
							{
								Message = $"AR Reserve Invoice successfully created.";
								DocEntry = int.Parse(company.ResultDescription);
								output = true;
							}
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription} - [AR Reserve Inv Posting]";
							DocEntry = 0;
							output = false;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription} - [AR Reserve Inv Posting]";
						DocEntry = 0;
						output = false;
					}
				}



























			}
			catch (Exception ex)
			{
				Message = ex.Message;
				DocEntry = 0;
				output = false;
			}

			return output;
		}





		public bool UpdateBatchDetails(SapHanaLayer company,
								   //DataTable generalData,
								   string Project, string Block, string Lot, string type,
								   out string Message)
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}

				string qry = $@"select ""AbsEntry"" from obtn where ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND ""U_Project"" = '{Project}'";
				DataTable dtGetSysNum = hana.GetData(qry, hana.GetConnection("SAPHana"));
				int sysNUmber = Convert.ToInt32(DataAccess.GetData(dtGetSysNum, 0, "AbsEntry", "0"));

				string lotStatus = "";
				if (type == "SOLD")
				{
					lotStatus = "S03";
				}
				else
				{
					lotStatus = "S01";
				}
				IDictionary<string, object> Invoices = new Dictionary<string, object>()
					{
						{"U_LotStatus", lotStatus}
					};

				//############################## [POSTING HERE] ##############################
				bool result = company.Connect();

				var json = DataHelper.JsonBuilder(Invoices, new StringBuilder());


				if (company.PATCH($"BatchNumberDetails({sysNUmber})", json))
				{
					Message = $"Invoices successfully created.";
					output = true;
				}
				else
				{
					Message = $"({company.ResultCode}) {company.ResultDescription}";
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
			}
			return output;
		}





		public bool CreateDeposit(SapHanaLayer company,
										string CommissionAccount, //2
										double CommissionAmount,
										string CommissionDate, //4
										string CommissionProject,
										string DepositType,
										string VoucherAccount,
										string DepositAccount,
										string TaxCode,
										double TaxAmount,

										string DeposNum,
										string RestructCode,

										string Project,
										string Block,
										string Lot,

										out int DocEntry,
										out string Message) //14
		{
			bool output = false;
			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}
				DataTable dt = new DataTable();

				string qryLocation = $@"SELECT B.""Name"",B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{Project}'";
				DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
				string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
				string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


				IDictionary<string, object> Deposits = new Dictionary<string, object>()
					{
						{"CommissionAccount", CommissionAccount},
						{"Commission", CommissionAmount},
						{"CommissionDate", CommissionDate},
						{"Project", CommissionProject},
						{"DepositType", DepositType},
						{"TaxCode", TaxCode},
						{"TaxAmount", TaxAmount},
						{"VoucherAccount", VoucherAccount},
						{"DepositAccount", DepositAccount},
						{"U_Branch", DepositAccount},
						{"U_SortCode", SortCode}
					};

				IList<IDictionary<string, object>> DepositLines = new List<IDictionary<string, object>>();

				DataTable dtRows = new DataTable();
				string qry = $@"SELECT 
                                    C.""AbsId"" 
                                FROM 
                                    ORCT A INNER JOIN 
                                    rct3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN 
                                    ocrh C ON B.""CrCardNum"" = C.""CrdCardNum"" AND B.""DocNum"" = C.""RctAbs"" INNER JOIN 
                                    ODPS E ON E.""DeposId"" = C.""DepNum"" 
                                WHERE 
                                    E.""DeposNum"" = {DeposNum} AND 
                                    A.""Canceled"" <> 'Y'

                                UNION ALL

                                SELECT 
                                    C.""AbsId"" 
                                FROM 
                                    ORCT A INNER JOIN	
                                    RCT3 B ON A.""DocEntry"" = B.""DocNum"" INNER JOIN
                                    OCRH C ON B.""CrCardNum"" = C.""CrdCardNum"" AND B.""DocNum"" = C.""RctAbs"" 
                                WHERE 
                                    A.""U_Restructured"" = 'Y' AND
	                                A.""DocDate"" = '{Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd")}' AND
	                                A.""U_Project"" = '{Project}' AND
	                                A.""U_BlockNo"" = '{Block}' AND
	                                A.""U_LotNo"" = '{Lot}' AND
	                                A.""U_WDeposit"" = 'Y'
                                ";


				dtRows = hana.GetData(qry, hana.GetConnection("SAPHana"));

				if (DataAccess.Exist(dtRows))
				{
					foreach (DataRow dr in dtRows.Rows)
					{
						var Lines = new Dictionary<string, object>();
						Lines.Add("AbsId", int.Parse(dr["AbsId"].ToString()));
						DepositLines.Add(Lines);
					}
				}

				//############################## [POSTING HERE] ##############################
				bool result = company.Connect();

				StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("CreditLines", DepositLines);
				var json = DataHelper.JsonBuilder(Deposits, DocumentLine);


				if (DepositLines.Count > 0)
				{
					if (company.POST("Deposits", json))
					{
						Message = $"Deposits successfully created.";
						DocEntry = int.Parse(company.ResultDescription);
						output = true;
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription}";
						DocEntry = 0;
					}
				}
				else
				{
					Message = $"({company.ResultCode}) {company.ResultDescription} -- (No Miscellaneous item available found in Sales Order)";
					DocEntry = 0;
				}

			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("IsSuccessful"))
				{
					Message = $"Deposits successfully created.";
					DocEntry = 0;
					output = true;
				}
				else
				{
					Message = ex.Message;
					DocEntry = 0;
				}

			}

			return output;
		}






		public bool CreateIncomingPayment(string CardCode,
										string RsvDate, //2
										string ORNumber,
										string Comments, //4
										int AddonDocEntry,
										string DPDocEntry, //6
										string InvoicesEntry,
										GridView dtPayments, //8
										string ORDate,
										string PRNumber, //10
										string PRDate,
										string ARNumber, //12
										string ARDate,
										double restructuringPayments, //14
										double CashDiscount,
										double IPAmount, //16
										string DocNum,
										int PaymentOrder, //18
										string Type,
										int DPTaggingPayment, //20
										double ARReserveInvoiceAmount,
										string UserCode, //22
										string Block,
										string Lot, //24
										string Project,
										string sundryPayment, //26
										string TagRestructuring,
										int RestructureDocEntryPayment, //28
										string PaymentType,
										string TaxDate, //30
										string WithDeposit,
										string CardBrandAccount, //32
										DataTable dtInterestAmount,
										int ARReserveEntry,
										int MiscEntry,

										//2023-09-21 : ADD SURCHARGE DATE ON POSTING
										string SurchargeDate,

										//2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
										string ContractStatus, //34

										out int DocEntry,
										out string Message, //36
										out string AccountNo,
										out double CashAmount, //38
										out double CheckAmount,
										out double CreditAmount, //40
										out double TransferAmount,
										out double OthersAmount) //42
		{
			bool output = false;
			string glAccount = "";
			CashAmount = 0;
			CheckAmount = 0;
			CreditAmount = 0;
			TransferAmount = 0;
			OthersAmount = 0;

			try
			{
				SapHanaLayer company = new SapHanaLayer();
				//** select existing partial payments **//
				DataTable dt = new DataTable();

				dt = ws.IsIncomingPaymentExists(DocNum, PaymentOrder, Type, ARReserveInvoiceAmount).Tables[0];


				ORDate = string.IsNullOrWhiteSpace(ORDate) ? "" : Convert.ToDateTime(ORDate).ToString("yyyy-MM-dd");
				PRDate = string.IsNullOrWhiteSpace(PRDate) ? "" : Convert.ToDateTime(PRDate).ToString("yyyy-MM-dd");
				ARDate = string.IsNullOrWhiteSpace(ARDate) ? "" : Convert.ToDateTime(ARDate).ToString("yyyy-MM-dd");

				string cashAccount = ConfigSettings.CashAccount;
				string checkAccount = ConfigSettings.CheckAccount;
				string Series = ConfigSettings.IncomingPaymentSeries;

				string qryOUSRInitials = $@"select  ""U_Initials""  from OUSR WHERE UPPER(""USER_CODE"") = UPPER('{UserCode}')";
				DataTable dtOUSRInitials = hana.GetData(qryOUSRInitials, hana.GetConnection("SAPHana"));

				DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAPHana"));
				string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "").ToString();

				string qryLocation = $@"SELECT B.""Name"", B.""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{Project}'";
				DataTable dtGetLocation = hana.GetData(qryLocation, hana.GetConnection("SAPHana"));
				string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "").ToString();
				string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "").ToString();


				IDictionary<string, object> IncomingPayments = new Dictionary<string, object>()
					{
						{"CardCode", CardCode},
						{"DocDate", RsvDate},
						{"TaxDate", TaxDate},
						{"JournalRemarks", $"Customer Name: {GetCustomerName} | Type: '{Type}' Pymnt Order: '{PaymentOrder}' | Project: {Project} BL{Block} LT{Lot}"},
						{"Remarks", $" {Project}-BL{Block}-LT{Lot} | POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"U_ORNo", ORNumber},
						{"U_Remarks", Comments},
                        //{"CashAccount", cashAccount},
                        {"U_ORDate", ORDate},
						{"U_PRNo", PRNumber},
						{"U_PRDate", PRDate},
						{"U_ARNo", ARNumber},
						{"U_ARDate", ARDate},
						{"U_BlockNo", Block},
						{"U_LotNo", Lot},
						{"U_Project", Project},
						{"U_PaymentOrder", PaymentOrder},
						{"U_PaymentType", PaymentType},
						{"ProjectCode", Project},
						{"Series", Series},
						{"U_Initials", DataAccess.GetData(dtOUSRInitials, 0, "U_Initials", "").ToString()},
						{"U_Branch",  Location},
						{"U_SortCode",  SortCode},


                        //2023-09-21 : ADD SURCHARGE DATE ON POSTING
                        {"U_SurchargeDate",  SurchargeDate},

                        //2023-06-13 : ADD CONTRACTSTATUS -- CONSIDERING ADVANCE PAYMENT / RESTRUCTURING
                        {"U_ContractStatus",  ContractStatus}

					};



				double sumPayment = 0;


				//GET TOTAL PAYMENT FOR PAYMENT IN ROWS
				if (dtPayments == null)
				{
					sumPayment = restructuringPayments;
				}
				else
				{
					foreach (GridViewRow row in dtPayments.Rows)
					{
						sumPayment += double.Parse(row.Cells[2].Text);
					}
					//sumPayment = IPAmount;

				}



				IList<IDictionary<string, object>> PaymentInvoices = new List<IDictionary<string, object>>();
				double _TotalPayment = sumPayment;











				if (TagRestructuring == "Y")
				{

					//CHECK IF THE PAYMENT HAS NO AR RESERVE IN IT
					if (ARReserveEntry == 0)
					{
						double ARInvDocTotal = 0;

						if (MiscEntry == 0)
						{
							//AR  INVOICE
							foreach (var docEntry in InvoicesEntry.Split(';'))
							{
								if (docEntry != "")
								{
									if (docEntry != "0")
									{
										var PaymentInvoicesLines = new Dictionary<string, object>();
										PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
										PaymentInvoicesLines.Add("DocEntry", docEntry);
										PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);

										string qryARInv = $@"SELECT ""DocTotal"" FROM OINV  
                                                    WHERE 
                                                    --""DocStatus"" = 'O' and  
                                                    ""isIns"" = 'N' AND ""DocEntry"" = {docEntry} AND 
                                                    ""CANCELED"" <> 'C' AND 
                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Restructured','Advance')    ";
										DataTable dtARInv = hana.GetData(qryARInv, hana.GetConnection("SAPHana"));
										ARInvDocTotal += Convert.ToDouble(DataAccess.GetData(dtARInv, 0, "DocTotal", "0").ToString());

										if (ARInvDocTotal > 0)
										{
											PaymentInvoicesLines.Add("SumApplied", ARInvDocTotal);
										}
										else
										{
											PaymentInvoicesLines.Add("SumApplied", restructuringPayments);
										}

										PaymentInvoices.Add(PaymentInvoicesLines);

									}
								}
							}



							//AR DOWNPAYMENT INVOICE
							foreach (var docEntry in DPDocEntry.Split(';'))
							{
								if (docEntry != "")
								{
									if (docEntry != "0")
									{
										//if ((restructuringPayments - ARInvDocTotal) > 0)
										//{
										var PaymentInvoicesLines = new Dictionary<string, object>();
										PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
										PaymentInvoicesLines.Add("DocEntry", docEntry);
										PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);


										PaymentInvoicesLines.Add("SumApplied", Math.Abs(restructuringPayments - ARInvDocTotal));
										PaymentInvoices.Add(PaymentInvoicesLines);
										//}

									}
								}
							}
						}
					}














					//IF THE TRANSACTION HAS AR RESERVE INVOICE
					else
					{
						double ARInvDocTotal = 0;

						// STANDALONE AR  INVOICE
						foreach (var docEntry in InvoicesEntry.Split(';'))
						{
							if (docEntry != "")
							{
								if (docEntry != "0")
								{
									var PaymentInvoicesLines = new Dictionary<string, object>();
									PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
									PaymentInvoicesLines.Add("DocEntry", docEntry);
									PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);

									string qryARInv = $@"SELECT ""DocTotal"" FROM OINV  
                                                    WHERE 
                                                    --""DocStatus"" = 'O' and  
                                                    ""isIns"" = 'N' AND ""DocEntry"" = {docEntry} AND 
                                                    ""CANCELED"" <> 'C' AND 
                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Restructured','Advance')    ";
									DataTable dtARInv = hana.GetData(qryARInv, hana.GetConnection("SAPHana"));
									ARInvDocTotal = Convert.ToDouble(DataAccess.GetData(dtARInv, 0, "DocTotal", "0").ToString());

									if (ARInvDocTotal > 0)
									{
										PaymentInvoicesLines.Add("SumApplied", ARInvDocTotal);
									}

									PaymentInvoices.Add(PaymentInvoicesLines);

								}
							}
						}



						// CHECK FOR AR RESERVE INVOICE DOC ENTRY
						if (ARReserveEntry > 0)
						{
							var PaymentInvoicesLines = new Dictionary<string, object>();
							PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
							PaymentInvoicesLines.Add("DocEntry", ARReserveEntry);
							PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);
							PaymentInvoicesLines.Add("SumApplied", restructuringPayments);

							PaymentInvoices.Add(PaymentInvoicesLines);
						}

					}



					//FOR MISCELLANEOUS PAYMENTS
					if (MiscEntry > 0)
					{
						var PaymentInvoicesLines = new Dictionary<string, object>();
						PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
						PaymentInvoicesLines.Add("DocEntry", MiscEntry);
						PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);
						PaymentInvoicesLines.Add("SumApplied", restructuringPayments);

						PaymentInvoices.Add(PaymentInvoicesLines);
					}
				}








				else
				{
					//AR INVOICE
					foreach (var docEntry in InvoicesEntry.Split(';'))
					{
						if (docEntry != "")
						{
							if (docEntry != "0")
							{
								var PaymentInvoicesLines = new Dictionary<string, object>();
								PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
								PaymentInvoicesLines.Add("DocEntry", docEntry);
								PaymentInvoicesLines.Add("TotalDiscount", CashDiscount);


								//GET INVOICE 
								//string qryARInv = $@"SELECT (""DocTotal"" - ""PaidToDate"") ""NetTotal"" FROM OINV  
								string qryARInv = $@"SELECT (""DocTotal"" - ""PaidToDate"") ""NetTotal"" FROM OINV  
                                                    WHERE 
                                                    --""DocStatus"" = 'O' and  
                                                    ""isIns"" = 'N' AND ""DocEntry"" = {docEntry} AND ""CANCELED"" = 'N' AND 
                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')   ";
								DataTable dtARInv = hana.GetData(qryARInv, hana.GetConnection("SAPHana"));
								double ARInvDocTotal = Convert.ToDouble(DataAccess.GetData(dtARInv, 0, "NetTotal", "0").ToString());

								//double ARInvDocTotalPlusInterest = 0;

								////GET INTEREST AMOUNT
								//var rows = dtInterestAmount.Select($"DocEntry = '{docEntry}'");
								//if (rows.Any())
								//{
								//    DataTable dtInterestAmount2 = dtInterestAmount.Select($"DocEntry = '{docEntry}'").CopyToDataTable();
								//    if (dtInterestAmount2.Rows.Count > 0)
								//    {
								//        foreach (DataRow row in dtInterestAmount2.Rows)
								//        {
								//            ARInvDocTotalPlusInterest = ARInvDocTotal + Convert.ToDouble(row[1].ToString());
								//        }
								//    }
								//}






								if (dtARInv.Rows.Count > 0)
								{
									if (_TotalPayment > 0)
									{
										if (Math.Round(_TotalPayment, 2) >= Math.Round(ARInvDocTotal, 2))
										{
											PaymentInvoicesLines.Add("SumApplied", ARInvDocTotal);
											_TotalPayment = _TotalPayment - ARInvDocTotal;
										}
										else
										{
											PaymentInvoicesLines.Add("SumApplied", _TotalPayment);
											_TotalPayment = 0;
										}
									}
									PaymentInvoices.Add(PaymentInvoicesLines);
								}

							}
						}
					}













					//AR DOWNPAYMENT INVOICE
					foreach (var docEntry in DPDocEntry.Split(';'))
					{
						if (docEntry != "")
						{
							if (docEntry != "0")
							{
								var PaymentInvoicesLines = new Dictionary<string, object>();
								PaymentInvoicesLines.Add("InvoiceType", "it_DownPayment");
								PaymentInvoicesLines.Add("DocEntry", docEntry);


								////APPLY CASH DISCOUNT
								//string qryCashDiscount = $@"SELECT * FROM OINV WHERE ""DocEntry"" = {docEntry}";
								//DataTable dtCashDiscount = hana.GetData(qryCashDiscount, hana.GetConnection("SAPHana"));
								//string cdTerm = DataAccess.GetData(dtCashDiscount, 0, "U_PaymentOrder", "0").ToString();
								//string cdType = DataAccess.GetData(dtCashDiscount, 0, "U_Type", "").ToString();

								//foreach (GridViewRow row in dtPayments.Rows)
								//{
								//    if (row.Cells[41].Text.ToString() == cdTerm && row.Cells[42].Text.ToString() == cdType)
								//    {
								//        PaymentInvoicesLines.Add("TotalDiscount", row.Cells[2].Text.ToString());
								//    }
								//}




								//GET INVOICE 
								string qryARDP = $@"SELECT (""DocTotal"" - ""PaidToDate"") ""NetTotal"" FROM ODPI  
                                                    WHERE ""DocStatus"" = 'O' AND ""DocEntry"" = {docEntry} AND ""CANCELED"" = 'N'  AND IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')   ";
								DataTable dtARDP = hana.GetData(qryARDP, hana.GetConnection("SAPHana"));
								double ARDPDocTotal = Convert.ToDouble(DataAccess.GetData(dtARDP, 0, "NetTotal", "0").ToString());


								if (dtARDP.Rows.Count > 0)
								{
									if (_TotalPayment > 0)
									{
										double test1 = Math.Round(_TotalPayment, 2);
										double test2 = Math.Round(ARDPDocTotal, 2);

										if (Math.Round(_TotalPayment, 2) >= Math.Round(ARDPDocTotal, 2))
										{
											PaymentInvoicesLines.Add("SumApplied", ARDPDocTotal);
											_TotalPayment = _TotalPayment - ARDPDocTotal;
										}
										else
										{
											PaymentInvoicesLines.Add("SumApplied", _TotalPayment);
											_TotalPayment = 0;
										}
									}
									PaymentInvoices.Add(PaymentInvoicesLines);
								}

							}
						}
					}














					//AR RESERVCE INVOICE
					foreach (var docEntry in InvoicesEntry.Split(';'))
					{
						if (docEntry != "")
						{
							if (docEntry != "0")
							{
								var PaymentInvoicesLines = new Dictionary<string, object>();
								PaymentInvoicesLines.Add("InvoiceType", "it_Invoice");
								PaymentInvoicesLines.Add("DocEntry", docEntry);


								////APPLY CASH DISCOUNT
								//string qryCashDiscount = $@"SELECT * FROM OINV WHERE ""DocEntry"" = {docEntry}";
								//DataTable dtCashDiscount = hana.GetData(qryCashDiscount, hana.GetConnection("SAPHana"));
								//string cdTerm = DataAccess.GetData(dtCashDiscount, 0, "U_PaymentOrder", "0").ToString();
								//string cdType = DataAccess.GetData(dtCashDiscount, 0, "U_Type", "").ToString();

								//foreach (GridViewRow row in dtPayments.Rows)
								//{
								//    if (row.Cells[41].Text.ToString() == cdTerm && row.Cells[42].Text.ToString() == cdType)
								//    {
								//        PaymentInvoicesLines.Add("TotalDiscount", row.Cells[2].Text.ToString());
								//    }
								//}


								//GET INVOICE 
								string qryARResInv = $@"SELECT (""DocTotal"" - ""PaidToDate"") ""NetTotal"" FROM OINV  
                                                    WHERE ""DocStatus"" = 'O' and  ""isIns"" = 'Y' AND ""DocEntry"" = {docEntry} AND ""CANCELED"" = 'N'  
                                                    AND IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')   ";
								DataTable dtARResInv = hana.GetData(qryARResInv, hana.GetConnection("SAPHana"));
								double ARResInvDocTotal = Convert.ToDouble(DataAccess.GetData(dtARResInv, 0, "NetTotal", "0").ToString());


								if (dtARResInv.Rows.Count > 0)
								{
									if (_TotalPayment > 0)
									{
										if (Math.Round(_TotalPayment, 2) >= Math.Round(ARResInvDocTotal, 2))
										{
											PaymentInvoicesLines.Add("SumApplied", ARResInvDocTotal);
											_TotalPayment = _TotalPayment - ARResInvDocTotal;
										}
										else
										{
											PaymentInvoicesLines.Add("SumApplied", _TotalPayment);
											_TotalPayment = 0;
										}
									}
									PaymentInvoices.Add(PaymentInvoicesLines);
								}

							}
						}
					}

				}
















				IList<IDictionary<string, object>> PaymentChecks = new List<IDictionary<string, object>>();
				IList<IDictionary<string, object>> PaymentCreditCards = new List<IDictionary<string, object>>();
				double TotalCash = 0;
				double TotalCheck = 0;
				double TotalCredit = 0;
				double TotalPayment = 0;
				double TotalTransferSum = 0;
				double TotalOtherSum = 0;



				//if (dtPayments == null)
				//{
				//    TotalCash = restructuringPayments;
				//    TotalPayment = restructuringPayments;
				//}
				//else
				//{







				double temp_ToBePaid = IPAmount;
				double temp_TotalPayment = temp_ToBePaid;

				//if payment is for restructuring
				if (TagRestructuring == "Y")
				{

					IncomingPayments.Add("U_Restructured", "Y");

					//GET EXISTING INCOMING PAYMENT
					string qryExistingPayments = $@"SELECT distinct ""CashSum"", ""CreditSum"", ""CheckSum"", ""TrsfrSum"", ""TrsfrDate"", ""TrsfrAcct"", ""TrsfrRef"", ""U_OthPayment""  FROM ORCT A  WHERE	A.""DocEntry"" = {RestructureDocEntryPayment} AND IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')   ;";
					DataTable dtExistingPayments = hana.GetData(qryExistingPayments, hana.GetConnection("SAPHana"));

					double PaymentCashSum = Convert.ToDouble(DataAccess.GetData(dtExistingPayments, 0, "CashSum", "0").ToString());
					double PaymentCreditSum = Convert.ToDouble(DataAccess.GetData(dtExistingPayments, 0, "CreditSum", "0").ToString());
					double PaymentCheckSum = Convert.ToDouble(DataAccess.GetData(dtExistingPayments, 0, "CheckSum", "0").ToString());
					double PaymentBankTransfer = Convert.ToDouble(DataAccess.GetData(dtExistingPayments, 0, "TrsfrSum", "0").ToString());

					var lines = new Dictionary<string, object>();

					//if cash
					if (PaymentCashSum > 0)
					{
						IncomingPayments.Add("CashAccount", cashAccount);
						TotalCash = PaymentCashSum;
						CashAmount = TotalCash;
					}

					//if credit card
					if (PaymentCreditSum > 0)
					{

						if (WithDeposit == "Y")
						{
							IncomingPayments.Add("U_WDeposit", "Y");
						}





						//GET CREDIT CARD DETAILS FROM EXISTING 
						DataTable dtCredit = hana.GetData($@"select distinct ""CreditCard"", ""CreditAcct"", ""VoucherNum"", ""CreditSum"", ""CardValid"", ""CrTypeCode"", ""NumOfPmnts"",
                                                              ""VoucherNum"", ""OwnerIdNum"", ""OwnerPhone"", ""U_CCBrand"" 
                                                            from rct3 where ""DocNum"" = {RestructureDocEntryPayment} ", hana.GetConnection("SAPHana"));

						string creditcard = DataAccess.GetData(dtCredit, 0, "CreditCard", "0").ToString();
						lines.Add("CreditCard", int.Parse(creditcard));
						lines.Add("CreditAcct", DataAccess.GetData(dtCredit, 0, "CreditAcct", "").ToString());
						lines.Add("CreditCardNumber", DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString());
						lines.Add("CreditSum", Convert.ToDouble(DataAccess.GetData(dtCredit, 0, "CreditSum", "0")));

						lines.Add("CardValidUntil", Convert.ToDateTime(DataAccess.GetData(dtCredit, 0, "CardValid", "")).ToString("yyyy-MM-dd"));
						lines.Add("PaymentMethodCode", int.Parse(DataAccess.GetData(dtCredit, 0, "CrTypeCode", "0").ToString()));
						lines.Add("NumOfPayments", int.Parse(DataAccess.GetData(dtCredit, 0, "NumOfPmnts", "0").ToString()));
						lines.Add("VoucherNum", DataAccess.GetData(dtCredit, 0, "VoucherNum", "").ToString());

						lines.Add("OwnerIdNum", DataAccess.GetData(dtCredit, 0, "OwnerIdNum", "").ToString());
						lines.Add("OwnerPhone", DataAccess.GetData(dtCredit, 0, "OwnerPhone", "").ToString());
						lines.Add("CreditType", "cr_Regular");
						lines.Add("U_CCBrand", DataAccess.GetData(dtCredit, 0, "U_CCBrand", "").ToString());

						PaymentCreditCards.Add(lines);
						CreditAmount = PaymentCreditSum;
					}

					//if check
					if (PaymentCheckSum > 0)
					{

						IncomingPayments.Add("CashAccount", checkAccount);

						//CHECK IF CHECK IS ALREADY DEPOSITED
						string qryDeposit = $@"SELECT DISTINCT E.* FROM ORCT A INNER JOIN RCT1 B ON A.""DocEntry"" = B.""DocNum"" 
                                            INNER JOIN OCHH C ON B.""CheckNum"" = C.""CheckNum"" INNER JOIN DPS1 D ON 
                                            C.""CheckKey"" = D.""CheckKey"" INNER JOIN ODPS E ON E.""DeposId"" = D.""DepositId""
                                            WHERE A.""DocEntry"" = {RestructureDocEntryPayment} AND E.""Canceled"" ='N' and E.""CnclDps"" = -1";
						DataTable dtDeposit = hana.GetData(qryDeposit, hana.GetConnection("SAPHana"));

						if (DataHelper.DataTableExist(dtDeposit))
						{
							TotalCash = PaymentCheckSum;
							CashAmount = TotalCash;
						}
						else
						{
							#region checkDetails 

							//GET CHECK DETAILS OF PAYMENT
							string qryCheck = $@"SELECT DISTINCT B.""CheckSum"", B.""DueDate"", B.""BankCode"", B.""Branch"", B.""U_Bank"", B.""AcctNum"", B.""U_Branch"", B.""CheckAct"", B.""U_CheckStat"",
                                                B.""U_UpdateDate"", B.""U_ReleaseDate"", B.""CheckNum""  FROM  RCT1 B INNER JOIN OCHH C ON B.""CheckNum"" = C.""CheckNum""  
                                              WHERE B.""DocNum"" = {RestructureDocEntryPayment} AND C.""CheckKey"" NOT IN     
                                              (SELECT x.""CheckKey"" FROM DPS1 x INNER JOIN ODPS y ON x.""DepositId"" = y.""DeposId""  
                                               WHERE y.""Canceled"" ='N' and y.""CnclDps"" = -1)";
							DataTable dtCheck = hana.GetData(qryCheck, hana.GetConnection("SAPHana"));

							foreach (DataRow dr in dtExistingPayments.Rows)
							{
								string checkNumber = DataAccess.GetData(dtCheck, 0, "CheckNum", "0").ToString();
								if (int.Parse(checkNumber) != 0)
								{
									lines.Add("CheckNumber", int.Parse(checkNumber));
									lines.Add("CheckSum", Convert.ToDouble(DataAccess.GetData(dtCheck, 0, "CheckSum", "0").ToString()));
									lines.Add("DueDate", Convert.ToDateTime(DataAccess.GetData(dtCheck, 0, "DueDate", "0").ToString()).ToString("yyyyMMdd"));
									lines.Add("BankCode", (DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "BankCode", "").ToString()));
									lines.Add("Branch", (DataAccess.GetData(dtCheck, 0, "Branch", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "Branch", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "Branch", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "Branch", "").ToString()));
									lines.Add("CountryCode", "PH");

									lines.Add("U_Bank", (DataAccess.GetData(dtCheck, 0, "U_Bank", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_Bank", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "U_Bank", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_Bank", "").ToString()));
									lines.Add("AccounttNum", (DataAccess.GetData(dtCheck, 0, "AcctNum", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "AcctNum", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "AcctNum", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "AcctNum", "").ToString()));
									lines.Add("U_Branch", (DataAccess.GetData(dtCheck, 0, "U_Branch", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_Branch", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "U_Branch", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_Branch", "").ToString()));
									lines.Add("CheckAccount", (DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "CheckAct", "").ToString()));

									lines.Add("U_CheckStat", (DataAccess.GetData(dtCheck, 0, "U_CheckStat", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_CheckStat", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "U_CheckStat", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_CheckStat", "").ToString()));
									lines.Add("U_UpdateDate", (DataAccess.GetData(dtCheck, 0, "U_UpdateDate", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_UpdateDate", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "U_UpdateDate", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_UpdateDate", "").ToString()));
									lines.Add("U_ReleaseDate", (DataAccess.GetData(dtCheck, 0, "U_ReleaseDate", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_ReleaseDate", "").ToString()) == "&#160;" ? "" : (DataAccess.GetData(dtCheck, 0, "U_ReleaseDate", "").ToString() == "&nbsp;" ? "" : DataAccess.GetData(dtCheck, 0, "U_ReleaseDate", "").ToString()));


									PaymentChecks.Add(lines);
									CheckAmount = PaymentCheckSum;
								}
							}
							#endregion

						}
					}

					//if bank transfer
					if (PaymentBankTransfer > 0)
					{
						IncomingPayments.Add("TransferDate", Convert.ToDateTime(DataAccess.GetData(dtExistingPayments, 0, "TrsfrDate", "").ToString()).ToString("yyyy-MM-dd"));
						IncomingPayments.Add("TransferAccount", DataAccess.GetData(dtExistingPayments, 0, "TrsfrAcct", "0").ToString());
						IncomingPayments.Add("TransferReference", DataAccess.GetData(dtExistingPayments, 0, "TrsfrRef", "0").ToString());
						IncomingPayments.Add("TransferSum", PaymentBankTransfer);
						IncomingPayments.Add("U_OthPayment", DataAccess.GetData(dtExistingPayments, 0, "U_OthPayment", "").ToString());

						if (DataAccess.GetData(dtExistingPayments, 0, "U_OthPayment", "").ToString() == "")
						{
							TransferAmount = PaymentBankTransfer;
						}
						else
						{
							OthersAmount = PaymentBankTransfer;
						}

					}





				}
				else
				{



					//if payment is from normal payment in cash register
					foreach (GridViewRow row in dtPayments.Rows)
					{
						var lines = new Dictionary<string, object>();

						//** get total cash payment **//
						string PayType = row.Cells[1].Text;
						switch (PayType)
						{
							case "Cash":
								IncomingPayments.Add("CashAccount", cashAccount);
								TotalCash += double.Parse(row.Cells[2].Text);

								CashAmount = TotalCash;
								break;

							case "Credit":

								TotalCredit += double.Parse(row.Cells[2].Text);

								DataTable dtCredit = hana.GetData($@"SELECT DISTINCT ""CreditCard"" FROM OCRC Where ""CardName"" ='{row.Cells[9].Text}'", hana.GetConnection("SAPHana"));

								string creditcard = dtCredit.Rows[0][0].ToString();
								lines.Add("CreditCard", int.Parse(creditcard));
								lines.Add("CreditAcct", row.Cells[11].Text);
								lines.Add("CreditCardNumber", row.Cells[12].Text);
								lines.Add("CreditSum", TotalCredit);
								lines.Add("CardValidUntil", Convert.ToDateTime(row.Cells[13].Text).ToString("yyyy-MM-dd"));
								lines.Add("PaymentMethodCode", int.Parse(row.Cells[16].Text));
								lines.Add("NumOfPayments", int.Parse(row.Cells[18].Text));
								//lines.Add("VoucherNum", row.Cells[19].Text);
								lines.Add("VoucherNum", row.Cells[12].Text);
								lines.Add("OwnerIdNum", row.Cells[14].Text);
								lines.Add("OwnerPhone", row.Cells[15].Text);
								lines.Add("CreditType", "cr_Regular");
								lines.Add("U_CCBrand", CardBrandAccount);

								PaymentCreditCards.Add(lines);
								glAccount = cashAccount;


								CreditAmount = TotalCredit;
								break;

							case "Check":
								TotalCheck += double.Parse(row.Cells[2].Text);


								string checkNumber = row.Cells[3].Text;
								if (int.Parse(checkNumber) != 0)
								{
									lines.Add("CheckNumber", int.Parse(checkNumber));
									lines.Add("CheckSum", double.Parse(row.Cells[2].Text));
									lines.Add("DueDate", Convert.ToDateTime(row.Cells[7].Text).ToString("yyyyMMdd"));
									lines.Add("BankCode", (row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text) == "&#160;" ? "" : (row.Cells[4].Text == "&nbsp;" ? "" : row.Cells[4].Text));
									lines.Add("Branch", (row.Cells[6].Text == "&nbsp;" ? "" : row.Cells[6].Text) == "&#160;" ? "" : (row.Cells[6].Text == "&nbsp;" ? "" : row.Cells[6].Text));
									lines.Add("CountryCode", "PH");


									lines.Add("U_Bank", (row.Cells[22].Text == "&nbsp;" ? "" : row.Cells[22].Text) == "&#160;" ? "" : (row.Cells[22].Text == "&nbsp;" ? "" : row.Cells[22].Text));
									lines.Add("AccounttNum", (row.Cells[8].Text == "&nbsp;" ? "" : row.Cells[8].Text) == "&#160;" ? "" : (row.Cells[8].Text == "&nbsp;" ? "" : row.Cells[8].Text));
									lines.Add("U_Branch", (row.Cells[24].Text == "&nbsp;" ? "" : row.Cells[24].Text) == "&#160;" ? "" : (row.Cells[24].Text == "&nbsp;" ? "" : row.Cells[24].Text));
									lines.Add("CheckAccount", (row.Cells[25].Text == "&nbsp;" ? "" : row.Cells[25].Text) == "&#160;" ? "" : (row.Cells[25].Text == "&nbsp;" ? "" : row.Cells[25].Text));
									PaymentChecks.Add(lines);
									glAccount = (row.Cells[25].Text.Contains("nbsp") ? row.Cells[25].Text : "");
								}


								CheckAmount = TotalCheck;
								break;

							case "Interbank":
								TotalTransferSum = double.Parse(row.Cells[2].Text);


								IncomingPayments.Add("TransferDate", Convert.ToDateTime(row.Cells[26].Text).ToString("yyyy-MM-dd"));
								IncomingPayments.Add("TransferAccount", row.Cells[28].Text);
								IncomingPayments.Add("TransferReference", row.Cells[29].Text);
								IncomingPayments.Add("TransferSum", TotalTransferSum);

								glAccount = row.Cells[28].Text;

								TransferAmount = TotalTransferSum;
								break;

							default:
								TotalOtherSum = +double.Parse(row.Cells[2].Text);


								string checkNumber1 = row.Cells[34].Text;

								IncomingPayments.Add("TransferDate", Convert.ToDateTime(row.Cells[37].Text).ToString("yyyy-MM-dd"));
								//IncomingPayments.Add("TransferAccount", ConfigSettings.OthersAccount"]);
								IncomingPayments.Add("TransferAccount", row.Cells[35].Text);
								IncomingPayments.Add("TransferReference", checkNumber1);
								IncomingPayments.Add("TransferSum", TotalOtherSum);
								IncomingPayments.Add("U_OthPayment", row.Cells[31].Text);

								glAccount = row.Cells[35].Text;

								OthersAmount = TotalOtherSum;
								break;
						}

						//** get total payments **//
						TotalPayment += double.Parse(row.Cells[2].Text);

					}
				}







































				if (TotalCash > 0)
				{
					IncomingPayments.Add("CashSum", TotalCash);
				}

				//############################## [POSTING HERE] ##############################
				bool result = company.Connect();

				StringBuilder PaymentInvoicesLine = DataHelper.JsonLinesBuilder("PaymentInvoices", PaymentInvoices);
				StringBuilder PaymentChecksLine = DataHelper.JsonLinesBuilder("PaymentChecks", PaymentChecks);
				StringBuilder PaymentCreditCardsLine = DataHelper.JsonLinesBuilder("PaymentCreditCards", PaymentCreditCards);
				StringBuilder strLines = DataHelper.JsonLinesCombine(PaymentInvoicesLine, PaymentChecksLine, PaymentCreditCardsLine);
				var json = DataHelper.JsonBuilder(IncomingPayments, strLines);

				if (PaymentInvoices.Count > 0 || PaymentChecks.Count > 0 || PaymentCreditCards.Count > 0)
				{
					if (company.POST("IncomingPayments", json))
					{
						Message = $"IncomingPayments successfully created.";
						DocEntry = int.Parse(company.ResultDescription);
						output = true;
						AccountNo = glAccount;
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription} - [Incoming Payment Posting]";
						DocEntry = 0;
						AccountNo = "";
					}
				}
				else
				{
					Message = $"({company.ResultCode}) -- No Incoming Payment created (No Invoice / DP selected)-- {company.ResultDescription}";
					DocEntry = 0;
					AccountNo = "";
				}


			}
			catch (Exception ex)
			{
				Message = ex.Message + " - [Incoming Payment Posting]";
				DocEntry = 0;
				AccountNo = "";
			}

			return output;
		}

		void cancelSAPTransactions(StringBuilder model, SapHanaLayer company, string module, int entry)
		{
			if (model.Length > 0)
			{
				company.POST($"{module}('{entry}')/Cancel", new StringBuilder());
			}
		}


		void cancelSAPTransactions1(SapHanaLayer company, string module, int entry)
		{
			company.POST($"{module}('{entry}')/Cancel", new StringBuilder());
		}




		bool IsSuccessful(string sResponse, string module, bool isPatch = false)
		{
			var output = false;
			try
			{
				if (string.IsNullOrEmpty(sResponse) && isPatch)
				{ output = true; }
				else
				{ output = !sResponse.Contains("error"); }
			}
			catch (Exception ex)
			{ throw new Exception($"Error : ({module}) IsSuccessful {ex.Message}"); }

			return output;
		}

		void deleteAddonTransactions(int DocEntry, string ORNumber)
		{
			//DELETE PAYMENT IF ERROR IN POSTING
			hana.Execute($@"DELETE FROM QUT2 Where ""DocEntry"" = {DocEntry} and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
			hana.Execute($@"DELETE FROM QUT3 Where ""DocEntry"" = {DocEntry} and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
			hana.Execute($@"DELETE FROM QUT4 Where ""DocEntry"" = {DocEntry} and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
			hana.Execute($@"DELETE FROM QUT7 Where ""DocEntry"" = {DocEntry} and ""ORNumber"" = '{ORNumber}'", hana.GetConnection("SAOHana"));
		}



		#region RESTRUCTURING

		public bool CreateReservationRestructuring(int DocEntry,
									string DocDate,
									string ORNumber,
									string ProjectCode,
									string Block,
									string Lot,
									double totalAmount,
									double ReservationPayment,
									string AddonCardCode,
									out int SQEntry,
									out int DPEntry,
									out string SapCardCode,
									out string Message)
		{
			bool output = false;
			SapHanaLayer company = new SapHanaLayer();
			SQEntry = 0;
			DPEntry = 0;
			SapCardCode = "";
			try
			{
				output = company.Connect();
				Message = $"({company.ResultCode}) {company.ResultDescription}";
				//int PymtEntry = 0;



				// ### CREATION OF BUSINESS PARTNER ### //
				#region Business Partner
				DataTable getBuyersInfo = new DataTable();

				getBuyersInfo = ws.GetBuyersInfo(AddonCardCode).Tables[0];

				// ### SAP TRANSACTION START HERE ### //
				output = CreateBusinessPartner(company,
													AddonCardCode,
													DataHelper.DataTableRet(getBuyersInfo, 0, "CardName", ""),
													ProjectCode,
													DataHelper.DataTableRet(getBuyersInfo, 0, "LastName", ""),
													DataHelper.DataTableRet(getBuyersInfo, 0, "FirstName", ""),
													DataHelper.DataTableRet(getBuyersInfo, 0, "MiddleName", ""),
													DataHelper.DataTableRet(getBuyersInfo, 0, "TIN", ""),
													DataHelper.DataTableRet(getBuyersInfo, 0, "TaxClassification", ""),
													DataHelper.DataTableRet(getBuyersInfo, 0, "SpecialBuyerRole", ""),
													out SapCardCode,
													out Message);
				#endregion

				//// ### CHECK IF PROCEED TO SALES QUOTATION ### //
				//#region Sales Quotation
				//if (output)
				//{
				//    output = CreateSalesQuotation(company,
				//                            DocEntry,
				//                            SapCardCode,
				//                            DocDate,
				//                            ProjectCode,
				//                            Block,
				//                            Lot,
				//                            DataHelper.DataTableRet(generalData, 0, "HouseModel", ""),
				//                            DataHelper.DataTableRet(generalData, 0, "FinancingScheme", ""),
				//                            DataHelper.DataTableRet(generalData, 0, "ProductType", ""),
				//                            ReservationPayment,
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "DPAmount", "")),
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "LBAmount", "")),
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "GrossTCP", "")),
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "ThresholdAmount", "")),
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "DiscountAmount", "")),
				//                            double.Parse(DataHelper.DataTableRet(generalData, 0, "MiscAmount", "")),
				//                            DataHelper.DataTableRet(generalData, 0, "BatchNum", ""),
				//                            out SQEntry,
				//                            out Message);
				//}
				//else
				//{
				//    if (!string.IsNullOrEmpty(SapCardCode))
				//    {
				//        StringBuilder model = new StringBuilder();
				//        model.Append(company.GET($"BusinessPartners('{SapCardCode}')"));

				//        if (model.Length > 0)
				//        {
				//            company.DELETE($"BusinessPartners('{SapCardCode}')");
				//            deleteAddonTransactions(DocEntry, ORNumber);
				//        }
				//    }
				//}
				//#endregion

				//// ### CHECK IF PROCEED TO DOWNPAYMENT ### //
				//#region AR Downpayment
				//if (output)
				//{

				//    output = CreateDownPayment(company,
				//                                SapCardCode,
				//                                DocDate,
				//                                ReservationPayment,
				//                                "RES",
				//                                "1",
				//                                ProjectCode,
				//                                Block,
				//                                Lot,
				//                                SQEntry,
				//                                out DPEntry,
				//                                out Message);
				//}
				//else
				//{
				//    StringBuilder model = new StringBuilder();
				//    model.Append(company.GET($"Quotations({SQEntry})"));

				//    if (model.Length > 0)
				//    {
				//        company.DELETE($"Quotations({SQEntry})");
				//        deleteAddonTransactions(DocEntry, ORNumber);
				//    }
				//}
				//#endregion
			}
			catch (Exception ex)
			{
				//DELETE PAYMENT IF ERROR IN POSTING
				deleteAddonTransactions(DocEntry, ORNumber);
				Message = ex.Message;
			}
			return output;
		}
		#endregion



		#region JOURNAL ENTRY
		public bool CreateJournalEntry(SapHanaLayer company,
									   string ProjectCode, //2
									   string BPCode,
									   double Amount, //4 
									   int SODocEntry,
									   string FinancingSchme, //6
									   string TaxClassification,
									   string BlockNo, //8
									   string LotNo,
									   string Account1, //10
									   string Account2,
									   string Account3, //12    
									   string Tag,
									   string DocNum, //14
									   string PaymentOrder,
									   string PaymentType, //16
									   string Vatable,
									   string PostingDate, //18
									   string TransCode,
									   out int DocEntry, //20
									   out string Message,
									   [Optional] string ReceiptNo, //22
									   [Optional] double DiscountAmount,
									   [Optional] DataTable dtJECancellation, //24
									   [Optional] double Amount3,
									   [Optional] string TransID
									   ) //14
		{
			bool output = false;

			try
			{
				if (company == null)
				{
					company = new SapHanaLayer();
				}





				//GET BP NAME
				DataTable dtGetBPName = hana.GetData($@"SELECT ""CardName"" FROM ""OCRD"" WHERE ""CardCode"" = '{BPCode}'", hana.GetConnection("SAPHana"));
				string GetCustomerName = DataAccess.GetData(dtGetBPName, 0, "CardName", "0").ToString();

				string qry = $@"SELECT IFNULL(B.""Name"",'CDO') ""Name"",IFNULL(B.""U_SortCode"",'CDO') ""U_SortCode"" FROM OPRJ A INNER JOIN ""@LOCATION"" B ON A.""U_Location"" = B.""Code"" WHERE A.""PrjCode"" = '{ProjectCode}'";
				DataTable dtGetLocation = hana.GetData(qry, hana.GetConnection("SAPHana"));
				string Location = DataAccess.GetData(dtGetLocation, 0, "Name", "CDO").ToString();
				string SortCode = DataAccess.GetData(dtGetLocation, 0, "U_SortCode", "CDO").ToString();


				IDictionary<string, object> JournalEntries = new Dictionary<string, object>()
					{
                    //2023-12-07 : ADDED TRANSID FOR OUR CHECKING WHICH JE 99 IS GOING TO CANCEL
                        //{"Memo", $"Customer Name: {GetCustomerName} | Type: {PaymentType} Pymnt Order: {PaymentOrder}  | Project: {ProjectCode} BL{BlockNo} LT{LotNo}"},
                        {"Memo", $"Customer Name: {GetCustomerName} | Type: {PaymentType} Pymnt Order: {PaymentOrder}  | Project: {ProjectCode} BL{BlockNo} LT{LotNo} | TransID: {TransID}"},

						{"ReferenceDate", PostingDate},
						{"Reference", SortCode},
						{"ProjectCode", ProjectCode},
						{"U_Project", ProjectCode},
						{"TransactionCode", TransCode},
						{"U_BlockNo", BlockNo},
						{"U_LotNo", LotNo},
						{"U_PaymentOrder", PaymentOrder},
						{"U_PaymentType", PaymentType},
						{"U_Branch", Location},
						{"U_SortCode", SortCode},
						{"U_Remarks", $"POSTED BY DREAMS | {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt")}"},
						{"U_ORNo", $"{ReceiptNo}"},
						{"U_JE1TransID", $"{TransID}"},
						{"U_CardCode", BPCode}

					};
				IList<IDictionary<string, object>> JournalEntryLines = new List<IDictionary<string, object>>();
				var Lines = new Dictionary<string, object>();




				//GET SALES DOC NUMBER
				DataTable dtSO = hana.GetData($@"SELECT ""DocNum"" FROM ""ORDR"" WHERE ""DocEntry"" = {SODocEntry}", hana.GetConnection("SAPHana"));
				int SODocNum = int.Parse(DataAccess.GetData(dtSO, 0, "DocNum", "0").ToString());

				//GET PAYMENT SCHEME
				DataTable dtPaymentScheme = hana.GetData($@"SELECT ""U_PmtSchemeType"" FROM ""@FINANCINGSCHEME"" WHERE ""Code"" = '{FinancingSchme}'", hana.GetConnection("SAPHana"));
				string PaymentScheme = DataAccess.GetData(dtPaymentScheme, 0, "U_PmtSchemeType", "0").ToString();

				string BIRCardCode = ConfigSettings.BIRCardCode;

				//GET BP CODE
				DataTable dtBusinessPartner = hana.GetData($@"SELECT ""SAPCardCode"" FROM ""OCRD"" WHERE ""CardCode"" = '{BPCode}'", hana.GetConnection("SAOHana"));
				string SAPBPCode = DataAccess.GetData(dtBusinessPartner, 0, "SAPCardCode", "").ToString();



				if (Tag == "FirstDP")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);


					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 0);
				}

				else if (Tag == "25TCP1")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 1);
				}

				else if (Tag == "25TCP2")
				{
					double amt = (Amount / 1.12);

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 2);
				}

				else if (Tag == "25TCP3")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 3);
				}

				else if (Tag == "25TCP4")
				{

					//2023-06-30: ADD DISCOUNT AMOUNT FOR SALES COLLECTED
					//double amt1 = (Vatable.ToUpper() == "Y" ? Amount / 1.12 : Amount);
					DiscountAmount = (Vatable.ToUpper() == "Y" ? Math.Round((DiscountAmount / 1.12), 2) : Math.Round(DiscountAmount, 2));
					double amt1 = (Vatable.ToUpper() == "Y" ? Math.Round((Amount / 1.12), 2) + DiscountAmount : Math.Round(Amount + DiscountAmount, 2));







					if (Vatable.ToUpper() == "Y")
					{

						//2023-09-05 : MOVED INSIDE VATABLE CONDITION   
						Lines.Add("AccountCode", Account1);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", Math.Round(amt1, 2));
						Lines.Add("Credit", 0);
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);


						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", Account3);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", 0);

						//2023-08-31 : ONLY DIVIDE THIS BY 1.12 WHEN VATABLE
						//Lines.Add("Credit", Amount / 1.12);
						Lines.Add("Credit", Math.Round((Vatable.ToUpper() == "Y" ? Math.Round((Amount / 1.12), 2) : Math.Round(Amount, 2)), 2));

						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);


						double amt2 = (Amount / 1.12) * 0.12;
						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", Account2);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", Math.Round(amt2, 2));
						Lines.Add("Credit", 0);
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);


						string ContractReceivablesInstallmentAccount = ConfigSettings.ContractReceivablesInstallmentAccount;
						Lines = new Dictionary<string, object>();
						//Lines.Add("ShortName", BPCode);
						Lines.Add("ShortName", BPCode);
						Lines.Add("AccountCode", ContractReceivablesInstallmentAccount);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", 0);
						Lines.Add("Credit", Math.Round(amt2, 2));
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);
					}
					//2023-08-31 : ADD POSTING OF JE WHEN NON VAT 
					else
					{
						string SalesCollectedNonVAT = ConfigSettings.SalesCollectedNonVAT;
						string SalesUncollectedNonVAT = ConfigSettings.SalesUncollectedNonVAT;

						double amt2 = (Amount);
						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", SalesCollectedNonVAT);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						//2024-02-12 : CHANGE TO AMT1 TO CONSIDER VATABLE AMOUNT
						//Lines.Add("Debit", Math.Round(amt2, 2));
						Lines.Add("Debit", Math.Round(amt1, 2));
						Lines.Add("Credit", 0);
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);

						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", SalesUncollectedNonVAT);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", 0);
						Lines.Add("Credit", Math.Round(amt2, 2));
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);
					}




					//2023-08-31 : SKIP POSTING OF DISCOUNT WHEN 0 AMOUNT
					if (DiscountAmount > 0)
					{
						//2023-06-30: ADD NEW ACCOUNT FOR DISCOUNT AMOUNT
						string SalesCollectedDiscount = ConfigSettings.SalesCollectedDiscount;
						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", SalesCollectedDiscount);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", 0);
						Lines.Add("Credit", Math.Round(DiscountAmount, 2));
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);
					}

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 4);
				}

				else if (Tag == "25TCP5")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 5);
				}

				else if (Tag == "25TCP6")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt2 = Amount / 1.12;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt2, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt3 = (Amount / 1.12) * 0.12;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt3, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 6);
				}

				else if (Tag == "25TCP7")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 7);
				}

				else if (Tag == "25TCP8")
				{
					double amt = (Amount / 1.12);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 8);
				}

				else if (Tag == "25TCP9")
				{
					//Lines.Add("AccountCode", Account1);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account2);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account3);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount);
					//JournalEntryLines.Add(Lines);


					// GET WITHHOLDING TAX RATE FROM SAP 

					double amt1 = ((Amount * StandardCostFactorRate) / 100) * (_TotalPayment / Amount);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt1, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt2 = ((Amount * StandardCostFactorRate) / 100) - (((Amount * StandardCostFactorRate) / 100) * (_TotalPayment / Amount));
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt2, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt3 = ((Amount * StandardCostFactorRate) / 100);
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt3, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 9);
				}

				#region commented
				//else if (Tag == "25TCP10")
				//{
				//    double amt = (Amount / 1.12) * 0.05;
				//    Lines.Add("AccountCode", Account1);
				//    Lines.Add("ProjectCode", ProjectCode);
				//    Lines.Add("U_SONo", SODocNum);
				//    Lines.Add("Debit", amt);
				//    Lines.Add("Credit", 0);
				//    JournalEntryLines.Add(Lines);

				//    Lines = new Dictionary<string, object>();
				//    //Lines.Add("ShortName", BPCode);
				//    Lines.Add("ShortName", BIRCardCode);
				//    Lines.Add("AccountCode", Account2);
				//    Lines.Add("ProjectCode", ProjectCode);
				//    Lines.Add("U_SONo", SODocNum);
				//    Lines.Add("Debit", 0);
				//    Lines.Add("Credit", amt);
				//    JournalEntryLines.Add(Lines);

				//    JournalEntries.Add("Reference2", DocNum);
				//    JournalEntries.Add("Reference3", 10);
				//}
				#endregion

				else if (Tag == "25TCP11")
				{
					double amt1 = (Vatable.ToUpper() == "Y" ? Amount / 1.12 : Amount);

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt1, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt1, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					if (Vatable.ToUpper() == "Y")
					{
						string ContractReceivablesInstallmentAccount = ConfigSettings.ContractReceivablesInstallmentAccount;
						double amt = (Amount / 1.12) * 0.12;
						Lines = new Dictionary<string, object>();
						Lines.Add("ShortName", BPCode);
						Lines.Add("AccountCode", ContractReceivablesInstallmentAccount);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", Math.Round(amt, 2));
						Lines.Add("Credit", 0);
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);

						amt = (Amount / 1.12) * 0.12;
						Lines = new Dictionary<string, object>();
						Lines.Add("AccountCode", Account3);
						Lines.Add("ProjectCode", ProjectCode);
						Lines.Add("U_SONo", SODocNum);
						Lines.Add("Debit", 0);
						Lines.Add("Credit", Math.Round(amt, 2));
						Lines.Add("U_API_Vendor", GetCustomerName);

						//2023-09-01 : ADD COSTING CODE
						Lines.Add("CostingCode", "SLSMKTG");
						Lines.Add("CostingCode2", SortCode);
						JournalEntryLines.Add(Lines);
					}





					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 11);
				}

				#region commented
				//else if (Tag == "25TCP12")
				//{
				//    double amt = (Amount / 1.12) * 0.12;
				//    Lines.Add("AccountCode", Account1);
				//    Lines.Add("ProjectCode", ProjectCode);
				//    Lines.Add("U_SONo", SODocNum);
				//    Lines.Add("Debit", amt);
				//    Lines.Add("Credit", 0);
				//    JournalEntryLines.Add(Lines);

				//    Lines = new Dictionary<string, object>();
				//    Lines.Add("AccountCode", Account2);
				//    Lines.Add("ProjectCode", ProjectCode);
				//    Lines.Add("U_SONo", SODocNum);
				//    Lines.Add("Debit", 0);
				//    Lines.Add("Credit", amt);
				//    JournalEntryLines.Add(Lines);

				//    JournalEntries.Add("Reference3", 12);
				//}
				#endregion

				else if (Tag == "25TCP13")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 13);
				}

				else if (Tag == "25TCP14")
				{
					double amt1 = (Vatable.ToUpper() == "Y" ? Amount / 1.12 : Amount);

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt1, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt1, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 14);
				}

				else if (Tag == "25TCP15")
				{
					//Lines.Add("AccountCode", Account1);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);

					//double amt = Amount / 1.12;
					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account2);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", amt);
					//JournalEntryLines.Add(Lines);

					//amt = (Amount / 1.12) * 0.12;
					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account3);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount);
					//JournalEntryLines.Add(Lines);

					//JournalEntries.Add("Reference3", 15);

					double amt1 = Amount / 1.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt1, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt2 = (Amount / 1.12) * 0.12;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt2, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 15);
				}

				else if (Tag == "25TCP16")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 16);
				}

				else if (Tag == "25TCP17")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 17);
				}

				else if (Tag == "25TCP18")
				{
					//Lines.Add("AccountCode", Account1);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account2);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount);
					//JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account3);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount);
					//JournalEntryLines.Add(Lines);

					double amt1 = Amount * 0.25;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt1, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt2 = (Amount * 0.75);
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt2, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 18);
				}

				else if (Tag == "25TCP19")
				{
					//Lines.Add("AccountCode", Account1);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);

					//double amt1 = Amount / 1.12;
					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account2);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", amt1);
					//JournalEntryLines.Add(Lines);

					//double amt2 = (Amount / 1.12) * 0.12;
					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account3);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount);
					//JournalEntryLines.Add(Lines);

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					double amt = Amount / 1.12;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					amt = (Amount / 1.12) * 0.12;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 19);
				}

				else if (Tag == "25TCP20")
				{
					double amt = (Amount / 1.12) * 0.12;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(amt, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(amt, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 20);
				}

				else if (Tag == "25TCP21")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 21);
				}

				else if (Tag == "25TCP22")
				{
					//double amt1 = (Amount / 1.12) * 0.05;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 22);
				}

				else if (Tag == "25TCP23")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 23);
				}

				else if (Tag == "25TCP24")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 24);
				}


				else if (Tag == "WithoutLTS")
				{

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					//Lines.Add("ShortName", BPCode);
					//Lines.Add("AccountCode", Account1);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);


					Lines = new Dictionary<string, object>();
					//2024-08-15: REMOVED POSTING OF SHORTNAME, ONLY ADD ACCOUNT CODE
					//Lines.Add("ShortName", BPCode);-- latest 
					//Lines.Add("ShortName", BIRCardCode); 
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "NLTS");
				}

				//2023-08-15 : CREATE WITHOUTLTS FOR MISCELLANEOUS
				else if (Tag == "WithoutLTSMisc")
				{

					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//2024-08-15: REMOVED POSTING OF SHORTNAME, ONLY ADD ACCOUNT CODE
					//Lines.Add("ShortName", BPCode); -- latest 
					//Lines.Add("ShortName", BIRCardCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "NLTSMisc");
				}





				else if (Tag == "Commission")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", Account3);
				}


				else if (Tag == "25TCP25")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 25);
				}

				else if (Tag == "25TCP26")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 26);
				}

				else if (Tag == "25TCP27")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 27);
				}

				else if (Tag == "25TCP28")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 28);
				}




				//2023-06-30 : ADDED NEW POSTING FOR SALES COLLECTED (DISCOUNT)
				else if (Tag == "25TCP29")
				{

					//2023-06-30: ADD DISCOUNT AMOUNT FOR SALES COLLECTED
					//double amt1 = (Vatable.ToUpper() == "Y" ? Amount / 1.12 : Amount);
					DiscountAmount = (Vatable.ToUpper() == "Y" ? (DiscountAmount / 1.12) : DiscountAmount);
					double amt1 = (Vatable.ToUpper() == "Y" ? (Amount / 1.12) + DiscountAmount : Amount + DiscountAmount);


					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(DiscountAmount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					//if (Vatable.ToUpper() == "Y")
					//{
					//    double amt2 = (Amount / 1.12) * 0.12;
					//    Lines = new Dictionary<string, object>();
					//    Lines.Add("AccountCode", Account2);
					//    Lines.Add("ProjectCode", ProjectCode);
					//    Lines.Add("U_SONo", SODocNum);
					//    Lines.Add("Debit", amt2);
					//    Lines.Add("Credit", 0);
					//    Lines.Add("U_API_Vendor", GetCustomerName);
					//    JournalEntryLines.Add(Lines);


					//    string ContractReceivablesInstallmentAccount = ConfigSettings.ContractReceivablesInstallmentAccount;
					//    Lines = new Dictionary<string, object>();
					//    //Lines.Add("ShortName", BPCode);
					//    Lines.Add("ShortName", BPCode);
					//    Lines.Add("AccountCode", ContractReceivablesInstallmentAccount);
					//    Lines.Add("ProjectCode", ProjectCode);
					//    Lines.Add("U_SONo", SODocNum);
					//    Lines.Add("Debit", 0);
					//    Lines.Add("Credit", amt2);
					//    Lines.Add("U_API_Vendor", GetCustomerName);
					//    JournalEntryLines.Add(Lines);
					//}


					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", Account3);
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount / 1.12);
					//Lines.Add("U_API_Vendor", GetCustomerName);
					//JournalEntryLines.Add(Lines);


					//2023-06-30: ADD NEW ACCOUNT FOR DISCOUNT AMOUNT
					string SalesCollectedDiscount = ConfigSettings.SalesCollectedDiscount;
					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", SalesCollectedDiscount);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(DiscountAmount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 29);
				}

				//2023-07-06 : DINW POSTING -- 25TCP30: https://docs.google.com/spreadsheets/d/1rbPD_Ml1CoRWerx3nPklB3XrODP5GBUslbJwvHmbuBQ/edit#gid=34173818
				else if (Tag == "25TCP30")
				{
					//double amt1 = (Amount / 1.12) * 0.05;
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BIRCardCode);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 30);
				}





				else if (Tag == "ARDPJE1")
				{


					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();

					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);





					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE1");
				}


				else if (Tag == "ARDPJE2")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE2");
				}


				else if (Tag == "ARINVJE3")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE3");
				}


				else if (Tag == "ARINVJE4")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE4");
				}





				else if (Tag == "Restructure1")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount * 0.12, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", "102103"); //Contract Receivable
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", 0);
					//Lines.Add("Credit", Amount * 0.12);
					//JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", "102103"); //Contract Receivable
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount * 0.12, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "Restructure1");
				}


				else if (Tag == "Restructure2")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					//Lines = new Dictionary<string, object>();
					//Lines.Add("AccountCode", "102103"); //Contract Receivable
					//Lines.Add("ProjectCode", ProjectCode);
					//Lines.Add("U_SONo", SODocNum);
					//Lines.Add("Debit", Amount * 0.12);
					//Lines.Add("Credit", 0);
					//JournalEntryLines.Add(Lines);


					Lines = new Dictionary<string, object>();
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", "102103"); //Contract Receivable
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount * 0.12, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", "102103");
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount * 0.12, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					//2023-09-01 : ADD COSTING CODE
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "Restructure2");
				}




				else if (Tag == "25TCP99")
				{


					//2023-08-01 : CHECK IF DATATABLE FETCHED IS NOT NULL
					if (dtJECancellation != null)
					{
						//2023-08-01 : LOOP ON DATATABLE TO GET 
						foreach (DataRow row in dtJECancellation.Rows)
						{
							string Account = "";
							string ShortName = "";
							string Project = "";
							string SONo = "";
							string U_API_Vendor = "";

							double Debit = 0;
							double Credit = 0;

							Account = row["Account"].ToString();
							ShortName = row["ShortName"].ToString();
							Project = row["Project"].ToString();
							SONo = row["U_SONo"].ToString();
							U_API_Vendor = row["U_API_Vendor"].ToString();

							Debit = double.Parse(row["Debit"].ToString());
							Credit = double.Parse(row["Credit"].ToString());

							Lines = new Dictionary<string, object>();

							//2023-08-01 : IF SHORTNAME <> ACCOUNT CODE THEN, GET CONTROL ACCOUNT
							if (ShortName != Account)
							{
								Lines.Add("ShortName", ShortName);

								//2023-10-12 : ADD ACCOUNT CODE ( Contract Receivables - Installment (102103) ) FOR 99 INSTALLMENT 
								string ContractReceivablesInstallmentAccount = ConfigSettings.ContractReceivablesInstallmentAccount;
								//2024-02-15 : REMOVE THIS CONDITION, BECAUSE CONTRACTRECINSTALLMENT ACCOUNT GIVES A FLOATING AMOUNT IN OUR REPORT
								//Lines.Add("AccountCode", ContractReceivablesInstallmentAccount);
								Lines.Add("AccountCode", Account);
							}
							else
							{
								Lines.Add("AccountCode", Account);
							}

							//2023-08-01 : IF DEBIT, POST ON CREDIT
							if (Debit > 0)
							{
								Lines.Add("Debit", 0);
								Lines.Add("Credit", Math.Round(Debit, 2));
							}
							else
							{
								Lines.Add("Debit", Credit);
								Lines.Add("Credit", 0);
							}

							Lines.Add("ProjectCode", Project);
							Lines.Add("U_SONo", SONo);
							Lines.Add("U_API_Vendor", U_API_Vendor);

							//2023-09-01 : ADD COSTING CODE
							Lines.Add("CostingCode", "SLSMKTG");
							Lines.Add("CostingCode2", SortCode);
							JournalEntryLines.Add(Lines);


						}
					}




					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", 99);

					JournalEntries.Add("U_CancelTag", "Y");
				}




				//20223-10-10 : Reversal of Deposit from Cust (APDP) - temporary
				else if (Tag == "RVDP")
				{
					//LINE 1
					Lines.Add("ShortName", BPCode);
					//Lines.Add("AccountCode", Account1);

					//2024-01-11 : ADD ACCOUNT CODE AS CONTROL ACCOUNT
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(DiscountAmount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount3, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "RVDP");

				}

				//20223-10-10 : Reversal of Deposit from Cust (APDP) - temporary
				else if (Tag == "XRVD")
				{
					//LINE 1
					Lines.Add("ShortName", BPCode);
					//Lines.Add("AccountCode", Account1);
					//2024-02-23 : C/O MS KATE : CHECK JE MAPPING 
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(DiscountAmount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);
					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "XRVD");

				}


				//20223-10-17 : Reversal of JE1 - partial payment
				else if (Tag == "JE5")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE5");
				}

				//20223-10-17 : Reversal of JE3 - partial payment
				else if (Tag == "JE6")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "JE6");
				}


				//20223-11-10 : Reversal of NLTS
				else if (Tag == "RNLT")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", Account3);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "RNLT");
				}


				//20223-11-10 : Reversal of RNLT
				else if (Tag == "XRNL")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "XRNL");
				}




				//20223-11-13 : Reversal of MLOI
				else if (Tag == "RMLO")
				{
					//Lines.Add("ShortName", BPCode);
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "RMLO");
				}


				//20223-11-13 : Reversal of RMLO
				else if (Tag == "XRML")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("ShortName", BPCode);
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);


					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "XRML");
				}


				//2023-09-23 : NO LTS - SURCHARGE JE POSTING 
				else if (Tag == "NLSC")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount3, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2) + Math.Round(Amount3, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "NLSC");
				}


				//2023-09-23 : NO LTS - SURCHARGE JE POSTING 
				else if (Tag == "NLIT")
				{
					Lines.Add("AccountCode", Account1);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account2);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", Math.Round(Amount3, 2));
					Lines.Add("Credit", 0);
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					Lines = new Dictionary<string, object>();
					Lines.Add("AccountCode", Account3);
					Lines.Add("ProjectCode", ProjectCode);
					Lines.Add("U_SONo", SODocNum);
					Lines.Add("Debit", 0);
					Lines.Add("Credit", Math.Round(Amount, 2) + Math.Round(Amount3, 2));
					Lines.Add("U_API_Vendor", GetCustomerName);

					Lines.Add("CostingCode", "SLSMKTG");
					Lines.Add("CostingCode2", SortCode);
					JournalEntryLines.Add(Lines);

					JournalEntries.Add("Reference2", DocNum);
					JournalEntries.Add("Reference3", "NLIT");
				}

				//############################## [POSTING HERE] ##############################
				bool result = company.Connect();

				StringBuilder DocumentLine = DataHelper.JsonLinesBuilder("JournalEntryLines", JournalEntryLines);
				var json = DataHelper.JsonBuilder(JournalEntries, DocumentLine);

				if (Math.Round(Amount, 2) > 0)
				{

					if (JournalEntryLines.Count > 0)
					{
						if (company.POST("JournalEntries", json))
						{
							Message = $"JournalEntries successfully created.";
							DocEntry = int.Parse(company.ResultDescription);
							output = true;
						}
						else
						{
							Message = $"({company.ResultCode}) {company.ResultDescription} | {Tag}";
							DocEntry = 0;
						}
					}
					else
					{
						Message = $"({company.ResultCode}) {company.ResultDescription} | {Tag}";
						DocEntry = 0;
					}
				}
				else
				{
					output = true;
					Message = $"JournalEntries successfully created. (0 Amount posted)";
					DocEntry = 0;
				}
			}
			catch (Exception ex)
			{
				Message = ex.Message + " | " + Tag;
				DocEntry = 0;
			}
			return output;
		}


		#endregion


	}
}