USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_RangeCount]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_RangeCount]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[Survey_RangeCount]
as
begin
select count(*) from Question_Range
end
GO
