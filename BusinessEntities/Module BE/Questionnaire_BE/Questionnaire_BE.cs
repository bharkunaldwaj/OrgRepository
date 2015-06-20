using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
    [Serializable]
    public class Questionnaire_BE : feedbackFramework_BE.BE_Base
    {
        public int? QuestionnaireID { get; set; }
        public int? AccountID { get; set; }
        public int? QSTNType { get; set; }
        public String QSTNCode { get; set; }
        public String QSTNName { get; set; }
        public String QSTNDescription { get; set; }
        public int? DisplayCategory { get; set; }
        public int? ProjectID { get; set; }
        public int? ManagerID { get; set; }
        public String QSTNPrologue { get; set; }
        public String QSTNEpilogue { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public Questionnaire_BE()
        {
            this.QuestionnaireID = null;
            this.AccountID = null;
            this.QSTNType = null;
            this.QSTNCode = String.Empty;
            this.QSTNName = String.Empty;
            this.QSTNDescription = String.Empty;
            this.DisplayCategory = null;
            this.ProjectID = null;
            this.ManagerID = null;
            this.QSTNPrologue = String.Empty;
            this.QSTNEpilogue = String.Empty;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
        } 
    }










    [Serializable]
    public class Survey_Questionnaire_BE : feedbackFramework_BE.Survey_BE_Base
    {
        public int? QuestionnaireID { get; set; }
        public int? AccountID { get; set; }
        public int? QSTNType { get; set; }
        public String QSTNCode { get; set; }
        public String QSTNName { get; set; }
        public String QSTNDescription { get; set; }
        public int? DisplayCategory { get; set; }
        public int? ProjectID { get; set; }
        public int? ManagerID { get; set; }
        public String QSTNPrologue { get; set; }
        public String QSTNEpilogue { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public int? IsActive { get; set; }

        public Survey_Questionnaire_BE()
        {
            this.QuestionnaireID = null;
            this.AccountID = null;
            this.QSTNType = null;
            this.QSTNCode = String.Empty;
            this.QSTNName = String.Empty;
            this.QSTNDescription = String.Empty;
            this.DisplayCategory = null;
            this.ProjectID = null;
            this.ManagerID = null;
            this.QSTNPrologue = String.Empty;
            this.QSTNEpilogue = String.Empty;
            this.ModifyBy = null;
            this.ModifyDate = null;
            this.IsActive = null;
        }
    }
















}
