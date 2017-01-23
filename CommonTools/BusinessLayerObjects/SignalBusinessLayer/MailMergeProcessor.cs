using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using SignalEFDataModel.SGNL_WAREHOUSE;
using CommonUtils.DataStructures;
using CommonUtils.Email;
using System.Collections;

namespace BusinessLayer
{
    public class MailMergeProcessor
    {
        public ListDictionary Replacements { get; set; }
        public string RegEx {get; set;}
        public static Regex Re { get; set; }
        public List<String> Images { get; set; }
        private static string SignalLogoPath = @"Assets\SignalLogo.png";
        private static readonly Regex t4Re = new Regex(@"\<#=(\w+)\#>", RegexOptions.Compiled);
        private EmailObject emailValues;

        public EmailObject SignalMessage { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// emailTo: 
        /// an override for sending directly to a doctor's email
        ///  from the database! Alway's assign this outside of production.
        ///-------------------------------------------------------------------------------------------------
        public string emailTo { get; set; }

        public MailMergeProcessor()
        {
            Replacements = new ListDictionary();
            if (!String.IsNullOrWhiteSpace(RegEx))
                Re = new Regex(RegEx);
            if (Images == null)
                Images = new List<string>();
            if (String.IsNullOrWhiteSpace(emailTo))
                emailTo = emailValues.EmailTo ?? String.Empty;
        }

        public MailMergeProcessor(EmailObject emailValues)
        {
            this.emailValues = emailValues;
            Replacements = new ListDictionary();
            if (!String.IsNullOrWhiteSpace(RegEx))
                Re = new Regex(RegEx);
            if (Images == null)
                Images = new List<string>();
            emailTo = emailValues.EmailTo ?? String.Empty;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   /public void AddDictionaryValues(string caseNumber, string emailType) </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160823. </remarks>
        ///
        /// <param name="caseNumber">   The case number. </param>
        ///
        /// <returns>   A vi_SignalMailMerge. </returns>
        ///-------------------------------------------------------------------------------------------------
        private vi_SignalMailMerge AddDictionaryValues(string caseNumber)
        {
            vi_SignalMailMerge caseViewRow = null;
            SGNL_WAREHOUSEEntities signalWarehouse = new SGNL_WAREHOUSEEntities();
            caseViewRow = (from value in signalWarehouse.vi_SignalMailMerge
                               where value.CaseNumber == caseNumber
                               //where value.DoctorEmailType == emailType
                               select value).FirstOrDefault();
            if (caseViewRow != null)
            {
                var rowDictionary = caseViewRow.ToDictionary();
                foreach (var pair in rowDictionary)
                {
                    Replacements.Add(pair.Key, pair.Value);
                }
            }
            return caseViewRow;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Adds a dictionary values. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160825. </remarks>
        ///
        /// <param name="emailTemplate">   html (or text) to be merged </param>
        /// <param name="dictionary">   Dictionary values to apply </param>
        ///
        /// <returns>   Body after values merged with template. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string MergeValues(string emailTemplate, IDictionary dictionary)
        {
            ListDictionary listDictionary = (ListDictionary)dictionary.ToDictionary();
            return ApplyDictionary(emailTemplate, listDictionary);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Email body. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160811. </remarks>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        private string EmailBodyHtml(string html)
        {
            return ApplyDictionary(html.Replace("<# =","<#="));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Email body file. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160823. </remarks>
        ///
        /// <param name="path"> Full pathname of the file. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string EmailBodyFile(string path)
        {
            string htmlPath = "EmailTemplates/emailTemplate.html";
            string htmlBody = File.ReadAllText(path ?? htmlPath);
            return ApplyDictionary(htmlBody.Replace("<# =", "<#="));
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Email body text. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160823. </remarks>
        ///
        /// <param name="html"> The HTML. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string EmailBodyToText(string html)
        {
            var htmlToText = new CommonUtils.Html.HtmlToText();
            return htmlToText.ConvertHtml(html);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Applies the dictionary described by txtToParse. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160823. </remarks>
        ///
        /// <param name="txtToParse">   The text to parse. </param>
        ///
        /// <returns>   A string. </returns>
        ///-------------------------------------------------------------------------------------------------

        public string ApplyDictionary(string t4EmailTemplate)
        {
            return t4Re.Replace(t4EmailTemplate, match => { return Replacements.Contains(match.Groups[1].Value) ? Replacements[match.Groups[1].Value].ToString() : match.Value; });
        }

        private string ApplyDictionary(string emailTemplate, ListDictionary listDictionary)
        {
            return Re.Replace(emailTemplate, match => { return listDictionary.Contains(match.Groups[1].Value) ? listDictionary[match.Groups[1].Value].ToString() : match.Value; });
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Sends a mail. </summary>
        ///
        /// <remarks>   Pdelosreyes, 20160811. </remarks>
        ///
        /// <param name="caseNumber">    The Case Number. </param>
        ///-------------------------------------------------------------------------------------------------
        //public void SendMail(string caseNumber, EmailObject SignalMessage)
        public void SendMail(string caseNumber)
        {
            var imagePaths = new List<string>();
            imagePaths.Add(SignalLogoPath);
            var emailMessage = new CommonUtils.Email.EmailMessage();
            foreach (var image in imagePaths)
            {
                string key = Path.GetFileNameWithoutExtension(image);
                string value = "<img src =\"cid:" + key + "\" />";
                Replacements.Add(key, value);
                emailMessage.AddImageObject(image);
            }

            var dataRow = AddDictionaryValues(caseNumber);
            string emailBodyHtml = EmailBodyHtml(dataRow.DoctorEmailTemplate);
            emailValues.EmailBody = emailBodyHtml;
            emailValues.EmailSubject = dataRow.DoctorEmailSubject;

#if PRODUCTION_RELEASE
            emailValues.EmailTo = dataRow.DoctorEmailAddress;
#else
            emailValues.EmailTo = emailTo;
#endif
            emailMessage.CreateEmailObject(emailValues);
            emailMessage.Send();
        }
    }
}

