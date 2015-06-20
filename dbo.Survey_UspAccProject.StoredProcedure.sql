USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAccProject]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAccProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_UspAccProject]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_UspAccProject]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create procedure [dbo].[Survey_UspAccProject]

@AccountID int,

@SelectFlag char(1)

as

IF (@AccountID != 2)

	BEGIN

		IF (@SelectFlag = ''I'') -- Id based

		Begin

		SELECT      [ProjectID]
				   ,[StatusID]
				   ,[AccountID]
				   ,[Reference]
				   ,[Title]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
				   ,[IsActive]   

			   FROM [Survey_Project]
			  WHERE [IsActive] =1
					AND [AccountID] = @AccountID

			 End

		
		  
		END

ELSE

	BEGIN

		IF (@SelectFlag = ''I'') -- Id based

		Begin

		SELECT      [ProjectID]
				   ,[StatusID]
				   ,[AccountID]
				   ,[Reference]
				   ,[Title]
				   ,[MaxCandidate]
				   ,[Logo]
				   ,Survey_Project.Password
				   ,[IsActive]    

			   FROM [Survey_Project]
			  WHERE [IsActive] =1

			 End
END
' 
END
GO
