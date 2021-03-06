USE [Feedback360_Dev2]
GO
/****** Object:  StoredProcedure [dbo].[UspProjectSearch]    Script Date: 06/23/2015 10:42:51 ******/
DROP PROCEDURE [dbo].[UspProjectSearch]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[UspProjectSearch]

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

                      dbo.Project.ProjectID,
                      dbo.Project.StatusID, 
                      dbo.Project.Reference, 
                      dbo.Project.Title, 
                      dbo.Project.Description,
                      dbo.Project.ManagerID, 
                      dbo.Project.MaxCandidate, 
                      dbo.Project.Logo, 
                      dbo.Project.StartDate, 
                      dbo.Project.EndDate, 
                      dbo.Project.Reminder1Date, 
                      dbo.Project.Reminder2Date, 
                      dbo.Project.Reminder3Date, 
                      dbo.Project.ReportAvaliableFrom, 
                      dbo.Project.ReportAvaliableTo, 
                      dbo.Project.EmailTMPLStart, 
                      dbo.Project.EmailTMPLReminder1, 
                      dbo.Project.EmailTMPLReminder2, 
                      dbo.Project.EmailTMPLReminder3, 
                      dbo.Project.EmailTMPLReportAvalibale, 
                      dbo.Project.ModifyBy, 
                      dbo.Project.ModifyDate, 
                      dbo.Project.IsActive, 
                      dbo.[User].FirstName AS firstname, 
                      dbo.[User].LastName AS lastname, 
                      dbo.[User].FirstName + '  ' + dbo.[User].LastName AS finalname, 
                      dbo.MSTProjectStatus.Name AS ProjectStatus, 
                      dbo.[User].UserID
                     
                     FROM                  
                     dbo.Project INNER JOIN
                     dbo.[User] ON dbo.Project.ManagerID = dbo.[User].UserID INNER JOIN
                     dbo.MSTProjectStatus ON dbo.Project.StatusID = dbo.MSTProjectStatus.PRJStatusID
                     
                     Where ( @Title     is null or Project.Title     Like @Title +'%'     )
                       and ( @StatusID      is null or Project.StatusID      Like @StatusID      )
                       and ( @Reference is null or Project.Reference Like @Reference + '%' ) 
                       and ( @ManagerID        is null or Project.ManagerID        Like @ManagerID        )  
                       and ( @StartDate         is null or Project.StartDate         Like @StartDate         )   
                       and ( @EndDate         is null or Project.EndDate         Like @EndDate         )
   
   
END
GO
