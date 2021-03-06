USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspAccProject]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspAccProject]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspAccProject]

@AccountID int,

@SelectFlag char(1)

as

IF (@AccountID != 2)

	BEGIN

		IF (@SelectFlag = 'I') -- Id based

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

		IF (@SelectFlag = 'I') -- Id based

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
GO
