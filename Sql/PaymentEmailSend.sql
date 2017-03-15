-- Подтверждает отправку письма пользователя при продлении подписки
if exists(select 1 from sysobjects where name = N'PaymentEmailSend' and xtype='P') drop proc PaymentEmailSend
go
create proc PaymentEmailSend (
	@UserId int	
	) as
begin	

	update UsersForecastInfo set IsTariffLetterSent = 1 where UserId = @UserId
	
end
go