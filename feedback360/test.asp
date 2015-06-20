<%


 Dim objConfig, objFields, objCDO
 Set objConfig = CreateObject("CDO.Configuration")
 Set objFields = ObjConfig.Fields
 set objCDO = server.CreateObject("CDO.Message")
 With objFields
  .Item("http://schemas.microsoft.com/cdo/configuration/sendusing")= 2
  .Item("http://schemas.microsoft.com/cdo/configuration/smtpserver")= "84.22.181.250"
  .Item("http://schemas.microsoft.com/cdo/configuration/smtpserverport")= 25
  .Item("http://schemas.microsoft.com/cdo/configuration/sendusername") = "tradeupsadmin"
  .Item("http://schemas.microsoft.com/cdo/configuration/sendpassword") = "tradeups01"
  .Update
 End With
 objCDO.Configuration = objConfig
 objCDO.From = "abc@abc.com"
 objCDO.To = "madhura@damcogroup.com"
 objCDO.Cc = "m76anand@gmail.com"
 objCDO.Subject = "Hello test"
 objCDO.htmlBody = "test"
 on error resume next
 objCDO.Send
 Set objCDO = nothing
 Set objFields = Nothing
 Set objConfig = Nothing


%>