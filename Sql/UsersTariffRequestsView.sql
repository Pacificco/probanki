-- Загружает историю подписок
if exists(select 1 from sysobjects where name = N'UsersTariffRequestsView' and xtype='P') drop proc UsersTariffRequestsView
go
create proc UsersTariffRequestsView (@UserId int, @IsArchive bit) as	
begin	

	if @IsArchive is null
		select top 20 * from UsersTariffRequests r where r.UserId = @UserId order by r.IsArchive, r.CreateDate
	else
		select top 20 * from UsersTariffRequests r where r.UserId = @UserId and r.IsArchive = @IsArchive order by r.CreateDate
	
end
go