USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspAccProject]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspAccProject]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Survey_UspAccProject]

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
				   ,Survey_Project.Password
				   ,[IsActive]   

			   FROM [Survey_Project]
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
				   ,Survey_Project.Password
				   ,[IsActive]    

			   FROM [Survey_Project]
			  WHERE [IsActive] =1

			 End
END
GO
