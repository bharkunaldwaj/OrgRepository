USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAssignCategoriesToAccount]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityAssignCategoriesToAccount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalityAssignCategoriesToAccount] 
	@CategoryIDs nvarchar(max),
	@DeleteCategoryIDs nvarchar(max),
	@AccountID int
	
AS
BEGIN
	BEGIN TRAN
		
		SET NOCOUNT ON;
		
		
		delete from PersonalityAccountCategories 
		where CategoryID not in(Select Items from dbo.TblUfSplit(@DeleteCategoryIDs,',') a)
		and AccountID =@AccountID
		
		
		
		insert into PersonalityAccountCategories
		(CreatedDate,CreatedBy,Name,Code,Title,Description,SubTitle,SubDescription,AccountID,CategoryID,Sequence,IsActive)
		(select GETDATE(),NEWID(), Name,Code,Title,Description,SubTitle,SubDescription,@AccountID,UniqueID,Sequence,IsActive 
			from PersonalityCategories 
			where UniqueID in(Select Items from dbo.TblUfSplit(@CategoryIDs,',') a) 
			AND UniqueID 
			not in (Select CategoryID from PersonalityAccountCategories where AccountID =@AccountID))
		
		
		
		
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
GO
