USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityUspUserUpdateSession]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityUspUserUpdateSession]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[PersonalityUspUserUpdateSession]

@UserID int
,@SessionData varchar(50)


as

UPdate [User] set SessionData=@Sessiondata,LoginTime=Getdate() WHERE UserID=@UserID
GO
