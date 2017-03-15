-- Подтверждает поступление средств для указанного пользователя
if exists(select 1 from sysobjects where name = N'PaymentConfirmed' and xtype='P') drop proc PaymentConfirmed
go
create proc PaymentConfirmed (
	@UserId int	
	) as
begin	

	update UsersForecastInfo set IsConfirmed = 1 where UserId = @UserId
	
end
go