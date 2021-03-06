USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Survey_Get_Contentlist_intro]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Survey_Get_Contentlist_intro]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Report_Survey_Get_Contentlist_intro](@programmeid int ,@projectid int )
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
GO
