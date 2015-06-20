USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[USPReminderEmailHistoryManagement]    Script Date: 06/19/2015 13:26:29 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USPReminderEmailHistoryManagement]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[USPReminderEmailHistoryManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USPReminderEmailHistoryManagement]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE proc [dbo].[USPReminderEmailHistoryManagement]

@Type int
,@AccountId int
,@AccountName varchar(250)
,@ParticipantId int
,@ParticipantName varchar(250)
,@CandidateId int
,@CandidateName varchar(250)
,@ProjectId int
,@ProjectName varchar(250)
,@ProgrammeId int
,@ProgrammeName varchar(250)
,@EmailDate datetime
,@EmailStatus bit

as

INSERT INTO [ReminderEmailHistory]
           ([Type]
           ,[AccountId]
           ,[AccountName]
           ,[ParticipantId]
           ,[ParticipantName]
           ,[CandidateId]
           ,[CandidateName]
           ,[ProjectId]
           ,[ProjectName]
           ,[ProgrammeId]
           ,[ProgrammeName]
           ,[EmailDate]
           ,[EmailStatus])
     VALUES
           (@Type
           ,@AccountId
           ,@AccountName
           ,@ParticipantId
           ,@ParticipantName
           ,@CandidateId
           ,@CandidateName
           ,@ProjectId
           ,@ProjectName
           ,@ProgrammeId
           ,@ProgrammeName
           ,@EmailDate
           ,@EmailStatus)
' 
END
GO
