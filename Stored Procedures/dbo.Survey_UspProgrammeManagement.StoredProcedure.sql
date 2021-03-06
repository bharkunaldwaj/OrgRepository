USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProgrammeManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProgrammeManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspProgrammeManagement]
@ProgrammeID int,
@ProgrammeName varchar(50),
@ProgrammeDescription varchar(1000),
@ClientName	varchar(250),
@Logo	varchar(50),
@ProjectID int,
@CompanyID int,
@AccountID int,
@StartDate datetime,
@EndDate datetime,
@Reminder1Date datetime,
@Reminder2Date datetime,
@Reminder3Date datetime,
@ModifyBy int,
@ModifyDate datetime,
@IsActive int,
@Analysis_I_Name varchar(200) ,
@Analysis_I_Category int,
@Analysis_II_Name varchar(200),
@Analysis_II_Category int,
@Analysis_III_Name varchar(200),
@Analysis_III_Category int,
@Operation char(1),
@prog_id int output
as

--Insert
IF (@Operation = 'I')

Begin

INSERT INTO [Survey_Analysis_Sheet]
           ([ProgrammeName]
           ,[ProgrammeDescription]
           ,[ClientName] 
           ,[Logo]
           ,[ProjectID]
           ,CompanyID
           ,[AccountID]
           ,[StartDate]
           ,[EndDate]
           ,[Reminder1Date]
           ,[Reminder2Date]
           ,[Reminder3Date]
           ,[ModifyBy]
           ,[ModifyDate]
           ,[IsActive]
            ,[Analysis_I_Name]
			,[Analysis_I_Category]
			,[Analysis_II_Name]
			,[Analysis_II_Category]
			,[Analysis_III_Name]
			,[Analysis_III_Category]
            )
     VALUES
           (@ProgrammeName,
            @ProgrammeDescription,
            @ClientName,
            @Logo,
			@ProjectID,
			@CompanyID,
			@AccountID,
			@StartDate,
			@EndDate,
			@Reminder1Date,
			@Reminder2Date,
			@Reminder3Date,
			@ModifyBy,
			@ModifyDate,
			@IsActive,
			@Analysis_I_Name,
			@Analysis_I_Category,
			@Analysis_II_Name,
			@Analysis_II_Category,
			@Analysis_III_Name,
			@Analysis_III_Category)


set @prog_id=scope_identity()

End

--Update
Else IF (@Operation = 'U')

Begin

UPDATE [Survey_Analysis_Sheet]
SET 
	[ProgrammeName] = @ProgrammeName
	,[ProgrammeDescription] = @ProgrammeDescription
	,[ClientName] =@ClientName
	,[Logo] =@Logo 
	,[ProjectID] = @ProjectID
	,CompanyID = @CompanyID
	,[AccountID] = @AccountID
	,[StartDate] = @StartDate
	,[EndDate] = @EndDate
	,[Reminder1Date] = @Reminder1Date
	,[Reminder2Date] = @Reminder2Date
	,[Reminder3Date] = @Reminder3Date
	,[ModifyBy] = @ModifyBy
	,[ModifyDate] = @ModifyDate
	,[IsActive] = @IsActive
    ,[Analysis_I_Name]=@Analysis_I_Name
	,[Analysis_I_Category]=@Analysis_I_Category
	,[Analysis_II_Name]=@Analysis_II_Name
	,[Analysis_II_Category]=@Analysis_II_Category
	,[Analysis_III_Name]=@Analysis_III_Name
	,[Analysis_III_Category]=@Analysis_III_Category

					


WHERE ProgrammeID=@ProgrammeID

End

--Delete
Else IF (@Operation = 'D')

Begin
delete from Survey_AnalysisSheet_Category_Details where [Programme_Id]=@ProgrammeID
DELETE from [Survey_Analysis_Sheet] where [ProgrammeID]=@ProgrammeID

End
GO
