USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_USPReminderEmailHistoryManagement]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_USPReminderEmailHistoryManagement]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Survey_USPReminderEmailHistoryManagement]

@Type int
,@AccountId int
,@AccountName varchar(250)
,@ParticipantId int
,@ParticipantName varchar(250)
,@ProjectId int
,@ProjectName varchar(250)
,@ProgrammeId int
,@ProgrammeName varchar(250)
,@EmailDate datetime
,@EmailStatus bit

as

INSERT INTO [Survey_ReminderEmailHistory]
           ([Type]
           ,[AccountId]
           ,[AccountName]
           ,[ParticipantId]
           ,[ParticipantName]
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
           ,@ProjectId
           ,@ProjectName
           ,@ProgrammeId
           ,@ProgrammeName
           ,@EmailDate
           ,@EmailStatus)
GO
