USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[PersonalitySaveGroupWithRights]    Script Date: 06/19/2015 13:26:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalitySaveGroupWithRights]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PersonalitySaveGroupWithRights]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PersonalitySaveGroupWithRights]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[PersonalitySaveGroupWithRights] 
	@MenuIDs nvarchar(max),
	@surveyMenuIDs nvarchar(max),
	@personalityMenuIDs nvarchar(max),
	@GroupID int,
	@GroupName nvarchar(50),
	@Description nvarchar(100),
	@IsActive bit
		
AS
BEGIN
	BEGIN TRAN
		--Declare @GroupID int
		--IF(@GroupID =0)
		----IF Not Exists (Select * from [Group] where [GroupName]=LTRIM(Rtrim(@GroupName)) and [Description]=LTRIM(RTRIM(@Description)))
		--	Begin
		--		SET NOCOUNT ON;
		--		insert into [Group](GroupName ,Description,IsActive,CreatedDate,ModifiedDate)
		--		Values(@GroupName,@Description,@IsActive,GETDATE(),GETDATE())
		--		SELECT @GroupID =IDENT_CURRENT(''Group'')
		--	End
		--ELSE
		--	Begin
				
		--		--Select @GroupID=GroupID from [Group] where [GroupName]=LTRIM(Rtrim(@GroupName)) and [Description]=LTRIM(RTRIM(@Description))
				
		--		Update [Group]
		--		set GroupName=@GroupName,
		--		[Description]=@Description,
		--		ModifiedDate=GETDATE()
		--		where GroupID=@GroupID
		--	End
		IF(@GroupID =0)
		Begin
			SELECT @GroupID =IDENT_CURRENT(''Group'')
		End
		
		--If (LEN(@MenuIDs)>0) and (LEN(@surveyMenuIDs)>0) and (LEN(@personalityMenuIDs)>0)
		--Begin
			Select @GroupID=GroupID from [Group] where [GroupName]=LTRIM(Rtrim(@GroupName))
			
			delete from GroupRights where GroupID=@GroupID
			
			insert into GroupRights (GroupID,MenuID,AccessRights) 
			(Select @GroupID,Items,''A,E,D,V'' from dbo.TblUfSplit(@MenuIDs,'','') where Items <>''''
			union 
			Select @GroupID,Items,''A,E,D,V'' from dbo.TblUfSplit(@surveyMenuIDs,'','') where Items <>''''
			union 
			Select @GroupID,Items,''A,E,D,V'' from dbo.TblUfSplit(@personalityMenuIDs,'','')where Items <>'''' )
			
			--insert into GroupRights (GroupID,MenuID,AccessRights) 
			--(Select @GroupID,Items,''A,E,D,V'' from dbo.TblUfSplit(@surveyMenuIDs,'','') a)
			
			--insert into GroupRights (GroupID,MenuID,AccessRights) 
			--(Select @GroupID,Items,''A,E,D,V'' from dbo.TblUfSplit(@personalityMenuIDs,'','') a)
		--End
		
		
		
IF @@ERROR <> 0
   BEGIN
	ROLLBACK TRAN
   END
ELSE
	COMMIT TRAN

END
' 
END
GO
