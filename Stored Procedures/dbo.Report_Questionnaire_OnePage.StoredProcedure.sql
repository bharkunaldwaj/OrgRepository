USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Questionnaire_OnePage]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Questionnaire_OnePage]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Amit Singh>
-- Create date: <17/2/2012>
-- Description:	<SP for Report>
-- =============================================

--exec Report_Questionnaire @Action=N'H',@QuestionnaireID=N'b0312c3c-235c-4040-bb73-ed1b13385d57',@AccountID=N'48',@ReportManagement=N'49a9561e-b10a-4d04-ada6-28b85a0c9348',@ParticipantID=N'abf81647-76b9-4170-9687-3d72dd0d00f0',@IsParticipate=N'True'
--exec [Report_Questionnaire_OnePage] @Action=N'C',@QuestionnaireID=N'b0312c3c-235c-4040-bb73-ed1b13385d57',@AccountID=N'48',@ReportManagement=N'52c4fea6-4919-4058-bc57-7beeb0321052',@ParticipantID=N'7cdfbe15-7b88-4380-861e-407e778263df',@IsParticipate=N'True',@FilterCategoryID='b3ac8295-08bd-4f94-97cf-554b3efbdac9'
--QuestionnaireID=b0312c3c-235c-4040-bb73-ed1b13385d57&AccountID=48&ReportManagement=&ParticipantID=5f4983fd-9e68-46b9-b480-d71e143f58f0&IsParticipate=True&Name=Martin%20Bush&WheelGraph=True&BarGraph=False&PageNumber=1&FooterText=Profile%20produced%20by:%20Colourful%20Relationships,%20www.colourfulrelationships.com&ReportName=Colourful%20Relationships%20Profile--

CREATE procedure [dbo].[Report_Questionnaire_OnePage]

@Action char(1),
@QuestionnaireID varchar(50) = null,
@AccountID varchar(50) = null,
@ReportManagement varchar(50) = null,
@ParticipantID varchar(50)=null,
@IsParticipate varchar(10)=null,
@FilterCategoryID UniqueIdentifier = null
as
begin
if(@Action = 'C')
begin
  
declare @Honorific VARCHAR(10)
set @Honorific =(select  Honorific from dbo.PersonalityParticiapntDetails where UniqueID=@ParticipantID)
set @Honorific = Isnull(@Honorific,'Male')

IF @IsParticipate='False'
BEGIN
 select  @Honorific=p.Honorific from dbo.PersonalityAssignScoreType p where UniqueID=@ParticipantID
 set @Honorific = Isnull(@Honorific,'Male')

 IF(@Honorific = '')
 BEGIN
 set @Honorific='Male'
 END
 
END

declare @LanguageID varchar(50)
set @LanguageID= (select LanguageID from PersonalityReportManagement where UniqueID=@ReportManagement)

declare @FirstName VARCHAR(100)
set @FirstName =(select  FirstName from dbo.PersonalityParticiapntDetails where UniqueID=@ParticipantID)

declare @LastName VARCHAR(100)
set @LastName =(select  LastName from dbo.PersonalityParticiapntDetails where UniqueID=@ParticipantID)

---Adding StatementCode-----
create table #TempHeading ([UniqueID] uniqueidentifier ,[CreatedBy] uniqueidentifier,[CreatedDate] datetime,[UpdatedBy] uniqueidentifier,[UpdatedDate] datetime,[AccountID] int,[QuestionnareID] uniqueidentifier ,[ReportType] nvarchar(50) ,[ReportName] nvarchar(50),[Heading1] nvarchar(250),[Heading2] nvarchar(50),[Heading3] nvarchar(50),[Copyright] nvarchar(MAX),[ReportImage1] image,[ReportImage2] image,[ReportImage3] image ,[ReportImage4] image,[GraphicsImage1] image,[GraphicsImage2] image ,[Introduction] nvarchar(MAX) ,[Conclusion] nvarchar(MAX),[HeadingColor] nvarchar(50) ,BarGraph Bit,WheelGraph Bit ,StatementCode Bit,LeftGraphText nvarchar(50),RightGraphText nvarchar(50),WheelGraphText nvarchar(50),ConclusionHeading varchar(500),
      
      reportImage1Value varchar(10),reportImage2Value varchar(10),reportImage3Value varchar(10),reportImage4Value varchar(10)
		,[ShowSecondGraph] bit
		,[SecondGraphExplanation] nvarchar(max)
		,[SecondGraphExplanationHeading] nvarchar(500)
		,[ShowWheelGraphExplanation] bit
		,[WheelGraphExplanation] nvarchar(max)
		,[WheelGraphExplanationHeading] nvarchar(500)
		,[IntroductionImage1] nvarchar(500),
		[IntroductionImage2] nvarchar(500),
		[IntroductionImage3] nvarchar(500),
		[IntroductionImage4] nvarchar(500),
		[ConclusionImage1] nvarchar(500),
		[ConclusionImage2] nvarchar(500),
		[ConclusionImage3] nvarchar(500),
		[ConclusionImage4] nvarchar(500))

insert into #TempHeading
exec [Report_Questionnaire] 'H',@QuestionnaireID,@AccountID,@ReportManagement,@ParticipantID,@IsParticipate


 

declare @StatementCode BIT
select @StatementCode = StatementCode from #TempHeading
set @StatementCode = Isnull(@StatementCode,0)
--set @StatementCode=1
---Adding StatementCode-----
  
declare @ColorOrder varchar(15)
set @ColorOrder = null

 select  @ColorOrder = ISNULL(@ColorOrder,'') + SUBSTRING(dbo.PersonalityQuestionChoices.ColorCode,1,1)
	
	from PersonalityQuestionsAnswers
	inner join PersonalityQuestionChoices ON 
	dbo.PersonalityQuestionChoices.UniqueID = dbo.PersonalityQuestionsAnswers.QuestionChoiceID
	inner join PersonalityQuestionnaires ON
	PersonalityQuestionnaires.UniqueID=PersonalityQuestionsAnswers.QuestionnaireID
	where PersonalityQuestionsAnswers.ParticiapntDetailsID=@ParticipantID 
	group by  dbo.PersonalityQuestionChoices.ColorCode
           ORDER BY AVG(CASE WHEN PersonalityQuestionnaires.[Type]=2 THEN  
PersonalityQuestionsAnswers.ScoreValue*10 WHEN PersonalityQuestionnaires.[Type] = 1 
THEN PersonalityQuestionsAnswers.ScoreValue END ) DESC
 -- To get TypeID according to ColorOrder
declare @Type int

IF @IsParticipate='True'
Begin
	set @Type = (
	 select PersonalityColorOrderType.Type 
	 from PersonalityColorOrderType 
	 where PersonalityColorOrderType.Color_Order = @ColorOrder)
End
Else
Begin
	set @Type=(SELECT Name FROM PersonalityTypes where UniqueID=
	(select TypeID from PersonalityAssignScoreType where QuestionnaireID=@QuestionnaireID and AccountID=@AccountID and UniqueID=@ParticipantID))
	
	 
set @FirstName =(select   (SELECT top 1 * from dbo.TblUfSplit(p.ParticipantName, ' ')) from dbo.PersonalityAssignScoreType p where UniqueID=@ParticipantID)
set @LastName =(select   (SELECT top 1 * from dbo.TblUfSplit(p.ParticipantName, ' ') WHERE Items<>@FirstName) from dbo.PersonalityAssignScoreType p where UniqueID=@ParticipantID)
	--SELECT @FirstName
	--SELECT @LastName
End
 
 
 if(@Type = '' or @Type is null ) 
(
 select @Type =  PersonalityColorOrderType.Type 
 from PersonalityColorOrderType 
 where @ColorOrder like  PersonalityColorOrderType.Color_Order + '%'
)

print @Type
/*if @Type > 16 
	BEGIN
		SELECT @Type = p.ConversionField 
			from PersonalityColorOrderType  p Where p.Type = @Type
	END
*/
PRINT @LanguageID

 --To get statement,DisplayType according to TypeID
 
 
 create table #temp (DisplayType int 
			,DisplayTypeValue int 
			,MaleStatement varchar(max) 
			,FemaleStatement varchar(max),CategoryID varchar(50) ,Name varchar(500) ,Honorific VARCHAR(10) ,[Description] VARCHAR(max),SubDescription VARCHAR(max),SubTitle VARCHAR(max),Sequence INT ,FirstName VARCHAR(50) ,LastName VARCHAR(50),Preffered BIT )				
 
 INSERT into #temp (DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered)
select     DisplayType,DisplayTypeValue,
			CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.MaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.MaleStatement END as MaleStatement,
			CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.FemaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.FemaleStatement END as FemaleStatement,
			PersonalityCategories.UniqueID AS CategoryID,PersonalityAccountCategories.Title,
		    @Honorific as Honorific,
		    REPLACE(PersonalityAccountCategories.[Description],'[FIRSTNAME]',ISNULL(@FirstName,'')) as [Description],
		    PersonalityAccountCategories.SubDescription,
		    PersonalityAccountCategories.SubTitle,PersonalityAccountCategories.Sequence,
		    @FirstName as FirstName,@LastName as LastName,PersonalityStatements.Preffered 
		    from PersonalityAccountCategories 
			INNER JOIN
			PersonalityCategories ON
			PersonalityCategories.UniqueID=PersonalityAccountCategories.CategoryID
			INNER JOIN
			PersonalityStatements ON
			PersonalityStatements.CategoryID=PersonalityCategories.UniqueID
where PersonalityStatements.UniqueID in (
select StatementID from PersonalityStatementTypesAssociation where
TypeID = ( select UniqueID from PersonalityTypes where Name=@Type))
and PersonalityStatements.Preffered =0
and PersonalityAccountCategories.AccountID = @AccountID AND 
PersonalityAccountCategories.UniqueID in (
select PersonalityReportGroupCategoryAssociations.ReportCategoryID from PersonalityReportManagement

				INNER JOIN 
				PersonalityReportGroupCategoryAssociations on 
				PersonalityReportGroupCategoryAssociations.ReportManagementID=PersonalityReportManagement.UniqueID
				--where PersonalityReportManagement.UniqueID like '%'
				where  
				--PersonalityReportGroupCategoryAssociations.ReportCategoryID IN (
				-- SELECT ga.CategoryID From PersonalityCategoriesGroups  GR INNER JOIN PersonalityCategoriesGroupAssociations GA
				-- ON GA.CategoriesGroupID = GR.UniqueID 
				-- WHERE GA.AccountID = @AccountID
				
				--) AND 
				PersonalityReportManagement.UniqueID = @ReportManagement 
				 
				 and PersonalityReportManagement.LanguageID = @LanguageID
				)
				order by  NEWID()
				--CategoryID,Sequence, NEWID()
				
--INSERT into #temp (DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered)
--SELECT  * FROM #_temp  order by   NEWID()
 
 --UPDATE #temp  SET #temp.Sequence = g.OrderBy
 --FROM #temp t INNER JOIN PersonalityReportGroupCategoryAssociations g on t.CategoryID = g.ReportCategoryID
 --WHERE g.ReportManagementID = @ReportManagement
 
-- Creating Functionality to filter the data according to DisplayTypeValue	
create table #temp1 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),OrderBy VARCHAR(10))			
create table #temp2 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),OrderBy VARCHAR(10))			
create table #temp3 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT ,FirstName VARCHAR(50),LastName VARCHAR(50),OrderBy VARCHAR(10))
create table #temp4 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT)
create table #temp6 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy VARCHAR(10))						
create table #temp7 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy VARCHAR(10))			
create table #temp8 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy VARCHAR(10))			
create table #temp9 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy VARCHAR(10))			
create table #temp10 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy VARCHAR(10))			
create table #temp11 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT)
create table #temp12 (DisplayType int ,DisplayTypeValue int ,MaleStatement varchar(max),FemaleStatement varchar(max),CategoryID varchar(50),Name varchar(500),Honorific VARCHAR(10),[Description] VARCHAR(Max),SubDescription VARCHAR(Max),SubTitle VARCHAR(Max),Sequence INT,FirstName VARCHAR(50),LastName VARCHAR(50),Preffered BIT,OrderBy CHAR(1))

declare @Count int
declare @CategoryID varchar(50)		
declare @CategoryIDCount int
declare @CategoryIDCount1 int
declare @Query varchar(max)
declare @Query1 varchar(max)
declare @DisplayType varchar(max)
declare @DisplayTypeValue int
declare @DisplayTypeValueCount int

--    exec [dbo].[Report_Questionnaire]'C','F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06','48','27AE7A1B-B8B7-4A10-9603-28F4880F4AF3','0426E556-C523-49D7-8DC1-9D8B82F0A590','True' 

--  Table for Display Type 2
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='2')
while (@CategoryIDCount > 0 )
BEGIN
set @Count = ( select top 1 DisplayTypeValue from #temp where DisplayType='2')
set @DisplayType = ( select top 1 DisplayType from #temp where DisplayType='2')
set @CategoryID = ( select top 1 CategoryID from #temp where DisplayType='2')

-------------------------------------
--select @CategoryID,@CategoryIDCount
-------------------------------------

set  @Query = 'select top ' + convert(varchar(max),@Count) + ' * from #temp where CategoryID = Convert(UniqueIdentifier, ''' + @CategoryID + ''')' + 
' and DisplayType=''2'' order by NEWID()'
print @query

insert into #temp4 
exec (@Query)

-----------------------------------------
--SELECT * FROM #temp4 
-----------------------------------------

insert into #temp8
select DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered,OrderBy from
(select *,'0' as OrderBy  from #temp4
union
Select * from (select Top 1 @DisplayType as DisplayType,@Count as DisplayTypeValue,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.MaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.MaleStatement END as MaleStatement,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.FemaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.FemaleStatement END as FemaleStatement,
			 PersonalityStatements.CategoryID,
			 PersonalityAccountCategories.Title,@Honorific as Honorific,
			 PersonalityAccountCategories.[Description],PersonalityAccountCategories.SubDescription,
			 PersonalityAccountCategories.SubTitle,PersonalityAccountCategories.Sequence,
			 @FirstName as FirstName,@LastName as LastName,Preffered,'1' as OrderBy 
			 from PersonalityStatements
	INNER JOIN PersonalityStatementTypesAssociation ON
			   PersonalityStatementTypesAssociation.StatementID=PersonalityStatements.UniqueID 
	INNER JOIN PersonalityTypes ON
			   PersonalityTypes.UniqueID=PersonalityStatementTypesAssociation.TypeID
	INNER JOIN PersonalityCategories ON		   
			   PersonalityCategories.UniqueID=PersonalityStatements.CategoryID
	INNER JOIN PersonalityAccountCategories ON
	           PersonalityAccountCategories.CategoryID=PersonalityCategories.UniqueID		   
			   where PersonalityStatements.CategoryID=@CategoryID and Preffered=1 and PersonalityTypes.Name=@Type And PersonalityAccountCategories.CategoryID=@CategoryID AND PersonalityAccountCategories.AccountID = @AccountID
			   order by NEWID())as tmp)as tmp1
			   order by OrderBy Desc
	  		   
--SELECT * FROM    #temp8
set @Query1 = 'select top ' + convert(varchar(max),@Count) + ' * from #temp8'
Truncate Table   #temp4

insert into #temp7
exec(@Query1)
Truncate Table   #temp8
delete from #temp where CategoryID = @CategoryID and DisplayType='2'
SET @CategoryID=null
SET @Query = ''
SET @Query1=''
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='2') 
-- set @CategoryIDCount = @CategoryIDCount - 1
 
END
 
-----------------------------------------
 -- select * from #temp7
-----------------------------------------

--  Table for Display Type 1
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='3')
delete from #temp4
while (@CategoryIDCount > 0 )
BEGIN

set @Count=null
set @CategoryID=null
set @DisplayType= null
set @Count = ( select top 1 DisplayTypeValue from #temp where DisplayType='3')
set @CategoryID = ( select top 1 CategoryID from #temp where DisplayType='3')
set @DisplayType = ( select top 1 DisplayType from #temp where DisplayType='3')


set  @Query = 'select top ' + convert(varchar(max),@Count) + ' * from #temp where CategoryID = ''' + @CategoryID + '''' + 
'and DisplayType=''3'' order by NEWID()' 

insert into #temp4 
exec (@Query)

insert into #temp9
select DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered,OrderBy from
(select *,'0' as OrderBy from #temp4
union
Select * from (select Top 1 @DisplayType as DisplayType,@Count as DisplayTypeValue,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.MaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.MaleStatement END as MaleStatement,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.FemaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.FemaleStatement END as FemaleStatement,
			 PersonalityStatements.CategoryID,
			 PersonalityAccountCategories.Title,@Honorific as Honorific,
			 PersonalityAccountCategories.[Description],PersonalityAccountCategories.SubDescription,
			 PersonalityAccountCategories.SubTitle,PersonalityAccountCategories.Sequence,
			 @FirstName as FirstName,@LastName as LastName,Preffered,'1' as OrderBy
			 from PersonalityStatements
	INNER JOIN PersonalityStatementTypesAssociation ON
			   PersonalityStatementTypesAssociation.StatementID=PersonalityStatements.UniqueID
	INNER JOIN PersonalityTypes ON
			   PersonalityTypes.UniqueID=PersonalityStatementTypesAssociation.TypeID
	INNER JOIN PersonalityCategories ON		   
			   PersonalityCategories.UniqueID=PersonalityStatements.CategoryID
	INNER JOIN PersonalityAccountCategories ON
	           PersonalityAccountCategories.CategoryID=PersonalityCategories.UniqueID		   
			   where PersonalityStatements.CategoryID=@CategoryID and Preffered=1 and PersonalityTypes.Name=@Type And PersonalityAccountCategories.CategoryID=@CategoryID AND PersonalityAccountCategories.AccountID = @AccountID
			   order by NEWID())as tmp)as tmp1
			   order by OrderBy Desc			   

set @Query1 = 'select top ' + convert(varchar(max),@Count) + ' * from #temp9'
Truncate Table   #temp4

insert into #temp7
exec(@Query1)
Truncate Table   #temp9	   
delete from #temp where CategoryID = @CategoryID and DisplayType='3'

SET @CategoryID=null
SET @Query = ''
SET @Query1=''
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='3')
END

--  exec [dbo].[Report_Questionnaire]'C','F830B9AE-5BA9-4ECD-9A0F-CA53A57F5E06','48','27AE7A1B-B8B7-4A10-9603-28F4880F4AF3','0426E556-C523-49D7-8DC1-9D8B82F0A590','True' 

--  Table for Display Type 2
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='1')
delete from #temp4
while (@CategoryIDCount > 0 )
BEGIN
set @Count=null
set @CategoryID=null
set @DisplayType= null
set @DisplayTypeValueCount =( select top 1 DisplayTypeValue from #temp where DisplayType='1')
set @Count = ( select top 1 DisplayTypeValue from #temp where DisplayType='1') * 8
set @CategoryID = ( select top 1 CategoryID from #temp where DisplayType='1')
set @DisplayType = ( select top 1 DisplayType from #temp where DisplayType='1')
set @DisplayTypeValue = ( select top 1 DisplayTypeValue from #temp where DisplayType='1')

set  @Query = 'select top ' + convert(varchar(max),@Count) + ' * from #temp where CategoryID = ''' + @CategoryID + '''' + 
'and DisplayType=''1'' '  --order by NEWID()

insert into #temp4 
exec (@Query)

declare @Descrption varchar(1000)
declare @SubDescrption varchar(1000)
declare @Sequences int

while (@DisplayTypeValueCount > 0 )
BEGIN
 
insert into #temp11
select top 8 * from #temp4

select @Descrption=Description,@SubDescrption=SubDescription,@Sequences=Sequence from #temp11

			  
insert into #temp10
select Top 8 DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered,OrderBy from
(select *,'0' as OrderBy from #temp11
union
Select * from (select Top 1 @DisplayType as DisplayType,@DisplayTypeValue as DisplayTypeValue,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.MaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.MaleStatement END as MaleStatement,
			 CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.FemaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.FemaleStatement END as FemaleStatement,
			 PersonalityStatements.CategoryID,
			 PersonalityAccountCategories.Title,@Honorific as Honorific,
			 @Descrption as [Description],@SubDescrption as SubDescription,
			 PersonalityAccountCategories.SubTitle,@Sequences as Sequence,
			 @FirstName as FirstName,@LastName as LastName,Preffered,'1' as OrderBy
			 from PersonalityStatements
	INNER JOIN PersonalityStatementTypesAssociation ON
			   PersonalityStatementTypesAssociation.StatementID=PersonalityStatements.UniqueID
	INNER JOIN PersonalityTypes ON
			   PersonalityTypes.UniqueID=PersonalityStatementTypesAssociation.TypeID
	INNER JOIN PersonalityCategories ON		   
			   PersonalityCategories.UniqueID=PersonalityStatements.CategoryID
	INNER JOIN PersonalityAccountCategories ON
	           PersonalityAccountCategories.CategoryID=PersonalityCategories.UniqueID		   
			   where PersonalityStatements.CategoryID=@CategoryID and Preffered=1 and PersonalityTypes.Name=@Type And PersonalityAccountCategories.CategoryID=@CategoryID AND PersonalityAccountCategories.AccountID = @AccountID
			    and PersonalityAccountCategories.AccountID=@AccountID
			   and (CASE WHEN @StatementCode = 1 THEN PersonalityStatements.Code + ':' + PersonalityStatements.MaleStatement WHEN @StatementCode = 0 THEN PersonalityStatements.MaleStatement END) not in ( select MaleStatement from #temp12)
			   order by NEWID())as tmp)as tmp1
			   order by OrderBy Desc	
			   
insert into #temp12			   	   	   
select top 1 DisplayType,DisplayTypeValue,MaleStatement,FemaleStatement,CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,Preffered,OrderBy from #temp10

------------------------------------------------
--select top 8 * from #temp10
--select * from #temp12
------------------------------------------------
set @Query1 = 'select top 8 * from #temp10'

Truncate Table  #temp11

insert into #temp7
exec(@Query1)

delete from #temp10

set @Descrption =''
set @SubDescrption =''
set @Sequences =''

set @DisplayTypeValueCount = @DisplayTypeValueCount - 1
delete top (8) from #temp4 where CategoryID = @CategoryID and DisplayType='1'

END

Truncate Table  #temp12
Truncate Table  #temp4
delete from #temp where CategoryID = @CategoryID and (DisplayType='1')
SET @CategoryID=null
SET @Query = ''
SET @Query1=''
set @CategoryIDCount = ( select COUNT(distinct CategoryID ) from #temp where DisplayType='1')

END
--  exec [dbo].[Report_Questionnaire] 'C', 'B0312C3C-235C-4040-BB73-ED1B13385D57', 48, 'FFA633B1-A811-490C-9137-89B5BBE158EE', '69511927-1EB4-4CB2-B589-10565D853006', 'true'


-- Replacing FIRSTNAME to Name in DisplayType ='2' or DisplayType='3'
insert into #temp1	
select  DisplayType , DisplayTypeValue,
		REPLACE(MaleStatement,'[FIRSTNAME]',ISNULL(@FirstName,'')),
		REPLACE(FemaleStatement,'[FIRSTNAME]',ISNULL(@FirstName,'')),
		CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,OrderBy 
		from #temp7 
		where DisplayType ='2' or DisplayType='3'
		order by OrderBy Desc,DisplayType, NEWID()

--  exec [dbo].[Report_Questionnaire] 'C', 'B0312C3C-235C-4040-BB73-ED1B13385D57', 48, 'FFA633B1-A811-490C-9137-89B5BBE158EE', '69511927-1EB4-4CB2-B589-10565D853006', 'true'

-- Replacing FIRSTNAME to Name in DisplayType ='1'

declare @MaleStatement VARCHAR(max)
declare @FemaleStatement VARCHAR(max)



-- select @CategoryIDCount
Declare @LoopCount int
 set @LoopCount = ( select COUNT(distinct CategoryID ) from #temp7 where DisplayType='1')
 

 set @CategoryID = ( select top 1 CategoryID from #temp7 where DisplayType='1')
 
WHILE (@LoopCount > 0 )
BEGIN
 

set @CategoryIDCount = (select COUNT(*) from #temp7 where DisplayType='1' AND  CategoryID=@CategoryID)


		while (@CategoryIDCount > 0 )
		BEGIN

 

		insert into #temp6 
		select Top 8 * from #temp7 where DisplayType='1' AND  CategoryID=@CategoryID --Order by OrderBy DESC--, NEWID()

--SELECT * FROM #temp6

		select Top 8 @MaleStatement=isnull(@MaleStatement + ' ','') + MaleStatement from #temp6 where DisplayType='1' Order by OrderBy DESC
		select Top 8 @FemaleStatement= isnull(@FemaleStatement + ' ','') + FemaleStatement from #temp6 where DisplayType='1' Order by OrderBy DESC

		insert into #temp2
		select distinct DisplayType , DisplayTypeValue,
		REPLACE(@MaleStatement,'[FIRSTNAME]',ISNULL(@FirstName,'')) AS MaleStatement,
		REPLACE(@FemaleStatement,'[FIRSTNAME]',ISNULL(@FirstName,'')) AS FemaleStatement,
		CategoryID,Name,Honorific,[Description],SubDescription,SubTitle,Sequence,FirstName,LastName,0--,Preffered,OrderBy  	
		FROM #temp6    -- -as Results
		where DisplayType='1'
		order by DisplayType


		-- select distinct * from #temp2
		 
		set @CategoryIDCount=@CategoryIDCount - 8
		 
		--SELECT * FROM   #temp7 where DisplayType='1' AND CategoryID=@CategoryID		 
		delete Top (8) from #temp7 where DisplayType='1' AND CategoryID=@CategoryID
		delete from #temp6

		set @MaleStatement = null
		set @FemaleStatement= null

		END
		--set @LoopCount = ( select COUNT(distinct CategoryID ) from #temp7 where DisplayType='1')
		set @CategoryID = ( select top 1 CategoryID from #temp7 where DisplayType='1')
		SET @LoopCount = @LoopCount - 1
		

END

 -- select * from #temp2

insert into #temp3
  select * from #temp2 WHERE CategoryID = @FilterCategoryID
insert into #temp3
  select * from #temp1 WHERE CategoryID = @FilterCategoryID
 --SELECT ROW_NUMBER() OVER(ORDER BY SalesYTD DESC) AS Row, 
--SELECT * FROM (

 UPDATE #temp3  SET #temp3.Sequence = g.OrderBy
 FROM #temp3 t 
 INNER JOIN PersonalityCategories C on C.UniqueID = t.CategoryID
 INNER JOIN PersonalityAccountCategories A on A.CategoryID = C.UniqueID
 INNER JOIN PersonalityReportGroupCategoryAssociations g on 
 A.UniqueID = g.ReportCategoryID
 WHERE g.ReportManagementID = @ReportManagement

select *,@Type as [Type],ROW_NUMBER() OVER(ORDER BY Sequence  ASC,CategoryID) AS RowNum from #temp3

--)
--order by ROWNUM,DisplayType,CategoryID

drop table #temp
drop table #temp1
drop table #temp2
drop table #temp3
drop table #temp4
drop table #temp6
drop table #temp7
drop table #temp8
drop table #temp9
drop table #temp10
drop table #temp11
drop table #temp12
drop table #TempHeading
                
end

if(@Action = 'H')
begin

set @FirstName =(select  FirstName from dbo.PersonalityParticiapntDetails where UniqueID=@ParticipantID)

set @LastName =(select  LastName from dbo.PersonalityParticiapntDetails where UniqueID=@ParticipantID)

IF @IsParticipate='False'
BEGIN
 select  @FirstName=(SELECT top 1 * from dbo.TblUfSplit(p.ParticipantName, ' ')) from dbo.PersonalityAssignScoreType p where UniqueID=@ParticipantID
--set @LastName =(SELECT top 1 * from dbo.TblUfSplit(p2.ParticipantName, ' ')) from dbo.PersonalityAssignScoreType p2 where UniqueID=@ParticipantID
set @LastName =(select   (SELECT top 1 * from dbo.TblUfSplit(p.ParticipantName, ' ') WHERE Items<>@FirstName) from dbo.PersonalityAssignScoreType p where UniqueID=@ParticipantID)
END

create table #temp5 ([UniqueID] uniqueidentifier
      ,[CreatedBy] uniqueidentifier
      ,[CreatedDate] datetime
      ,[UpdatedBy] uniqueidentifier
      ,[UpdatedDate] datetime
      ,[AccountID] int
      ,[QuestionnareID] uniqueidentifier
      ,[ReportType] nvarchar(50)
      ,[ReportName] nvarchar(50)
      ,[Heading1] nvarchar(250)
      ,[Heading2] nvarchar(50)
      ,[Heading3] nvarchar(50)
      ,[Copyright] nvarchar(MAX)
      ,[ReportImage1] image
      ,[ReportImage2] image
      ,[ReportImage3] image 
      ,[ReportImage4] image
      ,[GraphicsImage1] image
      ,[GraphicsImage2] image
      ,[Introduction] nvarchar(MAX)
      ,[Conclusion] nvarchar(MAX)
      ,[HeadingColor] nvarchar(50)
      ,BarGraph Bit
      ,WheelGraph Bit
      ,StatementCode Bit
      ,LeftGraphText nvarchar(50)
      ,RightGraphText nvarchar(50)
      ,WheelGraphText nvarchar(50)
      ,ConclusionHeading varchar(500)
      ,reportImage1Value varchar(10)
      ,reportImage2Value varchar(10)
      ,reportImage3Value varchar(10)
      ,reportImage4Value varchar(10)
      ,[ShowSecondGraph] bit
      ,[SecondGraphExplanation] nvarchar(max)
      ,[SecondGraphExplanationHeading] nvarchar(500)
      ,[ShowWheelGraphExplanation] bit
      ,[WheelGraphExplanation] nvarchar(max)
      ,[WheelGraphExplanationHeading] nvarchar(500)
      ,[IntroductionImage1] nvarchar(500),
		[IntroductionImage2] nvarchar(500),
		[IntroductionImage3] nvarchar(500),
		[IntroductionImage4] nvarchar(500),
		[ConclusionImage1] nvarchar(500),
		[ConclusionImage2] nvarchar(500),
		[ConclusionImage3] nvarchar(500),
		[ConclusionImage4] nvarchar(500),
		PieGraph Bit
		   
      ) 

insert into #temp5([UniqueID]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdatedBy]
      ,[UpdatedDate]
      ,[AccountID]
      ,[QuestionnareID]
      ,[ReportType]
      ,[ReportName]
      ,[Heading1]
      ,[Heading2]
      ,[Heading3]
      ,[Copyright]
      ,[ReportImage1]
      ,[ReportImage2]
      ,[ReportImage3]
      ,[ReportImage4]
      ,[GraphicsImage1]
      ,[GraphicsImage2]
      ,[Introduction]
      ,[Conclusion]
      ,[HeadingColor]
      ,[BarGraph]
      ,[WheelGraph]
      ,[StatementCode]
      ,[LeftGraphText]
      ,[RightGraphText]
      ,[WheelGraphText]
      ,ConclusionHeading
      ,[ShowSecondGraph]
      ,[SecondGraphExplanation]
      ,[SecondGraphExplanationHeading]
      ,[ShowWheelGraphExplanation]
      ,[WheelGraphExplanation]
      ,[WheelGraphExplanationHeading]
      ,[IntroductionImage1]
      ,[IntroductionImage2]
      ,[IntroductionImage3]
      ,[IntroductionImage4]
      ,[ConclusionImage1]
      ,[ConclusionImage2]
      ,[ConclusionImage3]
      ,[ConclusionImage4]
      ,PieGraph
      ,reportImage1Value 
      ,reportImage2Value
      ,reportImage3Value 
      ,reportImage4Value


)
SELECT [UniqueID]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[UpdatedBy]
      ,[UpdatedDate]
      ,[AccountID]
      ,[QuestionnareID]
      ,[ReportType]
      ,[ReportName]
      ,[Heading1]
      ,[Heading2]
      ,[Heading3]
      ,[Copyright]
      ,[ReportImage1]
      ,[ReportImage2]
      ,[ReportImage3]
      ,[ReportImage4]
      ,[GraphicsImage1]
      ,[GraphicsImage2]
      ,[Introduction]
      ,[Conclusion]
      ,[HeadingColor]
      ,[BarGraph]
      ,[WheelGraph]
	  ,[StatementCode]
	  ,[LeftGraphText]
      ,[RightGraphText]
      ,[WheelGraphText]
      ,ConclusionHeading
      ,[ShowSecondGraph]
      ,REPLACE([SecondGraphExplanation],CHAR(13),' <br/> ') SecondGraphExplanation
      ,[SecondGraphExplanationHeading]
      ,[ShowWheelGraphExplanation]
      ,REPLACE([WheelGraphExplanation],CHAR(13),' <br/> ') WheelGraphExplanation
      ,[WheelGraphExplanationHeading]
	  ,[IntroductionImage1]
      ,[IntroductionImage2]
      ,[IntroductionImage3]
      ,[IntroductionImage4]
      ,[ConclusionImage1]
      ,[ConclusionImage2]
      ,[ConclusionImage3]
      ,[ConclusionImage4]
       ,PieGraph
      

,'' as reportImage1Value,'' as reportImage2Value,'' as reportImage3Value,'' as reportImage4Value 
from PersonalityReportManagement where UniqueID=@ReportManagement
and QuestionnareID=@QuestionnaireID

Declare @STU_NO NVARCHAR(100) =''
DECLARE @enddate as varchar(12)
SELECT @enddate=CONVERT(varchar(30), pa.FinishedDate,113 ),@STU_NO=ISNULL(pa.Associate,'')  from PersonalityParticiapntDetails pa WHERE pa.UniqueID=@ParticipantID

IF @IsParticipate='False'
BEGIN
 SELECT @enddate=CONVERT(varchar(30), pa.EndDate,113 ) from PersonalityParticipantAssignments pa WHERE pa.QuestionnaireID=@QuestionnaireID
END

Declare @Account_Code VARCHAR(100) 
Select @Account_Code = Code FROM Account WHERE AccountID = @AccountID

UPDATE #temp5 SET Heading1=ISNULL(@FirstName,'') + ' ' + ISNULL(@LastName,'')+ CASE WHEN @Account_Code= 'RDI' THEN '-'+ @STU_NO ELSE '' END,Heading2=@enddate WHERE [UniqueID]=@ReportManagement
      AND AccountID=@AccountID  AND  QuestionnareID=@QuestionnaireID 
       
if( (select ReportImage1 from #temp5) is not null)
begin 
update #temp5 set reportImage1Value = '1'
end
else
begin
update #temp5 set reportImage1Value = '0'
end

if( (select ReportImage2 from #temp5) is not null)
begin 
update #temp5 set reportImage2Value = '1'
end
else
begin
update #temp5 set reportImage2Value = '0'
end

if( (select ReportImage3 from #temp5) is not null)
begin 
update #temp5 set reportImage3Value = '1'
end
else
begin
update #temp5 set reportImage3Value = '0'
end

if( (select ReportImage4 from #temp5) is not null)
begin 
update #temp5 set reportImage4Value = '1'
end
else
begin
update #temp5 set reportImage4Value = '0'
end

select * from  #temp5
drop table #temp5
		    
end
end
GO
