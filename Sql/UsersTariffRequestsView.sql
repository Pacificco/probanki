-- Загружает историю подписок
if exists(select 1 from sysobjects where name = N'UsersTariffRequestsView' and xtype='P') drop proc UsersTariffRequestsView
go
create proc UsersTariffRequestsView (@UserId int) as	
begin	

	select top 10 * from UsersTariffRequests r where r.UserId = @UserId order by r.CreateDate
	
end
go