USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Feedback_UspGetGroupRights]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Feedback_UspGetGroupRights]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Feedback_UspGetGroupRights]
	@intGroupID INT = NULL
	
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT GroupRights.GroupRightID , 
		   GroupRights.GroupID, 
		   GroupRights.MenuID, 
		   GroupRights.AccessRights, 
		   [Group].GroupID , 
		   [Group].GroupName,  
		   [Group].Description, 
		   [Group].WelcomeText, 
		   [Group].NewsText, 
		   [Group].IsActive, 
		   [Group].CreatedDate, 
		   [Group].ModifiedDate, 
		   MenuMaster.MenuID, 
		   MenuMaster.[Name], 
		   MenuMaster.Page, 
		   MenuMaster.LinkedPage, 
		   MenuMaster.ParentID, 
		   MenuMaster.Visibility, 
		   MenuMaster.IsActive , 
		   MenuMaster.SortOrder, 
		   MenuMaster.ADEVFlag 
	FROM  GroupRights 
	
	INNER JOIN  [Group] ON GroupRights.GroupID = [Group].GroupID 
	INNER JOIN MenuMaster ON GroupRights.MenuID = MenuMaster.MenuID
	
	WHERE (GroupRights.GroupID=@intGroupID) and (GroupRights.AccessRights !='') and (MenuMaster.ADEVFlag = 'F' or MenuMaster.ADEVFlag is null) 
END
GO
