USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspCompanyManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspCompanyManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspCompanyManagement]
@CompanyID	int=null,
@ProjectID	int=null,
@StatusID int=null,
@Title varchar(50)=null,
@Description varchar(1000)=null,
@AccountID int=null,
@ManagerID int=null,
@CompanyName varchar(50)=null,
@QuestLogo varchar(50)=null,
@ReportLogo varchar(50)=null,
@EmailTMPLStart int=null,
@EmailTMPLReminder1 int=null,
@EmailTMPLReminder2 int=null,
@EmailTMPLReminder3 int=null,

@FaqText varchar(8000)=null,
@ModifyBy int=null,
@ModifyDate datetime=null,
@IsActive int=null,
@finish_emailID varchar(100)=null,
@finish_emailID_Chkbox bit=null,
@Operation varchar(15)=null,
@EmailFinishEmailTemplate int=null


as


IF (@Operation = 'GETCOMP')
Begin

select * from Survey_Company where (@CompanyID is null or  CompanyID=@CompanyID)

End


--Insert
IF (@Operation = 'I')

Begin

INSERT INTO Survey_Company
           (
            ProjectID,
            StatusID
           ,Title
           ,Description
		   ,AccountID
           ,ManagerID
           ,CompanyName
           ,QuestLogo
           ,ReportLogo
           ,EmailTMPLStart
           ,EmailTMPLReminder1
           ,EmailTMPLReminder2
           ,EmailTMPLReminder3
           ,FaqText
           ,ModifyBy
           ,ModifyDate
           ,IsActive
           ,Finish_EmailID
           ,Finish_EmailID_Chkbox
          ,EmailFinishEmailTemplate
           )
     VALUES
           (
            @ProjectID
           ,@StatusID
           ,@Title
           ,@Description
		   ,@AccountID
           ,@ManagerID
           ,@CompanyName
           ,@QuestLogo
           ,@ReportLogo
           ,@EmailTMPLStart
           ,@EmailTMPLReminder1
           ,@EmailTMPLReminder2
           ,@EmailTMPLReminder3
           ,@FaqText
           ,@ModifyBy
           ,@ModifyDate
           ,@IsActive
           ,@Finish_EmailID
           ,@Finish_EmailID_Chkbox
           ,@EmailFinishEmailTemplate
            )
select  CONVERT(int,SCOPE_IDENTITY())

--To be remove in future
Update Survey_Project set EmailTMPLStart=@EmailTMPLStart
						,EmailTMPLReminder1=@EmailTMPLReminder1
						,EmailTMPLReminder2=@EmailTMPLReminder2
						,EmailTMPLReminder3=@EmailTMPLReminder3
						,Finish_EmailID=@Finish_EmailID
						,Finish_EmailID_Chkbox=@Finish_EmailID_Chkbox
						where ProjectID=@ProjectID
	
End

--Update
IF (@Operation = 'U')

Begin

UPDATE Survey_Company
SET 
	ProjectID=@ProjectID,
            StatusID=@StatusID,
            Title=@Title,
            Description=@Description
		   ,AccountID=@AccountID
           ,ManagerID=@ManagerID
           ,CompanyName=@CompanyName
           ,QuestLogo=@QuestLogo
           ,ReportLogo=@ReportLogo
           ,EmailTMPLStart=@EmailTMPLStart
           ,EmailTMPLReminder1=@EmailTMPLReminder1
           ,EmailTMPLReminder2=@EmailTMPLReminder2
           ,EmailTMPLReminder3=@EmailTMPLReminder3
           ,FaqText=@FaqText
           ,ModifyBy=@ModifyBy
           ,ModifyDate=@ModifyDate
           ,IsActive=@IsActive
           ,Finish_EmailID=@Finish_EmailID
           ,Finish_EmailID_Chkbox=@Finish_EmailID_Chkbox,
           EmailFinishEmailTemplate=@EmailFinishEmailTemplate
           
WHERE CompanyID=@CompanyID



Update Survey_Project set EmailTMPLStart=@EmailTMPLStart
						,EmailTMPLReminder1=@EmailTMPLReminder1
						,EmailTMPLReminder2=@EmailTMPLReminder2
						,EmailTMPLReminder3=@EmailTMPLReminder3
						,Finish_EmailID=@Finish_EmailID
						,Finish_EmailID_Chkbox=@Finish_EmailID_Chkbox
						where ProjectID=@ProjectID
						
						
SELECT CONVERT(int, 1)
End

--Delete
IF (@Operation = 'D')

Begin

DELETE from Survey_Company where CompanyID=@CompanyID

End
GO
