USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_Get_Contentlist_intro]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Survey_Get_Contentlist_intro]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_Survey_Get_Contentlist_intro]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Survey_Get_Contentlist_intro]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Report_Survey_Get_Contentlist_intro](@programmeid int ,@projectid int )
as
begin
select DISTINCT SAS.ProgrammeName,Survey_Project.Title ,SC.Title CompanyName
from
Account ac inner join Survey_Project SP on ac.accountid = SP.Accountid
INNER JOIN  Survey_Company SC on SP.ProjectID = SC.ProjectID INNER JOIN 
Survey_Analysis_Sheet SAS on  sas.CompanyID=sc.companyid  inner join Survey_Project
ON SAS.ProjectID=Survey_Project.ProjectID
where SAS.ProgrammeID=@programmeid
AND Survey_Project.ProjectID=@projectid
end

 ' 
END
GO
