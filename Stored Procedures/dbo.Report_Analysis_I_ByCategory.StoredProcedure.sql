USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Analysis_I_ByCategory]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Analysis_I_ByCategory]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=39,@AnaType='ANALYSIS- I',@Filter='TABLE'
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- I',@Filter='TOP'

--exec [Report_Analysis_I_ByCategory] @accountid=54,@projectid=6,@programmeid=36,@AnaType='ANALYSIS- I',@Filter='TABLE'
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- I',@Filter='TABLE'
--exec [Report_Analysis_I_ByCategory] @accountid=66,@projectid=8,@programmeid=16,@AnaType='ANALYSIS- II        '
CREATE PROCEDURE [dbo].[Report_Analysis_I_ByCategory] -- 2,7,28,'','ANALYSIS- I'
	@accountid int,
	@projectid int,
	@programmeid int,
	@Filter varchar(25)=null,
	@AnaType varchar(25)=null
	
	as

BEGIN



DECLARE @category_detail_count int, @enter_flag int, 
	@ChkProgrammeAvg int, @ChkFullProjectGroup int, @ChkPrvScore1 bit, 
	@ChkPrvScore2 bit, @QuestionnaireID INT=0,
                                                                                                                                                                               @ShowPreViousScore1 Bit =1,
                                                                                                                                                                                                        @ShowPreViousScore2 Bit =1 
 IF object_id('tempdb..#temp6') IS NOT NULL 
 BEGIN
	DROP TABLE #temp6
 END
 IF object_id('tempdb..#temp4') IS NOT NULL 
 BEGIN
	DROP TABLE #temp4
 END
 
 IF object_id('tempdb..#temp7') IS NOT NULL 
 BEGIN
	DROP TABLE #temp7
 END
 
 IF object_id('tempdb..#temp10') IS NOT NULL 
 BEGIN
	DROP TABLE #temp10
 END
 IF object_id('tempdb..#temp11') IS NOT NULL 
 BEGIN
	DROP TABLE #temp11
 END
 
If object_id('tempdb..#TEMP77') Is Not Null
Begin
  Drop Table #TEMP77
End
If object_id('tempdb..#tempPrev1') Is Not Null
Begin
  Drop Table #tempPrev1
End
If object_id('tempdb..#tempPrev2') Is Not Null
Begin
  Drop Table #tempPrev2
End

  If object_id('tempdb..#tempmain1') Is Not Null
Begin
  Drop Table #tempmain1
End

If object_id('tempdb..#tempPrev11') Is Not Null
Begin
  Drop Table #tempPrev11
End
If object_id('tempdb..#tempPrev22') Is Not Null
Begin
  Drop Table #tempPrev22
End

  If object_id('tempdb..#tempmain11') Is Not Null
Begin
  Drop Table #tempmain11
End
 
 
	CREATE TABLE #temp6(SR_No int identity(1,1),Analysis_type varchar(25),CategoryName varchar(50),Answer varchar(50))

	SELECT @QuestionnaireID=q.QuestionnaireID
	FROM Survey_Questionnaire Q
	INNER JOIN Survey_Project P ON Q.QuestionnaireID = P.QuestionnaireID
	WHERE Q.AccountID=@accountid
	AND P.ProjectID=@ProjectId

	SELECT @ShowPreViousScore1=ShowPreViousScore1,
	@ShowPreViousScore2=ShowPreViousScore1
	FROM Survey_ProjectReportSetting WHERE AccountID=@accountid
	AND ProjectID=@ProjectId

	SELECT @ChkPrvScore1=ISNULL(ShowPreviousScore1,0),
	@ChkPrvScore2=ISNULL(ShowPreviousScore2,0)
	FROM Survey_ProjectReportSetting WHERE AccountID=@accountid
	AND ProjectID=@projectid AND ReportType=5 

  
  DECLARE @cat_detail varchar(25),@loop int, @cat_name varchar(25),@last_ID int
  SET @loop=0 DECLARE c2
  CURSOR READ_ONLY
  FOR
  SELECT category_detail
  FROM survey_analysissheet_category_details WHERE analysis_type=@AnaType
  AND PROGRAMME_id=@programmeid OPEN c2 FETCH NEXT
  FROM c2 INTO @cat_detail 
  
	WHILE @@FETCH_STATUS = 0 
		  BEGIN 
		  IF(@loop=0) 
			BEGIN
		  SET @loop= @loop + 1
				 SELECT c.CategoryID,
				 c.CategoryName,
				 u.FirstName + ' ' + u.LastName AS ParticipantName,
				 CandidateName,
				 pg.ProgrammeName,
				 p.Title,
				 c.Sequence,
				 REPLACE(SUBSTRING (Answer,0, 2),'&','') AS Answer INTO #temp4 from Account a

		  LEFT JOIN Survey_Questionnaire qn ON qn.AccountId = a.accountId
		  LEFT JOIN Survey_Category c ON c.AccountID = a.AccountID
		  AND qn.QuestionnaireID = c.QuestionnaireID
		  LEFT JOIN Survey_Question q ON q.CateogryId = c.CategoryId
		  LEFT JOIN Survey_QuestionAnswer qa ON qa.QuestionId = q.QuestionId
		  LEFT JOIN Survey_AssignmentDetails ad ON ad.AsgnDetailID = qa.AssignDetId
		  LEFT JOIN Survey_AssignQuestionnaire aq ON aq.AssignmentID = ad.AssignmentID
		  LEFT JOIN [User] u ON u.AccountID = aq.AccountID
		  LEFT JOIN Survey_Analysis_Sheet pg ON pg.ProgrammeID = aq.ProgrammeID
		  LEFT JOIN Survey_Project p ON p.ProjectID = aq.ProjecctID WHERE a.AccountID = aq.AccountID
		  AND q.QuestionTypeID = 2
		  AND qa.Answer != ' '
		  AND aq.accountID =@accountid
		  AND aq.ProgrammeID = @programmeid
		  AND CASE
				  WHEN @AnaType = 'ANALYSIS- I' THEN Analysis_I
				  WHEN @AnaType ='ANALYSIS- II ' THEN Analysis_II
				  WHEN @AnaType='ANALYSIS- III' THEN Analysis_III
				  ELSE Analysis_I
			  END =@cat_detail
		  AND qa.answer !='N/A'
		  AND ad.SubmitFlag = 'True'
		ORDER BY c.Sequence 


		INSERT INTO #temp6(Analysis_type,CategoryName,Answer)
		SELECT @cat_detail AS 'Analysis_type',
			   CategoryName,
			   cast(SUBSTRING(cast(sum(Average)/count(Average) AS Varchar(50)),1,3)AS decimal(12,1)) AS Answer
		FROM
		  ( SELECT CategoryName,
				   cast(sum(cast(Answer AS decimal(12,1))) / count(Answer) AS decimal(12,1)) AS Average
		   FROM
			 ( SELECT *
			  FROM #temp4
		 ) AS t1
		   GROUP BY CategoryName ) AS t2
		   GROUP BY CategoryName 
		 END
		  ELSE 
			BEGIN
				SET @loop= @loop + 1
				SELECT c.CategoryID,
				c.CategoryName,
				u.FirstName+' '+ u.LastName AS ParticipantName,
				CandidateName,
				pg.ProgrammeName,
				p.Title,
				c.Sequence,
				REPLACE(SUBSTRING (Answer,0, 2),'&','') AS Answer INTO #temp5

				FROM Account a
				LEFT JOIN Survey_Questionnaire qn ON qn.AccountId = a.accountId
				LEFT JOIN Survey_Category c ON c.AccountID = a.AccountID
				AND qn.QuestionnaireID = c.QuestionnaireID
				LEFT JOIN Survey_Question q ON q.CateogryId = c.CategoryId
				LEFT JOIN Survey_QuestionAnswer qa ON qa.QuestionId = q.QuestionId
				LEFT JOIN Survey_AssignmentDetails ad ON ad.AsgnDetailID = qa.AssignDetId
				LEFT JOIN Survey_AssignQuestionnaire aq ON aq.AssignmentID = ad.AssignmentID
				LEFT JOIN [User] u ON u.AccountID = aq.AccountID
				LEFT JOIN Survey_Analysis_Sheet pg ON pg.ProgrammeID = aq.ProgrammeID
				LEFT JOIN Survey_Project p ON p.ProjectID = aq.ProjecctID
				WHERE a.AccountID = aq.AccountID
				  AND q.QuestionTypeID = 2
				  AND qa.Answer != ' '
				  AND aq.accountID =@accountid
				  AND aq.ProgrammeID = @programmeid
				  AND CASE
						  WHEN @AnaType = 'ANALYSIS- I' THEN Analysis_I
						  WHEN @AnaType ='ANALYSIS- II ' THEN Analysis_II
						  WHEN @AnaType='ANALYSIS- III' THEN Analysis_III
						  ELSE Analysis_I
					  END =@cat_detail
				  AND qa.answer !='N/A'
				  AND ad.SubmitFlag = 'True'
				ORDER BY c.Sequence 
				--DEBUG

			INSERT INTO #TEMP6(Analysis_type,CategoryName,Answer)
			SELECT @cat_detail AS 'Analysis_type',
				   CategoryName,
				   cast(SUBSTRING(cast(sum(Average)/count(Average) AS Varchar(50)),1,3)AS decimal(12,1)) AS Answer
			FROM
			  ( SELECT CategoryName,
					   cast(sum(cast(Answer AS decimal(12,1))) / count(Answer) AS decimal(12,1)) AS Average
			   FROM
				 ( SELECT *
				  FROM #temp5
			 ) AS t1
			   GROUP BY CategoryName ) AS t2
			GROUP BY CategoryName --Order By CategoryName

			DROP TABLE #temp5
		 END 
		  FETCH NEXT FROM c2 INTO @cat_detail 
		  END 
	CLOSE c2 
	DEALLOCATE c2


	SELECT Survey_Category.Sequence AS SR_No,
	#temp6.Analysis_type,Survey_Category.CategoryName, #temp6.Answer into #temp7 
	FROM #temp6
	INNER JOIN Survey_Category ON #temp6.CategoryName=Survey_Category.CategoryName where AccountID=@accountid and 
	Survey_Category.QuestionnaireID=@QuestionnaireID order by Survey_Category.Sequence
	 
	SELECT @ChkProgrammeAvg=Programme_Average,@ChkFullProjectGroup=FullProjectGrp
	FROM Survey_ProjectReportSetting
	WHERE AccountID=@accountid
	AND ProjectID=@projectid
  
-- For Programme Average -:
 
IF(@ChkProgrammeAvg=1) 
		BEGIN
		SELECT c.CategoryID,
		c.CategoryName,
		u.FirstName + ' ' + u.LastName AS ParticipantName,
		CandidateName,
		pg.ProgrammeName,
		p.Title,
		c.Sequence,
		REPLACE(SUBSTRING (Answer,0, 2),'&','') AS Answer INTO #temp8 from Account a

		LEFT JOIN Survey_Questionnaire qn ON qn.AccountId = a.accountId
		LEFT JOIN Survey_Category c ON c.AccountID = a.AccountID
		AND qn.QuestionnaireID = c.QuestionnaireID
		LEFT JOIN Survey_Question q ON q.CateogryId = c.CategoryId
		LEFT JOIN Survey_QuestionAnswer qa ON qa.QuestionId = q.QuestionId
		LEFT JOIN Survey_AssignmentDetails ad ON ad.AsgnDetailID = qa.AssignDetId
		LEFT JOIN Survey_AssignQuestionnaire aq ON aq.AssignmentID = ad.AssignmentID
		LEFT JOIN [User] u ON u.AccountID = aq.AccountID
		LEFT JOIN Survey_Analysis_Sheet pg ON pg.ProgrammeID = aq.ProgrammeID
		LEFT JOIN Survey_Project p ON p.ProjectID = aq.ProjecctID WHERE a.AccountID = aq.AccountID
		AND q.QuestionTypeID = 2
		AND qa.Answer != ' '
		AND aq.accountID =@accountid
		AND aq.ProgrammeID = @programmeid
		AND CASE
			  WHEN @AnaType = 'ANALYSIS- I' THEN Analysis_I
			  WHEN @AnaType ='ANALYSIS- II ' THEN Analysis_II
			  WHEN @AnaType='ANALYSIS- III' THEN Analysis_III
			  ELSE Analysis_I
			END IN
		(
		 SELECT DISTINCT 
		 CASE WHEN @AnaType = 'ANALYSIS- I' 
		 THEN Analysis_I WHEN @AnaType ='ANALYSIS- II ' 
		 THEN Analysis_II WHEN @AnaType='ANALYSIS- III' 
		 THEN Analysis_III ELSE Analysis_I END
		 FROM Survey_AssignmentDetails
		 INNER JOIN Survey_AssignQuestionnaire ON Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID
		 WHERE Survey_AssignQuestionnaire.AccountID=@accountid
		   AND Survey_AssignQuestionnaire.ProjecctID=@projectid
		   AND Survey_AssignQuestionnaire.ProgrammeID=@programmeid)
		AND qa.answer !='N/A'
		AND ad.SubmitFlag = 'True'
		ORDER BY c.Sequence
		SELECT 9999 AS 'SR_No',
		   'Programme Average' AS 'Analysis_type',
		   categoryname,
		   CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)
		  ) AS Answer INTO #TEMP9 from #temp8 group by Categoryname
	 --Here '0' is taken just like that(to give it any number)

		INSERT INTO #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
		SELECT * FROM #temp9
		--Select #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryName, #temp7.Answer from #TEMP7
		--inner join Survey_Category on #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid order by Survey_Category.Sequence

		DROP TABLE #TEMP9
		DROP TABLE #TEMP8
	END 

------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @AnalysisType1 varchar(50)= (SELECT Score1Title FROM Survey_PreviousScores	WHERE AccountID=@accountid	AND ProjectID=@projectid	AND ProgrammeID=@programmeid) 
DECLARE @AnalysisType2 varchar(50)= (SELECT Score2Title	FROM Survey_PreviousScores	WHERE AccountID=@accountid	AND ProjectID=@projectid AND ProgrammeID=@programmeid) 
	     
--For Project Group :

 
IF(@ChkFullProjectGroup=1) 
		BEGIN
			SELECT c.CategoryID,
			c.CategoryName,
			u.FirstName + ' ' + u.LastName AS ParticipantName,
			CandidateName,
			pg.ProgrammeName,
			p.Title,
			c.Sequence,
			REPLACE(SUBSTRING (Answer,0, 2),'&','') AS Answer INTO #temp10 from Account a

			LEFT JOIN Survey_Questionnaire qn ON qn.AccountId = a.accountId
			LEFT JOIN Survey_Category c ON c.AccountID = a.AccountID
			AND qn.QuestionnaireID = c.QuestionnaireID
			LEFT JOIN Survey_Question q ON q.CateogryId = c.CategoryId
			LEFT JOIN Survey_QuestionAnswer qa ON qa.QuestionId = q.QuestionId
			LEFT JOIN Survey_AssignmentDetails ad ON ad.AsgnDetailID = qa.AssignDetId
			LEFT JOIN Survey_AssignQuestionnaire aq ON aq.AssignmentID = ad.AssignmentID
			LEFT JOIN [User] u ON u.AccountID = aq.AccountID
			LEFT JOIN Survey_Analysis_Sheet pg ON pg.ProgrammeID = aq.ProgrammeID
			LEFT JOIN Survey_Project p ON p.ProjectID = aq.ProjecctID
			WHERE a.AccountID = aq.AccountID
			AND q.QuestionTypeID = 2
			AND qa.Answer != ' '
			AND aq.accountID =@accountid
			AND 
			CASE
				  WHEN @AnaType = 'ANALYSIS- I' THEN Analysis_I
				  WHEN @AnaType = 'ANALYSIS- II ' THEN Analysis_II
				  WHEN @AnaType =  'ANALYSIS- III' THEN Analysis_III
				  ELSE Analysis_I
			END IN
			(
				SELECT DISTINCT CASE WHEN @AnaType = 'ANALYSIS- I' THEN Analysis_I WHEN @AnaType ='ANALYSIS- II ' THEN Analysis_II WHEN @AnaType='ANALYSIS- III' THEN Analysis_III ELSE Analysis_I END
				FROM Survey_AssignmentDetails
				INNER JOIN Survey_AssignQuestionnaire ON Survey_AssignQuestionnaire.AssignmentID=Survey_AssignmentDetails.AssignmentID
				WHERE Survey_AssignQuestionnaire.AccountID=@accountid
				AND Survey_AssignQuestionnaire.ProjecctID=@projectid
			)
			AND qa.answer !='N/A'
			AND ad.SubmitFlag = 'True'
			ORDER BY c.Sequence
			
			SELECT 99999 AS 'SR_No','Full Project Group' AS 'Analysis_type', categoryname,CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)) AS Answer 
			INTO #TEMP11 from #temp10 group by Categoryname
			
			--Here '1' is taken just like that(to give it any number)

			INSERT INTO #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
			SELECT * FROM #temp11
	 
		drop table #TEMP10
		drop table #TEMP11	
	END

ELSE
		BEGIN 
		
			SELECT #temp7.SR_No AS SR_No, #temp7.Analysis_type, #temp7.CategoryName,
			#temp7.Answer,Survey_AnalysisSheet_Category_Details.Analysis_Category_Id into #temp77 from  #temp7
			INNER JOIN Survey_AnalysisSheet_Category_Details ON Survey_AnalysisSheet_Category_Details.Category_Detail = #temp7.Analysis_type
			WHERE Survey_AnalysisSheet_Category_Details.Programme_Id = @programmeid

			INSERT INTO #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id)
			SELECT #temp7.*,9999 FROM #temp7
			WHERE SR_No =9999
		  
			INSERT INTO #TEMP77(SR_No,Analysis_type, CategoryName, Answer,Analysis_Category_Id)
			SELECT #temp7.*,99999 FROM #temp7
			WHERE SR_No =99999 

			TRUNCATE TABLE #temp7		
			INSERT INTO #TEMP7(SR_No,Analysis_type, CategoryName, Answer)
			SELECT SR_No,Analysis_type, CategoryName, Answer FROM #TEMP77
	 
 
				
		END 

		
IF(@Filter IS NULL OR @Filter='') 
BEGIN
	SELECT a.*,Survey_Category.Sequence FROM
		(
			SELECT #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryTitle CategoryName, #temp7.Answer from #TEMP7
			INNER JOIN Survey_Category ON #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid
			
			UNION 
			SELECT sc.Sequence AS SR_No,@AnalysisType1 AS Analysis_Type,sc.CategoryTitle CategoryName,SUM(Score1)/COUNT(spqd.CategoryID) Answer
			FROM Survey_PrvScore_QstDetails spqd
			LEFT JOIN Survey_Question sq ON spqd.QuestionID=sq.QuestionID
			LEFT JOIN Survey_Category sc ON CateogryID=sc.CategoryID
			LEFT JOIN Survey_PreviousScores sps ON sps.PreviousScoreID=spqd.PreviousScoreID
			WHERE sps.AccountID=CASE WHEN @ChkPrvScore1 =1 THEN @accountid ELSE 0 END
			AND sps.ProjectID=@projectid
			AND sps.ProgrammeID=@programmeid
			AND QuestionTypeID=2
			GROUP BY spqd.CategoryID,
			sc.CategoryTitle,
			sc.Sequence
			HAVING SUM(spqd.Score1)/COUNT(spqd.CategoryID)>0
			
			UNION 
			SELECT sc.Sequence AS SR_No,@AnalysisType2 AS Analysis_Type,sc.CategoryTitle CategoryName,SUM(Score2)/COUNT(spqd.CategoryID) Answer
			FROM Survey_PrvScore_QstDetails spqd
			LEFT JOIN Survey_Question sq ON spqd.QuestionID=sq.QuestionID
			LEFT JOIN Survey_Category sc ON CateogryID=sc.CategoryID
			LEFT JOIN Survey_PreviousScores sps ON sps.PreviousScoreID=spqd.PreviousScoreID
			WHERE sps.AccountID=CASE WHEN @ChkPrvScore2 =1 THEN @accountid ELSE 0 END
			AND sps.ProjectID=@projectid
			AND sps.ProgrammeID=@programmeid
			AND QuestionTypeID=2
			GROUP BY spqd.CategoryID,
			sc.CategoryTitle,
			sc.Sequence
			HAVING SUM(spqd.Score2)/COUNT(spqd.CategoryID)>0 ) a
	INNER JOIN Survey_Category ON a.CategoryName=Survey_Category.CategoryTitle where AccountID=@accountid and 
	Survey_Category.QuestionnaireID=@QuestionnaireID
	ORDER BY Survey_Category.Sequence 
END
ELSE IF(Upper(@Filter)='TOP') 
		BEGIN
			SELECT top
			(
				SELECT top 1 RadarGraphCategoryCount
				FROM Survey_ProjectReportSetting
				WHERE AccountID=@accountid
				AND ProjectID=@ProjectId
			 ) CategoryName,
			CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)) AS Answer
			FROM
			(
			 SELECT #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryTitle CategoryName, #temp7.Answer from #TEMP7
			 INNER JOIN Survey_Category ON #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid
			 AND Analysis_type NOT IN ('Programme Average', 'Full Project Group'
			 ) --order by Survey_Category.Sequence,#temp7.SR_No desc
			) AS  tbtop --where tbtop.Analysis_type='Current'

			GROUP BY tbtop.CategoryName,Answer
			ORDER BY Answer DESC, tbtop.CategoryName ASC 
			--ORDER BY CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)) DESC, tbtop.CategoryName ASC 
			
		END 
	ELSE IF(Upper(@Filter)='BOTTOM') 
		BEGIN
			SELECT top
			(SELECT top 1 RadarGraphCategoryCount
			FROM Survey_ProjectReportSetting
			WHERE AccountID=@accountid
			AND ProjectID=@ProjectId
			) CategoryName,
			CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)) AS Answer
			FROM
			( 
				SELECT #temp7.SR_No,#temp7.Analysis_type,Survey_Category.CategoryTitle CategoryName, #temp7.Answer from #TEMP7
				INNER JOIN Survey_Category ON #TEMP7.CategoryName=Survey_Category.CategoryName where AccountID=@accountid
				AND Analysis_type NOT IN ('Programme Average','Full Project Group') --order by Survey_Category.Sequence,#temp7.SR_No desc
			) tbtop --where tbtop.Analysis_type='Current'

			GROUP BY tbtop.CategoryName,Answer
			ORDER BY Answer  ASC,tbtop.CategoryName DESC
			--- ORDER BY CAST(AVG(CAST(Answer AS decimal(12,1))) AS decimal(12,1)) ASC,tbtop.CategoryName DESC
			--CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) asc
		END 
	ELSE IF(Upper(@Filter)='TABLE') 
		BEGIN
		
		UPDATE t7  SET CategoryName = SC.CategoryTitle  
		from #temp7 t7 INNER JOIN Survey_Category SC   ON t7.CategoryName = SC.CategoryName 
		WHERE sc.AccountID=@accountid and 
			sc.QuestionnaireID=@QuestionnaireID
	
						
						SELECT * INTO #tempPrev1 from
						  (
						  
								SELECT 1 AS SR_No,
								@AnalysisType1 AS Analysis_Type,
								sc.CategoryTitle CategoryName,
								CAST(AVG(CAST(Score1 as decimal(12,1))) as decimal(12,1))  Answer
								--CAST(AVG(CAST(Score1 AS decimal(12,1))) AS decimal(12,1)) Answer
								FROM Survey_PrvScore_QstDetails spqd
								LEFT JOIN Survey_Question sq ON spqd.QuestionID=sq.QuestionID
								LEFT JOIN Survey_Category sc ON CateogryID=sc.CategoryID
								LEFT JOIN Survey_PreviousScores sps ON sps.PreviousScoreID=spqd.PreviousScoreID
								WHERE 
								sps.AccountID	=	CASE WHEN @ChkPrvScore1 =1 THEN @accountid ELSE 0 END
								AND sps.ProjectID=@projectid
								AND sps.ProgrammeID=@programmeid
								AND AnalysisType=1
								AND QuestionTypeID=2
								GROUP BY spqd.CategoryID,sc.CategoryTitle
								HAVING CAST(AVG(CAST(spqd.Score1 as decimal(12,1))) as decimal(12,1))> 0
						  ) as tbPrev1
						
						SELECT * INTO #tempPrev2 from
						(
							SELECT 1 AS SR_No,
							@AnalysisType2 AS Analysis_Type,
							sc.CategoryTitle CategoryName,
							CAST(AVG(CAST(Score2 as decimal(12,1))) as decimal(12,1))  Answer
							--CAST(AVG(CAST(Score2 AS decimal(12,1))) AS decimal(12,1)) Answer
							FROM Survey_PrvScore_QstDetails spqd
							LEFT JOIN Survey_Question sq ON spqd.QuestionID=sq.QuestionID
							LEFT JOIN Survey_Category sc ON CateogryID=sc.CategoryID
							LEFT JOIN Survey_PreviousScores sps ON sps.PreviousScoreID=spqd.PreviousScoreID
							WHERE 
							sps.AccountID	=	CASE WHEN @ChkPrvScore2 =1 THEN @accountid ELSE 0 END
							AND sps.ProjectID=@projectid
							AND sps.ProgrammeID=@programmeid
							AND AnalysisType=1
							AND QuestionTypeID=2
							GROUP BY spqd.CategoryID,
							sc.CategoryTitle
							HAVING CAST(AVG(CAST(spqd.Score2 as decimal(12,1))) as decimal(12,1))>0
					   ) AS tbPrev2 

						SELECT * INTO #tempmain1 from 
						(
							select --Analysis_Type
							CategoryName,CAST(AVG(CAST(Answer as decimal(12,1))) as decimal(12,1)) as Answer
							FROM #temp7
							WHERE Analysis_type NOT IN ('Programme Average','Full Project Group')
							GROUP BY CategoryName--,Analysis_Type
						) AS tbMain1 
			
							IF @ShowPreViousScore1=1 AND @ShowPreViousScore2 =1 
								BEGIN
								PRINT 1
									SELECT DISTINCT *
									FROM (
									SELECT Isnull(#tempmain1.CategoryName,#tempPrev1.CategoryName) CategoryName
									,
									Isnull(#tempmain1.Answer,0) CurrentScore,
									CAST(#tempPrev1.Answer as decimal(12,1)) Score1,
									CAST(#tempPrev2.Answer as decimal(12,1)) Score2,@AnalysisType1 Socre1Title,@AnalysisType2 Socre2Title

									FROM #tempmain1  LEFT JOIN #tempPrev1 ON #tempmain1.CategoryName=#tempPrev1.CategoryName
									LEFT JOIN #tempPrev2
									ON #tempPrev1.CategoryName=#tempPrev2.CategoryName
									) AS T
									ORDER BY T.CurrentScore DESC,T.CategoryName ASC 
								END

							IF @ShowPreViousScore1=1 AND @ShowPreViousScore2 =0 
								BEGIN
								PRINT 2
										SELECT *
										FROM (
										SELECT Isnull(#tempmain1.CategoryName,#tempPrev1.CategoryName) CategoryName
										,
										Isnull(#tempmain1.Answer,0) CurrentScore,
										CAST(#tempPrev1.Answer as decimal(12,1)) Score1,
										CAST(0 AS decimal(12,1)) Score2,@AnalysisType1 Socre1Title,@AnalysisType2 Socre2Title
										FROM #tempmain1 LEFT JOIN #tempPrev1 
										ON #tempmain1.CategoryName=#tempPrev1.CategoryName
										) AS T
										ORDER BY T.CurrentScore DESC,
										T.CategoryName ASC 
								END
						 IF @ShowPreViousScore1=0 AND @ShowPreViousScore2 =0 BEGIN
										SELECT Isnull(#tempmain1.CategoryName,'') CategoryName
										,
										Isnull(#tempmain1.Answer,0) CurrentScore,
										CAST(0 AS decimal(12,1)) Score1,
										CAST(0 AS decimal(12,1)) Score2,
										@AnalysisType1 Socre1Title,
										@AnalysisType2 Socre2Title
										FROM #tempmain1
										ORDER BY CurrentScore DESC,
										CategoryName ASC
						 END 
								
								
					 
		END 



--UNION
--SELECT * FROM #temp7 WHERE SR_No in (9999, 99999) order by Survey_AnalysisSheet_Category_Details.Analysis_Category_Id, Survey_Category.Sequence --#temp7.SR_No, Survey_Category.Sequence
--order by #temp7.SR_No
	IF object_id('tempdb..#TEMP77') IS NOT NULL 
	BEGIN
		DROP TABLE #TEMP77
	END 

END
	