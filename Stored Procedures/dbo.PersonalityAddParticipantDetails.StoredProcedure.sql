USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalityAddParticipantDetails]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalityAddParticipantDetails]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
--ALTER PROCEDURE [dbo].[PersonalityAddParticipantDetails] 
--	-- Add the parameters for the stored procedure here
--	@QuestionnaireID uniqueidentifier,
--	@AccountID int,
--	@ParticipantAssignmentID uniqueidentifier,
--	@CompanyName varchar(250),
--	--@Questionaire varchar(250),
--	@EmailAddress varchar(150),
--	@Department varchar(100),
--	@FirstName varchar(50),
--	@LastName varchar(50)
--AS
--BEGIN
--	-- SET NOCOUNT ON added to prevent extra result sets from
--	-- interfering with SELECT statements.
--	SET NOCOUNT ON;
	
--SELECT PPA.UniqueID from PersonalityParticipantAssignments PPA where PPA.AccountID=2 and
--	PPA.QuestionnaireID = @QuestionnaireID
	
--    -- Insert statements for procedure here
--	Insert into PersonalityParticiapntDetails (UniqueID,ParticipantAssignmentID,Company,Email,Department,FirstName,LastName,CreatedDate,CreatedBy,IsStartEmailSent,IsReminderEmailSent,IsFinishedEmailSent,IsEndEmailSent)
--	values((SELECT PPA.UniqueID from PersonalityParticipantAssignments PPA where PPA.AccountID=@AccountID and
--	PPA.QuestionnaireID = @QuestionnaireID),@ParticipantAssignmentID,@CompanyName,@EmailAddress,@Department,@FirstName,@LastName,GETDATE(),@ParticipantAssignmentID,0,0,0,0)
--END
CREATE PROCEDURE [dbo].[PersonalityAddParticipantDetails] 
	-- Add the parameters for the stored procedure here
	@QuestionnaireID uniqueidentifier,
	@AccountID int,
	@CompanyName varchar(250),
	@FirstName varchar(50),
	@LastName varchar(50),
	@EmailAddress varchar(150),
	@Department varchar(100),
	@Honorific varchar(50),
	@Country uniqueidentifier,
	@PrizeDraw varchar(1),
	@Associate varchar(50)
	--@UniqueID uniqueidentifier  = null OUTPUT
	--@Questionaire varchar(250),
	
AS

			-- SET NOCOUNT ON added to prevent extra result sets from
			-- interfering with SELECT statements.
			--SET NOCOUNT ON;
BEGIN
			declare @PPUniqueID uniqueidentifier;
			declare @PersonalID uniqueidentifier;
			DECLARE @PersonalityID INT
			
			set @PPUniqueID  = (SELECT PPA.UniqueID from PersonalityParticipantAssignments PPA where PPA.AccountID=@AccountID and
			PPA.QuestionnaireID = @QuestionnaireID)
			set @PersonalID = (Select @PPUniqueID from PersonalityParticiapntDetails where UniqueID = @PPUniqueID)
			
			DECLARE @participantID uniqueidentifier;
			SET @participantID = NEWID()
			--if(EXISTS(select PPD.UniqueID from PersonalityParticiapntDetails PPD where PPD.UniqueID = @PPUniqueID))
				--SELECT  -1
			--else
				--BEGIN
				
				
					--IF(@PersonalID = null)
					--	BEGIN TRANSACTION
							Insert into PersonalityParticiapntDetails (UniqueID,ParticipantAssignmentID,Company,Email,Department,FirstName,LastName,Honorific,CreatedDate,CreatedBy,IsStartEmailSent,IsReminderEmailSent,IsFinishedEmailSent,IsEndEmailSent,CountryID,PrizeDraw,Associate)
							values(@participantID,@PPUniqueID,@CompanyName,@EmailAddress,@Department,@FirstName,@LastName,@Honorific,GETDATE(),@PPUniqueID,1,1,1,1,@Country,CONVERT(bit, @PrizeDraw),@Associate)
							
							select * from PersonalityParticiapntDetails ppd where  ppd.UniqueID = @participantID
							--select 0
							--RETURN @PersonalityID
						--COMMIT TRANSACTION
				--END
			--ELSE
END
--	SET XACT_ABORT ON;
	
--		BEGIN TRY
--			-- SET NOCOUNT ON added to prevent extra result sets from
--			-- interfering with SELECT statements.
--			--SET NOCOUNT ON;
--			declare @PPUniqueID uniqueidentifier;
--			declare @PersonalID uniqueidentifier;

--			set @PPUniqueID  = (SELECT PPA.UniqueID from PersonalityParticipantAssignments PPA where PPA.AccountID=@AccountID and
--			PPA.QuestionnaireID = @QuestionnaireID)
--			set @PersonalID = (Select @PPUniqueID from PersonalityParticiapntDetails where UniqueID = @PPUniqueID)
--			IF(@PersonalID = null)
--			--BEGIN TRY
--				BEGIN TRANSACTION
--					Insert into PersonalityParticiapntDetails (UniqueID,ParticipantAssignmentID,Company,Email,Department,FirstName,LastName,Honorific,CreatedDate,CreatedBy,IsStartEmailSent,IsReminderEmailSent,IsFinishedEmailSent,IsEndEmailSent)
--					values(@PPUniqueID,@PPUniqueID,@CompanyName,@EmailAddress,@Department,@FirstName,@LastName,@Honorific,GETDATE(),@PPUniqueID,0,0,0,0)
--					set @PersonalityID = SCOPE_IDENTITY()
					
--				COMMIT TRANSACTION
--			--END TRY
--			--ELSE
--		END TRY
--		BEGIN CATCH
--			--IF(@PersonalityID IS NULL)
--			IF (XACT_STATE() = -1)
--				BEGIN
--					PRINT 'Records already exists.'
--					ROLLBACK TRANSACTION
--				END
--			--IF(@PersonalityID IS NOT NULL)
--			IF(XACT_STATE() = 1)
--				BEGIN
--					PRINT 'Records saved successfully.'
--					COMMIT TRANSACTION
--				END
--		END	CATCH
--SET XACT_ABORT OFF;
--   select * from PersonalityParticiapntDetails order by CreatedDate desc
--   delete from PersonalityParticiapntDetails where UniqueID = '43AF5444-BD45-4C94-9622-CF28B975635F';
GO
