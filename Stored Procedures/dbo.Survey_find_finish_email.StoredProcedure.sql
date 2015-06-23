USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_find_finish_email]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_find_finish_email]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_find_finish_email](@projectid int,@companyid int)
as
begin
select Finish_EmailID_Chkbox from Survey_Company where ProjectID=@projectid and companyid=@companyid
end
GO
