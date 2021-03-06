USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspReportSchedulerManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspReportSchedulerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Survey_UspReportSchedulerManagement]
	@ReportManagementID int,
	@AccountID int,
	@ProjectID int,
    @ProgramID int,
    
    @TotalCount int,
    @SubmitCount int,
   
    @ReportName varchar(50),
    @Operation char(2)
AS

-- Insert 

IF (@Operation = 'I')

BEGIN


INSERT INTO [ReportManagement]
           ([AccountID]
           ,[ProjectID]
           ,[ProgramID]
           
           ,[TotalCount]
           ,[SubmitCount]
           
           ,[ReportName]
           ,[DateCreated])
     VALUES
           (@AccountID
			,@ProjectID
			,@ProgramID
			
			,@TotalCount
			,@SubmitCount
		
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
     
      ,[TotalCount] =@TotalCount 
      ,[SubmitCount] = @SubmitCount 
   
      ,[ReportName] = @ReportName

 WHERE ReportManagementID = @ReportManagementID

END

-- Delete

ELSE IF (@Operation = 'D')

BEGIN

DELETE FROM [ReportManagement] WHERE ReportManagementID = @ReportManagementID

END
GO
