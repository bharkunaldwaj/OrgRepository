USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalitySaveExcelParticipantsDetailsToTable]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[PersonalitySaveExcelParticipantsDetailsToTable]
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

CREATE PROCEDURE [dbo].[PersonalitySaveExcelParticipantsDetailsToTable] 
	@ParticipantAssignmentID uniqueidentifier,
    @FirstName NVarChar (50),
    @LastName nvarchar (50),
    @EmailAddress NVarChar (150),
	@Company NVarChar (250),
    @Department NVarChar (100)
    

AS
BEGIN TRAN
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	IF (Len(Rtrim(Ltrim(@FirstName)))>0  and Len(Rtrim(Ltrim(@LastName)))>0   and Len(Rtrim(Ltrim(@EmailAddress)))>0  
															and Len(Rtrim(Ltrim(@Company)))>0 and Len(Rtrim(Ltrim(@Department)))>0 )
	Begin
		IF  Len((Rtrim(Ltrim(@ParticipantAssignmentID))))>0
		 Begin
		   -- Insert statements here
		   
			insert into PersonalityParticiapntDetails(UniqueID,CreatedDate,CreatedBy,ParticipantAssignmentID,FirstName,LastName,Email,Company,Department) 
										Values(NEWID(),GETDATE(),NEWID(),@ParticipantAssignmentID, @FirstName,@LastName,@EmailAddress,@Company,@Department)

		 End
	End
		 
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
GO
