USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[USPReminderEmailHistoryManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[USPReminderEmailHistoryManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USPReminderEmailHistoryManagement]

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
GO
