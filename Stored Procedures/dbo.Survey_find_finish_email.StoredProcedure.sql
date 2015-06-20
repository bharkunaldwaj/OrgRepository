USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_find_finish_email]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_find_finish_email]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Survey_find_finish_email]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Survey_find_finish_email]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[Survey_find_finish_email](@projectid int,@companyid int)
as
begin
select Finish_EmailID_Chkbox from Survey_Company where ProjectID=@projectid and companyid=@companyid
end
' 
END
GO
