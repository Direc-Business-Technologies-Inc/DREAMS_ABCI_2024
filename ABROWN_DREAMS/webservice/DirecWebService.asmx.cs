using ABROWN_DREAMS.VIewModels;
using DataCipher;
using DirecLayer;
using Newtonsoft.Json;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Web;
using ABROWN_DREAMS.pages;
using System.Security.Cryptography;
using System.Drawing;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
namespace ABROWN_DREAMS
{
	/// <summary>
	/// Summary description for DirecWebService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class DirecWebService : WebService
	{
		DirecService wcf;
		SAPHanaAccess hana = new SAPHanaAccess();

		#region "USER MANAGEMENT"
		//##### UPDATE ACTIVITY #####//
		[WebMethod]
		private bool LastActivityDate(int uid)
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand updatecmd = new HanaCommand($@"UPDATE ""OUSR"" SET ""LastActivityDate"" = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ""ID"" = :uid", conn))
					{
						updatecmd.Parameters.AddWithValue("uid", uid);
						conn.Open();
						updatecmd.ExecuteNonQuery();
						conn.Close();
						ret = true;
					}
				}
			}
			catch (Exception ex) { ret = false; }
			return ret;
		}

		//####### GET USERNAME ########//
		[WebMethod]
		public string WebUsername(int uid)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand($@"SELECT ""Username"" FROM OUSR WHERE ""ID"" = :uid", con))
						{
							cmd.Parameters.AddWithValue("uid", uid);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								con.Open();
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Username", "admin");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		public string WebProfile(int uid)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(@"SELECT LEFT(""FirstName"", 1) || '. ' || ""LastName"" ""Name"" FROM ""OMBR"" WHERE ""UserID"" = :uid", con))
						{
							cmd.Parameters.AddWithValue("uid", uid);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								con.Open();
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Name", "admin");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		public bool WebLogin(string uid, string pwd)
		{
			bool ret;

			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT * FROM OUSR WHERE ""IsLock""=false AND ""Username"" = '{uid}' AND ""Password"" = '{Cryption.Encrypt(uid + pwd)}'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							con.Open();
							using (DataTable dt = new DataTable())
							{
								da.Fill(dt);
								ret = DataAccess.Exist(dt);
								if (ret == true)
								{
									using (HanaCommand updatecmd = new HanaCommand($@"UPDATE OUSR SET ""UpdateDate"" = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE ""Username"" = '{uid}'", con))
									{
										updatecmd.ExecuteNonQuery();
										LastActivityDate(Convert.ToInt32(DataAccess.GetData(dt, 0, "ID", "1")));
									}
								}
							}
						}
					}
				}
			}
			catch (Exception e)
			{ ret = false; }
			return ret;
		}


		//####### GET USERID ########//
		[WebMethod]
		public int WebUserID(string uid)
		{
			int ret;

			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(@"SELECT ""ID"" FROM OUSR WHERE ""Username"" = :uid", con))
					{
						cmd.Parameters.AddWithValue(":uid", uid);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataTable dt = new DataTable())
							{
								con.Open();
								da.Fill(dt);
								ret = Convert.ToInt32(DataAccess.GetData(dt, 0, "ID", "1"));
							}
						}
					}
				}
			}
			catch
			{ ret = 0; }
			return ret;
		}


		[WebMethod]
		public DataSet GetUsers()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT A.""ID"",A.""Username"", TRIM(CONCAT(B.""LastName"", CONCAT(', ' ,B.""FirstName""))) ""Name"",  B.""Email"", A.""IsLock"", C.""IsActive"" FROM ""OUSR"" A INNER JOIN ""OMBR"" B ON A.""ID"" = B.""UserID"" LEFT JOIN ""USR3"" C ON A.""ID"" = C.""UserID"" WHERE A.""IsShow"" = TRUE ORDER BY UPPER(TRIM(CONCAT(B.""LastName"", CONCAT(', ' ,B.""FirstName""))))";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetUsers");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetUserRole()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT * FROM ""ROLE"" ORDER BY ""Id""";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetUserRole");
						ret = ds;
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public int SetOUSR(string LastName, string FirstName, string MiddleName, string Email, string Username, string Password, out string Msg)
		{
			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_SetInOUSR", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("LastName", LastName);
							cmd.Parameters.AddWithValue("FirstName", FirstName);
							cmd.Parameters.AddWithValue("MiddleName", MiddleName);
							cmd.Parameters.AddWithValue("Email", Email);
							cmd.Parameters.AddWithValue("UserName", Username);
							cmd.Parameters.AddWithValue("Password", Password);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								con.Open();
								da.Fill(dt);
								con.Close();
								Msg = string.Empty;
								return 1;
							}
						}
					}
				}
			}
			catch (Exception ex) { Msg = ex.Message; return 0; }
		}

		[WebMethod]
		public bool SetRole(int UserID, string Role, int User)
		{
			bool ret = true;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($"sp_SetInROLE", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("Role", Role);
						cmd.Parameters.AddWithValue("User", User);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool SetUpOUSR(int UserID, string LastName, string FirstName, string MiddleName, string Email, string Username, string Password)
		{
			bool ret = false;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($"sp_SetUpOUSR", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("LastName", LastName);
						cmd.Parameters.AddWithValue("FirstName", FirstName);
						cmd.Parameters.AddWithValue("MiddleName", MiddleName);
						cmd.Parameters.AddWithValue("Email", Email);
						cmd.Parameters.AddWithValue("UserName", Username);
						cmd.Parameters.AddWithValue("Password", Password);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool DeleteRole(int uid)
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand updatecmd = new HanaCommand($@"DELETE FROM ""USR2"" WHERE ""UserID"" = :UserID", conn))
					{
						updatecmd.Parameters.AddWithValue("UserID", uid);
						conn.Open();
						updatecmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool DeleteMode(int uid)
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand updatecmd = new HanaCommand($@"DELETE FROM ""USR1"" WHERE ""UserID"" = :UserID", conn))
					{
						updatecmd.Parameters.AddWithValue("UserID", uid);
						conn.Open();
						updatecmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool SetMode(int UserID, string Mode, int User)
		{
			bool ret = true;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($"sp_SetInMODE", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("Mode", Mode);
						cmd.Parameters.AddWithValue("User", User);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public DataSet GetUserByID(int UserID)
		{
			DataSet ret = null;
			try
			{
				string query = $"CALL sp_GetUserByID ({UserID})";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetUserByID");
						ret = ds;
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetUserRolesByID(int UserID)
		{
			DataSet ret = null;
			try
			{
				string query = $"CALL sp_GetUserRolesByID ({UserID})";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetUserRolesByID");
						ret = ds;
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		#endregion

		#region "BUYER'S INFORMATION"

		//########## [ GET OLST NAME ] ##########//
		[WebMethod]
		public string GetOLSTName(string Code)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(@"SELECT ""Name"" FROM ""OLST"" WHERE ""Code"" = :Code", con))
						{
							cmd.Parameters.AddWithValue("Code", Code);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Name", Code);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = Code; }
			return ret;
		}

		[WebMethod]
		public bool OLSTExist(string Code)
		{
			bool ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(@"SELECT ""Name"" FROM ""OLST"" WHERE ""Code"" = :Code AND ""IsShow"" = true", con))
						{
							cmd.Parameters.AddWithValue("Code", Code);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = DataAccess.Exist(dt);
							}
						}
					}
				}
			}
			catch
			{ ret = false; }
			return ret;
		}

		[WebMethod]
		public DataSet select_temp_crd2(int UserID, int ID)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""LineNum"" ""ID"", ""Name"", ""Age"", ""Relationship"" FROM ""temp_CRD2"" WHERE ""UserID"" = :UserID AND ""ID"" = :ID";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("UserID", UserID);
							cmd.Parameters.AddWithValue("ID", ID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "select_temp_crd2");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet select_temp_Listcrd2(int UserID, int ID)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT * FROM ""temp_CRD2"" WHERE ""UserID"" = :UserID AND ""ID"" = :ID";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("UserID", UserID);
							cmd.Parameters.AddWithValue("ID", ID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "select_temp_Listcrd2");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet select_temp_crd5(int UserID)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""ID"",""Relationship"", ""LastName"" || ', ' || ""FirstName"" ""Name"", ""Gender"",""Email"",""SPAFormDocument"" FROM ""temp_CRD5"" WHERE ""UserID"" = :UserID";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("UserID", UserID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "select_temp_crd5");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet select_temp_Listcrd5(int UserID)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT * FROM ""temp_CRD5"" WHERE ""UserID"" = :UserID";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("UserID", UserID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "select_temp_Listcrd5");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool DeleteSPADependent(int UserID, int ID, int LineNum)
		{
			bool ret;
			try
			{
				string query = @"DELETE FROM ""temp_CRD2"" WHERE ""UserID"" = :userID AND ""ID"" = :ID AND ""LineNum"" = :LineNUm";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("ID", ID);
						cmd.Parameters.AddWithValue("LineNum", LineNum);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool DeleteSPADependentByID(int UserID, int ID)
		{
			bool ret;
			try
			{
				string query = @"DELETE FROM ""temp_CRD2"" WHERE ""UserID"" = :userID AND ""ID"" = :ID";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("ID", ID);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool DeleteListSPA(int UserID, int ID)
		{
			bool ret;
			try
			{
				string query = @"DELETE FROM ""temp_CRD5"" WHERE ""UserID"" = :userID AND ""ID"" = :ID";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("ID", ID);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		//########## [ INSERT/UPDATE Business Partner ] ##########//
		[WebMethod]
		public string BusinessPartner(string CardCode, string Nature, //2
									  string IDType, string IDNo,//4
									  string SalesAgent, string LastName, //6
									  string FirstName, string MiddleName,//8
									  string Gender, string Citizenship,//10
									  string Birthday, string BirthPlace,//12
									  string HomeTelNo, string CellNo,//14
									  string EmailAddress, string FBAccount,//16
									  string TIN, string SSSNo,//18
									  string GSISNo, string PagibiNo,//20
									  string PresentAddress, string PermanentAddress,//22
																					 //string HomeOwnership, double YearsStay, 
									  string EmpBusName, string EmpBusAdd,//24
									  string Position, double YearsService,//26
									  string OfficeTelNo, string FaxNo,//28
									  string EmpStatus, string NatureEmp,//30
									  string CivilStatus, string SpouseLastName,//32
									  string SpouseFirstName, string SpouseMiddleName,//34
									  string SpouseGender, string SpouseCitizenship,//36
									  string SpouseBirthday, string SpouseBirthPlace,//38
									  string SpouseCellNo, string SpouseEmailAddress,//40
									  string SpouseFBAccount, string SpouseTIN,//42
									  string SpouseSSSNo, string SpouseGSISNo,//44
									  string SpousePagibiNo, string SpousePosition,//46
									  double SpouseYearsService, string SpouseOfficeTelNo,//48
									  string SpouseFaxNo, string SpouseEmpBusName,//50
									  string SpouseEmpBusAdd, string SpouseEmpStatus,//52
									  string SpouseNatureEmp, string Remarks,//54
									  DataTable dtDependent, DataTable dtBankAccount,//56
									  DataTable dtCharacterRef, bool IsUpdate,//58
									  int UserID, string BusinessType,//60
									  string SalesAgentDocument, string PresentPostalCode,//62
									  string PermanentPostalCode, string PresentCountry,//64
									  string PermanentCountry, string PermanentYearStay,//66
									  string Profession, string SourceofFunds,//68
									  string Occupation, string MonthlyGrossIncome, //70
									  string SpouseResidentialNo, string BusinessCountry,//72
									  string PresentYrStay, string CompanyName,//74 
									  string Comaker, string TaxClassification,//76
									  string IDType2, string IDNo2,//78
									  string IDType3, string IDNo3,  //80
									  string IDType4, string IDNo4,//82
									  string SpecialInstructions, string BusinessPhoneNo, //84
									  string OtherSourceOfFunds, string CertifyCompleteName,  //86
									  string CertifyDate, string IDOthers,//88
									  string IDOthers2, string IDOthers3,  //90
									  string IDOthers4, string AuthorizedPersonAddress, //92
									  string AuthorizedPersonStreet, string AuthorizedPersonSubdivision, //94
									  string AuthorizedPersonBarangay, string AuthorizedPersonCity,//96
									  string AuthorizedPersonProvince, string PresentStreet  //98
									  , string PresentSubdivision, string PresentBarangay //100
									  , string PresentCity, string PresentProvince //102
									  , string PermanentStreet, string PermanentSubdivision //104
									  , string PermanentBarangay, string PermanentCity //106
									  , string PermanentProvince, string SpecifiedBusinessType //108
									  , bool Conforme, string ProofOfBillingAttachment //110
									  , string ValidId1Attachment, string ValidId2Attachment //112
									  , string Religion, string SECCORIDNo//114
									  , string SpecialBuyerRole, string iGuid, //16
									   string ProofOfIncomeAttachment, DataTable dtSPA //117
									  , string SpouseBusinessCountry
										)
		{
			string ret;
			try
			{
				string TransType;
				string query = "sp_BusinessPartner";
				string Code = (int.Parse(GetAutoKey(1, "C")) + UserID).ToString();
				if (InsertBPTables(CardCode, Code, dtDependent, dtBankAccount, dtCharacterRef, IsUpdate, UserID, dtSPA) == true)
				{
					using (DataTable dt = new DataTable())
					{
						using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
						{
							using (HanaCommand cmd = new HanaCommand(query, con))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("CardCode", CardCode);
								cmd.Parameters.AddWithValue("Code", Code); //2
								cmd.Parameters.AddWithValue("SalesAgent", SalesAgent); //3
								cmd.Parameters.AddWithValue("LastName", LastName); //4
								cmd.Parameters.AddWithValue("FirstName", FirstName); //5
								cmd.Parameters.AddWithValue("MiddleName", MiddleName); //6
								cmd.Parameters.AddWithValue("Nature", Nature); //7
								cmd.Parameters.AddWithValue("IDType", IDType); //8
								cmd.Parameters.AddWithValue("IDNo", IDNo); //9
								cmd.Parameters.AddWithValue("Gender", Gender); //10
								cmd.Parameters.AddWithValue("Citizenship", Citizenship); //11
								cmd.Parameters.AddWithValue("Birthday", Convert.ToDateTime(Birthday)); //12
								cmd.Parameters.AddWithValue("BirthPlace", BirthPlace);//13
								cmd.Parameters.AddWithValue("HomeTelNo", HomeTelNo); //14
								cmd.Parameters.AddWithValue("CellNo", CellNo); //15
								cmd.Parameters.AddWithValue("EmailAddress", EmailAddress); //16
								cmd.Parameters.AddWithValue("FBAccount", FBAccount); //17
								cmd.Parameters.AddWithValue("TIN", TIN); //18
								cmd.Parameters.AddWithValue("SSSNo", SSSNo); //19
								cmd.Parameters.AddWithValue("GSISNo", GSISNo); //20
								cmd.Parameters.AddWithValue("PagibiNo", PagibiNo); //21
								cmd.Parameters.AddWithValue("PresentAddress", PresentAddress); //22
								cmd.Parameters.AddWithValue("PermanentAddress", PermanentAddress);   //23
																									 //cmd.Parameters.AddWithValue("HomeOwnership", HomeOwnership);
																									 //cmd.Parameters.AddWithValue("YearsStay", YearsStay); //24
								cmd.Parameters.AddWithValue("EmpBusName", EmpBusName); //24
								cmd.Parameters.AddWithValue("EmpBusAdd", EmpBusAdd); //25
								cmd.Parameters.AddWithValue("Position", Position); //26
								cmd.Parameters.AddWithValue("YearsService", YearsService);  //27
								cmd.Parameters.AddWithValue("OfficeTelNo", OfficeTelNo); //28
								cmd.Parameters.AddWithValue("FaxNo", FaxNo);   //29
								cmd.Parameters.AddWithValue("EmpStatus", EmpStatus); //30
								cmd.Parameters.AddWithValue("NatureEmp", NatureEmp);  //31
								cmd.Parameters.AddWithValue("CivilStatus", CivilStatus); //32
								cmd.Parameters.AddWithValue("SpouseLastName", SpouseLastName); //33
								cmd.Parameters.AddWithValue("SpouseFirstName", SpouseFirstName); //34
								cmd.Parameters.AddWithValue("SpouseMiddleName", SpouseMiddleName); //35
								cmd.Parameters.AddWithValue("SpouseGender", SpouseGender); //36
								cmd.Parameters.AddWithValue("SpouseCitizenship", SpouseCitizenship); //37
								cmd.Parameters.AddWithValue("SpouseBirthday", Convert.ToDateTime(SpouseBirthday)); //38
								cmd.Parameters.AddWithValue("SpouseBirthPlace", SpouseBirthPlace);  //39
								cmd.Parameters.AddWithValue("SpouseCellNo", SpouseCellNo); //40
								cmd.Parameters.AddWithValue("SpouseEmailAddress", SpouseEmailAddress); //41
								cmd.Parameters.AddWithValue("SpouseFBAccount", SpouseFBAccount); //42
								cmd.Parameters.AddWithValue("SpouseTIN", SpouseTIN); //43
								cmd.Parameters.AddWithValue("SpouseSSSNo", SpouseSSSNo); //44
								cmd.Parameters.AddWithValue("SpouseGSISNo", SpouseGSISNo); //45
								cmd.Parameters.AddWithValue("SpousePagibiNo", SpousePagibiNo); //46
								cmd.Parameters.AddWithValue("SpousePosition", SpousePosition); //47
								cmd.Parameters.AddWithValue("SpouseYearsService", SpouseYearsService); //48
								cmd.Parameters.AddWithValue("SpouseOfficeTelNo", SpouseOfficeTelNo);  //49
								cmd.Parameters.AddWithValue("SpouseFaxNo", SpouseFaxNo); //50
								cmd.Parameters.AddWithValue("SpouseEmpBusName", SpouseEmpBusName);  //51
								cmd.Parameters.AddWithValue("SpouseEmpBusAdd", SpouseEmpBusAdd); //52
								cmd.Parameters.AddWithValue("SpouseEmpStatus", SpouseEmpStatus);  //53
								cmd.Parameters.AddWithValue("SpouseNatureEmp", SpouseNatureEmp); //54
								cmd.Parameters.AddWithValue("Remarks", Remarks);  //55
								cmd.Parameters.AddWithValue("UserID", UserID); //56
								cmd.Parameters.AddWithValue("TransType", "C");//57
								cmd.Parameters.AddWithValue("BusinessType", BusinessType); //58
								cmd.Parameters.AddWithValue("SalesAgentDocument", SalesAgentDocument); //59

								//NEW FIELDS

								cmd.Parameters.AddWithValue("PresentPostalCode", PresentPostalCode); //60
								cmd.Parameters.AddWithValue("PermanentPostalCode", PermanentPostalCode); //61
								cmd.Parameters.AddWithValue("PresentCountry", PresentCountry); //62
								cmd.Parameters.AddWithValue("PermanentCountry", PermanentCountry); //63
								cmd.Parameters.AddWithValue("PermanentYrStay", Convert.ToDouble(PermanentYearStay)); //64
								cmd.Parameters.AddWithValue("Profession", Profession); //65
								cmd.Parameters.AddWithValue("SourceOfFunds", SourceofFunds);//66
								cmd.Parameters.AddWithValue("Occupation", Occupation); //67
								cmd.Parameters.AddWithValue("MonthlyIncome", MonthlyGrossIncome);//68
								cmd.Parameters.AddWithValue("EmpBusCountry", BusinessCountry); //69
								cmd.Parameters.AddWithValue("PresentYrStay", Convert.ToDouble(PresentYrStay));//70
								cmd.Parameters.AddWithValue("CompanyName", CompanyName); //71
								cmd.Parameters.AddWithValue("Comaker", Comaker); //72
								cmd.Parameters.AddWithValue("TaxClassification", TaxClassification); //73
								cmd.Parameters.AddWithValue("IDType2", IDType2); //74
								cmd.Parameters.AddWithValue("IDNo2", IDNo2); //75
								cmd.Parameters.AddWithValue("IDType3", IDType3); //76
								cmd.Parameters.AddWithValue("IDNo3", IDNo3); //77
								cmd.Parameters.AddWithValue("IDType4", IDType4);//78
								cmd.Parameters.AddWithValue("IDNo4", IDNo4); //79
								cmd.Parameters.AddWithValue("SpecialInstructions", SpecialInstructions); //80
								cmd.Parameters.AddWithValue("BusinessPhoneNo", BusinessPhoneNo);
								cmd.Parameters.AddWithValue("OtherSourceOfFund", OtherSourceOfFunds); //82
								cmd.Parameters.AddWithValue("CertifyCompleteName", CertifyCompleteName);
								cmd.Parameters.AddWithValue("CertifyDate", Convert.ToDateTime(CertifyDate)); //84
								cmd.Parameters.AddWithValue("IDOthers", IDOthers);
								cmd.Parameters.AddWithValue("IDOthers2", IDOthers2); //86
								cmd.Parameters.AddWithValue("IDOthers3", IDOthers3);
								cmd.Parameters.AddWithValue("IDOthers4", IDOthers4); //88
								cmd.Parameters.AddWithValue("AuthorizedPersonAddress", AuthorizedPersonAddress);
								cmd.Parameters.AddWithValue("AuthorizedPersonStreet", AuthorizedPersonStreet); //90
								cmd.Parameters.AddWithValue("AuthorizedPersonSubdivision", AuthorizedPersonSubdivision);
								cmd.Parameters.AddWithValue("AuthorizedPersonBarangay", AuthorizedPersonBarangay); //92
								cmd.Parameters.AddWithValue("AuthorizedPersonCity", AuthorizedPersonCity);
								cmd.Parameters.AddWithValue("AuthorizedPersonProvince", AuthorizedPersonProvince); //94

								cmd.Parameters.AddWithValue("PresentStreet", PresentStreet);
								cmd.Parameters.AddWithValue("PresentSubdivision", PresentSubdivision); //96
								cmd.Parameters.AddWithValue("PresentBarangay", PresentBarangay);
								cmd.Parameters.AddWithValue("PresentCity", PresentCity); //98
								cmd.Parameters.AddWithValue("PresentProvince", PresentProvince);
								cmd.Parameters.AddWithValue("PermanentStreet", PermanentStreet); //100
								cmd.Parameters.AddWithValue("PermanentSubdivision", PermanentSubdivision);
								cmd.Parameters.AddWithValue("PermanentBarangay", PermanentBarangay);//102
								cmd.Parameters.AddWithValue("PermanentCity", PermanentCity);
								cmd.Parameters.AddWithValue("PermanentProvince", PermanentProvince); //104
								cmd.Parameters.AddWithValue("SpecifiedBusinessType", SpecifiedBusinessType);
								cmd.Parameters.AddWithValue("Conforme", Conforme); //106
								cmd.Parameters.AddWithValue("ProofOfBillingAttachment", ProofOfBillingAttachment);
								cmd.Parameters.AddWithValue("ValidId1Attachment", ValidId1Attachment); //108
								cmd.Parameters.AddWithValue("ValidId2Attachment", ValidId2Attachment);
								cmd.Parameters.AddWithValue("Religion", Religion); //110
								cmd.Parameters.AddWithValue("SECCORIDNo", SECCORIDNo);
								cmd.Parameters.AddWithValue("SpouseResidentialNo", SpouseResidentialNo); //112
								cmd.Parameters.AddWithValue("SpecialBuyerRole", SpecialBuyerRole);
								cmd.Parameters.AddWithValue("Guid", iGuid); //114
								cmd.Parameters.AddWithValue("ProofOfIncomeAttachment", ProofOfIncomeAttachment);
								cmd.Parameters.AddWithValue("SpouseBusinessCountry", SpouseBusinessCountry);

								using (HanaDataAdapter da = new HanaDataAdapter(cmd))
								{
									da.Fill(dt);
									TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
								}

								DataTable dtRet = new DataTable();
								dtRet = hana.GetData($"CALL sp_TransactionNotification ('1','{TransType}','{CardCode}')", hana.GetConnection("SAOHana"));

								int err = Convert.ToInt32(DataAccess.GetData(dtRet, 0, "Column1", "0"));

								if (err == 0)
								{
									if (IsUpdate == false)
									{

										//UPDATE ALL TEMPORARY DATA (TAGGED WITH TEMPORARY CARDCODE) TO ACTUAL CARDCODE
										hana.Execute($"CALL sp_BPModify ('U','{CardCode}','{GetAutoKey(1, "G")}')", hana.GetConnection("SAOHana"));
										GetAutoKey(1, "S");
									}
									else
									{
										using (HanaCommand updcmd = new HanaCommand(query, con))
										{
											updcmd.CommandType = CommandType.StoredProcedure;
											updcmd.Parameters.AddWithValue("CardCode", CardCode);
											updcmd.Parameters.AddWithValue("Code", Code);
											updcmd.Parameters.AddWithValue("SalesAgent", SalesAgent); //3
											updcmd.Parameters.AddWithValue("LastName", LastName);
											updcmd.Parameters.AddWithValue("FirstName", FirstName);
											updcmd.Parameters.AddWithValue("MiddleName", MiddleName);
											updcmd.Parameters.AddWithValue("Nature", Nature);
											updcmd.Parameters.AddWithValue("IDType", IDType);
											updcmd.Parameters.AddWithValue("IDNo", IDNo);
											updcmd.Parameters.AddWithValue("Gender", Gender);
											updcmd.Parameters.AddWithValue("Citizenship", Citizenship);//10
											updcmd.Parameters.AddWithValue("Birthday", Convert.ToDateTime(Birthday));
											updcmd.Parameters.AddWithValue("BirthPlace", BirthPlace);
											updcmd.Parameters.AddWithValue("HomeTelNo", HomeTelNo);
											updcmd.Parameters.AddWithValue("CellNo", CellNo);
											updcmd.Parameters.AddWithValue("EmailAddress", EmailAddress); //15
											updcmd.Parameters.AddWithValue("FBAccount", FBAccount);
											updcmd.Parameters.AddWithValue("TIN", TIN);
											updcmd.Parameters.AddWithValue("SSSNo", SSSNo);
											updcmd.Parameters.AddWithValue("GSISNo", GSISNo);
											updcmd.Parameters.AddWithValue("PagibiNo", PagibiNo); //20
											updcmd.Parameters.AddWithValue("PresentAddress", PresentAddress);
											updcmd.Parameters.AddWithValue("PermanentAddress", PermanentAddress);
											//updcmd.Parameters.AddWithValue("HomeOwnership", HomeOwnership);
											//updcmd.Parameters.AddWithValue("YearsStay", YearsStay);
											updcmd.Parameters.AddWithValue("EmpBusName", EmpBusName);
											updcmd.Parameters.AddWithValue("EmpBusAdd", EmpBusAdd);
											updcmd.Parameters.AddWithValue("Position", Position); //25
											updcmd.Parameters.AddWithValue("YearsService", YearsService);
											updcmd.Parameters.AddWithValue("OfficeTelNo", OfficeTelNo);
											updcmd.Parameters.AddWithValue("FaxNo", FaxNo);
											updcmd.Parameters.AddWithValue("EmpStatus", EmpStatus);
											updcmd.Parameters.AddWithValue("NatureEmp", NatureEmp);
											updcmd.Parameters.AddWithValue("CivilStatus", CivilStatus); //30
											updcmd.Parameters.AddWithValue("SpouseLastName", SpouseLastName);
											updcmd.Parameters.AddWithValue("SpouseFirstName", SpouseFirstName);
											updcmd.Parameters.AddWithValue("SpouseMiddleName", SpouseMiddleName);
											updcmd.Parameters.AddWithValue("SpouseGender", SpouseGender);
											updcmd.Parameters.AddWithValue("SpouseCitizenship", SpouseCitizenship); //35
											updcmd.Parameters.AddWithValue("SpouseBirthday", Convert.ToDateTime(SpouseBirthday));
											updcmd.Parameters.AddWithValue("SpouseBirthPlace", SpouseBirthPlace);
											updcmd.Parameters.AddWithValue("SpouseCellNo", SpouseCellNo);
											updcmd.Parameters.AddWithValue("SpouseEmailAddress", SpouseEmailAddress);
											updcmd.Parameters.AddWithValue("SpouseFBAccount", SpouseFBAccount); //40
											updcmd.Parameters.AddWithValue("SpouseTIN", SpouseTIN);
											updcmd.Parameters.AddWithValue("SpouseSSSNo", SpouseSSSNo);
											updcmd.Parameters.AddWithValue("SpouseGSISNo", SpouseGSISNo);
											updcmd.Parameters.AddWithValue("SpousePagibiNo", SpousePagibiNo);
											updcmd.Parameters.AddWithValue("SpousePosition", SpousePosition); //45
											updcmd.Parameters.AddWithValue("SpouseYearsService", SpouseYearsService);
											updcmd.Parameters.AddWithValue("SpouseOfficeTelNo", SpouseOfficeTelNo);
											updcmd.Parameters.AddWithValue("SpouseFaxNo", SpouseFaxNo);
											updcmd.Parameters.AddWithValue("SpouseEmpBusName", SpouseEmpBusName);
											updcmd.Parameters.AddWithValue("SpouseEmpBusAdd", SpouseEmpBusAdd); //50
											updcmd.Parameters.AddWithValue("SpouseEmpStatus", SpouseEmpStatus);
											updcmd.Parameters.AddWithValue("SpouseNatureEmp", SpouseNatureEmp);
											updcmd.Parameters.AddWithValue("Remarks", Remarks);
											updcmd.Parameters.AddWithValue("UserID", UserID);
											updcmd.Parameters.AddWithValue("TransType", "A");
											updcmd.Parameters.AddWithValue("BusinessType", BusinessType); //55
											updcmd.Parameters.AddWithValue("SalesAgentDocument", SalesAgentDocument);
											updcmd.Parameters.AddWithValue("PresentPostalCode", PresentPostalCode);
											updcmd.Parameters.AddWithValue("PermanentPostalCode", PermanentPostalCode);
											updcmd.Parameters.AddWithValue("PresentCountry", PresentCountry); //60
											updcmd.Parameters.AddWithValue("PermanentCountry", PermanentCountry);
											updcmd.Parameters.AddWithValue("PermanentYrStay", Convert.ToDouble(PermanentYearStay));
											updcmd.Parameters.AddWithValue("Profession", Profession);
											updcmd.Parameters.AddWithValue("SourceOfFunds", SourceofFunds);
											updcmd.Parameters.AddWithValue("Occupation", Occupation); //65
											updcmd.Parameters.AddWithValue("MonthlyIncome", MonthlyGrossIncome);
											updcmd.Parameters.AddWithValue("EmpBusCountry", BusinessCountry);
											updcmd.Parameters.AddWithValue("PresentYrStay", Convert.ToDouble(PresentYrStay));
											updcmd.Parameters.AddWithValue("CompanyName", CompanyName);
											updcmd.Parameters.AddWithValue("Comaker", Comaker);//70
											updcmd.Parameters.AddWithValue("TaxClassification", TaxClassification);
											updcmd.Parameters.AddWithValue("IDType2", IDType2);
											updcmd.Parameters.AddWithValue("IDNo2", IDNo2);
											updcmd.Parameters.AddWithValue("IDType3", IDType3);
											updcmd.Parameters.AddWithValue("IDNo3", IDNo3);
											updcmd.Parameters.AddWithValue("IDType4", IDType4);
											updcmd.Parameters.AddWithValue("IDNo4", IDNo4);
											updcmd.Parameters.AddWithValue("SpecialInstructions", SpecialInstructions);
											updcmd.Parameters.AddWithValue("BusinessPhoneNo", BusinessPhoneNo);
											updcmd.Parameters.AddWithValue("OtherSourceOfFund", OtherSourceOfFunds);
											updcmd.Parameters.AddWithValue("CertifyCompleteName", CertifyCompleteName);
											updcmd.Parameters.AddWithValue("CertifyDate", Convert.ToDateTime(CertifyDate));
											updcmd.Parameters.AddWithValue("IDOthers", IDOthers);
											updcmd.Parameters.AddWithValue("IDOthers2", IDOthers2);
											updcmd.Parameters.AddWithValue("IDOthers3", IDOthers3);
											updcmd.Parameters.AddWithValue("IDOthers4", IDOthers4);
											updcmd.Parameters.AddWithValue("AuthorizedPersonAddress", AuthorizedPersonAddress);
											updcmd.Parameters.AddWithValue("AuthorizedPersonStreet", AuthorizedPersonStreet);
											updcmd.Parameters.AddWithValue("AuthorizedPersonSubdivision", AuthorizedPersonSubdivision);
											updcmd.Parameters.AddWithValue("AuthorizedPersonBarangay", AuthorizedPersonBarangay);
											updcmd.Parameters.AddWithValue("AuthorizedPersonCity", AuthorizedPersonCity);
											updcmd.Parameters.AddWithValue("AuthorizedPersonProvince", AuthorizedPersonProvince);

											updcmd.Parameters.AddWithValue("PresentStreet", PresentStreet);
											updcmd.Parameters.AddWithValue("PresentSubdivision", PresentSubdivision);
											updcmd.Parameters.AddWithValue("PresentBarangay", PresentBarangay);
											updcmd.Parameters.AddWithValue("PresentCity", PresentCity);
											updcmd.Parameters.AddWithValue("PresentProvince", PresentProvince);
											updcmd.Parameters.AddWithValue("PermanentStreet", PermanentStreet);
											updcmd.Parameters.AddWithValue("PermanentSubdivision", PermanentSubdivision);
											updcmd.Parameters.AddWithValue("PermanentBarangay", PermanentBarangay);
											updcmd.Parameters.AddWithValue("PermanentCity", PermanentCity);
											updcmd.Parameters.AddWithValue("PermanentProvince", PermanentProvince);
											updcmd.Parameters.AddWithValue("SpecifiedBusinessType", SpecifiedBusinessType);
											updcmd.Parameters.AddWithValue("Conforme", Conforme);
											updcmd.Parameters.AddWithValue("ProofOfBillingAttachment", ProofOfBillingAttachment);
											updcmd.Parameters.AddWithValue("ValidId1Attachment", ValidId1Attachment);
											updcmd.Parameters.AddWithValue("ValidId2Attachment", ValidId2Attachment);
											updcmd.Parameters.AddWithValue("Religion", Religion);
											updcmd.Parameters.AddWithValue("SECCORIDNo", SECCORIDNo);
											updcmd.Parameters.AddWithValue("SpouseResidentialNo", SpouseResidentialNo);
											updcmd.Parameters.AddWithValue("SpecialBuyerRole", SpecialBuyerRole);
											updcmd.Parameters.AddWithValue("Guid", iGuid);
											updcmd.Parameters.AddWithValue("ProofOfIncomeAttachment", ProofOfIncomeAttachment);
											updcmd.Parameters.AddWithValue("SpouseBusinessCountry", SpouseBusinessCountry);
											con.Open();
											updcmd.ExecuteNonQuery();
										}
										hana.Execute($"CALL sp_BPModify ('U','{Code}','{CardCode}')", hana.GetConnection("SAOHana"));
									}
									LastActivityDate(UserID);
								}
								else
								{ hana.Execute($"CALL sp_BPModify ('D','{CardCode}','')", hana.GetConnection("SAOHana")); }

								ret = GetErr(dtRet);

							}
						}
					}
				}
				else { hana.Execute($"sp_BPModify 'D','{Code}'", hana.GetConnection("SAOHana")); ret = "Error upon saving data."; }

			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		bool InsertBPTables(string CardCode, string Code, DataTable dtDependent, DataTable dtBankAccount, DataTable dtCharacterRef, bool IsUpdate, int UserID, DataTable dtSPA)
		{
			bool ret = true;
			try
			{
				int i = 0;
				string ID = "";
				if (IsUpdate == true)
				{
					if (CardCode.Substring(0, 2) == "BP")
					{
						ID = CardCode.Substring(2, CardCode.Length - 2);
						ID = int.Parse(ID).ToString();
					}
					else
					{
						//JOSES
						int indexOfDash = CardCode.IndexOf("-") + 1;
						ID = CardCode.Remove(0, indexOfDash);

					}
					//ID = CardCode.Substring(2, CardCode.Length - 2);
					//ID = int.Parse(ID).ToString();
				}
				else { ID = GetAutoKey(1, "B"); }
				hana.Execute($@"DELETE FROM ""CRD2"" WHERE ""CardCode"" = '{CardCode}'", hana.GetConnection("SAOHana"));
				foreach (DataRow dr in dtDependent.Rows)
				{ ret = BPDependent(Code, "B", int.Parse(ID), 0, i, dr["Name"].ToString(), Convert.ToInt64(dr["Age"]), dr["Relationship"].ToString()); i++; }

				if (ret == false)
				{ return ret; }

				foreach (DataRow dr in dtBankAccount.Rows)
				{ ret = BPBankAccount(Code, dr["Bank"].ToString(), dr["Branch"].ToString(), dr["AcctType"].ToString(), dr["AcctNo"].ToString()/*, Convert.ToInt64(dr["AvgDailyBal"]), Convert.ToInt64(dr["PresBal"])*/); }

				if (ret == false)
				{ return ret; }

				foreach (DataRow dr in dtCharacterRef.Rows)
				{ ret = BPCharacterRef(Code, dr["Name"].ToString(), dr["Address"].ToString(), dr["TelNo"].ToString(), dr["Email"].ToString()); }


				//2023-06-18: SAVING OF SPA
				foreach (DataRow dr in dtSPA.Rows)
				{
					//INSERT DATA LOOPED FROM TEMP_CRD5 TO CRD5 WITH TEMPORARY CARDCODE
					ret = BPSPACoBorrower(Code, ID, int.Parse(dr["ID"].ToString()), true, false, dr["Relationship"].ToString(), dr["LastName"].ToString(), dr["FirstName"].ToString(), dr["MiddleName"].ToString(),
						dr["Gender"].ToString(), dr["Citizenship"].ToString(), Convert.ToDateTime(dr["BirthDate"].ToString() == "" ? "0001-01-01" : dr["BirthDate"].ToString()).ToString("yyyy-MM-dd"),
						dr["BirthPlace"].ToString(), dr["TelNo"].ToString(), dr["MobileNo"].ToString(), dr["Email"].ToString(), dr["FB"].ToString(), "", "",
						"", "", dr["Address"].ToString(), dr["Address"].ToString(), "", dr["YearsOfStay"].ToString(),
						"", "", "", dr["YearsOfStay"].ToString(), dr["TelNo"].ToString(), dr["TelNo"].ToString()
									, "", "", dr["CivilStatus"].ToString(), dr["SPAFormDocument"].ToString());
				}

				if (ret == false)
				{ return ret; }



				//2023-06-18 : COMMENTED FOR NEW SPA PROCESS
				////sp_temp_InsertListSPA =  DELETE DEPENDENTS AND DELETE SPA BASED ON CARDCODE
				//if (hana.ExecuteStr($"CALL sp_temp_InsertListSPA ('{CardCode}');", hana.GetConnection("SAOHana")) == "Operation completed successfully.")
				//{
				//    DataTable dt = new DataTable();

				//    //GET ALL DATA FROM TEMP_CRD5 TEMPORARY TABLE
				//    dt = select_temp_Listcrd5(UserID).Tables["select_temp_Listcrd5"];
				//    i = 0;
				//    foreach (DataRow dr in dt.Rows)
				//    {
				//        int LineID = int.Parse(dr["ID"].ToString());
				//        bool SPA = false;
				//        bool CoBorrower = false;
				//        //if (Convert.ToBoolean(dr["SPA"]))
				//        //{
				//        SPA = true;
				//        CoBorrower = false;
				//        //}
				//        //else if (Convert.ToBoolean(dr["CoBorrower"]))
				//        //{
				//        //    SPA = false;
				//        //    CoBorrower = true;
				//        //}


				//        //INSERT DATA LOOPED FROM TEMP_CRD5 TO CRD5 WITH TEMPORARY CARDCODE
				//        ret = BPSPACoBorrower(Code, ID, LineID, SPA, CoBorrower, dr["Relationship"].ToString(), dr["LastName"].ToString(), dr["FirstName"].ToString(), dr["MiddleName"].ToString(), dr["Gender"].ToString()
				//                        , dr["Citizenship"].ToString(), Convert.ToDateTime(dr["BirthDate"].ToString() == "" ? "0001-01-01" : dr["BirthDate"].ToString()).ToString("yyyy-MM-dd"), dr["BirthPlace"].ToString(), dr["HomeTelNo"].ToString(), dr["CellNo"].ToString(), dr["Email"].ToString(), dr["FB"].ToString(), dr["TIN"].ToString(), dr["SSSNo"].ToString(), dr["GSISNo"].ToString()
				//                        , dr["PagIbigNo"].ToString(), dr["PresentAddress"].ToString(), dr["PermanentAddress"].ToString(), dr["HomeOwnership"].ToString(), dr["YearsOfStay"].ToString(), dr["EmpBusinessName"].ToString(), dr["EmpBusinessAddress"].ToString()
				//                        , dr["Position"].ToString(), dr["YearsOfStay"].ToString(), dr["OfficeTelNo"].ToString(), dr["FaxNo"].ToString()
				//                        , dr["EmploymentStatus"].ToString(), dr["NatureOfEmp"].ToString(), dr["CivilStatus"].ToString(), dr["SPAFormDocument"].ToString());

				//        //GET DATA FROM TEMP_CRD2 (DEPENDENTS TABLE)
				//        DataTable dt1 = new DataTable();
				//        dt1 = select_temp_Listcrd2(UserID, LineID).Tables["select_temp_Listcrd2"];

				//        //SAVE DATA FROM TEMP_CRD2 TO CRD2
				//        foreach (DataRow SPAdr in dt1.Rows)
				//        { ret = BPDependent(Code, "S", int.Parse(ID), LineID, int.Parse(SPAdr["LineNum"].ToString()), SPAdr["Name"].ToString(), Convert.ToInt64(SPAdr["Age"]), SPAdr["Relationship"].ToString()); i++; }
				//    }


				//    //DELETION OF TEMP_CRD2 AND TEMP_CRD5 DEPENDING ON USER ID
				//    InitializeSPA(UserID);
				//}
				//else { ret = false; }


			}
			catch (Exception ex) { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool BPDependent(string CardCode, string DependentType, int DocEntry, int LineNum, int ID, string Name, double Age, string Relationship)
		{
			bool ret;
			try
			{
				string query = "sp_BPDependent";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("DependentType", DependentType);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("LineNum", LineNum);
						cmd.Parameters.AddWithValue("ID", ID);
						cmd.Parameters.AddWithValue("Name", Name);
						cmd.Parameters.AddWithValue("Age", Age);
						cmd.Parameters.AddWithValue("Relationship", Relationship);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}


		[WebMethod]
		public bool insert_temp__crd5(int UserID, //1
									  int ID, //2
									  bool SPA, //3
									  bool CoBorrower, //4
									  string Relationship, //5
									  string LastName, //6
									  string FirstName, //7
									  string MiddleName, //8
									  string Gender, //9
									  string Citizenship, //10
									  string Birthday, //11
									  string BirthPlace, //12
									  string CellNo, //13
									  string HomeTelNo, //14
									  string Email, //15
									  string FB, //16
									  string TIN, //17
									  string SSSNo, //18
									  string GSISNo, //19
									  string PagibigNo, //20
									  string PresentAdd,  //21
									  string PermanentAdd, //22
									  string Position, //23
									  int YearsofService, //24
									  string OfcNo, //25
									  string FaxNo, //26
									  string HomeOwnership, //27
									  double YearsofStay, //28
									  string EmpBusName, //29
									  string EmpBusAdd, //30
									  string EmpStatus, //31
									  string NatureofEmp, //32
									  string CivilStatus, //33
									  string SPAFormDocument)
		{
			bool ret;
			try
			{
				string query = "sp_temp_ListSPACoBorrower";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("ID", ID);
						cmd.Parameters.AddWithValue("SPA", SPA);
						cmd.Parameters.AddWithValue("CoBorrower", CoBorrower);
						cmd.Parameters.AddWithValue("Relationship", Relationship);
						cmd.Parameters.AddWithValue("SPALastName", LastName);
						cmd.Parameters.AddWithValue("SPAFirstName", FirstName);
						cmd.Parameters.AddWithValue("SPAMiddleName", MiddleName);
						cmd.Parameters.AddWithValue("SPAGender", Gender);
						cmd.Parameters.AddWithValue("SPACitizenship", Citizenship);
						cmd.Parameters.AddWithValue("SPABirthday", Birthday == "" ? null : Birthday);
						cmd.Parameters.AddWithValue("SPABirthPlace", BirthPlace);
						cmd.Parameters.AddWithValue("SPACellNo", CellNo);
						cmd.Parameters.AddWithValue("SPAHomeTelNo", HomeTelNo);
						cmd.Parameters.AddWithValue("SPAEmailAddress", Email);
						cmd.Parameters.AddWithValue("SPAFBAccount", FB);
						cmd.Parameters.AddWithValue("SPATIN", TIN);
						cmd.Parameters.AddWithValue("SPASSSNo", SSSNo);
						cmd.Parameters.AddWithValue("SPAGSISNo", GSISNo);
						cmd.Parameters.AddWithValue("SPAPagibiNo", PagibigNo);
						cmd.Parameters.AddWithValue("SPAPresentAddress", PresentAdd);
						cmd.Parameters.AddWithValue("SPAPermanentAddress", PermanentAdd);
						cmd.Parameters.AddWithValue("SPAPosition", Position);
						cmd.Parameters.AddWithValue("SPAYearsService", YearsofService);
						cmd.Parameters.AddWithValue("SPAOfficeTelNo", OfcNo);
						cmd.Parameters.AddWithValue("SPAFaxNo", FaxNo);
						cmd.Parameters.AddWithValue("SPAHomeOwnership", HomeOwnership);
						cmd.Parameters.AddWithValue("SPAYearsStay", YearsofStay);
						cmd.Parameters.AddWithValue("SPAEmpBusName", EmpBusName);
						cmd.Parameters.AddWithValue("SPAEmpBusAdd", EmpBusAdd);
						cmd.Parameters.AddWithValue("SPAEmpStatus", EmpStatus);
						cmd.Parameters.AddWithValue("SPANatureEmp", NatureofEmp);
						cmd.Parameters.AddWithValue("SPACivilStatus", CivilStatus);
						cmd.Parameters.AddWithValue("SPAFormDocument", SPAFormDocument);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch (Exception ex) { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool InitializeSPA(int UserID)
		{
			bool ret;
			try
			{
				string query = "sp_temp_InitializeSPA";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool AddSPADependent(int UserID, int ID, int LineNum, string Name, int Age, string Relationship)
		{
			bool ret;
			try
			{
				string query = "sp_AddSPADependent";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("UserID", UserID);
						cmd.Parameters.AddWithValue("ID", ID);
						cmd.Parameters.AddWithValue("LineNum", LineNum);
						cmd.Parameters.AddWithValue("Name", Name);
						cmd.Parameters.AddWithValue("Age", Age);
						cmd.Parameters.AddWithValue("Relationship", Relationship);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		bool BPBankAccount(string CardCode, string Bank, string Branch, string AcctType, string AcctNo/*, double AvgDailyBal, double PresentBal*/)
		{
			bool ret;
			try
			{
				string query = "sp_BPBankAccount";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("Bank", Bank);
						cmd.Parameters.AddWithValue("Branch", Branch);
						cmd.Parameters.AddWithValue("AcctType", AcctType);
						cmd.Parameters.AddWithValue("AcctNo", AcctNo);
						//cmd.Parameters.AddWithValue("AvgDailyBal", AvgDailyBal);
						//cmd.Parameters.AddWithValue("PresentBal", PresentBal);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		bool BPCharacterRef(string CardCode, string Name, string Address, string TelNo, string Email)
		{
			bool ret;
			try
			{
				string query = "sp_BPCharacterRef";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("Name", Name);
						cmd.Parameters.AddWithValue("Address", Address);
						cmd.Parameters.AddWithValue("TelNo", TelNo);
						cmd.Parameters.AddWithValue("Email", Email);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool BPSPACoBorrower(string CardCode, string ID, int TargetEntry, bool SPA, bool CoBorrower, string Relationship, string SPALastName
			, string SPAFirstName, string SPAMiddleName, string SPAGender, string SPACitizenship, string SPABirthday, string SPABirthPlace
			, string SPAHomeTelNo, string SPACellNo, string SPAEmailAddress, string SPAFBAccount, string SPATIN, string SPASSSNo
			, string SPAGSISNo, string SPAPagibiNo, string SPAPresentAddress, string SPAPermanentAddress, string SPAHomeOwnership, string SPAYearsStay
			, string SPAEmpBusName, string SPAEmpBusAdd, string SPAPosition, string SPAYearsService
			, string SPAOfficeTelNo, string SPAFaxNo, string SPAEmpStatus, string SPANatureEmp, string SPACivilStatus, string SPAFormDocument)
		{
			bool ret;
			try
			{
				string query = "sp_SPACoBorrower";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("DocEntry", ID); //2
						cmd.Parameters.AddWithValue("LineNum", TargetEntry);
						cmd.Parameters.AddWithValue("SPA", SPA); //4
						cmd.Parameters.AddWithValue("CoBorrower", CoBorrower);
						cmd.Parameters.AddWithValue("Relationship", Relationship); //6
						cmd.Parameters.AddWithValue("SPALastName", SPALastName);
						cmd.Parameters.AddWithValue("SPAFirstName", SPAFirstName); //8
						cmd.Parameters.AddWithValue("SPAMiddleName", SPAMiddleName);
						cmd.Parameters.AddWithValue("SPAGender", SPAGender); //10
						cmd.Parameters.AddWithValue("SPACitizenship", SPACitizenship);
						cmd.Parameters.AddWithValue("SPABirthday", SPABirthday); //12
						cmd.Parameters.AddWithValue("SPABirthPlace", SPABirthPlace);
						cmd.Parameters.AddWithValue("SPACellNo", SPACellNo); //14
						cmd.Parameters.AddWithValue("SPAEmailAddress", SPAEmailAddress);
						cmd.Parameters.AddWithValue("SPAFBAccount", SPAFBAccount); //16
						cmd.Parameters.AddWithValue("SPATIN", SPATIN);
						cmd.Parameters.AddWithValue("SPASSSNo", SPASSSNo); //18
						cmd.Parameters.AddWithValue("SPAGSISNo", SPAGSISNo);
						cmd.Parameters.AddWithValue("SPAPagibiNo", SPAPagibiNo); //20
						cmd.Parameters.AddWithValue("SPAPosition", SPAPosition);
						cmd.Parameters.AddWithValue("SPAYearsService", SPAYearsService); //22
						cmd.Parameters.AddWithValue("SPAOfficeTelNo", SPAOfficeTelNo);
						cmd.Parameters.AddWithValue("SPAFaxNo", SPAFaxNo); //24
						cmd.Parameters.AddWithValue("SPAEmpBusName", SPAEmpBusName);
						cmd.Parameters.AddWithValue("SPAEmpBusAdd", SPAEmpBusAdd); //26
						cmd.Parameters.AddWithValue("SPAEmpStatus", SPAEmpStatus);
						cmd.Parameters.AddWithValue("SPANatureEmp", SPANatureEmp); //28
						cmd.Parameters.AddWithValue("SPAHomeTelNo", SPAHomeTelNo);
						cmd.Parameters.AddWithValue("SPAPresentAddress", SPAPresentAddress); //30
						cmd.Parameters.AddWithValue("SPAPermanentAddress", SPAPermanentAddress);
						cmd.Parameters.AddWithValue("SPAHomeOwnership", SPAHomeOwnership); //32
						cmd.Parameters.AddWithValue("SPAYearsStay", SPAYearsStay);
						cmd.Parameters.AddWithValue("SPACivilStatus", SPACivilStatus); //34
						cmd.Parameters.AddWithValue("SPAFormDocument", SPAFormDocument); //34
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch (Exception ex) { ret = false; }
			return ret;
		}

		[WebMethod]
		public bool BPDelete(string CardCode, int UserID)
		{
			bool ret;
			try
			{
				string query = @"SELECT 'True' FROM ""OQUT"" WHERE ""CardCode"" = :CardCode";
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("CardCode", CardCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								if (DataAccess.Exist(dt) == false)
								{
									query = @"UPDATE ""OCRD"" SET ""IsArchive"" = TRUE, ""UpdateDate"" = CURRENT_DATE, ""UpdateUserID"" = :UserID WHERE ""CardCode"" = :CardCode";
									using (HanaConnection cnn = new HanaConnection(hana.GetConnection("SAOHana")))
									{
										using (HanaCommand com = new HanaCommand(query, cnn))
										{
											com.Parameters.AddWithValue("CardCode", CardCode);
											com.Parameters.AddWithValue("UserID", UserID);
											cnn.Open();
											com.ExecuteNonQuery();
											ret = true;
											LastActivityDate(UserID);
										}
									}
								}
								else { ret = false; }
							}
						}
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		//##### Check if the name of buyer is existing #####//
		[WebMethod]
		public bool BPCheckName(string Name)
		{
			bool ret;
			try
			{
				string query = "SELECT 'True' FROM OCRD WHERE ( LastName + ' ' + FirstName + ' ' + MiddleName ) = @Name";

				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand(query, con))
					{
						cmd.Parameters.AddWithValue("@Name", Name);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		#endregion

		#region "QUOTATION"
		//########## Payment Terms ##########//
		[WebMethod]
		public DataSet GetCompSheet(
																	int Age, string FinancingScheme,//2
																	string HouseStatus, double ODAS, //4
																	double PromoDisc, double DPPercent, //6
																	double ResrvFee, double SpotDPDiscAmount, //8
																	string discountBased, int DPTerms, //10
																	int LTerms, string HideCharge, //12
																	double dpAmount, double disPercent, //14
																	double chargeAmt, string allowed, //16
																	string salesType, string model, //18
																	string Project, int UpdatedDPDueDate, //20
																	string DocDate, string DPDay, //22
																	string dtpDueDate, int dtpDueDateVisible, //24
																	double UserInterestRate, string RetType, //26
																	string adjacent, int MiscDPTerms, //28
																									  //ADDED MISC FIELDS  (2023-03-08)
																	decimal MiscDPAmount = 0, int MiscLBTerms = 0,
																	double AppliedPayment = 0,
																	string UpdateAmortBalance = "", //30
																	string RetainMonthlyAmort = "",
																	double AppliedPaymentMisc = 0
									)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetCompSheetNEW";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
								cmd.Parameters.AddWithValue("Age", Age); //2
								cmd.Parameters.AddWithValue("FinancingScheme", FinancingScheme);
								cmd.Parameters.AddWithValue("HouseStatus", HouseStatus); //4
								cmd.Parameters.AddWithValue("ODAS", ODAS);

								cmd.Parameters.AddWithValue("PromoDisc", PromoDisc); //6
								cmd.Parameters.AddWithValue("DPPercent", DPPercent);
								cmd.Parameters.AddWithValue("ResrvFee", ResrvFee); //8
								cmd.Parameters.AddWithValue("newSpotDPDiscAmount", SpotDPDiscAmount);
								cmd.Parameters.AddWithValue("newdiscountBased", discountBased); //10
								cmd.Parameters.AddWithValue("DPTerms", DPTerms);
								cmd.Parameters.AddWithValue("LTerms", LTerms); //12

								cmd.Parameters.AddWithValue("AddtlChargeBuffer", double.Parse(ConfigSettings.AddtlChargesBuffer));
								cmd.Parameters.AddWithValue("HideCharge", HideCharge); //14
								cmd.Parameters.AddWithValue("newDPAmount", dpAmount);

								cmd.Parameters.AddWithValue("newDiscPercent", disPercent); //16
								cmd.Parameters.AddWithValue("newChargeAmt", chargeAmt);
								cmd.Parameters.AddWithValue("newAllowed", allowed); //18    
								cmd.Parameters.AddWithValue("newSalesType", salesType);
								cmd.Parameters.AddWithValue("newModel", model);  //20

								cmd.Parameters.AddWithValue("newProject", Project);
								cmd.Parameters.AddWithValue("UpdatedDPDueDate", UpdatedDPDueDate); //22
								cmd.Parameters.AddWithValue("DocDate", DocDate);
								cmd.Parameters.AddWithValue("DPDay", DPDay); //24
								cmd.Parameters.AddWithValue("dtpDueDate", dtpDueDate);

								cmd.Parameters.AddWithValue("dtpDueDateVisible", dtpDueDateVisible);  //26
								cmd.Parameters.AddWithValue("UserInterestRate", UserInterestRate);
								cmd.Parameters.AddWithValue("RetType", RetType); //28
								cmd.Parameters.AddWithValue("adjacent", adjacent);

								cmd.Parameters.AddWithValue("AppliedPayment", AppliedPayment); // 30
								cmd.Parameters.AddWithValue("UpdateAmortBalanceTag", UpdateAmortBalance);
								cmd.Parameters.AddWithValue("RetainMonthlyAmortTag", RetainMonthlyAmort); //32

								cmd.Parameters.AddWithValue("MiscDPTerms", MiscDPTerms); //32
								cmd.Parameters.AddWithValue("MiscDPAmount", MiscDPAmount); //32
								cmd.Parameters.AddWithValue("MiscLBTerms", MiscLBTerms); //32

								cmd.Parameters.AddWithValue("MiscAppliedPayment", AppliedPaymentMisc); //32

								da.Fill(ds, "GetCompSheet");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		//########## ########## ##########//


		//########## SELECT STD ##########//
		[WebMethod]
		public DataSet IFExistBP(string LastName, string FirstName, string MiddleName, string CompanyName, string IDNo)
		{
			DataSet ret = null;
			try
			{
				string query = "";

				if (string.IsNullOrWhiteSpace(CompanyName))
				{
					query = $@"SELECT 
                                    A.""CardCode"", 
                                    B.""LastName"" || ', ' || B.""FirstName"" ""Name"", 
                                    A.""IDNo"" 
                                FROM 
                                    ""OCRD"" A INNER JOIN 
                                    ""CRD1"" B ON A.""CardCode"" = B.""CardCode"" AND 
                                    B.""CardType"" = 'Buyer'
                                WHERE 
                                    (B.""LastName"" LiKE '%{LastName}%' AND B.""FirstName"" LiKE '%{FirstName}%' AND B.""MiddleName"" LiKE '%{MiddleName}%') AND 
                                    A.""IDNo"" = '{IDNo}'";
				}
				else
				{
					query = $@"SELECT 
                                    A.""CardCode"", 
                                    B.""CompanyName"",
                                    A.""IDNo"" 
                                FROM 
                                    ""OCRD"" A INNER JOIN 
                                    ""CRD1"" B ON A.""CardCode"" = B.""CardCode"" AND 
                                    B.""CardType"" = 'Buyer'
                                WHERE 
                                    REPLACE(B.""CompanyName"",',','') LIKE  REPLACE('%{CompanyName.Replace("'", "''")}%',',','')  AND 
                                    A.""IDNo"" ='{IDNo}'";
				}

				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							//cmd.Parameters.AddWithValue("LastName", LastName);
							//cmd.Parameters.AddWithValue("FirstName", FirstName);
							//cmd.Parameters.AddWithValue("MiddleName", MiddleName);
							//cmd.Parameters.AddWithValue("CompanyName", CompanyName);
							//cmd.Parameters.AddWithValue("IDNo", IDNo);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "IFExistBP");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetHouseStatus(string oProject, string oBlock, string oLot, string oTerm, string oFScheme)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetHouseStatus", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", oProject);
							cmd.Parameters.AddWithValue("Block", oBlock);
							cmd.Parameters.AddWithValue("Lot", oLot);
							cmd.Parameters.AddWithValue("Term", oTerm);
							cmd.Parameters.AddWithValue("FScheme", oFScheme);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetHouseStatus");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetPaymentTerms(string oModel, string oFeat, string oPriceCat, string oSize, string structure, string oPrjCode, string oHoueStatus, double oLot, double oFloorArea)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetPaymentTerms_OB", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Model", oModel);
							cmd.Parameters.AddWithValue("Feat", oFeat);
							cmd.Parameters.AddWithValue("PriceCat", oPriceCat);
							cmd.Parameters.AddWithValue("Size", oSize);
							cmd.Parameters.AddWithValue("Structure", structure);
							cmd.Parameters.AddWithValue("PrjCode", oPrjCode);
							cmd.Parameters.AddWithValue("HouseStatus", oHoueStatus);
							cmd.Parameters.AddWithValue("Lot", oLot);
							cmd.Parameters.AddWithValue("FloorArea", oFloorArea);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetPaymentTerms");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetHouseModel(string oProject, string oModel)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetHouseModel", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Model", oModel);
							cmd.Parameters.AddWithValue("PrjCode", oProject);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetHouseModel");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetHousePicture(string oDocEntry)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetHousePicture", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetHousePicture");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetPriceDetails(string oModel, string oSalesType)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("sp_GetPriceDetails", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@Model", oModel);
							cmd.Parameters.AddWithValue("@SalesType", oSalesType);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetPriceDetails");
								ret = ds;
							}
						}
					}
				}

			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet PaymentBreakdown(int oTerms, DateTime oDueDate, //2
										double oMonthlyPayment, double oInterest,  //4
										double oTotalPayment, string PaymentType,  //6
										double IRate, double MiscAmount, //8
										double IPSAmount, int AdditionalTerm,
										[Optional] int ContLine, [Optional] DateTime OGDueDate,
										double OriginalMonthly = 0)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_PaymentBreakdown", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Terms", oTerms);
							cmd.Parameters.AddWithValue("DueDate", oDueDate.ToString("yyyy-MM-dd")); //2
							cmd.Parameters.AddWithValue("MonthlyPayment", oMonthlyPayment);
							cmd.Parameters.AddWithValue("InterestRate", oInterest); //4
							cmd.Parameters.AddWithValue("TotalPayment", oTotalPayment);
							cmd.Parameters.AddWithValue("PaymentType", PaymentType); //6
							cmd.Parameters.AddWithValue("IRate", IRate);
							cmd.Parameters.AddWithValue("MiscAmount", MiscAmount); //8
							cmd.Parameters.AddWithValue("IPSAmount", IPSAmount); //8
							cmd.Parameters.AddWithValue("AdditionalTerm", AdditionalTerm); //8 
							cmd.Parameters.AddWithValue("OGDueDate", OGDueDate); //8  
							cmd.Parameters.AddWithValue("ContLine", ContLine); //8      
							cmd.Parameters.AddWithValue("OriginalMonthly", OriginalMonthly); //8      

							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "PaymentBreakdown");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet PaymentBreakdownRestructure(int oTerms, DateTime oDueDate, double oMonthlyPayment, double oInterest, double oTotalPayment, string PaymentType, double IRate)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("sp_PaymentBreakdownRestructure", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Terms", oTerms);
							cmd.Parameters.AddWithValue("@DueDate", oDueDate);
							cmd.Parameters.AddWithValue("@MonthlyPayment", oMonthlyPayment);
							cmd.Parameters.AddWithValue("@InterestRate", oInterest);
							cmd.Parameters.AddWithValue("@TotalPayment", oTotalPayment);
							cmd.Parameters.AddWithValue("@PaymentType", PaymentType);
							cmd.Parameters.AddWithValue("@IRate", IRate);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "PaymentBreakdownRestructure");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet PaymentBreakdownBankPagibig(DateTime oDueDate, double oTotalPayment, double oIPSAmount, [Optional] int MiscContLine)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_PaymentBreakdownBankPagibig", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DueDate", oDueDate);
							cmd.Parameters.AddWithValue("TotalPayment", oTotalPayment);
							cmd.Parameters.AddWithValue("IPS", oIPSAmount);
							cmd.Parameters.AddWithValue("MiscContLine", MiscContLine);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "PaymentBreakdown");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool SetColorByProj(string Proj, string Block, string Lot, string upvalue)
		{
			try
			{
				//UPDATE THIS PLEASE
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"UPDATE ""PRJ2"" SET ""datestatus"" = '{upvalue}' WHERE ""PrjCode"" = :PrjCode AND ""Block"" = :Block AND ""Lot"" = :Lot", con))
					{
						cmd.Parameters.AddWithValue("PrjCode", Proj);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}

		[WebMethod]
		public DataSet GetQuotationByID(int oDocEntry)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetQuotationByID", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetQuotationByID");
								ret = ds;
							}
						}
					}
				}

			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetOtherCharges(string oItemCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("sp_GetOtherCharges", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@ItemCode", oItemCode);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetOtherCharges");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetTotalOtherCharges(string oItemCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetTotalOtherCharges", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@ItemCode", oItemCode);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetTotalOtherCharges");
								ret = ds;
							}
						}
					}
				}

			}
			catch { ret = null; }
			return ret;
		}
		//########## [ GET UFD1 NAME ] ##########//
		[WebMethod]
		public string GetUFD1Name(string oTable, string Code, int ID)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					{
						using (HanaCommand cmd = new HanaCommand($@"SELECT ""Descr"" ""Name"" FROM ""UFD1"" WHERE ""TableID"" = :Table AND ""FldValue"" = :Code AND ""FieldID"" = :ID", con))
						{
							cmd.Parameters.AddWithValue("Table", oTable);
							cmd.Parameters.AddWithValue("Code", Code);
							cmd.Parameters.AddWithValue("ID", ID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Name", "");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}


		//########## [ QUOTATION POSTING ] ###########//
		[WebMethod]
		public string Quotation(int oTab, int DocEntry,  //2
								string oCardCode, DateTime oDate, //4
								string oDocStatus, string oProjCode, //6
								string oPhase, string oBlock, //8
								string oLot, string oModel,  //10
								string oSize, string oFeature,  //12
								string oHouseStatus, double oLotArea, //14 
								double oFloorArea, string oFinancingScheme, //16 
								string oAcctType, string oSalesType,  //18
								string oItemCode, string oItemCodeOC, //20
								double oODAS, double oOMisc, //22
								double oOVAT, double oOTCP,  //24
								double oTcp, double oPromoDisc,  //26
								double oSpotDPPercent, double oSpotDPPercentDiscAmt, //28 DISCOUNT PERCENT AND DISCOUNT AMOUNT
								double oSpotDPDicsAmt, //30
								double oTotalDisc, double oNetDisc,  //34
								double oDAS, double oMisc,  //36
								double oVat, double oNetTcp, //38
								double oDPPercent, double oDPAmount, //40  DOWNPAYMENT PERCENT AND DOWNPAYMENT AMOUNT
								double oResrvFee, //42
								double oNetDP, int oDPTerms,  //44
								DateTime oDPDueDate, double oMonthlyDP, //46 
								int oLMaturityAge, double oLPercent, //48
								double oLAmount, int oLTerms, //50
								DateTime oLDueDate, double oInterestRate, //52
								double oMonthlyAmort, double oGDI, //54
								double oPBasicSalary, double oSBasicSalary, //56
								double oPAllowances, double oSAllowances, //58
								double oPCommission, double oSCommission, //60
								double oPRentalIncome, double oSRentalIncome, //62
								double oPRetainer, double oSRetainer, //64
								double oPOthers, double oSOthers, //66
								double oFood, double oLightWater, //68
								double oTelBill, double oTransportation, //70
								double oRent, double oEducation, //72 
								double oLoanAmort, double oMEOthers, //74
								double AddtlCharges, int UserID, //76 
								string Bank, string allowed, //78
								int termsMonths, string discbase,  //80
								int SpotDueDate, DataTable oDownpayment, //82  
								DataTable oAmort, DataTable oAddCharge,  //84
								GridView EmpList, double NetDas2, //86
								double tcpAddCharges, DateTime ACDueDate,
								double LBMonthly, DateTime selDueDate //88


								)
		{
			{
				string ret;
				try
				{
					string TransType;
					string query = "sp_Quotation";
					string Code = GetAutoKey(2, "C") + UserID;
					InsertSQTables(Code, oDownpayment, oAmort, oAddCharge, EmpList);
					using (var dt = new DataTable())
					{
						using (var con = new HanaConnection(hana.GetConnection("SAOHana")))
						{
							using (var cmd = new HanaCommand(query, con))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("ExistDocEntry", DocEntry);
								cmd.Parameters.AddWithValue("DocEntry", Code);  //2
								cmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D"));
								cmd.Parameters.AddWithValue("CardCode", oCardCode); //4
																					//cmd.Parameters.AddWithValue("DocDate", oDate.ToString("MM/dd/yyyy"));
								cmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd"));
								cmd.Parameters.AddWithValue("DocStatus", oDocStatus); //6
								cmd.Parameters.AddWithValue("ProjCode", oProjCode);
								cmd.Parameters.AddWithValue("Phase", oPhase); //8
								cmd.Parameters.AddWithValue("Block", oBlock);
								cmd.Parameters.AddWithValue("Lot", oLot);  //10
								cmd.Parameters.AddWithValue("Model", oModel);
								cmd.Parameters.AddWithValue("Size", oSize);  //12
								cmd.Parameters.AddWithValue("Feature", oFeature);
								cmd.Parameters.AddWithValue("HouseStatus", oHouseStatus); //14
								cmd.Parameters.AddWithValue("LotArea", oLotArea);
								cmd.Parameters.AddWithValue("FloorArea", oFloorArea); //16
								cmd.Parameters.AddWithValue("FinancingScheme", oFinancingScheme);
								cmd.Parameters.AddWithValue("AcctType", oAcctType); //18
								cmd.Parameters.AddWithValue("SalesType", oSalesType);
								cmd.Parameters.AddWithValue("ItemCode", oItemCode); //20
								cmd.Parameters.AddWithValue("ItemCodeOC", oItemCodeOC);

								cmd.Parameters.AddWithValue("ODas", oODAS);  //22
								cmd.Parameters.AddWithValue("OMisc", oOMisc);
								cmd.Parameters.AddWithValue("OVat", oOVAT); //24
								cmd.Parameters.AddWithValue("OTcp", oOTCP);
								cmd.Parameters.AddWithValue("Tcp", oTcp); //26
								cmd.Parameters.AddWithValue("PromoDisc", oPromoDisc);
								cmd.Parameters.AddWithValue("SpotDPPercent", oSpotDPPercent); //28
								cmd.Parameters.AddWithValue("SpotDPPercentDiscAmt", oSpotDPPercentDiscAmt);
								cmd.Parameters.AddWithValue("SpotDPDiscAmt", oSpotDPDicsAmt); //30
								cmd.Parameters.AddWithValue("TotalDisc", oTotalDisc);
								cmd.Parameters.AddWithValue("NetDisc", oNetDisc); //32
								cmd.Parameters.AddWithValue("Das", oDAS);
								cmd.Parameters.AddWithValue("Misc", oMisc); //34
								cmd.Parameters.AddWithValue("Vat", oVat);
								cmd.Parameters.AddWithValue("NetTcp", oNetTcp); //36
								cmd.Parameters.AddWithValue("DPPercent", oDPPercent);
								cmd.Parameters.AddWithValue("DPAmount", oDPAmount); //38
								cmd.Parameters.AddWithValue("ResrvFee", oResrvFee);
								cmd.Parameters.AddWithValue("NetDP", oNetDP); //40
								cmd.Parameters.AddWithValue("DPTerms", oDPTerms);
								//cmd.Parameters.AddWithValue("DPDueDate", oDPDueDate.ToString("MM/dd/yyyy"));    
								cmd.Parameters.AddWithValue("DPDueDate", oDPDueDate.ToString("yyyy/MM/dd"));    //42
								cmd.Parameters.AddWithValue("MonthlyDP", oMonthlyDP);
								cmd.Parameters.AddWithValue("LMaturityAge", oLMaturityAge); //44
								cmd.Parameters.AddWithValue("LPercent", oLPercent);
								cmd.Parameters.AddWithValue("LAmount", oLAmount); //46
								cmd.Parameters.AddWithValue("LTerms", oLTerms);
								//cmd.Parameters.AddWithValue("LDueDate", oLDueDate.ToString("MM/dd/yyyy")); //52
								cmd.Parameters.AddWithValue("LDueDate", oLDueDate.ToString("yyyy/MM/dd")); //48
								cmd.Parameters.AddWithValue("InterestRate", oInterestRate);
								cmd.Parameters.AddWithValue("MonthlyAmort", oMonthlyAmort); //50
								cmd.Parameters.AddWithValue("Gdi", oGDI);
								cmd.Parameters.AddWithValue("PBasicSalary", oPBasicSalary); //52
								cmd.Parameters.AddWithValue("SBasicSalary", oSBasicSalary);
								cmd.Parameters.AddWithValue("PAllowances", oPAllowances); //54
								cmd.Parameters.AddWithValue("SAllowances", oSAllowances);
								cmd.Parameters.AddWithValue("PCommission", oPCommission); //56
								cmd.Parameters.AddWithValue("SCommission", oSCommission);
								cmd.Parameters.AddWithValue("PRentalIncome", oPRentalIncome); //58
								cmd.Parameters.AddWithValue("SRentalIncome", oSRentalIncome);
								cmd.Parameters.AddWithValue("PRetainer", oPRetainer); //60
								cmd.Parameters.AddWithValue("SRetainer", oSRetainer);
								cmd.Parameters.AddWithValue("POthers", oPOthers); //62
								cmd.Parameters.AddWithValue("SOthers", oSOthers);
								cmd.Parameters.AddWithValue("Food", oFood); //64
								cmd.Parameters.AddWithValue("LightWater", oLightWater);
								cmd.Parameters.AddWithValue("TelBill", oTelBill); //66
								cmd.Parameters.AddWithValue("Transportation", oTransportation);
								cmd.Parameters.AddWithValue("Rent", oRent); //68
								cmd.Parameters.AddWithValue("Education", oEducation);
								cmd.Parameters.AddWithValue("LoanAmort", oLoanAmort); //70
								cmd.Parameters.AddWithValue("MEOthers", oMEOthers);
								cmd.Parameters.AddWithValue("CreateUserID", UserID); //72
								cmd.Parameters.AddWithValue("AddtlCharges", AddtlCharges);
								cmd.Parameters.AddWithValue("Bank", Bank); //74
								cmd.Parameters.AddWithValue("Allowed", allowed);
								cmd.Parameters.AddWithValue("TermsMonth", termsMonths);//76
								cmd.Parameters.AddWithValue("DiscBase", discbase);
								cmd.Parameters.AddWithValue("SpotDueDate", SpotDueDate); //78
								cmd.Parameters.AddWithValue("TransType", "A");
								cmd.Parameters.AddWithValue("NetDas2", NetDas2); //80
								cmd.Parameters.AddWithValue("TCPAddCharges", tcpAddCharges);
								//cmd.Parameters.AddWithValue("ACDueDate", ACDueDate.ToString("MM/dd/yyyy"));
								cmd.Parameters.AddWithValue("ACDueDate", ACDueDate.ToString("yyyy/MM/dd")); //82
								cmd.Parameters.AddWithValue("LBMonthly", LBMonthly);
								//cmd.Parameters.AddWithValue("selDueDate", selDueDate);

								using (var da = new HanaDataAdapter(cmd))
								{
									da.Fill(dt);
									TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
								}

								DataTable dtRet = new DataTable();
								dtRet = hana.GetData($@"CALL sp_TransactionNotification ('2','{TransType}','{Code}')", hana.GetConnection("SAOHana"));

								int err = Convert.ToInt32(DataAccess.GetData(dtRet, 0, "Column1", "0"));

								/**
                                 * Insert data when:
                                 * 1. wizard tab = 4
                                 * 2. error = 0
                                 */
								if (err == 0 && oTab == 4)
								{
									if (DocEntry == 0)
									{
										hana.Execute($@"CALL sp_SQModify ('U','{Code}','{GetAutoKey(2, "G")}')", hana.GetConnection("SAOHana"));
										GetAutoKey(2, "S");
									}
									else
									{
										using (HanaCommand updcmd = new HanaCommand(query, con))
										{
											updcmd.CommandType = CommandType.StoredProcedure;
											updcmd.Parameters.AddWithValue("ExistDocEntry", DocEntry);
											updcmd.Parameters.AddWithValue("DocEntry", Code);
											updcmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D"));
											updcmd.Parameters.AddWithValue("CardCode", oCardCode);
											//updcmd.Parameters.AddWithValue("DocDate", oDate.ToString("MM/dd/yyyy"));
											updcmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("DocStatus", oDocStatus);
											updcmd.Parameters.AddWithValue("ProjCode", oProjCode);
											updcmd.Parameters.AddWithValue("Phase", oPhase);
											updcmd.Parameters.AddWithValue("Block", oBlock);
											updcmd.Parameters.AddWithValue("Lot", oLot);
											updcmd.Parameters.AddWithValue("Model", oModel);
											updcmd.Parameters.AddWithValue("Size", oSize);
											updcmd.Parameters.AddWithValue("Feature", oFeature);
											updcmd.Parameters.AddWithValue("HouseStatus", oHouseStatus);
											updcmd.Parameters.AddWithValue("LotArea", oLotArea);
											updcmd.Parameters.AddWithValue("FloorArea", oFloorArea);
											updcmd.Parameters.AddWithValue("FinancingScheme", oFinancingScheme);
											updcmd.Parameters.AddWithValue("AcctType", oAcctType);
											updcmd.Parameters.AddWithValue("SalesType", oSalesType);
											updcmd.Parameters.AddWithValue("ItemCode", oItemCode);
											updcmd.Parameters.AddWithValue("ItemCodeOC", oItemCodeOC);
											updcmd.Parameters.AddWithValue("ODas", oODAS);
											updcmd.Parameters.AddWithValue("OMisc", oOMisc);
											updcmd.Parameters.AddWithValue("OVat", oOVAT);
											updcmd.Parameters.AddWithValue("OTcp", oOTCP);
											updcmd.Parameters.AddWithValue("Tcp", oTcp);
											updcmd.Parameters.AddWithValue("PromoDisc", oPromoDisc);
											updcmd.Parameters.AddWithValue("SpotDPPercent", oSpotDPPercent);
											updcmd.Parameters.AddWithValue("SpotDPPercentDiscAmt", oSpotDPPercentDiscAmt);
											updcmd.Parameters.AddWithValue("SpotDPDiscAmt", oSpotDPDicsAmt);
											updcmd.Parameters.AddWithValue("TotalDisc", oTotalDisc);
											updcmd.Parameters.AddWithValue("NetDisc", oNetDisc);
											updcmd.Parameters.AddWithValue("Das", oDAS);
											updcmd.Parameters.AddWithValue("Misc", oMisc);
											updcmd.Parameters.AddWithValue("Vat", oVat);
											updcmd.Parameters.AddWithValue("NetTcp", oNetTcp);
											updcmd.Parameters.AddWithValue("DPPercent", oDPPercent);
											updcmd.Parameters.AddWithValue("DPAmount", oDPAmount);
											updcmd.Parameters.AddWithValue("ResrvFee", oResrvFee);
											updcmd.Parameters.AddWithValue("NetDP", oNetDP);
											updcmd.Parameters.AddWithValue("DPTerms", oDPTerms);
											//updcmd.Parameters.AddWithValue("DPDueDate", oDPDueDate.ToString("MM/dd/yyyy"));
											updcmd.Parameters.AddWithValue("DPDueDate", oDPDueDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("MonthlyDP", oMonthlyDP);
											updcmd.Parameters.AddWithValue("LMaturityAge", oLMaturityAge);
											updcmd.Parameters.AddWithValue("LPercent", oLPercent);
											updcmd.Parameters.AddWithValue("LAmount", oLAmount);
											updcmd.Parameters.AddWithValue("LTerms", oLTerms);
											//updcmd.Parameters.AddWithValue("LDueDate", oLDueDate.ToString("MM/dd/yyyy"));
											updcmd.Parameters.AddWithValue("LDueDate", oLDueDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("InterestRate", oInterestRate);
											updcmd.Parameters.AddWithValue("MonthlyAmort", oMonthlyAmort);
											updcmd.Parameters.AddWithValue("Gdi", oGDI);
											updcmd.Parameters.AddWithValue("PBasicSalary", oPBasicSalary);
											updcmd.Parameters.AddWithValue("SBasicSalary", oSBasicSalary);
											updcmd.Parameters.AddWithValue("PAllowances", oPAllowances);
											updcmd.Parameters.AddWithValue("SAllowances", oSAllowances);
											updcmd.Parameters.AddWithValue("PCommission", oPCommission);
											updcmd.Parameters.AddWithValue("SCommission", oSCommission);
											updcmd.Parameters.AddWithValue("PRentalIncome", oPRentalIncome);
											updcmd.Parameters.AddWithValue("SRentalIncome", oSRentalIncome);
											updcmd.Parameters.AddWithValue("PRetainer", oPRetainer);
											updcmd.Parameters.AddWithValue("SRetainer", oSRetainer);
											updcmd.Parameters.AddWithValue("POthers", oPOthers);
											updcmd.Parameters.AddWithValue("SOthers", oSOthers);
											updcmd.Parameters.AddWithValue("Food", oFood);
											updcmd.Parameters.AddWithValue("LightWater", oLightWater);
											updcmd.Parameters.AddWithValue("TelBill", oTelBill);
											updcmd.Parameters.AddWithValue("Transportation", oTransportation);
											updcmd.Parameters.AddWithValue("Rent", oRent);
											updcmd.Parameters.AddWithValue("Education", oEducation);
											updcmd.Parameters.AddWithValue("LoanAmort", oLoanAmort);
											updcmd.Parameters.AddWithValue("MEOthers", oMEOthers);
											updcmd.Parameters.AddWithValue("CreateUserID", UserID);
											updcmd.Parameters.AddWithValue("AddtlCharges", AddtlCharges);
											updcmd.Parameters.AddWithValue("Bank", Bank);
											updcmd.Parameters.AddWithValue("Allowed", allowed);
											updcmd.Parameters.AddWithValue("TermsMonth", termsMonths);
											updcmd.Parameters.AddWithValue("DiscBase", discbase);
											updcmd.Parameters.AddWithValue("SpotDueDate", SpotDueDate);
											updcmd.Parameters.AddWithValue("TransType", "U");
											updcmd.Parameters.AddWithValue("NetDas2", NetDas2);
											updcmd.Parameters.AddWithValue("TCPAddCharges", tcpAddCharges); //86
																											//updcmd.Parameters.AddWithValue("ACDueDate", ACDueDate.ToString("MM/dd/yyyy"));
											updcmd.Parameters.AddWithValue("ACDueDate", ACDueDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("LBMonthly", LBMonthly);
											//updcmd.Parameters.AddWithValue("selDueDate", selDueDate);
											using (HanaDataAdapter da = new HanaDataAdapter(updcmd))
											{
												da.Fill(dt);
												TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
											}
										}
										hana.Execute($@"CALL sp_SQModify ('U','{Code}','{DocEntry}')", hana.GetConnection("SAOHana"));
									}

									LastActivityDate(UserID);
								}
								else
								{ hana.Execute($@"CALL sp_SQModify ('D','{Code}','')", hana.GetConnection("SAOHana")); }

								ret = GetErr(dtRet);

							}
						}
					}
				}
				catch (Exception ex)
				{ ret = ex.Message; }
				return ret;
			}
		}



		//########## [ QUOTATION NEW POSTING ] ###########//
		[WebMethod]
		public string QuotationNew(int oTab,
								int DocEntry,  //2
								string oCardCode,
								DateTime oDate, //4
								string oDocStatus,
								string ProjCode, //6

								string Block,
								string Lot, //8
								string Model,
								string FinancingScheme,  //10
								string LotArea,
								string FloorArea,  //12
								string ProductStatus,
								string Phase, //14 
								string LotClassification,
								string ProductType, //16 
								string LoanType,
								string Bank, //18 

								double OTcp,
								double ResrvFee, //20
								double DPPercent,
								double DPAmount, //22
								int DPTerms,

								double DiscPercent, //24 //discountPercent
								double DiscAmount,
								int LTerms, //26
								double InterestRate,
								DateTime LDueDate, //28
								double Gdi,

								double Tcp,  //30
								double OMisc,
								double GrossTCP, // 32
								double Vat,
								double NetTcp,  //34
								double TCPDownpayment,
								double MonthlyDP, //36
								DateTime DPDueDate,
								double LAmount,  //38
								int UserID,
								string EmployeeID,  //40  
								string EmployeeName,
								string EmployeePosition, //42

								DataTable oDownpayment,
								DataTable oAmort,  //44
								string DocNum,

								double PDMonthly, //46
								double TCPMonthly,
								double MiscFeesMonthly, //48
								double AddMiscFees,
								double DPBalanceOnEquity, //50 
								double TCPBalanceOnEquity,
								double PDMisc, //52
								double PDLoanableBalance,
								double TCPLoanableBalance, //54
								string Remarks,
								string RetitlingType, //56
								double CSTotal,
								string SoldWithAdjacentLot, //58
								string AdjacentLotQuotationNo,

								//RELEATED TO MISCELLANEOUS
								DateTime MiscDueDate,
								string MiscFinancingScheme,
								string MiscDPTerms,
								DataTable oDPMisc,
								double MiscDPAmount,
								int MiscLBTerms,
								double MiscLBAmount,
								double MiscLBMonthly,
								DataTable oLBMisc,

								//string TaxClassification,
								string LetterReqDocument = "", //60
								string LTSNo = "",
								string PaymentScheme = "", //62
								string Vatable = "",
								double MonthlyLB = 0, //64
								string Incentive = "N",
								DataTable oCoOwners = null,
								string Comaker = ""
								)
		{
			{
				string ret;
				try
				{
					string TransType;
					string query = "sp_QuotationNew";
					string Code = GetAutoKey(2, "C") + UserID;
					//InsertSQTablesNew(Code, oDownpayment, oAmort, oAddCharge, EmpList, EmployeeID, EmployeeName, EmployeePosition);
					InsertSQTablesNew(Code, oDownpayment, oAmort, oDPMisc, oLBMisc, EmployeeID, EmployeeName, EmployeePosition, oDate, ResrvFee, oCoOwners, UserID, ProjCode, ProductType);
					using (var dt = new DataTable())
					{
						using (var con = new HanaConnection(hana.GetConnection("SAOHana")))
						{
							using (var cmd = new HanaCommand(query, con))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("ExistDocEntry", DocEntry);
								cmd.Parameters.AddWithValue("DocEntry", Code);  //2
								cmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D"));
								cmd.Parameters.AddWithValue("CardCode", oCardCode); //4
								cmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd"));
								cmd.Parameters.AddWithValue("DocStatus", oDocStatus); //6

								cmd.Parameters.AddWithValue("ProjCode", ProjCode);
								cmd.Parameters.AddWithValue("Block", Block); //8
								cmd.Parameters.AddWithValue("Lot", Lot);
								cmd.Parameters.AddWithValue("Model", Model);  //10
								cmd.Parameters.AddWithValue("FinancingScheme", FinancingScheme);
								cmd.Parameters.AddWithValue("LotArea", LotArea); //12
								cmd.Parameters.AddWithValue("FloorArea", FloorArea);
								cmd.Parameters.AddWithValue("ProductStatus", ProductStatus); //14
								cmd.Parameters.AddWithValue("Phase", Phase);
								cmd.Parameters.AddWithValue("LotClassification", LotClassification);  //16
								cmd.Parameters.AddWithValue("ProductType", ProductType);
								cmd.Parameters.AddWithValue("LoanType", LoanType);  //18
								cmd.Parameters.AddWithValue("Bank", Bank);

								cmd.Parameters.AddWithValue("OTcp", OTcp); //20
								cmd.Parameters.AddWithValue("ResrvFee", ResrvFee);
								cmd.Parameters.AddWithValue("DPPercent", DPPercent); //22
								cmd.Parameters.AddWithValue("DPAmount", DPAmount);
								cmd.Parameters.AddWithValue("DPTerms", DPTerms); //24
								cmd.Parameters.AddWithValue("DiscPercent", DiscPercent);  //--DISC PERCENT
								cmd.Parameters.AddWithValue("DiscAmount", DiscAmount);  //26
								cmd.Parameters.AddWithValue("LTerms", LTerms);
								cmd.Parameters.AddWithValue("InterestRate", InterestRate);  //28
								cmd.Parameters.AddWithValue("LDueDate", LDueDate.ToString("yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("Gdi", Gdi);  //30
								cmd.Parameters.AddWithValue("TransType", "A");

								cmd.Parameters.AddWithValue("Tcp", Tcp); //32
								cmd.Parameters.AddWithValue("OMisc", OMisc);
								cmd.Parameters.AddWithValue("GrossTCP", GrossTCP); //34
								cmd.Parameters.AddWithValue("Vat", Vat);
								cmd.Parameters.AddWithValue("NetTcp", NetTcp);  //36
								cmd.Parameters.AddWithValue("TCPDownpayment", TCPDownpayment);
								cmd.Parameters.AddWithValue("MonthlyDP", MonthlyDP); //38
								cmd.Parameters.AddWithValue("DPDueDate", DPDueDate.ToString("yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("LAmount", LAmount); //40

								cmd.Parameters.AddWithValue("UserID", UserID);
								cmd.Parameters.AddWithValue("LetterReqDocument", LetterReqDocument); //42

								cmd.Parameters.AddWithValue("PDMonthly", PDMonthly);
								cmd.Parameters.AddWithValue("TCPMonthly", TCPMonthly); //44
								cmd.Parameters.AddWithValue("MiscFeesMonthly", MiscFeesMonthly);
								cmd.Parameters.AddWithValue("AddMiscFees", AddMiscFees); //46
								cmd.Parameters.AddWithValue("DPBalanceOnEquity", DPBalanceOnEquity);
								cmd.Parameters.AddWithValue("TCPBalanceOnEquity", TCPBalanceOnEquity); //48
								cmd.Parameters.AddWithValue("PDMisc", PDMisc);
								cmd.Parameters.AddWithValue("PDLoanableBalance", PDLoanableBalance); //50
								cmd.Parameters.AddWithValue("TCPLoanableBalance", TCPLoanableBalance);
								cmd.Parameters.AddWithValue("Remarks", Remarks); //52
								cmd.Parameters.AddWithValue("RetitlingType", RetitlingType);
								cmd.Parameters.AddWithValue("CSTotal", CSTotal); //54
								cmd.Parameters.AddWithValue("SoldWithAdjacentLot", SoldWithAdjacentLot);
								cmd.Parameters.AddWithValue("AdjacentLotQuotationNo", Convert.ToInt32(AdjacentLotQuotationNo == "" ? null : AdjacentLotQuotationNo)); //56

								cmd.Parameters.AddWithValue("MiscDueDate", MiscDueDate.ToString("yyyy/MM/dd"));
								cmd.Parameters.AddWithValue("MiscFinancingScheme", MiscFinancingScheme);
								cmd.Parameters.AddWithValue("MiscDPTerms", MiscDPTerms);

								cmd.Parameters.AddWithValue("MiscDPAmount", MiscDPAmount);
								cmd.Parameters.AddWithValue("MiscLBTerms", MiscLBTerms);
								cmd.Parameters.AddWithValue("MiscLBAmount", MiscLBAmount);
								cmd.Parameters.AddWithValue("MiscLBMonthly", MiscLBMonthly);



								cmd.Parameters.AddWithValue("LTSNo", LTSNo);
								cmd.Parameters.AddWithValue("PaymentScheme", PaymentScheme); //58
								cmd.Parameters.AddWithValue("Vatable", Vatable);
								cmd.Parameters.AddWithValue("MonthlyLB", MonthlyLB); //60
								cmd.Parameters.AddWithValue("Incentive", Incentive);
								cmd.Parameters.AddWithValue("Comaker", Comaker);
								//cmd.Parameters.AddWithValue("TaxClassification", TaxClassification);

								using (var da = new HanaDataAdapter(cmd))
								{
									da.Fill(dt);
									TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
								}

								DataTable dtRet = new DataTable();
								dtRet = hana.GetData($@"CALL sp_TransactionNotification ('2','{TransType}','{Code}')", hana.GetConnection("SAOHana"));

								int err = Convert.ToInt32(DataAccess.GetData(dtRet, 0, "Column1", "0"));

								/**
                                 * Insert data when:
                                 * 1. wizard tab = 4
                                 * 2. error = 0
                                 */
								if (err == 0 && oTab == 4)
								{
									if (DocEntry == 0)
									{
										hana.Execute($@"CALL sp_SQModify ('U','{Code}','{GetAutoKey(2, "G")}')", hana.GetConnection("SAOHana"));
										GetAutoKey(2, "S");
									}
									else
									{
										using (HanaCommand updcmd = new HanaCommand(query, con))
										{
											updcmd.CommandType = CommandType.StoredProcedure;
											updcmd.Parameters.AddWithValue("ExistDocEntry", DocEntry);
											updcmd.Parameters.AddWithValue("DocEntry", Code);  //2
											updcmd.Parameters.AddWithValue("DocNum", DocNum);
											//updcmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D"));
											updcmd.Parameters.AddWithValue("CardCode", oCardCode); //4
											updcmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("DocStatus", oDocStatus); //6

											updcmd.Parameters.AddWithValue("ProjCode", ProjCode);
											updcmd.Parameters.AddWithValue("Block", Block); //8
											updcmd.Parameters.AddWithValue("Lot", Lot);
											updcmd.Parameters.AddWithValue("Model", Model); //10
											updcmd.Parameters.AddWithValue("FinancingScheme", FinancingScheme);
											updcmd.Parameters.AddWithValue("LotArea", LotArea); //12
											updcmd.Parameters.AddWithValue("FloorArea", FloorArea);
											updcmd.Parameters.AddWithValue("ProductStatus", ProductStatus); //14
											updcmd.Parameters.AddWithValue("Phase", Phase);
											updcmd.Parameters.AddWithValue("LotClassification", LotClassification); //16
											updcmd.Parameters.AddWithValue("ProductType", ProductType);
											updcmd.Parameters.AddWithValue("LoanType", LoanType); //18
											updcmd.Parameters.AddWithValue("Bank", Bank);

											updcmd.Parameters.AddWithValue("OTcp", OTcp); //20
											updcmd.Parameters.AddWithValue("ResrvFee", ResrvFee);
											updcmd.Parameters.AddWithValue("DPPercent", DPPercent); //22
											updcmd.Parameters.AddWithValue("DPAmount", DPAmount);
											updcmd.Parameters.AddWithValue("DPTerms", DPTerms); //24
											updcmd.Parameters.AddWithValue("DiscPercent", DiscPercent); // --DISC PERCENT
											updcmd.Parameters.AddWithValue("DiscAmount", DiscAmount); //26
											updcmd.Parameters.AddWithValue("LTerms", LTerms);
											updcmd.Parameters.AddWithValue("InterestRate", InterestRate); //28
											updcmd.Parameters.AddWithValue("LDueDate", LDueDate.ToString("yyyy-MM-dd"));
											updcmd.Parameters.AddWithValue("Gdi", Gdi); //30
											updcmd.Parameters.AddWithValue("TransType", TransType);

											updcmd.Parameters.AddWithValue("Tcp", Tcp); //32
											updcmd.Parameters.AddWithValue("OMisc", OMisc);
											updcmd.Parameters.AddWithValue("GrossTCP", GrossTCP); //34
											updcmd.Parameters.AddWithValue("Vat", Vat);
											updcmd.Parameters.AddWithValue("NetTcp", NetTcp); //36
											updcmd.Parameters.AddWithValue("TCPDownpayment", TCPDownpayment);
											updcmd.Parameters.AddWithValue("MonthlyDP", MonthlyDP); //38
											updcmd.Parameters.AddWithValue("DPDueDate", DPDueDate.ToString("yyyy-MM-dd"));
											updcmd.Parameters.AddWithValue("LAmount", LAmount); //40

											updcmd.Parameters.AddWithValue("UserID", UserID);
											updcmd.Parameters.AddWithValue("LetterReqDocument", LetterReqDocument);  //42

											updcmd.Parameters.AddWithValue("PDMonthly", PDMonthly);
											updcmd.Parameters.AddWithValue("TCPMonthly", TCPMonthly); //44
											updcmd.Parameters.AddWithValue("MiscFeesMonthly", MiscFeesMonthly);
											updcmd.Parameters.AddWithValue("AddMiscFees", AddMiscFees); //46
											updcmd.Parameters.AddWithValue("DPBalanceOnEquity", DPBalanceOnEquity);
											updcmd.Parameters.AddWithValue("TCPBalanceOnEquity", TCPBalanceOnEquity);//48
											updcmd.Parameters.AddWithValue("PDMisc", PDMisc);
											updcmd.Parameters.AddWithValue("PDLoanableBalance", PDLoanableBalance);//50
											updcmd.Parameters.AddWithValue("TCPLoanableBalance", TCPLoanableBalance);
											updcmd.Parameters.AddWithValue("Remarks", Remarks);//52
											updcmd.Parameters.AddWithValue("RetitlingType", RetitlingType);
											updcmd.Parameters.AddWithValue("CSTotal", CSTotal); //54
																								//updcmd.Parameters.AddWithValue("TaxClassification", TaxClassification);
											updcmd.Parameters.AddWithValue("SoldWithAdjacentLot", SoldWithAdjacentLot);
											updcmd.Parameters.AddWithValue("AdjacentLotQuotationNo", Convert.ToInt32(AdjacentLotQuotationNo == "" ? null : AdjacentLotQuotationNo));//56

											updcmd.Parameters.AddWithValue("MiscDueDate", MiscDueDate.ToString("yyyy/MM/dd"));
											updcmd.Parameters.AddWithValue("MiscFinancingScheme", MiscFinancingScheme);
											updcmd.Parameters.AddWithValue("MiscDPTerms", MiscDPTerms);


											updcmd.Parameters.AddWithValue("MiscDPAmount", MiscDPAmount);
											updcmd.Parameters.AddWithValue("MiscLBTerms", MiscLBTerms);
											updcmd.Parameters.AddWithValue("MiscLBAmount", MiscLBAmount);
											updcmd.Parameters.AddWithValue("MiscLBMonthly", MiscLBMonthly);

											updcmd.Parameters.AddWithValue("LTSNo", LTSNo);
											updcmd.Parameters.AddWithValue("PaymentScheme", PaymentScheme);
											updcmd.Parameters.AddWithValue("Vatable", Vatable);
											updcmd.Parameters.AddWithValue("MonthlyLB", MonthlyLB);
											updcmd.Parameters.AddWithValue("Incentive", Incentive);
											updcmd.Parameters.AddWithValue("Comaker", Comaker);

											using (HanaDataAdapter da = new HanaDataAdapter(updcmd))
											{
												da.Fill(dt);
												TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
											}
										}
										hana.Execute($@"CALL sp_SQModify ('U','{Code}','{DocEntry}')", hana.GetConnection("SAOHana"));
									}

									LastActivityDate(UserID);
								}
								else
								{ hana.Execute($@"CALL sp_SQModify ('D','{Code}','')", hana.GetConnection("SAOHana")); }

								ret = GetErr(dtRet);

							}
						}
					}
				}
				catch (Exception ex)
				{ ret = ex.Message; }
				return ret;
			}
		}



		void InsertSQTables(string oDocEntry, DataTable dtDownpayment, DataTable dtAmorth, DataTable dtAddCharges, GridView gr)
		{
			try
			{
				if (dtDownpayment != null)
				{
					foreach (DataRow dr in dtDownpayment.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]), Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestRate"]), Convert.ToDouble(dr["Principal"]), Convert.ToDouble(dr["UnAll"]), Convert.ToDouble(dr["Balance"]), "DP");
					}
				}

				if (dtAddCharges != null)
				{
					foreach (DataRow dr in dtAddCharges.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);
						string principal = string.IsNullOrWhiteSpace((dr["Principal"]).ToString()) ? "0" : (dr["Principal"]).ToString();
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]), Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestRate"]), Convert.ToDouble(principal), Convert.ToDouble(dr["UnAll"]), Convert.ToDouble(dr["Balance"]), "AC");
					}
				}

				if (dtAmorth != null)
				{
					foreach (DataRow dr in dtAmorth.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);//Convert.ToDateTime($"{oConvert.Substring(6, 4)}-{oConvert.Substring(0, 2)}-{oConvert.Substring(3, 2)}");
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]), Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestRate"]), Convert.ToDouble(dr["Principal"]), Convert.ToDouble(dr["UnAll"]), Convert.ToDouble(dr["Balance"]), "MA");
					}
				}

				if (gr != null)
				{
					if (gr.Rows.Count > 0)
					{
						foreach (GridViewRow row in gr.Rows)
						{
							Label lblName = (Label)row.FindControl("lblSalesName");
							Label lblCode = (Label)row.FindControl("lblSalesCode");
							string POSCode = row.Cells[0].Text;
							if (lblCode.Text != "" || !string.IsNullOrEmpty(lblCode.Text))
							{ SetListOfEmployees(oDocEntry, POSCode, lblCode.Text, lblName.Text); }
						}
					}
				}

			}
			catch (Exception ex)
			{
				hana.Execute($@"CALL sp_SQModify ('D','{oDocEntry}','')", hana.GetConnection("SAOHana"));
			}
		}


		//########## [ QUOTATION NEW POSTING ] ###########//
		[WebMethod]
		public string SampleQuotation(int oTab,
								int DocEntry,  //2
								string oCardCode,
								DateTime oDate, //4
								string oDocStatus,
								string ProjCode, //6


								string Block,
								string Lot, //8
								string Model,
								string FinancingScheme,  //10
								string LotArea,
								string FloorArea,  //12
								string ProductStatus,
								string Phase, //14 
								string LotClassification,
								string ProductType, //16 
								string LoanType,
								string Bank, //18 

								double OTcp,
								double ResrvFee, //20
								double DPPercent,
								double DPAmount, //22
								int DPTerms,

								double DiscPercent, //24 //discountPercent
								double DiscAmount,
								int LTerms, //26
								double InterestRate,
								DateTime LDueDate, //28
								double Gdi,

								double Tcp,  //30
								double OMisc,
								double GrossTCP, // 32
								double Vat,
								double NetTcp,  //34
								double TCPDownpayment,
								double MonthlyDP, //36
								DateTime DPDueDate,
								double LAmount,  //38
								int UserID,
								string EmployeeID,  //40  
								string EmployeeName,
								string EmployeePosition, //42

								DataTable oDownpayment,
								DataTable oAmort,  //44
								string DocNum,

								double PDMonthly, //46
								double TCPMonthly,
								double MiscFeesMonthly, //48
								double AddMiscFees,
								double DPBalanceOnEquity, //50 
								double TCPBalanceOnEquity,
								double PDMisc, //52
								double PDLoanableBalance,
								double TCPLoanableBalance, //54
								string Remarks,
								string RetitlingType, //56
								double CSTotal,
								string SoldWithAdjacentLot, //58
								string AdjacentLotQuotationNo,

								//RELEATED TO MISCELLANEOUS
								DateTime MiscDueDate,
								string MiscFinancingScheme,
								string MiscDPTerms,
								DataTable oDPMisc,
								double MiscDPAmount,
								int MiscLBTerms,
								double MiscLBAmount,
								double MiscLBMonthly,
								DataTable oLBMisc,

								//string TaxClassification,
								string LetterReqDocument = "", //60
								string LTSNo = "",
								string PaymentScheme = "", //62
								string Vatable = "",
								double MonthlyLB = 0, //64
								string Incentive = "N",
								DataTable oCoOwners = null,
								string Comaker = ""
								)
		{
			{
				string ret;
				try
				{
					string TransType;
					string query = "sp_SampleQuotation";
					using (var dt = new DataTable())
					{
						using (var con = new HanaConnection(hana.GetConnection("SAOHana")))
						{
							using (var cmd = new HanaCommand(query, con))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("ExistDocEntry", DocEntry);
								cmd.Parameters.AddWithValue("DocEntry", DocEntry);  //2
																					//cmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D"));
								cmd.Parameters.AddWithValue("DocNum", DocNum);
								cmd.Parameters.AddWithValue("CardCode", oCardCode); //4
								cmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd"));
								cmd.Parameters.AddWithValue("DocStatus", oDocStatus); //6

								cmd.Parameters.AddWithValue("ProjCode", ProjCode);
								cmd.Parameters.AddWithValue("Block", Block); //8
								cmd.Parameters.AddWithValue("Lot", Lot);
								cmd.Parameters.AddWithValue("Model", Model);  //10
								cmd.Parameters.AddWithValue("FinancingScheme", FinancingScheme);
								cmd.Parameters.AddWithValue("LotArea", LotArea); //12
								cmd.Parameters.AddWithValue("FloorArea", FloorArea);
								cmd.Parameters.AddWithValue("ProductStatus", ProductStatus); //14
								cmd.Parameters.AddWithValue("Phase", Phase);
								cmd.Parameters.AddWithValue("LotClassification", LotClassification);  //16
								cmd.Parameters.AddWithValue("ProductType", ProductType);
								cmd.Parameters.AddWithValue("LoanType", LoanType);  //18
								cmd.Parameters.AddWithValue("Bank", Bank);

								cmd.Parameters.AddWithValue("OTcp", OTcp); //20
								cmd.Parameters.AddWithValue("ResrvFee", ResrvFee);
								cmd.Parameters.AddWithValue("DPPercent", DPPercent); //22
								cmd.Parameters.AddWithValue("DPAmount", DPAmount);
								cmd.Parameters.AddWithValue("DPTerms", DPTerms); //24
								cmd.Parameters.AddWithValue("DiscPercent", DiscPercent);  //--DISC PERCENT
								cmd.Parameters.AddWithValue("DiscAmount", DiscAmount);  //26
								cmd.Parameters.AddWithValue("LTerms", LTerms);
								cmd.Parameters.AddWithValue("InterestRate", InterestRate);  //28
								cmd.Parameters.AddWithValue("LDueDate", LDueDate.ToString(" yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("Gdi", Gdi);  //30
								cmd.Parameters.AddWithValue("TransType", "A");

								cmd.Parameters.AddWithValue("Tcp", Tcp); //32
								cmd.Parameters.AddWithValue("OMisc", OMisc);
								cmd.Parameters.AddWithValue("GrossTCP", GrossTCP); //34
								cmd.Parameters.AddWithValue("Vat", Vat);
								cmd.Parameters.AddWithValue("NetTcp", NetTcp);  //36
								cmd.Parameters.AddWithValue("TCPDownpayment", TCPDownpayment);
								cmd.Parameters.AddWithValue("MonthlyDP", MonthlyDP); //38
								cmd.Parameters.AddWithValue("DPDueDate", DPDueDate.ToString("yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("LAmount", LAmount); //40

								cmd.Parameters.AddWithValue("UserID", UserID);
								cmd.Parameters.AddWithValue("LetterReqDocument", LetterReqDocument); //42

								cmd.Parameters.AddWithValue("PDMonthly", PDMonthly);
								cmd.Parameters.AddWithValue("TCPMonthly", TCPMonthly); //44
								cmd.Parameters.AddWithValue("MiscFeesMonthly", MiscFeesMonthly);
								cmd.Parameters.AddWithValue("AddMiscFees", AddMiscFees); //46
								cmd.Parameters.AddWithValue("DPBalanceOnEquity", DPBalanceOnEquity);
								cmd.Parameters.AddWithValue("TCPBalanceOnEquity", TCPBalanceOnEquity); //48
								cmd.Parameters.AddWithValue("PDMisc", PDMisc);
								cmd.Parameters.AddWithValue("PDLoanableBalance", PDLoanableBalance); //50
								cmd.Parameters.AddWithValue("TCPLoanableBalance", TCPLoanableBalance);
								cmd.Parameters.AddWithValue("Remarks", Remarks); //52
								cmd.Parameters.AddWithValue("RetitlingType", RetitlingType);
								cmd.Parameters.AddWithValue("CSTotal", CSTotal); //54
								cmd.Parameters.AddWithValue("SoldWithAdjacentLot", SoldWithAdjacentLot);
								cmd.Parameters.AddWithValue("AdjacentLotQuotationNo", Convert.ToInt32(AdjacentLotQuotationNo == "" ? null : AdjacentLotQuotationNo)); //56

								cmd.Parameters.AddWithValue("MiscDueDate", MiscDueDate.ToString("yyyy/MM/dd"));
								cmd.Parameters.AddWithValue("MiscFinancingScheme", MiscFinancingScheme);
								cmd.Parameters.AddWithValue("MiscDPTerms", MiscDPTerms);

								cmd.Parameters.AddWithValue("MiscDPAmount", MiscDPAmount);
								cmd.Parameters.AddWithValue("MiscLBTerms", MiscLBTerms);
								cmd.Parameters.AddWithValue("MiscLBAmount", MiscLBAmount);
								cmd.Parameters.AddWithValue("MiscLBMonthly", MiscLBMonthly);



								cmd.Parameters.AddWithValue("LTSNo", LTSNo);
								cmd.Parameters.AddWithValue("PaymentScheme", PaymentScheme); //58
								cmd.Parameters.AddWithValue("Vatable", Vatable);
								cmd.Parameters.AddWithValue("MonthlyLB", MonthlyLB); //60
								cmd.Parameters.AddWithValue("Incentive", Incentive);
								cmd.Parameters.AddWithValue("Comaker", Comaker);
								using (var da = new HanaDataAdapter(cmd))
								{
									da.Fill(dt);
									TransType = (string)DataAccess.GetData(dt, 0, "TransType", "");
								}
								ret = "Operation completed successfully.";

							}
						}
					}
				}
				catch (Exception ex)
				{ ret = ex.Message; }
				return ret;
			}
		}



		void InsertSQTablesNew(string oDocEntry, DataTable dtDownpayment,
							   DataTable dtAmorth, DataTable dtDPMisc,
							   DataTable dtLBMisc, string EmployeeID,
							   string EmployeeName, string EmployeePosition,
							   DateTime ResDate, double ResrvFee,
							   DataTable dtCoOwners, int UserID,
							   string ProjCode, string ProductType)
		{
			try
			{

				//FOR RESERVATION
				SQPayment(oDocEntry,
						  1, //2
						  ResDate,
						  ResrvFee, //4
						  0,
						  0, //6
						  0,
						  ResrvFee, //8
						  0,
						  0, //10
						  "RES");


				if (dtDownpayment != null)
				{
					foreach (DataRow dr in dtDownpayment.Rows)
					{
						if (!(Convert.ToDouble(dr["Principal"]) <= 0))
						{
							string oConvert = dr["DueDate"].ToString();
							DateTime oDueDate = Convert.ToDateTime(oConvert);
							SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]),
								Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestAmount"]), Convert.ToDouble(dr["Principal"]), 0,
								Convert.ToDouble(dr["Balance"]), dr["PaymentType"].ToString());
						}
					}
				}

				//if (dtAddCharges != null)
				//{
				//    foreach (DataRow dr in dtAddCharges.Rows)
				//    {
				//        string oConvert = dr["DueDate"].ToString();
				//        DateTime oDueDate = Convert.ToDateTime(oConvert);
				//        string principal = string.IsNullOrWhiteSpace((dr["Principal"]).ToString()) ? "0" : (dr["Principal"]).ToString();
				//        SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]), Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestRate"]), Convert.ToDouble(principal), Convert.ToDouble(dr["UnAll"]), Convert.ToDouble(dr["Balance"]), "AC");
				//    }
				//}

				if (dtAmorth != null)
				{
					foreach (DataRow dr in dtAmorth.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);//Convert.ToDateTime($"{oConvert.Substring(6, 4)}-{oConvert.Substring(0, 2)}-{oConvert.Substring(3, 2)}");
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]),
							Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestAmount"]), Convert.ToDouble(dr["Principal"]), 0,
							Convert.ToDouble(dr["Balance"]), "LB");
					}
				}

				if (dtDPMisc != null)
				{
					foreach (DataRow dr in dtDPMisc.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);//Convert.ToDateTime($"{oConvert.Substring(6, 4)}-{oConvert.Substring(0, 2)}-{oConvert.Substring(3, 2)}");
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]),
							Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestAmount"]), Convert.ToDouble(dr["Principal"]), 0,
							Convert.ToDouble(dr["Balance"]), "MISC", "DP");
					}
				}


				if (dtLBMisc != null)
				{
					foreach (DataRow dr in dtLBMisc.Rows)
					{
						string oConvert = dr["DueDate"].ToString();
						DateTime oDueDate = Convert.ToDateTime(oConvert);//Convert.ToDateTime($"{oConvert.Substring(6, 4)}-{oConvert.Substring(0, 2)}-{oConvert.Substring(3, 2)}");
						SQPayment(oDocEntry, Convert.ToInt16(dr["Terms"]), oDueDate, Convert.ToDouble(dr["PaymentAmount"]), Convert.ToDouble(dr["Penalty"]),
							Convert.ToDouble(dr["Misc"]), Convert.ToDouble(dr["InterestAmount"]), Convert.ToDouble(dr["Principal"]), 0,
							Convert.ToDouble(dr["Balance"]), "MISC", "LB");
					}
				}

				//if (gr != null)
				//{
				//    if (gr.Rows.Count > 0)
				//    {
				//        foreach (GridViewRow row in gr.Rows)
				//        {
				//            Label lblName = (Label)row.FindControl("lblSalesName");
				//            Label lblCode = (Label)row.FindControl("lblSalesCode");
				//            string POSCode = row.Cells[0].Text;
				//            if (lblCode.Text != "" || !string.IsNullOrEmpty(lblCode.Text))
				//{
				SetListOfEmployees(oDocEntry, EmployeePosition, EmployeeID, EmployeeName);
				//}
				//        }
				//    }
				//}


				if (dtCoOwners != null)
				{
					foreach (DataRow dr in dtCoOwners.Rows)
					{
						string CardCode = dr["Code"].ToString();
						string Name = dr["Name"].ToString();
						SQCoOwners(oDocEntry, CardCode, Name, UserID);
					}
				}




				string type1 = "";

				if (ProductType.ToUpper().Contains("HOUSE"))
				{
					type1 = "HouseNLot";
				}
				else
				{
					type1 = "Lot";
				}

				//SAVING OF INCENTIVE  
				//2023-05-04 : ADDED PROJECT AND PRODUCT TYPE ON PARAMETERS
				DataTable dtIncentiveAgents = GetIncentiveAgents(EmployeeID, ProjCode, ProductType).Tables[0];

				if (dtIncentiveAgents != null)
				{
					foreach (DataRow dr in dtIncentiveAgents.Rows)
					{
						sp_SaveIncentiveCommissioninformation(oDocEntry, dr["Position"].ToString(), dr["SAPCardCode"].ToString(), dr["SalesPerson"].ToString(), UserID, dr["Status"].ToString(), dr["Id"].ToString(),
							dr["Percentage"].ToString(), dr["HouseAndLotPercentage"].ToString());
					}
				}




				// 05-05-2023 : SAVING OF INCENTIVE SCHEME FROM SAP
				string qryIncentiveScheme = $@" SELECT A.""Code"" ""IncentiveCode"", A.""U_Type"" ""Type"", A.""U_Project"" ""ProjCode"",
                                                IFNULL(A.""U_IncentiveAmount"",0) ""IncentiveAmount"",	IFNULL(B.""U_Position"",'') ""Position"",	
                                                IFNULL(B.""U_Release"",0) ""Release"",	IFNULL(B.""U_Amount"",0) ""Amount"" ,
                                                IFNULL(B.""U_EffDF"",'1999-01-01') ""EffectivityDateFrom"" ,	IFNULL(B.""U_EffDT"",'2100-12-31') ""EffectivityDateTo"" 	
                                                FROM ""@COMMINCSCHEME"" A INNER JOIN ""@INCENTIVE"" B ON A.""Code"" = B.""Code""  
                                                WHERE  A.""U_Project"" = '{ProjCode}' AND UPPER(A.""U_Type"") = UPPER('{ProductType}')";
				DataTable dtIncentiveScheme = hana.GetData(qryIncentiveScheme, hana.GetConnection("SAPHana"));
				if (dtIncentiveScheme != null)
				{
					foreach (DataRow dr in dtIncentiveScheme.Rows)
					{
						//2023-05-05 : SAVING OF INCENTIVE SCHEME TO QUT15
						sp_SaveIncentiveScheme(oDocEntry, dr["IncentiveCode"].ToString(),

												dr["Type"].ToString(), dr["ProjCode"].ToString(),
												double.Parse(dr["IncentiveAmount"].ToString()), dr["Position"].ToString(),
												int.Parse(dr["Release"].ToString()), double.Parse(dr["Amount"].ToString()),
												Convert.ToDateTime(dr["EffectivityDateFrom"]).ToString("yyyy-MM-dd"), Convert.ToDateTime(dr["EffectivityDateTo"]).ToString("yyyy-MM-dd"),
												UserID, DateTime.Now.ToString("yyyy-MM-dd")
												);
					}
				}



				// 04-20-2023 : SAVING OF COMMISSION SCHEME FROM SAP 
				string qryCommissionScheme = $@" SELECT A.""Code"" ""CommissionCode"", A.""U_Type"" ""Type"", A.""U_Project"" ""ProjCode"",
                                                IFNULL(B.""U_CollectedTCP"",0) ""CollectedTCP"", IFNULL(B.""U_Release"",0) ""Release"",	
                                                IFNULL(B.""U_CommissionRelease"",0) ""CommissionRelease"",
                                                IFNULL(A.""U_Commission"",0) ""CommissionPercent"" 
                                                FROM ""@COMMINCSCHEME"" A INNER JOIN	""@COMMISSION"" B ON A.""Code"" = B.""Code""  
                                                 WHERE  A.""U_Project"" = '{ProjCode}' AND UPPER(A.""U_Type"") = UPPER('{ProductType}')";
				DataTable dtCommissionScheme = hana.GetData(qryCommissionScheme, hana.GetConnection("SAPHana"));
				if (dtCommissionScheme != null)
				{
					foreach (DataRow dr in dtCommissionScheme.Rows)
					{
						//2023-05-04 : SAVING OF COMMISSION SCHEME TO QUT14
						sp_SaveCommissionScheme(oDocEntry, dr["CommissionCode"].ToString(),

												dr["Type"].ToString(), dr["ProjCode"].ToString(),
												double.Parse(dr["CollectedTCP"].ToString()), int.Parse(dr["Release"].ToString()),
												double.Parse(dr["CommissionRelease"].ToString()), double.Parse(dr["CommissionPercent"].ToString()),
												UserID, DateTime.Now.ToString("yyyy-MM-dd")
												);
					}
				}









			}
			catch (Exception ex)
			{
				hana.Execute($@"CALL sp_SQModify ('D','{oDocEntry}','')", hana.GetConnection("SAOHana"));
			}
		}


		void SQPayment(string oDocEntry,
					   int oTerms,
					   DateTime oDueDate,
					   double oPaymentAmount,
					   double oPenalty,
					   double oMisc,
					   double oInterestRate,
					   double oPrincipal,
					   double oUnAll,
					   double oBalance,
					   string oPaymentType,
					   string MiscType = "")
		{
			try
			{
				string query = "sp_SQPayment";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("Terms", oTerms);
						cmd.Parameters.AddWithValue("DueDate", oDueDate);
						cmd.Parameters.AddWithValue("PaymentAmount", oPaymentAmount);
						cmd.Parameters.AddWithValue("Penalty", oPenalty);
						cmd.Parameters.AddWithValue("Misc", oMisc);
						cmd.Parameters.AddWithValue("InterestRate", oInterestRate);
						cmd.Parameters.AddWithValue("Principal", oPrincipal);
						cmd.Parameters.AddWithValue("UnAll", oUnAll);
						cmd.Parameters.AddWithValue("Balance", oBalance);
						cmd.Parameters.AddWithValue("PaymentType", oPaymentType);
						cmd.Parameters.AddWithValue("MiscType", MiscType);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}



		void SQCoOwners(string oDocEntry,
						string CardCode,
						string Name,
						int CreateUserId
						)
		{
			try
			{
				string query = "sp_SQCoOwners";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("Name", Name);
						cmd.Parameters.AddWithValue("CreateUserID", CreateUserId);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}



		void SetListOfEmployees(string oDocEntry, string oPOSCode, string oEmpCode, string oEmpName)
		{
			try
			{
				string query = "sp_SetListOfEmployees";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("POSCode", oPOSCode);
						cmd.Parameters.AddWithValue("EmpCode", oEmpCode);
						cmd.Parameters.AddWithValue("EmpName", oEmpName);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}



		public void sp_SaveIncentiveCommissioninformation(string oDocEntry, string Position, string SAPCardCode, string SalesPerson, int CreateUserID, string Status,
													string OSLAID, string PercentageDetails, string HouseAndLotPercentage)
		{
			try
			{
				string query = "sp_SaveIncentiveCommissioninformation";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("Position", Position);
						cmd.Parameters.AddWithValue("SAPCardCode", SAPCardCode);
						cmd.Parameters.AddWithValue("SalesPerson", SalesPerson);
						cmd.Parameters.AddWithValue("Status", Status);
						cmd.Parameters.AddWithValue("OSLAID", OSLAID);
						cmd.Parameters.AddWithValue("PercentageDetails", PercentageDetails);
						cmd.Parameters.AddWithValue("HouseAndLotPercentage", HouseAndLotPercentage);
						cmd.Parameters.AddWithValue("CreateUserID", CreateUserID);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}


		public void sp_SaveCommissionScheme(string oDocEntry, string CommissionCode,
											string oType, string ProjCode,
											double CollectedTCP, int Release,
											double CommissionRelease, double CommissionPercent,
											int CreateUserID, string CreateDate)
		{
			try
			{
				string query = "sp_SaveCommissionScheme";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("CommissionCode", CommissionCode);
						cmd.Parameters.AddWithValue("Type", oType);
						cmd.Parameters.AddWithValue("ProjCode", ProjCode);
						cmd.Parameters.AddWithValue("CollectedTCP", CollectedTCP);
						cmd.Parameters.AddWithValue("Release", Release);
						cmd.Parameters.AddWithValue("CommissionRelease", CommissionRelease);
						cmd.Parameters.AddWithValue("CommissionPercent", CommissionPercent);
						cmd.Parameters.AddWithValue("CreateUserID", CreateUserID);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}


		public void sp_SaveIncentiveScheme(string oDocEntry, string CommissionCode,
										  string oType, string ProjCode,
										  double IncentiveAmount, string Position,
										  int Release, double Amount,
										  string EffectivityDateFrom, string EffectivityDateTo,
										  int CreateUserId, string CreateDate)
		{
			try
			{
				string query = "sp_SaveIncentiveScheme";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("IncentiveCode", CommissionCode);
						cmd.Parameters.AddWithValue("Type", oType);
						cmd.Parameters.AddWithValue("ProjCode", ProjCode);

						cmd.Parameters.AddWithValue("IncentiveAmount", IncentiveAmount);
						cmd.Parameters.AddWithValue("Position", Position);
						cmd.Parameters.AddWithValue("Release", Release);
						cmd.Parameters.AddWithValue("Amount", Amount);
						cmd.Parameters.AddWithValue("EffectivityDateFrom", EffectivityDateFrom);
						cmd.Parameters.AddWithValue("EffectivityDateTo", EffectivityDateTo);
						cmd.Parameters.AddWithValue("CreateUserId", CreateUserId);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}





		[WebMethod]
		public void SQDeleteLeads(string CardCode)
		{
			try
			{
				string query = $@"DELETE FROM ""OCRD"" WHERE ""CardCode"" = :CardCode; 
                                  DELETE FROM ""CRD1"" WHERE ""CardCode"" = :CardCode; 
                                  DELETE FROM ""CRD5"" WHERE ""CardCode"" = :CardCode; 
                                  DELETE FROM ""CRD7"" WHERE ""CardCode"" = :CardCode; ";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch { }
		}


		[WebMethod]
		public string LeadBusinessPartner(string LastName, string FirstName, string MiddleName, string CompanyName, DateTime Birthday, string NatureEmp,
									string IDType, string IDNo, int UserID, string BusinessType, string TinNumber, string Comaker, string TaxClassification)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_LeadBusinessPartner", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("CardCode", GetAutoKey(1, "G"));
							cmd.Parameters.AddWithValue("LastName", LastName);
							cmd.Parameters.AddWithValue("FirstName", FirstName);
							cmd.Parameters.AddWithValue("MiddleName", MiddleName);
							cmd.Parameters.AddWithValue("CompanyName", CompanyName);
							cmd.Parameters.AddWithValue("BirthDay", Birthday);
							cmd.Parameters.AddWithValue("Nature", NatureEmp);
							cmd.Parameters.AddWithValue("Comaker", Comaker);
							cmd.Parameters.AddWithValue("IDType", IDType);
							cmd.Parameters.AddWithValue("IDNo", IDNo);
							//if (BusinessType.ToLower().Contains("individual"))
							//{
							//    cmd.Parameters.AddWithValue("IDType", IDType);
							//    cmd.Parameters.AddWithValue("IDNo", IDNo);
							//}
							//else
							//{
							//    cmd.Parameters.AddWithValue("IDType", "TIN");
							//    cmd.Parameters.AddWithValue("IDNo", TinNumber);
							//}
							cmd.Parameters.AddWithValue("UserID", UserID);
							cmd.Parameters.AddWithValue("BusinessType", BusinessType);
							cmd.Parameters.AddWithValue("TaxClassification", TaxClassification);
							cmd.Parameters.AddWithValue("TIN", TinNumber);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "CardCode", "");
								GetAutoKey(1, "S");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		public string GetHouseModelByCode(string Code)
		{
			string ret;
			try
			{
				using (DataTable dt = new DataTable())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("sp_GetHouseModelByCode", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@Code", Code);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Code", "");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetAvailableLot_JSON(string PrjCode, string Block)
		{
			try
			{
				string query = "sp_JSON_AvailableLot";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("Block", Block);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch (Exception ex) { }
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetNewAvailableLot_JSON(string PrjCode)
		{
			try
			{
				string query = "sp_JSON_NewAvailableLot";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch (Exception ex) { }
		}

		#endregion

		#region "SALES ORDER"
		//########## SELECT STD ##########//
		[WebMethod]
		public DataSet GetSQDetails(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				//string query = "sp_GetSQDetails";
				string query = "SELECT A.ItemCode, A.LineNum, A.UseBaseUn,(SELECT ManBtchNum FROM OITM x Where x.ItemCode = A.ItemCode) [ManBtchNum] FROM QUT1 A (NOLOCK) WHERE A.DocEntry = " + Convert.ToString(DocEntry);
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("SAP")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							//cmd.CommandType = CommandType.StoredProcedure;
							////cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database","SAP");
							////cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetSQDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		#endregion

		#region RESERVATION
		[WebMethod]
		public DataSet GetQuotationList()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_QuotationList", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "QuotationList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetRestructuringList()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_RestructuringList", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "QuotationList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet SearchQuotationList(string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_SearchQuotation", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SearchQuotationList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet SearchQuotationListPerProject(string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_SearchQuotationPerProject", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SearchQuotationListPerProject");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet SearchQuotationListPerProjectName(string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_SearchQuotationPerProjectName", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SearchQuotationListPerProject");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet SearchRestructuringList(string Search, int UserID)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_SearchRestructuring", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Search", Search);
						cmd.Parameters.AddWithValue("UserID", UserID);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SearchQuotationList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBanks()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetBanks", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		//ADDED BY KARL 05/03/2021
		[WebMethod]
		public DataSet GetBanksSearch(string search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetBanksSearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("search", search);
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		//ADDED BY KARL 05/03/2021
		[WebMethod]
		public DataSet GetHouseBanksSearch(string search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetHouseBanksSearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("search", search);
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetHouseBanks()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetHouseBanks", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetInterBranch()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetInterBranch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		//ADDED BY KARL 05/03/2021
		[WebMethod]
		public DataSet GetInterBranchSearch(string search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetInterBranchSearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("search", search);
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "Banks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetOthersPaymentMean()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetOthersPaymentMean", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetOthersPaymentMean");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetBranch(string BankCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("GetBranch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("BankCode", BankCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Branch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetQuotationData(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("getQuotationData", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "QuotationData");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetRestructureData(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetRestructureData", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetRestructureData");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetRestructureDataSpecified(int DocEntry, int RSTDocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetRestructureDataSpecified", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("RSTDocEntry", RSTDocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetRestructureDataSpecified");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetGeneralData(int DocEntry, string Database)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetGeneralData", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Database", Database);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GeneralData");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetTotalPaymentsForTheYear(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetTotalPaymentsForTheYear", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetTotalPaymentsForTheYear");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetItemDetails(string ProjCode,
										string Block,
										string Lot,
										string Database)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetItemDetails1", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("ProjCode", ProjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						cmd.Parameters.AddWithValue("Database", Database);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GeneralData");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetSalesQuotationDetails(int SQDocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT A.""ItemCode"", A.""LineNum"",A.""Quantity"", A.""LineTotal"" FROM QUT1 A Where A.""DocEntry"" = {SQDocEntry} AND ""LineStatus"" ='O'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesQuotationDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetDownpaymentDetails(int DPIEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT A.""ItemCode"",A.""Quantity"",A.""Price"",A.""VatGroup"",A.""LineNum"",A.""LineTotal"" FROM DPI1 A Where A.""DocEntry"" = {DPIEntry} ", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "DownPaymentDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetSalesOrderDetails(int SQDocEntry, bool completeItems)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT 
                                                                A.""ItemCode"",
                                                                A.""Quantity"",
                                                                A.""Price"",
                                                                A.""VatGroup"",
                                                                A.""LineNum"",
                                                                A.""LineTotal"",
                                                                B.""QryGroup1""
                                                            FROM 
                                                                RDR1 A LEFT JOIN 
                                                                OITM B ON A.""ItemCode"" = B.""ItemCode""           
                                                                Where 
                                                                    A.""DocEntry"" = {SQDocEntry} {(completeItems ? "" : @"AND 
                                                                A.""LineStatus"" ='O' AND 
                                                                A.""ItemCode"" NOT IN (SELECT x.""ItemCode"" FROM OITM x WHERE x.""QryGroup4"" = 'Y' OR x.""QryGroup5"" = 'Y')")}", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesOrderDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}








		[WebMethod]
		//public DataSet GetARInvoiceDetails(int SQDocEntry, string fieldName)
		public DataSet GetARInvoiceDetails(string fieldName)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					//using (HanaCommand cmd = new HanaCommand($@"SELECT A.""LineNum"",A.""Quantity"" FROM RDR1 A Where A.""DocEntry"" = {SQDocEntry} AND ""LineStatus"" = 'O' AND ""ItemCode"" = (SELECT ""ItemCode"" FROM OITM x WHERE x.""{fieldName}"" = 'Y')", con))
					using (HanaCommand cmd = new HanaCommand($@"SELECT ""ItemCode"" FROM OITM x WHERE x.""{fieldName}"" = 'Y'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesOrderDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}










		public DataSet IsSalesQuotationExists(string Project,
												string Block,
												string Lot,
												string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT 
                                                                    ""DocEntry"",
                                                                    ""DocNum"" 
                                                                FROM 
                                                                    OQUT 
                                                                WHERE 
                                                                    ""Project"" = '{Project}' AND 
                                                                    ""U_BlockNo"" = '{Block}' AND 
                                                                    ""U_LotNo"" = '{Lot}' AND 
                                                                    ""CardCode"" = '{CardCode}' AND 
                                                                    ""CANCELED"" = 'N' AND 
                                                                    IFNULL(""U_IssueType"",'') <> 'CANCELED' AND
                                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed', 'Restructured','Advance') ",
															  con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesQuotationExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet IsSalesOrderExists(string Project,
												string Block,
												string Lot,
												string CardCode,
												string DocNum)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT 
                                                                    ""DocEntry"" 
                                                                FROM 
                                                                    ORDR 
                                                                WHERE 
                                                                    ""Project"" = '{Project}' AND 
                                                                    ""U_BlockNo"" = '{Block}' AND 
                                                                    ""U_LotNo"" = '{Lot}' AND 
                                                                    ""CardCode"" = '{CardCode}' AND 
                                                                    ""U_DreamsQuotationNo"" = '{DocNum}' AND  
                                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance') AND
                                                                    ""CANCELED"" = 'N'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesOrderExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet IsReserveInvoiceExists(string Project,
											  string Block,
											  string Lot,
											  string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					//2023-08-04 : ADDED U_PAYMENTTYPE TO GET PAYMENTSCHEME TYPE WHEN AN AR RES INV EXISTS
					using (HanaCommand cmd = new HanaCommand($@"SELECT 
                                                                    ""DocEntry"" ,
                                                                    ""U_PaymentType""
                                                                FROM 
                                                                    OINV 
                                                                WHERE 
                                                                    ""isIns"" = 'Y' AND 
                                                                    ""Project"" = '{Project}' AND 
                                                                    ""U_BlockNo"" = '{Block}' AND 
                                                                    ""U_LotNo"" = '{Lot}' AND 
                                                                    ""CardCode"" = '{CardCode}' AND 
                                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed') AND
                                                                    ""CANCELED"" = 'N'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "SalesOrderExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		public DataSet IsARDownpaymentExists(string Type,
												string PaymentOrder,
												string Project,
												string Block,
												string Lot,
												string CardCode,
												string DocDate,
												double TCP)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					string qry = $@"SELECT 
                                        A.""DocEntry"" 
                                    FROM 
                                        ODPI A 
                                    WHERE 
                                        A.""U_Type"" = '{Type}' AND 
                                        A.""U_PaymentOrder"" = '{PaymentOrder}' AND
                                        A.""Project"" = '{Project}' AND 
                                        A.""U_BlockNo"" = '{Block}' AND 
                                        A.""U_LotNo"" = '{Lot}' AND 
                                        A.""CardCode"" = '{CardCode}' AND 
                                        A.""CANCELED"" = 'N' AND 
                                        IFNULL(A.""U_ContractStatus"",'Open') IN ('Open', 'Closed','Advance') AND
                                        IFNULL(A.""U_RestructureTag"",'') <> 'OLD' AND
                                        --(SELECT ROUND(SUM(x.""DocTotal""),2) FROM ODPI x WHERE
                                        -- x.""U_Type"" = '{Type}' AND x.""U_PaymentOrder"" = '{PaymentOrder}' AND x.""Project"" = '{Project}' 
                                        --AND  x.""U_BlockNo"" = '{Block}'
                                        --AND x.""U_LotNo"" = '{Lot}' AND x.""CardCode"" = '{CardCode}'
                                        --AND x.""CANCELED"" = 'N') = {Math.Round(TCP, 2)} 
                                        --AND
                                        (SELECT Count(X.""DocEntry"") FROM DPI1 X WHERE X.""TargetType"" = 14  AND X.""DocEntry"" = A.""DocEntry"") = 0 ";
					using (HanaCommand cmd = new HanaCommand(qry, con))

					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "ARDownpaymentExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet IsARInvoiceStandaloneExists(string Type,
												string PaymentOrder,
												string Project,
												string Block,
												string Lot,
												string CardCode,
												double Amount,
												string Tagging,
												string DocDate,
												string Partial)
		{
			DataSet ret = null;
			try
			{
				Partial = string.IsNullOrWhiteSpace(Partial) ? "N" : Partial;
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT 
                                                                    ""DocEntry"" 
                                                                FROM 
                                                                    OINV
                                                                WHERE 
                                                                    ""U_Type"" = '{Type}' AND 
                                                                    ""U_PaymentOrder"" = '{PaymentOrder}' AND 
                                                                    ""Project"" = '{Project}' AND 
                                                                    ""U_BlockNo"" = '{Block}' AND 
                                                                    ""U_LotNo"" = '{Lot}' AND 
                                                                    ""CardCode"" = '{CardCode}'  AND 
                                                                    ""CANCELED"" = 'N' AND 
                                                                    ""U_TransactionType"" = '{Tagging}' AND 
                                                                    IFNULL(""U_Partial"",'N') = '{Partial}' AND
                                                                    IFNULL(""U_ContractStatus"",'Open') IN ('Open', 'Closed','Advance')  AND
                                                                    ""DocStatus"" = 'O'
                                                                    --AND
                                                                    --TO_VARCHAR(TO_DATE(""DocDate""), 'YYYYMMDD') = {DocDate} 
                                                                    --AND ( SELECT SUM(""PaidToDate"") FROM OINV WHERE ""U_Type"" = '{Type}' AND ""U_PaymentOrder"" = '{PaymentOrder}' 
                                                                    --AND ""Project"" = '{Project}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                                    --""CardCode"" = '{CardCode}'  AND ""CANCELED"" = 'N' AND ""U_TransactionType"" = '{Tagging}') >= {Amount}"
															, con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "ARInvoiceExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		public DataSet IsARInvoiceExists(string Type,
											  string PaymentOrder,
											  string Project,
											  string Block,
											  string Lot,
											  string CardCode,
											  string ARDate,
											  string Tagging)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT ""DocEntry"" FROM OINV WHERE ""U_Type"" = '{Type}' AND ""U_PaymentOrder"" = '{PaymentOrder}' 
                                                            AND ""Project"" = '{Project}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                            IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance') AND
															""CardCode"" = '{CardCode}'  AND ""CANCELED"" = 'N' AND IFNULL(""U_RestructureTag"",'') = '' "
															, con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "ARInvoiceExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		public DataSet IsDepositExists(string Type,
											string PaymentOrder,
											string Project,
											string Block,
											string Lot,
											string CardCode,
											string ARDate,
											string Tagging)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT ""DocEntry"" FROM OINV WHERE ""U_Type"" = '{Type}' AND ""U_PaymentOrder"" = '{PaymentOrder}' 
                                                            AND ""Project"" = '{Project}' AND ""U_BlockNo"" = '{Block}' AND ""U_LotNo"" = '{Lot}' AND 
                                                            ""CardCode"" = '{CardCode}'  AND ""CANCELED"" = 'N' "
															, con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "ARInvoiceExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}




		public DataSet IsARCMExists(string Project,
									string Block,
									string Lot,
									string CardCode,
									int DPIEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT A.""DocEntry"",A.""DocNum"" FROM ORIN A INNER JOIN RIN1 B ON A.""DocEntry"" = B.""DocEntry""
                                                              WHERE A.""U_SalesType"" = 'Real Estate' AND A.""Project"" = '{Project}' AND A.""U_BlockNo"" = '{Block}'
                                                              AND A.""U_LotNo"" = '{Lot}' AND A.""CardCode"" = '{CardCode}' AND A.""CANCELED"" = 'N' AND
                                                              IFNULL(A.""U_ContractStatus"",'Open') IN ('Open','Closed') AND
                                                              B.""BaseEntry"" = {DPIEntry} and B.""BaseType"" = 203", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "ARCMExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		public DataSet IsIncomingPaymentExists(string DocNum,
												int paymentOrder,
												string Type,
												double ARReserveInvoiceAmount)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{

					string qry = "";

					if (ARReserveInvoiceAmount > 0)
					{
						qry = $@"select A.""DocNum"", B.""DocStatus"", B.""CANCELED"" from rct2 A inner join oinv b on A.""DocEntry"" = B.""DocEntry"" and ""isIns"" = 'Y'
                                where A.""InvType"" = 13 and B.""U_DreamsQuotationNo"" = '{DocNum}' and B.""CANCELED"" = 'N' and B.""PaidToDate"" >= B.""DocTotal"" AND IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')";
					}
					else
					{
						qry = $@"select  A.""DocNum"", B.""DocStatus"", B.""CANCELED"" from rct2 A inner join odpi b on A.""DocEntry"" = B.""DocEntry"" 
                                                                        where A.""InvType"" = 203 and B.""U_DreamsQuotationNo"" = '{DocNum}' and B.""CANCELED"" = 'N' 
                                                            and	B.""U_PaymentOrder"" = '{paymentOrder}' and B.""U_Type"" = '{Type}' and B.""PaidToDate"" >= B.""DocTotal"" AND IFNULL(""U_ContractStatus"",'Open') IN ('Open','Closed','Advance')";

					}

					//using (HanaCommand cmd = new HanaCommand($@"SELECT ""DocEntry"" FROM ORCT WHERE ""CardCode"" = '{CardCode}' AND ""DocDate"" = '{DocDate}' AND ""U_ORNo"" = '{ORNumber}' AND ""Canceled"" = 'N'", con))
					using (HanaCommand cmd = new HanaCommand(qry, con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "IncomingPaymentExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet IsPaymentForSpecificTransactionExists(int DocEntry, int InvType)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT SUM(A.""SumApplied"") ""DocTotal""  FROM RCT2 A INNER JOIN ORCT B ON A.""DocNum"" = B.""DocEntry""
                                 WHERE A.""DocEntry"" = {DocEntry} AND A.""InvType"" = {InvType} AND B.""Canceled"" <> 'Y'", con))

					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "IncomingPaymentExists");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetPaymentSchedule(int DocEntry, string Type, string MiscType = "")
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetPaymentSchedule", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("MiscType", MiscType);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "PaymentSchedule");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}




		[WebMethod]
		public DataSet GetPaymentScheduleRestructuring(int DocEntry, string Type, string MiscType = "")
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetPaymentScheduleRestructuring", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("MiscType", MiscType);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetPaymentScheduleRestructuring");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public bool RemovePayments(int id, string Type)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("RemovePayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Id", id);
						cmd.Parameters.AddWithValue("@Type", Type);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool AddReservationPayments(int id, string Type,   //2
											int DocEntry, double Amount,  //4
											string ORNum, string Comments //6
																		  //    ,[Optional] string CheckDate, [Optional] string CheckNo , //8
																		  //    [Optional] string Bank, [Optional] string BankName,  //10
																		  //    [Optional] string Branch, [Optional] string Account //12
																		  //, [Optional] string CreditCard, [Optional] string CreditAcctCode,  //14
																		  //    [Optional] string CreditAcct, [Optional] string CreditCardNumber,  //16
																		  //    [Optional] string ValidUntil, [Optional] string IdNum,  //18
																		  //    [Optional] string TelNum, [Optional] string PymtTypeCode,  //20
																		  //    [Optional] string PymtType, [Optional] string NumOfPymts,  //22
																		  //    [Optional] string VoucherNum, [Optional] string CreditType //24

										  , string CheckDate = "2020-01-01", string CheckNo = "123", //8
											 string Bank = "", string BankName = "",  //10
											 string Branch = "", string Account = "" //12
										, string CreditCard = "", string CreditAcctCode = "",  //14
											 string CreditAcct = "", string CreditCardNumber = "",  //16
											 string ValidUntil = "2020-01-01", string IdNum = "",  //18
											 string TelNum = "", string PymtTypeCode = "",  //20
											 string PymtType = "", int NumOfPymts = 1,  //22
											 string VoucherNum = "", string CreditType = ""//24
			)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("AddReservationPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", id);
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Amount", Amount);
						cmd.Parameters.AddWithValue("CheckDate", CheckDate);
						cmd.Parameters.AddWithValue("CheckNum", CheckNo);
						cmd.Parameters.AddWithValue("Bank", Bank);
						cmd.Parameters.AddWithValue("BankName", BankName);
						cmd.Parameters.AddWithValue("Branch", Branch);
						cmd.Parameters.AddWithValue("Account", Account);
						cmd.Parameters.AddWithValue("CreditCard", CreditCard);
						cmd.Parameters.AddWithValue("CreditAcctCode", CreditAcctCode);
						cmd.Parameters.AddWithValue("CreditAcct", CreditAcct);
						cmd.Parameters.AddWithValue("CreditCardNumber", CreditCardNumber);
						cmd.Parameters.AddWithValue("ValidUntil", ValidUntil);
						cmd.Parameters.AddWithValue("IdNum", IdNum);
						cmd.Parameters.AddWithValue("TelNum", TelNum);
						cmd.Parameters.AddWithValue("PymtTypeCode", PymtTypeCode);
						cmd.Parameters.AddWithValue("PymtType", PymtType);
						cmd.Parameters.AddWithValue("NumOfPymts", NumOfPymts);
						cmd.Parameters.AddWithValue("VoucherNum", VoucherNum);
						cmd.Parameters.AddWithValue("CreditType", CreditType);
						cmd.Parameters.AddWithValue("ORNumber", ORNum);
						cmd.Parameters.AddWithValue("Comments", Comments);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}

		[WebMethod]
		public bool AddPDC(int DocEntry,
								  int SapDocEntry,
								  double Amount,
								  string CheckDate,
								  int CheckNo,
								  string BankCode,
								  string BankName,
								  string Branch,
								  string Account,
								  string ORNum,
								  string Comments,
								  string AccountNo,
								  string CreateDate,
								  string DepositBankCode,
								  string DepositBank,
								  string DepositBranch,
								  string ARPDCNo,
								  string CreateUserID,
								  string CreateUserName
		  )
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_AddPDC", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("SapDocEntry", SapDocEntry);
						cmd.Parameters.AddWithValue("Amount", Amount);
						cmd.Parameters.AddWithValue("CheckDate", CheckDate);
						cmd.Parameters.AddWithValue("CheckNum", CheckNo);

						cmd.Parameters.AddWithValue("BankCode", BankCode);
						cmd.Parameters.AddWithValue("BankName", BankName);
						cmd.Parameters.AddWithValue("Branch", Branch);
						cmd.Parameters.AddWithValue("Account", Account);
						cmd.Parameters.AddWithValue("ORNumber", ORNum);

						cmd.Parameters.AddWithValue("Comments", Comments);
						cmd.Parameters.AddWithValue("AccountNo", AccountNo);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						//cmd.Parameters.AddWithValue("DepositBankCode", DepositBankCode);
						//cmd.Parameters.AddWithValue("DepositBank", DepositBank);

						//cmd.Parameters.AddWithValue("DepositBranch", DepositBranch);
						cmd.Parameters.AddWithValue("ARPDCNo", ARPDCNo);
						cmd.Parameters.AddWithValue("CreateUserID", CreateUserID);
						cmd.Parameters.AddWithValue("CreateUserName", CreateUserName);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}

		[WebMethod]
		public bool PostQuotation(int Id,
								  string Type, //2
								  int DocEntry,
								  int SapDocEntry,  //4
								  double Amount,
								  string ORNum, //6
								  string Comments,
								  string CheckDate = "2020-01-01",  //8
								  string CheckNo = "123",
								  string Bank = "", //10
								  string BankName = "",
								  string Branch = "", //12
								  string Account = "",
								  string CreditCard = "",  //14
								  string CreditAcctCode = "",
								  string CreditAcct = "",   //16
								  string CreditCardNumber = "",
								  string ValidUntil = "2020-01-01", //18
								  string IdNum = "",
								  string TelNum = "",  //20
								  string PymtTypeCode = "",
								  string PymtType = "", //22
								  int NumOfPymts = 1,
								  string VoucherNum = "", //24
								  string CreditType = "",
								  string AccountNo = "", //26

								  string ORDate = "2020-01-01",
								  string ARNum = "", //28
								  string ARDate = "2020-01-01",
								  string PRNum = "", //30
								  string PRDate = "2020-01-01",

								  //others
								  string OthersModeOfPaymentCode = "",
								  string OthersModeOfPayment = "",
								  double OthersAmount = 0,
								  string OthersReferenceNo = "",
								  string OthersGLAccountCode = "",
								  string OthersGLAccountName = "",
								  string OthersPaymentDate = "2020-01-01",
								  string PostingDate = "2020-01-01"



			)



		{
			try
			{

				ORDate = string.IsNullOrWhiteSpace(ORDate) ? "2020-01-01" : ORDate;
				ARDate = string.IsNullOrWhiteSpace(ARDate) ? "2020-01-01" : ARDate;
				PRDate = string.IsNullOrWhiteSpace(PRDate) ? "2020-01-01" : PRDate;


				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("PostQuotation", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", Id);
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("SapDocEntry", SapDocEntry);
						cmd.Parameters.AddWithValue("Amount", Amount);
						cmd.Parameters.AddWithValue("CheckDate", CheckDate);
						cmd.Parameters.AddWithValue("CheckNum", CheckNo);
						cmd.Parameters.AddWithValue("Bank", Bank);
						cmd.Parameters.AddWithValue("BankName", BankName);
						cmd.Parameters.AddWithValue("Branch", Branch);
						cmd.Parameters.AddWithValue("Account", Account);
						cmd.Parameters.AddWithValue("CreditCard", CreditCard);
						cmd.Parameters.AddWithValue("CreditAcctCode", CreditAcctCode);
						cmd.Parameters.AddWithValue("CreditAcct", CreditAcct);
						cmd.Parameters.AddWithValue("CreditCardNumber", CreditCardNumber);
						cmd.Parameters.AddWithValue("ValidUntil", ValidUntil);
						cmd.Parameters.AddWithValue("IdNum", IdNum);
						cmd.Parameters.AddWithValue("TelNum", TelNum);
						cmd.Parameters.AddWithValue("PymtTypeCode", PymtTypeCode);
						cmd.Parameters.AddWithValue("PymtType", PymtType);
						cmd.Parameters.AddWithValue("NumOfPymts", NumOfPymts);
						cmd.Parameters.AddWithValue("VoucherNum", VoucherNum);
						cmd.Parameters.AddWithValue("CreditType", CreditType);
						cmd.Parameters.AddWithValue("ORNumber", ORNum.Replace("'", ""));
						cmd.Parameters.AddWithValue("Comments", Comments.Replace("'", ""));
						cmd.Parameters.AddWithValue("AccountNo", AccountNo.Replace("'", ""));
						//FOR OR, AR, AND PR fields
						cmd.Parameters.AddWithValue("ORDate", ORDate);
						cmd.Parameters.AddWithValue("ARNumber", ARNum);
						cmd.Parameters.AddWithValue("ARDate", ARDate);
						cmd.Parameters.AddWithValue("PRNumber", PRNum);
						cmd.Parameters.AddWithValue("PRDate", PRDate);
						//OTHERS
						cmd.Parameters.AddWithValue("OthersModeOfPaymentCode", OthersModeOfPaymentCode);
						cmd.Parameters.AddWithValue("OthersModeOfPayment", OthersModeOfPayment);
						cmd.Parameters.AddWithValue("OthersAmount", OthersAmount);
						cmd.Parameters.AddWithValue("OthersReferenceNo", OthersReferenceNo);
						cmd.Parameters.AddWithValue("OthersGLAccountCode", OthersGLAccountCode);
						cmd.Parameters.AddWithValue("OthersGLAccountName", OthersGLAccountName);
						cmd.Parameters.AddWithValue("OthersPaymentDate", OthersPaymentDate);
						cmd.Parameters.AddWithValue("PostingDate", PostingDate);


						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}


		public int UpdatePaymentQuotation(int DocEntry,
											string paymentdue_type, int paymentTerm)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"SELECT ""Id"" FROM QUT1 WHERE ""DocEntry"" = '{DocEntry}' AND ""PaymentType"" = '{paymentdue_type}' AND ""Terms"" = '{paymentTerm}'  AND IFNULL(""Cancelled"",'N') <> 'Y'", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "QuotationItem");
								return int.Parse(DataHelper.DataTableRet(ds.Tables[0], 0, "Id", "0"));
							}
						}
					}
				}

			}
			catch (Exception ex) { return 0; }
		}

		public int GetLatestRSTDocEntry()
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"select MAX(""RSTDocEntry"") ""RSTDocEntry"" from orst", con))
					{
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetLatestRSTDocEntry");
								return int.Parse(DataHelper.DataTableRet(ds.Tables[0], 0, "RSTDocEntry", "0"));
							}
						}
					}
				}

			}
			catch (Exception ex) { return 0; }
		}

		[WebMethod]
		public bool ReservationPayment(int DocEntry, string CardCode, double Payment, int PostToSAP, [Optional] int SapEntry, [Optional] string SapCardCode)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("ReservationPayment", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("@CardCode", CardCode);
						cmd.Parameters.AddWithValue("@Payment", Payment);
						cmd.Parameters.AddWithValue("@PostToSAP", PostToSAP);
						cmd.Parameters.AddWithValue("@SapEntry", SapEntry);
						cmd.Parameters.AddWithValue("@SapCardCode", SapEntry);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool ReservationPaymentCheck(int DocEntry, double Payment, string DueDate, string CheckNum, string BankCode, string Branch, string AccountNumber, int PostToSAP)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("ReservationPaymentCheck", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("@Payment", Payment);
						cmd.Parameters.AddWithValue("@DueDate", DueDate);
						cmd.Parameters.AddWithValue("@CheckNum", CheckNum);
						cmd.Parameters.AddWithValue("@BankCode", BankCode);
						cmd.Parameters.AddWithValue("@Branch", Branch);
						cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber);
						cmd.Parameters.AddWithValue("@PostToSAP", PostToSAP);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		public DataSet GetQuotationItem(string ProjCode, string Model, string Feature, string Category, string Size, string Lot)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetQuotationItemCode", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@ProjCode", ProjCode);
							cmd.Parameters.AddWithValue("@Model", Model);
							cmd.Parameters.AddWithValue("@Feature", Feature);
							cmd.Parameters.AddWithValue("@Category", Category);
							cmd.Parameters.AddWithValue("@Size", Size);
							cmd.Parameters.AddWithValue("@Lot", Lot.ToString());
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "QuotationItem");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		public DataSet GetReservationCash(string DocEntry)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetReservationCash", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetReservationCash");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		public DataSet GetReservationChecks(string DocEntry)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetReservationChecks", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetReservationChecks");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		public DataSet GetPartialPayments(int DocEntry)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("PartialPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "PartialPayments");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		public DataSet RsvGetOtherCharges(string ProjCode, string Model)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetOtherCharges", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("@ProjCode", ProjCode);
							cmd.Parameters.AddWithValue("@Model", Model);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "OtherCharges");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		#endregion

		#region "PROJECTS"
		[WebMethod]
		public DataSet GetSAPProjects()
		{
			DataSet ret = null;
			try
			{
				string query = @"SELECT 
                                    ""PrjCode"",
                                    ""PrjName"" 
                                FROM 
                                    ""OPRJ"" 
                                Where 
                                    ""Active"" = 'Y' AND 
                                    ""U_WithMap"" = 'Y'
                                ORDER BY 
                                    ""PrjCode"" ASC";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAPHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "SAPProjects");
						ret = ds;
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet SearchSAPProjects(string Keyword)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT 
                                    ""PrjCode"",""PrjName"" 
                                FROM 
                                    ""OPRJ"" 
                                Where 
                                    ""Active"" = 'Y' AND 
                                    ""U_WithMap"" = 'Y' AND
                                    (LOWER(""PrjCode"") like '%{Keyword.ToLower()}%' OR 
                                    LOWER(""PrjName"") like '%{Keyword.ToLower()}%')  
                                        
                                ORDER BY 
                                    ""PrjCode"" ASC";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAPHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "SearchSAPProjects");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetSAPBlock(string PrjCode)
		{
			DataSet ret = null;
			try
			{
				//string query = "SELECT DISTINCT MnfSerial[Block],U_Project [Project] FROM OBTN Where U_Project = @PrjCode and MnfSerial is not null OR MnfSerial <> ''";
				string query = "SELECT DISTINCT U_prBlock [Block],U_prProject [Project] FROM OBTN Where U_prProject = @PrjCode and U_prBlock is not null OR U_prBlock <> ''";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("SAP")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "SAPBlocks");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetProjects()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""PrjCode"",""PrjName"" FROM ""OPRJ"" ORDER BY ""PrjCode"" ASC";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Projects");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetProjectsSearch(string search)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""PrjCode"",""PrjName"" FROM ""OPRJ"" WHERE UPPER(""PrjCode"") LIKE UPPER('%{search}%')  or 
                                                    UPPER(""PrjName"") LIKE UPPER('%{search}%') ORDER BY ""PrjCode"" ASC";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Projects");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}





		[WebMethod]
		public DataSet GetProjectDetails(string ProjId)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT * FROM ""OPRJ"" Where ""PrjCode"" = '{ProjId}'";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					//using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							//cmd.Parameters.AddWithValue("ProjId", ProjId);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ProjectDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetProjectDetailsSAP(string ProjId)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT * FROM ""OPRJ"" Where ""PrjCode"" = '{ProjId}'";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					//using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							//cmd.Parameters.AddWithValue("ProjId", ProjId);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ProjectDetailsSAP");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool DeleteBlock(string ProjCode, string Block)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(@"Delete ""PRJ1"" Where ""PrjCode"" = :PrjCode and ""Block"" = :Block", con))
					{
						cmd.CommandType = CommandType.Text;
						cmd.Parameters.AddWithValue("PrjCode", ProjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool DeleteLot(string PrjCode, string Block, string Lot)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(@"Delete ""PRJ2"" Where ""PrjCode"" = :PrjCode and ""Block"" = :Block And ""Lot"" = :Lot", con))
					{
						cmd.CommandType = CommandType.Text;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public DataSet GetHouseInProjects(string PrjCode)
		{
			DataSet ret = null;
			try
			{
				string query = $"SELECT * FROM PRJ1 Where PrjCode = '{PrjCode}'";
				using (SqlDataAdapter da = new SqlDataAdapter(query, DataAccess.con("Addon")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "House");
						ret = ds;
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public byte[] GetProjectImage(string conString, string PrjCode)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = $@"SELECT ""PrjImage"" FROM ""OPRJ"" Where ""PrjCode"" = :ProjId";
					cmd.Parameters.AddWithValue("ProjId", PrjCode);
					cmd.CommandType = CommandType.Text;


					//Blob blob = new Blob();
					//blob = (Blob)cmd.ExecuteScalar();

					//byte[] data = (byte[])cmd.ExecuteScalar();
					//string base64 = System.Text.Encoding.ASCII.GetString(data);
					//var ex = Convert.FromBase64String(base64.Replace(" ","+"));

					return (byte[])cmd.ExecuteScalar();
				}
			}
			catch (Exception ex1) { return null; }
		}

		[WebMethod]
		public byte[] BlockPreview(string conString, string PrjCode, string Block)
		{
			using (HanaConnection con = new HanaConnection(conString))
			{
				HanaCommand cmd = new HanaCommand();
				cmd = con.CreateCommand();
				con.Open();
				cmd.CommandText = $@"SELECT ""BlockImage"" FROM ""PRJ1"" Where ""PrjCode"" = :PrjCode and ""Block"" = :Block";
				cmd.Parameters.AddWithValue("PrjCode", PrjCode);
				cmd.Parameters.AddWithValue("Block", Block);
				cmd.CommandType = CommandType.Text;

				return (byte[])cmd.ExecuteScalar();
			}
		}

		[WebMethod]
		public DataSet GetProjectBlocks(string PrjCode, string Block)
		{
			DataSet ret = null;
			try
			{
				string query = "GetProjectBlocks";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("Block", Block);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetProjectBlocks");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBlockByProject(string PrjCode)
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""Id"",""PrjCode"",""Block"",""BlockImage"",""ImgWidth"",""ImgHeight"",""description"",""left"",""top"" FROM ""PRJ1"" Where ""PrjCode"" = '{PrjCode}' Order By ""Block"" ASC";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ProjectBlocks");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBlockList(string PrjCode)
		{
			DataSet ret = null;
			try
			{
				string query = "projectBlocks";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Blocks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBlockLotsList(string PrjCode, string Block)
		{
			DataSet ret = null;
			try
			{
				string query = "projectBlocksLots";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("Block", Block);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Blocks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetLotList(string PrjCode, string Block)
		{
			DataSet ret = null;
			try
			{
				string query = "projectLots";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("Block", Block);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Lot");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetLotPerProject(string PrjCode, string Block)
		{
			DataSet ret = null;
			try
			{
				string query = "SELECT Id,PrjCode,Block,Lot,name,[description],[left],[top] FROM PRJ2 Where PrjCode = @PrjCode and Block=@Block Order By Lot ASC";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("@Block", Block);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "ProjectLot");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool UpdateProjectImage(string PrjCode, byte[] Image, int width, int height, int User)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("UpdateProjectImage", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("PrjImage", Image);
						cmd.Parameters.AddWithValue("width", width);
						cmd.Parameters.AddWithValue("height", height);
						cmd.Parameters.AddWithValue("UserCode2", User);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool AddNewProject(string conString, string PrjCode, string PrjName, byte[] Image, int width, int height, string User)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "AddProject";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("PrjCode", PrjCode);
					cmd.Parameters.AddWithValue("PrjName", PrjName);
					cmd.Parameters.AddWithValue("Image", Image);
					cmd.Parameters.AddWithValue("ImgWidth", width);
					cmd.Parameters.AddWithValue("ImgHeight", height);
					cmd.Parameters.AddWithValue("UserCode1", User);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool AddNewProjectLot(string PrjCode, string Block, string Lot, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("addLotPerProject", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						cmd.Parameters.AddWithValue("name", Lot);
						cmd.Parameters.AddWithValue("description", Convert.ToInt32(Lot.Replace("LOT", "")).ToString());
						cmd.Parameters.AddWithValue("left1", x - 17);
						cmd.Parameters.AddWithValue("top1", y - 17);
						cmd.Parameters.AddWithValue("radius", ConfigurationManager.AppSettings["LotRadius"].ToString());
						cmd.Parameters.AddWithValue("color", ConfigurationManager.AppSettings["LotColor"].ToString());
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateProjectLot(string PrjCode, string Block, string Lot, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("updateProjectLot", con))
					{
						string radius = Session["LotRadius"] == null ? ConfigurationManager.AppSettings["LotRadius"].ToString() : Session["LotRadius"].ToString();
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						cmd.Parameters.AddWithValue("name", Lot);
						cmd.Parameters.AddWithValue("description", Lot.Replace("LOT", ""));
						cmd.Parameters.AddWithValue("left1", x);
						cmd.Parameters.AddWithValue("top1", y);
						cmd.Parameters.AddWithValue("radius", radius);
						//cmd.Parameters.AddWithValue("radius", ConfigSettings.LotRadius"].ToString());
						cmd.Parameters.AddWithValue("color", ConfigurationManager.AppSettings["LotColor"].ToString());
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool UpdateImageProjectBlock(string PrjCode, string Block, byte[] Image, int width, int height)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("UpdateImageProjectBlock", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("BlockImage", Image);
						cmd.Parameters.AddWithValue("width", width);
						cmd.Parameters.AddWithValue("height", height);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool AddNewProjectBlock(string PrjCode, string Block, string BlockDescription, byte[] Image, int width, int height, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("addProjectBlock", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("BlockImage", Image);
						cmd.Parameters.AddWithValue("width", width);
						cmd.Parameters.AddWithValue("height", height);
						cmd.Parameters.AddWithValue("name", Block);
						cmd.Parameters.AddWithValue("description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("left1", x - 17);
						cmd.Parameters.AddWithValue("top1", y - 17);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateProjectBlock(string conString, string PrjCode, string Block, byte[] Image, int width, int height, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("updateProjectBlock", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("BlockImage", Image);
						cmd.Parameters.AddWithValue("width", width);
						cmd.Parameters.AddWithValue("height", height);
						cmd.Parameters.AddWithValue("name", Block);
						cmd.Parameters.AddWithValue("description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("left1", x);
						cmd.Parameters.AddWithValue("top1", y);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateProjectCanvass(string conString, string PrjCode, byte[] Canvass)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("updateprojectcanvass", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Canvass", Canvass);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex)
			{ return false; }
		}

		[WebMethod]
		public bool UpdateProjectBlockNoImage(string conString, string PrjCode, string Block, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("updateProjectBlockNoImage", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("name", Block);
						cmd.Parameters.AddWithValue("description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("left1", x - 17);
						cmd.Parameters.AddWithValue("top1", y - 17);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetMapMarkers_JSON(string PrjCode)
		{
			try
			{
				string query = "getMapMarkers";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch { }
		}


		[WebMethod]
		public DataSet GetSAPLot(string PrjCode, string Block)
		{
			DataSet ret = null;
			try
			{
				//string query = $"SELECT MnfSerial [Block], LotNumber [Lot] FROM OBTN Where U_Project = @PrjCode and MnfSerial = @Block";
				string query = "SELECT U_prBlock [Block], U_prLot [Lot] FROM OBTN Where U_prProject = @PrjCode and U_prBlock = @Block";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("SAP")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("@Block", Block);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "SAPLot");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetLot_JSON(string PrjCode, string Block)
		{
			try
			{
				string query = "getLotMap";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("Block", Block);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch { }
		}



		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetNewLot_JSON(string PrjCode)
		{
			try
			{
				string query = "getNewLotMap";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("PrjCode", PrjCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch { }
		}

		[WebMethod]
		public bool AddHouseToProject(string conString, string PrjCode, string ItemCode, string ItemName, int X, int Y)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(conString))
				{
					SqlCommand cmd = new SqlCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "AddHouseToProject";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
					cmd.Parameters.AddWithValue("@ItemCode", ItemCode);
					cmd.Parameters.AddWithValue("@ItemName", ItemName);
					cmd.Parameters.AddWithValue("@CoordinateX", X);
					cmd.Parameters.AddWithValue("@CoordinateY", Y);
					cmd.Parameters.AddWithValue("@Radius", 10);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception ex) { return false; }
		}

		[WebMethod]
		public bool AddNewLotTemp(string PrjCode, string Block, string Lot, int UserId)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("addLotTemp", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("@Block", Block);
						cmd.Parameters.AddWithValue("@Lot", Lot);
						cmd.Parameters.AddWithValue("@name", Lot);
						cmd.Parameters.AddWithValue("@description", Convert.ToInt32(Lot.Replace("LOT", "")).ToString());
						cmd.Parameters.AddWithValue("@radius", ConfigurationManager.AppSettings["LotRadius"].ToString());
						cmd.Parameters.AddWithValue("@color", ConfigurationManager.AppSettings["tempLotColor"].ToString());
						cmd.Parameters.AddWithValue("@UserId", UserId);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool UpdateBlockLocation(string PrjCode, string Block, int x, int y)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("UpdateBlockLocation", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("name", Block);
						cmd.Parameters.AddWithValue("description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("left1", x - 17);
						cmd.Parameters.AddWithValue("top1", y - 17);
						cmd.Parameters.AddWithValue("radius", ConfigurationManager.AppSettings["LotRadius"].ToString());
						cmd.Parameters.AddWithValue("fill", ConfigurationManager.AppSettings["LotColor"].ToString());
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool UpdateLotLocation(string PrjCode, string Block, int x, int y)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("UpdateBlockLocation", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("@Block", Block);
						cmd.Parameters.AddWithValue("@name", Block);
						cmd.Parameters.AddWithValue("@description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("@left", x - 17);
						cmd.Parameters.AddWithValue("@top", y - 17);
						cmd.Parameters.AddWithValue("@radius", ConfigurationManager.AppSettings["LotRadius"].ToString());
						cmd.Parameters.AddWithValue("@fill", ConfigurationManager.AppSettings["LotColor"].ToString());
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool AddNewBlockTemp(string PrjCode, string Block, int x, int y)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("addBlockTemp", con))
					{

						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("@Block", Block);
						cmd.Parameters.AddWithValue("@name", Block);
						cmd.Parameters.AddWithValue("@description", Convert.ToInt32(Block.Replace("BL", "")).ToString());
						cmd.Parameters.AddWithValue("@left", x);
						cmd.Parameters.AddWithValue("@top", y);
						cmd.Parameters.AddWithValue("@radius", ConfigurationManager.AppSettings["LotRadius"].ToString());
						cmd.Parameters.AddWithValue("@color", ConfigurationManager.AppSettings["tempLotColor"].ToString());
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void GetLotTemp_JSON(string PrjCode, string Block)
		{
			try
			{
				string query = "getLotMapTemp";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
							cmd.Parameters.AddWithValue("@Block", Block);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "objects");
								Context.Response.Output.Write(JsonConvert.SerializeObject(ds, Formatting.Indented));
							}
						}
					}
				}
			}
			catch (Exception ex) { }
		}
		[WebMethod]
		public bool UpdateLotTemp(string PrjCode, string Block, string Lot, int x, int y)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("updateLotTemp", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("@Block", Block);
						cmd.Parameters.AddWithValue("@lot", Lot);
						cmd.Parameters.AddWithValue("@left", x - 17);
						cmd.Parameters.AddWithValue("@top", y - 17);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateBlockTemp(string PrjCode, string Block, int x, int y, int UserId)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("updateBlockTemp", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("PrjCode", PrjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("left1", x - 17);
						cmd.Parameters.AddWithValue("top1", y - 17);
						cmd.Parameters.AddWithValue("UserId", UserId);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch { return false; }
		}
		#endregion

		#region "DOCUMENTS REQUIREMENTS"
		[WebMethod]
		public string GetDocStatusName(int ID)
		{
			string ret;
			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand($@"SELECT ""Code"" FROM ""ROLE"" WHERE ""Sequence"" = :ID", con))
						{
							cmd.Parameters.AddWithValue("ID", ID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Code", "");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}
		[WebMethod]
		public int GetDocStatusID(string Code)
		{
			int ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand($@"SELECT ""Sequence"" FROM ""ROLE"" WHERE ""Code"" = :Code", con))
						{
							cmd.Parameters.AddWithValue("Code", Code);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = Convert.ToInt32(DataAccess.GetData(dt, 0, "Sequence", "0"));
							}
						}
					}
				}
			}
			catch
			{ ret = 0; }
			return ret;
		}
		[WebMethod]
		public DataSet GetDocuments(string DocEntry, string CardCode, string SeqId)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_DocList";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocEntry", DocEntry);
							cmd.Parameters.AddWithValue("CardCode", CardCode);
							cmd.Parameters.AddWithValue("SequenceId", SeqId);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "DocumentList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBrokerDocuments()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"" FROM ""OBRD"" ";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerDocuments");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBrokerDocumentsStandard()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"" FROM ""OBRD"" WHERE ""Section"" = 'Standard';";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerDocumentsStandard");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBrokerDocumentsSoleProp()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"" FROM ""OBRD"" WHERE ""Section"" = 'Sole Proprietorship';";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerDocumentsSoleProprietorship");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBrokerDocumentsPartnership()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"" FROM ""OBRD"" WHERE ""Section"" = 'Partnership';";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerDocumentsPartnership");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBrokerDocumentsCorporation()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"" FROM ""OBRD"" WHERE ""Section"" = 'Corporation';";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerDocumentsCorporation");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBrokerList()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT 
                                    *, 
                                    CASE WHEN UPPER(""TypeOfBusiness"") = 'SOLE PROPRIETOR' THEN ""LastName"" || ', ' || ""FirstName"" || ' ' || ""MiddleName"" ELSE	""Partnership"" END ""Name"" 
                                 FROM 
                                    ""OBRK""";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBrokerList");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetDocumentsList()
		{
			DataSet ret = null;
			try
			{
				string query = "DocumentsList";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "DocumentsList");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetUserRoles()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(@"SELECT ""Code"",""Name"" FROM ""ROLE""", con))
						{
							cmd.CommandType = CommandType.Text;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetUserRoles");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetRequieredDocuments()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(@"SELECT ""DocId"",""Document"",""Code"" FROM ""ODOC""", con))
						{
							cmd.CommandType = CommandType.Text;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetRequieredDocuments");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetDocPerCode(string Code)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("GetDocPerCode", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Code", Code);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetDocPerCode");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetBuyers()
		{
			DataSet ret = null;
			try
			{
				string query = "sp_Buyers";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "BuyersList");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBuyerAttachments()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"", IFNULL(""Required"",0) ""Required"" FROM ""OCRA"" WHERE ""Section"" = 'Standard' AND IFNULL(""Required"",0) = 1 ;";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBuyerDocumentStandard");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBuyerAttachments_NotRequired()
		{
			DataSet ret = null;
			try
			{
				string query = $@"SELECT ""DocId"", ""Document"" ""DocumentName"", ""Section"", IFNULL(""Required"",0) ""Required"" FROM ""OCRA"" WHERE ""Section"" = 'Standard' AND IFNULL(""Required"",0) = 0;";
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection("SAOHana")))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, "GetBuyerAttachments_NotRequired");
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet BuyersSelection()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("BuyersSelection", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "BuyersSelection");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet BuyersAssessment()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("BuyersAssessment", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "BuyersAssessment");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet SearchBuyersSelection(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("SearchBuyersSelection", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "SearchBuyersSelection");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet SearchBuyersAssessment(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("SearchBuyersAssessment", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "SearchBuyersAssessment");
								ret = ds;
							}
						}
					}
				}
			}
			catch { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool UpdateDocumentCode(string DocId, string Code)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "UpdateDocCode";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("DocId", DocId);
					if (string.IsNullOrEmpty(Code))
					{
						cmd.Parameters.AddWithValue("Code", DBNull.Value);
					}
					else
					{
						cmd.Parameters.AddWithValue("Code", Code);
					}
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateDocuments(string DocEntry, string DocId, string DocName, byte[] Attachment, int UserID)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					SqlCommand cmd = new SqlCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "sp_UpdateDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
					cmd.Parameters.AddWithValue("@DocId", DocId);
					cmd.Parameters.AddWithValue("@DocName", DocName);
					cmd.Parameters.AddWithValue("@DocAttachment", Attachment);
					cmd.Parameters.AddWithValue("@UserID", UserID);
					cmd.ExecuteNonQuery();
					LastActivityDate(UserID);
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool AddBPDocuments(string CardCode, string DocId, string DocName, int UserID)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					SqlCommand cmd = new SqlCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "addBPDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@CardCode", CardCode);
					cmd.Parameters.AddWithValue("@DocId", DocId);
					cmd.Parameters.AddWithValue("@DocName", DocName);
					cmd.Parameters.AddWithValue("@CreateUserID", UserID);
					cmd.ExecuteNonQuery();
					LastActivityDate(UserID);
					return true;
				}
			}
			catch (Exception ex) { return false; }
		}
		[WebMethod]
		public bool DeleteBPDocuments(int UserID)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					SqlCommand cmd = new SqlCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "deleteBpDoc";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@UserID", UserID);
					cmd.ExecuteNonQuery();
					LastActivityDate(UserID);
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool AddDocuments(string DocEntry,
								 string CardCode,
								 string DocId,
								 string DocName,
								 //byte[] Attachment,
								 int UserID,
								 //string IDType,
								 string ReferenceNo,
								 string IssueDate,
								 string ExpirationDate,
								 string Remarks)
		{
			try
			{
				IssueDate = string.IsNullOrWhiteSpace(IssueDate) ? "2000-01-01" : IssueDate;
				ExpirationDate = string.IsNullOrWhiteSpace(ExpirationDate) ? "2000-01-01" : ExpirationDate;

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "sp_addDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("DocEntry", DocEntry);
					cmd.Parameters.AddWithValue("CardCode", CardCode);
					cmd.Parameters.AddWithValue("DocId", DocId);
					cmd.Parameters.AddWithValue("DocName", DocName);
					//cmd.Parameters.AddWithValue("DocAttachment", Attachment);
					cmd.Parameters.AddWithValue("UserID", UserID);
					//cmd.Parameters.AddWithValue("IDType", IDType);
					cmd.Parameters.AddWithValue("ReferenceNo", ReferenceNo);
					cmd.Parameters.AddWithValue("IssueDate", Convert.ToDateTime(IssueDate).ToString("yyyy-MM-dd"));
					cmd.Parameters.AddWithValue("ExpirationDate", Convert.ToDateTime(ExpirationDate).ToString("yyyy-MM-dd"));
					cmd.Parameters.AddWithValue("Remarks", Remarks);
					cmd.ExecuteNonQuery();
					LastActivityDate(UserID);
					return true;
				}
			}
			catch (Exception ex) { return false; }
		}


		[WebMethod]
		public bool AddRestructuringDocuments(string DocEntry,
								 string DocId,
								 string DocName,
								 string FileName,
								 string ExpirationDate,
								 int UserId,
								 string CreationDate)
		{
			try
			{
				ExpirationDate = string.IsNullOrWhiteSpace(ExpirationDate) ? "2000-01-01" : ExpirationDate;
				CreationDate = string.IsNullOrWhiteSpace(CreationDate) ? "2000-01-01" : CreationDate;

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "sp_AddRestructuringDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("DocEntry", DocEntry);
					cmd.Parameters.AddWithValue("DocId", DocId);
					cmd.Parameters.AddWithValue("DocName", DocName);
					cmd.Parameters.AddWithValue("FileName", FileName);
					cmd.Parameters.AddWithValue("ExpirationDate", Convert.ToDateTime(ExpirationDate).ToString("yyyy-MM-dd"));
					cmd.Parameters.AddWithValue("UserId", UserId);
					cmd.ExecuteNonQuery();
					LastActivityDate(UserId);
					return true;
				}
			}
			catch (Exception ex) { return false; }
		}















		[WebMethod]
		public byte[] DocumentPreview(string DocEntry, string DocId)
		{
			using (HanaConnection con = new HanaConnection(DataAccess.con("SAOHana")))
			{
				HanaCommand cmd = new HanaCommand();
				cmd = con.CreateCommand();
				con.Open();
				cmd.CommandText = $@"SELECT ""DocAttachment"" FROM ""QDOC"" Where ""DocEntry"" = :DocEntry and ""DocId"" = :DocId";
				cmd.Parameters.AddWithValue("DocEntry", DocEntry);
				cmd.Parameters.AddWithValue("DocId", DocId);
				cmd.CommandType = CommandType.Text;
				return (byte[])cmd.ExecuteScalar();
			}
		}

		[WebMethod]
		public string DeleteImage(string DocEntry, string DocId)
		{
			string ret = "";
			try
			{
				string query = "sp_DeleteImg";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("DocId", DocId);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}

		#endregion

		#region "DOWNPAYMENT"

		[WebMethod]
		public string SetDownpayment(int DocEntry, string DocType, int UserID)
		{
			string ret = "";
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_SetDownpayment", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("DocType", DocType);
						cmd.Parameters.AddWithValue("UserID", UserID);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		public int GetSQDocEntry(int aDocEntry)
		{
			int ret = 0;
			try
			{
				string query = "sp_GetSQDocEntry";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", aDocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataTable dt = new DataTable())
							{
								da.Fill(dt);
								ret = int.Parse((string)DataAccess.GetData(dt, 0, "SAPDocEntry", "0"));
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = 0; }
			return ret;
		}

		[WebMethod]
		public int GetSODocEntry(int DocEntry)
		{
			int ret = 0;
			try
			{
				string query = "sp_GetSODocEntry";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{App.GetConnectionDetails("SAPHana", "CS=")}");
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataTable dt = new DataTable())
							{
								da.Fill(dt);
								ret = int.Parse((string)DataAccess.GetData(dt, 0, "SOEntry", "0"));
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = 0; }
			return ret;
		}
		[WebMethod]
		public int GetDocEntryDPRef(int DocEntry)
		{
			int ret = 0;
			try
			{
				string query = "sp_GetDocEntryDPRef";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataTable dt = new DataTable())
							{
								da.Fill(dt);
								ret = int.Parse((string)DataAccess.GetData(dt, 0, "DocEntry", "0"));
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = 0; }
			return ret;
		}

		[WebMethod]
		public string SetDownpaymentActualPay(string DocEntry, DataTable Terms, int SAPDocEntry)
		{
			string ret = "";
			try
			{
				string query = "sp_SetDownpaymentActualPay";
				foreach (DataRow dr in Terms.Rows)
				{
					using (SqlConnection cnn = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand(query, cnn))
						{
							cnn.Open();
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							cmd.Parameters.AddWithValue("@Terms", dr["Terms"].ToString());
							cmd.Parameters.AddWithValue("@SAPDocEntry", SAPDocEntry);
							cmd.Parameters.AddWithValue("@Amount", dr["ActualPay"].ToString());
							cmd.ExecuteNonQuery();
							ret = "Operation completed successfully.";
						}
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}

		#endregion

		#region MAINTENANCE
		[WebMethod]
		public DataSet GetValidValues()
		{
			DataSet ret = null;
			try
			{
				string query = "getValidValues";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ValidValues");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetRoles()
		{
			DataSet ret = null;
			try
			{
				string query = "getRoles";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "getRoles");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetTerms(string GrpCode)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetTerms";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("GrpCode", GrpCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Terms");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetDPTermsSAP(string GrpCode)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetDPTermsSAP";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Code", GrpCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetDPTermsSAP");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet GetLTermsSAP(string GrpCode)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetLTermsSAP";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Code", GrpCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "sp_GetLTermsSAP");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}







		[WebMethod]
		public DataSet GetFinancingScheme()
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetFinancingScheme";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "FinancingScheme");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetDPDays()
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetDPDays";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Code", "");
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetDPDays");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetDocumentStatus()
		{
			DataSet ret = null;
			try
			{
				string query = "DocumentStatus";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "DocumentStatus");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet ComputeSurcharge(double RunningBal, double DPPenalty, double MonthsDue, double DaysDue)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_ComputeSurcharge";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("RunningBal", RunningBal);
							cmd.Parameters.AddWithValue("DPPenalty", DPPenalty);
							cmd.Parameters.AddWithValue("MonthsDue", MonthsDue);
							cmd.Parameters.AddWithValue("DaysDue", DaysDue);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ComputeSurcharge");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public bool AddColor(string Code, string Color)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "addColor";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Code", Code);
					cmd.Parameters.AddWithValue("Color", Color);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateColor(string Code, string Color)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "updateColor";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Code", Code);
					cmd.Parameters.AddWithValue("Color", Color);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool UpdateValidValues(string Code, string Name)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "updateValidValues";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Code", Code);
					cmd.Parameters.AddWithValue("Name", Name);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool AddTerms(string Code, string GrpCode)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					SqlCommand cmd = new SqlCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "sp_AddTerms";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@Code", Code);
					cmd.Parameters.AddWithValue("@GrpCode", GrpCode);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool AddRole(int Id, int Seq, string Code, string Name)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "AddRoles";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Id", Id);
					cmd.Parameters.AddWithValue("Seq", Seq);
					cmd.Parameters.AddWithValue("Code", Code);
					cmd.Parameters.AddWithValue("Name", Name);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}
		[WebMethod]
		public bool AddDocumentRequirements(string Doc, string UserId, string BusinessType)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "addDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Document", Doc);
					cmd.Parameters.AddWithValue("CreateUserID", UserId);
					cmd.Parameters.AddWithValue("BusinessType", BusinessType);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}

		[WebMethod]
		public bool AddBrokerDocumentRequirements(string Doc, string Section, string UserId)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					HanaCommand cmd = new HanaCommand();
					cmd = con.CreateCommand();
					con.Open();
					cmd.CommandText = "addBrokerDocuments";
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Document", Doc);
					cmd.Parameters.AddWithValue("Section", Section);
					cmd.Parameters.AddWithValue("CreateUserID", UserId);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch { return false; }
		}

		[WebMethod]
		public bool UpdatePassword(string UserID, string Password)
		{
			bool ret = false;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand($"sp_UpdatePassword", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Username", UserID);
						cmd.Parameters.AddWithValue("Password", Password);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch (Exception ex) { ret = false; }
			return ret;
		}


		#endregion

		#region CASHIER
		public DataSet GetBuyersProject(string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetBuyersProject", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetBuyersProject");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet GetReceiptNumber(string Project, string Type)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetReceiptNumber", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("Project", Project);
						cmd.Parameters.AddWithValue("Type", Type);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetReceiptNumber");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		public DataSet GetDuePayments(string CardCode, string ProjCode, string Block, string Lot)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("DuePayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						cmd.Parameters.AddWithValue("ProjCode", ProjCode);
						cmd.Parameters.AddWithValue("Block", Block);
						cmd.Parameters.AddWithValue("Lot", Lot);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "DuePayments");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetDownPayments(int DocEntry, string Finance)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("DownPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Finance", Finance);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetDownPayments");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet GetPaymentBreakdown(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("PaymentBreakdown", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetPaymentBreakdown");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		public DataSet GetReservationPayments(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetReservationPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetReservationPayments");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}




		public DataSet GetReservationPaymentsNoOR(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetReservationPaymentsNoOR", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetReservationPaymentsNoOR");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetAllReservationPayments(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetAllReservationPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetAllReservationPayments");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetGLAccounts()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetGLAccounts", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetGLAccounts");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet SearchGLAccounts(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("SearchGLAccounts", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
							cmd.Parameters.AddWithValue("Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "SearchGLAccounts");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetCreditCards()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetCreditCards", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCreditCards");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet GetCreditPaymentMethod()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetCreditPaymentMethod", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCreditPaymentMethod");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet GetCardBrandAccount()
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetCardBrandAccount", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCardBrandAccount");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public bool insertORNumber(int DocEntry, string OR, string Comments)
		{
			try
			{
				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("insertORNumber", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("@ORNumber", OR);
						cmd.Parameters.AddWithValue("@Comments", Comments);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		public bool UpdateDPPayment(int DocEntry, int Terms, string Type, double AmountPaid, double Penalty)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("UpdateDPPayment", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Terms", Terms);
						cmd.Parameters.AddWithValue("PaymentType", Type);
						cmd.Parameters.AddWithValue("AmountPaid", AmountPaid);
						cmd.Parameters.AddWithValue("Penalty", Penalty);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		public bool CreateDownPayment(int DocEntry, int SapEntry, string OR, string Remarks, double Total, double Penalties, double Interest)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("CreateDownPayment", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("SapEntry", SapEntry);
						cmd.Parameters.AddWithValue("ORNum", OR);
						cmd.Parameters.AddWithValue("Remarks", Remarks);
						cmd.Parameters.AddWithValue("Amount", Total);
						cmd.Parameters.AddWithValue("Penalty", Penalties);
						cmd.Parameters.AddWithValue("Interest", Interest);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		public bool CreateDownPaymentTerms(int DocEntry, int SapDocEntry, string Type, int Terms)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("CreateDownPaymentTerms", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("SapDocEntry", SapDocEntry);
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("Terms", Terms);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}
		public bool AddDownPayments(string Type, int DocEntry, double Amount, string ORNum, string Comments
			, [Optional] string CheckDate, [Optional] string CheckNo, [Optional] string Bank, [Optional] string BankName, [Optional] string Branch, [Optional] string Account
			, [Optional] string CreditCard, [Optional] string CreditAcctCode, [Optional] string CreditAcct, [Optional] string CreditCardNumber, [Optional] string ValidUntil, [Optional] string IdNum, [Optional] string TelNum
			, [Optional] string PymtTypeCode, [Optional] string PymtType, [Optional] string NumOfPymts, [Optional] string VoucherNum, [Optional] string CreditType
			)
		{
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("AddDownPayments", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Type", Type);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Amount", Amount);
						cmd.Parameters.AddWithValue("CheckDate", CheckDate);
						cmd.Parameters.AddWithValue("CheckNum", CheckNo);
						cmd.Parameters.AddWithValue("Bank", Bank);
						cmd.Parameters.AddWithValue("BankName", BankName);
						cmd.Parameters.AddWithValue("Branch", Branch);
						cmd.Parameters.AddWithValue("Account", Account);
						cmd.Parameters.AddWithValue("CreditCard", CreditCard);
						cmd.Parameters.AddWithValue("CreditAcctCode", CreditAcctCode);
						cmd.Parameters.AddWithValue("CreditAcct", CreditAcct);
						cmd.Parameters.AddWithValue("CreditCardNumber", CreditCardNumber);
						cmd.Parameters.AddWithValue("ValidUntil", ValidUntil);
						cmd.Parameters.AddWithValue("IdNum", IdNum);
						cmd.Parameters.AddWithValue("TelNum", TelNum);
						cmd.Parameters.AddWithValue("PymtTypeCode", PymtTypeCode);
						cmd.Parameters.AddWithValue("PymtType", PymtType);
						cmd.Parameters.AddWithValue("NumOfPymts", NumOfPymts);
						cmd.Parameters.AddWithValue("VoucherNum", VoucherNum);
						cmd.Parameters.AddWithValue("CreditType", CreditType);
						cmd.Parameters.AddWithValue("ORNumber", ORNum);
						cmd.Parameters.AddWithValue("Comments", Comments);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}

		public DataSet CheckORPerLocation(string Project, string ReceiptNo //,string User
																			)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_CheckORPerLocation", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						cmd.Parameters.AddWithValue("Project", Project);
						cmd.Parameters.AddWithValue("ReceiptNo", ReceiptNo);
						//cmd.Parameters.AddWithValue("User", User);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "CheckORPerLocation");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		#endregion

		#region "OTHERS"
		//########## SELECT STD ##########//
		[WebMethod]
		public DataSet Select(string query, string TableName, string con)
		{
			DataSet ret = null;
			try
			{
				using (HanaDataAdapter da = new HanaDataAdapter(query, hana.GetConnection(con)))
				{
					using (DataSet ds = new DataSet())
					{
						da.Fill(ds, TableName);
						ret = ds;
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public string GetRoleByID(int ID)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand($"sp_GetRoleByID", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@UserID", ID);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								if (DataAccess.Exist(dt) == true)
								{
									ret = Cryption.Decrypt((string)DataAccess.GetData(dt, 0, "CodeEncrypt", ""));
									int cnt = ret.Length - ID.ToString().Length;
									ret = ret.Substring(ID.ToString().Length, cnt);
								}
								else { ret = ""; }
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}
		[WebMethod]
		public string GetSeqId(string Code)
		{
			string ret;

			try
			{
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("GetSeqId", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Code", Code);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "Sequence", "");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = null; }
			return ret;
		}

		string GetErr(DataTable dt)
		{
			string ret;
			string code = (string)DataAccess.GetData(dt, 0, "Column1", "0");
			string msg = (string)DataAccess.GetData(dt, 0, "Column2", "Operation completed successfully.");
			if (code == "0")
			{ ret = msg; }
			else { ret = $"({code}) {msg}"; }
			return ret;
		}

		//########## SELECT STD ##########//
		[WebMethod]
		public string Execute(string query, string con)
		{
			string ret = "";
			try
			{
				using (HanaConnection cnn = new HanaConnection(con))
				{
					using (HanaCommand cmd = new HanaCommand(query, cnn))
					{
						cnn.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}
		[WebMethod]
		public bool ExecuteCmd(string query, string con)
		{
			try
			{
				using (SqlConnection cnn = new SqlConnection(DataAccess.con(con)))
				{
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						cnn.Open();
						cmd.ExecuteNonQuery();
						return true;
					}
				}
			}
			catch (Exception ex) { return false; }
		}

		//########## [ GET SERIES ] ##########//
		[WebMethod]
		public string GetAutoKey(int ObjCode, string Type)
		{
			string ret;

			try
			{
				string query = "sp_AutoKey";
				using (DataTable dt = new DataTable())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("ObjCode", ObjCode);
							cmd.Parameters.AddWithValue("Type", Type);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(dt);
								ret = (string)DataAccess.GetData(dt, 0, "AutoKey", "");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{ ret = ex.Message; }
			return ret;
		}

		[WebMethod]
		[ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
		public void DatatableToJson(DataTable dt)
		{
			System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
			Dictionary<string, object> row;
			foreach (DataRow dr in dt.Rows)
			{
				row = new Dictionary<string, object>();
				foreach (DataColumn col in dt.Columns)
				{
					row.Add(col.ColumnName, dr[col]);
				}
				rows.Add(row);
			}
			Context.Response.Output.Write(serializer.Serialize(rows));
		}
		#endregion

		#region RESTRUCTURE

		[WebMethod]
		public DataSet RestructureBuyerList()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("RestructureBuyerList", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "RestructureBuyerList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet OwnerList()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_OwnersList", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "OwnerList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public DataSet OwnerListSearch(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_OwnersListSearch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "OwnerListSearch");
								ret = ds;
							}
						}
					}

				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet RestructureBuyerListSearch(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("RestructureBuyerListSearch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "RestructureBuyerListSearch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet LoadRestructuringTypes(string UserAccess)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_LoadRestructuringTypes", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							cmd.Parameters.AddWithValue("UserAccess", UserAccess);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "LoadRestructuringTypes");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}









		[WebMethod]
		public DataSet GetAccountProjectList(string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("GetAccountProjectList", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("CardCode", CardCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetAccountProjectList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		public DataSet GetAccountDocuments(string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetAccountDocuments", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@CardCode", CardCode);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetAccountDocuments");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public bool Restructure(
								int DocEntry,
							   string oCardCode,  //2
							   DateTime oDate,
							   string oDocStatus, //4
							   string ProjCode,

							   string Block, //6 
							   string Lot,
							   string Model, //8
							   string FinancingScheme,
							   string LotArea, //10

							   string FloorArea,
							   string ProductStatus,   //12
							   string Phase,
							   string LotClassification, //14 
							   string ProductType,

							   string LoanType, //16 
							   string Bank,
							   double OTcp,  //18  
							   double ResrvFee,
							   double DPPercent, //20

							   double DPAmount,
							   int DPTerms,  //22 
							   double DiscPercent,
							   double DiscAmount,  //24
							   int LTerms,

							   double InterestRate, //26
							   DateTime LDueDate,
							   double Gdi,    //28
							   double Tcp,
							   double OMisc, // 30

							   double GrossTCP,
							   double Vat, //32
							   double NetTcp,
							   double TCPDownpayment, //34
							   double MonthlyDP,

							   DateTime DPDueDate,  //36
							   double LAmount,
							   int UserID, //38
							   string EmployeeID,
							   string EmployeeName,  //40

							   string EmployeePosition,
							   DataTable oDownpayment, //42
							   DataTable oAmort,
							   string DocNum,  // 44
							   double PDMonthly,

							   double TCPMonthly,  // 46
							   double MiscFeesMonthly,
							   double AddMiscFees,  //48
							   double DPBalanceOnEquity,
							   double TCPBalanceOnEquity, //50

							   double PDMisc,
							   double PDLoanableBalance,  //52
							   double TCPLoanableBalance,
							   string Remarks,//54
							   string RetitlingType,

							   double CSTotal,  //56 
												//DP
							   int oldDPTerms,
							   DateTime oDPDueDate, //58
							   DateTime oldDPDueDate,
							   double oldTcp, //60

								double oDPAmount,
								//LB
								int oldLBTerms, //62
								DateTime oldLDueDate,
								DateTime oLDueDate, //64
													//Paid
								double AmountPaid,

								string oldFinancingScheme, //66
								string oldProductType,
								string oldLot, //68
								string oldHouseModel,
								string oldProject, //70

								string RestructuringType,
								DateTime ApprovalDate, //72
								string RequestLetter,
								string QuotationDocEntry, //74
								string DPDocEntry,

								string employeeID, //76
								string employeeName,
								string employeePosition, //78
								string RestructureAction,
								double IPS, //80

								string Vatable,
								double MonthlyLB, //82

								DateTime RestructuringDate,
								string MiscEntry,
								double TotalMiscPayment,
								string oldCardCode,
								string oldRetitlingType,

								string UpdateAmortBalance,

								//-- MGA NEW FIELDS (2023-03-07)
								string CoMaker,
								DateTime MiscDueDate,
								string MiscFinancingScheme,
								int MiscDPTerms,
								int oldMiscTerms,
								DataTable oMiscDP,
								DataTable oMiscAmort,

								double MiscDPAmount,
								int MiscLBTerms,
								double MiscLBAmount,
								double MiscLBMonthly,
								//DataTable dtCheckedLBForAdvancedPayments,

								//2023-06-27 : FOR CHECKING IF AGENT IS DIFFERENT
								int checkAgent,

								//2024-10-05 : ADDITIONAL PARAMETERS: MIN LB TERM AND MIN DUEDATE FOR ADDING OF ADVANCE TO PRINCIPAL PAYMENT
								string MinLBTerm,
								string MinLBDate,
								double AdvancePayment,
								int PaymentEntry,

								out string Message //84



		   ) //58
		{
			try
			{
				bool isError = false;



				HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana"));
				HanaCommand cmd = new HanaCommand("sp_Restructure", con);
				con.Open();
				cmd.CommandType = CommandType.StoredProcedure;

				cmd.Parameters.AddWithValue("DocEntry", DocEntry);
				//cmd.Parameters.AddWithValue("DocNum", GetAutoKey(2, "D")); //2
				cmd.Parameters.AddWithValue("DocNum", DocNum); //2
				cmd.Parameters.AddWithValue("CardCode", oCardCode);
				cmd.Parameters.AddWithValue("DocDate", oDate.ToString("yyyy/MM/dd")); //4
				cmd.Parameters.AddWithValue("DocStatus", oDocStatus);

				cmd.Parameters.AddWithValue("ProjCode", ProjCode);  //6
				cmd.Parameters.AddWithValue("Block", Block);
				cmd.Parameters.AddWithValue("Lot", Lot); //8
				cmd.Parameters.AddWithValue("Model", Model);
				cmd.Parameters.AddWithValue("FinancingScheme", FinancingScheme); //10

				cmd.Parameters.AddWithValue("LotArea", LotArea);
				cmd.Parameters.AddWithValue("FloorArea", FloorArea); //12
				cmd.Parameters.AddWithValue("ProductStatus", ProductStatus);
				cmd.Parameters.AddWithValue("Phase", Phase);  //14
				cmd.Parameters.AddWithValue("LotClassification", LotClassification);

				cmd.Parameters.AddWithValue("ProductType", ProductType);  //16
				cmd.Parameters.AddWithValue("LoanType", LoanType);
				cmd.Parameters.AddWithValue("Bank", Bank);  //18
				cmd.Parameters.AddWithValue("OTcp", OTcp);
				cmd.Parameters.AddWithValue("ResrvFee", ResrvFee); //20

				cmd.Parameters.AddWithValue("DPPercent", DPPercent);
				cmd.Parameters.AddWithValue("DPAmount", DPAmount); //22
				cmd.Parameters.AddWithValue("DPTerms", DPTerms);
				cmd.Parameters.AddWithValue("DiscPercent", DiscPercent);  //24 //--DISC PERCENT
				cmd.Parameters.AddWithValue("DiscAmount", DiscAmount);

				cmd.Parameters.AddWithValue("LTerms", LTerms); //26
				cmd.Parameters.AddWithValue("InterestRate", InterestRate);
				cmd.Parameters.AddWithValue("LDueDate", LDueDate.ToString("yyyy-MM-dd")); //28
				cmd.Parameters.AddWithValue("Gdi", Gdi);
				cmd.Parameters.AddWithValue("TransType", "A");  //30

				cmd.Parameters.AddWithValue("Tcp", Tcp);
				cmd.Parameters.AddWithValue("OMisc", OMisc);  //32
				cmd.Parameters.AddWithValue("GrossTCP", GrossTCP);
				cmd.Parameters.AddWithValue("Vat", Vat); //34
				cmd.Parameters.AddWithValue("NetTcp", NetTcp);
				cmd.Parameters.AddWithValue("TCPDownpayment", TCPDownpayment); //36

				cmd.Parameters.AddWithValue("MonthlyDP", MonthlyDP);
				cmd.Parameters.AddWithValue("DPDueDate", DPDueDate.ToString("yyyy-MM-dd")); //38
				cmd.Parameters.AddWithValue("LAmount", LAmount);
				cmd.Parameters.AddWithValue("UserID", UserID); //40
				cmd.Parameters.AddWithValue("LetterReqDocument", "");

				cmd.Parameters.AddWithValue("PDMonthly", PDMonthly); //42
				cmd.Parameters.AddWithValue("TCPMonthly", TCPMonthly);
				cmd.Parameters.AddWithValue("MiscFeesMonthly", MiscFeesMonthly); //44
				cmd.Parameters.AddWithValue("AddMiscFees", AddMiscFees);
				cmd.Parameters.AddWithValue("DPBalanceOnEquity", DPBalanceOnEquity);//46

				cmd.Parameters.AddWithValue("TCPBalanceOnEquity", TCPBalanceOnEquity);
				cmd.Parameters.AddWithValue("PDMisc", PDMisc); //48
				cmd.Parameters.AddWithValue("PDLoanableBalance", PDLoanableBalance);
				cmd.Parameters.AddWithValue("TCPLoanableBalance", TCPLoanableBalance); //50

				cmd.Parameters.AddWithValue("Remarks", Remarks);
				cmd.Parameters.AddWithValue("RetitlingType", RetitlingType);  //52
				cmd.Parameters.AddWithValue("CSTotal", CSTotal);
				cmd.Parameters.AddWithValue("RestructuringType", RestructuringType); //54
				cmd.Parameters.AddWithValue("ApprovalDate", ApprovalDate);

				cmd.Parameters.AddWithValue("RequestLetter", RequestLetter); //56

				cmd.Parameters.AddWithValue("employeeID", employeeID);
				cmd.Parameters.AddWithValue("employeeName", employeeName); //58
				cmd.Parameters.AddWithValue("employeePosition", employeePosition);
				cmd.Parameters.AddWithValue("RestructuringDate", RestructuringDate.ToString("yyyy/MM/dd")); //60

				cmd.Parameters.AddWithValue("Vatable", Vatable);
				cmd.Parameters.AddWithValue("MonthlyLB", MonthlyLB); //62

				//-- MGA NEW FIELDS (2023-03-07)
				cmd.Parameters.AddWithValue("CoMaker", CoMaker);
				cmd.Parameters.AddWithValue("MiscDueDate", MiscDueDate);
				cmd.Parameters.AddWithValue("MiscFinancingScheme", MiscFinancingScheme);
				cmd.Parameters.AddWithValue("MiscDPTerms", MiscDPTerms);

				cmd.Parameters.AddWithValue("MiscDPAmount", MiscDPAmount);
				cmd.Parameters.AddWithValue("MiscLBTerms", MiscLBTerms);
				cmd.Parameters.AddWithValue("MiscLBAmount", MiscLBAmount);
				cmd.Parameters.AddWithValue("MiscLBMonthly", MiscLBMonthly);

				//2023-06-27 : FOR CHECKING IF AGENT IS DIFFERENT
				cmd.Parameters.AddWithValue("checkAgent", checkAgent);


				using (HanaDataAdapter da = new HanaDataAdapter(cmd))
				{

					cmd.ExecuteNonQuery();
				}

				int DPEntry = Convert.ToInt32(DataAccess.GetData(hana.GetData($@"SELECT MAX(""RSTDocEntry"") ""RSTDocEntry"" FROM ORST WHERE ""DocEntry"" = {DocEntry}",
					hana.GetConnection("SAOHana")), 0, "RSTDocEntry", "0"));


				int RSTId = GetLatestRSTDocEntry();



				string LineStatusDP = "O";
				double TotalPayment = AmountPaid;
				double TotalPaymentMisc = TotalMiscPayment;

				double tempResPayment = 0;


				string errmsg = string.Empty;

				if (RestructureAction == "NEW")
				{

					if (UpdateAmortBalance == "YES")
					{


						//if (RestructureAction == "NEW")
						//{

						//**RECREATE DP SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
						//2023-05-18 : REMOVED CONDITION : ALL SCHEDULE SHOULD BE CANCELLED REGARDLESS IF PAID OR NOT 
						// if (oDownpayment != null)
						// {


						//if (FinancingScheme != oldFinancingScheme ||
						//    ProductType != oldProductType ||
						//    Lot != oldLot ||
						//    Model != oldHouseModel ||
						//    ProjCode != oldProject ||
						//    oCardCode != oldCardCode ||
						//    RetitlingType != oldRetitlingType)
						//{


						if (RestructuringType != "AdvancePayment")
						{


							//CANCEL ALL SCHEDULE ROWS IN QUT1 (DP, MISC AND RES PAYMENT TYPES)
							if (CancelDPI(DocEntry, "DP", RSTId, con))
							{
								#region comment
								////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE RESERVATION FEE   
								//if (Math.Round(TotalPayment, 2) >= Math.Round(ResrvFee, 2))
								//{
								//    LineStatusDP = "C";
								//    TotalPayment -= ResrvFee;
								//    tempResPayment = ResrvFee;
								//}
								//else
								//{
								//    LineStatusDP = "O";
								//    tempResPayment = TotalPayment;
								//    TotalPayment -= ResrvFee;
								//}


								//ADD NEW ROW IN QUT1 (SCHEDULE) -- RESERVATION
								//if (!RPayment(DocEntry,
								//               1,
								//              oDate,
								//              ResrvFee,
								//              0,
								//              0,
								//              0,
								//              ResrvFee,
								//              0,
								//              0,
								//              "RES",
								//              LineStatusDP,
								//              tempResPayment,
								//              QuotationDocEntry,
								//              0,
								//              con))
								//{
								//    isError = true;
								//    errmsg = "Error on saving Reservation schedule locally. Please contact administrator.";
								//}
								#endregion


								if (!isError)
								{

									//2023-05-18 : CHECK IF DOWNPAYMENT SCHEDULE EXISTS WHEN GGENERATED FROM RESTRUCTURING
									if (oDownpayment != null)
									{

										// ** loop for inserting data to QUT1
										foreach (DataRow dr in oDownpayment.Rows)
										{

											string oConvert = dr["DueDate"].ToString();
											DateTime oDueDate = Convert.ToDateTime(oConvert);
											LineStatusDP = "O";

											double ToBePaidAmount = 0;
											double tempAmt = 0;

											string SAPDocEntry = "0";

											//CHECK IF PAYMENT TYPE IS DP (MISC DOESNT HAVE ANY SAP DOC ENTRIES)
											if (dr["PaymentType"].ToString() == "DP")
											{
												ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);

												string qryDPEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = ''";
												DataTable dtDPEntry = hana.GetData(qryDPEntry, hana.GetConnection("SAOHana"));
												DPDocEntry = DataAccess.GetData(dtDPEntry, 0, "SapDocEntry", "").ToString();

												SAPDocEntry = DPDocEntry;


												////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE DP
												//if (Math.Round(TotalPayment, 2) >= Math.Round(ToBePaidAmount, 2))
												//{
												//    tempAmt = ToBePaidAmount;
												//    TotalPayment -= ToBePaidAmount;
												//    LineStatusDP = "C";
												//}
												//else
												//{
												//    tempAmt = (TotalPayment <= 0 ? 0 : TotalPayment);
												//    TotalPayment -= ToBePaidAmount;
												//    TotalPayment = (TotalPayment <= 0 ? 0 : TotalPayment);
												//    LineStatusDP = "O";
												//}

											}
											else if (dr["PaymentType"].ToString() == "MISC")
											{
												ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);

												string qryMiscEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = ''";
												DataTable dtMiscEntry = hana.GetData(qryMiscEntry, hana.GetConnection("SAOHana"));
												MiscEntry = DataAccess.GetData(dtMiscEntry, 0, "SapDocEntry", "").ToString();

												SAPDocEntry = MiscEntry;

												////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE MISC FEE
												//if (Math.Round(TotalPaymentMisc, 2) >= Math.Round(ToBePaidAmount))
												//{
												//    tempAmt = ToBePaidAmount;
												//    TotalPaymentMisc -= ToBePaidAmount;
												//    LineStatusDP = "C";
												//}
												//else
												//{
												//    tempAmt = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
												//    TotalPaymentMisc -= ToBePaidAmount;
												//    TotalPaymentMisc = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
												//    LineStatusDP = "O";
												//}

											}






											//ADD NEW ROW IN QUT1 (SCHEDULE)
											if (!RPayment(DocEntry,
															  Convert.ToInt16(dr["Terms"]),
															  oDueDate,
															  Convert.ToDouble(dr["PaymentAmount"]),
															  Convert.ToDouble(dr["Penalty"]),
															  Convert.ToDouble(dr["Misc"]),
															  Convert.ToDouble(dr["InterestAmount"]),
															  Convert.ToDouble(dr["Principal"]),
															  0,
															  Convert.ToDouble(dr["Balance"]),
															  dr["PaymentType"].ToString(),
															  LineStatusDP,
															  tempAmt,
															  SAPDocEntry,
															  Convert.ToDouble(dr["IPS"].ToString()),
															  con))
											{
												isError = true;
												errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
											}



										}

									}

								}
							}
							else
							{
								Message = errmsg;
								return false;
							}
							//}
							//   }
						}

							



						//MISCELLANEOUS RELATED GRIDVIEWS
						if (RestructuringType != "AdvancePayment")
						{
							//**RECREATE DP SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
							if (oMiscDP != null)
							{
								errmsg = string.Empty;

								if (CancelDPI(DocEntry, "MISC", RSTId, con))
								{
									if (!isError)
									{
										// ** loop for inserting data to QUT1
										foreach (DataRow dr in oMiscDP.Rows)
										{

											string oConvert = dr["DueDate"].ToString();
											DateTime oDueDate = Convert.ToDateTime(oConvert);
											LineStatusDP = "O";

											double ToBePaidAmount = 0;
											double tempAmt = 0;
											string SAPDocEntry = "0";

											if (dr["PaymentType"].ToString() == "MISC")
											{
												ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);

												string qryMiscEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = 'DP'";
												DataTable dtMiscEntry = hana.GetData(qryMiscEntry, hana.GetConnection("SAOHana"));
												MiscEntry = DataAccess.GetData(dtMiscEntry, 0, "SapDocEntry", "").ToString();

												SAPDocEntry = MiscEntry;

												////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE MISC FEE
												//if (Math.Round(TotalPaymentMisc, 2) >= Math.Round(ToBePaidAmount))
												//{
												//    tempAmt = ToBePaidAmount;
												//    TotalPaymentMisc -= ToBePaidAmount;
												//    LineStatusDP = "C";
												//}
												//else
												//{
												//    tempAmt = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
												//    TotalPaymentMisc -= ToBePaidAmount;
												//    TotalPaymentMisc = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
												//    LineStatusDP = "O";
												//}

											}

											//ADD NEW ROW IN QUT1 (SCHEDULE)
											if (!RPayment(DocEntry,
															  Convert.ToInt16(dr["Terms"]),
															  oDueDate,
															  Convert.ToDouble(dr["PaymentAmount"]),
															  Convert.ToDouble(dr["Penalty"]),
															  Convert.ToDouble(dr["Misc"]),
															  Convert.ToDouble(dr["InterestAmount"]),
															  Convert.ToDouble(dr["Principal"]),
															  0,
															  Convert.ToDouble(dr["Balance"]),
															  dr["PaymentType"].ToString(),
															  LineStatusDP,
															  tempAmt,
															  SAPDocEntry,
															  Convert.ToDouble(dr["IPS"].ToString()),
															  con,
															  "DP"))
											{
												isError = true;
												errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
											}

										}
									}

								}

							}



							//**RECREATE DP SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
							if (oMiscAmort != null)
							{
								errmsg = string.Empty;

								//if (CancelDPI(DocEntry, "MISC", RSTId, con))
								//{
								if (!isError)
								{
									// ** loop for inserting data to QUT1
									foreach (DataRow dr in oMiscAmort.Rows)
									{

										string oConvert = dr["DueDate"].ToString();
										DateTime oDueDate = Convert.ToDateTime(oConvert);
										LineStatusDP = "O";

										double ToBePaidAmount = 0;
										double tempAmt = 0;
										string SAPDocEntry = "0";

										if (dr["PaymentType"].ToString() == "MISC")
										{
											ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);

											string qryMiscEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = 'LB'";
											DataTable dtMiscEntry = hana.GetData(qryMiscEntry, hana.GetConnection("SAOHana"));
											MiscEntry = DataAccess.GetData(dtMiscEntry, 0, "SapDocEntry", "").ToString();

											SAPDocEntry = MiscEntry;

											////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE MISC FEE
											//if (Math.Round(TotalPaymentMisc, 2) >= Math.Round(ToBePaidAmount))
											//{
											//    tempAmt = ToBePaidAmount;
											//    TotalPaymentMisc -= ToBePaidAmount;
											//    LineStatusDP = "C";
											//}
											//else
											//{
											//    tempAmt = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
											//    TotalPaymentMisc -= ToBePaidAmount;
											//    TotalPaymentMisc = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
											//    LineStatusDP = "O";
											//}

										}

										//ADD NEW ROW IN QUT1 (SCHEDULE)
										if (!RPayment(DocEntry,
														  Convert.ToInt16(dr["Terms"]),
														  oDueDate,
														  Convert.ToDouble(dr["PaymentAmount"]),
														  Convert.ToDouble(dr["Penalty"]),
														  Convert.ToDouble(dr["Misc"]),
														  Convert.ToDouble(dr["InterestAmount"]),
														  Convert.ToDouble(dr["Principal"]),
														  0,
														  Convert.ToDouble(dr["Balance"]),
														  dr["PaymentType"].ToString(),
														  LineStatusDP,
														  tempAmt,
														  SAPDocEntry,
														  Convert.ToDouble(dr["IPS"].ToString()),
														  con,
														  "LB"))
										{
											isError = true;
											errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
										}

									}
								}

								//}

							}

						}








						//amoritization schedule
						if (oAmort != null)
						{
							//if (FinancingScheme != oldFinancingScheme ||
							//    ProductType != oldProductType ||
							//    Lot != oldLot ||
							//    Model != oldHouseModel ||
							//    ProjCode != oldProject ||
							//    oCardCode != oldCardCode ||
							//    RetitlingType != oldRetitlingType)
							//{

							errmsg = string.Empty;


							//CANCEL ALL SCHEDULE ROWS IN QUT1 (LB PAYMENT TYPES)
							if (CancelDPI(DocEntry, "LB", RSTId, con))
							{

								foreach (DataRow dr in oAmort.Rows)
								{
									string oConvert = dr["DueDate"].ToString();
									DateTime oDueDate = Convert.ToDateTime(oConvert);
									//LineStatusDP = "O";

									//double ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);
									double tempAmt = 0;


									//2024-10-05: ADD TO AMORTIZATION TABLE THE ADVANCE PAYMENT ADDED
									if (Convert.ToInt16(MinLBTerm) == Convert.ToInt16(dr["Terms"]) &&
										Convert.ToDateTime(MinLBDate) >= oDueDate
										)
									{
										if (!RPayment(DocEntry,
										   Convert.ToInt16(MinLBTerm),
										   Convert.ToDateTime(oDate),
										   0,
										   0,
										   0,
										   0,
										   AdvancePayment,
										   0,
										   0,
										   "LB",
										   "A",
										   AdvancePayment,
										   PaymentEntry.ToString(),
										   0,
										   con))
										{
											isError = true;
											errmsg = "Error on saving LB schedule locally. Please contact administrator.";
										}
									}


									string qryLBEntry = $@"SELECT IFNULL(""SapDocEntry"",0) ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = ''";
									DataTable dtLBEntry = hana.GetData(qryLBEntry, hana.GetConnection("SAOHana"));
									DPDocEntry = DataAccess.GetData(dtLBEntry, 0, "SapDocEntry", "").ToString();





									if (!RPayment(DocEntry,
									   Convert.ToInt16(dr["Terms"]),
									   oDueDate,
									   Convert.ToDouble(dr["PaymentAmount"]),
									   Convert.ToDouble(dr["Penalty"]),
									   Convert.ToDouble(dr["Misc"]),
									   Convert.ToDouble(dr["InterestAmount"]),
									   Convert.ToDouble(dr["Principal"]),
									   0,
									   Convert.ToDouble(dr["Balance"]),
									   "LB",
									   LineStatusDP,
									   tempAmt,
									   DPDocEntry,
									   Convert.ToDouble(dr["IPS"].ToString()),
									   con))
									{
										isError = true;
										errmsg = "Error on saving LB schedule locally. Please contact administrator.";
									}
									//else
									//{
									//    Balance = Balance - PaymentAmount;
									//}

								}
							}
							else
							{
								Message = errmsg;
								return false;
							}
							//}
						}


					}

















					else
					{

						if (DPTerms != oldDPTerms || LTerms != oldLBTerms || MiscDPTerms != oldMiscTerms)
						{
							//if (RestructureAction == "NEW")
							//{

							//**RECREATE DP SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
							if (oDownpayment != null)
							{
								//if (FinancingScheme != oldFinancingScheme ||
								//    ProductType != oldProductType ||
								//    Lot != oldLot ||
								//    Model != oldHouseModel ||
								//    ProjCode != oldProject ||
								//    oCardCode != oldCardCode ||
								//    RetitlingType != oldRetitlingType)
								//{

								errmsg = string.Empty;

								//CANCEL ALL SCHEDULE ROWS IN QUT1 (DP, MISC AND RES PAYMENT TYPES)
								if (CancelDPI(DocEntry, "DP", RSTId, con))
								{
									////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE RESERVATION FEE   
									//if (Math.Round(TotalPayment, 2) >= Math.Round(ResrvFee, 2))
									//{
									//    LineStatusDP = "C";
									//    TotalPayment -= ResrvFee;
									//    tempResPayment = ResrvFee;
									//}
									//else
									//{
									//    LineStatusDP = "O";
									//    tempResPayment = TotalPayment;
									//    TotalPayment -= ResrvFee;
									//}


									//ADD NEW ROW IN QUT1 (SCHEDULE) -- RESERVATION
									//if (!RPayment(DocEntry,
									//               1,
									//              oDate,
									//              ResrvFee,
									//              0,
									//              0,
									//              0,
									//              ResrvFee,
									//              0,
									//              0,
									//              "RES",
									//              LineStatusDP,
									//              tempResPayment,
									//              QuotationDocEntry,
									//              0,
									//              con))
									//{
									//    isError = true;
									//    errmsg = "Error on saving Reservation schedule locally. Please contact administrator.";
									//}









									if (!isError)
									{
										// ** loop for inserting data to QUT1
										foreach (DataRow dr in oDownpayment.Rows)
										{

											string oConvert = dr["DueDate"].ToString();
											DateTime oDueDate = Convert.ToDateTime(oConvert);
											LineStatusDP = "O";

											double ToBePaidAmount = 0;
											double tempAmt = 0;

											string SAPDocEntry = "0";

											//CHECK IF PAYMENT TYPE IS DP (MISC DOESNT HAVE ANY SAP DOC ENTRIES)
											if (dr["PaymentType"].ToString() == "DP")
											{
												ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);


												string qryDPEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = ''";
												DataTable dtDPEntry = hana.GetData(qryDPEntry, hana.GetConnection("SAOHana"));
												DPDocEntry = DataAccess.GetData(dtDPEntry, 0, "SapDocEntry", "").ToString();



												SAPDocEntry = DPDocEntry;


												////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE DP
												//if (Math.Round(TotalPayment, 2) >= Math.Round(ToBePaidAmount, 2))
												//{
												//    tempAmt = ToBePaidAmount;
												//    TotalPayment -= ToBePaidAmount;
												//    LineStatusDP = "C";
												//}
												//else
												//{
												//    tempAmt = (TotalPayment <= 0 ? 0 : TotalPayment);
												//    TotalPayment -= ToBePaidAmount;
												//    TotalPayment = (TotalPayment <= 0 ? 0 : TotalPayment);
												//    LineStatusDP = "O";
												//}

											}
											//else if (dr["PaymentType"].ToString() == "MISC")
											//{
											//    ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);
											//    SAPDocEntry = MiscEntry;

											//    ////CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE MISC FEE
											//    //if (Math.Round(TotalPaymentMisc, 2) >= Math.Round(ToBePaidAmount))
											//    //{
											//    //    tempAmt = ToBePaidAmount;
											//    //    TotalPaymentMisc -= ToBePaidAmount;
											//    //    LineStatusDP = "C";
											//    //}
											//    //else
											//    //{
											//    //    tempAmt = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
											//    //    TotalPaymentMisc -= ToBePaidAmount;
											//    //    TotalPaymentMisc = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
											//    //    LineStatusDP = "O";
											//    //}

											//}






											//ADD NEW ROW IN QUT1 (SCHEDULE)
											if (!RPayment(DocEntry,
															  Convert.ToInt16(dr["Terms"]),
															  oDueDate,
															  Convert.ToDouble(dr["PaymentAmount"]),
															  Convert.ToDouble(dr["Penalty"]),
															  Convert.ToDouble(dr["Misc"]),
															  Convert.ToDouble(dr["InterestAmount"]),
															  Convert.ToDouble(dr["Principal"]),
															  0,
															  Convert.ToDouble(dr["Balance"]),
															  dr["PaymentType"].ToString(),
															  LineStatusDP,
															  tempAmt,
															  SAPDocEntry,
															  Convert.ToDouble(dr["IPS"].ToString()),
															  con))
											{
												isError = true;
												errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
											}



										}





									}
								}
								else
								{
									Message = errmsg;
									return false;
								}
								//}
							}





							if (RestructuringType != "AdvancePayment")
							{
								//**RECREATE MISC SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
								if (oMiscDP != null)
								{
									errmsg = string.Empty;

									if (CancelDPI(DocEntry, "MISC", RSTId, con))
									{

										if (!isError)
										{
											// ** loop for inserting data to QUT1
											foreach (DataRow dr in oMiscDP.Rows)
											{
												string oConvert = dr["DueDate"].ToString();
												DateTime oDueDate = Convert.ToDateTime(oConvert);
												LineStatusDP = "O";

												double ToBePaidAmount = 0;
												double tempAmt = 0;

												string SAPDocEntry = "0";

												if (dr["PaymentType"].ToString() == "MISC")
												{
													ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);


													string qryMiscEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = 'LB'";
													DataTable dtMiscEntry = hana.GetData(qryMiscEntry, hana.GetConnection("SAOHana"));
													MiscEntry = DataAccess.GetData(dtMiscEntry, 0, "SapDocEntry", "").ToString();


													SAPDocEntry = MiscEntry;
												}


												//ADD NEW ROW IN QUT1 (SCHEDULE)
												if (!RPayment(DocEntry,
																  Convert.ToInt16(dr["Terms"]),
																  oDueDate,
																  Convert.ToDouble(dr["PaymentAmount"]),
																  Convert.ToDouble(dr["Penalty"]),
																  Convert.ToDouble(dr["Misc"]),
																  Convert.ToDouble(dr["InterestAmount"]),
																  Convert.ToDouble(dr["Principal"]),
																  0,
																  Convert.ToDouble(dr["Balance"]),
																  dr["PaymentType"].ToString(),
																  LineStatusDP,
																  tempAmt,
																  SAPDocEntry,
																  Convert.ToDouble(dr["IPS"].ToString()),
																  con,
																  "LB"))
												{
													isError = true;
													errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
												}


											}
										}
									}

								}



								//**RECREATE MISC SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
								if (oMiscAmort != null)
								{
									errmsg = string.Empty;

									//if (CancelDPI(DocEntry, "MISC", RSTId, con))
									//{

									if (!isError)
									{
										// ** loop for inserting data to QUT1
										foreach (DataRow dr in oMiscAmort.Rows)
										{
											string oConvert = dr["DueDate"].ToString();
											DateTime oDueDate = Convert.ToDateTime(oConvert);
											LineStatusDP = "O";

											double ToBePaidAmount = 0;
											double tempAmt = 0;

											string SAPDocEntry = "0";

											if (dr["PaymentType"].ToString() == "MISC")
											{
												ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);

												string qryMiscEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = 'LB'";
												DataTable dtMiscEntry = hana.GetData(qryMiscEntry, hana.GetConnection("SAOHana"));
												MiscEntry = DataAccess.GetData(dtMiscEntry, 0, "SapDocEntry", "").ToString();

												SAPDocEntry = MiscEntry;
											}


											//ADD NEW ROW IN QUT1 (SCHEDULE)
											if (!RPayment(DocEntry,
															  Convert.ToInt16(dr["Terms"]),
															  oDueDate,
															  Convert.ToDouble(dr["PaymentAmount"]),
															  Convert.ToDouble(dr["Penalty"]),
															  Convert.ToDouble(dr["Misc"]),
															  Convert.ToDouble(dr["InterestAmount"]),
															  Convert.ToDouble(dr["Principal"]),
															  0,
															  Convert.ToDouble(dr["Balance"]),
															  dr["PaymentType"].ToString(),
															  LineStatusDP,
															  tempAmt,
															  SAPDocEntry,
															  Convert.ToDouble(dr["IPS"].ToString()),
															  con,
															  "LB"))
											{
												isError = true;
												errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
											}


										}
									}
									//}

								}
							}

							//amoritization schedule
							if (oAmort != null)
							{
								//if (FinancingScheme != oldFinancingScheme ||
								//    ProductType != oldProductType ||
								//    Lot != oldLot ||
								//    Model != oldHouseModel ||
								//    ProjCode != oldProject ||
								//    oCardCode != oldCardCode ||
								//    RetitlingType != oldRetitlingType)
								//{
								errmsg = string.Empty;


								//CANCEL ALL SCHEDULE ROWS IN QUT1 (LB PAYMENT TYPES)
								if (CancelDPI(DocEntry, "LB", RSTId, con))
								{

									foreach (DataRow dr in oAmort.Rows)
									{
										string oConvert = dr["DueDate"].ToString();
										DateTime oDueDate = Convert.ToDateTime(oConvert);
										LineStatusDP = "O";

										double ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);
										double tempAmt = 0;



										//if (Math.Round(TotalPayment, 2) >= Math.Round(ToBePaidAmount, 2))
										//{
										//    tempAmt = ToBePaidAmount;
										//    TotalPayment -= ToBePaidAmount;
										//    LineStatusDP = "C";
										//}
										//else
										//{
										//    tempAmt = (TotalPayment <= 0 ? 0 : TotalPayment); ;
										//    TotalPayment -= ToBePaidAmount;
										//    TotalPayment = (TotalPayment <= 0 ? 0 : TotalPayment);
										//    LineStatusDP = "O";
										//}

										string qryDPEntry = $@"SELECT ""SapDocEntry"" FROM QUT1 WHERE ""DocEntry"" = {DocEntry} AND ""Terms"" = '{dr["Terms"].ToString()}' AND 
                                                                            ""PaymentType"" = '{dr["PaymentType"].ToString()}' AND IFNULL(""MiscType"", '') = ''";
										DataTable dtDPEntry = hana.GetData(qryDPEntry, hana.GetConnection("SAOHana"));
										DPDocEntry = DataAccess.GetData(dtDPEntry, 0, "SapDocEntry", "").ToString();

										if (!RPayment(DocEntry,
											Convert.ToInt16(dr["Terms"]),
											oDueDate,
											Convert.ToDouble(dr["PaymentAmount"]),
											Convert.ToDouble(dr["Penalty"]),
											Convert.ToDouble(dr["Misc"]),
											Convert.ToDouble(dr["InterestAmount"]),
											Convert.ToDouble(dr["Principal"]),
											0,
											Convert.ToDouble(dr["Balance"]),
											"LB",
											LineStatusDP,
											tempAmt,
											DPDocEntry,
											Convert.ToDouble(dr["IPS"].ToString()),
											con))
										{
											isError = true;
											errmsg = "Error on saving LB schedule locally. Please contact administrator.";
										}
										//else
										//{
										//    Balance = Balance - PaymentAmount;
										//}
									}
								}
								else
								{
									Message = errmsg;
									return false;
								}
								//}
							}

						}


						#region with reservation posting





						////**RECREATE DP SCHEDULE IF COUNT IS DIFFERENT FROM THE CURRENT
						//if (oDownpayment != null)
						//{
						//    string errmsg = string.Empty;



						//    //CANCEL ALL SCHEDULE ROWS IN QUT1 (DP, MISC AND RES PAYMENT TYPES)
						//    if (CancelDPI(DocEntry, "DP", RSTId, con))
						//    {


						//        //CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE RESERVATION FEE   
						//        if (Math.Round(TotalPayment, 2) >= Math.Round(ResrvFee, 2))
						//        {
						//            LineStatusDP = "C";
						//            TotalPayment -= ResrvFee;
						//            tempResPayment = ResrvFee;
						//        }
						//        else
						//        {
						//            LineStatusDP = "O";
						//            tempResPayment = TotalPayment;
						//            TotalPayment -= ResrvFee;
						//        }


						//        //ADD NEW ROW IN QUT1(SCHEDULE)-- RESERVATION
						//        if (!RPayment(DocEntry,
						//                       1,
						//                      oDate,
						//                      ResrvFee,
						//                      0,
						//                      0,
						//                      0,
						//                      ResrvFee,
						//                      0,
						//                      0,
						//                      "RES",
						//                      LineStatusDP,
						//                      tempResPayment,
						//                      QuotationDocEntry,
						//                      0,
						//                      con))
						//        {
						//            isError = true;
						//            errmsg = "Error on saving Reservation schedule locally. Please contact administrator.";
						//        }




						//        if (!isError)
						//        {
						//            // ** loop for inserting data to QUT1
						//            foreach (DataRow dr in oDownpayment.Rows)
						//            {

						//                string oConvert = dr["DueDate"].ToString();
						//                DateTime oDueDate = Convert.ToDateTime(oConvert);
						//                LineStatusDP = "O";

						//                double ToBePaidAmount = 0;
						//                double tempAmt = 0;

						//                string SAPDocEntry = "0";

						//                //CHECK IF PAYMENT TYPE IS DP (MISC DOESNT HAVE ANY SAP DOC ENTRIES)
						//                if (dr["PaymentType"].ToString() == "DP")
						//                {
						//                    ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);
						//                    SAPDocEntry = DPDocEntry;


						//                    //CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE DP
						//                    if (Math.Round(TotalPayment, 2) >= Math.Round(ToBePaidAmount, 2))
						//                    {
						//                        tempAmt = ToBePaidAmount;
						//                        TotalPayment -= ToBePaidAmount;
						//                        LineStatusDP = "C";
						//                    }
						//                    else
						//                    {
						//                        tempAmt = (TotalPayment <= 0 ? 0 : TotalPayment);
						//                        TotalPayment -= ToBePaidAmount;
						//                        TotalPayment = (TotalPayment <= 0 ? 0 : TotalPayment);
						//                        LineStatusDP = "O";
						//                    }

						//                }
						//                else if (dr["PaymentType"].ToString() == "MISC")
						//                {
						//                    ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]);
						//                    SAPDocEntry = MiscEntry;

						//                    //CHECK IF PAYMENT IS SUFFICIENT TO PAY THE WHOLE MISC FEE
						//                    if (Math.Round(TotalPaymentMisc, 2) >= Math.Round(ToBePaidAmount))
						//                    {
						//                        tempAmt = ToBePaidAmount;
						//                        TotalPaymentMisc -= ToBePaidAmount;
						//                        LineStatusDP = "C";
						//                    }
						//                    else
						//                    {
						//                        tempAmt = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
						//                        TotalPaymentMisc -= ToBePaidAmount;
						//                        TotalPaymentMisc = (TotalPaymentMisc <= 0 ? 0 : TotalPaymentMisc);
						//                        LineStatusDP = "O";
						//                    }
						//                }






						//                //ADD NEW ROW IN QUT1 (SCHEDULE)
						//                if (!RPayment(DocEntry,
						//                                  Convert.ToInt16(dr["Terms"]),
						//                                  oDueDate,
						//                                  Convert.ToDouble(dr["PaymentAmount"]),
						//                                  Convert.ToDouble(dr["Penalty"]),
						//                                  Convert.ToDouble(dr["Misc"]),
						//                                  Convert.ToDouble(dr["InterestAmount"]),
						//                                  Convert.ToDouble(dr["Principal"]),
						//                                  0,
						//                                  Convert.ToDouble(dr["Balance"]),
						//                                  dr["PaymentType"].ToString(),
						//                                  LineStatusDP,
						//                                  tempAmt,
						//                                  SAPDocEntry,
						//                                  Convert.ToDouble(dr["IPS"].ToString()),
						//                                  con))
						//                {
						//                    isError = true;
						//                    errmsg = $@"Error on saving {dr["PaymentType"].ToString()} schedule locally. Please contact administrator.";
						//                }



						//            }





						//        }
						//    }
						//    else
						//    {
						//        Message = errmsg;
						//        return false;
						//    }
						//}












						////amoritization schedule
						//if (oAmort != null)
						//{
						//    string errmsg = string.Empty;


						//    //CANCEL ALL SCHEDULE ROWS IN QUT1 (LB PAYMENT TYPES)
						//    if (CancelDPI(DocEntry, "LB", RSTId, con))
						//    {

						//        foreach (DataRow dr in oAmort.Rows)
						//        {
						//            string oConvert = dr["DueDate"].ToString();
						//            DateTime oDueDate = Convert.ToDateTime(oConvert);
						//            LineStatusDP = "O";

						//            double ToBePaidAmount = Convert.ToDouble(dr["PaymentAmount"]) + Convert.ToDouble(dr["IPS"]);
						//            double tempAmt = 0;



						//            if (Math.Round(TotalPayment, 2) >= Math.Round(ToBePaidAmount, 2))
						//            {
						//                tempAmt = ToBePaidAmount;
						//                TotalPayment -= ToBePaidAmount;
						//                LineStatusDP = "C";
						//            }
						//            else
						//            {
						//                tempAmt = (TotalPayment <= 0 ? 0 : TotalPayment); ;
						//                TotalPayment -= ToBePaidAmount;
						//                TotalPayment = (TotalPayment <= 0 ? 0 : TotalPayment);
						//                LineStatusDP = "O";
						//            }



						//            if (!RPayment(DocEntry,
						//                Convert.ToInt16(dr["Terms"]),
						//                oDueDate,
						//                Convert.ToDouble(dr["PaymentAmount"]),
						//                Convert.ToDouble(dr["Penalty"]),
						//                Convert.ToDouble(dr["Misc"]),
						//                Convert.ToDouble(dr["InterestAmount"]),
						//                Convert.ToDouble(dr["Principal"]),
						//                0,
						//                Convert.ToDouble(dr["Balance"]),
						//                "LB",
						//                LineStatusDP,
						//                tempAmt,
						//                DPDocEntry,
						//                Convert.ToDouble(dr["IPS"].ToString()),
						//                con))
						//            {
						//                isError = true;
						//                errmsg = "Error on saving LB schedule locally. Please contact administrator.";
						//            }
						//        }
						//    }
						//    else
						//    {
						//        Message = errmsg;
						//        return false;
						//    }
						//}
						#endregion

					}

				}







				//if (!isError)
				//{

				//    int newIPSTerms = DPTerms - IPSTermTag;
				//    double ipsAmount = IPS / newIPSTerms;

				//    if (IPS > 0)
				//    {
				//        //INSERT IPS AMOUNT ON SCHEDULE
				//        string qryUpdateIPS = $@" UPDATE QUT1 SET ""IPS"" = {ipsAmount} WHERE ""DocEntry"" = {DocEntry} AND ""LineStatus"" = 'O' AND ""Terms"" IN ({IPSTerms.Remove(IPSTerms.Length - 1, 1)})";
				//        if (!hana.Execute(qryUpdateIPS, hana.GetConnection("SAOHana")))
				//        {
				//            isError = true;
				//            Message = "Error on saving IP&S amount on schedule locally. Please contact administrator.";
				//        }
				//    }

				//}




				//}







				//UPDATING OF SCHEDULE'S DATE IF CONTRACT GOT LTSNo
				if (RestructuringType == ConfigSettings.RestructuringWithLTS)
				{
					// for reservation
					ForLTSIssuance(oDate.ToString("yyyy/MM/dd"), DocEntry, 1, "RES");


					// ** loop for upating data to QUT1
					foreach (DataRow dr in oDownpayment.Rows)
					{
						string DueDate = dr["DueDate"].ToString();
						int Terms = Convert.ToInt32(dr["Terms"].ToString());
						string PaymentType = dr["PaymentType"].ToString();

						ForLTSIssuance(DueDate, DocEntry, Terms, PaymentType);

					}

				}


				if (isError == false)
				{
					//trans.Commit();
					con.Close();

					Message = "";
					return true;
				}
				else
				{
					//trans.Rollback(); 

					//ROLLBACK POSTED TRANSACTIONS OF ORST
					string qryUpdateQuotationStatus = $@"delete from orst where ""RSTDocEntry"" = {DPEntry} ";
					bool update = hana.Execute(qryUpdateQuotationStatus, hana.GetConnection("SAOHana"));

					con.Close();
					Message = "Error in saving restructuring";
					return false;
				}
				//    }
				//}
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return false;
			}
		}



		void ForLTSIssuance(string oDueDate,
							int oDocEntry,
							int oTerms,
							string oPaymentType)
		{
			try
			{
				string query = "sp_ForLTSIssuance";

				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DueDate", Convert.ToDateTime(oDueDate).ToString("yyyy-MM-dd"));
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						cmd.Parameters.AddWithValue("Terms", oTerms);
						cmd.Parameters.AddWithValue("PaymentType", oPaymentType);
						con.Open();
						cmd.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex) { }
		}




		[WebMethod]
		public DataSet GetCompSheetRestructure(int Age, int DocEntry,  //2
												string FinancingScheme, string HouseStatus, //4
												double ODAS, double OMisc, //6
												double OVat, double PromoDisc, //8 
												double SpotDPAmt, double DPPercent,//10 
												double ResrvFee, int DPTerms, //12
												int LTerms, double Payments,  //14
												string Stage, string TotalInterest, //16
												string ComputeArrears,
												double discAmount, string discountBased, //18
												int discPercent, double ACAmount, //20
												string Allowed, string SalesType,  //22
												string Model, string Project,  //24
												int UpdatedDPDueDate, string DocDate, //26
												string DPDay, string dtpDueDate,  //28
												int dtpDueDateVisible,
												string adjacent
												)
		{
			DataSet ret = null;
			try
			{
				string query = "sp_GetCompSheetRestructure";
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand(query, con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
								cmd.Parameters.AddWithValue("@DocEntry", DocEntry); //2
								cmd.Parameters.AddWithValue("@Age", Age);
								cmd.Parameters.AddWithValue("@FinancingScheme", FinancingScheme); //4
								cmd.Parameters.AddWithValue("@HouseStatus", HouseStatus);
								cmd.Parameters.AddWithValue("@ODAS", ODAS); //6
								cmd.Parameters.AddWithValue("@OMisc", OMisc);
								cmd.Parameters.AddWithValue("@OVat", OVat); //8
								cmd.Parameters.AddWithValue("@PromoDisc", PromoDisc);
								cmd.Parameters.AddWithValue("@SpotDPAmt", SpotDPAmt); //10
								cmd.Parameters.AddWithValue("@DPPercent", DPPercent);
								cmd.Parameters.AddWithValue("@ResrvFee", ResrvFee); //12
								cmd.Parameters.AddWithValue("@DPTerms", DPTerms);
								cmd.Parameters.AddWithValue("@LTerms", LTerms);  //14
								cmd.Parameters.AddWithValue("@Payments", Payments);
								cmd.Parameters.AddWithValue("@Stage", Stage); //16
								cmd.Parameters.AddWithValue("@AddtlChargeBuffer", double.Parse(ConfigSettings.AddtlChargesBuffer));
								cmd.Parameters.AddWithValue("@TotalInterest", Convert.ToDouble(TotalInterest)); //18
								cmd.Parameters.AddWithValue("@ComputeArrears", ComputeArrears);
								//PARAMETERS FOR COMPSHEET()
								cmd.Parameters.AddWithValue("@newSpotDPDiscAmount", discAmount);
								cmd.Parameters.AddWithValue("@newdiscountBased", discountBased);
								cmd.Parameters.AddWithValue("@newDPAmount", SpotDPAmt);
								cmd.Parameters.AddWithValue("@newDiscPercent", discPercent);
								cmd.Parameters.AddWithValue("@newChargeAmt", ACAmount);
								cmd.Parameters.AddWithValue("@newAllowed", Allowed);
								cmd.Parameters.AddWithValue("@newSalesType", SalesType);
								cmd.Parameters.AddWithValue("@newModel", Model);
								cmd.Parameters.AddWithValue("@newProject", Project);
								cmd.Parameters.AddWithValue("@UpdatedDPDueDate", UpdatedDPDueDate);
								cmd.Parameters.AddWithValue("@DocDate", DocDate);
								cmd.Parameters.AddWithValue("@DPDay", DPDay);
								cmd.Parameters.AddWithValue("@dtpDueDate", dtpDueDate);
								cmd.Parameters.AddWithValue("@dtpDueDateVisible", dtpDueDateVisible);


								da.Fill(ds, "GetCompSheetRestructure");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		public bool CancelDPI(int DocEntry, string Type, int RSTId, HanaConnection con)
		{
			try
			{
				using (HanaCommand cmd = new HanaCommand("CancelDPI", con))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("DocEntry", DocEntry);
					cmd.Parameters.AddWithValue("Type", Type);
					cmd.Parameters.AddWithValue("RSTId", RSTId);
					cmd.ExecuteNonQuery();
					return true;
				}
				//using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				//{

				//}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		bool RPayment(int oDocEntry,
					  int oTerms, //2
					  DateTime oDueDate,
					  double oPaymentAmount, //4
					  double oPenalty,
					  double oMisc, //6
					  double oInterestRate,
					  double oPrincipal, //8
					  double oUnAll,
					  double oBalance, //10
					  string oPaymentType,
					  string LineStatus, //12
					  double oAmountPaid,
					  string SAPDocEntry, //14
					  double IPSAmount,
					  HanaConnection con,
					  string MiscType = "") //16  
		{
			try
			{
				string query = "sp_RPayment";
				using (HanaCommand cmd = new HanaCommand(query, con))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
					cmd.Parameters.AddWithValue("Terms", oTerms);
					cmd.Parameters.AddWithValue("DueDate", oDueDate);
					cmd.Parameters.AddWithValue("PaymentAmount", oPaymentAmount);
					cmd.Parameters.AddWithValue("Penalty", oPenalty);
					cmd.Parameters.AddWithValue("Misc", oMisc);
					cmd.Parameters.AddWithValue("InterestRate", oInterestRate);
					cmd.Parameters.AddWithValue("Principal", oPrincipal);
					cmd.Parameters.AddWithValue("UnAll", oUnAll);
					cmd.Parameters.AddWithValue("Balance", oBalance);
					cmd.Parameters.AddWithValue("PaymentType", oPaymentType);
					cmd.Parameters.AddWithValue("LineStatus", LineStatus);
					cmd.Parameters.AddWithValue("AmountPaid", oAmountPaid);
					cmd.Parameters.AddWithValue("SapDocEntry", SAPDocEntry);
					cmd.Parameters.AddWithValue("IPSAmount", IPSAmount);
					cmd.Parameters.AddWithValue("MiscType", MiscType);
					cmd.ExecuteNonQuery();
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		[WebMethod]
		public DataSet Cancellation(int SQEntry, string Type)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("Cancellation", con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
								//cmd.Parameters.AddWithValue("@Database", $"{DataAccess.GetconDetails("SAP", "Data Source")}].[{DataAccess.GetconDetails("SAP", "Initial Catalog")}");
								cmd.Parameters.AddWithValue("@DocEntry", SQEntry);
								cmd.Parameters.AddWithValue("@Type", Type);
								da.Fill(ds, "Cancellation");
								return ds;
							}
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}

		public DataSet GetAccountComputedTotals(int @DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetAccountComputedTotals", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								da.Fill(ds, "GetAccountComputedTotals");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		#endregion

		#region REPORTS
		[WebMethod]
		public DataSet GetReportPerGroup(string group, string LOI)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetReportPerGroup", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Group1", group);
						cmd.Parameters.AddWithValue("LOI", LOI);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetReportPerGroup");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}
		[WebMethod]
		public DataSet GetPaymentHistory(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("PaymentHistory", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "PaymentHistory");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		//2023-07-02: GET NEW PAYMENT HISTORY
		[WebMethod]
		public DataSet GetPaymentHistoryNew(int DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAPHana")))
				{
					using (HanaCommand cmd = new HanaCommand($@"CALL USRSP_DBTI_SAO_PAYMENTHISTORY ('WHERE QT.""DocEntry"" = ''{DocEntry}''' )", con))
					{
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "PaymentHistory");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetPaymentHistorySearch(int DocEntry, string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetPaymentHistorySearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetPaymentHistorySearch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		public DataSet ForHouseConstruction()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("ForHouseConstruction", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "ForHouseConstruction");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		#endregion

		#region STATUS
		[WebMethod]
		public DataSet GetStatus(string Role)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetStatus", con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@Role", Role);
								da.Fill(ds, "GetStatus");
								return ds;
							}
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}
		[WebMethod]
		public DataSet GetSubStatus(int Id)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetSubStatus", con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@StatusId", Id);
								da.Fill(ds, "GetSubStatus");
								return ds;
							}
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}
		[WebMethod]
		public DataSet GetDiary(int DocEntry, string Role)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetDiary", con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
								cmd.Parameters.AddWithValue("@Role", Role);
								da.Fill(ds, "GetDiary");
								return ds;
							}
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}
		[WebMethod]
		public bool AddDiary(int Id, int DocEntry, string Role, int StatusId, int SubStatusId, string Remarks, string Date, out string Message)
		{
			try
			{

				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("AddDiary", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Id", Id);
						cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("@Role", Role);
						cmd.Parameters.AddWithValue("@StatusId", StatusId);
						cmd.Parameters.AddWithValue("@SubStatusId", SubStatusId);
						cmd.Parameters.AddWithValue("@Remarks", Remarks);
						cmd.Parameters.AddWithValue("@Date", Date);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
					}
				}
				Message = string.Empty;
				return true;
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return false;
			}
		}
		[WebMethod]
		public bool RemoveDiary(int Id, out string Message)
		{
			try
			{

				using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
				{
					using (SqlCommand cmd = new SqlCommand("RemoveDiary", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Id", Id);
						con.Open();
						cmd.ExecuteNonQuery();
						con.Close();
					}
				}
				Message = string.Empty;
				return true;
			}
			catch (Exception ex)
			{
				Message = ex.Message;
				return false;
			}
		}

		public DataSet GetRemainingTerms(int DocEntry)
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (SqlConnection con = new SqlConnection(DataAccess.con("Addon")))
					{
						using (SqlCommand cmd = new SqlCommand("GetRemainingTerms", con))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("@DocEntry", DocEntry);
								da.Fill(ds, "GetRemainingTerms");
								return ds;
							}
						}
					}
				}
			}
			catch
			{
				return null;
			}
		}
		public DataSet CleanupQuotationReservation(string DFrom,
												   string DTo,
												   string Type = "")
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("CleanupQuotationReservation", con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								cmd.Parameters.AddWithValue("DateFrom", Convert.ToDateTime(DFrom).ToString("yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("DateTo", Convert.ToDateTime(DTo).ToString("yyyy-MM-dd"));
								cmd.Parameters.AddWithValue("Type", Type);
								da.Fill(ds, "CleanupQuotationReservation");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public DataSet CleanBuyer()
		{
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("CleanBuyer", con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								cmd.CommandType = CommandType.StoredProcedure;
								da.Fill(ds, "CleanBuyer");
								return ds;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		#endregion


		#region BROKER
		//Add by Karl
		//01/12/2021
		[WebMethod]
		public string BrokerExternal(
									 string BrokerId,
									 string TypeOfBusiness, //2
									 string ATPDate,
									 string Partnership, //4
									 string SECRegNo,

									 string FirstName, //6
									 string MiddleName,
									 string LastName, //8
									 string Nickname,
									 string Address, //10

									 string City,
									 int ZipCode, //12
									 string NatureOfBusiness,
									 string BusinessName, //14
									 string BusinessAddress,
									 int BusinessZipCode, //16
									 string BusinessPhoneNo,
									 string FaxNo, //18

									 string EmailAddress,
									 string Birthday, //20
									 string PlaceOfBirth,
									 string Religion, //22
									 string Citizenship,
									 string Tax, //24
									 string SSS,

									 string Passport, //26
									 DateTime PassportValidFrom,
									 string IssuedBy, //28
									 string PlacedIssued,

									 string PRCRegis, //30
									 DateTime PRCLicenseValid,
									 string PTRNo, //32
									 DateTime validFrom,
									 DateTime validTo, //34
									 string Status,

									 string BrokerApplicationForm, //36
									 string ListOfAccredited,
									 string AccreditationAgreement, //38
									 string BrokerAccreditationGenrealPolicies,

									 DateTime CreatedDate, //40
									 DateTime UpdateDate,
									 string Function,

									string ResidenceNo,
									string MobileNo,
									string Sex,
									DateTime PassportValidTo,
									string Spouse,
									string CivilStatus,
									string PRCLicenseRegistration,
									string AIPOOrganization,
									DateTime AIPOValidFrom,
									DateTime AIPOValidTo,
									string AIPOReceiptNo,
									string Designation
									)
		{
			string ret;
			try
			{
				string query = "sp_AddBrokerExternal";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						//cmd.Parameters.AddWithValue("Id", 1);
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);
						cmd.Parameters.AddWithValue("TypeOfBusiness", TypeOfBusiness);
						cmd.Parameters.AddWithValue("ATPDate", ATPDate);
						cmd.Parameters.AddWithValue("Partnership", Partnership);
						cmd.Parameters.AddWithValue("SECRegNo", SECRegNo);

						cmd.Parameters.AddWithValue("FirstName", FirstName);
						cmd.Parameters.AddWithValue("MiddleName", MiddleName);
						cmd.Parameters.AddWithValue("LastName", LastName);
						cmd.Parameters.AddWithValue("Nickname", Nickname);
						cmd.Parameters.AddWithValue("Address", Address);


						cmd.Parameters.AddWithValue("City", City);
						cmd.Parameters.AddWithValue("ZipCode", ZipCode);
						cmd.Parameters.AddWithValue("NatureOfBusiness", NatureOfBusiness);
						cmd.Parameters.AddWithValue("BusinessName", BusinessName);
						cmd.Parameters.AddWithValue("BusinessAddress", BusinessAddress);
						cmd.Parameters.AddWithValue("BusinessZipCode", BusinessZipCode);
						cmd.Parameters.AddWithValue("BusinessPhoneNo", BusinessPhoneNo);
						cmd.Parameters.AddWithValue("FaxNo", FaxNo);

						cmd.Parameters.AddWithValue("EmailAddress", EmailAddress);
						cmd.Parameters.AddWithValue("Birthday", Birthday);
						cmd.Parameters.AddWithValue("PlaceOfBirth", PlaceOfBirth);
						cmd.Parameters.AddWithValue("Religion", Religion);
						cmd.Parameters.AddWithValue("Citizenship", Citizenship);
						cmd.Parameters.AddWithValue("Tax", Tax);
						cmd.Parameters.AddWithValue("SSS", SSS);

						cmd.Parameters.AddWithValue("Passport", Passport);
						cmd.Parameters.AddWithValue("PassportValidFrom", PassportValidFrom);
						cmd.Parameters.AddWithValue("IssuedBy", IssuedBy);
						cmd.Parameters.AddWithValue("PlacedIssued", PlacedIssued);

						cmd.Parameters.AddWithValue("PRCRegNum", PRCRegis);
						cmd.Parameters.AddWithValue("PRCLicenseValid", PRCLicenseValid);
						cmd.Parameters.AddWithValue("PTRNo", PTRNo);
						cmd.Parameters.AddWithValue("ValidFrom", validFrom);
						cmd.Parameters.AddWithValue("ValidTo", validTo);
						cmd.Parameters.AddWithValue("Status", Status);

						cmd.Parameters.AddWithValue("BrokerApplicationForm", BrokerApplicationForm);
						cmd.Parameters.AddWithValue("ListOfAccredited", ListOfAccredited);
						cmd.Parameters.AddWithValue("AccreditationAgreement", AccreditationAgreement);
						cmd.Parameters.AddWithValue("BrokerAccreditationGenrealPolicies", BrokerAccreditationGenrealPolicies);

						cmd.Parameters.AddWithValue("CreatedDate", CreatedDate);
						cmd.Parameters.AddWithValue("UpdateDate", UpdateDate);
						cmd.Parameters.AddWithValue("Func", Function);

						cmd.Parameters.AddWithValue("ResidenceNo", ResidenceNo);
						cmd.Parameters.AddWithValue("MobileNo", MobileNo);
						cmd.Parameters.AddWithValue("Sex", Sex);
						cmd.Parameters.AddWithValue("PassportValidTo", PassportValidTo);
						cmd.Parameters.AddWithValue("Spouse", Spouse);
						cmd.Parameters.AddWithValue("CivilStatus", CivilStatus);
						cmd.Parameters.AddWithValue("PRCLicenseRegistration", PRCLicenseRegistration);
						cmd.Parameters.AddWithValue("AIPOOrganization", AIPOOrganization);
						cmd.Parameters.AddWithValue("AIPOValidFrom", AIPOValidFrom);
						cmd.Parameters.AddWithValue("AIPOValidTo", AIPOValidTo);
						cmd.Parameters.AddWithValue("AIPOReceiptNo", AIPOReceiptNo);
						cmd.Parameters.AddWithValue("Designation", Designation);

						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception e)
			{
				ret = e.Message;
			}
			return ret;
		}


		//Add by Erwin
		//10/15/2020
		[WebMethod]
		public string Broker(
									 string BrokerId,
									 string TypeOfBusiness, //2
									 string ATPDate,
									 string Partnership, //4
									 string SECRegNo,

									 string FirstName, //6
									 string MiddleName,
									 string LastName, //8
									 string Nickname,
									 string Address, //10

									 string City,
									 int ZipCode, //12
									 string NatureOfBusiness,
									 string BusinessName, //14
									 string BusinessAddress,
									 int BusinessZipCode, //16
									 string BusinessPhoneNo,
									 string FaxNo, //18

									 string EmailAddress,
									 string Birthday, //20
									 string PlaceOfBirth,
									 string Religion, //22
									 string Citizenship,
									 string Tax, //24
												 //string SSS,

									 //string Passport, //26
									 //DateTime PassportValidFrom,
									 //string IssuedBy, //28
									 string PlacedIssued,

									 string PRCRegis, //30
									 DateTime PRCLicenseValid,
									 string PTRNo, //32
									 DateTime validFrom,
									 DateTime validTo, //34
									 string Status,

									 string BrokerApplicationForm, //36
									 string ListOfAccredited,
									 string AccreditationAgreement, //38
									 string BrokerAccreditationGenrealPolicies,

									 DateTime CreatedDate, //40
									 DateTime UpdateDate,
									 string Function,

									 string ResidenceNo,
									string MobileNo,
									string Sex,
									//DateTime PassportValidTo,
									string Spouse,
									string CivilStatus,
									string PRCLicenseRegistration,
									string AIPOOrganization,
									DateTime AIPOValidFrom,
									DateTime AIPOValidTo,
									string AIPOReceiptNo,
									string Designation,

									string ValidID,
									string IDNo,
									string IDOthers,
									DateTime IDExpirationDate,
									string ValidID2,
									string IDNo2,
									string IDOthers2,
									DateTime IDExpirationDate2,
									string ValidID3,
									string IDNo3,
									string IDOthers3,
									DateTime IDExpirationDate3,
									string ValidID4,
									string IDNo4,
									string IDOthers4,
									DateTime IDExpirationDate4,
									string PositionContact,
									string EmailContact,
									string AddressContact,
									string ResidenceContact,
									string EmailNotifStage,
									DateTime HLURB,

									string BrokerApplicationFormIssuedDate,
									string ListOfAccreditedIssuedDate,
									string AccreditationAgreementIssuedDate,
									string BrokerAccreditationGenrealPoliciesIssuedDate,
									string ContactPersonPosition,
									string HLURBLicenseNo,
									string AuthorizedRepresentative,
									string TradeName,
									string Province,

									string ContactValidID,
									string ContactIDOthers,
									string ContactIDNo,

									DateTime PRCRegistrationDate,
									DateTime CommitmentDate,
									string CommitmentName,
									DateTime CommitAffiliationDate,
									bool Conforme,
									string WTaxCode,
									string VATCode,
									string Location
									)
		{
			string ret;
			try
			{
				string query = "sp_AddBroker";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						//cmd.Parameters.AddWithValue("Id", 1);
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);
						cmd.Parameters.AddWithValue("TypeOfBusiness", TypeOfBusiness);
						cmd.Parameters.AddWithValue("ATPDate", ATPDate);
						cmd.Parameters.AddWithValue("Partnership", Partnership);
						cmd.Parameters.AddWithValue("SECRegNo", SECRegNo);

						cmd.Parameters.AddWithValue("FirstName", FirstName);
						cmd.Parameters.AddWithValue("MiddleName", MiddleName);
						cmd.Parameters.AddWithValue("LastName", LastName);
						cmd.Parameters.AddWithValue("Nickname", Nickname);
						cmd.Parameters.AddWithValue("Address", Address);


						cmd.Parameters.AddWithValue("City", City);
						cmd.Parameters.AddWithValue("ZipCode", ZipCode);
						cmd.Parameters.AddWithValue("NatureOfBusiness", NatureOfBusiness);
						cmd.Parameters.AddWithValue("BusinessName", BusinessName);
						cmd.Parameters.AddWithValue("BusinessAddress", BusinessAddress);
						cmd.Parameters.AddWithValue("BusinessZipCode", BusinessZipCode);
						cmd.Parameters.AddWithValue("BusinessPhoneNo", BusinessPhoneNo);
						cmd.Parameters.AddWithValue("FaxNo", FaxNo);

						cmd.Parameters.AddWithValue("EmailAddress", EmailAddress);
						cmd.Parameters.AddWithValue("Birthday", Birthday);
						cmd.Parameters.AddWithValue("PlaceOfBirth", PlaceOfBirth);
						cmd.Parameters.AddWithValue("Religion", Religion);
						cmd.Parameters.AddWithValue("Citizenship", Citizenship);
						cmd.Parameters.AddWithValue("Tax", Tax);
						//cmd.Parameters.AddWithValue("SSS", SSS);

						//cmd.Parameters.AddWithValue("Passport", Passport);
						//cmd.Parameters.AddWithValue("PassportValidFrom", PassportValidFrom);
						//cmd.Parameters.AddWithValue("IssuedBy", IssuedBy);
						cmd.Parameters.AddWithValue("PlacedIssued", PlacedIssued);

						cmd.Parameters.AddWithValue("PRCRegNum", PRCRegis);
						cmd.Parameters.AddWithValue("PRCLicenseValid", PRCLicenseValid);
						cmd.Parameters.AddWithValue("PTRNo", PTRNo);
						cmd.Parameters.AddWithValue("ValidFrom", validFrom);
						cmd.Parameters.AddWithValue("ValidTo", validTo);
						cmd.Parameters.AddWithValue("Status", Status);

						cmd.Parameters.AddWithValue("BrokerApplicationForm", BrokerApplicationForm);
						cmd.Parameters.AddWithValue("ListOfAccredited", ListOfAccredited);
						cmd.Parameters.AddWithValue("AccreditationAgreement", AccreditationAgreement);
						cmd.Parameters.AddWithValue("BrokerAccreditationGenrealPolicies", BrokerAccreditationGenrealPolicies);

						cmd.Parameters.AddWithValue("CreatedDate", CreatedDate);
						cmd.Parameters.AddWithValue("UpdateDate", UpdateDate);
						cmd.Parameters.AddWithValue("Func", Function);

						cmd.Parameters.AddWithValue("ResidenceNo", ResidenceNo);
						cmd.Parameters.AddWithValue("MobileNo", MobileNo);
						cmd.Parameters.AddWithValue("Sex", Sex);
						//cmd.Parameters.AddWithValue("PassportValidTo", PassportValidTo);
						cmd.Parameters.AddWithValue("Spouse", Spouse);
						cmd.Parameters.AddWithValue("CivilStatus", CivilStatus);
						cmd.Parameters.AddWithValue("PRCLicenseRegistration", PRCLicenseRegistration);
						cmd.Parameters.AddWithValue("AIPOOrganization", AIPOOrganization);
						cmd.Parameters.AddWithValue("AIPOValidFrom", AIPOValidFrom);
						cmd.Parameters.AddWithValue("AIPOValidTo", AIPOValidTo);
						cmd.Parameters.AddWithValue("AIPOReceiptNo", AIPOReceiptNo);
						cmd.Parameters.AddWithValue("Designation", Designation);

						cmd.Parameters.AddWithValue("ValidID", ValidID);
						cmd.Parameters.AddWithValue("IDOthers", IDOthers);
						cmd.Parameters.AddWithValue("IDNo", IDNo);
						cmd.Parameters.AddWithValue("IDExpirationDate", IDExpirationDate);
						cmd.Parameters.AddWithValue("ValidID2", ValidID2);
						cmd.Parameters.AddWithValue("IDOthers2", IDOthers2);
						cmd.Parameters.AddWithValue("IDNo2", IDNo2);
						cmd.Parameters.AddWithValue("IDExpirationDate2", IDExpirationDate2);
						cmd.Parameters.AddWithValue("ValidID3", ValidID3);
						cmd.Parameters.AddWithValue("IDOthers3", IDOthers3);
						cmd.Parameters.AddWithValue("IDNo3", IDNo3);
						cmd.Parameters.AddWithValue("IDExpirationDate3", IDExpirationDate3);
						cmd.Parameters.AddWithValue("ValidID4", ValidID4);
						cmd.Parameters.AddWithValue("IDOthers4", IDOthers4);
						cmd.Parameters.AddWithValue("IDNo4", IDNo4);
						cmd.Parameters.AddWithValue("IDExpirationDate4", IDExpirationDate4);
						cmd.Parameters.AddWithValue("PositionContact", PositionContact);
						cmd.Parameters.AddWithValue("EmailContact", EmailContact);
						cmd.Parameters.AddWithValue("AddressContact", AddressContact);
						cmd.Parameters.AddWithValue("ResidenceContact", ResidenceContact);
						cmd.Parameters.AddWithValue("EmailNotifStage", EmailNotifStage);
						cmd.Parameters.AddWithValue("HLURBValidFrom", HLURB);

						cmd.Parameters.AddWithValue("BrokerApplicationFormIssuedDate", (BrokerApplicationFormIssuedDate != "" ? Convert.ToDateTime(BrokerApplicationFormIssuedDate).ToString("yyyy-MM-dd") : null));
						cmd.Parameters.AddWithValue("ListOfAccreditedIssuedDate", (ListOfAccreditedIssuedDate != "" ? Convert.ToDateTime(ListOfAccreditedIssuedDate).ToString("yyyy-MM-dd") : null));
						cmd.Parameters.AddWithValue("AccreditationAgreementIssuedDate", (AccreditationAgreementIssuedDate != "" ? Convert.ToDateTime(AccreditationAgreementIssuedDate).ToString("yyyy-MM-dd") : null));
						cmd.Parameters.AddWithValue("BrokerAccreditationGenrealPoliciesIssuedDate", (BrokerAccreditationGenrealPoliciesIssuedDate != "" ? Convert.ToDateTime(BrokerAccreditationGenrealPoliciesIssuedDate).ToString("yyyy-MM-dd") : null));

						cmd.Parameters.AddWithValue("ContactPersonPosition", ContactPersonPosition);
						cmd.Parameters.AddWithValue("HLURBLicenseNo", HLURBLicenseNo);
						cmd.Parameters.AddWithValue("AuthorizedRepresentative", AuthorizedRepresentative);
						cmd.Parameters.AddWithValue("TradeName", TradeName);
						cmd.Parameters.AddWithValue("Province", Province);

						cmd.Parameters.AddWithValue("ValidIDContactInfo", ContactValidID);
						cmd.Parameters.AddWithValue("IDOthersContactInfo", ContactIDOthers);
						cmd.Parameters.AddWithValue("IDNoContactInfo", ContactIDNo);

						cmd.Parameters.AddWithValue("PRCRegistrationDate", PRCRegistrationDate);
						cmd.Parameters.AddWithValue("CommitmentDate", CommitmentDate);
						cmd.Parameters.AddWithValue("CommitmentName", CommitmentName);
						cmd.Parameters.AddWithValue("CommitAffiliationDate", CommitAffiliationDate);
						cmd.Parameters.AddWithValue("Conforme", Convert.ToInt32(Conforme));

						cmd.Parameters.AddWithValue("WTaxCode", WTaxCode);
						cmd.Parameters.AddWithValue("VATCode", VATCode);
						cmd.Parameters.AddWithValue("Location", Location);

						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception e)
			{
				ret = e.Message;
			}
			return ret;
		}



		//Add by Erwin
		//10/16/2020
		[WebMethod]
		public string insertBrokerSalesPerson(
										string index,
										string BrokerId,
										DateTime ValidFrom,
										DateTime ValidTo,
										DateTime CreateDate,
										string function)
		{
			string ret;
			try
			{
				string query = "sp_AddBRK1";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", index);
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);

						cmd.Parameters.AddWithValue("ValidFrom", ValidFrom);
						cmd.Parameters.AddWithValue("ValidTo", ValidTo);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						cmd.Parameters.AddWithValue("Func", function);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}



		//Add by Erwin
		//10/16/2020
		[WebMethod]
		public string insertBrokerSharePerson(
										int Id,
										string SalesPersonId,
										string BrokerId,
										string PositionSharedDetails,
										string SalesPersonNameSharedDetails,
										double PercentageSharedDetails,
										double HouseandLotSharingDetails,
										//DateTime ValidFromSharedDetails,
										//DateTime ValidToSharedDetails,
										DateTime CreateDate,
										string oslaID,
										string function,

										//2023-07-10 : ADDITIONAL PARAMETERS
										string ProjectCode,
										string ProjectName,
										double CommissionPercentage,
										string Type

			)
		{
			string ret;
			try
			{
				string query = "sp_AddBRK2";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", Id);
						cmd.Parameters.AddWithValue("SalesPersonId", SalesPersonId);
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);
						cmd.Parameters.AddWithValue("PositionSharedDetails", PositionSharedDetails);
						cmd.Parameters.AddWithValue("SalesPersonNameSharedDetails", SalesPersonNameSharedDetails);
						cmd.Parameters.AddWithValue("PercentageSharedDetails", PercentageSharedDetails);
						cmd.Parameters.AddWithValue("HouseandLotSharingDetails", HouseandLotSharingDetails);
						//cmd.Parameters.AddWithValue("ValidFromSharedDetails", ValidFromSharedDetails);
						//cmd.Parameters.AddWithValue("ValidToSharedDetails", ValidToSharedDetails);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						cmd.Parameters.AddWithValue("oslaID", oslaID);
						cmd.Parameters.AddWithValue("Func", function);

						//2023-07-10 : ADDITIONAL PARAMETERS
						cmd.Parameters.AddWithValue("ProjectCode", ProjectCode);
						cmd.Parameters.AddWithValue("ProjectName", ProjectName);
						cmd.Parameters.AddWithValue("CommissionPercentage", CommissionPercentage);
						cmd.Parameters.AddWithValue("Type", Type);

						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		//Add by Erwin
		//10/16/2020
		[WebMethod]
		public bool insertBrokerAttachments(
										int Id,
										string BrokerId,
										string TypeOfBusiness,
										string RealEstateBrokerLicense,
										string HLURBCertificateOfRegistration,
										string ProofOfVAT,
										string GovtIssuedID,
										string DTIRegistration,
										string OfficialReceipt,
										string SECRegistration,
										string ArticleOfPartnership,
										string BoardOfResolution,
										string ProofOfTIN,
										DateTime CreateDate)
		{
			bool ret;
			try
			{
				string query = "sp_AddBRK3";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", Id);
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);
						cmd.Parameters.AddWithValue("TypeOfBusiness", TypeOfBusiness);
						cmd.Parameters.AddWithValue("RealEstateBrokerLicense", RealEstateBrokerLicense);
						cmd.Parameters.AddWithValue("HLURBCertificateOfRegistration", HLURBCertificateOfRegistration);
						cmd.Parameters.AddWithValue("ProofOfVAT", ProofOfVAT);
						cmd.Parameters.AddWithValue("GovtIssuedID", GovtIssuedID);
						cmd.Parameters.AddWithValue("DTIRegistration", DTIRegistration);
						cmd.Parameters.AddWithValue("OfficialReceipt", OfficialReceipt);
						cmd.Parameters.AddWithValue("SECRegistration", SECRegistration);
						cmd.Parameters.AddWithValue("ArticleOfPartnership", ArticleOfPartnership);
						cmd.Parameters.AddWithValue("BoardOfResolution", BoardOfResolution);
						cmd.Parameters.AddWithValue("ProofOfTIN", ProofOfTIN);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		[WebMethod]
		public string insertBrokerDocument(
								  string BrokerId,
								  string DocumentName,
								  string Section,
								  string FileName,
								  DateTime CreateDate,
								  string function,
								  string date
								  )
		{
			string ret;
			try
			{
				string query = "sp_AddBRK3";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("BrokerId", BrokerId);
						cmd.Parameters.AddWithValue("DocumentName", DocumentName);
						cmd.Parameters.AddWithValue("Section", Section);
						cmd.Parameters.AddWithValue("FileName", FileName);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						cmd.Parameters.AddWithValue("Func", function);
						cmd.Parameters.AddWithValue("DateIssued", Convert.ToDateTime(date).ToString("yyyy-MM-dd"));
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		[WebMethod]
		public string updateBrokerApproval(
								  string BrokerId,
								  string TypeOfApproval,
								  string GeneralInfoComments,
								  string AddressBusinessComments,
								  string SupplimentaryDetailsComments,
								  string PRCLicenseInformationComments
								  )
		{
			string ret;
			try
			{
				string query = "sp_BrokerApproval";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("BrokerID", BrokerId);
						cmd.Parameters.AddWithValue("TypeOfApproval", TypeOfApproval);
						cmd.Parameters.AddWithValue("GeneralInfoComments", GeneralInfoComments);
						cmd.Parameters.AddWithValue("AddressBusinessComments", AddressBusinessComments);
						cmd.Parameters.AddWithValue("SupplimentaryDetailsComments", SupplimentaryDetailsComments);
						cmd.Parameters.AddWithValue("PRCLicenseInformationComments", PRCLicenseInformationComments);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		[WebMethod]
		public string updateBrokerApprovalDocuments(
							  string BrokerId,
							  string Comments,
							  string DocumentName,
							  string Section,
							  string Approved,
							  string filename,
							  DateTime dateissued)
		{
			string ret;
			try
			{
				string query = "sp_BrokerApprovalDocuments";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("BrokerID", BrokerId);
						cmd.Parameters.AddWithValue("Comments", Comments);
						cmd.Parameters.AddWithValue("DocumentName", DocumentName);
						cmd.Parameters.AddWithValue("Section", Section);
						cmd.Parameters.AddWithValue("Approved", Approved);
						cmd.Parameters.AddWithValue("FileName", filename);
						cmd.Parameters.AddWithValue("DateIssued", dateissued);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		//Add by Karl
		//4/12/2021
		[WebMethod]
		public string addBrokerApprovalDocuments(
							  string BrokerId,
							  string Comments,
							  string DocumentName,
							  string Section,
							  string Approved,
							  string filename,
							  DateTime dateissued)
		{
			string ret;
			try
			{
				string query = "sp_AddBrokerApprovalDocuments";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("BrokerID", BrokerId);
						cmd.Parameters.AddWithValue("Comments", Comments);
						cmd.Parameters.AddWithValue("DocumentName", DocumentName);
						cmd.Parameters.AddWithValue("Section", Section);
						cmd.Parameters.AddWithValue("Approved", Approved);
						cmd.Parameters.AddWithValue("FileName", filename);
						cmd.Parameters.AddWithValue("DateIssued", dateissued);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		//Add by Joses
		//12/19/2020
		[WebMethod]
		public string registerSalesAgent(
										string index,
										string SalesPerson,
										string EmailAddress,
										string Position,
										string PRCLicense,
										DateTime PRCLicenseExpirationDate,
										DateTime ATPDateSalesPerson,
										string TIN,
										string VATCode,
										string WTaxCode,
										string MobileNumber,
										DateTime CreateDate,
										string HLURBLicenseNo,
										 string PTRNo,
										string CreateBrokerID)
		{
			string ret;
			try
			{
				string query = "sp_RegisterSalesAgent";
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand(query, con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Id", index);
						cmd.Parameters.AddWithValue("SalesPerson", SalesPerson);
						cmd.Parameters.AddWithValue("EmailAddress", EmailAddress);
						cmd.Parameters.AddWithValue("Position", Position);
						cmd.Parameters.AddWithValue("PRCLicense", PRCLicense);
						cmd.Parameters.AddWithValue("PRCLicenseExpirationDate", PRCLicenseExpirationDate);
						cmd.Parameters.AddWithValue("ATPDateSalesPerson", ATPDateSalesPerson);
						cmd.Parameters.AddWithValue("TIN", TIN);
						cmd.Parameters.AddWithValue("VATCode", VATCode);
						cmd.Parameters.AddWithValue("WTaxCode", WTaxCode);
						cmd.Parameters.AddWithValue("MobileNumber", MobileNumber);
						cmd.Parameters.AddWithValue("CreateDate", CreateDate);
						cmd.Parameters.AddWithValue("HLURBLicenseNo", HLURBLicenseNo);
						cmd.Parameters.AddWithValue("PTRNo", PTRNo);
						cmd.Parameters.AddWithValue("CreateBrokerID", CreateBrokerID);
						con.Open();
						cmd.ExecuteNonQuery();
						ret = "Operation completed successfully.";
					}
				}
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}





		[WebMethod]
		public DataSet GetIncentiveAgents(string EmpCode, string ProjectCode, string ProductType)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetIncentiveAgents", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("EmpCode", EmpCode);
							cmd.Parameters.AddWithValue("ProjectCode", ProjectCode);
							cmd.Parameters.AddWithValue("ProductType", ProductType);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetIncentiveAgents");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetCommissionAgents(string EmpCode)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetCommissionAgents", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", ConfigurationManager.AppSettings["HANADatabase"]);
							cmd.Parameters.AddWithValue("EmpCode", EmpCode);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetCommissionAgents");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetHouseDetails(string oProject, string oBlock, string oLot, string FinScheme)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetHouseDetails", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							cmd.Parameters.AddWithValue("PrjCode", oProject);
							cmd.Parameters.AddWithValue("Block", oBlock);
							cmd.Parameters.AddWithValue("Lot", oLot);
							cmd.Parameters.AddWithValue("FinScheme", FinScheme);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetHouseDetails");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetHouseModelNew(string oProject)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetHouseModelNew", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
							cmd.Parameters.AddWithValue("PrjCode", oProject);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetHouseModelNew");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetSalesEmployees(DateTime DocDate, string BrokerId)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetSalesEmployees", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocDate", DocDate);
							cmd.Parameters.AddWithValue("BrokerId", BrokerId);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetSalesEmployees");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetForfeitures(string Database)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_Forfeitures", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", Database);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetForfeitures");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetForfeituresSearch(string Database, string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_ForfeituresSearch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("Database", Database);
							cmd.Parameters.AddWithValue("Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetForfeituresSearch");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetSalesEmployeesSearch(DateTime DocDate, string BrokerId, string search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("sp_GetSalesEmployeesSearch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocDate", DocDate);
							cmd.Parameters.AddWithValue("BrokerId", BrokerId);
							cmd.Parameters.AddWithValue("search", search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "GetSalesEmployeesSearch");
								ret = ds;
							}
						}
					}
				}

			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

















		[WebMethod]
		public string sendEmail(string ID, string subject, string receiver, string body, string Link, string LinkMessage, string IDType, string Code)
		{
			string ret;
			try
			{
				string qry = $@"SELECT * FROM ""@EMAIL"" WHERE ""Code"" = '{Code}'";
				DataTable dt = hana.GetData(qry, hana.GetConnection("SAPHana"));

				MailMessage mail = new MailMessage();
				//mail.To.Add($"{receiver},{ConfigSettings.EmailCC"].ToString()}");
				mail.To.Add($"{receiver},{DataAccess.GetData(dt, 0, "U_CC", "").ToString()}");
				//mail.From = new MailAddress(ConfigSettings.EmailAddFrom"].ToString(), ConfigSettings.EmailAlias"].ToString(), System.Text.Encoding.UTF8);
				mail.From = new MailAddress(DataAccess.GetData(dt, 0, "U_Email", "").ToString(), DataAccess.GetData(dt, 0, "U_Alias", "").ToString(), System.Text.Encoding.UTF8);
				mail.Subject = subject + " ";
				mail.SubjectEncoding = System.Text.Encoding.UTF8;
				mail.Body = createEmailBody(ID, body, Link, LinkMessage, IDType, DataAccess.GetData(dt, 0, "U_Picture", "").ToString());
				mail.BodyEncoding = System.Text.Encoding.UTF8;
				mail.IsBodyHtml = true;

				mail.Priority = MailPriority.High;
				SmtpClient client = new SmtpClient();
				client.UseDefaultCredentials = false;
				//client.Credentials = new System.Net.NetworkCredential(ConfigSettings.EmailAddFrom"].ToString(), ConfigSettings.EmailPassword"].ToString());
				client.Credentials = new System.Net.NetworkCredential(DataAccess.GetData(dt, 0, "U_Email", "").ToString(), DataAccess.GetData(dt, 0, "U_Password", "").ToString());
				//client.Port = Convert.ToInt32(ConfigSettings.EmailPort"].ToString());
				client.Port = Convert.ToInt32(DataAccess.GetData(dt, 0, "U_Port", "0").ToString());
				//client.Host = ConfigSettings.EmailHost"].ToString();
				client.Host = DataAccess.GetData(dt, 0, "U_Host", "").ToString();
				client.EnableSsl = true;

				client.Send(mail);
				ret = "Operation completed successfully.";
			}
			catch (Exception ex) { ret = ex.Message; }
			return ret;
		}


		public string createEmailBody(string brokerID, string bodyMessage, string Link, string LinkMessage, string IDType, string picture)
		{
			string qry = $@"SELECT 
	                            CASE WHEN LOWER(""TypeOfBusiness"") <> 'sole proprietor'
	                            THEN
		                            ""Partnership""
	                            ELSE
		                            IFNULL(""FirstName"" || ' ','') || IFNULL(""MiddleName"" || ' ','') || IFNULL(""LastName"" || ' ','')
	                            END AS ""name""
                            FROM OBRK
                            WHERE ""BrokerId"" = '{brokerID}'";
			DataTable dt = hana.GetData(qry, hana.GetConnection("SAOHana"));

			string body = string.Empty;
			//using streamreader for reading my htmltemplate   

			using (StreamReader reader = new StreamReader(Server.MapPath("~/class/emailTemplate.html")))

			{
				body = reader.ReadToEnd();
			}

			bodyMessage = bodyMessage.Replace("{yyyy}", DateTime.Now.Year.ToString());
			bodyMessage = bodyMessage.Replace("{status}", Session["BrkStatus"].ToString());
			bodyMessage = bodyMessage.Replace("{creds}", Session["Brkcreds"].ToString());
			bodyMessage = bodyMessage.Replace("{BrokerName}", DataAccess.GetData(dt, 0, "name", "").ToString());

			body = body.Replace("{message}", bodyMessage + " ");
			body = body.Replace("{BrokerID}", brokerID);
			body = body.Replace("{Link}", Link);
			body = body.Replace("{LinkMessage}", LinkMessage);
			body = body.Replace("{IDType}", IDType);
			body = body.Replace("{reason}", Session["BrkReason"].ToString());
			//body = body.Replace("{picture}", ConfigSettings.EmailPicture"].ToString());
			body = body.Replace("{picture}", picture);
			body = body.Replace("{Randomness}", DateTime.Now.ToString());


			return body;
		}

		[WebMethod]
		public DataSet AddDocumentStatus(int DocEntry, DateTime InputDate, string Scheme, string DocCode, string Document, DateTime DateRequired, DateTime IssueDate, DateTime DocumentDate, DateTime ReceivedDate, DateTime ExpiryDate, string Status, string ReferenceNo, string Remarks, string Attachment)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("addDocumentStatus", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocEntry", DocEntry);
							cmd.Parameters.AddWithValue("InputDate", InputDate);
							cmd.Parameters.AddWithValue("LoanType", Scheme);
							cmd.Parameters.AddWithValue("DocCode", DocCode);
							cmd.Parameters.AddWithValue("Document", Document);
							cmd.Parameters.AddWithValue("DateRequired", DateRequired);
							cmd.Parameters.AddWithValue("IssueDate", IssueDate);
							cmd.Parameters.AddWithValue("DocumentDate", DocumentDate);
							cmd.Parameters.AddWithValue("ReceivedDate", ReceivedDate);
							cmd.Parameters.AddWithValue("ExpiryDate", ExpiryDate);
							cmd.Parameters.AddWithValue("Status", Status);
							cmd.Parameters.AddWithValue("ReferenceNo", ReferenceNo);
							cmd.Parameters.AddWithValue("Remarks", Remarks);
							cmd.Parameters.AddWithValue("Attachment", Attachment);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "AddDocumentStatus");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet UpdateDocumentStatus(int DocEntry, string DocCode, DateTime DateRequired, DateTime IssueDate, DateTime DocumentDate, DateTime ReceivedDate, DateTime ExpiryDate, string Status, string ReferenceNo, string Remarks, string Attachment)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("UpdateDocumentStatus", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("DocEntry", DocEntry);
							cmd.Parameters.AddWithValue("DocCode", DocCode);
							cmd.Parameters.AddWithValue("DateRequired", DateRequired);
							cmd.Parameters.AddWithValue("IssueDate", IssueDate);
							cmd.Parameters.AddWithValue("DocumentDate", DocumentDate);
							cmd.Parameters.AddWithValue("ReceivedDate", ReceivedDate);
							cmd.Parameters.AddWithValue("ExpiryDate", ExpiryDate);
							cmd.Parameters.AddWithValue("Status", Status);
							cmd.Parameters.AddWithValue("ReferenceNo", ReferenceNo);
							cmd.Parameters.AddWithValue("Remarks", Remarks);
							cmd.Parameters.AddWithValue("Attachment", Attachment);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "UpdateDocumentStatus");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet DocStatBuyerList()
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("DocStatBuyerList", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "DocStatBuyerList");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet DocStatBuyerListSearch(string Search)
		{
			DataSet ret = null;
			try
			{
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand("DocStatBuyerListSearch", con))
						{
							cmd.CommandType = CommandType.StoredProcedure;
							cmd.Parameters.AddWithValue("@Search", Search);
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "DocStatBuyerListSearch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		//GAB 05/03/2023 Add to database
		[WebMethod]
		public bool AddSharingDetails(int Id, string salesPersonId, string brokerId, string position, string salesName, double lotShare, DateTime CreateDate, string salesAgentId, double hNLotShare, string projCode, string projName, double commission, string type)
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					conn.Open();
					using (HanaTransaction transaction = conn.BeginTransaction())
					{
						try
						{
							//string sharingDetail = isLot ? "PercentageSharedDetails" : "HouseandLotSharingDetails";
							//string type = isLot ? "Lot" : "HouseNLot";
							string query = $@"INSERT INTO ""temp_BRK2"" 
                            (""ProjectCode"", ""ProjectName"", ""CommissionPercentage"", ""SalesPersonId"", ""SalesPersonNameSharedDetails"", ""OslaID"", ""PositionSharedDetails"", ""PercentageSharedDetails"", ""HouseandLotSharingDetails"", ""Id"", ""Type"", ""BrokerId"", ""CreateDate"") 
                            VALUES (:projCode, :projName, :commission, :salesPersonId, :salesName, :salesAgentId, :position, :lotShare, :hNLotShare, :Id, :type, :brokerId, :CreateDate)";
							using (HanaCommand insertData = new HanaCommand(query, conn))
							{
								insertData.Parameters.AddWithValue("projCode", projCode);
								insertData.Parameters.AddWithValue("projName", projName);
								insertData.Parameters.AddWithValue("commission", commission);
								insertData.Parameters.AddWithValue("salesPersonId", salesPersonId);
								insertData.Parameters.AddWithValue("salesName", salesName);
								insertData.Parameters.AddWithValue("salesAgentId", salesAgentId);
								insertData.Parameters.AddWithValue("lotShare", lotShare);
								insertData.Parameters.AddWithValue("hNLotShare", hNLotShare);
								insertData.Parameters.AddWithValue("position", position);
								insertData.Parameters.AddWithValue("Id", Id);
								insertData.Parameters.AddWithValue("type", type);
								insertData.Parameters.AddWithValue("brokerId", brokerId);
								insertData.Parameters.AddWithValue("CreateDate", CreateDate);
								insertData.Transaction = transaction;
								insertData.ExecuteNonQuery();
								ret = true;
							}
							transaction.Commit();
						}
						catch (HanaException ex)
						{
							transaction.Rollback();
							// Log the exception or rethrow it
							throw ex;
						}
					}
				}
			}
			catch (HanaException)
			{
				ret = false;
			}
			return ret;
		}

		//GAB 05/03/2023 Delete from Database
		[WebMethod]
		public bool DeleteSharingDetails(string brokerId)
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					conn.Open();
					HanaTransaction transaction = conn.BeginTransaction();
					try
					{
						using (HanaCommand updatecmd = new HanaCommand($@"DELETE FROM ""temp_BRK2"" WHERE ""BrokerId"" = :brokerId", conn))
						{
							updatecmd.Parameters.AddWithValue("brokerId", brokerId);
							updatecmd.Transaction = transaction;
							updatecmd.ExecuteNonQuery();
							ret = true;
						}
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						// Log the exception or rethrow it
						throw ex;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}

		//04/25/2023 Saving from temp_BRK2 to BRK2
		[WebMethod]
		public bool TransferSharingDetails()
		{
			bool ret = true;
			try
			{
				using (HanaConnection conn = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					conn.Open();
					HanaTransaction transaction = conn.BeginTransaction();
					try
					{
						HanaCommand updatecmd = new HanaCommand($@"INSERT INTO ""BRK2"" (""ProjectCode"", ""ProjectName"", ""BrokerId"", ""CommissionPercentage"", ""SalesPersonId"", ""SalesPersonNameSharedDetails"", ""OslaID"", ""PositionSharedDetails"", ""PercentageSharedDetails"", ""HouseandLotSharingDetails"", ""Id"", ""Type"") 
                                                                        SELECT ""ProjectCode"", ""ProjectName"", ""BrokerId"",""CommissionPercentage"", ""SalesPersonId"", ""SalesPersonNameSharedDetails"", ""OslaID"", ""PositionSharedDetails"", ""PercentageSharedDetails"", ""HouseandLotSharingDetails"", ""Id"", ""Type""
                                                                        FROM ""temp_BRK2"";
                                                                        DELETE FROM ""temp_BRK2"";", conn);
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						// Log the exception or rethrow it
						throw ex;
					}
				}
			}
			catch { ret = false; }
			return ret;
		}
		#endregion



		// CANVASS IMAGES


		[WebMethod]
		public ReportViewModel GetProjectForCanvas()
		{
			var model = new ReportViewModel();
			try
			{
				string query = $@"SELECT ""PrjCode"",""PrjName"",""PrjImage"", ""ImgWidth"" ,""ImgHeight"" FROM ""OPRJ"" ORDER BY ""PrjCode"" ASC";
				using (DataSet ds = new DataSet())
				{
					using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
					{
						using (HanaCommand cmd = new HanaCommand(query, con))
						{
							using (HanaDataAdapter da = new HanaDataAdapter(cmd))
							{
								da.Fill(ds, "Projects");
								model.ImageDatas = ds.Tables[0].AsEnumerable().Select(x => new ReportViewModel.ImageData
								{

									projectCode = x["PrjCode"].ToString(),
									projectName = x["PrjName"].ToString(),
									imgWidth = int.Parse(x["ImgWidth"].ToString()),
									imgHeight = int.Parse(x["ImgHeight"].ToString())
								}).ToList();
							}
						}
					}
				}
			}
			catch { model = null; }
			return model;
		}

		[WebMethod]
		public DataSet GetRestructureHistory(string DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetRestructureHistory", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Database", $"{ConfigurationManager.AppSettings["HANADatabase"]}");
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetRestructureHistory");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}

		[WebMethod]
		public DataSet GetBuyersInfo(string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("GetBuyersInfo", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "BuyerInfo");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}





		[WebMethod]
		public DataSet GetCommission(string Database)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetCommissionNEW", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCommission");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetCommissionPosting(string Database, string DocEntry, string Position, string Release, string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetCommissionPosting", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						cmd.Parameters.AddWithValue("Position", Position);
						cmd.Parameters.AddWithValue("Release", Release);
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCommissionPosting");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}




		[WebMethod]
		public DataSet GetCommissionSearch(string Database, string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetCommissionSearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCommissionSearch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		//2023-11-13 : GET COMMISSIONS FOR RELEASE BASE ON DOCENTRY -- FOR NEW CONDITION FOR RELEASING OF COMMISISON
		[WebMethod]
		public DataSet GetCommissionPerDocEntry(string Database, string DocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetCommissionJE", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						cmd.Parameters.AddWithValue("DocEntry", DocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetCommissionPerDocEntry");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet LoadDocumentRemarks(string Database, string oDocEntry)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_LoadDocumentRemarks", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						cmd.Parameters.AddWithValue("DocEntry", oDocEntry);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "LoadDocumentRemarks");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetIncentive(string Database)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetIncentiveNEW", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetIncentive");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public DataSet GetIncentivePosting(string Database, string DocEntry, string Position, string Release, string CardCode)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetIncentivePosting1", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database1", Database);
						cmd.Parameters.AddWithValue("DocNum1", DocEntry);
						cmd.Parameters.AddWithValue("Post1", Position);
						cmd.Parameters.AddWithValue("Release1", Release);
						cmd.Parameters.AddWithValue("CardCode", CardCode);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetIncentivePosting");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}



		[WebMethod]
		public DataSet GetIncentiveSearch(string Database, string Search)
		{
			DataSet ret = null;
			try
			{
				using (HanaConnection con = new HanaConnection(hana.GetConnection("SAOHana")))
				{
					using (HanaCommand cmd = new HanaCommand("sp_GetIncentiveSearch", con))
					{
						cmd.CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("Database", Database);
						cmd.Parameters.AddWithValue("Search", Search);
						using (HanaDataAdapter da = new HanaDataAdapter(cmd))
						{
							using (DataSet ds = new DataSet())
							{
								da.Fill(ds, "GetIncentiveSearch");
								ret = ds;
							}
						}
					}
				}
			}
			catch (Exception ex) { ret = null; }
			return ret;
		}


		[WebMethod]
		public bool ReportCreation(ReportViewModel model)
		{
			try
			{
				foreach (var x in model.ImageDatas)
				{

				}

			}
			catch { return false; }
			return true;
		}

		public void CreateRequest()
		{


			var httpRequest = HttpContext.Current.Request;

		}




	}
}
