USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalitySaveExcelDataToTable]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalitySaveExcelDataToTable]
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
CREATE PROCEDURE [dbo].[PersonalitySaveExcelDataToTable] 
	@StatementCode nvarchar(50),
	@Category nvarchar(20),
	@Types  nvarchar(500), 
	@Language nvarchar(100),
	@Male nvarchar (2500),
	@Female nvarchar (2500),
	@Preffered bit	
AS
BEGIN
BEGIN TRAN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @CategoryID as uniqueidentifier
	Declare @LanguageID as uniqueidentifier
	Declare @StatementID as uniqueidentifier
	Declare @CategoryCode as nvarchar(5)
	Declare @LanguageName as nvarchar(5)
	Declare @MaxSequence as int
	Declare @PersonalityStatementCode as nvarchar(50)
	
	SELECT @StatementID = UniqueID FROM PersonalityStatements  where Code = @StatementCode
	SELECT @CategoryID = UniqueID from PersonalityCategories where Code = @Category
	SELECT @LanguageID = UniqueID from PersonalityLanguages where Name = @Language
	--set @StatementID =NEWID()
	
	IF (@StatementID is null)
	BEGIN
		set @StatementID =NEWID()
		IF  (@CategoryID is not null)
		BEGIN
			 If (@LanguageID is not null) and  (@CategoryID is not null)
			 Begin
			   -- Insert statements here
			   Select @CategoryCode=SUBSTRING(Code,1,3) from PersonalityCategories where UniqueID=@CategoryID
			   Select @LanguageName=SUBSTRING(Name,1,3) from PersonalityLanguages where UniqueID=@LanguageID
			   Select @MaxSequence=Max(Sequence)+1 from PersonalityStatements
			   
			   set @PersonalityStatementCode=@CategoryCode+'-'+@LanguageName+'-'+CONVERT(nvarchar(50),@MaxSequence)
			   
			   
				INSERT into PersonalityStatements(UniqueID,CreatedDate,CreatedBy,CategoryID,LanguageID,MaleStatement,FemaleStatement,Preffered,Code,Sequence,IsApproved) 
				values (@StatementID,GETDATE(), newid(),@CategoryID,@LanguageID,@Male,@Female,@Preffered,@PersonalityStatementCode,@MaxSequence,1)
				
				insert into PersonalityStatementTypesAssociation(StatementID,TypeID)
				(Select @StatementID, b.UniqueID  from dbo.TblUfSplit(@Types,',') a
				inner join PersonalityTypes b on a.Items=b.Name)
			 End
		END
	END
	ELSE
	BEGIN
		IF  (@CategoryID is not null)
		BEGIN
			 If (@LanguageID is not null) and  (@CategoryID is not null)
			 Begin
				-- Update statements here
				
				delete from PersonalityStatementTypesAssociation where StatementID=@StatementID
			   
				UPDATE PersonalityStatements
				SET UniqueID=@StatementID,
				UpdatedDate=GETDATE(),
				UpdatedBy=newid(),
				CategoryID=@CategoryID,
				LanguageID=@LanguageID,
				MaleStatement=@Male,
				FemaleStatement=@Female,
				Preffered=@Preffered,
				IsApproved=1
				WHERE UniqueID=@StatementID	
			
				
				insert into PersonalityStatementTypesAssociation(StatementID,TypeID)
				(Select @StatementID, b.UniqueID  from dbo.TblUfSplit(@Types,',') a
				inner join PersonalityTypes b on a.Items=b.Name)
				
			 End
			 
		END
	END
	
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
GO
