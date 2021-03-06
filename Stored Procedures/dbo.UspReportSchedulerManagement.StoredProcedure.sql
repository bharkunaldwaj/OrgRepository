USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspReportSchedulerManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspReportSchedulerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[UspReportSchedulerManagement]
	@ReportManagementID int,
	@AccountID int,
	@ProjectID int,
    @ProgramID int,
    @TargetPersonID int,
    @TotalCount int,
    @SubmitCount int,
    @SelfAssessment int,
    @ReportName varchar(50),
    @Operation char(2)
AS

-- Insert 

IF (@Operation = 'I')

BEGIN

delete [ReportManagement] where [TargetPersonID]=@TargetPersonID

INSERT INTO [ReportManagement]
           ([AccountID]
           ,[ProjectID]
           ,[ProgramID]
           ,[TargetPersonID]
           ,[TotalCount]
           ,[SubmitCount]
           ,[SelfAssessment]
           ,[ReportName]
           ,[DateCreated])
     VALUES
           (@AccountID
			,@ProjectID
			,@ProgramID
			,@TargetPersonID
			,@TotalCount
			,@SubmitCount
			,@SelfAssessment
			,@ReportName
			,GETDATE())

End

-- Update

ELSE IF (@Operation = 'U')

BEGIN

UPDATE .[ReportManagement]

   SET [AccountID] = @AccountID
      ,[ProjectID] = @ProjectID
      ,[ProgramID] = @ProgramID
      ,[TargetPersonID] = @TargetPersonID
      ,[TotalCount] =@TotalCount 
      ,[SubmitCount] = @SubmitCount 
      ,[SelfAssessment] = @SelfAssessment 
      ,[ReportName] = @ReportName

 WHERE ReportManagementID = @ReportManagementID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [ReportManagement] WHERE ReportManagementID = @ReportManagementID

END
GO
