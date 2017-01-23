using ViewModel.BaiFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDataLib.BaiParser
{
    public class BAIProcessor
    {

        public BaiFile CreateBaiFile(string baiData)
        {

            List<String> baiLines = RemoveContdLineFromBaiStream(baiData);
            return ParseBaiData(baiLines);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse bai data. </summary>
        ///
        /// <remarks>   Pdelosreyes, 1/4/2016. </remarks>
        ///
        /// <param name="filePath"> Full pathname of the file. </param>
        ///
        /// <returns>   A BaiFile. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BaiFile ParseBaiFile(string filePath)
        {
            List<String> baiLines = RemoveContdLineFromBai(filePath);
            var baiFile = ParseBaiData(baiLines);
            int position = filePath.LastIndexOf(@"\") + 1;
            baiFile.FileName = filePath.Substring(position, filePath.Length - position);
            return baiFile;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse bai stream. </summary>
        ///
        /// <remarks>   Pdelosreyes, 1/4/2016. </remarks>
        ///
        /// <param name="baiData">  Information describing the bai. </param>
        ///
        /// <returns>   A BaiFile. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BaiFile ParseBaiStream(string baiData)
        {
            List<String> baiLines = RemoveContdLineFromBaiStream(baiData);
            return ParseBaiData(baiLines);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Parse bai file into BaiFile object.
        ///              </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="baiLines"> Content of the Bai File in List of String . </param>
        ///
        /// <returns>   A BaiFile. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static BaiFile ParseBaiData(List<String> baiLines)
        {
            BaiFile baiFile = new BaiFile();
            baiFile.AccountGroupList = new List<AccountGroup>();
            AccountGroup accountGroup = new AccountGroup();
            accountGroup.AccountSummaryList = new List<AccountSummary>();
            AccountSummary accountSummary = new AccountSummary();
            accountSummary.TransactionList = new List<Transaction>();
            Transaction trans = new Transaction();

            foreach (string line in baiLines)
            {
                String[] baiLine = line.Split(',');
                switch (baiLine[0].ToString())
                {
                    case "01": //begin file
                        baiFile = PopulateBaiFile(baiFile, baiLine);
                        break;

                    case "02": //begin group
                        accountGroup = PopulateAccountGroup(accountGroup, baiLine);
                        break;

                    case "03": //begin account
                        accountSummary = PopulateAccountSummary(accountSummary, baiLine);
                        break;

                    case "16": //transaction within account
                        trans = PopulateTransaction(trans, baiLine, baiFile.SenderIdentification);
                        accountSummary.TransactionList.Add(trans);

                        trans = new Transaction();
                        break;

                    case "49": //end account
                        accountGroup.AccountSummaryList.Add(accountSummary);
                        accountSummary = new AccountSummary();
                        accountSummary.TransactionList = new List<Transaction>();

                        break;

                    case "98": //end group
                        baiFile.AccountGroupList.Add(accountGroup);
                        accountGroup = new AccountGroup();
                        accountGroup.AccountSummaryList = new List<AccountSummary>();
                        break;

                    case "99": //end file
                        break;
                }
            }

            return baiFile;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Removes the contd line from bai described by filePath.
        ///              </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="filePath"> Full pathname of the file. </param>
        ///
        /// <returns>   A List&lt;String&gt; </returns>
        ///-------------------------------------------------------------------------------------------------

        private static List<String> RemoveContdLineFromBai(string filePath)
        {
            int counter = 0;
            string line;
            var baiLines = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = file.ReadLine()) != null)
            {

                if (line.Split(',')[0].ToString() != "88")
                {
                    baiLines.Add(line.TrimEnd('/'));
                    counter++;
                }
                if (line.Split(',')[0].ToString() == "88")
                {
                    counter--;
                    baiLines[counter] = baiLines[counter].TrimEnd('/') + line.Remove(0, 2).TrimEnd('/');
                    counter++;
                }
            }
            file.Close();
            return baiLines;
        }

        private static List<String> RemoveContdLineFromBaiStream(string baiData)
        {
            int counter = 0;
            List<String> baiLines = baiData.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string line in baiLines)
            {
                if (line.Split(',')[0].ToString() != "88")
                {
                    baiLines.Add(line.TrimEnd('/'));
                    counter++;
                }
                if (line.Split(',')[0].ToString() == "88")
                {
                    counter--;
                    baiLines[counter] = baiLines[counter].TrimEnd('/') + line.Remove(0, 2).TrimEnd('/');
                    counter++;
                }
            }
            return baiLines;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Populate bai file object. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="baiFile">  The bai file object. </param>
        /// <param name="baiLine">  The bai line. </param>
        ///
        /// <returns>   A BaiFile. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static BaiFile PopulateBaiFile(BaiFile baiFile, String[] baiLine)
        {
            baiFile.RecordCode = baiLine[0];
            baiFile.SenderIdentification = baiLine[1];
            baiFile.ReceiverIdentification = baiLine[2];
            baiFile.FileCreationDate = baiLine[3];
            baiFile.FileCreationTime = baiLine[4];
            baiFile.FileIdentificationNumber = baiLine[5];
            baiFile.PhysicalRecordLength = baiLine[6];
            baiFile.BlockSize = baiLine[7];
            baiFile.VersionNumber = baiLine[8];

            return baiFile;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Populate account group object. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="accountGroup"> Group the account belongs to. </param>
        /// <param name="baiLine">      The bai line. </param>
        ///
        /// <returns>   An AccountGroup. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static AccountGroup PopulateAccountGroup(AccountGroup accountGroup, String[] baiLine)
        {
            accountGroup.RecordCode = baiLine[0];
            accountGroup.UltimateReceiverIdentification = baiLine[1];
            accountGroup.OriginatorIdentification = baiLine[2];
            accountGroup.GroupStatus = baiLine[3];
            accountGroup.AsOfDate = baiLine[4];
            accountGroup.AsOfTime = baiLine[5];
            accountGroup.CurrencyCode = baiLine[6];
            accountGroup.AsOfDateModifier = baiLine[7];

            return accountGroup;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Populate account summary object. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="accountSummary">   The account summary. </param>
        /// <param name="baiLine">          The bai line. </param>
        ///
        /// <returns>   An AccountSummary. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static AccountSummary PopulateAccountSummary(AccountSummary accountSummary, String[] baiLine)
        {
            accountSummary.RecordCode = baiLine[0];
            accountSummary.CustomerAccountNumber = baiLine[1];
            accountSummary.CurrencyCode = baiLine[2];

            //accountSummary can contain a variable number of TypeCode fields in each record
            accountSummary.TypeCodeList = new List<TypeCodes>();
            for (int i = 1; i <= (baiLine.Length - 3) / 4; i++)
            {
                TypeCodes tc = new TypeCodes();
                tc.TypeCode = baiLine[i * 3];
                tc.Amount = baiLine[i * 3 + 1];
                tc.ItemCount = baiLine[i * 3 + 2];
                tc.FundsType = baiLine[i * 3 + 3];
                accountSummary.TypeCodeList.Add(tc);
            }

            return accountSummary;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Populate transaction object. </summary>
        ///
        /// <remarks>   Rphilavanh, 20151123. </remarks>
        ///
        /// <param name="trans">    The transaction. </param>
        /// <param name="baiLine">  The bai line. </param>
        ///
        /// <returns>   A Transaction. </returns>
        ///-------------------------------------------------------------------------------------------------

        private static Transaction PopulateTransaction(Transaction trans, String[] baiLine, string SenderId)
        {
            trans.SenderIdentification = SenderId;
            trans.RecordCode = baiLine[0];
            trans.TypeCode = baiLine[1];
            trans.Amount = baiLine[2];
            trans.FundsType = baiLine[3];
            trans.BankReferenceNumber = baiLine[4];
            trans.CustomerReferenceNumber = baiLine[5];

            //Concatenate text containing comma
            string txt = "";
            for (int i = 6; i < baiLine.Length; i++) { txt = txt + "," + baiLine[i]; }
            trans.Text = txt.Substring(1); //Remove initial comma

            return trans;
        }

    }
}
