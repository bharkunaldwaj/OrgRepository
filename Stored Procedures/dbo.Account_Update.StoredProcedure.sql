USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Account_Update]    Script Date: 06/23/2015 10:42:50 ******/
DROP PROCEDURE [dbo].[Account_Update]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Account_Update](@sMode varchar(1), 						@AccountID int,						@Code char(5),						@LoginID varchar(50),						@Password varchar(50),						@OrganisationName varchar(100),						@AccountTypeID int,						@Description varchar(1000),						@EmailID varchar(100),						@Website varchar(50),						@StatusID int,						@CompanyLogo varchar(100),						@HeaderBGColor varchar(50),						@MenuBGColor varchar(50),						@ModifyBy int,						@ModifyDate datetime,						@IsActive int) as  if @sMode ='A' --For Adding/Inseting Record  Begin 	Insert into Account(Code,LoginID,Password,OrganisationName,AccountTypeID,Description,EmailID,Website,StatusID,CompanyLogo,HeaderBGColor,MenuBGColor,ModifyBy,ModifyDate,IsActive) 	Values ( @Code,@LoginID,@Password,@OrganisationName,@AccountTypeID,@Description,@EmailID,@Website,@StatusID,@CompanyLogo,@HeaderBGColor,@MenuBGColor,@ModifyBy,@ModifyDate,@IsActive) End if @sMode ='U' --For Adding/Inseting Record  Begin 	Update Account set 					Code = @Code,					LoginID = @LoginID,					Password = @Password,					OrganisationName = @OrganisationName,					AccountTypeID = @AccountTypeID,					Description = @Description,					EmailID = @EmailID,					Website = @Website,					StatusID = @StatusID,					CompanyLogo = @CompanyLogo,					HeaderBGColor = @HeaderBGColor,					MenuBGColor = @MenuBGColor,					ModifyBy = @ModifyBy,					ModifyDate = @ModifyDate,					IsActive = @IsActive	where AccountID = @AccountID End if @sMode ='D' --For deleting Record  Begin 		Delete from Account where AccountID=@AccountID End
GO
