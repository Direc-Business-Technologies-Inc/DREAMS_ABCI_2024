using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace ABROWN_DREAMS
{
	public class DataAccess
	{
		public static string con(string contype)
		{ return ConfigurationManager.ConnectionStrings[contype].ConnectionString; }
		public static string GetconDetails(string contype, string value)
		{
			SqlConnectionStringBuilder bldr = new SqlConnectionStringBuilder(con(contype));
			return bldr[value].ToString();
		}

		public static DataTable Select(string constr, string query)
		{
			try
			{
				using (DataTable dt = new DataTable())
				{
					using (SqlConnection cnn = new SqlConnection(con(constr)))
					{
						using (SqlCommand cmd = new SqlCommand(query, cnn))
						{
							using (SqlDataAdapter da = new SqlDataAdapter(cmd))
							{
								cnn.Open();
								da.Fill(dt);
								cnn.Close();
								return dt;
							}
						}
					}
				}
			}
			catch (Exception ex) { return null; }
		}

		public static bool Execute(string constr, string query)
		{
			DataTable dt = new DataTable();
			Boolean ret = false;

			try
			{
				using (SqlConnection cnn = new SqlConnection(con(constr)))
				{
					using (SqlCommand cmd = new SqlCommand(query, cnn))
					{
						cnn.Open();
						cmd.ExecuteNonQuery();
						ret = true;
					}
				}
			}
			catch (Exception ex) { ret = false; }
			return ret;
		}

		public static bool Exist(DataTable dt)
		{
			bool ret;
			try
			{
				if (dt.Rows.Count > 0)
				{ ret = true; }
				else
				{ ret = false; }
			}
			catch (Exception ex)
			{ ret = false; }
			return ret;
		}
		public static string GetSession(string oSession, string oReplace)
		{
			string ret;
			if (!string.IsNullOrEmpty(oSession))
			{ ret = oSession; }
			else { ret = oReplace; }
			return ret;
		}

		public static string GetViewState(string oSession, string oReplace)
		{
			string ret;
			if (!string.IsNullOrEmpty(oSession))
			{ ret = oSession; }
			else { ret = oReplace; }
			return ret;
		}

		public static object GetData(DataTable dt, int row, string ColumnName, string newvalue)
		{
			try
			{
				object value;

				if (dt.Rows[row][ColumnName] == null) value = ""; else value = dt.Rows[row][ColumnName].ToString();

				value = dt.Rows[row][ColumnName].ToString();

				if (value == null || string.IsNullOrEmpty(value.ToString()))
				{ return newvalue; }
				else { return value; }
			}
			catch (Exception)
			{
				return newvalue;
			}
		}

		public static object GetValue(string con, string query, int row, string ColumnName, string newvalue)
		{
			double n;
			string value;
			DataTable dt = new DataTable();
			dt = Select(con, query);
			if (dt.Rows[row][ColumnName] == null) value = ""; else value = dt.Rows[row][ColumnName].ToString();

			bool isNumeric = double.TryParse(value, out n);

			if (isNumeric == false)
			{ value = dt.Rows[row][ColumnName].ToString(); }
			else
			{ value = dt.Rows[row][ColumnName].ToString(); }

			if (value == null || string.IsNullOrEmpty(value.ToString()))
			{ return newvalue; }
			else { return value.ToString(); }
		}
		public static byte[] ConvertImageToByteArray(System.Drawing.Image imageToConvert,
									   System.Drawing.Imaging.ImageFormat formatOfImage)
		{
			byte[] Ret;
			try
			{
				using (MemoryStream ms = new MemoryStream())
				{
					imageToConvert.Save(ms, formatOfImage);
					Ret = ms.ToArray();
				}
			}
			catch (Exception) { throw; }
			return Ret;
		}
	}
}