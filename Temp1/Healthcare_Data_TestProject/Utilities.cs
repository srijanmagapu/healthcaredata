using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Healthcare_Data_TestProject
{
    public class HealthCareDataDirectories
    {
        public string SourceDemographicFileDirectory { get; set; }
        public string SourceDemographicFileName { get; set; }
        public string DestinationConvertedFileDirectory { get; set; }
        public string ProcessedFilesDirectory { get; set; }
        public string ErrorFilesDirectory { get; set; }
        public string LogFilesDirectory { get; set; }

        public HealthCareDataDirectories(String xml)
        {
            XDocument document = XDocument.Parse(xml);
           
                var root = document.Root;

            if (root != null)
            {
                SourceDemographicFileDirectory = (string) root.Element("SDFD");
                SourceDemographicFileName = (string) root.Element("SDFN");
                DestinationConvertedFileDirectory = (string) root.Element("DCFD");
                ProcessedFilesDirectory = (string) root.Element("PFD");
                ErrorFilesDirectory = (string) root.Element("EFD");
                LogFilesDirectory = (string) root.Element("LFD");
            }
        }
    }

    class Guarantor
    {
        public const string StrRecordTypeCode = "01";
        public const string StrDataSet = "EGO";
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0},{1}", this.LastName, this.FirstName);
            }
        }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int ZipCode1 { get; set; }

        public int ZipCode2 { get; set; }

        public int HomePhoneAreaCode { get; set; }

        public int HomePhoneNumber { get; set; }

        public int EmployerName { get; set; }

        public string EmployerAddress { get; set; }

        public string EmployerCity { get; set; }

        public string EmployerState { get; set; }

        public int EmployerZipCode1 { get; set; }

        public int EmployerZipCode2 { get; set; }

        public int EmployerPhoneAreaCode { get; set; }

        public int EmployerPhoneNumber { get; set; }

        public string EmployerWorkExt { get; set; }

        public string ForeignZipCode { get; set; }

        public string FinancialClass { get; set; }

        public string SocialSecurityNumber { get; set; }

        public string HospitalMedicalRecordNumber { get; set; }

        public string HospitalCode { get; set; }

        public string DriverLicenseNumber { get; set; }

        public string EmergencyContact { get; set; }

        public string EmergencyContractRelationship { get; set; }

        public int EmergencyContactPhoneNumber { get; set; }

        public string ReferralCode { get; set; }

        public string AltMailToName { get; set; }

        public string AltMailToAddress { get; set; }

        public string AltMailToCity { get; set; }

        public string AltMailToState { get; set; }

        public int AltMailToZipCode1 { get; set; }

        public int AltMailToZipCode2 { get; set; }

        public string ContractIdCode { get; set; }

        public int StatementMessageNumber { get; set; }

        public string BillingIdCode { get; set; }

        public string UserDefinedField1 { get; set; }

         public string UserDefinedField2 { get; set; }

         public string UserDefinedField3 { get; set; }

         public string UserDefinedField4 { get; set; }

         public string UserDefinedField5 { get; set; }

         public string Unused { get; set; }

       }

    public class Patient
    {

        public static string GetService(string strService)
        {
            string Service = null;
            if (strService != null)
                switch (strService)
                {

                    case "Inpatient":
                        Service = "I";
                        break;
                    case "Outpatient":
                        Service = "O";
                        break;
                    case "Nursing":
                        Service = "N";
                        break;

                    default:
                        Service = " ";
                        break;
                }
            return Service;
        }

        public static string GetAccidentCode(string strAccCode)
        {
            string strCode = null;
        if (strAccCode != null)
                switch (strAccCode)
                {

                    case "Employement":
                        strCode = "E";
                        break;
                    case "Auto":
                        strCode = "A";
                        break;
                    case "Other":
                        strCode = "O";
                        break;

                    default:
                        strCode = " ";
                        break;
                }
            return strCode;
        } 
        
        public static string GetMarStatus(string strStatus)
        {
            string marStatus = null;
            if (strStatus != null)
                switch (strStatus)
                {

                    case "Married": marStatus = "M";
                        break;
                    case "Single": marStatus = "S";
                        break;
                    case "Other":
                        marStatus = "O";
                        break;
                    default:
                        marStatus = " ";
                        break;
                }
            return marStatus;
        }
       

        public static string GetEmploymentStatus(string strStatus)
        {
            string empStatus = null;
            if (strStatus != null)
                switch (strStatus)
                {

                    case "Employed": empStatus = "E";
                        break;
                    case "F/T Student": empStatus = "F";
                        break;
                    case "P/T Student": empStatus = "P";
                        break;
                    case "Blank": empStatus = " ";
                        break;
                    default  :
                        empStatus = " ";
                     break;
                }
            return empStatus;
        }

        public  string StrRecordTypeCode = "02";
        public  string StrDataSet = "EGO";

        public string AccountNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sex { get; set; }
        
        public string PlaceOfService { get; set; }
     
        public string EmpStatus { get; set; }
        public string MarriageStatus { get; set; }
        public string AccidentCode { get; set; }
        public string  SocialSecurityNumber { get; set; }
        public string LocationOfService { get; set; }
        public string AttendingPhysician { get; set; }
        public string ReferringPhysician { get; set; }
        public string HospitalMedicalRecordNumber { get; set; }

        public DateTime AdmitDate { get; set; }
        public DateTime AdmitTime { get; set; }
        public string DischargeDate { get; set; }

        public string DeceasedDate { get; set; }


        public string InjuryDate { get; set; }

        public string DiagnosisCode1 { get; set; }

        public string DiagnosisCode2 { get; set; }

        public string DiagnosisCode3 { get; set; }

        public string DiagnosisCode4 { get; set; }

        public string PrimaryCareDoctor { get; set; }

        public string AccidentState { get; set; }
        public string MilitaryBranch { get; set; }
        public string HospitalCode { get; set; }

      

        public string UserDefinedField1 { get; set; }

        public string UserDefinedField2 { get; set; }

        public string UserDefinedField3 { get; set; }

        public string UserDefinedField4 { get; set; }

        public string UserDefinedField5 { get; set; }

        public string Client { get; set; }

        public string AccountBalance { get; set; }

        public string DebitOrCreditIndicator { get; set; }

        public string Unused { get; set; }

        public static string GetMarriageStatus(string strStatus)
        {
            string marrStatus = null;
            if (strStatus != null)
                switch (strStatus)
                {
                       
                    case "Married" : marrStatus = "M";
                        break;
                    case "Single" :
                        marrStatus = "S";
                        break;
                    case "Other":
                        marrStatus = "O";
                        break;
                    default  :
                        marrStatus = " ";
                     break;
                }
            return marrStatus;
        }
    }

    class Insurance
    {
        
    }

  
    public static class StringUtils
    {
        public static string PadValue(this string s, int width, bool IsNumeric)


        {
            // Fields: Alpha Left justified, blank filled Numeric Right justified, zero filled
           // if (s == null || width <= s.Length) return s;
            if (!string.IsNullOrEmpty(s)  )

                if (s.Length < width)
                {
                    int numDigFill = width - s.Length;
                    if (IsNumeric)
                        s = s + "0".PadLeft(numDigFill);
                    else
                    {
                        s = s + "".PadRight(numDigFill);
                        s = s.ToUpper();
                    }
                }
                else
                {
                    s = IsNumeric ? "0".PadLeft(width) : "".PadRight(width);
                }

            return s;

        }
    }

    public static class EnumHelper
    {
        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

    }
}
