IF EXISTS(SELECT * FROM sys.objects WHERE object_id= OBJECT_ID(N'Survey_UspUploadPreviousScoreByXML') AND type IN (N'P',N'PC'))
	DROP PROCEDURE dbo.Survey_UspUploadPreviousScoreByXML
GO
-- =============================================
-- Author:		Manish
-- Create date: 9 June 2015
-- Description:	This procedure takes the xml as an input for previous score
-- =============================================
CREATE PROCEDURE dbo.Survey_UspUploadPreviousScoreByXML
	-- Add the parameters for the stored procedure here
	@AccountID			INT,
	@ProjectID			INT,
	@CompanyID			INT,
	@ProgramID			INT,
	@TeamName			varchar(50),
	@Score1Title		varchar(50),
	@Score2Title		varchar(50),
	@PreviousScoreXML	XML
	
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @QuestionaireID Int
	
	
	DECLARE @PreviousScoreTable TABLE(
									Analysis varchar(50)
									,Category varchar(50)
									,QuestionSequence int
									,P1 int
									,P2 int
									)
	
	DECLARE @ihnd int
	
	EXEC sp_xml_preparedocument @ihnd OUTPUT, @PreviousScoreXML
	
	INSERT INTO @PreviousScoreTable(Analysis,Category,QuestionSequence,P1,P2)
	SELECT Analysis,Category,QuestionSequence,P1,P2
	FROM OPENXML(@ihnd,'DataTable/DocumentElement/SurveyPreviousScore',2)
	WITH (Analysis varchar(50),Category varchar(50),QuestionSequence int,P1 int,P2 int)
	
	EXEC sp_xml_removedocument @ihnd
	--SELECT * FROM @PreviousScoreTable
	
	SELECT @QuestionaireID = QuestionnaireID from Survey_Project WHERE ProjectID =@ProjectID
	--SELECT 'QID',@QuestionaireID
	
	IF(SELECT COUNT(1) FROM @PreviousScoreTable) >0
		BEGIN
			DECLARE @PreviousScoreID INT
			DECLARE @ab INT
			SET @ab =0;
			BEGIN TRANSACTION
			IF EXISTS(SELECT 1 FROM Survey_PreviousScores WHERE AccountID = @AccountID AND ProjectID = @ProjectID AND @CompanyID = @CompanyID AND ProgrammeID = @ProgramID)
				BEGIN
					SELECT @PreviousScoreID = PreviousScoreID FROM Survey_PreviousScores WHERE AccountID = @AccountID AND ProjectID = @ProjectID AND @CompanyID = @CompanyID AND ProgrammeID = @ProgramID
					
					--DELETE FROM Previous Score
					DELETE FROM Survey_PreviousScores WHERE AccountID=@AccountID AND ProjectID=@ProjectID AND ProgrammeID=@ProgramID AND CompanyID=@CompanyID
					
					--INSERT THE FRESH RECORD
					INSERT INTO Survey_PreviousScores(AccountID,CompanyID,ProgrammeID,ProjectID,TeamName,Score1Title,Score2Title)
					VALUES(@AccountID,@CompanyID,@ProgramID,@ProjectID,@TeamName,@Score1Title,@Score2Title)
					
					--GET THE NEW PREVIOUS SCORE ID
					DECLARE @NewPreviousScoreID INT
					SET @NewPreviousScoreID = @@IDENTITY;
					
					IF(@NewPreviousScoreID >0)
						BEGIN
							
							-- CHECK For User for Analysis I
							IF (SELECT COUNT(*) FROM @PreviousScoreTable WHERE Analysis='ANALYSIS- I') > 0
								BEGIN
									--DELETE from Survey_PrvScore_QstDetails Analysis 1
									DELETE FROM	Survey_PrvScore_QstDetails WHERE PreviousScoreID = @PreviousScoreID AND AnalysisType=1
									
									--INSERT THE SCORES
									INSERT INTO Survey_PrvScore_QstDetails 
										(
											PreviousScoreID
											,CategoryID
											,QuestionID
											,AnalysisType
											,Score1
											,Score2
											,Category_Detail
											,Analysis_Type
										)
									SELECT 
											@NewPreviousScoreID
											,a.Analysis_Category_Id
											,q.QuestionID
											,CASE a.Analysis_Type WHEN 'ANALYSIS- I' THEN 1 END
											,ps.P1
											,ps.P2
											,a.Category_Detail
											,a.Analysis_Type
									FROM		@PreviousScoreTable ps
									INNER JOIN 
												Survey_AnalysisSheet_Category_Details a
									ON
												a.Programme_Id = @ProgramID 
												AND ps.Analysis ='ANALYSIS- I'
												AND a.Analysis_Type = ps.Analysis 
												AND a.Category_Detail = ps.Category
									INNER JOIN
												Survey_Question q
									ON
												q.AccountID = @AccountID 
												AND q.QuestionnaireID = @QuestionaireID 
												AND q.Sequence = ps.QuestionSequence
									
								END
							SELECT @ab= SCOPE_IDENTITY();
						
							-- CHECK For Analysis II
							IF (SELECT COUNT(*) FROM @PreviousScoreTable WHERE Analysis='ANALYSIS- II') > 0
								BEGIN
									--DELETE from Survey_PrvScore_QstDetails Analysis 1
									DELETE FROM	Survey_PrvScore_QstDetails WHERE PreviousScoreID = @PreviousScoreID AND AnalysisType=2
									
									--INSERT THE SCORES
									INSERT INTO Survey_PrvScore_QstDetails 
										(
											PreviousScoreID
											,CategoryID
											,QuestionID
											,AnalysisType
											,Score1
											,Score2
											,Category_Detail
											,Analysis_Type
										)
									SELECT 
											@NewPreviousScoreID
											,a.Analysis_Category_Id
											,q.QuestionID
											,CASE a.Analysis_Type WHEN 'ANALYSIS- II' THEN 2 END
											,ps.P1
											,ps.P2
											,a.Category_Detail
											,a.Analysis_Type
									FROM		@PreviousScoreTable ps
									INNER JOIN 
												Survey_AnalysisSheet_Category_Details a
									ON
												a.Programme_Id = @ProgramID 
												AND ps.Analysis ='ANALYSIS- II'
												AND a.Analysis_Type = ps.Analysis 
												AND a.Category_Detail = ps.Category
									INNER JOIN
												Survey_Question q
									ON
												q.AccountID = @AccountID 
												AND q.QuestionnaireID = @QuestionaireID 
												AND q.Sequence = ps.QuestionSequence
									
								END
							SELECT @ab= SCOPE_IDENTITY();	
							
							-- CHECK For Analysis III
							IF (SELECT COUNT(*) FROM @PreviousScoreTable WHERE Analysis='ANALYSIS- III') >0
								BEGIN
									--DELETE from Survey_PrvScore_QstDetails Analysis 1
									DELETE FROM	Survey_PrvScore_QstDetails WHERE PreviousScoreID = @PreviousScoreID AND AnalysisType=3
									
									--INSERT THE SCORES
									INSERT INTO Survey_PrvScore_QstDetails 
										(
											PreviousScoreID
											,CategoryID
											,QuestionID
											,AnalysisType
											,Score1
											,Score2
											,Category_Detail
											,Analysis_Type
										)
									SELECT 
											@NewPreviousScoreID
											,a.Analysis_Category_Id
											,q.QuestionID
											,CASE a.Analysis_Type WHEN 'ANALYSIS- III' THEN 3 END
											,ps.P1
											,ps.P2
											,a.Category_Detail
											,a.Analysis_Type
									FROM		@PreviousScoreTable ps
									INNER JOIN 
												Survey_AnalysisSheet_Category_Details a
									ON
												a.Programme_Id = @ProgramID 
												AND ps.Analysis ='ANALYSIS- III'
												AND a.Analysis_Type = ps.Analysis 
												AND a.Category_Detail = ps.Category
									INNER JOIN
												Survey_Question q
									ON
												q.AccountID = @AccountID 
												AND q.QuestionnaireID = @QuestionaireID 
												AND q.Sequence = ps.QuestionSequence
								END
							SELECT @ab= SCOPE_IDENTITY();
						END
					
				END
			ELSE
				BEGIN
					--INSERT THE FRESH RECORD
					INSERT INTO Survey_PreviousScores(AccountID,CompanyID,ProgrammeID,ProjectID,TeamName,Score1Title,Score2Title)
					VALUES(@AccountID,@CompanyID,@ProgramID,@ProjectID,@TeamName,@Score1Title,@Score2Title)
					
					--GET THE NEW PREVIOUS SCORE ID
					SET @NewPreviousScoreID = @@IDENTITY;
					
					IF(@NewPreviousScoreID >0)
						BEGIN
							--INSERT THE SCORES
							INSERT INTO Survey_PrvScore_QstDetails 
								(
									PreviousScoreID
									,CategoryID
									,QuestionID
									,AnalysisType
									,Score1
									,Score2
									,Category_Detail
									,Analysis_Type
								)
							SELECT 
									@NewPreviousScoreID
									,a.Analysis_Category_Id
									,q.QuestionID
									,CASE a.Analysis_Type WHEN 'ANALYSIS- I' THEN 1
											 WHEN 'ANALYSIS- II' THEN 2
											 WHEN 'ANALYSIS- III' THEN 3
										END
									,ps.P1
									,ps.P2
									,a.Category_Detail
									,a.Analysis_Type
							FROM		@PreviousScoreTable ps
							INNER JOIN 
										Survey_AnalysisSheet_Category_Details a
							ON
										a.Programme_Id = @ProgramID 
										AND a.Analysis_Type = ps.Analysis 
										AND a.Category_Detail = ps.Category
							INNER JOIN
										Survey_Question q
							ON
										q.AccountID = @AccountID 
										AND q.QuestionnaireID = @QuestionaireID 
										AND q.Sequence = ps.QuestionSequence
								
							SELECT @ab = SCOPE_IDENTITY();		
						END
				END		
			IF @ab>0
				COMMIT TRANSACTION
			ELSE
				ROLLBACK TRANSACTION	
			
			SELECT @ab;
		END
END
GO
