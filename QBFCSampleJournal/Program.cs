//The following sample code is generated as an illustration of
//Creating requests and parsing responses ONLY
//This code is NOT intended to show best practices or ideal code
//Use at your most careful discretion

using System;
using System.Net;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

using System.Data;
using System.IO;
using Interop.QBFC16;

namespace com.intuit.idn.samples
{
    public class Sample
    {
        static void Main(string[] args)
        {
            Sample sample = new Sample();
            try
            {

                sample.DoJournalEntryAdd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void DoJournalEntryAdd()
        {
            bool sessionBegun = false;
            bool connectionOpen = false;
            QBSessionManager sessionManager = null;

            try
            {
                //Create the session Manager object
                sessionManager = new QBSessionManager();

                //Create the message set request object to hold our request
                IMsgSetRequest requestMsgSet = sessionManager.CreateMsgSetRequest("US", 16, 0);
                requestMsgSet.Attributes.OnError = ENRqOnError.roeContinue;

                BuildJournalEntryAddRq(requestMsgSet);

                //Connect to QuickBooks and begin a session
                sessionManager.OpenConnection("", "Sample Code from OSR");
                connectionOpen = true;
                sessionManager.BeginSession("", ENOpenMode.omDontCare);
                sessionBegun = true;

                //Send the request and get the response from QuickBooks
                IMsgSetResponse responseMsgSet = sessionManager.DoRequests(requestMsgSet);

                //End the session and close the connection to QuickBooks
                sessionManager.EndSession();
                sessionBegun = false;
                sessionManager.CloseConnection();
                connectionOpen = false;

                WalkJournalEntryAddRs(responseMsgSet);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error:{e.Message}");
                if (sessionBegun)
                {
                    sessionManager.EndSession();
                }
                if (connectionOpen)
                {
                    sessionManager.CloseConnection();
                }
            }
        }
        void BuildJournalEntryAddRq(IMsgSetRequest requestMsgSet)
        {
            IJournalEntryAdd JournalEntryAddRq = requestMsgSet.AppendJournalEntryAddRq();
            JournalEntryAddRq.TxnDate.SetValue(DateTime.Parse("02/12/2025"));
            //JournalEntryAddRq.IsAdjustment.SetValue(true);
            
            IORJournalLine ORJournalLineListElement16394 = JournalEntryAddRq.ORJournalLineList.Append();
            string ORJournalLineListElementType16395 = "JournalDebitLine";
            if (ORJournalLineListElementType16395 == "JournalDebitLine")
            {
                ORJournalLineListElement16394.JournalDebitLine.AccountRef.FullName.SetValue("Revenue:Cal.net");
                ORJournalLineListElement16394.JournalDebitLine.Amount.SetValue(4092.86); // Debit Amount
                ORJournalLineListElement16394.JournalDebitLine.Memo.SetValue("Customer: Test - Accounts Receivable");
                ORJournalLineListElement16394.JournalDebitLine.EntityRef.FullName.SetValue("Test");  // Customer Name

            }

            
            

            // Second Journal Line (Credit)
            IORJournalLine ORJournalLineListElement16397 = JournalEntryAddRq.ORJournalLineList.Append();
            if (ORJournalLineListElementType16395 == "JournalDebitLine")
            {
                ORJournalLineListElement16397.JournalDebitLine.AccountRef.FullName.SetValue("Cash");
                ORJournalLineListElement16397.JournalDebitLine.Amount.SetValue(10784.16); // Debit Amount
                ORJournalLineListElement16397.JournalDebitLine.Memo.SetValue("Customer: Test - Cash");
                ORJournalLineListElement16397.JournalDebitLine.EntityRef.FullName.SetValue("Test");  // Customer Name

            }

            IORJournalLine ORJournalLineListElement16381 = JournalEntryAddRq.ORJournalLineList.Append();
           string ORJournalLineListElementType16382 = "JournalCreditLine";
            if (ORJournalLineListElementType16382 == "JournalCreditLine")
            {
                ORJournalLineListElement16381.JournalCreditLine.AccountRef.FullName.SetValue("Earned Revenue");
                ORJournalLineListElement16381.JournalCreditLine.Amount.SetValue(5382.86); // Credit Amount
                ORJournalLineListElement16381.JournalCreditLine.Memo.SetValue("Customer: Test - Earned Revenue");
                ORJournalLineListElement16381.JournalCreditLine.EntityRef.FullName.SetValue("Test");  // Customer Name

            }
           ;
            IORJournalLine ORJournalLineListElement16383 = JournalEntryAddRq.ORJournalLineList.Append();
            string ORJournalLineListElementType16384 = "JournalCreditLine";
            if (ORJournalLineListElementType16384 == "JournalCreditLine")
            {
                ORJournalLineListElement16383.JournalCreditLine.AccountRef.FullName.SetValue("Deferred Revenue");
                ORJournalLineListElement16383.JournalCreditLine.Amount.SetValue(9494.16); // Credit Amount
                ORJournalLineListElement16383.JournalCreditLine.Memo.SetValue("Customer: Test - Unearned Revenue");
                ORJournalLineListElement16383.JournalCreditLine.EntityRef.FullName.SetValue("Test");  // Customer Name

            }
        }






            void WalkJournalEntryAddRs(IMsgSetResponse responseMsgSet)
            {
                if (responseMsgSet == null) return;
                IResponseList responseList = responseMsgSet.ResponseList;
                if (responseList == null) return;
                //if we sent only one request, there is only one response, we'll walk the list for this sample
                for (int i = 0; i < responseList.Count; i++)
                {
                    IResponse response = responseList.GetAt(i);
                //check the status code of the response, 0=ok, >0 is warning
                if (response.StatusCode == 0 && response.Detail != null)
                {
                    //the request-specific response is in the details, make sure we have some

                    //make sure the response is the type we're expecting
                    ENResponseType responseType = (ENResponseType)response.Type.GetValue();
                    if (responseType == ENResponseType.rtJournalEntryAddRs)
                    {
                        //upcast to more specific type here, this is safe because we checked with response.Type check above
                        IJournalEntryRet JournalEntryRet = (IJournalEntryRet)response.Detail;
                        WalkJournalEntryRet(JournalEntryRet);
                    }

                }
                else
                {
                    Console.WriteLine($"STATUS MESSAGE:{response.StatusMessage}");
                    Console.WriteLine($"STATUS CODE:{response.StatusCode}");
                }
                    
                }
            }




            void WalkJournalEntryRet(IJournalEntryRet JournalEntryRet)
            {
                if (JournalEntryRet == null) return;
                //Go through all the elements of IJournalEntryRet
                //Get value of TxnID
                string TxnID16396 = (string)JournalEntryRet.TxnID.GetValue();
                Console.Write($"TxnID16396:{TxnID16396},  ");
                //Get value of TimeCreated
                DateTime TimeCreated16397 = (DateTime)JournalEntryRet.TimeCreated.GetValue();
                Console.Write($"TimeCreated16397:{TimeCreated16397},  ");
                //Get value of TimeModified
                DateTime TimeModified16398 = (DateTime)JournalEntryRet.TimeModified.GetValue();
                Console.Write($"TimeModified16398:{TimeModified16398},  ");
                //Get value of EditSequence
                string EditSequence16399 = (string)JournalEntryRet.EditSequence.GetValue();
                Console.Write($"EditSequence16399:{EditSequence16399},  ");
                //Get value of TxnNumber
                if (JournalEntryRet.TxnNumber != null)
                {
                    int TxnNumber16400 = (int)JournalEntryRet.TxnNumber.GetValue();
                    Console.Write($"TxnNumber16400:{TxnNumber16400},  ");
                }
                //Get value of TxnDate
                DateTime TxnDate16401 = (DateTime)JournalEntryRet.TxnDate.GetValue();
                //Get value of RefNumber
                if (JournalEntryRet.RefNumber != null)
                {
                    string RefNumber16402 = (string)JournalEntryRet.RefNumber.GetValue();
                    Console.Write($"RefNumber16402:{RefNumber16402},  ");

                }
                //Get value of IsAdjustment
                if (JournalEntryRet.IsAdjustment != null)
                {
                    bool IsAdjustment16403 = (bool)JournalEntryRet.IsAdjustment.GetValue();
                    Console.Write($"IsAdjustment16403:{IsAdjustment16403},  ");
                    Console.WriteLine("");
                }
                                //////Get value of ExternalGUID
               
                if (JournalEntryRet.ORJournalLineList != null)
                {
                    for (int i16410 = 0; i16410 < JournalEntryRet.ORJournalLineList.Count; i16410++)
                    {
                        IORJournalLine ORJournalLine16411 = JournalEntryRet.ORJournalLineList.GetAt(i16410);
                        if (ORJournalLine16411.JournalDebitLine != null)
                        {
                            if (ORJournalLine16411.JournalDebitLine != null)
                            {
                                //Get value of TxnLineID
                                if (ORJournalLine16411.JournalDebitLine.TxnLineID != null)
                                {
                                    string TxnLineID16412 = (string)ORJournalLine16411.JournalDebitLine.TxnLineID.GetValue();
                                    Console.Write($"TxnLineID16412:{TxnLineID16412},  ");
                                }
                                if (ORJournalLine16411.JournalDebitLine.AccountRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalDebitLine.AccountRef.ListID != null)
                                    {
                                        string ListID16413 = (string)ORJournalLine16411.JournalDebitLine.AccountRef.ListID.GetValue();
                                        Console.Write($"ListID16413:{ListID16413},  ");
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalDebitLine.AccountRef.FullName != null)
                                    {
                                        string FullName16414 = (string)ORJournalLine16411.JournalDebitLine.AccountRef.FullName.GetValue();
                                        Console.Write($"FullName16414:{FullName16414},  ");
                                    }
                                }
                                //Get value of Amount
                                if (ORJournalLine16411.JournalDebitLine.Amount != null)
                                {
                                    double Amount16415 = (double)ORJournalLine16411.JournalDebitLine.Amount.GetValue();
                                    Console.Write($"Amount16415:{Amount16415},  ");
                                }
                                //Get value of Memo
                                if (ORJournalLine16411.JournalDebitLine.Memo != null)
                                {
                                    string Memo16416 = (string)ORJournalLine16411.JournalDebitLine.Memo.GetValue();
                                    Console.Write($"Memo16416:{Memo16416},  ");
                                }
                                if (ORJournalLine16411.JournalDebitLine.EntityRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalDebitLine.EntityRef.ListID != null)
                                    {
                                        string ListID16417 = (string)ORJournalLine16411.JournalDebitLine.EntityRef.ListID.GetValue();
                                        Console.Write($"ListID16417:{ListID16417},  ");
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalDebitLine.EntityRef.FullName != null)
                                    {
                                        string FullName16418 = (string)ORJournalLine16411.JournalDebitLine.EntityRef.FullName.GetValue();
                                        Console.Write($"FullName16418:{FullName16418},  ");
                                    }
                                }
                                if (ORJournalLine16411.JournalDebitLine.ClassRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalDebitLine.ClassRef.ListID != null)
                                    {
                                        string ListID16419 = (string)ORJournalLine16411.JournalDebitLine.ClassRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalDebitLine.ClassRef.FullName != null)
                                    {
                                        string FullName16420 = (string)ORJournalLine16411.JournalDebitLine.ClassRef.FullName.GetValue();
                                    }
                                }
                                if (ORJournalLine16411.JournalDebitLine.ItemSalesTaxRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalDebitLine.ItemSalesTaxRef.ListID != null)
                                    {
                                        string ListID16421 = (string)ORJournalLine16411.JournalDebitLine.ItemSalesTaxRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalDebitLine.ItemSalesTaxRef.FullName != null)
                                    {
                                        string FullName16422 = (string)ORJournalLine16411.JournalDebitLine.ItemSalesTaxRef.FullName.GetValue();
                                    }
                                }
                                //Get value of BillableStatus
                                if (ORJournalLine16411.JournalDebitLine.BillableStatus != null)
                                {
                                    ENBillableStatus BillableStatus16423 = (ENBillableStatus)ORJournalLine16411.JournalDebitLine.BillableStatus.GetValue();
                                }
                            }
                        }
                        if (ORJournalLine16411.JournalCreditLine != null)
                        {
                            if (ORJournalLine16411.JournalCreditLine != null)
                            {
                                //Get value of TxnLineID
                                if (ORJournalLine16411.JournalCreditLine.TxnLineID != null)
                                {
                                    string TxnLineID16424 = (string)ORJournalLine16411.JournalCreditLine.TxnLineID.GetValue();
                                }
                                if (ORJournalLine16411.JournalCreditLine.AccountRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalCreditLine.AccountRef.ListID != null)
                                    {
                                        string ListID16425 = (string)ORJournalLine16411.JournalCreditLine.AccountRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalCreditLine.AccountRef.FullName != null)
                                    {
                                        string FullName16426 = (string)ORJournalLine16411.JournalCreditLine.AccountRef.FullName.GetValue();
                                    }
                                }
                                //Get value of Amount
                                if (ORJournalLine16411.JournalCreditLine.Amount != null)
                                {
                                    double Amount16427 = (double)ORJournalLine16411.JournalCreditLine.Amount.GetValue();
                                }
                                //Get value of Memo
                                if (ORJournalLine16411.JournalCreditLine.Memo != null)
                                {
                                    string Memo16428 = (string)ORJournalLine16411.JournalCreditLine.Memo.GetValue();
                                }
                                if (ORJournalLine16411.JournalCreditLine.EntityRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalCreditLine.EntityRef.ListID != null)
                                    {
                                        string ListID16429 = (string)ORJournalLine16411.JournalCreditLine.EntityRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalCreditLine.EntityRef.FullName != null)
                                    {
                                        string FullName16430 = (string)ORJournalLine16411.JournalCreditLine.EntityRef.FullName.GetValue();
                                    }
                                }
                                if (ORJournalLine16411.JournalCreditLine.ClassRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalCreditLine.ClassRef.ListID != null)
                                    {
                                        string ListID16431 = (string)ORJournalLine16411.JournalCreditLine.ClassRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalCreditLine.ClassRef.FullName != null)
                                    {
                                        string FullName16432 = (string)ORJournalLine16411.JournalCreditLine.ClassRef.FullName.GetValue();
                                    }
                                }
                                if (ORJournalLine16411.JournalCreditLine.ItemSalesTaxRef != null)
                                {
                                    //Get value of ListID
                                    if (ORJournalLine16411.JournalCreditLine.ItemSalesTaxRef.ListID != null)
                                    {
                                        string ListID16433 = (string)ORJournalLine16411.JournalCreditLine.ItemSalesTaxRef.ListID.GetValue();
                                    }
                                    //Get value of FullName
                                    if (ORJournalLine16411.JournalCreditLine.ItemSalesTaxRef.FullName != null)
                                    {
                                        string FullName16434 = (string)ORJournalLine16411.JournalCreditLine.ItemSalesTaxRef.FullName.GetValue();
                                    }
                                }
                                //Get value of BillableStatus
                                if (ORJournalLine16411.JournalCreditLine.BillableStatus != null)
                                {
                                    ENBillableStatus BillableStatus16435 = (ENBillableStatus)ORJournalLine16411.JournalCreditLine.BillableStatus.GetValue();
                                }
                            }
                        }
                    }
                }
                if (JournalEntryRet.DataExtRetList != null)
                {
                    for (int i16436 = 0; i16436 < JournalEntryRet.DataExtRetList.Count; i16436++)
                    {
                        IDataExtRet DataExtRet = JournalEntryRet.DataExtRetList.GetAt(i16436);
                        //Get value of OwnerID
                        if (DataExtRet.OwnerID != null)
                        {
                            string OwnerID16437 = (string)DataExtRet.OwnerID.GetValue();
                        }
                        //Get value of DataExtName
                        string DataExtName16438 = (string)DataExtRet.DataExtName.GetValue();
                        //Get value of DataExtType
                        ENDataExtType DataExtType16439 = (ENDataExtType)DataExtRet.DataExtType.GetValue();
                        //Get value of DataExtValue
                        string DataExtValue16440 = (string)DataExtRet.DataExtValue.GetValue();
                    }
                }
            }




        }
    }