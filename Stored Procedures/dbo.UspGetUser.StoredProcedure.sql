USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspGetUser]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspGetUser]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--UspGetUser 'admin','admin','AC001'
  
CREATE PROCEDURE [dbo].[UspGetUser]         
 @LoginID VARCHAR(50),      
 @Password varchar(50),           
 @AccountCode char(5) = NULL

AS          

BEGIN          
  
 SELECT [User].UserID,                      
     [User].LoginID,                   
     [User].Password,          
	 [User].GroupID,
     [User].FirstName,                     
     [User].LastName,           
     [User].EmailID,           
                 
     [Group].GroupName,           
     [Group].Description,           

     [Account].AccountID,
     [Account].Code,
     [Account].OrganisationName,   
	 [Account].CompanyLogo,   
	 [Account].HeaderBGColor,   
	 [Account].MenuBGColor,
	 [Account].CopyRightLine    
     
 FROM [User]           

     INNER JOIN [Group] ON [User].GroupID = [Group].GroupID          
     inner join [Account] on [Account].AccountID = [User].AccountID

 WHERE 
	[User].LoginID = @LoginID and 
	[User].Password = @Password and
	[Account].Code = @AccountCode and 
	[User].[StatusID]=1	and
	[Account].[StatusID]=1	
         
END
GO
