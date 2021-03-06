USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProjectManagement]
@ProjectID	int,
@StatusID int,
@Reference varchar(50),
@Title varchar(250),
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
@EmailTMPLReportAvalibale int,
@EmailTMPLParticipant int,
@EmailTMPPartReminder1 int,
@EmailTMPPartReminder2 int,
@EmailTMPManager int,
@EmailTMPSelfAssissment int,
@Relationship1 varchar(50),
@Relationship2 varchar(50),
@Relationship3 varchar(50),
@Relationship4 varchar(50),
@Relationship5 varchar(50),
@FaqText varchar(8000),
@ModifyBy int,
@ModifyDate datetime,
@IsActive int,
@Operation char(1)

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Project]
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
           ,[EmailTMPLReportAvalibale]
			,[EmailTMPLParticipant]
			,[EmailTMPPartReminder1] 
			,[EmailTMPPartReminder2] 
			,[EmailTMPManager]
			,[EmailTMPSelfReminder]
		   ,[Relationship1]
		   ,[Relationship2]
		   ,[Relationship3]
		   ,[Relationship4]
		   ,[Relationship5]
			,[FaqText]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive])
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
            @EmailTMPLReportAvalibale,
			@EmailTMPLParticipant,
			@EmailTMPPartReminder1,
			@EmailTMPPartReminder2,	
			@EmailTMPManager,	
			@EmailTMPSelfAssissment,	
			@Relationship1,
			@Relationship2,
			@Relationship3,
			@Relationship4,
			@Relationship5,
			@FaqText,
            @ModifyBy,
            @ModifyDate,
            @IsActive
            )

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Project]
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
	,[EmailTMPLStart] = @EmailTMPLStart
	,[EmailTMPLReminder1] = @EmailTMPLReminder1
	,[EmailTMPLReminder2] = @EmailTMPLReminder2
	,[EmailTMPLReminder3] = @EmailTMPLReminder3
	,[EmailTMPLReportAvalibale] = @EmailTMPLReportAvalibale
	,[EmailTMPLParticipant]=@EmailTMPLParticipant
	,[EmailTMPPartReminder1] = @EmailTMPPartReminder1
	,[EmailTMPPartReminder2] = @EmailTMPPartReminder2	
	,[EmailTMPManager]=@EmailTMPManager
	,[EmailTMPSelfReminder]=@EmailTMPSelfAssissment
    ,[Relationship1]=@Relationship1
    ,[Relationship2]=@Relationship2
    ,[Relationship3]=@Relationship3
    ,[Relationship4]=@Relationship4
    ,[Relationship5]=@Relationship5
	,[FaqText]=@FaqText
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifyDate
	,[IsActive] = @IsActive

WHERE ProjectID=@ProjectID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Project] where [ProjectID]=@ProjectID

End
GO
