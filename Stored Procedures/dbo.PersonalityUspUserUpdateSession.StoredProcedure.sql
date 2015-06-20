USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityUspUserUpdateSession]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityUspUserUpdateSession]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalityUspUserUpdateSession]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalityUspUserUpdateSession]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[PersonalityUspUserUpdateSession]

@UserID int
,@SessionData varchar(50)


as

UPdate [User] set SessionData=@Sessiondata,LoginTime=Getdate() WHERE UserID=@UserID
' 
END
GO
