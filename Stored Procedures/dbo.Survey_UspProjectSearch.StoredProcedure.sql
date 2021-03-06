USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[Survey_UspProjectSearch]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[Survey_UspProjectSearch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Survey_UspProjectSearch]

@ProjectID int,
@Title  varchar(50),
@StatusID int,
@Reference varchar(50),
@ManagerID int,
@StartDate datetime,
@EndDate datetime

as



Begin
 If @Title  is not null and Len(@Title    )=0 Set @Title     = null   
 If @StatusID   is not null and @StatusID =0 Set @StatusID      = null   
 If @Reference is not null and Len(@Reference)=0 Set @Reference = null 
 If @ManagerID is not null and @ManagerID = 0 Set @ManagerID        = null
 If @StartDate is not null and Len(@StartDate        )=0 Set @StartDate         = null
 If @EndDate   is not null and Len(@EndDate        )=0 Set @EndDate         = null
 


SELECT               

                      dbo.Survey_Project.ProjectID,
                      dbo.Survey_Project.StatusID, 
                      dbo.Survey_Project.Reference, 
                      dbo.Survey_Project.Title, 
                      dbo.Survey_Project.Description,
                      dbo.Survey_Project.ManagerID, 
                      dbo.Survey_Project.MaxCandidate, 
                      dbo.Survey_Project.Logo, 
                      dbo.Survey_Project.StartDate, 
                      dbo.Survey_Project.EndDate, 
                      dbo.Survey_Project.Reminder1Date, 
                      dbo.Survey_Project.Reminder2Date, 
                      dbo.Survey_Project.Reminder3Date, 
                      dbo.Survey_Project.ReportAvaliableFrom, 
                      dbo.Survey_Project.ReportAvaliableTo, 
                      dbo.Survey_Project.EmailTMPLStart, 
                      dbo.Survey_Project.EmailTMPLReminder1, 
                      dbo.Survey_Project.EmailTMPLReminder2, 
                      dbo.Survey_Project.EmailTMPLReminder3, 
                      
                      dbo.Survey_Project.ModifyBy, 
                      dbo.Survey_Project.ModifyDate, 
                      dbo.Survey_Project.IsActive, 
                      
                      dbo.Survey_Project.Finish_EmailID,
                      dbo.Survey_Project.Finish_EmailID_Chkbox,
                      
                      dbo.[User].FirstName AS firstname, 
                      dbo.[User].LastName AS lastname, 
                      dbo.[User].FirstName + '  ' + dbo.[User].LastName AS finalname, 
                      dbo.Survey_MSTProjectStatus.Name AS ProjectStatus, 
                      dbo.[User].UserID
                     
                     FROM                  
                     dbo.Survey_Project INNER JOIN
                     dbo.[User] ON dbo.Survey_Project.ManagerID = dbo.[User].UserID INNER JOIN
                     dbo.Survey_MSTProjectStatus ON dbo.Survey_Project.StatusID = dbo.Survey_MSTProjectStatus.PRJStatusID
                     
                     Where ( @Title     is null or Survey_Project.Title     Like @Title +'%'     )
                       and ( @StatusID      is null or Survey_Project.StatusID      Like @StatusID      )
                       and ( @Reference is null or Survey_Project.Reference Like @Reference + '%' ) 
                       and ( @ManagerID        is null or Survey_Project.ManagerID        Like @ManagerID        )  
                       and ( @StartDate         is null or Survey_Project.StartDate         Like @StartDate         )   
                       and ( @EndDate         is null or Survey_Project.EndDate         Like @EndDate         )
   
   
END
GO
