USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProgrammeManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProgrammeManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProgrammeManagement]
@ProgrammeID int,
@ProgrammeName varchar(50),
@ProgrammeDescription varchar(1000),
@ClientName	varchar(250),
@Logo	varchar(50),
@ProjectID int,
@AccountID int,
@StartDate datetime,
@EndDate datetime,
@Reminder1Date datetime,
@Reminder2Date datetime,
@Reminder3Date datetime,
@ReportAvaliableFrom datetime,
@ReportAvaliableTo datetime,
@PartReminder1Date datetime, 
@PartReminder2Date datetime,
@ModifyBy int,
@ModifiedDate datetime,
@IsActive int,
@Instructions nvarchar(2000),
@ColleagueNo SmallInt,
@Operation char(1),
@ReportTopLogo varchar(100)

as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Programme]
           ([ProgrammeName]
           ,[ProgrammeDescription]
           ,[ClientName] 
           ,[Logo]
           ,[ProjectID]
           ,[AccountID]
           ,[StartDate]
           ,[EndDate]
           ,[Reminder1Date]
           ,[Reminder2Date]
           ,[Reminder3Date]
           ,[ReportAvaliableFrom]
           ,[ReportAvaliableTo]
		  ,[PartReminder1Date] 
		  ,[PartReminder2Date]            
           ,[ModifyBy]
           ,[ModifyDate]
           ,[Instructions]
           ,[ColleagueNo]
           ,[IsActive]
            ,[ReportLogo]
            )
     VALUES
           (@ProgrammeName,
            @ProgrammeDescription,
            @ClientName,
            @Logo,
			@ProjectID,
			@AccountID,
			@StartDate,
			@EndDate,
			@Reminder1Date,
			@Reminder2Date,
			@Reminder3Date,
			@ReportAvaliableFrom,
			@ReportAvaliableTo,
			@PartReminder1Date,
			@PartReminder2Date,
			@ModifyBy,
			@ModifiedDate,
			@Instructions,
			@ColleagueNo,
			@IsActive,
			@ReportTopLogo)

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Programme]
SET 
	[ProgrammeName] = @ProgrammeName
	,[ProgrammeDescription] = @ProgrammeDescription
	,[ClientName] =@ClientName
	,[Logo] =@Logo 
	,[ProjectID] = @ProjectID
	,[AccountID] = @AccountID
	,[StartDate] = @StartDate
	,[EndDate] = @EndDate
	,[Reminder1Date] = @Reminder1Date
	,[Reminder2Date] = @Reminder2Date
	,[Reminder3Date] = @Reminder3Date
	,[ReportAvaliableFrom] = @ReportAvaliableFrom
	,[ReportAvaliableTo] = @ReportAvaliableTo
	,[PartReminder1Date] = @PartReminder1Date
	,[PartReminder2Date] = @PartReminder2Date 	
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifiedDate
	,[Instructions]=@Instructions
	,[ColleagueNo]=@ColleagueNo
	,[IsActive] = @IsActive
	,[ReportLogo] = @ReportTopLogo

WHERE ProgrammeID=@ProgrammeID

End

--Delete
Else IF (@Operation = 'D')

Begin

DELETE from [Programme] where [ProgrammeID]=@ProgrammeID

End
GO
