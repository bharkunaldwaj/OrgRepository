USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspReportSchedulerManagement]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspReportSchedulerManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspReportSchedulerManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspReportSchedulerManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create PROCEDURE [dbo].[Survey_UspReportSchedulerManagement]
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

IF (@Operation = ''I'')

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

ELSE IF (@Operation = ''U'')

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

ELSE IF (@Operation = ''D'')

BEGIN

DELETE FROM [ReportManagement] WHERE ReportManagementID = @ReportManagementID

END
' 
END
GO
