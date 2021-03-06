USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspProjectManagement]
@ProjectID	int,
@StatusID int,
@Reference varchar(50),
@Title varchar(50),
@Description varchar(1000),
@AccountID int,
@ManagerID int,
@MaxCandidate int,
@Logo varchar(50),
@Password varchar(50),
@QuestionnaireID int,
@StartDate datetime,
@EndDate datetime,
@Reminder1Date datetime,
@Reminder2Date datetime,
@Reminder3Date datetime,
@ReportAvaliableFrom datetime,
@ReportAvaliableTo datetime,
@EmailTMPLStart int,
@EmailTMPLReminder1 int,
@EmailTMPLReminder2 int,
@EmailTMPLReminder3 int,

@FaqText varchar(8000),
@ModifyBy int,
@ModifyDate datetime,
@IsActive int,
@finish_emailID varchar(100),
@finish_emailID_Chkbox bit,
@Operation char(1)

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Survey_Project]
           ([StatusID]
           ,[Reference]
           ,[Title]
           ,[Description]
		   ,[AccountID]
           ,[ManagerID]
           ,[MaxCandidate]
           ,[Logo]
           ,[Password]
		   ,[QuestionnaireID]	
           ,[StartDate]
           ,[EndDate]
           ,[Reminder1Date]
           ,[Reminder2Date]
           ,[Reminder3Date]
           ,[ReportAvaliableFrom]
           ,[ReportAvaliableTo]
           ,[EmailTMPLStart]
           ,[EmailTMPLReminder1]
           ,[EmailTMPLReminder2]
           ,[EmailTMPLReminder3]
           ,[FaqText]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive]
           ,[Finish_EmailID]
           ,[Finish_EmailID_Chkbox]
           )
     VALUES
           (@StatusID,
            @Reference,
            @Title,
            @Description,
			@AccountID,
            @ManagerID,
            @MaxCandidate,
            @Logo,
            @Password,
			@QuestionnaireID,
            @StartDate,
            @EndDate,
            @Reminder1Date,
            @Reminder2Date,
            @Reminder3Date,
            @ReportAvaliableFrom,
            @ReportAvaliableTo,
            @EmailTMPLStart,
            @EmailTMPLReminder1,
            @EmailTMPLReminder2,
            @EmailTMPLReminder3,
            
            @FaqText,
            @ModifyBy,
            @ModifyDate,
            @IsActive,
            @finish_emailID,
			@finish_emailID_Chkbox
            )

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Survey_Project]
SET 
	[StatusID] = @StatusID
	,[Reference] = @Reference
	,[Title] = @Title
	,[Description] = @Description
	,[ManagerID] = @ManagerID
	,[MaxCandidate] = @MaxCandidate
	,[Logo] = @Logo
	,[Password] = @Password
    ,[QuestionnaireID]=@QuestionnaireID
	,[StartDate] = @StartDate
	,[EndDate] = @EndDate
	,[Reminder1Date] = @Reminder1Date
	,[Reminder2Date] = @Reminder2Date
	,[Reminder3Date] = @Reminder3Date
	,[ReportAvaliableFrom] = @ReportAvaliableFrom
	,[ReportAvaliableTo] = @ReportAvaliableTo
	--,[EmailTMPLStart] = @EmailTMPLStart
	--,[EmailTMPLReminder1] = @EmailTMPLReminder1
	--,[EmailTMPLReminder2] = @EmailTMPLReminder2
	--,[EmailTMPLReminder3] = @EmailTMPLReminder3
	
	,[FaqText]=@FaqText
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifyDate
	,[IsActive] = @IsActive
	--,[Finish_EmailID]= @finish_emailID
	--,[Finish_EmailID_Chkbox]= @finish_emailID_Chkbox

           
WHERE ProjectID=@ProjectID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Survey_Project] where [ProjectID]=@ProjectID

End
GO
