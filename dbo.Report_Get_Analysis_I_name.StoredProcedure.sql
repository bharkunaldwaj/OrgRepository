USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Get_Analysis_I_name]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Get_Analysis_I_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Report_Get_Analysis_I_name]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Report_Get_Analysis_I_name]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'create proc [dbo].[Report_Get_Analysis_I_name](@AccountID int,@ProjectID int,@ProgrammeID int)
as
begin
select Analysis_I_Name from Survey_Analysis_Sheet where AccountID=@AccountID 
and  ProjectID=@ProjectID and ProgrammeID=@ProgrammeID
end
' 
END
GO
