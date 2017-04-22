-- ѕодтверждает поступление средств дл€ указанного пользовател€
if exists(select 1 from sysobjects where name = N'PaymentConfirmed' and xtype='P') drop proc PaymentConfirmed
go
create proc PaymentConfirmed (
	@UserId int	
	) as
begin	

	update UsersForecastInfo set IsConfirmed = 1 where UserId = @UserId
	
	-- ƒобавл€ем запись в архив баланса
	insert into UserBalanceHistory(UserId,Balance,Operation,ReportDate,ReportUserId,Comment,TariffId,PeriodId) 
	select ui.UserId, ui.Balance, 1, GETDATE(),30, '', ui.TariffId, ui.PeriodId
	from UsersForecastInfo ui
	
end
go