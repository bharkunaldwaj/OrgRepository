USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[ExportPersonalityStatementsToExcel]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[ExportPersonalityStatementsToExcel]
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
CREATE PROCEDURE [dbo].[ExportPersonalityStatementsToExcel]
	@CategoryID uniqueidentifier,
	@TypeID  nvarchar(max), 
	@LanguageID uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	if LEN(@TypeID)>0
	begin
	SELECT StatementCode,		Category,		TypeList,	Language,		MaleStatement,	FemaleStatement,	Preffered  FROM (
	select PS.Code as StatementCode,PS.CategoryID,		PS.LanguageID,	PC.Code as Category,	PL.Name as Language,
	
	SUBSTRING((SELECT ',' + S.Name from PersonalityTypes S inner join PersonalityStatementTypesAssociation PSA 
	on PSA.TypeID=S.UniqueID where PSA.StatementID=PS.UniqueID ORDER BY S.Name FOR XML PATH('')),2,200000) as TypeList ,
	SUBSTRING((SELECT ',' + CONVERT(varchar(50),S.UniqueID) from PersonalityTypes S inner join PersonalityStatementTypesAssociation PSA 
	on PSA.TypeID=S.UniqueID where PSA.StatementID=PS.UniqueID ORDER BY S.Name FOR XML PATH('')),2,200000) as TypeListUniqueID ,
	PS.MaleStatement,PS.FemaleStatement,PS.Preffered from PersonalityStatements PS
	inner join PersonalityCategories PC on PC.UniqueID=PS.CategoryID
	inner join PersonalityLanguages PL on PL.UniqueID=PS.LanguageID
	)  ExportedData 
	where TypeListUniqueID like '%'+CONVERT(varchar(50), @TypeID) +'%' 
	and ExportedData.CategoryID=@CategoryID 
	and ExportedData.LanguageID=@LanguageID
	end
	else
	begin
	SELECT StatementCode,		Category,		TypeList,	Language,		MaleStatement,	FemaleStatement,	Preffered  FROM (
	select PS.Code as StatementCode,PS.CategoryID,		PS.LanguageID,	PC.Code as Category,	PL.Name as Language,
	
	SUBSTRING((SELECT ',' + S.Name from PersonalityTypes S inner join PersonalityStatementTypesAssociation PSA 
	on PSA.TypeID=S.UniqueID where PSA.StatementID=PS.UniqueID ORDER BY S.Name FOR XML PATH('')),2,200000) as TypeList ,
	SUBSTRING((SELECT ',' + CONVERT(varchar(50),S.UniqueID) from PersonalityTypes S inner join PersonalityStatementTypesAssociation PSA 
	on PSA.TypeID=S.UniqueID where PSA.StatementID=PS.UniqueID ORDER BY S.Name FOR XML PATH('')),2,200000) as TypeListUniqueID ,
	PS.MaleStatement,PS.FemaleStatement,PS.Preffered from PersonalityStatements PS
	inner join PersonalityCategories PC on PC.UniqueID=PS.CategoryID
	inner join PersonalityLanguages PL on PL.UniqueID=PS.LanguageID
	)  ExportedData 
	where ExportedData.CategoryID=@CategoryID 
	and ExportedData.LanguageID=@LanguageID
	end
		
END
GO
