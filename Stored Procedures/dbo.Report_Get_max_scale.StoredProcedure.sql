USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Get_max_scale]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Get_max_scale]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Report_Get_max_scale](
@accountid int,
@projectid int,
@programmeid int)
as
begin
/*create table #tempo(SR_No int,Analysis_type Varchar(100),CategoryName Varchar(100),Answer decimal(18,2))
insert into #tempo exec  Report_Analysis_I_ByCategory @accountid,@projectid,@programmeid
insert into #tempo exec Report_Analysis_II_ByCategory @accountid,@projectid,@programmeid
insert into #tempo exec Report_Analysis_III_ByCategory @accountid,@projectid,@programmeid
select MAX(ceiling(Answer)) as Max_Scale from #tempo*/

select MAX(range_upto) as Max_Scale from Question_Range where Range_Name in (select distinct(range_name) from Survey_Question where AccountID = @accountid and QuestionTypeID = 2 and QuestionnaireID in (select QuestionnaireID from Survey_AssignQuestionnaire where AccountID = @accountID and ProjecctID = @projectid and ProgrammeID = @programmeid))

end
GO
