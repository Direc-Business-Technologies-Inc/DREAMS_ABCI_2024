﻿using System;
using System.Reflection;

namespace ABROWN_DREAMS
{
    public class AssemblyInfo
    {
        // The assembly information values.
        public string Title = "", Description = "", Company = "",
            Product = "", Copyright = "", Trademark = "",
            AssemblyVersion = "", FileVersion = "", Guid = "",
            NeutralLanguage = "";
        public bool IsComVisible = false;

        // Constructors.
        public AssemblyInfo()
            : this(Assembly.GetExecutingAssembly())
        {
        }

        public AssemblyInfo(Assembly assembly)
        {
            if (assembly == null)
                assembly = Assembly.GetExecutingAssembly();
            // Get values from the assembly.
            AssemblyTitleAttribute titleAttr =
                GetAssemblyAttribute<AssemblyTitleAttribute>(assembly);
            if (titleAttr != null) Title = titleAttr.Title;

            AssemblyDescriptionAttribute assemblyAttr =
                GetAssemblyAttribute<AssemblyDescriptionAttribute>(assembly);
            if (assemblyAttr != null) Description = assemblyAttr.Description;

            AssemblyCompanyAttribute companyAttr =
                GetAssemblyAttribute<AssemblyCompanyAttribute>(assembly);
            if (companyAttr != null) Company = companyAttr.Company;

            AssemblyProductAttribute productAttr =
                GetAssemblyAttribute<AssemblyProductAttribute>(assembly);
            if (productAttr != null) Product = productAttr.Product;

            AssemblyCopyrightAttribute copyrightAttr =
                GetAssemblyAttribute<AssemblyCopyrightAttribute>(assembly);
            if (copyrightAttr != null) Copyright = copyrightAttr.Copyright;

            AssemblyTrademarkAttribute trademarkAttr =
                GetAssemblyAttribute<AssemblyTrademarkAttribute>(assembly);
            if (trademarkAttr != null) Trademark = trademarkAttr.Trademark;

            AssemblyVersion = assembly.GetName().Version.ToString();

            AssemblyFileVersionAttribute fileVersionAttr =
                GetAssemblyAttribute<AssemblyFileVersionAttribute>(assembly);
            if (fileVersionAttr != null) FileVersion = fileVersionAttr.Version;

            System.Runtime.InteropServices.GuidAttribute guidAttr =
                GetAssemblyAttribute<System.Runtime.InteropServices.GuidAttribute>(assembly);
            if (guidAttr != null) Guid = guidAttr.Value;

            System.Resources.NeutralResourcesLanguageAttribute languageAttr =
                GetAssemblyAttribute<System.Resources.NeutralResourcesLanguageAttribute>(assembly);
            if (languageAttr != null) NeutralLanguage = languageAttr.CultureName;

            System.Runtime.InteropServices.ComVisibleAttribute comAttr =
                GetAssemblyAttribute<System.Runtime.InteropServices.ComVisibleAttribute>(assembly);
            if (comAttr != null) IsComVisible = comAttr.Value;
        }

        // Return a particular assembly attribute value.
        public static T GetAssemblyAttribute<T>(Assembly assembly) where T : Attribute
        {
            // Get attributes of this type.
            object[] attributes = assembly.GetCustomAttributes(typeof(T), true);

            // If we didn't get anything, return null.
            if ((attributes == null) || (attributes.Length == 0)) return null;

            // Convert the first attribute value into the desired type and return it.
            return (T)attributes[0];
        }
    }
}
