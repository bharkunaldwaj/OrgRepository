USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[GetProjectFAQ]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[GetProjectFAQ]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetProjectFAQ]

@ProjectID int

as

select faqtext from project where projectid=@ProjectID
GO
