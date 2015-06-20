using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Survey_Scheduler
{
            public partial class Survey_SchedulerApp : Form
        {

            int AccountId, ProjectId, ProgrammeId, ParticipantId, CandidateId;
            string AccountName, ProjectName, ProgrammeName, ParticipantName, CandidateName;
            DateTime EmailDate;
            Boolean EmailStatus;

            public Survey_SchedulerApp()
            {
                InitializeComponent();
            }

            public void SchedulerApp_Load(object sender, EventArgs e)
            {
                try
                {
                    //int i = 0;
                    //int j = 1 / i;
                    SendReminder1Email();
                    SendReminder2Email();
                    SendReminder3Email();

                    //   SendReportAvailableEmail();

                    //  SendParticipantReminder1();
                    //  SendParticipantReminder2();

                    //SendTestEmail();
                }
                catch (Exception ex)
                {
                    Survey_LookUp.HandleException(ex);
                }

                this.Close();
            }

            private void SendTestEmail()
            {
                try
                {
                    bool status = false;
                    //bool status1 = false;

                    status = SendEmail.Send("o''test@damcogroup.com"
                                     , ""
                                     , ""
                                     , ""
                                     , "Test Email "
                                     , "Hello Sumnesh, This is a test mail."
                                     , true
                                     , "");

                    //status1 = SendEmail.Send("sumneshl@damcogroup.com"
                    //                 , ""
                    //                 , ""
                    //                 , ""
                    //                 , "Test Email "
                    //                 , "Hello Sumnesh, This is a test mail."
                    //                 , true
                    //                 , "");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            private void SendReminder1Email()
            {
                try
                {
                    string emailImage = "";
                    string questionnaireID = "";
                    string candidateID = "";
                    string emailText = "";

                    string imagePath = System.Configuration.ConfigurationSettings.AppSettings["ImagePath"].ToString();

                    DataTable dtReminder1 = new DataTable();
                    dtReminder1 = Survey_LookUp.FetchDataForReminder1();

                    if (dtReminder1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReminder1.Rows.Count; i++)
                        {
                            emailImage = "";
                            emailText = "";

                            questionnaireID = PasswordGenerator.EnryptString(dtReminder1.Rows[i]["QuestionnaireID"].ToString());
                            candidateID = PasswordGenerator.EnryptString(dtReminder1.Rows[i]["AsgnDetailID"].ToString());
                            string urlPath = System.Configuration.ConfigurationSettings.AppSettings["Survey_FeedbackURL"].ToString();

                            string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                            emailText = dtReminder1.Rows[i]["EmailText"].ToString();
                            emailText = emailText.Replace("[LINK]", link);

                            if (dtReminder1.Rows[i]["EmailImage"].ToString() != "")
                                emailImage = imagePath + dtReminder1.Rows[i]["EmailImage"].ToString();

                            EmailStatus = SendEmail.Send(dtReminder1.Rows[i]["CandidateEmail"].ToString()
                                                    , ""
                                                    , ""
                                                    , ""
                                                    , dtReminder1.Rows[i]["Subject"].ToString()
                                                    , emailText
                                                    , true
                                                    , emailImage);

                            AccountId = Convert.ToInt32(dtReminder1.Rows[i]["AccountId"].ToString());
                            ProjectId = Convert.ToInt32(dtReminder1.Rows[i]["ProjectID"].ToString());
                            ProgrammeId = Convert.ToInt32(dtReminder1.Rows[i]["ProgrammeID"].ToString());
                            //ParticipantId = Convert.ToInt32(dtReminder1.Rows[i]["UserID"].ToString());
                            ParticipantId = 0;
                            CandidateId = Convert.ToInt32(dtReminder1.Rows[i]["AsgnDetailID"].ToString());

                            AccountName = dtReminder1.Rows[i]["OrganisationName"].ToString();
                            ProjectName = dtReminder1.Rows[i]["Title"].ToString();
                            ProgrammeName = dtReminder1.Rows[i]["ProgrammeName"].ToString();
                            //ParticipantName = dtReminder1.Rows[i]["ParticipantName"].ToString();
                            CandidateName = dtReminder1.Rows[i]["CandidateName"].ToString();

                            EmailDate = Convert.ToDateTime(dtReminder1.Rows[i]["EmailDate"].ToString());

                            Survey_LookUp.InsertReminderData(5, AccountId, AccountName, ParticipantId, CandidateName, ProjectId, ProjectName, ProgrammeId, ProgrammeName, EmailDate, EmailStatus);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Survey_LookUp.HandleException(ex);
                }
            }

            private void SendReminder2Email()
            {
                try
                {
                    string emailImage = "";
                    string questionnaireID = "";
                    string candidateID = "";
                    string emailText = "";

                    string imagePath = System.Configuration.ConfigurationSettings.AppSettings["ImagePath"].ToString();

                    DataTable dtReminder2 = new DataTable();
                    dtReminder2 = Survey_LookUp.FetchDataForReminder2();

                    if (dtReminder2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReminder2.Rows.Count; i++)
                        {
                            emailImage = "";
                            emailText = "";

                            questionnaireID = PasswordGenerator.EnryptString(dtReminder2.Rows[i]["QuestionnaireID"].ToString());
                            candidateID = PasswordGenerator.EnryptString(dtReminder2.Rows[i]["AsgnDetailID"].ToString());
                            string urlPath = System.Configuration.ConfigurationSettings.AppSettings["Survey_FeedbackURL"].ToString();

                            string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                            emailText = dtReminder2.Rows[i]["EmailText"].ToString();
                            emailText = emailText.Replace("[LINK]", link);

                            if (dtReminder2.Rows[i]["EmailImage"].ToString() != "")
                                emailImage = imagePath + dtReminder2.Rows[i]["EmailImage"].ToString();

                            EmailStatus = SendEmail.Send(dtReminder2.Rows[i]["CandidateEmail"].ToString()
                                                    , ""
                                                    , ""
                                                    , ""
                                                    , dtReminder2.Rows[i]["Subject"].ToString()
                                                    , emailText
                                                    , true
                                                    , emailImage);

                            AccountId = Convert.ToInt32(dtReminder2.Rows[i]["AccountId"].ToString());
                            ProjectId = Convert.ToInt32(dtReminder2.Rows[i]["ProjectID"].ToString());
                            ProgrammeId = Convert.ToInt32(dtReminder2.Rows[i]["ProgrammeID"].ToString());
                            //ParticipantId = Convert.ToInt32(dtReminder2.Rows[i]["UserID"].ToString());
                            ParticipantId = 0;
                            CandidateId = Convert.ToInt32(dtReminder2.Rows[i]["AsgnDetailID"].ToString());

                            AccountName = dtReminder2.Rows[i]["OrganisationName"].ToString();
                            ProjectName = dtReminder2.Rows[i]["Title"].ToString();
                            ProgrammeName = dtReminder2.Rows[i]["ProgrammeName"].ToString();
                            //ParticipantName = dtReminder2.Rows[i]["ParticipantName"].ToString();
                            CandidateName = dtReminder2.Rows[i]["CandidateName"].ToString();

                            EmailDate = Convert.ToDateTime(dtReminder2.Rows[i]["EmailDate"].ToString());

                            Survey_LookUp.InsertReminderData(6, AccountId, AccountName, ParticipantId, CandidateName, ProjectId, ProjectName, ProgrammeId, ProgrammeName, EmailDate, EmailStatus);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Survey_LookUp.HandleException(ex);
                }
            }

            private void SendReminder3Email()
            {
                try
                {
                    string emailImage = "";
                    string questionnaireID = "";
                    string candidateID = "";
                    string emailText = "";

                    string imagePath = System.Configuration.ConfigurationSettings.AppSettings["ImagePath"].ToString();

                    DataTable dtReminder3 = new DataTable();
                    dtReminder3 = Survey_LookUp.FetchDataForReminder3();

                    if (dtReminder3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtReminder3.Rows.Count; i++)
                        {
                            emailImage = "";
                            emailText = "";

                            questionnaireID = PasswordGenerator.EnryptString(dtReminder3.Rows[i]["QuestionnaireID"].ToString());
                            candidateID = PasswordGenerator.EnryptString(dtReminder3.Rows[i]["AsgnDetailID"].ToString());
                            string urlPath = System.Configuration.ConfigurationSettings.AppSettings["Survey_FeedbackURL"].ToString();

                            string link = "<a Target='_BLANK' href= '" + urlPath + "Feedback.aspx?QID=" + questionnaireID + "&CID=" + candidateID + "' >Click Link</a> ";

                            emailText = dtReminder3.Rows[i]["EmailText"].ToString();
                            emailText = emailText.Replace("[LINK]", link);

                            if (dtReminder3.Rows[i]["EmailImage"].ToString() != "")
                                emailImage = imagePath + dtReminder3.Rows[i]["EmailImage"].ToString();

                            EmailStatus = SendEmail.Send(dtReminder3.Rows[i]["CandidateEmail"].ToString()
                                                    , ""
                                                    , ""
                                                    , ""
                                                    , dtReminder3.Rows[i]["Subject"].ToString()
                                                    , emailText
                                                    , true
                                                    , emailImage);

                            AccountId = Convert.ToInt32(dtReminder3.Rows[i]["AccountId"].ToString());
                            ProjectId = Convert.ToInt32(dtReminder3.Rows[i]["ProjectID"].ToString());
                            ProgrammeId = Convert.ToInt32(dtReminder3.Rows[i]["ProgrammeID"].ToString());
                            //ParticipantId = Convert.ToInt32(dtReminder3.Rows[i]["UserID"].ToString());
                            ParticipantId = 0;
                            CandidateId = Convert.ToInt32(dtReminder3.Rows[i]["AsgnDetailID"].ToString());

                            AccountName = dtReminder3.Rows[i]["OrganisationName"].ToString();
                            ProjectName = dtReminder3.Rows[i]["Title"].ToString();
                            ProgrammeName = dtReminder3.Rows[i]["ProgrammeName"].ToString();
                            //ParticipantName = dtReminder3.Rows[i]["ParticipantName"].ToString();
                            CandidateName = dtReminder3.Rows[i]["CandidateName"].ToString();

                            EmailDate = Convert.ToDateTime(dtReminder3.Rows[i]["EmailDate"].ToString());

                            Survey_LookUp.InsertReminderData(7, AccountId, AccountName, ParticipantId, CandidateName, ProjectId, ProjectName, ProgrammeId, ProgrammeName, EmailDate, EmailStatus);

                        }
                    }
                }
                catch (Exception ex)
                {
                    Survey_LookUp.HandleException(ex);
                }
            }

        }
    }
//}
