USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Report_Get_Analysis_III_name]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Report_Get_Analysis_III_name]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Report_Get_Analysis_III_name](@AccountID int,@ProjectID int,@ProgrammeID int)
as
begin
select Analysis_III_Name from Survey_Analysis_Sheet where AccountID=@AccountID 
and  ProjectID=@ProjectID and ProgrammeID=@ProgrammeID
end
GO
