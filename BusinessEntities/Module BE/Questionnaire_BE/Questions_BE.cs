using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Questionnaire_BE
{
  public  class Questions_BE
    {
       public int? QuestionID { get; set; }
    public int? AccountID { get; set; }
    public int? CompanyID { get; set; }
    public int? QuestionnaireID { get; set; }
    public int? QuestionTypeID { get; set; }
    public int? CateogryID { get; set; }
    public int? Sequence { get; set; }
    public int? Validation { get; set; }
    public String ValidationText { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }
    public String DescriptionSelf { get; set; }
    public String Hint { get; set; }
    public int? Token { get; set; }
    public String TokenText { get; set; }
    public int? LengthMIN { get; set; }
    public int? LengthMAX { get; set; }
    public bool? Multiline { get; set; }
    public String LowerLabel { get; set; }
    public String UpperLabel { get; set; }
    public int? LowerBound { get; set; }
    public int? UpperBound { get; set; }
    public int? Increment { get; set; }
    public bool? Reverse { get; set; }
    public int? ModifyBy { get; set; }
    public DateTime? ModifyDate { get; set; }
    public int? IsActive { get; set; }

    public Questions_BE()
    {
        this.AccountID = null;
        this.QuestionID = null ;
        this.CompanyID = null ;
        this.QuestionnaireID = null ;
        this.QuestionTypeID = null ;
        this.CateogryID = null ;
        this.Sequence = null ;
        this.Validation = null ;
        this.ValidationText = null;
        this.Title = String.Empty ;
        this.Description = String.Empty ;
        this.DescriptionSelf = String.Empty;
        this.Hint = String.Empty ;
        this.Token = null ;
        this.TokenText = null;
        this.LengthMIN = null ;
        this.LengthMAX = null ;
        this.Multiline = null ;
        this.LowerLabel = String.Empty ;
        this.UpperLabel = String.Empty ;
        this.LowerBound = null ;
        this.UpperBound = null ;
        this.Increment = null ;
        this.Reverse = null ;
        this.ModifyBy = null ;
        this.ModifyDate = null ;
        this.IsActive = null ;
    } 
    }




  public class Survey_Questions_BE
  {
      public int? QuestionID { get; set; }
      public int? AccountID { get; set; }
      public int? CompanyID { get; set; }
      public int? QuestionnaireID { get; set; }
      public int? QuestionTypeID { get; set; }
      public int? CateogryID { get; set; }
      public int? Sequence { get; set; }
      public int? Validation { get; set; }
      public String ValidationText { get; set; }
      public String Title { get; set; }
      public String Description { get; set; }
     // public String DescriptionSelf { get; set; }
      public String Hint { get; set; }
      public int? Token { get; set; }
      public String TokenText { get; set; }
      public int? LengthMIN { get; set; }
      public int? LengthMAX { get; set; }
      public bool? Multiline { get; set; }

   

      public int? ModifyBy { get; set; }
      public DateTime? ModifyDate { get; set; }
      public int? IsActive { get; set; }
      public String Range_Name { get; set; }

      public Survey_Questions_BE()
      {
          this.AccountID = null;
          this.QuestionID = null;
          this.CompanyID = null;
          this.QuestionnaireID = null;
          this.QuestionTypeID = null;
          this.CateogryID = null;
          this.Sequence = null;
          this.Validation = null;
          this.ValidationText = null;
          this.Title = String.Empty;
          this.Description = String.Empty;
       //   this.DescriptionSelf = String.Empty;
          this.Hint = String.Empty;
          this.Token = null;
          this.TokenText = null;
          this.LengthMIN = null;
          this.LengthMAX = null;
          this.Multiline = null;

        

          this.ModifyBy = null;
          this.ModifyDate = null;
          this.IsActive = null;
          this.Range_Name = null;
      }
  }
}