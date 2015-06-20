USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccProject]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAccProject]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UspAccProject]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UspAccProject]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[UspAccProject]

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
				   ,Project.Password
				   ,[IsActive]   

			   FROM [Project]
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
				   ,Project.Password
				   ,[IsActive]    

			   FROM [Project]
			  WHERE [IsActive] =1

			 End

		
		  
		

		   
		
		   
		   

		


	END
' 
END
GO
