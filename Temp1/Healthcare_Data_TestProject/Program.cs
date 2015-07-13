using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Linq;

namespace Healthcare_Data_TestProject
{
   
    class Program
    {
        static void Main(string[] args)
        {
            //Read xml file for directories

            var xmlDoc = new XmlDocument();
          
            //Need to sanitize the xml to check the filename conent
            // SanitizeXml();
            xmlDoc.Load(@"C:/HealthCareDataTest/DataSource/directories.xml");
           
            var xmlContent = xmlDoc.InnerXml;

            
            var objHealthCareDataDirectories = new HealthCareDataDirectories(xmlContent);

            var sdfn = objHealthCareDataDirectories.SourceDemographicFileName;

            
            var theDataTable = new DataTable("HealthCareDataTable");
            
            using (var sr = new StreamReader(sdfn))
            {
                string strColumns = null;
                var readLine = sr.ReadLine();
                if (readLine != null)
                {
                     var currentLine = readLine.Split(new[] {'|'});


                    if (theDataTable.Columns.Count == 0)
                    {

                        foreach (var t in currentLine)
                        {
                            if (!theDataTable.Columns.Contains(t))
                            {
                                theDataTable.Columns.Add(t); //check already exists col more than once
                                strColumns = (strColumns == null) ? t : strColumns + t + ',';

                            }
                            else
                            {
                                string strDuplicateColn = t + "_1";
                                theDataTable.Columns.Add(strDuplicateColn);
                                strColumns = (strColumns == null) ? strDuplicateColn : strColumns + strDuplicateColn + ',';
                            }
                        }


                    }

                    string lines = null;
                    while ((lines = sr.ReadLine()) != null)
                    {
                        string[] drow = lines.Split('|');

                        theDataTable.NewRow();

                        theDataTable.Rows.Add(drow);
                    }
                }
            }

       

      
            var lstMRNS = (from mrn in theDataTable.AsEnumerable()
                               select mrn["MRN"]).Distinct().ToList(); 

            foreach (string mrn in lstMRNS)
            {
               // var mrnRow =
                   // theDataTable.AsEnumerable().Where(selectedMrn => selectedMrn.Field<string>("MRN") = mrn.ToString());

                var patRow =
                    (from mrnRecord in theDataTable.AsEnumerable()
                     where mrnRecord.Field<string>("MRN") == mrn
                     select mrnRecord).First().Table;
                
                var patientInfo = new Patient();
                foreach (DataRow row in patRow.Rows)
                {


                    //DISCH_DATE,DISCH_TIME,PAT_ADDR_1,PAT_ADDR_2,PAT_CITY,PAT_STATE,PAT_ZIP,PAT_HOME_PHONE,PAT_STATUS,ACCOUNT_CLASS,BASE_CLASS,CONTACT_DATE
                    patientInfo = new Patient
                    {
                        AccountNumber = StringUtils.PadValue(row["HSP_ACCOUNT_ID"].ToString(), 15, false),
                        LastName = StringUtils.PadValue(row["PAT_LAST_NAME"].ToString(), 15, false),
                        FirstName = StringUtils.PadValue(row["PAT_FIRST_NAME"].ToString(), 10, false),
                        MiddleInitial = StringUtils.PadValue(row["PAT_MIDDLE_NAME"].ToString(), 1, false),
                        DateOfBirth = Convert.ToDateTime(row["PAT_DOB"].ToString()), 
                        Sex = row["PAT_GENDER"].ToString() == "FEMALE" ? "F" : "M",
                        SocialSecurityNumber = StringUtils.PadValue(row["SSN"].ToString(), 11, false),
                        LocationOfService = StringUtils.PadValue(row["LOCATION_NAME"].ToString(), 15, false),
                        AttendingPhysician = row.Table.Columns.Contains("PAT_ATT") != false ? StringUtils.PadValue(row["PAT_ATT"].ToString(),30,false) :  StringUtils.PadValue("PAT_ATT_Testing", 30, false),
                        ReferringPhysician = row.Table.Columns.Contains("PAT_REF") != false ? StringUtils.PadValue(row["PAT_REF"].ToString(), 30, false) : StringUtils.PadValue("PAT_REF_Testing", 30, false),
                        HospitalMedicalRecordNumber = StringUtils.PadValue(row["MRN"].ToString(), 18, false),
                        PlaceOfService = StringUtils.PadValue(Patient.GetService(row["PAT_STATUS"].ToString()), 2, false),
                        AdmitDate = Convert.ToDateTime(row["ADMIT_DATE"].ToString()),
                       
                        DischargeDate = row.Table.Columns.Contains("DISCH_DATE") != false ? StringUtils.PadValue(row["DISCH_DATE"].ToString(), 6, false) : StringUtils.PadValue("DDTEST", 6, false),
                        DeceasedDate = row.Table.Columns.Contains("DECSD_DATE") != false ? StringUtils.PadValue(row["DECSD_DATE"].ToString(), 6, false) : StringUtils.PadValue("DDTEST", 6, false),
                        InjuryDate = row.Table.Columns.Contains("INJ_DATE") != false ? StringUtils.PadValue(row["INJ_DATE"].ToString(), 6, false) : StringUtils.PadValue("IJTEST", 6, false),
                        AccidentCode = row.Table.Columns.Contains("ACC_CODE") != false ? StringUtils.PadValue(row["ACC_CODE"].ToString(), 1, false) : StringUtils.PadValue("A", 1, false),
                        MarriageStatus = StringUtils.PadValue(Patient.GetMarriageStatus(row["MARITAL_STATUS"].ToString()), 10, false),
                        DiagnosisCode1 = row.Table.Columns.Contains("DG_CODE_1") != false ? StringUtils.PadValue(row["DG_CODE_1"].ToString(), 7, false) : StringUtils.PadValue("DG1TEST", 7, false),
                        DiagnosisCode2 = row.Table.Columns.Contains("DG_CODE_2") != false ? StringUtils.PadValue(row["DG_CODE_2"].ToString(), 7, false) : StringUtils.PadValue("DG2TEST", 7, false),
                        DiagnosisCode3 = row.Table.Columns.Contains("DG_CODE_3") != false ? StringUtils.PadValue(row["DG_CODE_3"].ToString(), 7, false) : StringUtils.PadValue("DG3TEST", 7, false),
                        DiagnosisCode4 = row.Table.Columns.Contains("DG_CODE_4") != false ? StringUtils.PadValue(row["DG_CODE_4"].ToString(), 7, false) : StringUtils.PadValue("DG4TEST", 7, false),
                        PrimaryCareDoctor = row.Table.Columns.Contains("PRIMARY_CARE_DOCTOR") != false ? StringUtils.PadValue(row["PRIMARY_CARE_DOCTOR"].ToString(), 30, false) : StringUtils.PadValue("PRIMARY_CARE_DOCTOR_TEST", 30, false),
                        AccidentState = row.Table.Columns.Contains("ACC_STATE") != false ? StringUtils.PadValue(row["ACC_STATE"].ToString(), 2, false) : StringUtils.PadValue("AS", 2, false),
                        MilitaryBranch = row.Table.Columns.Contains("MIL_BRN") != false ? StringUtils.PadValue(row["MIL_BRN"].ToString(), 10, false) : StringUtils.PadValue("MILBRNTST", 10, false),
                        HospitalCode = row.Table.Columns.Contains("HSP_CODE") != false ? StringUtils.PadValue(row["HSP_CODE"].ToString(), 3, false) : StringUtils.PadValue("HSP", 3, false),
                        EmpStatus = row.Table.Columns.Contains("EMP_STATUS") != false ? StringUtils.PadValue(row["EMP_STATUS"].ToString(), 10, false) : StringUtils.PadValue("ESTAT_TEST", 10, false),
                        UserDefinedField1 = row.Table.Columns.Contains("USER_DEFINED_1") != false ? StringUtils.PadValue(row["USER_DEFINED_1"].ToString(), 5,  false) : StringUtils.PadValue("USER1", 5, false),
                        UserDefinedField2 = row.Table.Columns.Contains("USER_DEFINED_2") != false ? StringUtils.PadValue(row["USER_DEFINED_2"].ToString(), 10, false) : StringUtils.PadValue("USER2", 10, false),
                        UserDefinedField3 = row.Table.Columns.Contains("USER_DEFINED_3") != false ? StringUtils.PadValue(row["USER_DEFINED_3"].ToString(), 15, false) : StringUtils.PadValue("USER3", 15, false),
                        UserDefinedField4 = row.Table.Columns.Contains("USER_DEFINED_4") != false ? StringUtils.PadValue(row["USER_DEFINED_4"].ToString(), 20, false) : StringUtils.PadValue("USER4", 20, false),
                        UserDefinedField5 = row.Table.Columns.Contains("USER_DEFINED_5") != false ? StringUtils.PadValue(row["USER_DEFINED_5"].ToString(), 30, false) : StringUtils.PadValue("USER5", 30, false),
                        Client = row.Table.Columns.Contains("Client") != false ? StringUtils.PadValue(row["Client"].ToString(), 15, false) : StringUtils.PadValue("Client_TEST", 15, false),
                        AccountBalance = row.Table.Columns.Contains("AccountBalance") != false ? StringUtils.PadValue(row["AccountBalance"].ToString(), 11, false) : StringUtils.PadValue("ACCT", 11, false),
                        DebitOrCreditIndicator = row.Table.Columns.Contains("DebitOrCreditIndicator") != false ? StringUtils.PadValue(row["DebitOrCreditIndicator"].ToString(), 1, false) : StringUtils.PadValue("D", 1, false),
                        Unused = row.Table.Columns.Contains("Unused") != false ? StringUtils.PadValue(row["Unused"].ToString(), 448, false) : StringUtils.PadValue("Unused_TEST", 448, false),


                    };


                    var strPatientRec = patientInfo.StrRecordTypeCode + patientInfo.StrDataSet +
                                                  patientInfo.AccountNumber + patientInfo.LastName + patientInfo.FirstName +
                                                  patientInfo.MiddleInitial + patientInfo.DateOfBirth.Year + patientInfo.DateOfBirth.Month + patientInfo.DateOfBirth.Day +
                                                    patientInfo.Sex + patientInfo.SocialSecurityNumber + patientInfo.LocationOfService +
                                                    patientInfo.AttendingPhysician + patientInfo.ReferringPhysician + patientInfo.HospitalMedicalRecordNumber + patientInfo.PlaceOfService
                                                    + patientInfo.AdmitDate.ToString("yyMMdd") + patientInfo.DischargeDate + patientInfo.DeceasedDate + patientInfo.InjuryDate + patientInfo.AccidentCode +
                                                    patientInfo.MarriageStatus + patientInfo.DiagnosisCode1 + patientInfo.DiagnosisCode2 + patientInfo.DiagnosisCode3 + patientInfo.DiagnosisCode4 +
                                                    patientInfo.PrimaryCareDoctor + patientInfo.AccidentState + patientInfo.MilitaryBranch + patientInfo.HospitalCode + patientInfo.EmpStatus +
                                                     patientInfo.UserDefinedField1 +
                                                    patientInfo.UserDefinedField2 + patientInfo.UserDefinedField3 + patientInfo.UserDefinedField4 + patientInfo.UserDefinedField5 + patientInfo.Client + patientInfo.AccountBalance + 
                                                    patientInfo.DebitOrCreditIndicator + patientInfo.Unused + "$"; 
                    
                   
                    //patient file name
                    var patFN = patientInfo.StrRecordTypeCode + "_" + patientInfo.AccountNumber.TrimEnd() + "_" + "PIN.txt";
                    var DestinationDir = objHealthCareDataDirectories.DestinationConvertedFileDirectory;

                    var file = new FileInfo(Path.Combine(DestinationDir, patFN));
                    if (!file.Exists) // you may not want to overwrite existing files
                    {

                        using (Stream stream = file.OpenWrite())
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(strPatientRec);
                        }
                    }

                }
                    // Finished patient 

                    

                    // Start guarantorInfo data generation 
                var guarRow =
                    (from mrnRecord in theDataTable.AsEnumerable()
                     where mrnRecord.Field<string>("MRN") == mrn
                     select mrnRecord).First().Table;
                
                var guarantorInfo = new Guarantor();
                // ************************* To be done *******************
                foreach (DataRow row in guarRow.Rows)
                {

                    guarantorInfo = new Guarantor
                    {
                     
                    };

                    //var strGuarantorRec = guarantorInfo.StrRecordTypeCode + guarantorInfo.StrDataSet +
                    //                              guarantorInfo.AccountNumber + guarantorInfo.LastName + guarantorInfo.FirstName +
                    //                              guarantorInfo.MiddleInitial + guarantorInfo.DateOfBirth.Year + guarantorInfo.DateOfBirth.Month + guarantorInfo.DateOfBirth.Day;
                    ////patient file name
                    //var patFN = guarantorInfo.StrRecordTypeCode + "_" + guarantorInfo.AccountNumber.TrimEnd() + "_" +
                    //               "PIN.txt";
                    //var DestinationDir = objHealthCareDataDirectories.DestinationConvertedFileDirectory;

                    //// var dir = new DirectoryInfo(@"C:\Temp");
                    //var file = new FileInfo(Path.Combine(DestinationDir, patFN));
                    //if (!file.Exists) // you may not want to overwrite existing files
                    //{
                    //    using (Stream stream = file.OpenWrite())
                    //    using (StreamWriter writer = new StreamWriter(stream))
                    //    {
                    //        writer.Write(strGuarantorRec);
                    //    }
                    //}
                 }

                 // Insurance start
                    var insRow =
                    (from mrnRecord in theDataTable.AsEnumerable()
                     where mrnRecord.Field<string>("MRN") == mrn
                     select mrnRecord).CopyToDataTable();
                
                var insuranceInfo = new Insurance();
                foreach (DataRow row in insRow.Rows)
                {

                    insuranceInfo = new Insurance
                    {
                        
                    };

                    //var insuranceInfoRec = insuranceInfo.StrRecordTypeCode + insuranceInfo.StrDataSet +
                    //                              insuranceInfo.AccountNumber + insuranceInfo.LastName + insuranceInfo.FirstName +
                    //                              insuranceInfo.MiddleInitial + insuranceInfo.DateOfBirth.Year + insuranceInfo.DateOfBirth.Month + insuranceInfo.DateOfBirth.Day;
                    // var patFN = insuranceInfo.StrRecordTypeCode + "_" + insuranceInfo.AccountNumber.TrimEnd() + "_" +
                    //               "PIN.txt";
                    //var DestinationDir = objHealthCareDataDirectories.DestinationConvertedFileDirectory;

                    // var file = new FileInfo(Path.Combine(DestinationDir, patFN));
                    //if (!file.Exists) 
                    //{
                    //    using (Stream stream = file.OpenWrite())
                    //    using (StreamWriter writer = new StreamWriter(stream))
                    //    {
                    //        writer.Write(insuranceInfoRec);
                    //    }
                    //}
                 

                }


            }

            
        }

       
    }
}
